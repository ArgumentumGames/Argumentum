/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "http://localhost:8100/dist/";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 33);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.React;

/***/ }),
/* 1 */
/***/ (function(module, exports, __webpack_require__) {

/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

if (false) { var throwOnDirectAccess, ReactIs; } else {
  // By explicitly using `prop-types` you are opting into new production behavior.
  // http://fb.me/prop-types-in-prod
  module.exports = __webpack_require__(16)();
}


/***/ }),
/* 2 */
/***/ (function(module, exports, __webpack_require__) {

var domToReact = __webpack_require__(22);
var htmlToDOM = __webpack_require__(31);

// decode HTML entities by default for `htmlparser2`
var domParserOptions = { decodeEntities: true, lowerCaseAttributeNames: false };

/**
 * Convert HTML string to React elements.
 *
 * @param  {String}   html              - The HTML string.
 * @param  {Object}   [options]         - The additional options.
 * @param  {Function} [options.replace] - The replace method.
 * @return {ReactElement|Array}
 */
function HTMLReactParser(html, options) {
  if (typeof html !== 'string') {
    throw new TypeError('First argument must be a string');
  }
  return domToReact(htmlToDOM(html, domParserOptions), options);
}

/**
 * Export HTML to React parser.
 */
module.exports = HTMLReactParser;

module.exports.domToReact = domToReact;
module.exports.htmlToDOM = htmlToDOM;


/***/ }),
/* 3 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.Redux;

/***/ }),
/* 4 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactRedux;

/***/ }),
/* 5 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(18);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(20)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 6 */
/***/ (function(module, exports, __webpack_require__) {

var React = __webpack_require__(0);
var hyphenPatternRegex = /-([a-z])/g;
var CUSTOM_PROPERTY_OR_NO_HYPHEN_REGEX = /^--[a-zA-Z0-9-]+$|^[^-]+$/;

/**
 * Converts a string to camelCase.
 *
 * @param  {String} string - The string.
 * @return {String}
 */
function camelCase(string) {
  if (typeof string !== 'string') {
    throw new TypeError('First argument must be a string');
  }

  // custom property or no hyphen found
  if (CUSTOM_PROPERTY_OR_NO_HYPHEN_REGEX.test(string)) {
    return string;
  }

  // convert to camelCase
  return string
    .toLowerCase()
    .replace(hyphenPatternRegex, function(_, character) {
      return character.toUpperCase();
    });
}

/**
 * Swap key with value in an object.
 *
 * @param  {Object}   obj        - The object.
 * @param  {Function} [override] - The override method.
 * @return {Object}              - The inverted object.
 */
function invertObject(obj, override) {
  if (!obj || typeof obj !== 'object') {
    throw new TypeError('First argument must be an object');
  }

  var key;
  var value;
  var isOverridePresent = typeof override === 'function';
  var overrides = {};
  var result = {};

  for (key in obj) {
    value = obj[key];

    if (isOverridePresent) {
      overrides = override(key, value);
      if (overrides && overrides.length === 2) {
        result[overrides[0]] = overrides[1];
        continue;
      }
    }

    if (typeof value === 'string') {
      result[value] = key;
    }
  }

  return result;
}

/**
 * Check if a given tag is a custom component.
 *
 * @see {@link https://github.com/facebook/react/blob/v16.6.3/packages/react-dom/src/shared/isCustomComponent.js}
 *
 * @param {string} tagName - The name of the html tag.
 * @param {Object} props   - The props being passed to the element.
 * @return {boolean}
 */
function isCustomComponent(tagName, props) {
  if (tagName.indexOf('-') === -1) {
    return props && typeof props.is === 'string';
  }

  switch (tagName) {
    // These are reserved SVG and MathML elements.
    // We don't mind this whitelist too much because we expect it to never grow.
    // The alternative is to track the namespace in a few places which is convoluted.
    // https://w3c.github.io/webcomponents/spec/custom/#custom-elements-core-concepts
    case 'annotation-xml':
    case 'color-profile':
    case 'font-face':
    case 'font-face-src':
    case 'font-face-uri':
    case 'font-face-format':
    case 'font-face-name':
    case 'missing-glyph':
      return false;
    default:
      return true;
  }
}

/**
 * @constant {Boolean}
 * @see {@link https://reactjs.org/blog/2017/09/08/dom-attributes-in-react-16.html}
 */
var PRESERVE_CUSTOM_ATTRIBUTES = React.version.split('.')[0] >= 16;

module.exports = {
  PRESERVE_CUSTOM_ATTRIBUTES: PRESERVE_CUSTOM_ATTRIBUTES,
  camelCase: camelCase,
  invertObject: invertObject,
  isCustomComponent: isCustomComponent
};


/***/ }),
/* 7 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 8 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */



var _prodInvariant = __webpack_require__(24);

var invariant = __webpack_require__(25);

function checkMask(value, bitmask) {
  return (value & bitmask) === bitmask;
}

var DOMPropertyInjection = {
  /**
   * Mapping from normalized, camelcased property names to a configuration that
   * specifies how the associated DOM property should be accessed or rendered.
   */
  MUST_USE_PROPERTY: 0x1,
  HAS_BOOLEAN_VALUE: 0x4,
  HAS_NUMERIC_VALUE: 0x8,
  HAS_POSITIVE_NUMERIC_VALUE: 0x10 | 0x8,
  HAS_OVERLOADED_BOOLEAN_VALUE: 0x20,

  /**
   * Inject some specialized knowledge about the DOM. This takes a config object
   * with the following properties:
   *
   * isCustomAttribute: function that given an attribute name will return true
   * if it can be inserted into the DOM verbatim. Useful for data-* or aria-*
   * attributes where it's impossible to enumerate all of the possible
   * attribute names,
   *
   * Properties: object mapping DOM property name to one of the
   * DOMPropertyInjection constants or null. If your attribute isn't in here,
   * it won't get written to the DOM.
   *
   * DOMAttributeNames: object mapping React attribute name to the DOM
   * attribute name. Attribute names not specified use the **lowercase**
   * normalized name.
   *
   * DOMAttributeNamespaces: object mapping React attribute name to the DOM
   * attribute namespace URL. (Attribute names not specified use no namespace.)
   *
   * DOMPropertyNames: similar to DOMAttributeNames but for DOM properties.
   * Property names not specified use the normalized name.
   *
   * DOMMutationMethods: Properties that require special mutation methods. If
   * `value` is undefined, the mutation method should unset the property.
   *
   * @param {object} domPropertyConfig the config as described above.
   */
  injectDOMPropertyConfig: function (domPropertyConfig) {
    var Injection = DOMPropertyInjection;
    var Properties = domPropertyConfig.Properties || {};
    var DOMAttributeNamespaces = domPropertyConfig.DOMAttributeNamespaces || {};
    var DOMAttributeNames = domPropertyConfig.DOMAttributeNames || {};
    var DOMPropertyNames = domPropertyConfig.DOMPropertyNames || {};
    var DOMMutationMethods = domPropertyConfig.DOMMutationMethods || {};

    if (domPropertyConfig.isCustomAttribute) {
      DOMProperty._isCustomAttributeFunctions.push(domPropertyConfig.isCustomAttribute);
    }

    for (var propName in Properties) {
      !!DOMProperty.properties.hasOwnProperty(propName) ?  false ? undefined : _prodInvariant('48', propName) : void 0;

      var lowerCased = propName.toLowerCase();
      var propConfig = Properties[propName];

      var propertyInfo = {
        attributeName: lowerCased,
        attributeNamespace: null,
        propertyName: propName,
        mutationMethod: null,

        mustUseProperty: checkMask(propConfig, Injection.MUST_USE_PROPERTY),
        hasBooleanValue: checkMask(propConfig, Injection.HAS_BOOLEAN_VALUE),
        hasNumericValue: checkMask(propConfig, Injection.HAS_NUMERIC_VALUE),
        hasPositiveNumericValue: checkMask(propConfig, Injection.HAS_POSITIVE_NUMERIC_VALUE),
        hasOverloadedBooleanValue: checkMask(propConfig, Injection.HAS_OVERLOADED_BOOLEAN_VALUE)
      };
      !(propertyInfo.hasBooleanValue + propertyInfo.hasNumericValue + propertyInfo.hasOverloadedBooleanValue <= 1) ?  false ? undefined : _prodInvariant('50', propName) : void 0;

      if (false) {}

      if (DOMAttributeNames.hasOwnProperty(propName)) {
        var attributeName = DOMAttributeNames[propName];
        propertyInfo.attributeName = attributeName;
        if (false) {}
      }

      if (DOMAttributeNamespaces.hasOwnProperty(propName)) {
        propertyInfo.attributeNamespace = DOMAttributeNamespaces[propName];
      }

      if (DOMPropertyNames.hasOwnProperty(propName)) {
        propertyInfo.propertyName = DOMPropertyNames[propName];
      }

      if (DOMMutationMethods.hasOwnProperty(propName)) {
        propertyInfo.mutationMethod = DOMMutationMethods[propName];
      }

      DOMProperty.properties[propName] = propertyInfo;
    }
  }
};

/* eslint-disable max-len */
var ATTRIBUTE_NAME_START_CHAR = ':A-Z_a-z\\u00C0-\\u00D6\\u00D8-\\u00F6\\u00F8-\\u02FF\\u0370-\\u037D\\u037F-\\u1FFF\\u200C-\\u200D\\u2070-\\u218F\\u2C00-\\u2FEF\\u3001-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFFD';
/* eslint-enable max-len */

/**
 * DOMProperty exports lookup objects that can be used like functions:
 *
 *   > DOMProperty.isValid['id']
 *   true
 *   > DOMProperty.isValid['foobar']
 *   undefined
 *
 * Although this may be confusing, it performs better in general.
 *
 * @see http://jsperf.com/key-exists
 * @see http://jsperf.com/key-missing
 */
var DOMProperty = {
  ID_ATTRIBUTE_NAME: 'data-reactid',
  ROOT_ATTRIBUTE_NAME: 'data-reactroot',

  ATTRIBUTE_NAME_START_CHAR: ATTRIBUTE_NAME_START_CHAR,
  ATTRIBUTE_NAME_CHAR: ATTRIBUTE_NAME_START_CHAR + '\\-.0-9\\u00B7\\u0300-\\u036F\\u203F-\\u2040',

  /**
   * Map from property "standard name" to an object with info about how to set
   * the property in the DOM. Each object contains:
   *
   * attributeName:
   *   Used when rendering markup or with `*Attribute()`.
   * attributeNamespace
   * propertyName:
   *   Used on DOM node instances. (This includes properties that mutate due to
   *   external factors.)
   * mutationMethod:
   *   If non-null, used instead of the property or `setAttribute()` after
   *   initial render.
   * mustUseProperty:
   *   Whether the property must be accessed and mutated as an object property.
   * hasBooleanValue:
   *   Whether the property should be removed when set to a falsey value.
   * hasNumericValue:
   *   Whether the property must be numeric or parse as a numeric and should be
   *   removed when set to a falsey value.
   * hasPositiveNumericValue:
   *   Whether the property must be positive numeric or parse as a positive
   *   numeric and should be removed when set to a falsey value.
   * hasOverloadedBooleanValue:
   *   Whether the property can be used as a flag as well as with a value.
   *   Removed when strictly equal to false; present without a value when
   *   strictly equal to true; present with a value otherwise.
   */
  properties: {},

  /**
   * Mapping from lowercase property names to the properly cased version, used
   * to warn in the case of missing properties. Available only in __DEV__.
   *
   * autofocus is predefined, because adding it to the property whitelist
   * causes unintended side effects.
   *
   * @type {Object}
   */
  getPossibleStandardName:  false ? undefined : null,

  /**
   * All of the isCustomAttribute() functions that have been injected.
   */
  _isCustomAttributeFunctions: [],

  /**
   * Checks whether a property name is a custom attribute.
   * @method
   */
  isCustomAttribute: function (attributeName) {
    for (var i = 0; i < DOMProperty._isCustomAttributeFunctions.length; i++) {
      var isCustomAttributeFn = DOMProperty._isCustomAttributeFunctions[i];
      if (isCustomAttributeFn(attributeName)) {
        return true;
      }
    }
    return false;
  },

  injection: DOMPropertyInjection
};

module.exports = DOMProperty;

/***/ }),
/* 9 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


/**
 * Format DOM attributes to an associative array.
 *
 * @param  {NamedNodeMap} - The list of attributes.
 * @return {Object}       - The object of attributes.
 */
function formatAttributes(attributes) {
    var result = {};
    var attribute;

    // NamedNodeMap is array-like
    for (var i = 0, len = attributes.length; i < len; i++) {
        attribute = attributes[i];
        result[attribute.name] = attribute.value;
    }

    return result;
}

/**
 * Format the browser DOM nodes to mimic the output of `htmlparser2.parseDOM()`.
 *
 * @param  {NodeList} nodes        - The DOM nodes.
 * @param  {Object}   [parentObj]  - The formatted parent node.
 * @param  {String}   [directive]  - The directive.
 * @return {Object}                - The formatted DOM object.
 */
function formatDOM(nodes, parentObj, directive) {
    parentObj = parentObj || null;

    var result = [];
    var node;
    var prevNode;
    var nodeObj;

    // NodeList is array-like
    for (var i = 0, len = nodes.length; i < len; i++) {
        node = nodes[i];
        // reset
        nodeObj = {
            next: null,
            prev: result[i - 1] || null,
            parent: parentObj
        };

        // set the next node for the previous node (if applicable)
        prevNode = result[i - 1];
        if (prevNode) {
            prevNode.next = nodeObj;
        }

        // set the node name if it's not "#text" or "#comment"
        // e.g., "div"
        if (node.nodeName.indexOf('#') !== 0) {
            nodeObj.name = node.nodeName.toLowerCase();

            // also, nodes of type "tag" have "attribs"
            nodeObj.attribs = {}; // default
            if (node.attributes && node.attributes.length) {
                nodeObj.attribs = formatAttributes(node.attributes);
            }
        }

        // set the node type
        // e.g., "tag"
        switch (node.nodeType) {
            // 1 = element
            case 1:
                if (nodeObj.name === 'script' || nodeObj.name === 'style') {
                    nodeObj.type = nodeObj.name;
                } else {
                    nodeObj.type = 'tag';
                }
                // recursively format the children
                nodeObj.children = formatDOM(node.childNodes, nodeObj);
                break;
                // 2 = attribute
                // 3 = text
            case 3:
                nodeObj.type = 'text';
                nodeObj.data = node.nodeValue;
                break;
                // 8 = comment
            case 8:
                nodeObj.type = 'comment';
                nodeObj.data = node.nodeValue;
                break;
            default:
                break;
        }

        result.push(nodeObj);
    }

    if (directive) {
        result.unshift({
            name: directive.substring(0, directive.indexOf(' ')).toLowerCase(),
            data: directive,
            type: 'directive',
            next: result[0] ? result[0] : null,
            prev: null,
            parent: parentObj
        });

        if (result[1]) {
            result[1].prev = result[0];
        }
    }

    return result;
}

/**
 * Detect IE with or without version.
 *
 * @param  {Number} [version] - The IE version to detect.
 * @return {Boolean}          - Whether IE or the version has been detected.
 */
function isIE(version) {
    if (version) {
        return document.documentMode === version;
    }
    return /(MSIE |Trident\/|Edge\/)/.test(navigator.userAgent);
}

/**
 * Export utilities.
 */
module.exports = {
    formatAttributes: formatAttributes,
    formatDOM: formatDOM,
    isIE: isIE
};


