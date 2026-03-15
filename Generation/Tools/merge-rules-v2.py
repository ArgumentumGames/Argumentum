#!/usr/bin/env python3
"""
Script pour fusionner les cartes Rules selon les instructions de l'Issue #119
Version 2: Recherche par titre + gestion correcte des newlines dans les cellules CSV
"""

import csv
from pathlib import Path

# Chemins
csv_file = Path("d:/Dev/Argumentum/Cards/Rules/Argumentum Rules - Cards.csv")
backup_file = Path("d:/Dev/Argumentum/Cards/Rules/Argumentum Rules - Cards.backup.csv")

# Faire une sauvegarde
import shutil
shutil.copy(csv_file, backup_file)
print(f"✅ Backup créé: {backup_file}")

# Lire le CSV correctement (newlines dans les cellules)
rows = []
with open(csv_file, 'r', encoding='utf-8') as f:
    reader = csv.reader(f, quotechar='"', delimiter=',', quoting=csv.QUOTE_MINIMAL)
    try:
        header = next(reader)
        rows = list(reader)
    except StopIteration:
        print("❌ Fichier vide")
        exit(1)

print(f"📖 Lu {len(rows)} cartes")

def find_card_by_title(title_substring, start_idx=0):
    """Trouve l'index d'une carte par son titre"""
    target = title_substring.lower()
    for i in range(start_idx, min(len(rows), start_idx + 500)):
        if len(rows[i]) > 0 and rows[i][0]:
            content = rows[i][0].lower()
            if target in content:
                return i
    return -1

def merge_cards(row1_idx, row2_idx):
    """Fusionne deux cartes"""
    if row1_idx >= len(rows) or row2_idx >= len(rows):
        return None
    if row1_idx < 0 or row2_idx < 0:
        return None

    card1 = rows[row1_idx]
    card2 = rows[row2_idx]

    # Fusionner les 4 colonnes de langue
    merged = []
    for i in range(min(4, len(card1), len(card2))):
        content2 = card2[i] if len(card2) > i else ""
        # Enlever "# Argumentum" au début de card2 si présent
        if content2 and content2.startswith('# Argumentum'):
            lines = content2.split('\n')
            content2 = '\n'.join(lines[1:]) if len(lines) > 1 else content2

        merged_content = f"{card1[i]}\n\n{content2}"
        merged.append(merged_content)

    # Conserver print_and_play de la première carte
    if len(card1) > 4:
        merged.append(card1[4])
    elif len(card2) > 4:
        merged.append(card2[4])
    else:
        merged.append('')

    return merged

# Trouver les variantes
bingo_start = find_card_by_title("bingo")
parleur_start = find_card_by_title("dernier beau parleur", bingo_start + 1 if bingo_start >= 0 else 200)
moulin_start = find_card_by_title("moulin", parleur_start + 1 if parleur_start >= 0 else 400)
parlote_start = find_card_by_title("parlote coinchée", moulin_start + 1 if moulin_start >= 0 else 600)

print(f"📍 Variantes trouvées:")
print(f"   - Bingo: index {bingo_start}")
print(f"   - Dernier Beau Parleur: index {parleur_start}")
print(f"   - Moulin à Baratin: index {moulin_start}")
print(f"   - Parlote Coinchée: index {parlote_start}")

# --- FUSIONS ---

# Dernier Beau Parleur: fusionner (0,1) et (2,3)
if parleur_start >= 0:
    parler_merged_1 = merge_cards(parleur_start, parleur_start + 1)
    parler_merged_2 = merge_cards(parleur_start + 2, parleur_start + 3)
    rows[parleur_start] = parler_merged_1
    rows[parleur_start + 2] = parler_merged_2
    # Marquer pour suppression les cartes 1 et 3 (relatives)
    rows[parleur_start + 1] = None  # À supprimer
    rows[parleur_start + 3] = None  # À supprimer

# Moulin à Baratin: fusionner (0,1)
if moulin_start >= 0:
    moulin_merged = merge_cards(moulin_start, moulin_start + 1)
    rows[moulin_start] = moulin_merged
    rows[moulin_start + 1] = None  # À supprimer

# Parlote Coinchée: fusionner (0,1)
if parlote_start >= 0:
    parlote_merged = merge_cards(parlote_start, parlote_start + 1)
    rows[parlote_start] = parlote_merged
    rows[parlote_start + 1] = None  # À supprimer

    # Renommer "Début de la manche" → "Déroulé de la manche"
    for i in range(min(4, len(parlote_merged))):
        parlote_merged[i] = parlote_merged[i].replace("## Début de la manche", "## Déroulé de la manche")
        parlote_merged[i] = parlote_merged[i].replace("## Start of the round", "## Roll of the round")
        parlote_merged[i] = parlote_merged[i].replace("## Начало игры", "## Ход партии")

# Construire nouvelle liste sans les None
new_rows = [row for row in rows if row is not None]

# Écrire
with open(csv_file, 'w', encoding='utf-8', newline='') as f:
    writer = csv.writer(f, quotechar='"', delimiter=',', quoting=csv.QUOTE_MINIMAL)
    writer.writerow(header)
    writer.writerows(new_rows)

print(f"✅ Fusion terminée: {len(rows) - len(new_rows)} cartes supprimées")
print(f"✅ Nouveau total: {len(new_rows)} cartes")
