/*
  This examples shows a JS WebComponent which makes a custom WYSIWYG
*/

// always use an IFFE to ensure you don't put variables in the window scope
(() => {
  const tagName = 'field-string-wysiwyg-micro';
  const builtInWysiwyg = '[System:Path]/system/field-string-wysiwyg/index.js';

  /** Our WebComponent which is a custom, lightweight wysiwyg editor */
  class StringWysiwygCustom extends HTMLElement {

    /* connectedCallback() is the standard callback  when the component has been attached */
    connectedCallback() {
      // We need to ensure that the standard WYSIWYG is also loaded
      this.connector.loadScript('tinymce', builtInWysiwyg, (x) => { this.initWysiwygCallback() })
    }

    initWysiwygCallback() {
      // 1. Create a built-in field-string-wysiwyg control
      const wysiwyg = document.createElement('field-string-wysiwyg');
      // 2. tell it if it should start in preview or edit
      wysiwyg.mode = 'edit'; // can be 'preview' or 'edit'
      // 3. attach connector
      wysiwyg.connector = this.connector;
      // 4. also attach reconfigure object which can change the TinyMCE as it's initialized
      wysiwyg.reconfigure = new WysiwygReconfigurator();
      // 5. Append it to the DOM. Do this last, as it will trigger connectedCallback() in the wysiwyg
      this.appendChild(wysiwyg);
    }
  }

  /** The object which helps reconfigure what the editor will do */
  class WysiwygReconfigurator {
    configureOptions(options) {
      options.toolbar = "undo redo | bold italic"
      return options;
    }
  }

  // Register this web component - if it hasn't been registered yet
  if (!customElements.get(tagName)) customElements.define(tagName, StringWysiwygCustom);
})();