/***/ }),
/* 10 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 11 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 12 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 13 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 14 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 15 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.CommonComponents;

/***/ }),
/* 16 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(17);

function emptyFunction() {}
function emptyFunctionWithReset() {}
emptyFunctionWithReset.resetWarningCache = emptyFunction;

module.exports = function() {
  function shim(props, propName, componentName, location, propFullName, secret) {
    if (secret === ReactPropTypesSecret) {
      // It is still safe when called from React.
      return;
    }
    var err = new Error(
      'Calling PropTypes validators directly is not supported by the `prop-types` package. ' +
      'Use PropTypes.checkPropTypes() to call them. ' +
      'Read more at http://fb.me/use-check-prop-types'
    );
    err.name = 'Invariant Violation';
    throw err;
  };
  shim.isRequired = shim;
  function getShim() {
    return shim;
  };
  // Important!
  // Keep this list in sync with production version in `./factoryWithTypeCheckers.js`.
  var ReactPropTypes = {
    array: shim,
    bool: shim,
    func: shim,
    number: shim,
    object: shim,
    string: shim,
    symbol: shim,

    any: shim,
    arrayOf: getShim,
    element: shim,
    elementType: shim,
    instanceOf: getShim,
    node: shim,
    objectOf: getShim,
    oneOf: getShim,
    oneOfType: getShim,
    shape: getShim,
    exact: getShim,

    checkPropTypes: emptyFunctionWithReset,
    resetWarningCache: emptyFunction
  };

  ReactPropTypes.PropTypes = ReactPropTypes;

  return ReactPropTypes;
};


/***/ }),
/* 17 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = 'SECRET_DO_NOT_PASS_THIS_OR_YOU_WILL_BE_FIRED';

module.exports = ReactPropTypesSecret;


/***/ }),
/* 18 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(19)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer {\n  /* .wrapper */\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer * {\n  font-size: 16px !important;\n  box-sizing: border-box;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper {\n  display: block;\n  width: 100%;\n  height: 100%;\n  position: absolute;\n  top: 0;\n  left: 0;\n  background: #0B1C24;\n  border: solid 1px #FBFCFC;\n  box-shadow: 0 0 3px #4B4E4F, 0 0 8px #4B4E4F inset;\n  /* remove MS Edge clear button */\n  /* Output text\n    ----------------------------------*/\n  /* Table */\n  /* Busy/Loading Indicator */\n  /* Persona Bar Styling for Help */\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt {\n  background: #0B1C24;\n  position: absolute;\n  top: 0;\n  bottom: 0;\n  left: 0;\n  right: 0;\n  padding: 5px;\n  padding-bottom: 1em;\n  margin: 2px;\n  /* keep status bar from covering prompt input */\n  overflow: auto;\n  z-index: 2000;\n  width: calc(100% - 25px);\n  box-shadow: none;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-output > div {\n  margin: 10px 0;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-output .dnn-prompt-cmd {\n  /*pending*/\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-input-wrapper {\n  position: relative;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-input {\n  display: inline-block;\n  width: 98%;\n  background: transparent;\n  border: none;\n  color: #8AE234;\n  margin: 0;\n  padding: 0;\n  outline: none;\n  /* Get rid of Chrome focus border */\n  position: absolute;\n  left: 17px;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-input.hidden-text {\n  opacity: 0;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-input-wrapper::before {\n  content: \">\";\n  color: #8AE234;\n  margin-left: 3px;\n  display: inline-block;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-input-wrapper.hidden-cursor::before {\n  content: \" \";\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-input::-ms-clear {\n  display: none;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-cmd {\n  color: #AAA;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-error {\n  margin-top: 10px;\n  color: #F00;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-ok {\n  color: #0FF;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-tbl {\n  color: #8AE234;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-tbl thead th {\n  color: #03ade0;\n  border-bottom: 1px dashed;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-tbl td,\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-tbl th {\n  padding: 2px 5px;\n  margin: 2px 5px;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-tbl td:first-child,\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-tbl th:first-child {\n  padding-left: 0;\n  margin-left: 0;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-lbl {\n  color: #FFF;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-val {\n  color: #D3D7CF;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper a.dnn-prompt-cmd-insert,\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper a.dnn-prompt-cmd-insert:link,\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper a.dnn-prompt-cmd-insert:visited,\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper a.dnn-prompt-cmd-insert:active {\n  color: #03ade0;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper .dnn-prompt-busy {\n  width: 24px;\n  height: 24px;\n  border-radius: 50%;\n  border-top: 2px solid #03ade0;\n  border-right: 2px solid transparent;\n  margin: 4px 0;\n  animation: spin 0.6s linear infinite;\n}\n@keyframes spin {\n  to {\n    transform: rotate(360deg);\n  }\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help {\n  color: #AAA;\n  margin-top: 0.8em !important;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help em {\n  font-style: italic !important;\n  font-size: 1em !important;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help strong {\n  font-weight: bold !important;\n  font-size: 1em !important;\n  color: #DDD;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help .text-danger {\n  color: red;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help h4 {\n  color: #FFF;\n  margin: 15px auto 10px auto !important;\n  font-size: 1.3em !important;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help h3 {\n  color: #03ade0 !important;\n  background-color: azure;\n  font-size: 1.5em !important;\n  margin: 10px auto !important;\n  padding: 0.5em;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help p {\n  margin: 10px auto;\n  line-height: 1.2em !important;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help p.lead {\n  font-size: 1.1em !important;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help blockquote {\n  line-height: 1.2em !important;\n  border: 1px solid rgba(0, 255, 0, 0.4);\n  border-left: 10px solid rgba(0, 255, 0, 0.4);\n  padding: 10px;\n  font-size: 0.85em !important;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help .mono {\n  color: #ff8c00;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help li {\n  margin-left: 2.2em;\n  margin-bottom: 0.6em;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help code.block {\n  display: inline-block;\n  padding: 8px;\n  border: 1px solid rgba(255, 140, 0, 0.1);\n  background: rgba(255, 140, 0, 0.08);\n  color: #ff8c00;\n  margin: 5px auto;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help table thead th {\n  background: rgba(255, 255, 255, 0.4);\n  color: #FFF;\n  padding: 8px;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help table.table td {\n  padding: 5px;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help table.table tr td:first-child {\n  white-space: nowrap;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help table.command-result-tbl {\n  margin-top: 10px;\n  margin-bottom: 10px;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help table.command-result-tbl td {\n  font-size: 0.85em !important;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help table.command-result-tbl tr td:first-child {\n  white-space: nowrap;\n}\n#dnnPrompt-container .dnnPrompt-app.personaBar-mainContainer .wrapper section.dnn-prompt-inline-help table tr.divider td {\n  text-transform: uppercase;\n  font-size: 1.3em !important;\n  text-align: center;\n  padding: 1em !important;\n  color: #DDD;\n}\n#showsite {\n  margin-right: 25px;\n}\n", ""]);



/***/ }),
/* 19 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


/*
  MIT License http://www.opensource.org/licenses/mit-license.php
  Author Tobias Koppers @sokra
*/
// css base code, injected by the css-loader
module.exports = function (useSourceMap) {
  var list = []; // return the list of modules as css string

  list.toString = function toString() {
    return this.map(function (item) {
      var content = cssWithMappingToString(item, useSourceMap);

      if (item[2]) {
        return '@media ' + item[2] + '{' + content + '}';
      } else {
        return content;
      }
    }).join('');
  }; // import a list of modules into the list


  list.i = function (modules, mediaQuery) {
    if (typeof modules === 'string') {
      modules = [[null, modules, '']];
    }

    var alreadyImportedModules = {};

    for (var i = 0; i < this.length; i++) {
      var id = this[i][0];

      if (id != null) {
        alreadyImportedModules[id] = true;
      }
    }

    for (i = 0; i < modules.length; i++) {
      var item = modules[i]; // skip already imported module
      // this implementation is not 100% perfect for weird media query combinations
      // when a module is imported multiple times with different media queries.
      // I hope this will never occur (Hey this way we have smaller bundles)

      if (item[0] == null || !alreadyImportedModules[item[0]]) {
        if (mediaQuery && !item[2]) {
          item[2] = mediaQuery;
        } else if (mediaQuery) {
          item[2] = '(' + item[2] + ') and (' + mediaQuery + ')';
        }

        list.push(item);
      }
    }
  };

  return list;
};

function cssWithMappingToString(item, useSourceMap) {
  var content = item[1] || '';
  var cssMapping = item[3];

  if (!cssMapping) {
    return content;
  }

  if (useSourceMap && typeof btoa === 'function') {
    var sourceMapping = toComment(cssMapping);
    var sourceURLs = cssMapping.sources.map(function (source) {
      return '/*# sourceURL=' + cssMapping.sourceRoot + source + ' */';
    });
    return [content].concat(sourceURLs).concat([sourceMapping]).join('\n');
  }

  return [content].join('\n');
} // Adapted from convert-source-map (MIT)


function toComment(sourceMap) {
  // eslint-disable-next-line no-undef
  var base64 = btoa(unescape(encodeURIComponent(JSON.stringify(sourceMap))));
  var data = 'sourceMappingURL=data:application/json;charset=utf-8;base64,' + base64;
  return '/*# ' + data + ' */';
}

