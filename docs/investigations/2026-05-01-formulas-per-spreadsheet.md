# Formula Audit — All GSheet Spreadsheets
Generated: 2026-05-01 14:25

## Fallacies Taxonomy
Spreadsheet: `1TrQUyzXMMM-9pHdNWz1fdJ3xQ5XcHgwVH52SOnM61ow` | GID: `969304769`

**Protected cells (formulas): 50358**

| Column | Header | Formula Count | Sample Formula |
|--------|--------|---------------|----------------|
| A1 | PK | 1407 | `=A2+1` |
| C1 | decimal_path | 1408 | `=SUBSTITUTE(SUBSTITUTE(B2,".",",",1),".","")` |
| D1 | depth | 1407 | `=INT(len(B3)/2)+1` |
| E1 | Famille | 1408 | `=VLOOKUP(Left(C2,1), $C$2:$N$1404, 12, TRUE)` |
| F1 | Famille_camelCase | 1406 | `=REPLACE(SUBSTITUTE(PROPER(TRIM(E2))," ",),1,1,LEFT(LOWER(TRIM(E2))))` |
| G1 | Sous-Famille | 1408 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$N$1404, 12, TRUE),"") ` |
| H1 | Soussousfamille | 1408 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$N$1404, 12, TRUE),"")` |
| O1 | LTfr | 1354 | `= int(len(N2))
` |
| Q1 | Lfr115 | 1386 | `= int(len(P2))
` |
| S1 | Lxfr145 | 1390 | `=int(len(R2))` |
| U1 | Family | 1360 | `=VLOOKUP(Left(C2,1), $C$2:$X$1404, 22, TRUE)` |
| V1 | Subfamily | 1360 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$X$1404, 22, TRUE),"") ` |
| W1 | Subsubfamily | 1360 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$X$1404, 22, TRUE),"")` |
| Y1 | LTen | 1387 | `=int(len(X2))` |
| AB1 | Len115 | 1391 | `= INT(len(AA2))` |
| AE1 | Lxen145 | 1408 | `=int(len(AC2))` |
| AL1 | Family_ru | 1397 | `=VLOOKUP(Left(C2,1), $C$2:$AO$1404, 39, TRUE)` |
| AM1 | Subfamily_ru | 1397 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$AO$1404, 39, TRUE),"")` |
| AN1 | Subsubfamily_ru | 1397 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$AO$1404, 39, TRUE),"")` |
| AS1 | Family_pt | 1404 | `=VLOOKUP(Left(C2,1), $C$2:$AV$1404, 46, TRUE)` |
| AT1 | Subfamily_pt | 1404 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$AV$1404, 46, TRUE),"")` |
| AU1 | Subsubfamily_pt | 1404 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$AV$1404, 46, TRUE),"")` |
| AZ1 | Family_ar | 1408 | `=VLOOKUP(Left(C2,1), $C$2:$BC$1404, 53, TRUE)` |
| BA1 | Subfamily_ar | 1408 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$BC$1404, 53, TRUE),"")` |
| BB1 | Subsubfamily_ar | 1408 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$BC$1404, 53, TRUE),"")` |
| BG1 | Family_es | 1408 | `=VLOOKUP(Left(C2,1), $C$2:$BJ$1404, 60, TRUE)` |
| BH1 | Subfamily_es | 1408 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$BJ$1404, 60, TRUE),"")` |
| BI1 | Subsubfamily_es | 1408 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$BJ$1404, 60, TRUE),"")` |
| BN1 | Family_zh | 1408 | `=VLOOKUP(Left(C2,1), $C$2:$BQ$1404, 67, TRUE)` |
| BO1 | Subfamily_zh | 1408 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$BQ$1404, 67, TRUE),"")` |
| BP1 | Subsubfamily_zh | 1408 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$BQ$1404, 67, TRUE),"")` |
| BU1 | Family_fa | 1408 | `=VLOOKUP(Left(C2,1), $C$2:$BX$1404, 74, TRUE)` |
| BV1 | Subfamily_fa | 1408 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$BX$1404, 74, TRUE),"")` |
| BW1 | Subsubfamily_fa | 1408 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$BX$1404, 74, TRUE),"")` |
| CW1 | decimal_path_padded | 1408 | `=TEXT(SUBSTITUTE(SUBSTITUTE(SUBSTITUTE(B2,".",",",1),".",""),",","."), "0.000...` |
| CX1 | depth_max4 | 1403 | `=if(D3<4,D3,"4+")` |

### Unique Formula Patterns

