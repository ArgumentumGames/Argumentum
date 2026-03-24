#!/usr/bin/env python3
"""
Script pour fusionner les cartes Rules selon les instructions de l'Issue #119
"""

import csv
import re
from pathlib import Path

# Chemins
csv_file = Path("d:/Dev/Argumentum/Cards/Rules/Argumentum Rules - Cards.csv")
backup_file = Path("d:/Dev/Argumentum/Cards/Rules/Argumentum Rules - Cards.backup.csv")
output_file = csv_file

# Faire une sauvegarde
import shutil
shutil.copy(csv_file, backup_file)
print(f"✅ Backup créé: {backup_file}")

# Lire le CSV
rows = []
with open(csv_file, 'r', encoding='utf-8') as f:
    reader = csv.reader(f)
    header = next(reader)
    rows = list(reader)

print(f"📖 Lu {len(rows)} cartes")

def merge_cards(row1_idx, row2_idx):
    """Fusionne deux cartes consécutives"""
    if row2_idx >= len(rows):
        return None

    card1 = rows[row1_idx]
    card2 = rows[row2_idx]

    # Fusionner les 4 colonnes de langue
    merged = []
    for i in range(4):
        # Fusionner: card1 + "\n\n" + card2
        # Mais enlever le titre "# Argumentum" s'il est présent dans card2
        content2 = card2[i]
        if content2.startswith('# Argumentum'):
            content2 = '\n'.join(content2.split('\n')[1:])  # Enlever la première ligne

        merged_content = f"{card1[i]}\n\n{content2}"
        merged.append(merged_content)

    # Garder le print_and_play de la première carte
    merged.append(card1[4] if len(card1) > 4 else card2[4] if len(card2) > 4 else '')

    return merged

# --- MODIFICATIONS ---

new_rows = []
skip_indices = set()

# 1. Bingo: PAS DE CHANGEMENT (déjà OK)
# Lignes 285-358 (3 cartes)
# Conserver tel quel

# 2. Dernier Beau Parleur (variante 3)
# Fusionner:
#   - Carte 1 (ligne 425, index 424) + Carte 2 (ligne 475, index 474)
#   - Carte 3 (ligne 500, index 499) + Carte 4 (ligne 561, index 560)
parleur_1_2 = merge_cards(424, 474)
parleur_3_4 = merge_cards(499, 560)
skip_indices.update({474, 560})  # Sauter les cartes 2 et 4 après fusion
rows[424] = parleur_1_2
rows[499] = parleur_3_4

# 3. Moulin à Baratin (variante 4)
# Fusionner Carte 1 (ligne 579, index 578) + Carte 2 (ligne 630, index 629)
moulin_1_2 = merge_cards(578, 629)
skip_indices.add(629)
rows[578] = moulin_1_2

# 4. Parlote Coinchée (variante 5)
# Fusionner Carte 1 (ligne 756, index 755) + Carte 2 (ligne 807, index 806)
parlote_1_2 = merge_cards(755, 806)
skip_indices.add(806)
rows[755] = parlote_1_2

# Renommer "Début de la manche" en "Déroulé de la manche" dans la carte fusionnée
for i in range(4):
    rows[755][i] = rows[755][i].replace("## Début de la manche", "## Déroulé de la manche")
    rows[755][i] = rows[755][i].replace("## Start of the round", "## Roll of the round")
    rows[755][i] = rows[755][i].replace("## Начало игры", "## Ход партии")
    rows[755][i] = rows[755][i].replace("## Instalação", "## Rolamento da rodada")

# Construire la nouvelle liste en supprimant les cartes fusionnées
for i, row in enumerate(rows):
    if i not in skip_indices:
        new_rows.append(row)

# Écrire le fichier modifié
with open(output_file, 'w', encoding='utf-8', newline='') as f:
    writer = csv.writer(f)
    writer.writerow(header)
    writer.writerows(new_rows)

print(f"✅ CSV modifié: {len(rows) - len(new_rows)} cartes supprimées")
print(f"✅ Nouveau total: {len(new_rows)} cartes")
print(f"✅ Backup: {backup_file}")