/***/ }),
/* 20 */
/***/ (function(module, exports, __webpack_require__) {

/*
	MIT License http://www.opensource.org/licenses/mit-license.php
	Author Tobias Koppers @sokra
*/

var stylesInDom = {};

var	memoize = function (fn) {
	var memo;

	return function () {
		if (typeof memo === "undefined") memo = fn.apply(this, arguments);
		return memo;
	};
};

var isOldIE = memoize(function () {
	// Test for IE <= 9 as proposed by Browserhacks
	// @see http://browserhacks.com/#hack-e71d8692f65334173fee715c222cb805
	// Tests for existence of standard globals is to allow style-loader
	// to operate correctly into non-standard environments
	// @see https://github.com/webpack-contrib/style-loader/issues/177
	return window && document && document.all && !window.atob;
});

var getTarget = function (target, parent) {
  if (parent){
    return parent.querySelector(target);
  }
  return document.querySelector(target);
};

var getElement = (function (fn) {
	var memo = {};

	return function(target, parent) {
                // If passing function in options, then use it for resolve "head" element.
                // Useful for Shadow Root style i.e
                // {
                //   insertInto: function () { return document.querySelector("#foo").shadowRoot }
                // }
                if (typeof target === 'function') {
                        return target();
                }
                if (typeof memo[target] === "undefined") {
			var styleTarget = getTarget.call(this, target, parent);
			// Special case to return head of iframe instead of iframe itself
			if (window.HTMLIFrameElement && styleTarget instanceof window.HTMLIFrameElement) {
				try {
					// This will throw an exception if access to iframe is blocked
					// due to cross-origin restrictions
					styleTarget = styleTarget.contentDocument.head;
				} catch(e) {
					styleTarget = null;
				}
			}
			memo[target] = styleTarget;
		}
		return memo[target]
	};
})();

var singleton = null;
var	singletonCounter = 0;
var	stylesInsertedAtTop = [];

var	fixUrls = __webpack_require__(21);

module.exports = function(list, options) {
	if (typeof DEBUG !== "undefined" && DEBUG) {
		if (typeof document !== "object") throw new Error("The style-loader cannot be used in a non-browser environment");
	}

	options = options || {};

	options.attrs = typeof options.attrs === "object" ? options.attrs : {};

	// Force single-tag solution on IE6-9, which has a hard limit on the # of <style>
	// tags it will allow on a page
	if (!options.singleton && typeof options.singleton !== "boolean") options.singleton = isOldIE();

	// By default, add <style> tags to the <head> element
        if (!options.insertInto) options.insertInto = "head";

	// By default, add <style> tags to the bottom of the target
	if (!options.insertAt) options.insertAt = "bottom";

	var styles = listToStyles(list, options);

	addStylesToDom(styles, options);

	return function update (newList) {
		var mayRemove = [];

		for (var i = 0; i < styles.length; i++) {
			var item = styles[i];
			var domStyle = stylesInDom[item.id];

			domStyle.refs--;
			mayRemove.push(domStyle);
		}

		if(newList) {
			var newStyles = listToStyles(newList, options);
			addStylesToDom(newStyles, options);
		}

		for (var i = 0; i < mayRemove.length; i++) {
			var domStyle = mayRemove[i];

			if(domStyle.refs === 0) {
				for (var j = 0; j < domStyle.parts.length; j++) domStyle.parts[j]();

				delete stylesInDom[domStyle.id];
			}
		}
	};
};

function addStylesToDom (styles, options) {
	for (var i = 0; i < styles.length; i++) {
		var item = styles[i];
		var domStyle = stylesInDom[item.id];

		if(domStyle) {
			domStyle.refs++;

			for(var j = 0; j < domStyle.parts.length; j++) {
				domStyle.parts[j](item.parts[j]);
			}

			for(; j < item.parts.length; j++) {
				domStyle.parts.push(addStyle(item.parts[j], options));
			}
		} else {
			var parts = [];

			for(var j = 0; j < item.parts.length; j++) {
				parts.push(addStyle(item.parts[j], options));
			}

			stylesInDom[item.id] = {id: item.id, refs: 1, parts: parts};
		}
	}
}

function listToStyles (list, options) {
	var styles = [];
	var newStyles = {};

	for (var i = 0; i < list.length; i++) {
		var item = list[i];
		var id = options.base ? item[0] + options.base : item[0];
		var css = item[1];
		var media = item[2];
		var sourceMap = item[3];
		var part = {css: css, media: media, sourceMap: sourceMap};

		if(!newStyles[id]) styles.push(newStyles[id] = {id: id, parts: [part]});
		else newStyles[id].parts.push(part);
	}

	return styles;
}

function insertStyleElement (options, style) {
	var target = getElement(options.insertInto)

	if (!target) {
		throw new Error("Couldn't find a style target. This probably means that the value for the 'insertInto' parameter is invalid.");
	}

	var lastStyleElementInsertedAtTop = stylesInsertedAtTop[stylesInsertedAtTop.length - 1];

	if (options.insertAt === "top") {
		if (!lastStyleElementInsertedAtTop) {
			target.insertBefore(style, target.firstChild);
		} else if (lastStyleElementInsertedAtTop.nextSibling) {
			target.insertBefore(style, lastStyleElementInsertedAtTop.nextSibling);
		} else {
			target.appendChild(style);
		}
		stylesInsertedAtTop.push(style);
	} else if (options.insertAt === "bottom") {
		target.appendChild(style);
	} else if (typeof options.insertAt === "object" && options.insertAt.before) {
		var nextSibling = getElement(options.insertAt.before, target);
		target.insertBefore(style, nextSibling);
	} else {
		throw new Error("[Style Loader]\n\n Invalid value for parameter 'insertAt' ('options.insertAt') found.\n Must be 'top', 'bottom', or Object.\n (https://github.com/webpack-contrib/style-loader#insertat)\n");
	}
}

function removeStyleElement (style) {
	if (style.parentNode === null) return false;
	style.parentNode.removeChild(style);

	var idx = stylesInsertedAtTop.indexOf(style);
	if(idx >= 0) {
		stylesInsertedAtTop.splice(idx, 1);
	}
}

function createStyleElement (options) {
	var style = document.createElement("style");

	if(options.attrs.type === undefined) {
		options.attrs.type = "text/css";
	}

	if(options.attrs.nonce === undefined) {
		var nonce = getNonce();
		if (nonce) {
			options.attrs.nonce = nonce;
		}
	}

	addAttrs(style, options.attrs);
	insertStyleElement(options, style);

	return style;
}

function createLinkElement (options) {
	var link = document.createElement("link");

	if(options.attrs.type === undefined) {
		options.attrs.type = "text/css";
	}
	options.attrs.rel = "stylesheet";

	addAttrs(link, options.attrs);
	insertStyleElement(options, link);

	return link;
}

function addAttrs (el, attrs) {
	Object.keys(attrs).forEach(function (key) {
		el.setAttribute(key, attrs[key]);
	});
}

function getNonce() {
	if (false) {}

	return __webpack_require__.nc;
}

function addStyle (obj, options) {
	var style, update, remove, result;

	// If a transform function was defined, run it on the css
	if (options.transform && obj.css) {
	    result = typeof options.transform === 'function'
		 ? options.transform(obj.css) 
		 : options.transform.default(obj.css);

	    if (result) {
	    	// If transform returns a value, use that instead of the original css.
	    	// This allows running runtime transformations on the css.
	    	obj.css = result;
	    } else {
	    	// If the transform function returns a falsy value, don't add this css.
	    	// This allows conditional loading of css
	    	return function() {
	    		// noop
	    	};
	    }
	}

	if (options.singleton) {
		var styleIndex = singletonCounter++;

		style = singleton || (singleton = createStyleElement(options));

		update = applyToSingletonTag.bind(null, style, styleIndex, false);
		remove = applyToSingletonTag.bind(null, style, styleIndex, true);

	} else if (
		obj.sourceMap &&
		typeof URL === "function" &&
		typeof URL.createObjectURL === "function" &&
		typeof URL.revokeObjectURL === "function" &&
		typeof Blob === "function" &&
		typeof btoa === "function"
	) {
		style = createLinkElement(options);
		update = updateLink.bind(null, style, options);
		remove = function () {
			removeStyleElement(style);

			if(style.href) URL.revokeObjectURL(style.href);
		};
	} else {
		style = createStyleElement(options);
		update = applyToTag.bind(null, style);
		remove = function () {
			removeStyleElement(style);
		};
	}

	update(obj);

	return function updateStyle (newObj) {
		if (newObj) {
			if (
				newObj.css === obj.css &&
				newObj.media === obj.media &&
				newObj.sourceMap === obj.sourceMap
			) {
				return;
			}

			update(obj = newObj);
		} else {
			remove();
		}
	};
}

var replaceText = (function () {
	var textStore = [];

	return function (index, replacement) {
		textStore[index] = replacement;

		return textStore.filter(Boolean).join('\n');
	};
})();

function applyToSingletonTag (style, index, remove, obj) {
	var css = remove ? "" : obj.css;

	if (style.styleSheet) {
		style.styleSheet.cssText = replaceText(index, css);
	} else {
		var cssNode = document.createTextNode(css);
		var childNodes = style.childNodes;

		if (childNodes[index]) style.removeChild(childNodes[index]);

		if (childNodes.length) {
			style.insertBefore(cssNode, childNodes[index]);
		} else {
			style.appendChild(cssNode);
		}
	}
}

function applyToTag (style, obj) {
	var css = obj.css;
	var media = obj.media;

	if(media) {
		style.setAttribute("media", media)
	}

	if(style.styleSheet) {
		style.styleSheet.cssText = css;
	} else {
		while(style.firstChild) {
			style.removeChild(style.firstChild);
		}

		style.appendChild(document.createTextNode(css));
	}
}

function updateLink (link, options, obj) {
	var css = obj.css;
	var sourceMap = obj.sourceMap;

	/*
		If convertToAbsoluteUrls isn't defined, but sourcemaps are enabled
		and there is no publicPath defined then lets turn convertToAbsoluteUrls
		on by default.  Otherwise default to the convertToAbsoluteUrls option
		directly
	*/
	var autoFixUrls = options.convertToAbsoluteUrls === undefined && sourceMap;

	if (options.convertToAbsoluteUrls || autoFixUrls) {
		css = fixUrls(css);
	}

	if (sourceMap) {
		// http://stackoverflow.com/a/26603875
		css += "\n/*# sourceMappingURL=data:application/json;base64," + btoa(unescape(encodeURIComponent(JSON.stringify(sourceMap)))) + " */";
	}

	var blob = new Blob([css], { type: "text/css" });

	var oldSrc = link.href;

	link.href = URL.createObjectURL(blob);

	if(oldSrc) URL.revokeObjectURL(oldSrc);
}


/***/ }),
/* 21 */
/***/ (function(module, exports) {


/**
 * When source maps are enabled, `style-loader` uses a link element with a data-uri to
 * embed the css on the page. This breaks all relative urls because now they are relative to a
 * bundle instead of the current page.
 *
 * One solution is to only use full urls, but that may be impossible.
 *
 * Instead, this function "fixes" the relative urls to be absolute according to the current page location.
 *
 * A rudimentary test suite is located at `test/fixUrls.js` and can be run via the `npm test` command.
 *
 */

module.exports = function (css) {
  // get current location
  var location = typeof window !== "undefined" && window.location;

  if (!location) {
    throw new Error("fixUrls requires window.location");
  }

	// blank or null?
	if (!css || typeof css !== "string") {
	  return css;
  }

  var baseUrl = location.protocol + "//" + location.host;
  var currentDir = baseUrl + location.pathname.replace(/\/[^\/]*$/, "/");

	// convert each url(...)
	/*
	This regular expression is just a way to recursively match brackets within
	a string.

	 /url\s*\(  = Match on the word "url" with any whitespace after it and then a parens
	   (  = Start a capturing group
	     (?:  = Start a non-capturing group
	         [^)(]  = Match anything that isn't a parentheses
	         |  = OR
	         \(  = Match a start parentheses
	             (?:  = Start another non-capturing groups
	                 [^)(]+  = Match anything that isn't a parentheses
	                 |  = OR
	                 \(  = Match a start parentheses
	                     [^)(]*  = Match anything that isn't a parentheses
	                 \)  = Match a end parentheses
	             )  = End Group
              *\) = Match anything and then a close parens
          )  = Close non-capturing group
          *  = Match anything
       )  = Close capturing group
	 \)  = Match a close parens

	 /gi  = Get all matches, not the first.  Be case insensitive.
	 */
	var fixedCss = css.replace(/url\s*\(((?:[^)(]|\((?:[^)(]+|\([^)(]*\))*\))*)\)/gi, function(fullMatch, origUrl) {
		// strip quotes (if they exist)
		var unquotedOrigUrl = origUrl
			.trim()
			.replace(/^"(.*)"$/, function(o, $1){ return $1; })
			.replace(/^'(.*)'$/, function(o, $1){ return $1; });

		// already a full url? no change
		if (/^(#|data:|http:\/\/|https:\/\/|file:\/\/\/|\s*$)/i.test(unquotedOrigUrl)) {
		  return fullMatch;
		}

		// convert the url to a full url
		var newUrl;

		if (unquotedOrigUrl.indexOf("//") === 0) {
		  	//TODO: should we add protocol?
			newUrl = unquotedOrigUrl;
		} else if (unquotedOrigUrl.indexOf("/") === 0) {
			// path should be relative to the base url
			newUrl = baseUrl + unquotedOrigUrl; // already starts with '/'
		} else {
			// path should be relative to current directory
			newUrl = currentDir + unquotedOrigUrl.replace(/^\.\//, ""); // Strip leading './'
		}

		// send back the fixed url(...)
		return "url(" + JSON.stringify(newUrl) + ")";
	});

	// send back the fixed css
	return fixedCss;
};


/***/ }),
/* 22 */
/***/ (function(module, exports, __webpack_require__) {

var React = __webpack_require__(0);
var attributesToProps = __webpack_require__(23);
var utilities = __webpack_require__(6);

/**
 * Converts DOM nodes to React elements.
 *
 * @param  {Array}    nodes             - The DOM nodes.
 * @param  {Object}   [options]         - The additional options.
 * @param  {Function} [options.replace] - The replace method.
 * @return {ReactElement|Array}
 */
function domToReact(nodes, options) {
  options = options || {};
  var result = [];
  var node;
  var isReplacePresent = typeof options.replace === 'function';
  var replacement;
  var props;
  var children;

  for (var i = 0, len = nodes.length; i < len; i++) {
    node = nodes[i];

    // replace with custom React element (if applicable)
    if (isReplacePresent) {
      replacement = options.replace(node);

      if (React.isValidElement(replacement)) {
        // specify a "key" prop if element has siblings
        // https://fb.me/react-warning-keys
        if (len > 1) {
          replacement = React.cloneElement(replacement, {
            key: replacement.key || i
          });
        }
        result.push(replacement);
        continue;
      }
    }

    if (node.type === 'text') {
      result.push(node.data);
      continue;
    }

    props = node.attribs;
    if (!shouldPassAttributesUnaltered(node)) {
      // update values
      props = attributesToProps(node.attribs);
    }

    children = null;

    // node type for <script> is "script"
    // node type for <style> is "style"
    if (node.type === 'script' || node.type === 'style') {
      // prevent text in <script> or <style> from being escaped
      // https://facebook.github.io/react/tips/dangerously-set-inner-html.html
      if (node.children[0]) {
        props.dangerouslySetInnerHTML = {
          __html: node.children[0].data
        };
      }
    } else if (node.type === 'tag') {
      // setting textarea value in children is an antipattern in React
      // https://reactjs.org/docs/forms.html#the-textarea-tag
      if (node.name === 'textarea' && node.children[0]) {
        props.defaultValue = node.children[0].data;

        // continue recursion of creating React elements (if applicable)
      } else if (node.children && node.children.length) {
        children = domToReact(node.children, options);
      }

      // skip all other cases (e.g., comment)
    } else {
      continue;
    }

    // specify a "key" prop if element has siblings
    // https://fb.me/react-warning-keys
    if (len > 1) {
      props.key = i;
    }

    result.push(React.createElement(node.name, props, children));
  }

  return result.length === 1 ? result[0] : result;
}

function shouldPassAttributesUnaltered(node) {
  return (
    utilities.PRESERVE_CUSTOM_ATTRIBUTES &&
    node.type === 'tag' &&
    utilities.isCustomComponent(node.name, node.attribs)
  );
}

module.exports = domToReact;


/***/ }),
/* 23 */
/***/ (function(module, exports, __webpack_require__) {

var DOMProperty = __webpack_require__(8);
var propertyConfig = __webpack_require__(26);
var styleToObject = __webpack_require__(29);
var utilities = __webpack_require__(6);

var config = propertyConfig.config;
var isCustomAttribute = propertyConfig.HTMLDOMPropertyConfig.isCustomAttribute;
DOMProperty.injection.injectDOMPropertyConfig(
  propertyConfig.HTMLDOMPropertyConfig
);

/**
 * Makes attributes compatible with React props.
 *
 * @param  {Object} [attributes={}] - The attributes.
 * @return {Object}                 - The props.
 */
function attributesToProps(attributes) {
  attributes = attributes || {};
  var props = {};
  var propertyName;
  var propertyValue;
  var reactProperty;

  for (propertyName in attributes) {
    propertyValue = attributes[propertyName];

    // custom attributes (`data-` and `aria-`)
    if (isCustomAttribute(propertyName)) {
      props[propertyName] = propertyValue;
      continue;
    }

    // make HTML DOM attribute/property consistent with React attribute/property
    reactProperty = config.html[propertyName.toLowerCase()];
    if (reactProperty) {
      if (
        DOMProperty.properties.hasOwnProperty(reactProperty) &&
        DOMProperty.properties[reactProperty].hasBooleanValue
      ) {
        props[reactProperty] = true;
      } else {
        props[reactProperty] = propertyValue;
      }
      continue;
    }

    // make SVG DOM attribute/property consistent with React attribute/property
    reactProperty = config.svg[propertyName];
    if (reactProperty) {
      props[reactProperty] = propertyValue;
    } else if (utilities.PRESERVE_CUSTOM_ATTRIBUTES) {
      props[propertyName] = propertyValue;
    }
  }

  // convert inline style to object
  if (attributes.style != null) {
    props.style = cssToJs(attributes.style);
  }
  return props;
}

/**
 * Converts CSS style string to JS style object.
 *
 * @param  {String} style - The CSS style.
 * @return {Object}       - The JS style object.
 */
function cssToJs(style) {
  if (typeof style !== 'string') {
    throw new TypeError('First argument must be a string.');
  }
  var styleObj = {};

  styleToObject(style, function(propName, propValue) {
    // Check if it's not a comment node
    if (propName && propValue) {
      styleObj[utilities.camelCase(propName)] = propValue;
    }
  });
  return styleObj;
}

module.exports = attributesToProps;


/***/ }),
/* 24 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 * 
 */


/**
 * WARNING: DO NOT manually require this module.
 * This is a replacement for `invariant(...)` used by the error code system
 * and will _only_ be required by the corresponding babel pass.
 * It always throws.
 */

function reactProdInvariant(code) {
  var argCount = arguments.length - 1;

  var message = 'Minified React error #' + code + '; visit ' + 'http://facebook.github.io/react/docs/error-decoder.html?invariant=' + code;

  for (var argIdx = 0; argIdx < argCount; argIdx++) {
    message += '&args[]=' + encodeURIComponent(arguments[argIdx + 1]);
  }

  message += ' for the full message or use the non-minified dev environment' + ' for full errors and additional helpful warnings.';

  var error = new Error(message);
  error.name = 'Invariant Violation';
  error.framesToPop = 1; // we don't care about reactProdInvariant's own frame

  throw error;
}

module.exports = reactProdInvariant;

/***/ }),
/* 25 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 * 
 */


var validateFormat =  false ? undefined : function (format) {
  if (format === undefined) {
    throw new Error('invariant(...): Second argument must be a string.');
  }
};
/**
 * Use invariant() to assert state which your program assumes to be true.
 *
 * Provide sprintf-style format (only %s is supported) and arguments to provide
 * information about what broke and what you were expecting.
 *
 * The invariant message will be stripped in production, but the invariant will
 * remain to ensure logic does not differ in production.
 */

function invariant(condition, format) {
  for (var _len = arguments.length, args = new Array(_len > 2 ? _len - 2 : 0), _key = 2; _key < _len; _key++) {
    args[_key - 2] = arguments[_key];
  }

  validateFormat(format);

  if (!condition) {
    var error;

    if (format === undefined) {
      error = new Error('Minified exception occurred; use the non-minified dev environment ' + 'for the full error message and additional helpful warnings.');
    } else {
      var argIndex = 0;
      error = new Error(format.replace(/%s/g, function () {
        return String(args[argIndex++]);
      }));
      error.name = 'Invariant Violation';
    }

    error.framesToPop = 1; // Skip invariant's own stack frame.

    throw error;
  }
}

module.exports = invariant;

/***/ }),
/* 26 */
/***/ (function(module, exports, __webpack_require__) {

var HTMLDOMPropertyConfig = __webpack_require__(27);
var SVGDOMPropertyConfig = __webpack_require__(28);
var utilities = __webpack_require__(6);

var config = {
  html: {},
  svg: {}
};

var propertyName;

/**
 * HTML DOM property config.
 *
 * https://github.com/facebook/react/blob/15-stable/src/renderers/dom/shared/HTMLDOMPropertyConfig.js
 */

// first map out the HTML attribute names
// e.g., { className: 'class' } => { 'class': 'className' }
config.html = utilities.invertObject(HTMLDOMPropertyConfig.DOMAttributeNames);

// then map out the rest of the HTML properties
// e.g., { readOnly: 0 } => { readonly: 'readOnly' }
for (propertyName in HTMLDOMPropertyConfig.Properties) {
  // lowercase to make matching property names easier
  config.html[propertyName.toLowerCase()] = propertyName;
}

/**
 * SVG DOM property config.
 *
 * https://github.com/facebook/react/blob/15-stable/src/renderers/dom/shared/SVGDOMPropertyConfig.js
 */

// first map out the SVG attribute names
// e.g., { fontSize: 'font-size' } => { 'font-size': 'fontSize' }
config.svg = utilities.invertObject(SVGDOMPropertyConfig.DOMAttributeNames);

// then map out the rest of the SVG properties
// e.g., { fillRule: 0 } => { fillRule: 'fillRule' }
for (propertyName in SVGDOMPropertyConfig.Properties) {
  // do not lowercase as some svg properties are camel cased
  config.html[propertyName] = propertyName;
}

module.exports = {
  config: config,
  HTMLDOMPropertyConfig: HTMLDOMPropertyConfig,
  SVGDOMPropertyConfig: SVGDOMPropertyConfig
};


/***/ }),
/* 27 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */



var DOMProperty = __webpack_require__(8);

var MUST_USE_PROPERTY = DOMProperty.injection.MUST_USE_PROPERTY;
var HAS_BOOLEAN_VALUE = DOMProperty.injection.HAS_BOOLEAN_VALUE;
var HAS_NUMERIC_VALUE = DOMProperty.injection.HAS_NUMERIC_VALUE;
var HAS_POSITIVE_NUMERIC_VALUE = DOMProperty.injection.HAS_POSITIVE_NUMERIC_VALUE;
var HAS_OVERLOADED_BOOLEAN_VALUE = DOMProperty.injection.HAS_OVERLOADED_BOOLEAN_VALUE;