| Pattern | Count | Example |
|---------|-------|---------|
| `=int(len(CELL))` | 4185 | `=int(len(R2))` |
| `= int(len(CELL))
` | 2740 | `= int(len(N2))
` |
| `=SUBSTITUTE(SUBSTITUTE(CELL,".",",",1),".","")` | 1408 | `=SUBSTITUTE(SUBSTITUTE(B2,".",",",1),".","")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$BC$1404, 53, TRUE)` | 1408 | `=VLOOKUP(Left(C2,1), $C$2:$BC$1404, 53, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$BC$1404, 53,...` | 1408 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$BC$1404, 53, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$BC$1404, 53, ...` | 1408 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$BC$1404, 53, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$BJ$1404, 60, TRUE)` | 1408 | `=VLOOKUP(Left(C2,1), $C$2:$BJ$1404, 60, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$BJ$1404, 60,...` | 1408 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$BJ$1404, 60, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$BJ$1404, 60, ...` | 1408 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$BJ$1404, 60, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$BQ$1404, 67, TRUE)` | 1408 | `=VLOOKUP(Left(C2,1), $C$2:$BQ$1404, 67, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$BQ$1404, 67,...` | 1408 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$BQ$1404, 67, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$BQ$1404, 67, ...` | 1408 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$BQ$1404, 67, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$BX$1404, 74, TRUE)` | 1408 | `=VLOOKUP(Left(C2,1), $C$2:$BX$1404, 74, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$BX$1404, 74,...` | 1408 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$BX$1404, 74, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$BX$1404, 74, ...` | 1408 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$BX$1404, 74, TRUE),"")` |
| `=TEXT(SUBSTITUTE(SUBSTITUTE(SUBSTITUTE(CELL,".",",",1),"....` | 1408 | `=TEXT(SUBSTITUTE(SUBSTITUTE(SUBSTITUTE(B2,".",",",1),".",""),",","."), "0.0000000")` |
| `=CELL+1` | 1407 | `=A2+1` |
| `=INT(len(CELL)/2)+1` | 1407 | `=INT(len(B3)/2)+1` |
| `=REPLACE(SUBSTITUTE(PROPER(TRIM(CELL))," ",),1,1,LEFT(LOW...` | 1406 | `=REPLACE(SUBSTITUTE(PROPER(TRIM(E2))," ",),1,1,LEFT(LOWER(TRIM(E2))))` |
| `=if(CELL<4,CELL,"4+")` | 1403 | `=if(D3<4,D3,"4+")` |
| `= INT(len(CELL))` | 1391 | `= INT(len(AA2))` |
| `=VLOOKUP(Left(CELL,1), $C$2:$N$1404, 12, TRUE)` | 1242 | `=VLOOKUP(Left(C2,1), $C$2:$N$1404, 12, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$N$1404, 12, ...` | 1242 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$N$1404, 12, TRUE),"") ` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$N$1404, 12, T...` | 1242 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$N$1404, 12, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$AV$1404, 46, TRUE)` | 1240 | `=VLOOKUP(Left(C2,1), $C$2:$AV$1404, 46, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$AV$1404, 46,...` | 1240 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$AV$1404, 46, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$AV$1404, 46, ...` | 1240 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$AV$1404, 46, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$AO$1404, 39, TRUE)` | 1236 | `=VLOOKUP(Left(C2,1), $C$2:$AO$1404, 39, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$AO$1404, 39,...` | 1236 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$AO$1404, 39, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$AO$1404, 39, ...` | 1235 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$AO$1404, 39, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$X$1404, 22, TRUE)` | 1199 | `=VLOOKUP(Left(C2,1), $C$2:$X$1404, 22, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$X$1404, 22, ...` | 1199 | `=IF(Len(C2)>2, VLOOKUP(Left(C2,3), $C$2:$X$1404, 22, TRUE),"") ` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$X$1404, 22, T...` | 1199 | `=IF(Len(C2)>3,VLOOKUP(Left(C2,4), $C$2:$X$1404, 22, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$N$1403, 12, TRUE)` | 153 | `=VLOOKUP(Left(C19,1), $C$2:$N$1403, 12, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$N$1403, 12, ...` | 153 | `=IF(Len(C19)>2, VLOOKUP(Left(C19,3), $C$2:$N$1403, 12, TRUE),"") ` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$N$1403, 12, T...` | 153 | `=IF(Len(C19)>3,VLOOKUP(Left(C19,4), $C$2:$N$1403, 12, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$AV$1403, 46, TRUE)` | 151 | `=VLOOKUP(Left(C56,1), $C$2:$AV$1403, 46, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$AV$1403, 46,...` | 151 | `=IF(Len(C56)>2, VLOOKUP(Left(C56,3), $C$2:$AV$1403, 46, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$AV$1403, 46, ...` | 151 | `=IF(Len(C56)>3,VLOOKUP(Left(C56,4), $C$2:$AV$1403, 46, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$AO$1403, 39, ...` | 149 | `=IF(Len(C56)>3,VLOOKUP(Left(C56,4), $C$2:$AO$1403, 39, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$X$1403, 22, TRUE)` | 148 | `=VLOOKUP(Left(C56,1), $C$2:$X$1403, 22, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$X$1403, 22, ...` | 148 | `=IF(Len(C56)>2, VLOOKUP(Left(C56,3), $C$2:$X$1403, 22, TRUE),"") ` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$X$1403, 22, T...` | 148 | `=IF(Len(C56)>3,VLOOKUP(Left(C56,4), $C$2:$X$1403, 22, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$AO$1403, 39, TRUE)` | 148 | `=VLOOKUP(Left(C56,1), $C$2:$AO$1403, 39, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$AO$1403, 39,...` | 148 | `=IF(Len(C56)>2, VLOOKUP(Left(C56,3), $C$2:$AO$1403, 39, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$N$1402, 12, TRUE)` | 12 | `=VLOOKUP(Left(C400,1), $C$2:$N$1402, 12, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$N$1402, 12, ...` | 12 | `=IF(Len(C400)>2, VLOOKUP(Left(C400,3), $C$2:$N$1402, 12, TRUE),"") ` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$N$1402, 12, T...` | 12 | `=IF(Len(C400)>3,VLOOKUP(Left(C400,4), $C$2:$N$1402, 12, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$X$1402, 22, TRUE)` | 12 | `=VLOOKUP(Left(C400,1), $C$2:$X$1402, 22, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$X$1402, 22, ...` | 12 | `=IF(Len(C400)>2, VLOOKUP(Left(C400,3), $C$2:$X$1402, 22, TRUE),"") ` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$X$1402, 22, T...` | 12 | `=IF(Len(C400)>3,VLOOKUP(Left(C400,4), $C$2:$X$1402, 22, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$AO$1402, 39, TRUE)` | 12 | `=VLOOKUP(Left(C400,1), $C$2:$AO$1402, 39, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$AO$1402, 39,...` | 12 | `=IF(Len(C400)>2, VLOOKUP(Left(C400,3), $C$2:$AO$1402, 39, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$AO$1402, 39, ...` | 12 | `=IF(Len(C400)>3,VLOOKUP(Left(C400,4), $C$2:$AO$1402, 39, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$AV$1402, 46, TRUE)` | 12 | `=VLOOKUP(Left(C400,1), $C$2:$AV$1402, 46, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$AV$1402, 46,...` | 12 | `=IF(Len(C400)>2, VLOOKUP(Left(C400,3), $C$2:$AV$1402, 46, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$AV$1402, 46, ...` | 12 | `=IF(Len(C400)>3,VLOOKUP(Left(C400,4), $C$2:$AV$1402, 46, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$N$1401, 12, TRUE)` | 1 | `=VLOOKUP(Left(C453,1), $C$2:$N$1401, 12, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$N$1401, 12, ...` | 1 | `=IF(Len(C453)>2, VLOOKUP(Left(C453,3), $C$2:$N$1401, 12, TRUE),"") ` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$N$1401, 12, T...` | 1 | `=IF(Len(C453)>3,VLOOKUP(Left(C453,4), $C$2:$N$1401, 12, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$X$1401, 22, TRUE)` | 1 | `=VLOOKUP(Left(C453,1), $C$2:$X$1401, 22, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$X$1401, 22, ...` | 1 | `=IF(Len(C453)>2, VLOOKUP(Left(C453,3), $C$2:$X$1401, 22, TRUE),"") ` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$X$1401, 22, T...` | 1 | `=IF(Len(C453)>3,VLOOKUP(Left(C453,4), $C$2:$X$1401, 22, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$AO$1401, 39, TRUE)` | 1 | `=VLOOKUP(Left(C453,1), $C$2:$AO$1401, 39, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$AO$1401, 39,...` | 1 | `=IF(Len(C453)>2, VLOOKUP(Left(C453,3), $C$2:$AO$1401, 39, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$AO$1401, 39, ...` | 1 | `=IF(Len(C453)>3,VLOOKUP(Left(C453,4), $C$2:$AO$1401, 39, TRUE),"")` |
| `=VLOOKUP(Left(CELL,1), $C$2:$AV$1401, 46, TRUE)` | 1 | `=VLOOKUP(Left(C453,1), $C$2:$AV$1401, 46, TRUE)` |
| `=IF(Len(CELL)>2, VLOOKUP(Left(CELL,3), $C$2:$AV$1401, 46,...` | 1 | `=IF(Len(C453)>2, VLOOKUP(Left(C453,3), $C$2:$AV$1401, 46, TRUE),"")` |
| `=IF(Len(CELL)>3,VLOOKUP(Left(CELL,4), $C$2:$AV$1401, 46, ...` | 1 | `=IF(Len(C453)>3,VLOOKUP(Left(C453,4), $C$2:$AV$1401, 46, TRUE),"")` |

---

## Scenarii Cards
Spreadsheet: `1SQb9R7Dpi0jPz2JX-HXk1WFn9t68e3aq9MCGif7lM10` | GID: `1376497878`

**Protected cells (formulas): 167**

| Column | Header | Formula Count | Sample Formula |
|--------|--------|---------------|----------------|
| B1 | coordonnées | 167 | `=TEXT(LEFT(A2;FIND(".";A2)-1);"0") & "," & TEXT(MID(A2;FIND(".";A2)+1;FIND("....` |

### Unique Formula Patterns

| Pattern | Count | Example |
|---------|-------|---------|
| `=TEXT(LEFT(CELL;FIND(".";CELL)-1);"0") & "," & TEXT(MID(C...` | 167 | `=TEXT(LEFT(A2;FIND(".";A2)-1);"0") & "," & TEXT(MID(A2;FIND(".";A2)+1;FIND(".";A2;FIND(".";A2)+1)...` |

---

## Virtues Taxonomy
Spreadsheet: `1Asxe0Kb3_pLUSWJnB1HNiBG_EOaz_oU_X3eO9ixnVhA` | GID: `349188118`

**Protected cells (formulas): 1933**

| Column | Header | Formula Count | Sample Formula |
|--------|--------|---------------|----------------|
| A1 | pk | 222 | `=A2+1` |
| C1 | depth | 221 | `=INT(len(B3)/2)+1` |
| D1 | decimal_path_padded | 223 | `=TEXT(SUBSTITUTE(SUBSTITUTE(SUBSTITUTE(B2,".",",",1),".",""),",","."), "0.000...` |
| E1 | family_fr | 223 | `=VLOOKUP(CONCAT(Left(B2,1),".0000000"), $D$2:$H$209, 5, FALSE)` |
| F1 | subfamily_fr | 223 | `=IF(Len(B2)>2, VLOOKUP(CONCAT(Left(D2,3),"000000"), $D$2:$H$242, 5, False),"") ` |
| G1 | subsubfamily_fr | 223 | `=IF(Len(B2)>3,VLOOKUP(CONCAT(Left(D2,4),"00000"), $D$2:$N$242, 5, False),"")` |
| L1 | family_fr_camelcase | 220 | `=REPLACE(SUBSTITUTE(PROPER(TRIM(E2))," ",),1,1,LEFT(LOWER(TRIM(E2))))` |
| M1 | depth_max4 | 220 | `=if(C2<4,C2,"4+")` |
| N1 | card | 158 | `=if(C2<5,1,"")` |

### Unique Formula Patterns

| Pattern | Count | Example |
|---------|-------|---------|
| `=TEXT(SUBSTITUTE(SUBSTITUTE(SUBSTITUTE(CELL,".",",",1),"....` | 223 | `=TEXT(SUBSTITUTE(SUBSTITUTE(SUBSTITUTE(B2,".",",",1),".",""),",","."), "0.0000000")` |
| `=VLOOKUP(CONCAT(Left(CELL,1),".0000000"), $D$2:$H$209, 5,...` | 223 | `=VLOOKUP(CONCAT(Left(B2,1),".0000000"), $D$2:$H$209, 5, FALSE)` |
| `=IF(Len(CELL)>2, VLOOKUP(CONCAT(Left(CELL,3),"000000"), $...` | 223 | `=IF(Len(B2)>2, VLOOKUP(CONCAT(Left(D2,3),"000000"), $D$2:$H$242, 5, False),"") ` |
| `=IF(Len(CELL)>3,VLOOKUP(CONCAT(Left(CELL,4),"00000"), $D$...` | 223 | `=IF(Len(B2)>3,VLOOKUP(CONCAT(Left(D2,4),"00000"), $D$2:$N$242, 5, False),"")` |
| `=CELL+1` | 222 | `=A2+1` |
| `=INT(len(CELL)/2)+1` | 221 | `=INT(len(B3)/2)+1` |
| `=REPLACE(SUBSTITUTE(PROPER(TRIM(CELL))," ",),1,1,LEFT(LOW...` | 220 | `=REPLACE(SUBSTITUTE(PROPER(TRIM(E2))," ",),1,1,LEFT(LOWER(TRIM(E2))))` |
| `=if(CELL<4,CELL,"4+")` | 220 | `=if(C2<4,C2,"4+")` |
| `=if(CELL<5,1,"")` | 158 | `=if(C2<5,1,"")` |

---

## Rules Cards
Spreadsheet: `1jnhlod6PLgvVI-Qgrz3sTYytMgnrMyZrHcc8htPn_DQ` | GID: `0`

**Protected cells (formulas): 0**

No formulas detected. All cells are literal values.

---

## Summary

- Total protected cells across all spreadsheets: **52458**

