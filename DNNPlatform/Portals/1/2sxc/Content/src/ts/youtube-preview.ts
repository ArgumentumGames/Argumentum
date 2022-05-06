export function activateYouTubeInline() {
  let youtubeElem = document.querySelectorAll('[data-youtube^="iframe-"]');
  // If youtube element is on page
  if(youtubeElem.length != 0) {

    // goes through all youtube elements
    youtubeElem.forEach((ytElem: HTMLElement, index) => {
      
      if(!ytElem.classList.contains('added-listener')) {
        // add click event if not available 
        ytElem.addEventListener('click', () => {  
          const iframeId = ytElem.getAttribute('data-youtube');
          const iframeElem = document.getElementById(iframeId);
          const youtubeUrl = iframeElem.getAttribute('data-youtube-src');
          iframeElem.setAttribute('src', youtubeUrl);
          ytElem.classList.add('hide');
        });

        ytElem.classList.add('added-listener');
      }
      
    });  
  }
}