var HTMLDOMPropertyConfig = {
  isCustomAttribute: RegExp.prototype.test.bind(new RegExp('^(data|aria)-[' + DOMProperty.ATTRIBUTE_NAME_CHAR + ']*$')),
  Properties: {
    /**
     * Standard Properties
     */
    accept: 0,
    acceptCharset: 0,
    accessKey: 0,
    action: 0,
    allowFullScreen: HAS_BOOLEAN_VALUE,
    allowTransparency: 0,
    alt: 0,
    // specifies target context for links with `preload` type
    as: 0,
    async: HAS_BOOLEAN_VALUE,
    autoComplete: 0,
    // autoFocus is polyfilled/normalized by AutoFocusUtils
    // autoFocus: HAS_BOOLEAN_VALUE,
    autoPlay: HAS_BOOLEAN_VALUE,
    capture: HAS_BOOLEAN_VALUE,
    cellPadding: 0,
    cellSpacing: 0,
    charSet: 0,
    challenge: 0,
    checked: MUST_USE_PROPERTY | HAS_BOOLEAN_VALUE,
    cite: 0,
    classID: 0,
    className: 0,
    cols: HAS_POSITIVE_NUMERIC_VALUE,
    colSpan: 0,
    content: 0,
    contentEditable: 0,
    contextMenu: 0,
    controls: HAS_BOOLEAN_VALUE,
    controlsList: 0,
    coords: 0,
    crossOrigin: 0,
    data: 0, // For `<object />` acts as `src`.
    dateTime: 0,
    'default': HAS_BOOLEAN_VALUE,
    defer: HAS_BOOLEAN_VALUE,
    dir: 0,
    disabled: HAS_BOOLEAN_VALUE,
    download: HAS_OVERLOADED_BOOLEAN_VALUE,
    draggable: 0,
    encType: 0,
    form: 0,
    formAction: 0,
    formEncType: 0,
    formMethod: 0,
    formNoValidate: HAS_BOOLEAN_VALUE,
    formTarget: 0,
    frameBorder: 0,
    headers: 0,
    height: 0,
    hidden: HAS_BOOLEAN_VALUE,
    high: 0,
    href: 0,
    hrefLang: 0,
    htmlFor: 0,
    httpEquiv: 0,
    icon: 0,
    id: 0,
    inputMode: 0,
    integrity: 0,
    is: 0,
    keyParams: 0,
    keyType: 0,
    kind: 0,
    label: 0,
    lang: 0,
    list: 0,
    loop: HAS_BOOLEAN_VALUE,
    low: 0,
    manifest: 0,
    marginHeight: 0,
    marginWidth: 0,
    max: 0,
    maxLength: 0,
    media: 0,
    mediaGroup: 0,
    method: 0,
    min: 0,
    minLength: 0,
    // Caution; `option.selected` is not updated if `select.multiple` is
    // disabled with `removeAttribute`.
    multiple: MUST_USE_PROPERTY | HAS_BOOLEAN_VALUE,
    muted: MUST_USE_PROPERTY | HAS_BOOLEAN_VALUE,
    name: 0,
    nonce: 0,
    noValidate: HAS_BOOLEAN_VALUE,
    open: HAS_BOOLEAN_VALUE,
    optimum: 0,
    pattern: 0,
    placeholder: 0,
    playsInline: HAS_BOOLEAN_VALUE,
    poster: 0,
    preload: 0,
    profile: 0,
    radioGroup: 0,
    readOnly: HAS_BOOLEAN_VALUE,
    referrerPolicy: 0,
    rel: 0,
    required: HAS_BOOLEAN_VALUE,
    reversed: HAS_BOOLEAN_VALUE,
    role: 0,
    rows: HAS_POSITIVE_NUMERIC_VALUE,
    rowSpan: HAS_NUMERIC_VALUE,
    sandbox: 0,
    scope: 0,
    scoped: HAS_BOOLEAN_VALUE,
    scrolling: 0,
    seamless: HAS_BOOLEAN_VALUE,
    selected: MUST_USE_PROPERTY | HAS_BOOLEAN_VALUE,
    shape: 0,
    size: HAS_POSITIVE_NUMERIC_VALUE,
    sizes: 0,
    span: HAS_POSITIVE_NUMERIC_VALUE,
    spellCheck: 0,
    src: 0,
    srcDoc: 0,
    srcLang: 0,
    srcSet: 0,
    start: HAS_NUMERIC_VALUE,
    step: 0,
    style: 0,
    summary: 0,
    tabIndex: 0,
    target: 0,
    title: 0,
    // Setting .type throws on non-<input> tags
    type: 0,
    useMap: 0,
    value: 0,
    width: 0,
    wmode: 0,
    wrap: 0,

    /**
     * RDFa Properties
     */
    about: 0,
    datatype: 0,
    inlist: 0,
    prefix: 0,
    // property is also supported for OpenGraph in meta tags.
    property: 0,
    resource: 0,
    'typeof': 0,
    vocab: 0,

    /**
     * Non-standard Properties
     */
    // autoCapitalize and autoCorrect are supported in Mobile Safari for
    // keyboard hints.
    autoCapitalize: 0,
    autoCorrect: 0,
    // autoSave allows WebKit/Blink to persist values of input fields on page reloads
    autoSave: 0,
    // color is for Safari mask-icon link
    color: 0,
    // itemProp, itemScope, itemType are for
    // Microdata support. See http://schema.org/docs/gs.html
    itemProp: 0,
    itemScope: HAS_BOOLEAN_VALUE,
    itemType: 0,
    // itemID and itemRef are for Microdata support as well but
    // only specified in the WHATWG spec document. See
    // https://html.spec.whatwg.org/multipage/microdata.html#microdata-dom-api
    itemID: 0,
    itemRef: 0,
    // results show looking glass icon and recent searches on input
    // search fields in WebKit/Blink
    results: 0,
    // IE-only attribute that specifies security restrictions on an iframe
    // as an alternative to the sandbox attribute on IE<10
    security: 0,
    // IE-only attribute that controls focus behavior
    unselectable: 0
  },
  DOMAttributeNames: {
    acceptCharset: 'accept-charset',
    className: 'class',
    htmlFor: 'for',
    httpEquiv: 'http-equiv'
  },
  DOMPropertyNames: {},
  DOMMutationMethods: {
    value: function (node, value) {
      if (value == null) {
        return node.removeAttribute('value');
      }

      // Number inputs get special treatment due to some edge cases in
      // Chrome. Let everything else assign the value attribute as normal.
      // https://github.com/facebook/react/issues/7253#issuecomment-236074326
      if (node.type !== 'number' || node.hasAttribute('value') === false) {
        node.setAttribute('value', '' + value);
      } else if (node.validity && !node.validity.badInput && node.ownerDocument.activeElement !== node) {
        // Don't assign an attribute if validation reports bad
        // input. Chrome will clear the value. Additionally, don't
        // operate on inputs that have focus, otherwise Chrome might
        // strip off trailing decimal places and cause the user's
        // cursor position to jump to the beginning of the input.
        //
        // In ReactDOMInput, we have an onBlur event that will trigger
        // this function again when focus is lost.
        node.setAttribute('value', '' + value);
      }
    }
  }
};

module.exports = HTMLDOMPropertyConfig;

/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */



var NS = {
  xlink: 'http://www.w3.org/1999/xlink',
  xml: 'http://www.w3.org/XML/1998/namespace'
};

// We use attributes for everything SVG so let's avoid some duplication and run
// code instead.
// The following are all specified in the HTML config already so we exclude here.
// - class (as className)
// - color
// - height
// - id
// - lang
// - max
// - media
// - method
// - min
// - name
// - style
// - target
// - type
// - width
var ATTRS = {
  accentHeight: 'accent-height',
  accumulate: 0,
  additive: 0,
  alignmentBaseline: 'alignment-baseline',
  allowReorder: 'allowReorder',
  alphabetic: 0,
  amplitude: 0,
  arabicForm: 'arabic-form',
  ascent: 0,
  attributeName: 'attributeName',
  attributeType: 'attributeType',
  autoReverse: 'autoReverse',
  azimuth: 0,
  baseFrequency: 'baseFrequency',
  baseProfile: 'baseProfile',
  baselineShift: 'baseline-shift',
  bbox: 0,
  begin: 0,
  bias: 0,
  by: 0,
  calcMode: 'calcMode',
  capHeight: 'cap-height',
  clip: 0,
  clipPath: 'clip-path',
  clipRule: 'clip-rule',
  clipPathUnits: 'clipPathUnits',
  colorInterpolation: 'color-interpolation',
  colorInterpolationFilters: 'color-interpolation-filters',
  colorProfile: 'color-profile',
  colorRendering: 'color-rendering',
  contentScriptType: 'contentScriptType',
  contentStyleType: 'contentStyleType',
  cursor: 0,
  cx: 0,
  cy: 0,
  d: 0,
  decelerate: 0,
  descent: 0,
  diffuseConstant: 'diffuseConstant',
  direction: 0,
  display: 0,
  divisor: 0,
  dominantBaseline: 'dominant-baseline',
  dur: 0,
  dx: 0,
  dy: 0,
  edgeMode: 'edgeMode',
  elevation: 0,
  enableBackground: 'enable-background',
  end: 0,
  exponent: 0,
  externalResourcesRequired: 'externalResourcesRequired',
  fill: 0,
  fillOpacity: 'fill-opacity',
  fillRule: 'fill-rule',
  filter: 0,
  filterRes: 'filterRes',
  filterUnits: 'filterUnits',
  floodColor: 'flood-color',
  floodOpacity: 'flood-opacity',
  focusable: 0,
  fontFamily: 'font-family',
  fontSize: 'font-size',
  fontSizeAdjust: 'font-size-adjust',
  fontStretch: 'font-stretch',
  fontStyle: 'font-style',
  fontVariant: 'font-variant',
  fontWeight: 'font-weight',
  format: 0,
  from: 0,
  fx: 0,
  fy: 0,
  g1: 0,
  g2: 0,
  glyphName: 'glyph-name',
  glyphOrientationHorizontal: 'glyph-orientation-horizontal',
  glyphOrientationVertical: 'glyph-orientation-vertical',
  glyphRef: 'glyphRef',
  gradientTransform: 'gradientTransform',
  gradientUnits: 'gradientUnits',
  hanging: 0,
  horizAdvX: 'horiz-adv-x',
  horizOriginX: 'horiz-origin-x',
  ideographic: 0,
  imageRendering: 'image-rendering',
  'in': 0,
  in2: 0,
  intercept: 0,
  k: 0,
  k1: 0,
  k2: 0,
  k3: 0,
  k4: 0,
  kernelMatrix: 'kernelMatrix',
  kernelUnitLength: 'kernelUnitLength',
  kerning: 0,
  keyPoints: 'keyPoints',
  keySplines: 'keySplines',
  keyTimes: 'keyTimes',
  lengthAdjust: 'lengthAdjust',
  letterSpacing: 'letter-spacing',
  lightingColor: 'lighting-color',
  limitingConeAngle: 'limitingConeAngle',
  local: 0,
  markerEnd: 'marker-end',
  markerMid: 'marker-mid',
  markerStart: 'marker-start',
  markerHeight: 'markerHeight',
  markerUnits: 'markerUnits',
  markerWidth: 'markerWidth',
  mask: 0,
  maskContentUnits: 'maskContentUnits',
  maskUnits: 'maskUnits',
  mathematical: 0,
  mode: 0,
  numOctaves: 'numOctaves',
  offset: 0,
  opacity: 0,
  operator: 0,
  order: 0,
  orient: 0,
  orientation: 0,
  origin: 0,
  overflow: 0,
  overlinePosition: 'overline-position',
  overlineThickness: 'overline-thickness',
  paintOrder: 'paint-order',
  panose1: 'panose-1',
  pathLength: 'pathLength',
  patternContentUnits: 'patternContentUnits',
  patternTransform: 'patternTransform',
  patternUnits: 'patternUnits',
  pointerEvents: 'pointer-events',
  points: 0,
  pointsAtX: 'pointsAtX',
  pointsAtY: 'pointsAtY',
  pointsAtZ: 'pointsAtZ',
  preserveAlpha: 'preserveAlpha',
  preserveAspectRatio: 'preserveAspectRatio',
  primitiveUnits: 'primitiveUnits',
  r: 0,
  radius: 0,
  refX: 'refX',
  refY: 'refY',
  renderingIntent: 'rendering-intent',
  repeatCount: 'repeatCount',
  repeatDur: 'repeatDur',
  requiredExtensions: 'requiredExtensions',
  requiredFeatures: 'requiredFeatures',
  restart: 0,
  result: 0,
  rotate: 0,
  rx: 0,
  ry: 0,
  scale: 0,
  seed: 0,
  shapeRendering: 'shape-rendering',
  slope: 0,
  spacing: 0,
  specularConstant: 'specularConstant',
  specularExponent: 'specularExponent',
  speed: 0,
  spreadMethod: 'spreadMethod',
  startOffset: 'startOffset',
  stdDeviation: 'stdDeviation',
  stemh: 0,
  stemv: 0,
  stitchTiles: 'stitchTiles',
  stopColor: 'stop-color',
  stopOpacity: 'stop-opacity',
  strikethroughPosition: 'strikethrough-position',
  strikethroughThickness: 'strikethrough-thickness',
  string: 0,
  stroke: 0,
  strokeDasharray: 'stroke-dasharray',
  strokeDashoffset: 'stroke-dashoffset',
  strokeLinecap: 'stroke-linecap',
  strokeLinejoin: 'stroke-linejoin',
  strokeMiterlimit: 'stroke-miterlimit',
  strokeOpacity: 'stroke-opacity',
  strokeWidth: 'stroke-width',
  surfaceScale: 'surfaceScale',
  systemLanguage: 'systemLanguage',
  tableValues: 'tableValues',
  targetX: 'targetX',
  targetY: 'targetY',
  textAnchor: 'text-anchor',
  textDecoration: 'text-decoration',
  textRendering: 'text-rendering',
  textLength: 'textLength',
  to: 0,
  transform: 0,
  u1: 0,
  u2: 0,
  underlinePosition: 'underline-position',
  underlineThickness: 'underline-thickness',
  unicode: 0,
  unicodeBidi: 'unicode-bidi',
  unicodeRange: 'unicode-range',
  unitsPerEm: 'units-per-em',
  vAlphabetic: 'v-alphabetic',
  vHanging: 'v-hanging',
  vIdeographic: 'v-ideographic',
  vMathematical: 'v-mathematical',
  values: 0,
  vectorEffect: 'vector-effect',
  version: 0,
  vertAdvY: 'vert-adv-y',
  vertOriginX: 'vert-origin-x',
  vertOriginY: 'vert-origin-y',
  viewBox: 'viewBox',
  viewTarget: 'viewTarget',
  visibility: 0,
  widths: 0,
  wordSpacing: 'word-spacing',
  writingMode: 'writing-mode',
  x: 0,
  xHeight: 'x-height',
  x1: 0,
  x2: 0,
  xChannelSelector: 'xChannelSelector',
  xlinkActuate: 'xlink:actuate',
  xlinkArcrole: 'xlink:arcrole',
  xlinkHref: 'xlink:href',
  xlinkRole: 'xlink:role',
  xlinkShow: 'xlink:show',
  xlinkTitle: 'xlink:title',
  xlinkType: 'xlink:type',
  xmlBase: 'xml:base',
  xmlns: 0,
  xmlnsXlink: 'xmlns:xlink',
  xmlLang: 'xml:lang',
  xmlSpace: 'xml:space',
  y: 0,
  y1: 0,
  y2: 0,
  yChannelSelector: 'yChannelSelector',
  z: 0,
  zoomAndPan: 'zoomAndPan'
};

var SVGDOMPropertyConfig = {
  Properties: {},
  DOMAttributeNamespaces: {
    xlinkActuate: NS.xlink,
    xlinkArcrole: NS.xlink,
    xlinkHref: NS.xlink,
    xlinkRole: NS.xlink,
    xlinkShow: NS.xlink,
    xlinkTitle: NS.xlink,
    xlinkType: NS.xlink,
    xmlBase: NS.xml,
    xmlLang: NS.xml,
    xmlSpace: NS.xml
  },
  DOMAttributeNames: {}
};

Object.keys(ATTRS).forEach(function (key) {
  SVGDOMPropertyConfig.Properties[key] = 0;
  if (ATTRS[key]) {
    SVGDOMPropertyConfig.DOMAttributeNames[key] = ATTRS[key];
  }
});

module.exports = SVGDOMPropertyConfig;

