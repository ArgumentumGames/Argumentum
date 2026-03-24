"""
Script pour vérifier les dimensions des images dans les harvests multilingues.
Détecte les anomalies comme les dimensions 170x321px signalées.
"""
import json
from pathlib import Path
from collections import defaultdict

def check_harvest_dimensions(harvest_path: str) -> dict:
    """Analyse un fichier .harvest.json pour détecter les dimensions d'images."""
    with open(harvest_path, 'r', encoding='utf-8') as f:
        harvest = json.load(f)

    dimensions_count = defaultdict(int)
    issues = []

    for card_id, card_data in harvest.get('Images', {}).items():
        if 'width' in card_data and 'height' in card_data:
            w, h = card_data['width'], card_data['height']
            dimensions_count[(w, h)] += 1

            # Dimensions suspectes (RU issue signalée)
            if w <= 200 or h <= 400:
                issues.append({
                    'card_id': card_id,
                    'dimensions': (w, h),
                    'url': card_data.get('url', 'N/A')
                })

    return {
        'dimensions_count': dict(dimensions_count),
        'issues': issues,
        'total_images': len(harvest.get('Images', {}))
    }

def main():
    base_path = Path("Generation/Converters/Argumentum.AssetConverter/bin/Debug/net9.0/Target")

    print("=" * 60)
    print("VÉRIFICATION DIMENSIONS HARVESTS MULTILINGUES")
    print("=" * 60)

    for lang in ['en', 'ru', 'pt', 'fr']:
        harvest_dir = base_path / lang / "Harvest"
        if not harvest_dir.exists():
            print(f"\n⚠️  {lang.upper()}: Harvest dir not found")
            continue

        print(f"\n📂 {lang.upper()}")
        print("-" * 40)

        harvest_files = list(harvest_dir.glob("*.harvest.json"))
        if not harvest_files:
            print("  Aucun fichier harvest trouvé")
            continue

        for harvest_file in sorted(harvest_files):
            result = check_harvest_dimensions(harvest_file)
            cards_name = harvest_file.stem.replace('_harvest', '')

            print(f"\n  {cards_name}:")
            print(f"    Images: {result['total_images']}")

            if result['dimensions_count']:
                print(f"    Dimensions trouvées:")
                for (w, h), count in sorted(result['dimensions_count'].items()):
                    print(f"      {w}x{h}px: {count} images")

            if result['issues']:
                print(f"    ⚠️  ANOMALIES ({len(result['issues'])}):")
                for issue in result['issues'][:5]:  # Max 5 exemples
                    print(f"      - {issue['card_id']}: {issue['dimensions']}px")
                if len(result['issues']) > 5:
                    print(f"      ... et {len(result['issues']) - 5} autres")

if __name__ == "__main__":
    main()
