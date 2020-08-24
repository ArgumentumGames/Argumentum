$json = ConvertFrom-Json -InputObject (Get-Content -Path "brand-icons.json")
$baseCss = [System.IO.File]::ReadAllText("fontawesome-all.min.css")

foreach($hit in $json.hits) {
    $name = $hit.name
    $regex = [regex] "(?is)\.fa-$($name):before[ ]*\{[^}]*\}[ \r\n]*"
    $baseCss = $baseCss -replace $regex,""
    
}

[System.IO.File]::WriteAllText("fontawesome-nobrands.min.css", $baseCss)