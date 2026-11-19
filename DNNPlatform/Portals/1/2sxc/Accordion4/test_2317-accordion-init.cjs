'use strict';

// #2317 — exécute le bundle BS5 réellement livré, sans réseau ni navigateur.
// Deux instances 2sxc appellent init() comme turnOn le fait sur l'accueil.
// Les états sont lus après RAF + transitionend, jamais juste après un clic.

const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const vm = require('node:vm');

const bundlePath = path.join(__dirname, 'bs5', 'dist', 'scripts.min.js');

class ClassList {
  constructor() { this.values = new Set(); }
  add(name) { this.values.add(name); }
  toggle(name) {
    if (this.values.has(name)) { this.values.delete(name); return false; }
    this.values.add(name); return true;
  }
  contains(name) { return this.values.has(name); }
}

class Element {
  constructor(attributes, computed = {}) {
    this.attributes = { ...attributes };
    this.dataset = { accordionParent: attributes['data-accordion-parent'] };
    this.style = {};
    this.computed = {
      height: '120', paddingTop: '0', paddingBottom: '0',
      borderTopWidth: '0', borderBottomWidth: '0', ...computed,
    };    this.classList = new ClassList();
    this.listeners = new Map();
    this.parentElement = null;
    this.clickListenersAdded = 0;
  }
  addEventListener(name, callback) {
    if (!this.listeners.has(name)) this.listeners.set(name, []);
    this.listeners.get(name).push(callback);
    if (name === 'click') this.clickListenersAdded += 1;
  }
  removeEventListener(name, callback) {
    const callbacks = this.listeners.get(name) || [];
    this.listeners.set(name, callbacks.filter(item => item !== callback));
  }
  click() {
    for (const callback of [...(this.listeners.get('click') || [])]) {
      callback({ preventDefault() {}, currentTarget: this });
    }
  }
  transitionEnd() {
    for (const callback of [...(this.listeners.get('transitionend') || [])]) callback({});
  }
  setAttribute(name, value) { this.attributes[name] = value; }
  getAttribute(name) { return this.attributes[name] ?? null; }
  getBoundingClientRect() { return { top: 300 }; }
}

function fixture(hash = '') {
  const items = ['10047', '11889'].map(id => {
    const item = new Element({});
    const title = new Element({ 'data-accordion-parent': id });
    const panel = new Element({ 'data-accordion-child': id }, { height: '0' });
    title.parentElement = item;
    panel.parentElement = item;
    return { id, item, title, panel };
  });
  const frames = [];
  const scrolls = [];
  const location = {
    get hash() { return this._hash; },
    set hash(value) { this._hash = value.startsWith('#') ? value : `#${value}`; },
    _hash: hash,
  };
  const window = {
    location,
    scrollY: 20,
    getComputedStyle: element => ({ ...element.computed, display: element.style.display || 'none' }),
    scrollTo: options => scrolls.push(options),
  };
  const document = {
    querySelectorAll: selector => selector === '[data-accordion-parent]'
      ? items.map(item => item.title) : [],
    querySelector: selector => {
      const match = selector.match(/^\[data-accordion-child="(.+)"\]$/);
      return match ? items.find(item => item.id === match[1])?.panel || null : null;
    },
    getElementsByTagName: () => [],
  };
  const context = {
    window, document,
    location: window.location,
    HTMLElement: Element,
    requestAnimationFrame: callback => frames.push(callback),
  };
  vm.createContext(context);
  vm.runInContext(fs.readFileSync(bundlePath, 'utf8'), context, { filename: bundlePath });

  function flushFrames() {
    while (frames.length) frames.shift()();
  }
  function settle() {
    // Deux générations RAF par animation ; chaque transitionend est suivi d'un
    // tour de drain pour observer une animation concurrente avant de conclure.
    for (let pass = 0; pass < 3; pass += 1) {
      flushFrames();
      for (const item of items) item.panel.transitionEnd();
    }
    flushFrames();
  }
  function initTwice() {
    const data = id => ({
      domId: `app-accordion4-${id}`,
      options: {
        tagStickyHeader: 'header', classIsExpanded: 'is-expanded',
        attrParent: 'data-accordion-parent', attrChild: 'data-accordion-child',
      },
    });
    window.appAccordion4.init(data('first'));
    window.appAccordion4.init(data('second'));
  }
  function snapshot(item) {
    return {
      display: item.panel.style.display || '',
      toggle: item.panel.getAttribute('data-slide-toggle'),
      expanded: item.item.classList.contains('is-expanded'),
      clickListeners: item.title.clickListenersAdded,
      transitionListeners: (item.panel.listeners.get('transitionend') || []).length,
    };
  }
  return { items, initTwice, settle, snapshot, scrolls, window };
}

const failures = [];
function check(name, action) {
  try { action(); console.log(`OK ${name}`); }
  catch (error) { failures.push(name); console.error(`ROUGE ${name}: ${error.message}`); }
}

for (const viewport of [390, 1280]) {
  check(`${viewport}px — deux init(), un écouteur par titre`, () => {
    const page = fixture();
    page.initTwice();
    for (const item of page.items) assert.equal(page.snapshot(item).clickListeners, 1);
  });

  check(`${viewport}px — état ouvert, fermer durablement, rouvrir après stabilisation`, () => {
    const page = fixture('#11889'); // état initial visible, comme le lien direct de la recette
    page.initTwice();
    page.settle();
    const item = page.items[1];
    assert.equal(page.snapshot(item).display, 'block');
    assert.equal(page.snapshot(item).toggle, 'true');

    item.title.click();
    page.settle();
    assert.deepEqual({ ...page.snapshot(item) }, {
      display: 'block', toggle: 'false', expanded: false,
      clickListeners: 1, transitionListeners: 0,
    });
    assert.equal(page.window.location.hash, '#11889');

    item.title.click();
    page.settle();
    assert.deepEqual({ ...page.snapshot(item) }, {
      display: 'block', toggle: 'true', expanded: true,
      clickListeners: 1, transitionListeners: 0,
    });
    // L'autre instance n'a pas été animée par les clics sur celle-ci.
    assert.equal(page.items[0].panel.getAttribute('data-slide-toggle'), null);
  });
}

check('lien direct — le second init ne duplique ni animation ni défilement', () => {
  const page = fixture('#11889');
  page.initTwice();
  page.settle();
  const item = page.items[1];
  assert.equal(page.snapshot(item).toggle, 'true');
  assert.equal(page.snapshot(item).expanded, true);
  assert.equal(page.snapshot(item).clickListeners, 1);
  assert.equal(page.scrolls.length, 1);
  assert.deepEqual({ ...page.scrolls[0] }, { top: 320, left: 0, behavior: 'smooth' });
});

if (failures.length) {
  console.error(`CONTRAT ROUGE (${failures.length}) : ${failures.join(' | ')}`);
  process.exitCode = 1;
} else {
  console.log('CONTRAT VERT : bundle réel, double init, 390/1280 px et lien direct.');
}
