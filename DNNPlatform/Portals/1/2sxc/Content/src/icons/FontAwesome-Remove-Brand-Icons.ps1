$json = ConvertFrom-Json -InputObject (Get-Content -Path "brand-icons.json")
$baseCss = Get-Content -Path "all.min.css" # [System.IO.File]::ReadAllText("all.min.css")

foreach($hit in $json.hits) {
    $name = $hit.name
    $regex = [regex] "(?is)\.fa-$($name):before[ ]*\{[^}]*\}[ \r\n]*"
    $baseCss = $baseCss -replace $regex,""
}

# remove the font references we don't need
$faBrands = '@font-face{font-family:"Font Awesome 5 Brands";font-style:normal;font-weight:normal;font-display:auto;src:url(../webfonts/fa-brands-400.eot);src:url(../webfonts/fa-brands-400.eot?#iefix) format("embedded-opentype"),url(../webfonts/fa-brands-400.woff2) format("woff2"),url(../webfonts/fa-brands-400.woff) format("woff"),url(../webfonts/fa-brands-400.ttf) format("truetype"),url(../webfonts/fa-brands-400.svg#fontawesome) format("svg")}.fab{font-family:"Font Awesome 5 Brands"}'
$faLight = '@font-face{font-family:"Font Awesome 5 Free";font-style:normal;font-weight:400;font-display:auto;src:url(../webfonts/fa-regular-400.eot);src:url(../webfonts/fa-regular-400.eot?#iefix) format("embedded-opentype"),url(../webfonts/fa-regular-400.woff2) format("woff2"),url(../webfonts/fa-regular-400.woff) format("woff"),url(../webfonts/fa-regular-400.ttf) format("truetype"),url(../webfonts/fa-regular-400.svg#fontawesome) format("svg")}.far{font-weight:400}'
$faRegular = 'url(../webfonts/fa-solid-900.eot);src:url(../webfonts/fa-solid-900.eot?#iefix) format("embedded-opentype"),url(../webfonts/fa-solid-900.woff2) format("woff2"),'
$faTtfAndSvg = ',url(../webfonts/fa-solid-900.ttf) format("truetype"),url(../webfonts/fa-solid-900.svg#fontawesome) format("svg")'

$baseCss = $baseCss.Replace($faBrands, "").Replace($faLight, "").Replace($faRegular, "").Replace($faTtfAndSvg, "")

Set-Content -Path "fontawesome-nobrands.min.css" -Value $baseCss
# [System.IO.File]::WriteAllText("fontawesome-nobrands.min.css", $baseCss)