/***/ }),
/* 29 */
/***/ (function(module, exports, __webpack_require__) {

var parse = __webpack_require__(30);

/**
 * Parses inline style.
 *
 * Example: 'color:red' => { color: 'red' }
 *
 * @param  {String}      style      - The inline style.
 * @param  {Function}    [iterator] - The iterator function.
 * @return {null|Object}
 */
module.exports = function parseInlineStyle(style, iterator) {
  if (!style || typeof style !== 'string') return null;

  // make sure to wrap declarations in placeholder
  var declarations = parse('p{' + style + '}').stylesheet.rules[0].declarations;
  var declaration, property, value;

  var output = null;
  var hasIterator = typeof iterator === 'function';

  for (var i = 0, len = declarations.length; i < len; i++) {
    declaration = declarations[i];
    property = declaration.property;
    value = declaration.value;

    if (hasIterator) {
      iterator(property, value, declaration);
    } else if (value) {
      output || (output = {});
      output[property] = value;
    }
  }

  return output;
};


/***/ }),
/* 30 */
/***/ (function(module, exports) {

// http://www.w3.org/TR/CSS21/grammar.html
// https://github.com/visionmedia/css-parse/pull/49#issuecomment-30088027
var commentre = /\/\*[^*]*\*+([^/*][^*]*\*+)*\//g

module.exports = function(css, options){
  options = options || {};

  /**
   * Positional.
   */

  var lineno = 1;
  var column = 1;

  /**
   * Update lineno and column based on `str`.
   */

  function updatePosition(str) {
    var lines = str.match(/\n/g);
    if (lines) lineno += lines.length;
    var i = str.lastIndexOf('\n');
    column = ~i ? str.length - i : column + str.length;
  }

  /**
   * Mark position and patch `node.position`.
   */

  function position() {
    var start = { line: lineno, column: column };
    return function(node){
      node.position = new Position(start);
      whitespace();
      return node;
    };
  }

  /**
   * Store position information for a node
   */

  function Position(start) {
    this.start = start;
    this.end = { line: lineno, column: column };
    this.source = options.source;
  }

  /**
   * Non-enumerable source string
   */

  Position.prototype.content = css;

  /**
   * Error `msg`.
   */

  var errorsList = [];

  function error(msg) {
    var err = new Error(options.source + ':' + lineno + ':' + column + ': ' + msg);
    err.reason = msg;
    err.filename = options.source;
    err.line = lineno;
    err.column = column;
    err.source = css;

    if (options.silent) {
      errorsList.push(err);
    } else {
      throw err;
    }
  }

  /**
   * Parse stylesheet.
   */

  function stylesheet() {
    var rulesList = rules();

    return {
      type: 'stylesheet',
      stylesheet: {
        source: options.source,
        rules: rulesList,
        parsingErrors: errorsList
      }
    };
  }

  /**
   * Opening brace.
   */

  function open() {
    return match(/^{\s*/);
  }

  /**
   * Closing brace.
   */

  function close() {
    return match(/^}/);
  }

  /**
   * Parse ruleset.
   */

  function rules() {
    var node;
    var rules = [];
    whitespace();
    comments(rules);
    while (css.length && css.charAt(0) != '}' && (node = atrule() || rule())) {
      if (node !== false) {
        rules.push(node);
        comments(rules);
      }
    }
    return rules;
  }

  /**
   * Match `re` and return captures.
   */

  function match(re) {
    var m = re.exec(css);
    if (!m) return;
    var str = m[0];
    updatePosition(str);
    css = css.slice(str.length);
    return m;
  }

  /**
   * Parse whitespace.
   */

  function whitespace() {
    match(/^\s*/);
  }

  /**
   * Parse comments;
   */

  function comments(rules) {
    var c;
    rules = rules || [];
    while (c = comment()) {
      if (c !== false) {
        rules.push(c);
      }
    }
    return rules;
  }

  /**
   * Parse comment.
   */

  function comment() {
    var pos = position();
    if ('/' != css.charAt(0) || '*' != css.charAt(1)) return;

    var i = 2;
    while ("" != css.charAt(i) && ('*' != css.charAt(i) || '/' != css.charAt(i + 1))) ++i;
    i += 2;

    if ("" === css.charAt(i-1)) {
      return error('End of comment missing');
    }

    var str = css.slice(2, i - 2);
    column += 2;
    updatePosition(str);
    css = css.slice(i);
    column += 2;

    return pos({
      type: 'comment',
      comment: str
    });
  }

  /**
   * Parse selector.
   */

  function selector() {
    var m = match(/^([^{]+)/);
    if (!m) return;
    /* @fix Remove all comments from selectors
     * http://ostermiller.org/findcomment.html */
    return trim(m[0])
      .replace(/\/\*([^*]|[\r\n]|(\*+([^*/]|[\r\n])))*\*\/+/g, '')
      .replace(/"(?:\\"|[^"])*"|'(?:\\'|[^'])*'/g, function(m) {
        return m.replace(/,/g, '\u200C');
      })
      .split(/\s*(?![^(]*\)),\s*/)
      .map(function(s) {
        return s.replace(/\u200C/g, ',');
      });
  }

  /**
   * Parse declaration.
   */

  function declaration() {
    var pos = position();

    // prop
    var prop = match(/^(\*?[-#\/\*\\\w]+(\[[0-9a-z_-]+\])?)\s*/);
    if (!prop) return;
    prop = trim(prop[0]);

    // :
    if (!match(/^:\s*/)) return error("property missing ':'");

    // val
    var val = match(/^((?:'(?:\\'|.)*?'|"(?:\\"|.)*?"|\([^\)]*?\)|[^};])+)/);

    var ret = pos({
      type: 'declaration',
      property: prop.replace(commentre, ''),
      value: val ? trim(val[0]).replace(commentre, '') : ''
    });

    // ;
    match(/^[;\s]*/);

    return ret;
  }

  /**
   * Parse declarations.
   */

  function declarations() {
    var decls = [];

    if (!open()) return error("missing '{'");
    comments(decls);

    // declarations
    var decl;
    while (decl = declaration()) {
      if (decl !== false) {
        decls.push(decl);
        comments(decls);
      }
    }

    if (!close()) return error("missing '}'");
    return decls;
  }

  /**
   * Parse keyframe.
   */

  function keyframe() {
    var m;
    var vals = [];
    var pos = position();

    while (m = match(/^((\d+\.\d+|\.\d+|\d+)%?|[a-z]+)\s*/)) {
      vals.push(m[1]);
      match(/^,\s*/);
    }

    if (!vals.length) return;

    return pos({
      type: 'keyframe',
      values: vals,
      declarations: declarations()
    });
  }

  /**
   * Parse keyframes.
   */

  function atkeyframes() {
    var pos = position();
    var m = match(/^@([-\w]+)?keyframes\s*/);

    if (!m) return;
    var vendor = m[1];

    // identifier
    var m = match(/^([-\w]+)\s*/);
    if (!m) return error("@keyframes missing name");
    var name = m[1];

    if (!open()) return error("@keyframes missing '{'");

    var frame;
    var frames = comments();
    while (frame = keyframe()) {
      frames.push(frame);
      frames = frames.concat(comments());
    }

    if (!close()) return error("@keyframes missing '}'");

    return pos({
      type: 'keyframes',
      name: name,
      vendor: vendor,
      keyframes: frames
    });
  }

  /**
   * Parse supports.
   */

  function atsupports() {
    var pos = position();
    var m = match(/^@supports *([^{]+)/);

    if (!m) return;
    var supports = trim(m[1]);

    if (!open()) return error("@supports missing '{'");

    var style = comments().concat(rules());

    if (!close()) return error("@supports missing '}'");

    return pos({
      type: 'supports',
      supports: supports,
      rules: style
    });
  }

  /**
   * Parse host.
   */

  function athost() {
    var pos = position();
    var m = match(/^@host\s*/);

    if (!m) return;

    if (!open()) return error("@host missing '{'");

    var style = comments().concat(rules());

    if (!close()) return error("@host missing '}'");

    return pos({
      type: 'host',
      rules: style
    });
  }

  /**
   * Parse media.
   */

  function atmedia() {
    var pos = position();
    var m = match(/^@media *([^{]+)/);

    if (!m) return;
    var media = trim(m[1]);

    if (!open()) return error("@media missing '{'");

    var style = comments().concat(rules());

    if (!close()) return error("@media missing '}'");

    return pos({
      type: 'media',
      media: media,
      rules: style
    });
  }


  /**
   * Parse custom-media.
   */

  function atcustommedia() {
    var pos = position();
    var m = match(/^@custom-media\s+(--[^\s]+)\s*([^{;]+);/);
    if (!m) return;

    return pos({
      type: 'custom-media',
      name: trim(m[1]),
      media: trim(m[2])
    });
  }

  /**
   * Parse paged media.
   */

  function atpage() {
    var pos = position();
    var m = match(/^@page */);
    if (!m) return;

    var sel = selector() || [];

    if (!open()) return error("@page missing '{'");
    var decls = comments();

    // declarations
    var decl;
    while (decl = declaration()) {
      decls.push(decl);
      decls = decls.concat(comments());
    }

    if (!close()) return error("@page missing '}'");

    return pos({
      type: 'page',
      selectors: sel,
      declarations: decls
    });
  }

  /**
   * Parse document.
   */

  function atdocument() {
    var pos = position();
    var m = match(/^@([-\w]+)?document *([^{]+)/);
    if (!m) return;

    var vendor = trim(m[1]);
    var doc = trim(m[2]);

    if (!open()) return error("@document missing '{'");

    var style = comments().concat(rules());

    if (!close()) return error("@document missing '}'");

    return pos({
      type: 'document',
      document: doc,
      vendor: vendor,
      rules: style
    });
  }

  /**
   * Parse font-face.
   */

  function atfontface() {
    var pos = position();
    var m = match(/^@font-face\s*/);
    if (!m) return;

    if (!open()) return error("@font-face missing '{'");
    var decls = comments();

    // declarations
    var decl;
    while (decl = declaration()) {
      decls.push(decl);
      decls = decls.concat(comments());
    }

    if (!close()) return error("@font-face missing '}'");

    return pos({
      type: 'font-face',
      declarations: decls
    });
  }

  /**
   * Parse import
   */

  var atimport = _compileAtrule('import');

  /**
   * Parse charset
   */

  var atcharset = _compileAtrule('charset');

  /**
   * Parse namespace
   */

  var atnamespace = _compileAtrule('namespace');

  /**
   * Parse non-block at-rules
   */


  function _compileAtrule(name) {
    var re = new RegExp('^@' + name + '\\s*([^;]+);');
    return function() {
      var pos = position();
      var m = match(re);
      if (!m) return;
      var ret = { type: name };
      ret[name] = m[1].trim();
      return pos(ret);
    }
  }

  /**
   * Parse at rule.
   */

  function atrule() {
    if (css[0] != '@') return;

    return atkeyframes()
      || atmedia()
      || atcustommedia()
      || atsupports()
      || atimport()
      || atcharset()
      || atnamespace()
      || atdocument()
      || atpage()
      || athost()
      || atfontface();
  }

  /**
   * Parse rule.
   */

  function rule() {
    var pos = position();
    var sel = selector();

    if (!sel) return error('selector missing');
    comments();

    return pos({
      type: 'rule',
      selectors: sel,
      declarations: declarations()
    });
  }

  return addParent(stylesheet());
};

/**
 * Trim `str`.
 */

function trim(str) {
  return str ? str.replace(/^\s+|\s+$/g, '') : '';
}

/**
 * Adds non-enumerable parent node reference to each node.
 */

function addParent(obj, parent) {
  var isNode = obj && typeof obj.type === 'string';
  var childParent = isNode ? obj : parent;

  for (var k in obj) {
    var value = obj[k];
    if (Array.isArray(value)) {
      value.forEach(function(v) { addParent(v, childParent); });
    } else if (value && typeof value === 'object') {
      addParent(value, childParent);
    }
  }

  if (isNode) {
    Object.defineProperty(obj, 'parent', {
      configurable: true,
      writable: true,
      enumerable: false,
      value: parent || null
    });
  }

  return obj;
}


/***/ }),
/* 31 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


/**
 * Module dependencies.
 */
var domparser = __webpack_require__(32);
var utilities = __webpack_require__(9);
var formatDOM = utilities.formatDOM;
var isIE9 = utilities.isIE(9);

/**
 * Constants.
 */
var DIRECTIVE_REGEX = /<(![a-zA-Z\s]+)>/; // e.g., <!doctype html>

/**
 * Parses HTML and reformats DOM nodes output.
 *
 * @param  {String} html - The HTML string.
 * @return {Array}       - The formatted DOM nodes.
 */
module.exports = function parseDOM(html) {
    if (typeof html !== 'string') {
        throw new TypeError('First argument must be a string.');
    }
    if (!html) return [];

    // match directive
    var match = html.match(DIRECTIVE_REGEX);
    var directive;
    if (match && match[1]) {
        directive = match[1];

        // remove directive in IE9 because DOMParser uses
        // MIME type 'text/xml' instead of 'text/html'
        if (isIE9) {
            html = html.replace(match[0], '');
        }
    }

    return formatDOM(domparser(html), null, directive);
};


/***/ }),
/* 32 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


/**
 * Module dependencies.
 */
var utilities = __webpack_require__(9);
var detectIE = utilities.isIE;

/**
 * Constants.
 */
var HTML_TAG_NAME = 'html';
var BODY_TAG_NAME = 'body';
var HEAD_TAG_NAME = 'head';
var FIRST_TAG_REGEX = /<([a-zA-Z]+[0-9]?)/; // e.g., <h1>
var HEAD_REGEX = /<\/head>/i;
var BODY_REGEX = /<\/body>/i;
// http://www.w3.org/TR/html/syntax.html#void-elements
var VOID_ELEMENTS_REGEX = /<(area|base|br|col|embed|hr|img|input|keygen|link|menuitem|meta|param|source|track|wbr)(.*?)\/?>/gi;

// browser support
var isIE = detectIE();
var isIE9 = detectIE(9);

/**
 * DOMParser (performance: slow).
 *
 * https://developer.mozilla.org/docs/Web/API/DOMParser#Parsing_an_SVG_or_HTML_document
 */
var parseFromString;
if (typeof window.DOMParser === 'function') {
    var domParser = new window.DOMParser();
    // IE9 does not support 'text/html' MIME type
    // https://msdn.microsoft.com/en-us/library/ff975278(v=vs.85).aspx
    var MIME_TYPE = isIE9 ? 'text/xml' : 'text/html';

    /**
     * Creates an HTML document using `DOMParser.parseFromString`.
     *
     * @param  {String} html      - The HTML string.
     * @param  {String} [tagName] - The element to render the HTML (with 'body' as fallback).
     * @return {HTMLDocument}
     */
    parseFromString = function domStringParser(html, tagName) {
        if (tagName) {
            html = ['<', tagName, '>', html, '</', tagName, '>'].join('');
        }
        // because IE9 only supports MIME type 'text/xml', void elements need to be self-closed
        if (isIE9) {
            html = html.replace(VOID_ELEMENTS_REGEX, '<$1$2$3/>');
        }
        return domParser.parseFromString(html, MIME_TYPE);
    };
}

/**
 * DOMImplementation (performance: fair).
 *
 * https://developer.mozilla.org/docs/Web/API/DOMImplementation/createHTMLDocument
 */
var parseFromDocument;
if (typeof document.implementation === 'object') {
    // title parameter is required in IE
    // https://msdn.microsoft.com/en-us/library/ff975457(v=vs.85).aspx
    var doc = document.implementation.createHTMLDocument(isIE ? 'HTML_DOM_PARSER_TITLE' : undefined);

    /**
     * Use HTML document created by `document.implementation.createHTMLDocument`.
     *
     * @param  {String} html      - The HTML string.
     * @param  {String} [tagName] - The element to render the HTML (with 'body' as fallback).
     * @return {HTMLDocument}
     */
    parseFromDocument = function createHTMLDocument(html, tagName) {
        if (tagName) {
            doc.documentElement.getElementsByTagName(tagName)[0].innerHTML = html;
            return doc;
        }

        try {
            doc.documentElement.innerHTML = html;
            return doc;
        // fallback when certain elements in `documentElement` are read-only (IE9)
        } catch (err) {
            if (parseFromString) return parseFromString(html);
        }
    };
}

/**
 * Template (performance: fast).
 *
 * https://developer.mozilla.org/docs/Web/HTML/Element/template
 */
var parseFromTemplate;
var template = document.createElement('template');
if (template.content) {

    /**
     * Uses a template element (content fragment) to parse HTML.
     *
     * @param  {String} html - The HTML string.
     * @return {NodeList}
     */
    parseFromTemplate = function templateParser(html) {
        template.innerHTML = html;
        return template.content.childNodes;
    };
}

/** Fallback document parser. */
var parseWithFallback = parseFromDocument || parseFromString;

/**
 * Parses HTML string to DOM nodes.
 *
 * @param  {String} html      - The HTML string.
 * @param  {String} [tagName] - The tag name.
 * @return {NodeList|Array}
 */
module.exports = function domparser(html) {
    // try to match first tag
    var tagName;
    var match = html.match(FIRST_TAG_REGEX);
    if (match && match[1]) {
        tagName = match[1].toLowerCase();
    }

    var doc;
    var element;
    var elements;

    switch (tagName) {
        case HTML_TAG_NAME:
            if (parseFromString) {
                doc = parseFromString(html);

                // the created document may come with filler head/body elements,
                // so ake sure to remove them if they don't actually exist
                if (!HEAD_REGEX.test(html)) {
                    element = doc.getElementsByTagName(HEAD_TAG_NAME)[0];
                    if (element) element.parentNode.removeChild(element);
                }
                if (!BODY_REGEX.test(html)) {
                    element = doc.getElementsByTagName(BODY_TAG_NAME)[0];
                    if (element) element.parentNode.removeChild(element);
                }

                return doc.getElementsByTagName(HTML_TAG_NAME);
            }
            break;

        case HEAD_TAG_NAME:
            if (parseWithFallback) {
                elements = parseWithFallback(html).getElementsByTagName(HEAD_TAG_NAME);

                // account for possibility of sibling
                if (BODY_REGEX.test(html)) {
                    return elements[0].parentNode.childNodes;
                }
                return elements;
            }
            break;

        case BODY_TAG_NAME:
            if (parseWithFallback) {
                elements = parseWithFallback(html).getElementsByTagName(BODY_TAG_NAME);

                // account for possibility of sibling (return both body and head)
                if (HEAD_REGEX.test(html)) {
                    return elements[0].parentNode.childNodes;
                }
                return elements;
            }
            break;

        // low-level tag or text
        default:
            if (parseFromTemplate) return parseFromTemplate(html);
            if (parseWithFallback) {
                return parseWithFallback(html, BODY_TAG_NAME).getElementsByTagName(BODY_TAG_NAME)[0].childNodes;
            }
            break;
    }

    return [];
};


/***/ }),
/* 33 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
var prompt_namespaceObject = {};
__webpack_require__.r(prompt_namespaceObject);
__webpack_require__.d(prompt_namespaceObject, "getCommandList", function() { return getCommandList; });
__webpack_require__.d(prompt_namespaceObject, "runCommand", function() { return runCommand; });
__webpack_require__.d(prompt_namespaceObject, "runHelpCommand", function() { return runHelpCommand; });
__webpack_require__.d(prompt_namespaceObject, "runLocalCommand", function() { return runLocalCommand; });
__webpack_require__.d(prompt_namespaceObject, "endPaging", function() { return endPaging; });
__webpack_require__.d(prompt_namespaceObject, "changeUserMode", function() { return prompt_changeUserMode; });

// EXTERNAL MODULE: external "window.dnn.nodeModules.React"
var external_window_dnn_nodeModules_React_ = __webpack_require__(0);
var external_window_dnn_nodeModules_React_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_React_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactDOM"
var external_window_dnn_nodeModules_ReactDOM_ = __webpack_require__(10);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactRedux"
var external_window_dnn_nodeModules_ReactRedux_ = __webpack_require__(4);

// CONCATENATED MODULE: ./src/utils/helpers.js

function formatString() {
  var format = arguments[0];
  var methodsArgs = arguments;
  return format.replace(/[{[](\d+)[\]}]/gi, function (value, index) {
    var argsIndex = parseInt(index) + 1;
    return methodsArgs[argsIndex];
  });
}
function sort(items, column, order) {
  order = order === undefined ? "asc" : order;
  items = items.sort(function (a, b) {
    if (a[column] > b[column]) //sort string descending
      return order === "asc" ? 1 : -1;
    if (a[column] < b[column]) return order === "asc" ? -1 : 1;
    return 0; //default return value (no sorting)
  });
  return items;
}
function formatLabel(input) {
  if (typeof input === "string") {
    // format camelcase and remove Is from labels
    var output = input.replace("$", "").replace(/^(Is)(.+)/i, "$2");
    output = output.match(/[A-Z][a-z]+/g).join(" "); // rudimentary but should handle normal Camelcase

    return output;
  }

  return "";
}
function getColumnsFromRow(row) {
  var columns = [];

  for (var key in row) {
    if (key.indexOf("__") !== 0) {
      columns.push(key);
    }
  }

  return columns;
}
function renderObject(data, fieldOrder) {
  var columns = !fieldOrder || fieldOrder.length === 0 ? getColumnsFromRow(data) : fieldOrder;
  var rows = columns.map(function (fldName, index) {
    var lbl = formatLabel(fldName); // explicitly checking for null and undefined to cover case where { data["isDeleted"] : false }

    var fldVal = data[fldName] !== undefined && data[fldName] !== null ? data[fldName].toString() : "";
    var cmd = data["__" + fldName] ? data["__" + fldName] : null;

    if (cmd) {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("tr", {
          key: index
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("td", {
          className: "dnn-prompt-lbl"
        }, lbl),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("td", null, ":"),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("td", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("a", {
          href: "#",
          className: "dnn-prompt-cmd-insert",
          "data-cmd": cmd,
          title: cmd.replace(/'/g, "&quot;")
        }, fldVal)))
      );
    } else {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("tr", {
          key: index
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("td", {
          className: "dnn-prompt-lbl"
        }, lbl),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("td", null, ":"),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("td", null, fldVal))
      );
    }
  });
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("table", {
      className: "dnn-prompt-tbl"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("tbody", null, rows))
  );
}
function stripWhiteSpaces() {
  var html = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
  var re = />[^\w^<]+</igm;
  return html.replace(re, "><");
}
var util = {
  init: function init(utilities) {
    if (!utilities) {
      throw new Error("Utilities is undefined.");
    }

    this.utilities = utilities;
  },
  utilities: null
};
// CONCATENATED MODULE: ./src/globals/promptInit.js

var promptInit = {
  init: function init() {
    // This setting is required and define the public path 
    // to allow the web application to download assets on demand 
    // eslint-disable-next-line no-undef
    // __webpack_public_path__ = options.publicPath;
    var options = window.dnn.initPrompt();
    util.init(options.utility);
    util.moduleName = options.moduleName;
    util.settings = options.settings; // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production
    //require("../less/style.less");
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
/* harmony default export */ var globals_promptInit = (promptInit);
/* eslint-disable */

var IS_DEV = "production" !== "production";
// EXTERNAL MODULE: external "window.dnn.nodeModules.Redux"
var external_window_dnn_nodeModules_Redux_ = __webpack_require__(3);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxThunk"
var external_window_dnn_nodeModules_ReduxThunk_ = __webpack_require__(7);
var external_window_dnn_nodeModules_ReduxThunk_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxThunk_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxImmutableStateInvariant"
var external_window_dnn_nodeModules_ReduxImmutableStateInvariant_ = __webpack_require__(11);
var external_window_dnn_nodeModules_ReduxImmutableStateInvariant_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxImmutableStateInvariant_);

// CONCATENATED MODULE: ./src/constants/actionTypes.js
var actionTypes = {
  RETRIEVED_COMMAND_LIST: "RETRIEVED_COMMAND_LIST",
  EXECUTED_COMMAND: "EXECUTED_COMMAND",
  EXECUTED_HELP_COMMAND: "EXECUTED_HELP_COMMAND",
  EXECUTED_LOCAL_COMMAND: "EXECUTED_LOCAL_COMMAND",
  END_PAGING: "END_PAGING",
  CHANGE_USER_MODE: "CHANGE_USER_MODE"
};
/* harmony default export */ var constants_actionTypes = (actionTypes);
// CONCATENATED MODULE: ./src/reducers/promptReducers.js
function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



function promptReducers() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    commandList: null,
    pagingInfo: null,
    nextPageCommand: null,
    reload: false,
    fieldOrder: null,
    output: null,
    isHtml: false,
    isError: false,
    isBusy: false,
    data: null,
    clearOutput: false,
    style: null,
    isHelp: false,
    name: null,
    description: null,
    options: null,
    resultHtml: null,
    error: null
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case constants_actionTypes.RETRIEVED_COMMAND_LIST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        commandList: action.data.commands,
        pagingInfo: null,
        nextPageCommand: null,
        reload: false,
        fieldOrder: null,
        output: null,
        isHtml: false,
        isError: false,
        data: null,
        clearOutput: false,
        style: null,
        isHelp: true,
        name: null,
        description: null,
        options: null,
        resultHtml: null,
        error: null,
        isBusy: false
      });

    case constants_actionTypes.END_PAGING:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        commandList: null,
        pagingInfo: null,
        nextPageCommand: null,
        reload: false,
        fieldOrder: null,
        output: null,
        isHtml: false,
        isError: false,
        data: null,
        clearOutput: false,
        style: null,
        isHelp: false,
        name: null,
        description: null,
        options: null,
        resultHtml: null,
        error: null,
        isBusy: false
      });

    case constants_actionTypes.EXECUTED_COMMAND:
      if (action.data.result.Message) {
        return _objectSpread2(_objectSpread2({}, state), {}, {
          isHelp: false,
          isError: true,
          output: action.data.result.Message,
          style: action.style,
          isBusy: false
        });
      } else {
        return _objectSpread2(_objectSpread2({}, state), {}, {
          isHelp: false,
          pagingInfo: action.data.result.pagingInfo,
          isHtml: action.data.result.isHtml,
          isError: action.data.result.isError,
          reload: action.data.result.mustReload,
          data: action.data.result.data,
          fieldOrder: action.data.result.fieldOrder,
          output: action.data.result.output,
          nextPageCommand: action.data.result.nextPageCommand,
          style: action.style,
          isBusy: false
        });
      }

    case constants_actionTypes.EXECUTED_HELP_COMMAND:
      if (action.data.result.Message) {
        return _objectSpread2(_objectSpread2({}, state), {}, {
          isHelp: false,
          isError: true,
          output: action.data.result.Message,
          style: action.style,
          isBusy: false
        });
      } else {
        return _objectSpread2(_objectSpread2({}, state), {}, {
          commandList: null,
          isHelp: true,
          name: action.data.result.name,
          description: action.data.result.description,
          options: action.data.result.options,
          resultHtml: action.data.result.resultHtml,
          error: action.data.result.error,
          isError: action.data.result.error !== undefined && action.data.result.error !== null && action.data.result.error !== "",
          isBusy: false
        });
      }

    case constants_actionTypes.EXECUTED_LOCAL_COMMAND:
      switch (action.command) {
        case "CLS":
        case "CLEAR-SCREEN":
          return _objectSpread2(_objectSpread2({}, state), {}, {
            isHelp: false,
            pagingInfo: null,
            output: action.output,
            clearOutput: true,
            style: action.style,
            isBusy: false
          });

        case "RELOAD":
          return _objectSpread2(_objectSpread2({}, state), {}, {
            isHelp: false,
            pagingInfo: null,
            reload: true,
            output: action.output,
            clearOutput: false,
            style: action.style,
            isBusy: false
          });

        case "ERROR":
          return _objectSpread2(_objectSpread2({}, state), {}, {
            isHelp: false,
            pagingInfo: null,
            reload: false,
            output: action.output,
            clearOutput: false,
            isError: true,
            data: null,
            isHtml: false,
            style: action.style,
            isBusy: false
          });

        case "CLH":
        case "CLEAR-HISTORY":
        case "INFO":
          return _objectSpread2(_objectSpread2({}, state), {}, {
            isHelp: false,
            pagingInfo: null,
            reload: false,
            output: action.output,
            clearOutput: false,
            isError: false,
            data: null,
            isHtml: false,
            style: action.style,
            isBusy: false
          });

        case "EXIT":
          util.utilities.closePersonaBar();
          return _objectSpread2(_objectSpread2({}, state), {}, {
            isBusy: false
          });

        default:
          return _objectSpread2(_objectSpread2({}, state), {}, {
            isBusy: false
          });
      }

    default:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        isBusy: false
      });
  }
}
// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevTools"
var external_window_dnn_nodeModules_ReduxDevTools_ = __webpack_require__(12);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsLogMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_ = __webpack_require__(13);
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsDockMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_ = __webpack_require__(14);
var external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_);

