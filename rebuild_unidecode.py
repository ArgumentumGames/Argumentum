import requests

# URLs for the raw files on GitHub
datasheet_url = "https://raw.githubusercontent.com/xen0n/jsunidecode/master/src/datasheet.js"
unidecode_url = "https://raw.githubusercontent.com/xen0n/jsunidecode/master/src/unidecode.js"

# Fetch the content of the files
datasheet_content = requests.get(datasheet_url).text
unidecode_content = requests.get(unidecode_url).text

# Combine the files
# The placeholder is actually '/*!DATASHEET*/'
combined_content = unidecode_content.replace("/*!DATASHEET*/", datasheet_content)

# Write the combined content to the local file
with open("Generation/CardPen/lib/unidecode.js", "w", encoding="utf-8") as f:
    f.write(combined_content)

print("unidecode.js has been rebuilt successfully.")