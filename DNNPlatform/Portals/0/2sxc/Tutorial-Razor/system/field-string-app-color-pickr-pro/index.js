/*
  This examples shows a plain JS WebComponent which has has a Pickr color-picker
  Uses the neat Pickr from https://simonwep.github.io/pickr/
  This pro-picker allows the admin to define a set of recommended colors
*/

// always use an IFFE to ensure you don't put variables in the window scope
(() => {
  const tagName = 'field-string-app-color-pickr-pro';
  const pickrJsCdn = 'https://cdn.jsdelivr.net/npm/@simonwep/pickr/dist/pickr.min.js';
  const html = `
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@simonwep/pickr/dist/themes/classic.min.css"/>
  <div class="pickr-container"></div>`;

  class StringColorPickerFlat extends HTMLElement {

    /* Constructor for WebComponents - the first line must always be super() */
    constructor() {
      super();
    }

    /* connectedCallback() is the standard callback  when the component has been attached */
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


    /** Load the settings and convert to swatch-list */
    getSwatches() {
      // the field "Swatches" is the field in the content-type containing the colors
      // it's upper-case, because that's how the field is named
      var swatches = this.connector.field.settings.Swatches;
      if (!swatches) return [];
      return swatches.split('\n').map((colorLine) => {
        var withLabel = colorLine.trim().split(' ');
        return withLabel[0]; // first part is the color
      });
    }
  }

  // Register this web component - if it hasn't been registered yet
  if (!customElements.get(tagName)) customElements.define(tagName, StringColorPickerFlat);
})();