// CONCATENATED MODULE: ./src/containers/DevTools.js




/* harmony default export */ var DevTools = (Object(external_window_dnn_nodeModules_ReduxDevTools_["createDevTools"])(
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_default.a, {
  toggleVisibilityKey: "ctrl-h",
  changePositionKey: "ctrl-q"
},
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_default.a, null))));
// CONCATENATED MODULE: ./src/store/configureStore.js






function configureStore(initialState) {
  var enhancer;

  if (IS_DEV) {
    enhancer = Object(external_window_dnn_nodeModules_Redux_["compose"])(Object(external_window_dnn_nodeModules_Redux_["applyMiddleware"])(external_window_dnn_nodeModules_ReduxThunk_default.a, external_window_dnn_nodeModules_ReduxImmutableStateInvariant_default()()), DevTools.instrument());
  } else {
    enhancer = Object(external_window_dnn_nodeModules_Redux_["applyMiddleware"])(external_window_dnn_nodeModules_ReduxThunk_default.a);
  }

  return Object(external_window_dnn_nodeModules_Redux_["createStore"])(promptReducers, initialState, enhancer);
}
// EXTERNAL MODULE: D:/a/1/s/node_modules/prop-types/index.js
var prop_types = __webpack_require__(1);
var prop_types_default = /*#__PURE__*/__webpack_require__.n(prop_types);

// EXTERNAL MODULE: ./src/components/Prompt.less
var Prompt = __webpack_require__(5);

// EXTERNAL MODULE: D:/a/1/s/node_modules/html-react-parser/index.js
var html_react_parser = __webpack_require__(2);
var html_react_parser_default = /*#__PURE__*/__webpack_require__.n(html_react_parser);

// CONCATENATED MODULE: ./src/services/DomKey.js
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var instance = null;

var DomKey =
/*#__PURE__*/
function () {
  function DomKey() {
    _classCallCheck(this, DomKey);

    if (!instance) {
      instance = this;
      this.key = 0;
    }

    return instance;
  }

  _createClass(DomKey, null, [{
    key: "get",
    value: function get() {
      var prefix = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "";
      var self = instance ? instance : new DomKey();
      return "".concat(prefix, "-").concat(self.key++);
    }
  }]);

  return DomKey;
}();


// CONCATENATED MODULE: ./src/components/DataTable.jsx






var DataTable_DataTable = function DataTable(_ref) {
  var rows = _ref.rows,
      columns = _ref.columns,
      cssClass = _ref.cssClass;

  var renderTableHeader = function renderTableHeader(columns) {
    var tableCols = columns.map(function (col) {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("th", {
          key: DomKey.get("datatable")
        }, formatLabel(col))
      );
    });
    return (
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("thead", null,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("tr", {
        key: DomKey.get("datatable")
      }, tableCols))
    );
  };

  var renderTableRows = function renderTableRows(rows, columns) {
    return rows.map(function (row) {
      var rowCells = columns.map(function (fieldName) {
        var fieldValue = row[fieldName.replace("$", "")] ? row[fieldName.replace("$", "")] + "" : "";
        var cmd = row["__" + fieldName] ? row["__" + fieldName] : null;

        if (cmd) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("td", {
              key: DomKey.get("datatable")
            },
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("a", {
              href: "#",
              className: "dnn-prompt-cmd-insert",
              "data-cmd": cmd,
              title: cmd.replace(/'/g, "&quot;")
            }, html_react_parser_default()(fieldValue)))
          );
        } else if (fieldName.indexOf("$") >= 0) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("td", {
              key: DomKey.get("datatable"),
              className: "mono"
            }, "--", html_react_parser_default()(fieldValue))
          );
        } else {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("td", {
              key: DomKey.get("datatable")
            }, html_react_parser_default()(fieldValue))
          );
        }
      });
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("tr", {
          key: DomKey.get("datatable")
        }, rowCells)
      );
    });
  };

  var renderTable = function renderTable(columns) {
    if (!rows || !rows.length) {
      return;
    }

    columns = columns ? columns : getColumnsFromRow(rows[0]); // build header

    var tableHeader = renderTableHeader(columns, cssClass); // build rows

    var tableRows = renderTableRows(rows, columns);
    return (
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("table", {
        key: DomKey.get("datatable"),
        className: cssClass ? cssClass : "dnn-prompt-tbl"
      }, tableHeader,
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("tbody", null, tableRows))
    );
  };

  return renderTable(columns);
};

DataTable_DataTable.propTypes = {
  rows: prop_types_default.a.array.isRequired,
  columns: prop_types_default.a.array.isRequired,
  cssClass: prop_types_default.a.string.isRequired
};
/* harmony default export */ var components_DataTable = (DataTable_DataTable);
// CONCATENATED MODULE: ./src/localization/Localization.jsx

var Localization = {
  get: function get(key) {
    var moduleName = util.moduleName;
    return util.utilities.getResx(moduleName, key);
  }
};
/* harmony default export */ var localization_Localization = (Localization);
// CONCATENATED MODULE: ./src/components/Command.jsx
function _toConsumableArray(arr) { return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread(); }

function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance"); }

function _iterableToArray(iter) { if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter); }

function _arrayWithoutHoles(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } }




 //import { sort } from "utils/helpers";



