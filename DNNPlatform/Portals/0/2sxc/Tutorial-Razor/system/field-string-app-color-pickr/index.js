/*
  This examples shows a plain JS WebComponent which has has a Pickr color-picker
  Uses the neat Pickr from https://simonwep.github.io/pickr/
  This simple picker has a predefined set of colors and doesn't allow field configuration
*/

// always use an IFFE to ensure you don't put variables in the window scope
(() => {

  const tagName = 'field-string-app-color-pickr';
  const pickrJsCdn = 'https://cdn.jsdelivr.net/npm/@simonwep/pickr/dist/pickr.min.js';
  const html = `
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@simonwep/pickr/dist/themes/classic.min.css"/>
  <div class="pickr-container"></div>`;

  class StringColorPicker extends HTMLElement {

    /** connectedCallback() is the standard callback when the component has been attached */
    connectedCallback() {
      this.innerHTML = html;
      // if the window.Pickr doesn't exist yet, load the JS from the CDN () and then do a callback
      this.connector.loadScript('Pickr', pickrJsCdn, () => { this.initPick() });
    }

    /** disconnectedCallback() is a standard callback for clean-up */
    disconnectedCallback() {
      if (this.pickr) this.pickr.destroyAndRemove();
    }

    /** This is called when the JS is loaded from loadScript - so Pickr is ready */
    initPick() {
      this.pickr = new Pickr({
        el: '.pickr-container',
        theme: 'classic',
        default: this.connector.data.value || null,
        defaultRepresentation: 'HEXA',
        swatches: this.getSwatches(),
        components: {
          // Main components
          preview: true,
          opacity: true,
          hue: true,

          // Input / output Options
          interaction: {
            hex: true,
            rgba: true,
            hsla: true,
            hsva: true,
            cmyk: true,
            input: true,
            cancel: true,
            clear: true,
            save: true,
          },
        },
      });
      
      // remember if we're working empty as of now
      this.cleared = !this.connector.data.value;

      // bind events for changes etc. to live-update the preview
      this.pickr.on('change', (color, instance) => this.applyColor(instance));
      this.pickr.on('changestop', (instance) => this.applyColor(instance));
      this.pickr.on('swatchselect', (color, instance) => this.applyColor(instance));

      this.pickr.on('save', (color,instance) => instance.hide());
      this.pickr.on('hide', (instance) => this.update(instance));
      this.pickr.on('clear', (instance) => {
        this.cleared = true;
        this.update();
      });
    }

    /** Update the preview */
    applyColor(instance) {
      this.cleared = false;
      instance.applyColor(true);
    }

    /** Update the value */
    update(instance) {
      // if it's still cleared, just save null
      if (this.cleared) {
        return this.updateIfChanged(null);
      }
      // otherwise get the current color
      var color = instance.getColor();
      if (color) color = color.toHEXA().toString();
      this.updateIfChanged(color);
    }

    /** Only update the value if it really changed, so form isn't dirty if nothing was set */
    updateIfChanged(value) {
      var data = this.connector.data;
      if (data.value === '' && value == null) return;
      if (data.value === value) return;
      data.update(value);
    }
    

    /** Create the default color recommendations for the color picker */
    getSwatches() {
      return [
        'rgba(244, 67, 54, 1)',
        'rgba(233, 30, 99, 0.95)',
        'rgba(156, 39, 176, 0.9)',
        'rgba(103, 58, 183, 0.85)',
        'rgba(63, 81, 181, 0.8)',
        'rgba(33, 150, 243, 0.75)',
        'rgba(3, 169, 244, 0.7)',
        'rgba(0, 188, 212, 0.7)',
        'rgba(0, 150, 136, 0.75)',
        'rgba(76, 175, 80, 0.8)',
        'rgba(139, 195, 74, 0.85)',
        'rgba(205, 220, 57, 0.9)',
        'rgba(255, 235, 59, 0.95)',
        'rgba(255, 193, 7, 1)',
      ]
    }
  }

  // Register this web component - if it hasn't been registered yet
  if (!customElements.get(tagName)) customElements.define(tagName, StringColorPicker);
})();