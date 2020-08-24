/*
  This examples shows a plain JS WebComponent that will just show some messages
  This is just to demonstrate how such a component is built.
*/

// always use an IFFE to ensure you don't put variables in the window scope
(() => {
  const tagName = 'field-empty-app-hello-world';

  class EmptyHelloWorld extends HTMLElement {
    
    /* Constructor for WebComponents - the first line must always be super() */
    constructor() {
      super();
      console.log('FYI: EmptyHelloWorld just constructed!');
    }

    /* connectedCallback() is the standard callback when the component has been attached */
    connectedCallback() {
      this.innerHTML = 'Hello <em>world</em>!';
      console.log('FYI: EmptyHelloWorld just got connected!');
    }

    /** disconnectedCallback() is a standard callback for clean-up */
    disconnectedCallback() {
      console.log('FYI: EmptyHelloWorld getting disconnected - nothing to clean up');
    }
  }

  // Register this web component - if it hasn't been registered yet
  if (!customElements.get(tagName)) customElements.define(tagName, EmptyHelloWorld);
})();