var Command_Command = function Command(_ref) {
  var commandList = _ref.commandList,
      IsPaging = _ref.IsPaging;
  IsPaging(false);
  var headingName =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("h3", {
    className: "mono"
  }, html_react_parser_default()(localization_Localization.get("Prompt_Help_PromptCommands")));
  var paragraphDescription =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("p", {
    className: "lead"
  }, html_react_parser_default()(localization_Localization.get("Prompt_Help_ListOfAvailableMsg")));
  var headingCommands =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("h4", null, html_react_parser_default()(localization_Localization.get("Prompt_Help_Commands")));
  var commandsList = commandList.sort(function (a, b) {
    var catA = a.Category;
    var catB = b.Category;
    var kA = a.Key;
    var kB = b.Key;
    if (catA === catB && kA === kB) return 0;

    if (catA === catB) {
      return kA < kB ? -1 : 1;
    } else {
      return catA < catB ? -1 : 1;
    }
  }).reduce(function (prev, current, index, arr) {
    if (index > 0) {
      var currentCat = current.Category;
      var prevCat = arr[index - 1].Category;

      if (currentCat !== prevCat) {
        return [].concat(_toConsumableArray(prev), [{
          separator: true,
          Category: current.Category
        }, current]);
      }
    } else {
      return [{
        separator: true,
        Category: current.Category
      }, current];
    }

    return [].concat(_toConsumableArray(prev), [current]);
  }, []);
  var commandsOutput = commandsList.map(function (cmd) {
    if (cmd.separator) {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("tr", {
          key: DomKey.get("command"),
          className: "divider"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("td", {
          colSpan: "2"
        }, cmd.Category))
      );
    }

    return (
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("tr", {
        key: DomKey.get("command")
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("td", {
        key: DomKey.get("command"),
        className: "mono"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("a", {
        className: "dnn-prompt-cmd-insert",
        "data-cmd": "help ".concat(cmd.Key.toLowerCase()),
        href: "#"
      }, cmd.Key)),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("td", {
        key: DomKey.get("command")
      }, html_react_parser_default()(cmd.Description)))
    );
  });
  var divCommands =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("div", null,
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("table", {
    className: "table"
  },
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("thead", null,
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("tr", null,
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("th", null, html_react_parser_default()(localization_Localization.get("Prompt_Help_Command"))),
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("th", null, html_react_parser_default()(localization_Localization.get("Prompt_Help_Description"))))),
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("tbody", null, commandsOutput)));
  var headingSeeAlso =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("h4", null, html_react_parser_default()(localization_Localization.get("Prompt_Help_SeeAlso")));
  var anchorSyntax =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("a", {
    href: "#",
    className: "dnn-prompt-cmd-insert",
    "data-cmd": "help syntax"
  }, html_react_parser_default()(localization_Localization.get("Prompt_Help_Syntax")));
  var anchorLearn =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("a", {
    href: "#",
    className: "dnn-prompt-cmd-insert",
    style: {
      marginLeft: "10px"
    },
    "data-cmd": "help learn"
  }, html_react_parser_default()(localization_Localization.get("Prompt_Help_Learn")));
  var out =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("section", {
    className: "dnn-prompt-inline-help"
  }, headingName, paragraphDescription, headingCommands, divCommands, headingSeeAlso, anchorSyntax, anchorLearn);
  return out;
};

Command_Command.propTypes = {
  commandList: prop_types_default.a.array.isRequired,
  IsPaging: prop_types_default.a.func.isRequired
};
/* harmony default export */ var components_Command = (Command_Command);
// CONCATENATED MODULE: ./src/components/TextLine.jsx
function TextLine_toConsumableArray(arr) { return TextLine_arrayWithoutHoles(arr) || TextLine_iterableToArray(arr) || TextLine_nonIterableSpread(); }

function TextLine_nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance"); }

function TextLine_iterableToArray(iter) { if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter); }

function TextLine_arrayWithoutHoles(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } }






var TextLine_TextLine = function TextLine(_ref) {
  var txt = _ref.txt,
      css = _ref.css;

  if (!txt) {
    return null;
  }

  var textLines = txt.split("\\n");
  var rows = textLines.map(function (line) {
    return line ?
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("span", {
      key: DomKey.get("textline"),
      className: css
    }, html_react_parser_default()(line)) : null;
  }).reduce(function (prev, current) {
    if (current !== "" && current !== null && current !== undefined) {
      return [].concat(TextLine_toConsumableArray(prev), [current]);
    }

    return [].concat(TextLine_toConsumableArray(prev), [
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("br", {
      key: DomKey.get("textline")
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("br", {
      key: DomKey.get("textline")
    })]);
  }, []);
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("div", {
      key: DomKey.get("textline")
    }, rows)
  );
};

TextLine_TextLine.defaultProps = {
  css: "dnn-prompt-cmd"
};
TextLine_TextLine.propTypes = {
  txt: prop_types_default.a.string.isRequired,
  css: prop_types_default.a.string
};
/* harmony default export */ var components_TextLine = (TextLine_TextLine);
// CONCATENATED MODULE: ./src/components/Help.jsx









var Help_Help = function Help(_ref) {
  var style = _ref.style,
      isError = _ref.isError,
      error = _ref.error,
      name = _ref.name,
      description = _ref.description,
      options = _ref.options,
      IsPaging = _ref.IsPaging,
      resultHtml = _ref.resultHtml;
  IsPaging(false);
  var css = style ? style : isError ? "dnn-prompt-error" : "dnn-prompt-ok";

  if (isError) {
    return (
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(components_TextLine, {
        txt: error,
        css: css
      })
    );
  }

  var headingName =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("h3", {
    className: "mono"
  }, name);
  var anchorName =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("a", {
    name: name
  });
  var paragraphDescription =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("p", {
    className: "lead"
  }, description);
  var fields = ["$Flag", "Type", "Required", "Default", "Description"];
  var out =
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("section", {
    key: DomKey.get("help"),
    className: "dnn-prompt-inline-help"
  }, anchorName, headingName, paragraphDescription, options && options.length > 0 &&
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("h4", null, localization_Localization.get("Help_Options")), options && options.length > 0 &&
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("div", null,
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement(components_DataTable, {
    rows: options,
    columns: fields,
    cssClass: "table"
  })), resultHtml &&
  /*#__PURE__*/
  external_window_dnn_nodeModules_React_default.a.createElement("div", null, html_react_parser_default()(stripWhiteSpaces(resultHtml))));
  return out;
};

Help_Help.propTypes = {
  IsPaging: prop_types_default.a.func.isRequired,
  style: prop_types_default.a.string.isRequired,
  isError: prop_types_default.a.bool.isRequired,
  error: prop_types_default.a.string,
  name: prop_types_default.a.string.isRequired,
  description: prop_types_default.a.string,
  options: prop_types_default.a.array,
  resultHtml: prop_types_default.a.string
};
Help_Help.defaultProps = {
  isError: false
};
/* harmony default export */ var components_Help = (Help_Help);
// CONCATENATED MODULE: ./src/components/Output.jsx
function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function Output_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function Output_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function Output_createClass(Constructor, protoProps, staticProps) { if (protoProps) Output_defineProperties(Constructor.prototype, protoProps); if (staticProps) Output_defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }










var Output_Output =
/*#__PURE__*/
function (_Component) {
  _inherits(Output, _Component);

  function Output() {
    Output_classCallCheck(this, Output);

    return _possibleConstructorReturn(this, _getPrototypeOf(Output).apply(this, arguments));
  }

  Output_createClass(Output, [{
    key: "renderResults",
    value: function renderResults() {
      var props = this.props;
      var fieldOrder = props.fieldOrder;
      props.IsPaging(false);

      if (props.isHelp) {
        if (props.commandList !== null && props.commandList.length > 0) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(components_Command, _extends({
              key: DomKey.get("output")
            }, props, {
              commandList: props.commandList,
              IsPaging: props.IsPaging
            }))
          );
        } else {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(components_Help, _extends({
              key: DomKey.get("output")
            }, props, {
              IsPaging: props.IsPaging,
              style: props.style,
              isError: props.isError,
              name: props.name
            }))
          );
        }
      }

      if (props.reload) {
        if (props.output !== null && props.output !== "" && props.output.toLowerCase().indexOf("http") >= 0) {
          window.top.location.href = props.output;
        } else {
          location.reload(true);
        }
      } else if (props.data) {
        var result = [];

        if (props.paging !== null && props.paging !== undefined) {
          props.IsPaging(props.paging.pageNo < props.paging.totalPages);
        }

        result.push(this.renderData(props.data, fieldOrder));
        result.push(
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_TextLine, {
          key: DomKey.get("output"),
          txt: props.output,
          css: "dnn-prompt-ok"
        }));
        return result;
      } else if (props.isHtml) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_TextLine, {
            key: DomKey.get("output"),
            txt: props.output
          })
        );
      } else if (props.output) {
        var style = props.isError ? "dnn-prompt-error" : "dnn-prompt-".concat(props.style === "cmd" ? "cmd" : "ok");
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_TextLine, {
            key: DomKey.get("output"),
            txt: props.output,
            css: style
          })
        );
      }
    }
  }, {
    key: "renderData",
    value: function renderData(data, fieldOrder) {
      if (data.length > 1) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(components_DataTable, {
            rows: data,
            columns: fieldOrder,
            cssClass: ""
          })
        );
      } else if (data.length === 1) {
        return renderObject(data[0], fieldOrder);
      }

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("br", null)
      );
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      this.output = !props.clearOutput && this.output ? this.output : [];
      var out = this.renderResults();
      this.output.push(out);
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "dnn-prompt-output"
        }, !props.clearOutput && this.output)
      );
    }
  }]);

  return Output;
}(external_window_dnn_nodeModules_React_["Component"]);
Output_Output.propTypes = {
  output: prop_types_default.a.string,
  data: prop_types_default.a.array,
  paging: prop_types_default.a.object,
  isHtml: prop_types_default.a.bool,
  reload: prop_types_default.a.bool,
  isError: prop_types_default.a.bool,
  clearOutput: prop_types_default.a.bool,
  fieldOrder: prop_types_default.a.array,
  commandList: prop_types_default.a.array,
  style: prop_types_default.a.string,
  isHelp: prop_types_default.a.bool,
  name: prop_types_default.a.string,
  description: prop_types_default.a.string,
  options: prop_types_default.a.array,
  resultHtml: prop_types_default.a.string,
  error: prop_types_default.a.string,
  IsPaging: prop_types_default.a.func.isRequired
};
/* harmony default export */ var components_Output = (Output_Output);
// CONCATENATED MODULE: ./src/constants/promptLabel.js
var commands = {
  HELP: "HELP",
  CLS: "CLS",
  CLEAR_SCREEN: "CLEAR-SCREEN",
  EXIT: "EXIT",
  RELOAD: "RELOAD",
  CLH: "CLH",
  CLEAR_HISTORY: "CLEAR-HISTORY",
  SET_MODE: "SET-MODE"
};
var modes = {
  EDIT: "EDIT",
  LAYOUT: "LAYOUT",
  VIEW: "VIEW"
};
// CONCATENATED MODULE: ./src/components/Input.jsx
function Input_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { Input_typeof = function _typeof(obj) { return typeof obj; }; } else { Input_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return Input_typeof(obj); }

function _slicedToArray(arr, i) { return _arrayWithHoles(arr) || _iterableToArrayLimit(arr, i) || _nonIterableRest(); }

function _nonIterableRest() { throw new TypeError("Invalid attempt to destructure non-iterable instance"); }

function _iterableToArrayLimit(arr, i) { var _arr = []; var _n = true; var _d = false; var _e = undefined; try { for (var _i = arr[Symbol.iterator](), _s; !(_n = (_s = _i.next()).done); _n = true) { _arr.push(_s.value); if (i && _arr.length === i) break; } } catch (err) { _d = true; _e = err; } finally { try { if (!_n && _i["return"] != null) _i["return"](); } finally { if (_d) throw _e; } } return _arr; }

function _arrayWithHoles(arr) { if (Array.isArray(arr)) return arr; }

function Input_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function Input_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function Input_createClass(Constructor, protoProps, staticProps) { if (protoProps) Input_defineProperties(Constructor.prototype, protoProps); if (staticProps) Input_defineProperties(Constructor, staticProps); return Constructor; }

function Input_possibleConstructorReturn(self, call) { if (call && (Input_typeof(call) === "object" || typeof call === "function")) { return call; } return Input_assertThisInitialized(self); }

function Input_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function Input_getPrototypeOf(o) { Input_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return Input_getPrototypeOf(o); }

function Input_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) Input_setPrototypeOf(subClass, superClass); }

function Input_setPrototypeOf(o, p) { Input_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return Input_setPrototypeOf(o, p); }








var Input_Input =
/*#__PURE__*/
function (_Component) {
  Input_inherits(Input, _Component);

  function Input() {
    Input_classCallCheck(this, Input);

    return Input_possibleConstructorReturn(this, Input_getPrototypeOf(Input).apply(this, arguments));
  }

  Input_createClass(Input, [{
    key: "setValue",
    value: function setValue(value) {
      this.cmdPromptInput.value = value;
    }
  }, {
    key: "getValue",
    value: function getValue() {
      return this.cmdPromptInput.value;
    }
  }, {
    key: "setFocus",
    value: function setFocus(focus) {
      focus ? this.cmdPromptInput.focus() : this.cmdPromptInput.blur();
    }
  }, {
    key: "getTabId",
    value: function getTabId() {
      var dnnVariable = JSON.parse(window.parent.document.getElementsByName("__dnnVariable")[0].value);
      return dnnVariable.sf_tabId;
    }
  }, {
    key: "readInput",
    value: function readInput() {
      var props = this.props;
      var txt = this.getValue().trim();

      if (txt === "" && props.nextPageCommand !== null && props.nextPageCommand !== "") {
        txt = props.nextPageCommand;
      }

      props.updateHistory(txt);
      return txt;
    }
  }, {
    key: "runLocalCmd",
    value: function runLocalCmd(txt) {
      var props = this.props;
      var actions = props.actions;

      if (!this.tabId) {
        this.tabId = this.getTabId();
      }

      this.setValue(""); // clear input for future commands.
      // Client Command

      var tokens = txt.split(" ");
      var cmd = tokens[0].toUpperCase();
      var done = true;

      switch (cmd) {
        case commands.CLS:
        case commands.CLEAR_SCREEN:
        case commands.EXIT:
        case commands.RELOAD:
          actions.runLocalCommand(cmd, null);
          break;

        case commands.CLH:
        case commands.CLEAR_HISTORY:
          props.updateHistory("", true);
          actions.runLocalCommand(cmd, localization_Localization.get("SessionHistoryCleared"));
          break;

        case commands.SET_MODE:
          this.changeUserMode(tokens);
          break;

        default:
          done = false;
          break;
      }

      return done;
    }
  }, {
    key: "runServerCmd",
    value: function runServerCmd(txt) {
      var props = this.props;
      var actions = props.actions;

      if (!this.tabId) {
        this.tabId = this.getTabId();
      }

      this.setValue(""); // clear input for future commands.
      // Client Command

      var tokens = txt.split(" ");
      var cmd = tokens[0].toUpperCase(); // Server Command

      var errorCallback = function errorCallback(error) {
        actions.runLocalCommand("ERROR", error.responseJSON.Message);
      };

      if (cmd === commands.HELP) {
        if (txt.toUpperCase() === "HELP") {
          actions.getCommandList({
            cmdLine: "list-commands",
            currentPage: self.tabId
          }, function () {}, errorCallback.bind(this));
        } else {
          actions.runHelpCommand({
            cmdLine: txt,
            currentPage: self.tabId
          }, function () {}, errorCallback.bind(this));
        }
      } else {
        if (!commands[cmd]) {
          actions.runCommand({
            cmdLine: txt,
            currentPage: self.tabId
          }, function () {}, errorCallback.bind(this));
        }
      }
    }
  }, {
    key: "displayCmdInfo",
    value: function displayCmdInfo(txt) {
      var props = this.props;
      var actions = props.actions;
      actions.runLocalCommand("INFO", "\n" + txt + "\n", "cmd");
    }
  }, {
    key: "runCmd",
    value: function runCmd() {
      var txt = this.readInput();

      if (txt) {
        this.displayCmdInfo(txt);

        if (!this.runLocalCmd(txt)) {
          this.runServerCmd(txt);
        }

        this.setFocus(false);
      }
    }
  }, {
    key: "changeUserMode",
    value: function changeUserMode(tokens) {
      var props = this.props;
      var actions = props.actions;

      if (!tokens && tokens.length >= 2) {
        return;
      }

      tokens = tokens.map(function (token) {
        return token.toUpperCase();
      });

      var _tokens$slice = tokens.slice(-1),
          _tokens$slice2 = _slicedToArray(_tokens$slice, 1),
          mode = _tokens$slice2[0];

      if (mode && !this.isFlag(mode) && modes[mode]) {
        actions.changeUserMode({
          UserMode: mode
        }, function () {
          if (mode === "EDIT") util.utilities.closePersonaBar();
          window.parent.document.location.reload(true);
        }, function (error) {
          return actions.runLocalCommand("ERROR", error);
        });
      } else {
        actions.runLocalCommand("ERROR", formatString(localization_Localization.get("Prompt_FlagIsRequired"), "Mode"));
      }
    }
  }, {
    key: "isFlag",
    value: function isFlag(token) {
      return token && token.indexOf("--") === 0;
    }
  }, {
    key: "getFlag",
    value: function getFlag(flag, tokens) {
      var _this = this;

      if (!tokens || tokens.length === 0) {
        return null;
      }

      return tokens.find(function (token) {
        return _this.isFlag(token) && flag.toUpperCase() === token.toUpperCase();
      });
    }
  }, {
    key: "hasFlag",
    value: function hasFlag(flag, tokens) {
      return tokens ? tokens.find(function (token) {
        return token.toUpperCase === flag.toUpperCase();
      }) : false;
    }
  }, {
    key: "isPaging",
    value: function isPaging() {
      var props = this.props;
      var IS_PAGING = props.paging !== null && props.paging.pageNo <= props.paging.totalPages;
      return IS_PAGING;
    }
  }, {
    key: "isLastPage",
    value: function isLastPage() {
      var props = this.props;
      var IS_LAST_PAGE = props.paging !== null && props.paging.pageNo === props.paging.totalPages;
      return IS_LAST_PAGE;
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      var IS_PAGING = this.isPaging();
      var IS_LAST_PAGE = this.isLastPage();
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "dnn-prompt-input-wrapper ".concat(IS_PAGING && !IS_LAST_PAGE ? "hidden-cursor" : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("input", {
          className: "dnn-prompt-input ".concat(IS_PAGING && !IS_LAST_PAGE ? "hidden-text" : ""),
          ref: function ref(el) {
            _this2.cmdPromptInput = el;

            if (el) {
              el.readOnly = _this2.isPaging();
              /**
               * Note: this is a patch to be removed once a complete refactoring of the Redux
               * implementation will be done.
               * All changes in the DOM must happen accordingly to Redux state update and this
               * usage of setTimeout has to be considered wrong.
               * Patch needed to close DNN-10688
               */

              if (_this2.isPaging() && _this2.isLastPage() === true) {
                el.value = "";
                setTimeout(function () {
                  return el.readOnly = false;
                }, 500);
              }
            }
          }
        }))
      );
    }
  }]);

  return Input;
}(external_window_dnn_nodeModules_React_["Component"]);

