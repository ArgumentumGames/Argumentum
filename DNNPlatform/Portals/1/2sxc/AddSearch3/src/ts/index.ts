declare const addsearch: any;
var winAny = window as any;

function init({ wrapperAttribute } : { wrapperAttribute: string }) {
  // disable DNN form wrapper
  if (document.getElementsByTagName('form').length) document.getElementsByTagName('form')[0].setAttribute('novalidate', '');
  
  const wrapper = document.querySelector(`[${wrapperAttribute}]`);
  const input = wrapper.querySelector(`input`);  

  winAny.addsearch_settings ??= {
    display_url: true,
    display_resultscount: true,
    display_date: true,
    display_sortby: true
  };

  wrapper.querySelector('button').addEventListener('click', () => search(input.value, wrapper.getAttribute('data-resultpage')));
  wrapper.querySelector('input').addEventListener('keypress', (e) => {
    if (e.key !== 'Enter') return;
    e.preventDefault();
    search(input.value, wrapper.getAttribute('data-resultpage'));
  });
}
  
function search(input: string, resultPage: string) {
  location.href = resultPage + "?addsearch=" + input;
  addsearch.submit();
}

winAny.appSearch3 ??= {};
winAny.appSearch3.init ??= init;