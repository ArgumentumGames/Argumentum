var winAny = window as any;
winAny.appCounter2 ??= {};
winAny.appCounter2.init ??= initAppCounter2;
winAny.appCounter2.domIds ??= [];

function initAppCounter2({ domId } : { domId: string }) {
  winAny.appCounter2.domIds.push(domId);
  checkCounters();
}

window.addEventListener('scroll', () => checkCounters());

function checkCounters() {
  winAny.appCounter2.domIds
    .forEach((domId: string) => {
      Array.from(document.getElementsByClassName(domId))
      .forEach((counter: HTMLElement) => {
          if (!counter.classList.contains('app-counter2-js-seen') && isInViewport(counter)) {
            startCounter(counter, counter.getAttribute('data-count'), 2000);
            counter.classList.add('app-counter2-js-seen')
          }
        });
  });
}

function isInViewport(counter: HTMLElement) {
  var elementTop = counter.getBoundingClientRect().top;
  var elementBottom = elementTop + counter.getBoundingClientRect().bottom;
  var viewportTop = window.scrollY;
  var viewportBottom = viewportTop + window.innerHeight;
  return elementBottom > viewportTop && elementTop < viewportBottom;
}

function startCounter(counter: any, lastVal: any, duration: any) {
  let startTime: any = Date.now();
  const step = () => {
      //calculate the value to be used in calculating the number to be displayed
      const progress = Math.min((Date.now() - startTime) / duration, 1);
      //calculate what to be displayed using the value gotten above
      counter.innerHTML = Math.floor(progress * lastVal);
      //checking to make sure the counter does not exceed the last value (lastVal)
      if (progress < 1) window.requestAnimationFrame(step);
      else window.cancelAnimationFrame(window.requestAnimationFrame(step));
  };

  window.requestAnimationFrame(step);
}