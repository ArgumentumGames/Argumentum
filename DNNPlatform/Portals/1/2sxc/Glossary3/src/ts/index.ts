var winAny = window as any;
winAny.appGlossary3 ??= {};
winAny.appGlossary3.init ??= initGloassary3;

function initGloassary3() {
  // get navHeight for later use
  var nav = document.getElementsByTagName('header')[0];
  var navHeight = (nav != null ? nav.offsetHeight : 0);

  document.querySelectorAll('.app-glossary3 a[href*="#"]:not([href="#"])').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
      e.preventDefault();
      
      window.location.hash = this.getAttribute('href');

      // substract scrollOffset with navHeight for accurate position 
      const elemOffsetY = document.querySelector(this.getAttribute('href')).getBoundingClientRect().top + window.scrollY - navHeight;
      // scroll to hash element
      window.scrollTo({
        top: elemOffsetY,
        left: 0,
        behavior: 'smooth'
      });
    });
  });

  // get hash from url and open specific item
  if (window.location.hash) {
    // substract scrollOffset with navHeight for accurate position 
    const elemOffsetY = document.querySelector(window.location.hash).getBoundingClientRect().top + window.scrollY - navHeight;
    // scroll to hash element
    window.scrollTo({
      top: elemOffsetY,
      left: 0,
      behavior: 'smooth'
    });
  }
}