Input_Input.propTypes = {
  nextPageCommand: prop_types_default.a.string,
  updateHistory: prop_types_default.a.func.isRequired,
  setHeight: prop_types_default.a.func.isRequired,
  actions: prop_types_default.a.object.isRequired,
  paging: prop_types_default.a.object
};
/* harmony default export */ var components_Input = (Input_Input);
// CONCATENATED MODULE: ./src/services/applicationService.js
function applicationService_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function applicationService_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function applicationService_createClass(Constructor, protoProps, staticProps) { if (protoProps) applicationService_defineProperties(Constructor.prototype, protoProps); if (staticProps) applicationService_defineProperties(Constructor, staticProps); return Constructor; }



var applicationService_ApplicationService =
/*#__PURE__*/
function () {
  function ApplicationService() {
    applicationService_classCallCheck(this, ApplicationService);
  }

  applicationService_createClass(ApplicationService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = util.utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getServiceFrameworkWithRoot",
    value: function getServiceFrameworkWithRoot(moduleRoot, controller) {
      var sf = util.utilities.sf;
      sf.moduleRoot = moduleRoot;
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "runCommand",
    value: function runCommand(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("Command");
      sf.post("Cmd", payload, callback, failureCallback);
    }
  }, {
    key: "changeUserMode",
    value: function changeUserMode(payload, callback, failureCallback) {
      var sf = this.getServiceFrameworkWithRoot("InternalServices", "ControlBar");
      sf.post("ToggleUserMode", payload, callback, failureCallback);
    }
  }]);

  return ApplicationService;
}();

var applicationService = new applicationService_ApplicationService();
/* harmony default export */ var services_applicationService = (applicationService);
// CONCATENATED MODULE: ./src/actions/prompt.js


function getCommandList(payload, callback, errorCallback) {
  return function (dispatch) {
    services_applicationService.runCommand(payload, function (data) {
      dispatch({
        type: constants_actionTypes.RETRIEVED_COMMAND_LIST,
        data: {
          commands: data.data
        }
      });

      if (callback) {
        callback(data.data);
      }
    }, function (data) {
      if (errorCallback) {
        errorCallback(data);
      }
    });
  };
}
function runCommand(payload, callback, errorCallback) {
  return function (dispatch) {
    services_applicationService.runCommand(payload, function (data) {
      dispatch({
        type: constants_actionTypes.EXECUTED_COMMAND,
        data: {
          result: data
        }
      });

      if (callback) {
        callback(data);
      }
    }, function (data) {
      if (errorCallback) {
        errorCallback(data);
      }
    });
  };
}
function runHelpCommand(payload, callback, errorCallback) {
  return function (dispatch) {
    services_applicationService.runCommand(payload, function (data) {
      dispatch({
        type: constants_actionTypes.EXECUTED_HELP_COMMAND,
        data: {
          result: data
        }
      });

      if (callback) {
        callback(data);
      }
    }, function (data) {
      if (errorCallback) {
        errorCallback(data);
      }
    });
  };
}
function runLocalCommand(command, output, style) {
  return function (dispatch) {
    dispatch({
      type: constants_actionTypes.EXECUTED_LOCAL_COMMAND,
      command: command,
      output: output,
      style: style
    });
  };
}
function endPaging() {
  return function (dispatch) {
    dispatch({
      type: constants_actionTypes.END_PAGING
    });
  };
}
function prompt_changeUserMode(payload, callback, errorCallback) {
  return function (dispatch) {
    services_applicationService.changeUserMode(payload, function (data) {
      dispatch({
        type: constants_actionTypes.CHANGE_USER_MODE,
        data: {
          result: data
        }
      });

      if (callback) {
        callback(data);
      }
    }, function (data) {
      if (errorCallback) {
        errorCallback(data);
      }
    });
  };
}
// EXTERNAL MODULE: external "window.dnn.nodeModules.CommonComponents"
var external_window_dnn_nodeModules_CommonComponents_ = __webpack_require__(15);

// CONCATENATED MODULE: ./src/components/App.jsx
function App_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { App_typeof = function _typeof(obj) { return typeof obj; }; } else { App_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return App_typeof(obj); }

function App_extends() { App_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return App_extends.apply(this, arguments); }

function App_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function App_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function App_createClass(Constructor, protoProps, staticProps) { if (protoProps) App_defineProperties(Constructor.prototype, protoProps); if (staticProps) App_defineProperties(Constructor, staticProps); return Constructor; }

function App_possibleConstructorReturn(self, call) { if (call && (App_typeof(call) === "object" || typeof call === "function")) { return call; } return App_assertThisInitialized(self); }

function App_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function App_getPrototypeOf(o) { App_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return App_getPrototypeOf(o); }

function App_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) App_setPrototypeOf(subClass, superClass); }

function App_setPrototypeOf(o, p) { App_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return App_setPrototypeOf(o, p); }












var App_App =
/*#__PURE__*/
function (_Component) {
  App_inherits(App, _Component);

  function App(props) {
    var _this;

    App_classCallCheck(this, App);

    _this = App_possibleConstructorReturn(this, App_getPrototypeOf(App).call(this, props));
    _this.isPaging = false;
    _this.history = [];
    _this.cmdOffset = 0; // reverse offset into history

    var dispatch = props.dispatch;
    _this.actions = Object(external_window_dnn_nodeModules_Redux_["bindActionCreators"])(prompt_namespaceObject, dispatch);
    return _this;
  }

  App_createClass(App, [{
    key: "componentDidUpdate",
    value: function componentDidUpdate() {
      this.setFocus(true);
      this.scrollToBottom();
    }
  }, {
    key: "componentDidMount",
    value: function componentDidMount() {
      this.showGreeting();
    }
  }, {
    key: "onClickHandler",
    value: function onClickHandler(e) {
      if (e.target.classList.contains("dnn-prompt-cmd-insert")) {
        this.setValue(e.target.dataset.cmd.replace(/'/g, "\""));
        this.setFocus(true);
      } else {
        this.setFocus(true);
      }
    }
  }, {
    key: "scrollToBottom",
    value: function scrollToBottom() {
      if (this.cmdPrompt) {
        this.cmdPrompt.scrollTop = this.cmdPrompt.scrollHeight;
      }
    }
  }, {
    key: "showGreeting",
    value: function showGreeting() {
      this.actions.runLocalCommand("INFO", localization_Localization.get("PromptGreeting"), "cmd");
    }
  }, {
    key: "setValue",
    value: function setValue(value) {
      this.cmdPromptInputControl.setValue(value);
    }
  }, {
    key: "setFocus",
    value: function setFocus(focus) {
      this.cmdPromptInputControl.setFocus(focus);
    }
  }, {
    key: "runCmd",
    value: function runCmd() {
      this.cmdPromptInputControl.runCmd();
    }
  }, {
    key: "updateHistory",
    value: function updateHistory(value, isClear) {
      this.cmdOffset = 0; // reset history index

      if (isClear) {
        this.history = [];
      } else {
        //Remove command from history if already exists. 
        if (this.history.some(function (item) {
          return item === value;
        })) {
          this.history = this.history.filter(function (item) {
            return item !== value;
          });
        } //Push the command as the last run.


        this.history.push(value);
      }
    }
  }, {
    key: "paging",
    value: function paging(isPaging) {
      this.isPaging = isPaging;
    }
  }, {
    key: "setHeight",
    value: function setHeight(height) {
      if (this.cmdPrompt !== null && this.cmdPrompt !== undefined && height !== undefined && height !== "") {
        height = height.replace("%", "");
        if (parseInt(height) > 0 && parseInt(height) <= 100) this.cmdPrompt.style.height = height + "%";
      }
    }
  }, {
    key: "endPaging",
    value: function endPaging() {
      this.actions.endPaging();
      this.setValue("");
      this.setFocus(true);
    }
  }, {
    key: "keyDownHandler",
    value: function keyDownHandler(e) {
      var props = this.props;
      var lastPage = props.paging !== null && props.paging.pageNo === props.paging.totalPages; //CTRL + Key

      if (e.ctrlKey) {
        if (e.keyCode === 192) {
          if (this.wrapper[0].offsetLeft <= 0) {
            this.util.loadPanel("Dnn.Prompt", {
              moduleName: "Dnn.Prompt",
              folderName: "",
              identifier: "Dnn.Prompt",
              path: "Prompt"
            });
          } else {
            this.util.closePersonaBar();
          }

          return;
        }

        if (e.keyCode === 88) {
          this.endPaging();
          return;
        }
      }

      if (props.isBusy) return;

      if (document.activeElement.className.indexOf("dnn-prompt-input") > -1 && document.activeElement.offsetParent !== null && document.activeElement.offsetParent.className.indexOf("dnn-prompt-input-wrapper") > -1 && document.activeElement.tagName === "INPUT" && document.activeElement.type === "text") {
        switch (e.keyCode) {
          case 13:
            // enter key
            if (this.isPaging) {
              this.setValue("");
              this.runCmd();
              this.setFocus(false);
              return;
            }

            return this.runCmd();

          case 38:
            // Up arrow
            if (this.history.length + this.cmdOffset > 0) {
              this.cmdOffset--;
              this.setValue(this.history[this.history.length + this.cmdOffset]);
              e.preventDefault();
            }

            break;

          case 40:
            // Down arrow
            if (this.cmdOffset < -1) {
              this.cmdOffset++;
              this.setValue(this.history[this.history.length + this.cmdOffset]);
              e.preventDefault();
            }

            break;
        }
      }

      if (this.isPaging && !e.ctrlKey) {
        this.setValue("");
        this.setFocus(false);
        this.runCmd();

        if (lastPage) {
          this.endPaging();
        }

        return;
      }
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: true,
          fullWidth: true
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "wrapper"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "dnn-prompt",
          style: {
            display: "block"
          },
          onKeyDown: this.keyDownHandler.bind(this),
          onClick: this.onClickHandler.bind(this),
          ref: function ref(el) {
            return _this2.cmdPrompt = el;
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_Output, App_extends({}, props, {
          className: "Output",
          scrollToBottom: this.scrollToBottom.bind(this),
          IsPaging: this.paging.bind(this)
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("br", null), !props.isBusy &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_Input, App_extends({
          ref: function ref(el) {
            return _this2.cmdPromptInputControl = el;
          }
        }, props, {
          actions: this.actions,
          updateHistory: this.updateHistory.bind(this),
          paging: props.paging,
          setHeight: this.setHeight.bind(this)
        })))))
      );
    }
  }]);

  return App;
}(external_window_dnn_nodeModules_React_["Component"]);
App_App.propTypes = {
  output: prop_types_default.a.string,
  data: prop_types_default.a.array,
  isHtml: prop_types_default.a.bool,
  reload: prop_types_default.a.bool,
  isError: prop_types_default.a.bool,
  clearOutput: prop_types_default.a.bool,
  fieldOrder: prop_types_default.a.array,
  commandList: prop_types_default.a.array,
  style: prop_types_default.a.string,
  isBusy: prop_types_default.a.bool,
  isHelp: prop_types_default.a.bool,
  name: prop_types_default.a.string,
  nextPageCommand: prop_types_default.a.string,
  description: prop_types_default.a.string,
  options: prop_types_default.a.array,
  resultHtml: prop_types_default.a.string,
  error: prop_types_default.a.string
};

function mapStateToProps(state) {
  return {
    output: state.output,
    data: state.data,
    paging: state.pagingInfo,
    isBusy: state.isBusy,
    isHtml: state.isHtml,
    reload: state.reload,
    style: state.style,
    fieldOrder: state.fieldOrder,
    commandList: state.commandList,
    isError: state.isError,
    clearOutput: state.clearOutput,
    isHelp: state.isHelp,
    name: state.name,
    description: state.description,
    options: state.options,
    resultHtml: state.resultHtml,
    error: state.error,
    nextPageCommand: state.nextPageCommand
  };
}

/* harmony default export */ var components_App = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(mapStateToProps)(App_App));
// CONCATENATED MODULE: ./src/containers/Root.js
function Root_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { Root_typeof = function _typeof(obj) { return typeof obj; }; } else { Root_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return Root_typeof(obj); }

function Root_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function Root_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function Root_createClass(Constructor, protoProps, staticProps) { if (protoProps) Root_defineProperties(Constructor.prototype, protoProps); if (staticProps) Root_defineProperties(Constructor, staticProps); return Constructor; }

function Root_possibleConstructorReturn(self, call) { if (call && (Root_typeof(call) === "object" || typeof call === "function")) { return call; } return Root_assertThisInitialized(self); }

function Root_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function Root_getPrototypeOf(o) { Root_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return Root_getPrototypeOf(o); }

function Root_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) Root_setPrototypeOf(subClass, superClass); }

function Root_setPrototypeOf(o, p) { Root_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return Root_setPrototypeOf(o, p); }






var Root_Root =
/*#__PURE__*/
function (_Component) {
  Root_inherits(Root, _Component);

  function Root() {
    Root_classCallCheck(this, Root);

    return Root_possibleConstructorReturn(this, Root_getPrototypeOf(Root).call(this));
  }

  Root_createClass(Root, [{
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "dnnPrompt-app personaBar-mainContainer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_App, null), IS_DEV &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(DevTools, null))
      );
    }
  }]);

  return Root;
}(external_window_dnn_nodeModules_React_["Component"]);


// CONCATENATED MODULE: ./src/main.jsx






var store = configureStore();
globals_promptInit.dispatch = store.dispatch;
globals_promptInit.init();
var appContainer = document.getElementById("dnnPrompt-container");
Object(external_window_dnn_nodeModules_ReactDOM_["render"])(
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactRedux_["Provider"], {
  store: store
},
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(Root_Root, null)), appContainer);

/***/ })
/******/ ]);
//# sourceMappingURL=prompt-bundle.js.map