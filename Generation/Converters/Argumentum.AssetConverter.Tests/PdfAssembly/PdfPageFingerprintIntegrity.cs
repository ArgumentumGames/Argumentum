using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Argumentum.AssetConverter.Tests.PdfAssembly
{
    /// <summary>
    /// #1460 <b>phase D</b> (pool #458, P24-38) — l'empreinte par page. Chaque page d'un PDF reçoit
    /// un sha256 de son <b>sous-graphe d'objets décodé</b> : dictionnaire de page (clés triées),
    /// flux de contenu décompressé, et récursivement chaque XObject / police référencé — l'ordre
    /// des pages est celui de l'arbre /Pages → /Kids. La liste d'empreintes se compare à une liste
    /// attendue : page permutée ⇒ rouge à l'indice, page modifiée ⇒ rouge à l'indice, PDF logiquement
    /// identique ⇒ vert.
    ///
    /// <para><b>Pourquoi un parseur maison</b> (aucune dépendance) : la stable fidélité exigée
    /// d'une empreinte est l'<b>indépendance à la sérialisation physique</b> — deux générations du
    /// même document (ordre des objets, numérotation, niveau de compression, /Length direct ou
    /// indirect, espacements) doivent donner des listes égales, faute de quoi la référence réelle
    /// (une fois les « PDF validés » de #2132 disponibles) serait liée à une version de générateur.
    /// Les clés exclues de la forme canonique le garantissent : <c>Parent</c> (l'arbre amont,
    /// partagé — sinon toute page absorberait tout le document), <c>Length</c> et <c>Filter</c> /
    /// <c>DecodeParms</c> (propriétés de l'<b>encodage</b>, pas du contenu décodé). Les numéros
    /// d'objets n'entrent pas non plus dans la forme canonique.</para>
    ///
    /// <para><b>Décodage</b> : les préfixes <c>FlateDecode</c> de la chaîne de filtres sont retirés
    /// un à un (en-tête zlib de 2 octets sauté, repli deflate brut) ; le premier filtre non-Flate
    /// (p. ex. <c>DCTDecode</c>) termine le décodage et ses octets forment la forme terminale —
    /// l'identité d'une image JPEG est ses octets JPEG, ils ne sont jamais ré-encodés.</para>
    ///
    /// <para><b>Fermeture éclairée</b> (jamais de résultat partiel silencieux) : /Root introuvable,
    /// objet de /Kids absent, <c>/Type /ObjStm</c> détecté (objets compressés dans un flux — la
    /// table d'objets serait incomplète), ou flate illisible ⇒ <see cref="PdfFingerprintException"/>
    /// nommant l'objet en cause.</para>
    /// </summary>
    public static class PdfPageFingerprintIntegrity
    {
        // ─────────────────────────────────────────────────────────────────────────
        // API
        // ─────────────────────────────────────────────────────────────────────────

        /// <summary>Empreinte (sha256 hex, 64 caractères) de chaque page, dans l'ordre de l'arbre /Pages.</summary>
        public static IReadOnlyList<string> Fingerprints(byte[] pdf)
        {
            var table = ObjectTable.Parse(pdf);
            var pagesNode = ResolveRootPages(table);
            var leaves = WalkLeaves(pagesNode, table);
            return leaves.Select(leaf => HashPage(leaf, table)).ToList();
        }

        /// <summary>Compare une liste attendue à la liste mesurée. Jamais un booléen : chaque écart
        /// est une ligne nommant la page (1-based), l'attendu et le mesuré (préfixes 12 hex).</summary>
        public static List<string> Compare(IReadOnlyList<string> expected, IReadOnlyList<string> actual)
        {
            var failures = new List<string>();
            if (expected.Count != actual.Count)
            {
                failures.Add(
                    $"nombre de pages : attendu {expected.Count}, mesuré {actual.Count} — les listes ne sont pas comparables indice à indice.");
            }

            for (var i = 0; i < Math.Min(expected.Count, actual.Count); i++)
            {
                if (!string.Equals(expected[i], actual[i], StringComparison.Ordinal))
                {
                    failures.Add(
                        $"page {i + 1} : empreinte attendue {Prefix(expected[i])}…, mesurée {Prefix(actual[i])}…");
                }
            }

            return failures;
        }

        private static string Prefix(string digest) =>
            digest.Length <= 12 ? digest : digest[..12];

        // ─────────────────────────────────────────────────────────────────────────
        // Modèle de tokens
        // ─────────────────────────────────────────────────────────────────────────

        private abstract record Token;

        private sealed record TokNumber(string Raw) : Token;

        private sealed record TokRef(int Obj, int Gen) : Token;

        private sealed record TokName(string Value) : Token;

        private sealed record TokString(byte[] Value) : Token;

        private sealed record TokBool(bool Value) : Token;

        private sealed record TokNull : Token;

        private sealed record TokArray(List<Token> Items) : Token;

        /// <summary>Dictionnaire : paires dans l'ordre de rencontre (la forme canonique trie).</summary>
        private sealed record TokDict(List<KeyValuePair<string, Token>> Entries) : Token;

        private sealed class ObjectRecord
        {
            public required int ObjNum { get; init; }
            public required Token Value { get; init; }
            /// <summary>Octets bruts du flux (encodés), ou null si l'objet n'est pas un flux.</summary>
            public byte[]? Stream { get; init; }
            /// <summary>Chaîne de filtres de /Filter (liste vide = aucun).</summary>
            public List<string> Filters { get; init; } = new();
        }

        public sealed class PdfFingerprintException : Exception
        {
            public PdfFingerprintException(string message) : base(message) { }
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Table d'objets : scan séquentiel des en-têtes « N M obj », les flux sont
        // sautés par leur /Length (direct) ou par recherche de « endstream » (repli,
        /// /Length indirect) — les faux en-têtes dans les flux sont ainsi épargnés.
        // ─────────────────────────────────────────────────────────────────────────

        private sealed class ObjectTable : Dictionary<int, ObjectRecord>
        {
            public static ObjectTable Parse(byte[] data)
            {
                var table = new ObjectTable();
                var pos = 0;
                while (pos < data.Length)
                {
                    var header = ScanNextHeader(data, ref pos);
                    if (header is null) break;
                    var (objNum, gen, bodyStart) = header.Value;
                    try
                    {
                        var lexer = new Lexer(data, bodyStart);
                        var value = lexer.ParseElement();
                        byte[]? stream = null;
                        List<string> filters = new();
                        if (value is TokDict dict && lexer.PeekKeyword() == "stream")
                        {
                            lexer.ReadKeyword(); // consomme « stream »
                            var length = ResolveStreamLength(dict, table, data, lexer.PositionAfterEol());
                            stream = lexer.ReadStreamBytes(length);
                            filters = ReadFilters(dict);
                        }

                        var record = new ObjectRecord { ObjNum = objNum, Value = value, Stream = stream, Filters = filters };
                        if (record.Value is TokDict d &&
                            d.Entries.Any(kv => kv.Key == "Type" && kv.Value is TokName n && n.Value == "ObjStm"))
                        {
                            throw new PdfFingerprintException(
                                $"objet {objNum} : /Type /ObjStm détecté — les objets compressés en flux ne sont pas lus " +
                                "par cet instrument (table incomplète => fermeture éclairée, jamais un résultat partiel).");
                        }

                        if (table.TryGetValue(objNum, out var existing) && !SameRecord(existing, record))
                        {
                            throw new PdfFingerprintException(
                                $"objet {objNum} : défini deux fois avec des contenus différents — mise à jour incrémentale " +
                                "non gérée, l'ambiguïté ferme (jamais un choix silencieux de version).");
                        }

                        table[objNum] = record;
                        pos = lexer.Position;
                    }
                    catch (PdfFingerprintException)
                    {
                        throw;
                    }
                    catch (Exception)
                    {
                        // En-tête candidat non parsable (débris binaire hors flux) : on reprend le
                        // scan après cet en-tête, l'objet est simplement absent de la table.
                        pos = bodyStart;
                    }
                }

                if (table.Count == 0)
                {
                    throw new PdfFingerprintException(
                        "aucun objet indirect parsable — ce fichier n'est pas un PDF lisible par l'instrument.");
                }

                return table;
            }

            private static bool SameRecord(ObjectRecord a, ObjectRecord b) =>
                ReferenceEquals(a, b) ||
                (a.Stream is null && b.Stream is null && a.Value == b.Value) ||
                (a.Stream is not null && b.Stream is not null && a.Stream.SequenceEqual(b.Stream));

            private static List<string> ReadFilters(TokDict dict)
            {
                var entry = dict.Entries.FirstOrDefault(kv => kv.Key == "Filter");
                return entry.Value switch
                {
                    TokName n => new List<string> { n.Value },
                    TokArray arr => arr.Items.OfType<TokName>().Select(n => n.Value).ToList(),
                    _ => new List<string>(),
                };
            }

            private static int ResolveStreamLength(TokDict dict, ObjectTable partial, byte[] data, int streamStart)
            {
                var entry = dict.Entries.FirstOrDefault(kv => kv.Key == "Length");
                if (entry.Value is TokNumber num && int.TryParse(num.Raw, out var direct) && direct >= 0)
                {
                    // Défensif : le /Length direct doit tomber sur « endstream » (à EOL près).
                    var probe = streamStart + direct;
                    if (IndexAfterEol(data, probe) != -1) return direct;
                }

                if (entry.Value is TokRef r && partial.TryGetValue(r.Obj, out var holder))
                {
                    // /Length indirect déjà parsé (l'objet précède le flux dans le fichier).
                    if (holder.Value is TokNumber n2 && int.TryParse(n2.Raw, out var indirect) && indirect >= 0)
                    {
                        return indirect;
                    }
                }

                // Repli standard : prochaine occurrence de « endstream ».
                var idx = IndexOfKeyword(data, streamStart, "endstream");
                if (idx == -1)
                {
                    throw new PdfFingerprintException(
                        $"flux démarrant à l'octet {streamStart} : /Length irrésoluble et aucun « endstream » trouvé.");
                }

                var end = idx;
                while (end > streamStart && (data[end - 1] == (byte)'\n' || data[end - 1] == (byte)'\r')) end--;
                return end - streamStart;
            }

            /// <summary>Position après l'EOL qui suit « stream », ou -1 si le mot-clé suivant n'y est pas.</summary>
            private static int IndexAfterEol(byte[] data, int probe)
            {
                var i = probe;
                while (i < data.Length && (data[i] == (byte)'\n' || data[i] == (byte)'\r')) i++;
                return HasKeyword(data, i, "endstream") ? i : -1;
            }

            private static (int ObjNum, int Gen, int BodyStart)? ScanNextHeader(byte[] data, ref int pos)
            {
                while (pos < data.Length)
                {
                    if (!IsDigit(data[pos])) { pos++; continue; }
                    var start = pos;
                    while (pos < data.Length && IsDigit(data[pos])) pos++;
                    var firstEnd = pos;
                    while (pos < data.Length && IsWs(data[pos])) pos++;
                    var secondStart = pos;
                    if (secondStart == firstEnd || !IsDigit(data[pos])) { pos = start + 1; continue; }
                    while (pos < data.Length && IsDigit(data[pos])) pos++;
                    var secondEnd = pos;
                    while (pos < data.Length && IsWs(data[pos])) pos++;
                    if (pos + 3 < data.Length && data[pos] == (byte)'o' && data[pos + 1] == (byte)'b' &&
                        data[pos + 2] == (byte)'j' && IsDelimiter(data[pos + 3]))
                    {
                        var objNum = int.Parse(Encoding.ASCII.GetString(data, start, firstEnd - start));
                        var gen = int.Parse(Encoding.ASCII.GetString(data, secondStart, secondEnd - secondStart));
                        var bodyStart = pos + 3;
                        pos = bodyStart; // le parseur de l'objet reprendra la main sur pos
                        return (objNum, gen, bodyStart);
                    }

                    pos = start + 1;
                }

                return null;
            }
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Lexer
        // ─────────────────────────────────────────────────────────────────────────

        private sealed class Lexer
        {
            private readonly byte[] _data;
            public int Position { get; private set; }

            public Lexer(byte[] data, int pos)
            {
                _data = data;
                Position = pos;
            }

            /// <summary>Avance et retourne la position juste après l'EOL qui suit « stream » —
            /// point de départ EXACT des octets du fil (sans cet avancement, la lecture du fil
            /// partirait de l'EOL et serait décalée d'un octet de bout en bout).</summary>
            public int PositionAfterEol()
            {
                if (Position < _data.Length && _data[Position] == (byte)'\r') Position++;
                if (Position < _data.Length && _data[Position] == (byte)'\n' &&
                    _data[Position - 1] == (byte)'\r') Position++;
                else if (Position < _data.Length && _data[Position] == (byte)'\n') Position++;
                return Position;
            }

            public byte[] ReadStreamBytes(int length)
            {
                if (length < 0 || Position + length > _data.Length)
                {
                    throw new PdfFingerprintException(
                        $"flux de longueur {length} dépassant le fichier depuis l'octet {Position}.");
                }

                var bytes = new byte[length];
                Array.Copy(_data, Position, bytes, 0, length);
                Position += length;
                var idx = IndexOfKeyword(_data, Position, "endstream");
                if (idx == -1)
                {
                    throw new PdfFingerprintException($"« endstream » introuvable après l'octet {Position}.");
                }

                Position = idx + "endstream".Length;
                return bytes;
            }

            public string? PeekKeyword()
            {
                var i = Position;
                while (i < _data.Length && IsWs(_data[i])) i++;
                var sb = new StringBuilder();
                while (i < _data.Length && (char)_data[i] is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z')) sb.Append((char)_data[i++]);
                var word = sb.ToString();
                return word is "stream" or "endobj" ? word : null;
            }

            public void ReadKeyword()
            {
                while (Position < _data.Length && IsWs(_data[Position])) Position++;
                while (Position < _data.Length && (char)_data[Position] is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z')) Position++;
            }

            public Token ParseElement()
            {
                SkipWhitespace();
                if (Position >= _data.Length) throw new PdfFingerprintException("fin de fichier inattendue.");
                var b = _data[Position];
                return b switch
                {
                    (byte)'[' => ParseArray(),
                    (byte)'<' when Position + 1 < _data.Length && _data[Position + 1] == (byte)'<' => ParseDict(),
                    (byte)'/' => ParseName(),
                    (byte)'(' => ParseLiteralString(),
                    (byte)'<' => ParseHexString(),
                    (byte)'t' or (byte)'f' or (byte)'n' => ParseKeywordValue(),
                    _ when IsDigit(b) || b == (byte)'-' || b == (byte)'+' || b == (byte)'.' => ParseNumberOrRef(),
                    _ => throw new PdfFingerprintException($"octet inattendu '{(char)b}' à {Position}."),
                };
            }

            private void SkipWhitespace()
            {
                while (Position < _data.Length)
                {
                    if (_data[Position] == (byte)'%')
                    {
                        while (Position < _data.Length && _data[Position] != (byte)'\n' && _data[Position] != (byte)'\r') Position++;
                    }
                    else if (IsWs(_data[Position])) Position++;
                    else break;
                }
            }

            private Token ParseArray()
            {
                Position++; // '['
                var items = new List<Token>();
                SkipWhitespace();
                while (Position < _data.Length && _data[Position] != (byte)']')
                {
                    items.Add(ParseElement());
                    SkipWhitespace();
                }

                if (Position >= _data.Length) throw new PdfFingerprintException("tableau non fermé.");
                Position++; // ']'
                return new TokArray(items);
            }

            private Token ParseDict()
            {
                Position += 2; // '<<'
                var entries = new List<KeyValuePair<string, Token>>();
                SkipWhitespace();
                while (Position < _data.Length && !(_data[Position] == (byte)'>' && Position + 1 < _data.Length && _data[Position + 1] == (byte)'>'))
                {
                    SkipWhitespace();
                    if (Position >= _data.Length || _data[Position] != (byte)'/')
                    {
                        throw new PdfFingerprintException($"clé de dictionnaire attendue à l'octet {Position}.");
                    }

                    var key = ((TokName)ParseName()).Value;
                    var value = ParseElement();
                    entries.Add(new KeyValuePair<string, Token>(key, value));
                    SkipWhitespace();
                }

                if (Position + 1 >= _data.Length) throw new PdfFingerprintException("dictionnaire non fermé.");
                Position += 2; // '>>'
                return new TokDict(entries);
            }

            private Token ParseName()
            {
                Position++; // '/'
                var sb = new StringBuilder();
                while (Position < _data.Length && !IsDelimiter(_data[Position]))
                {
                    sb.Append((char)_data[Position]);
                    Position++;
                }

                return new TokName(sb.ToString());
            }

            private Token ParseLiteralString()
            {
                Position++; // '('
                var bytes = new List<byte>();
                var depth = 1;
                while (Position < _data.Length)
                {
                    var c = _data[Position];
                    if (c == (byte)'\\')
                    {
                        Position++;
                        if (Position >= _data.Length) break;
                        var e = _data[Position];
                        bytes.Add(e switch
                        {
                            (byte)'n' => (byte)'\n',
                            (byte)'r' => (byte)'\r',
                            (byte)'t' => (byte)'\t',
                            (byte)'b' => 8,
                            (byte)'f' => 12,
                            (byte)'(' => (byte)'(',
                            (byte)')' => (byte)')',
                            (byte)'\\' => (byte)'\\',
                            _ when e >= (byte)'0' && e <= (byte)'7' => ParseOctal(),
                            _ => e,
                        });
                        Position++;
                    }
                    else if (c == (byte)'(') { depth++; bytes.Add(c); Position++; }
                    else if (c == (byte)')')
                    {
                        depth--;
                        if (depth == 0) { Position++; break; }
                        bytes.Add(c);
                        Position++;
                    }
                    else { bytes.Add(c); Position++; }
                }

                return new TokString(bytes.ToArray());

                byte ParseOctal()
                {
                    var oct = 0;
                    var digits = 0;
                    while (digits < 3 && Position < _data.Length && _data[Position] >= (byte)'0' && _data[Position] <= (byte)'7')
                    {
                        oct = oct * 8 + (_data[Position] - (byte)'0');
                        Position++;
                        digits++;
                    }

                    return (byte)(oct & 0xFF);
                }
            }

            private Token ParseHexString()
            {
                Position++; // '<'
                var nybbles = new List<int>();
                while (Position < _data.Length && _data[Position] != (byte)'>')
                {
                    var c = (char)_data[Position];
                    if (Uri.IsHexDigit(c)) nybbles.Add(Uri.FromHex(c));
                    Position++;
                }

                Position++; // '>'
                var bytes = new byte[(nybbles.Count + 1) / 2];
                for (var i = 0; i < nybbles.Count; i++)
                {
                    if (i % 2 == 0) bytes[i / 2] = (byte)(nybbles[i] << 4);
                    else bytes[i / 2] |= (byte)nybbles[i];
                }

                return new TokString(bytes);
            }

            private Token ParseKeywordValue()
            {
                var word = ReadWord();
                return word switch
                {
                    "true" => new TokBool(true),
                    "false" => new TokBool(false),
                    "null" => new TokNull(),
                    _ => throw new PdfFingerprintException($"mot-clé inattendu « {word} » à {Position}."),
                };
            }

            private string ReadWord()
            {
                var sb = new StringBuilder();
                while (Position < _data.Length && (char)_data[Position] is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z')) sb.Append((char)_data[Position++]);
                return sb.ToString();
            }

            private Token ParseNumberOrRef()
            {
                var start = Position;
                while (Position < _data.Length &&
                       (IsDigit(_data[Position]) || _data[Position] is (byte)'.' or (byte)'-' or (byte)'+' or (byte)'e' or (byte)'E'))
                {
                    Position++;
                }

                var raw = Encoding.ASCII.GetString(_data, start, Position - start);
                var save = Position;
                SkipWhitespace();
                if (Position < _data.Length && IsDigit(_data[Position]))
                {
                    var genStart = Position;
                    while (Position < _data.Length && IsDigit(_data[Position])) Position++;
                    var genRaw = Encoding.ASCII.GetString(_data, genStart, Position - genStart);
                    var afterGen = Position;
                    SkipWhitespace();
                    if (Position < _data.Length && _data[Position] == (byte)'R' && (Position + 1 >= _data.Length || IsDelimiter(_data[Position + 1])))
                    {
                        Position++; // 'R'
                        return new TokRef(int.Parse(raw), int.Parse(genRaw));
                    }

                    Position = afterGen;
                }

                Position = save;
                return new TokNumber(raw);
            }
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Arbre de pages
        // ─────────────────────────────────────────────────────────────────────────

        private static TokDict ResolveRootPages(ObjectTable table)
        {
            // Le trailer n'est pas un objet indirect : le catalogue se résout par balayage de la
            // table — un objet /Type /Catalog unique, portant /Pages. Tout autre cas ferme.
            var catalogs = table.Values
                .Where(r => r.Value is TokDict d && d.Entries.Any(kv =>
                    kv.Key == "Type" && kv.Value is TokName n && n.Value == "Catalog"))
                .ToList();
            if (catalogs.Count == 1 && catalogs[0].Value is TokDict catalog)
            {
                var pagesRef = catalog.Entries.FirstOrDefault(kv => kv.Key == "Pages").Value;
                if (pagesRef is TokRef pr && table.TryGetValue(pr.Obj, out var pages) && pages.Value is TokDict pagesDict)
                {
                    return pagesDict;
                }

                throw new PdfFingerprintException(
                    $"catalogue (objet {catalogs[0].ObjNum}) : /Pages absent ou non résoluble.");
            }

            var candidates = string.Join(", ", catalogs.Select(c => c.ObjNum));
            throw new PdfFingerprintException(
                $"/Root introuvable : {catalogs.Count} objets /Type /Catalog ({candidates}) — " +
                "un catalogue unique portant /Pages est requis (fermeture éclairée).");
        }

        private static List<ObjectRecord> WalkLeaves(TokDict pagesNode, ObjectTable table)
        {
            var leaves = new List<ObjectRecord>();
            Walk(pagesNode);
            return leaves;

            void Walk(TokDict node)
            {
                var kids = node.Entries.FirstOrDefault(kv => kv.Key == "Kids").Value;
                if (kids is not TokArray kidArray)
                {
                    throw new PdfFingerprintException("nœud /Pages sans /Kids exploitable.");
                }

                foreach (var kid in kidArray.Items)
                {
                    if (kid is not TokRef kr)
                    {
                        throw new PdfFingerprintException("entrée /Kids non-référence rencontrée.");
                    }

                    if (!table.TryGetValue(kr.Obj, out var record) || record.Value is not TokDict kidDict)
                    {
                        throw new PdfFingerprintException(
                            $"entrée /Kids pointant sur l'objet {kr.Obj}, absent de la table — l'objet est peut-être " +
                            "compressé (ObjStm) ou le fichier est tronqué.");
                    }

                    var type = kidDict.Entries.FirstOrDefault(kv => kv.Key == "Type").Value as TokName;
                    if (type?.Value == "Page") leaves.Add(record);
                    else Walk(kidDict); // /Pages intermédiaire (avec ou sans /Type)
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Hachage : DFS déterministe depuis la feuille, clés triées, Parent/Length/
        // Filter/DecodeParms exclus, flots décodés par retrait des FlateDecode.
        // ─────────────────────────────────────────────────────────────────────────

        private static readonly string[] ExcludedKeys = { "Parent", "Length", "Filter", "DecodeParms" };

        private static string HashPage(ObjectRecord page, ObjectTable table)
        {
            var cache = new Dictionary<int, string>();
            var canonical = CanonicalObject(page, table, cache);
            return Sha256Hex(Encoding.UTF8.GetBytes(canonical));
        }

        private static string CanonicalObject(ObjectRecord record, ObjectTable table, Dictionary<int, string> cache)
        {
            if (cache.TryGetValue(record.ObjNum, out var cached)) return $"#({cached})";
            // Réservation avant récursion : un cycle légitime (référence partagée) relit le cache.
            cache[record.ObjNum] = "";
            string form;
            if (record.Stream is { } stream)
            {
                var decoded = DecodeFilters(stream, record.Filters, record.ObjNum);
                form = $"STREAM({Sha256Hex(decoded)})";
                // Les autres clés du dictionnaire de flux participent aussi (Width, Subtype…).
                if (record.Value is TokDict streamDict)
                {
                    form += CanonicalValue(new TokDict(FilteredEntries(streamDict).ToList()), table, cache);
                }
            }
            else
            {
                form = CanonicalValue(record.Value, table, cache);
            }

            cache[record.ObjNum] = form;
            return form;
        }

        private static IEnumerable<KeyValuePair<string, Token>> FilteredEntries(TokDict dict) =>
            dict.Entries.Where(kv => !ExcludedKeys.Contains(kv.Key, StringComparer.Ordinal));

        private static string CanonicalValue(Token token, ObjectTable table, Dictionary<int, string> cache) => token switch
        {
            TokRef r => table.TryGetValue(r.Obj, out var target)
                ? CanonicalObject(target, table, cache)
                : throw new PdfFingerprintException($"référence {r.Obj} {r.Gen} R non résolue (objet absent)."),
            TokName n => $"/{n.Value}",
            TokNumber num => $"N{NormalizeNumber(num.Raw)}",
            TokBool b => b.Value ? "true" : "false",
            TokNull => "null",
            TokString s => $"<{Convert.ToHexString(s.Value).ToLowerInvariant()}>",
            TokArray arr => "[" + string.Join(",", arr.Items.Select(i => CanonicalValue(i, table, cache))) + "]",
            TokDict dict => "{" + string.Join(",",
                FilteredEntries(dict)
                    .OrderBy(kv => kv.Key, StringComparer.Ordinal)
                    .Select(kv => $"/{kv.Key}:{CanonicalValue(kv.Value, table, cache)}")) + "}",
            _ => throw new PdfFingerprintException("token inconnu lors de la canonisation."),
        };

        private static string NormalizeNumber(string raw) =>
            double.TryParse(raw, System.Globalization.CultureInfo.InvariantCulture, out var d)
                ? d.ToString("0.0############", System.Globalization.CultureInfo.InvariantCulture)
                : raw;

        private static byte[] DecodeFilters(byte[] stream, List<string> filters, int objNum)
        {
            var current = stream;
            var index = 0;
            while (index < filters.Count && filters[index] == "FlateDecode")
            {
                current = Inflate(current, objNum);
                index++;
            }

            // Le premier filtre restant (DCTDecode, CCITTFaxDecode, LZWDecode…) définit la forme
            // terminale : ses octets sont l'identité, ils ne sont jamais transcodés.
            return current;
        }

        private static byte[] Inflate(byte[] input, int objNum)
        {
            if (input.Length > 2)
            {
                try
                {
                    return InflateFrom(input, 2); // en-tête zlib standard (0x78 …)
                }
                catch (PdfFingerprintException)
                {
                    // repli : flux deflate sans en-tête
                }
            }

            try
            {
                return InflateFrom(input, 0);
            }
            catch (PdfFingerprintException inner)
            {
                throw new PdfFingerprintException(
                    $"objet {objNum} : flux FlateDecode illisible ({inner.Message}) — fermeture éclairée.");
            }
        }

        private static byte[] InflateFrom(byte[] input, int offset)
        {
            try
            {
                using var ms = new MemoryStream(input, offset, input.Length - offset);
                using var ds = new DeflateStream(ms, CompressionMode.Decompress);
                using var output = new MemoryStream();
                ds.CopyTo(output);
                return output.ToArray();
            }
            catch (Exception ex)
            {
                throw new PdfFingerprintException($"deflate échoué : {ex.Message}");
            }
        }

        private static string Sha256Hex(byte[] bytes)
        {
            var hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        // ─────────────────────────────────────────────────────────────────────────
        // Utilitaires d'octets
        // ─────────────────────────────────────────────────────────────────────────

        private static bool IsDigit(byte b) => b is >= (byte)'0' and <= (byte)'9';

        private static bool IsWs(byte b) => b is (byte)' ' or (byte)'\t' or (byte)'\n' or (byte)'\r' or 0 or 12;

        private static bool IsDelimiter(byte b) =>
            IsWs(b) || b is (byte)'<' or (byte)'>' or (byte)'[' or (byte)']' or (byte)'/' or (byte)'(' or (byte)')' or (byte)'%' or (byte)'#';

        private static bool HasKeyword(byte[] data, int pos, string keyword)
        {
            if (pos < 0 || pos + keyword.Length > data.Length) return false;
            for (var i = 0; i < keyword.Length; i++)
            {
                if (data[pos + i] != (byte)keyword[i]) return false;
            }

            return true;
        }

        private static int IndexOfKeyword(byte[] data, int from, string keyword)
        {
            for (var i = Math.Max(from, 0); i + keyword.Length <= data.Length; i++)
            {
                if (HasKeyword(data, i, keyword)) return i;
            }

            return -1;
        }
    }
}
