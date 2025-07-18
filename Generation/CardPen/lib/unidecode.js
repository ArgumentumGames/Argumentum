/*! JSunidecode v0.04.9 | GPL */
(function(){
  /*!DATASHEET**/

  String.prototype.unidecode = function()
  {
    var retval="",
        idx=0,
        codepoint,
        section,
        position,
        table;

    for (;idx<this.length;idx++) {
      codepoint = this.charCodeAt(idx);
      if (codepoint < 128) {
        // basic ASCII
        retval += this[idx];
        continue;
      }

      if (codepoint > 983039) { // 0xeffff
        // characters in Private Use Area and above are ignored
        continue;
      }

      section = codepoint >> 8;
      position = codepoint % 256;
      // document.write(section + " " + position + "<br />");

      table = datasheet[section];
      if (table && table.length > position) {
        retval += table[position];
      }
    }

    return retval;
  }
})();


// vim:ai:et:ts=2:sw=2:sts=2:ff=unix:fenc=utf-8:
