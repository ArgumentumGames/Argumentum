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
/******/ 	__webpack_require__.p = "http://localhost:8080/dist/";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 108);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.React;

/***/ }),
/* 1 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.CommonComponents;

/***/ }),
/* 2 */
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
  module.exports = __webpack_require__(54)();
}


/***/ }),
/* 3 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";

// CONCATENATED MODULE: ./src/constants/actionTypes/pagination.js
var paginationActionTypes = {
  LOAD_MORE: "LOAD_MORE",
  LOAD_TAB_DATA: "LOAD_TAB_DATA"
};
/* harmony default export */ var pagination = (paginationActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/visiblePanel.js
var visiblePanel_paginationActionTypes = {
  SELECT_PANEL: "SELECT_PANEL"
};
/* harmony default export */ var visiblePanel = (visiblePanel_paginationActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/extension.js
var extensionActionTypes = {
  RETRIEVED_INSTALLED_PACKAGES: "RETRIEVED_INSTALLED_PACKAGES",
  RETRIEVED_AVAILABLE_PACKAGES: "RETRIEVED_AVAILABLE_PACKAGES",
  UPDATED_EXTENSION: "UPDATED_EXTENSION",
  ADDED_NEW_EXTENSION: "ADDED_NEW_EXTENSION",
  DELETED_EXTENSION: "DELETED_EXTENSION",
  TOGGLE_DELETE_EXTENSION_FILES: "TOGGLE_DELETE_EXTENSION_FILES",
  SET_EXTENSION_BEING_DELETED: "SET_EXTENSION_BEING_DELETED",
  EDIT_EXTENSION: "EDIT_EXTENSION",
  RETRIEVED_INSTALLED_PACKAGE_TYPES: "RETRIEVED_INSTALLED_PACKAGE_TYPES",
  RETRIEVED_AVAILABLE_PACKAGE_TYPES: "RETRIEVED_AVAILABLE_PACKAGE_TYPES",
  CREATED_NEW_MODULE: "CREATED_NEW_MODULE",
  UPDATED_EXTENSION_BEING_EDITED: "UPDATED_EXTENSION_BEING_EDITED",
  TOGGLE_TRIED_TO_SAVE: "TOGGLE_TRIED_TO_SAVE",
  TOGGLE_TAB_ERROR: "TOGGLE_TAB_ERROR",
  RETRIEVED_MODULE_CATEGORIES: "RETRIEVED_MODULE_CATEGORIES",
  INSTALLED_EXTENSION: "INSTALLED_EXTENSION",
  PARSED_AVAILABLE_PACKAGE: "PARSED_AVAILABLE_PACKAGE",
  SELECT_EDITING_TAB: "SELECT_EDITING_TAB",
  RETRIEVED_LOCALE_LIST: "RETRIEVED_LOCALE_LIST",
  RETRIEVED_PACKAGE_LOCALE_LIST: "RETRIEVED_PACKAGE_LOCALE_LIST",
  DEPLOYED_AVAILABLE_PACKAGE: "DEPLOYED_AVAILABLE_PACKAGE",
  RETRIEVED_PACKAGE_USAGE_FILTER: "RETRIEVED_PACKAGE_USAGE_FILTER",
  RETRIEVED_PACKAGE_USAGE: "RETRIEVED_PACKAGE_USAGE"
};
/* harmony default export */ var extension = (extensionActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/installation.js
var installationActionTypes = {
  PARSED_INSTALLATION_PACKAGE: "PARSED_INSTALLATION_PACKAGE",
  GO_TO_WIZARD_STEP: "GO_TO_WIZARD_STEP",
  INSTALLED_EXTENSION_LOGS: "INSTALLED_EXTENSION_LOGS",
  CLEAR_PARSED_INSTALLATION_PACKAGE: "CLEAR_PARSED_INSTALLATION_PACKAGE",
  INSTALLING_AVAILABLE_PACKAGE: "INSTALLING_AVAILABLE_PACKAGE",
  NOT_INSTALLING_AVAILABLE_PACKAGE: "NOT_INSTALLING_AVAILABLE_PACKAGE",
  SET_FAILED_INSTALLATION_LOGS: "SET_FAILED_INSTALLATION_LOGS",
  TOGGLE_ACCEPT_LICENSE: "TOGGLE_ACCEPT_LICENSE",
  TOGGLE_VIEWING_LOG: "TOGGLE_VIEWING_LOG",
  SET_IS_PORTAL_PACKAGE: "SET_IS_PORTAL_PACKAGE"
};
/* harmony default export */ var installation = (installationActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/folder.js
var folderActionTypes = {
  RETRIEVED_OWNER_FOLDERS: "RETRIEVED_OWNER_FOLDERS",
  RETRIEVED_MODULE_FOLDERS: "RETRIEVED_MODULE_FOLDERS",
  RETRIEVED_MODULE_FILES: "RETRIEVED_MODULE_FILES",
  CREATED_NEW_OWNER_FOLDER: "CREATED_NEW_OWNER_FOLDER",
  CREATED_NEW_MODULE_FOLDER: "CREATED_NEW_MODULE_FOLDER"
};
/* harmony default export */ var folder = (folderActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/moduleDefinition.js
var moduleDefinitionActionTypes = {
  SET_FORM_DIRT: "SET_FORM_DIRT",
  SET_CONTROL_FORM_DIRT: "SET_CONTROL_FORM_DIRT",
  RETRIEVED_SOURCE_FOLDERS: "RETRIEVED_SOURCE_FOLDERS",
  RETRIEVED_SOURCE_FILES: "RETRIEVED_SOURCE_FILES",
  RETRIEVED_CONTROL_ICONS: "RETRIEVED_CONTROL_ICONS"
};
/* harmony default export */ var moduleDefinition = (moduleDefinitionActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/createPackage.js
var createPackageActionTypes = {
  RETRIEVED_PACKAGE_MANIFEST: "RETRIEVED_PACKAGE_MANIFEST",
  CREATED_PACKAGE_MANIFEST: "CREATED_PACKAGE_MANIFEST",
  UPDATED_PACKAGE_MANIFEST: "UPDATED_PACKAGE_MANIFEST",
  UPDATED_PACKAGE_PAYLOAD: "UPDATED_PACKAGE_PAYLOAD",
  RETRIEVED_GENERATED_MANIFEST: "RETRIEVED_GENERATED_MANIFEST",
  REFRESH_PACKAGE_FILES: "REFRESH_PACKAGE_FILES",
  CREATED_PACKAGE: "CREATED_PACKAGE",
  GO_TO_STEP: "GO_TO_STEP"
};
/* harmony default export */ var createPackage = (createPackageActionTypes);
// CONCATENATED MODULE: ./src/constants/actionTypes/index.js
/* concated harmony reexport pagination */__webpack_require__.d(__webpack_exports__, "f", function() { return pagination; });
/* concated harmony reexport visiblePanel */__webpack_require__.d(__webpack_exports__, "g", function() { return visiblePanel; });
/* concated harmony reexport extension */__webpack_require__.d(__webpack_exports__, "b", function() { return extension; });
/* concated harmony reexport installation */__webpack_require__.d(__webpack_exports__, "d", function() { return installation; });
/* concated harmony reexport folder */__webpack_require__.d(__webpack_exports__, "c", function() { return folder; });
/* concated harmony reexport moduleDefinition */__webpack_require__.d(__webpack_exports__, "e", function() { return moduleDefinition; });
/* concated harmony reexport createPackage */__webpack_require__.d(__webpack_exports__, "a", function() { return createPackage; });









/***/ }),
/* 4 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
var utils = {
  init: function init(utilities) {
    if (!utilities) {
      throw new Error("Utilities is undefined.");
    }

    this.utilities = utilities.utility;
    this.moduleName = utilities.moduleName;
    this.settings = utilities.settings;
    this.siteRoot = utilities.siteRoot;
  },
  utilities: null,
  moduleName: "",
  settings: {}
};
/* harmony default export */ __webpack_exports__["a"] = (utils);

/***/ }),
/* 5 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactRedux;

/***/ }),
/* 6 */
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
/* 7 */
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

var	fixUrls = __webpack_require__(53);

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
/* 8 */
/***/ (function(module, exports) {

module.exports = [4, 53, 10, 10, 12, 11];

/***/ }),
/* 9 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactCustomScrollBars;

/***/ }),
/* 10 */
/***/ (function(module, exports) {

module.exports = [4, 50, 11, 35];

/***/ }),
/* 11 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.Redux;

/***/ }),
/* 12 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(73);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 13 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 14 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<svg id=\"Layer_1\" data-name=\"Layer 1\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 550.08 300.98\">\r\n    <defs>\r\n        <style>.cls-1{fill:#ccc;}.cls-2{fill:#9f9fa0;}.cls-3{fill:#b3b3b3;}.cls-4{fill:#9ca9ae;}</style>\r\n    </defs>\r\n    <ellipse class=\"cls-1\" cx=\"176.57\" cy=\"238.17\" rx=\"60.51\" ry=\"5.31\"/>\r\n    <path class=\"cls-2\" d=\"M220.64,175.7c-3.36,6-11.23,7.3-18.51,2.79-6.48-4-10.47-11.39-10.23-18V157l-4-2.51c-2.87,9.55,2.62,21.94,12.65,28.15,9.72,6,20.25,4.1,24.5-4.09Z\"/>\r\n    <path class=\"cls-3\" d=\"M447.71,71.6h-.51c-7.88-.2-14.18-4.93-14-10.55h0c.14-5.71,6.52-10,14.55-9.83,7.88.2,14.18,4.93,14,10.55S455.36,71.6,447.71,71.6ZM436.16,61.12c-.1,3.9,5,7.32,11.12,7.47s11.38-3,11.48-6.9-5-7.32-11.12-7.47-11.38,3-11.48,6.9Z\"/>\r\n    <path class=\"cls-4\" d=\"M337.16,162.53l-4-.45c.23-2,.52-4.09.87-6.11l3.94.68C337.66,158.59,337.38,160.57,337.16,162.53Zm2-11.71-3.9-.91c.46-2,1-4,1.58-6l3.84,1.13C340.14,147,339.62,148.9,339.18,150.82Zm3.37-11.4-3.76-1.35c.69-1.92,1.45-3.86,2.26-5.75l3.68,1.57C343.94,135.7,343.21,137.56,342.54,139.41Zm4.66-10.94-3.58-1.78c.91-1.84,1.89-3.67,2.9-5.45l3.47,2C349,124.94,348.08,126.71,347.21,128.48Zm5.89-10.33L349.74,116c1.11-1.71,2.29-3.42,3.5-5.08l3.23,2.36C355.3,114.85,354.16,116.5,353.09,118.15Zm7-9.59L357,106c1.3-1.58,2.66-3.15,4.05-4.65l2.94,2.71C362.68,105.53,361.37,107,360.11,108.56Zm8.06-8.74-2.79-2.87c1.47-1.43,3-2.83,4.55-4.17l2.62,3C371.06,97.09,369.59,98.44,368.17,99.82Zm9-7.78-2.45-3.16c1.62-1.25,3.3-2.48,5-3.64l2.27,3.3C380.33,89.64,378.71,90.82,377.15,92Zm9.79-6.74-2.08-3.42c1.75-1.07,3.55-2.1,5.35-3.06l1.89,3.53C390.36,83.27,388.62,84.27,386.94,85.29Zm10.47-5.61-1.69-3.62c1.85-.86,3.76-1.69,5.66-2.45l1.49,3.71C401,78.05,399.2,78.85,397.41,79.68Zm11-4.41-1.28-3.79c1.94-.65,3.92-1.26,5.9-1.81l1.07,3.85C412.22,74.05,410.3,74.64,408.43,75.27Zm11.46-3.17L419,68.19l1.63-.34.79,3.92Z\"/>\r\n    <path class=\"cls-4\" d=\"M334,162.28c-.7-1.82-1.46-3.68-2.27-5.51l3.66-1.62c.83,1.89,1.62,3.81,2.35,5.69Zm-4.81-10.91c-.88-1.76-1.82-3.54-2.79-5.28l3.49-1.95c1,1.79,2,3.62,2.88,5.44Zm-5.81-10.42c-1-1.67-2.12-3.36-3.25-5l3.31-2.24c1.15,1.7,2.27,3.44,3.34,5.16Zm-6.7-9.9c-1.17-1.59-2.4-3.19-3.65-4.74l3.12-2.5c1.28,1.6,2.54,3.23,3.74,4.87Zm-7.47-9.34c-1.29-1.49-2.64-3-4-4.45l2.93-2.73c1.39,1.5,2.77,3,4.1,4.56ZM301.08,113c-1.4-1.4-2.85-2.8-4.31-4.16l2.73-2.93c1.49,1.39,3,2.82,4.41,4.26Zm-8.76-8.17c-1.49-1.3-3-2.6-4.59-3.86l2.53-3.1c1.58,1.29,3.16,2.62,4.69,3.94ZM283,97.22c-1.58-1.2-3.2-2.4-4.83-3.55l2.32-3.26c1.66,1.18,3.32,2.4,4.93,3.63Zm-9.77-7c-1.66-1.1-3.35-2.19-5-3.25l2.11-3.4c1.73,1.07,3.46,2.19,5.15,3.31Zm-10.18-6.34c-1.71-1-3.47-2-5.23-2.94l1.91-3.52c1.79,1,3.59,2,5.33,3ZM252.52,78.2c-1.76-.89-3.58-1.77-5.4-2.63l1.7-3.62c1.85.87,3.7,1.77,5.5,2.68Zm-175.31-.5-2.15-3.37q1.33-.85,2.7-1.67l2.06,3.43Q78.5,76.88,77.21,77.71ZM85,73.14l-1.89-3.52c1.79-1,3.64-1.9,5.49-2.79l1.73,3.61C88.53,71.29,86.74,72.2,85,73.14Zm156.65,0c-1.82-.79-3.68-1.57-5.54-2.31l1.49-3.71c1.89.76,3.79,1.55,5.64,2.36Zm-11.14-4.47c-1.87-.69-3.77-1.36-5.66-2l1.28-3.79c1.92.65,3.87,1.33,5.77,2ZM95.76,68,94.2,64.29C96.06,63.5,98,62.73,99.9,62l1.41,3.74C99.44,66.45,97.58,67.2,95.76,68ZM219.13,64.8c-1.91-.59-3.85-1.15-5.77-1.68l1.06-3.86c1.95.54,3.93,1.11,5.88,1.71Zm-112.18-1L105.7,60c1.93-.63,3.9-1.24,5.87-1.8l1.1,3.85C110.75,62.56,108.82,63.15,106.95,63.77Zm100.6-2.16c-1.93-.47-3.9-.93-5.85-1.35l.85-3.91c2,.43,4,.89,6,1.38Zm-89.1-1.13-.95-3.89c2-.48,4-.93,6-1.34l.8,3.92C122.33,59.57,120.37,60,118.45,60.48Zm77.37-1.41c-2-.36-4-.71-5.92-1l.62-4c2,.31,4,.66,6,1Zm-65.64-1-.66-3.95c2-.34,4-.64,6.06-.9l.52,4C134.13,57.45,132.13,57.75,130.17,58.08Zm53.77-.85c-2-.25-4-.47-6-.66l.38-4c2,.19,4.06.42,6.09.67ZM142,56.52l-.38-4c2-.19,4.07-.36,6.11-.49l.25,4C146,56.17,144,56.32,142,56.52ZM172,56.08c-2-.13-4-.23-6-.29l.13-4c2,.07,4.09.17,6.12.3Zm-18-.32-.12-4c1.88-.06,3.8-.08,5.7-.08H160v4h-.42C157.73,55.68,155.85,55.71,154,55.76Z\"/>\r\n    <ellipse class=\"cls-1\" cx=\"443.59\" cy=\"233.55\" rx=\"24.96\" ry=\"5.31\"/>\r\n    <path class=\"cls-3\" d=\"M117.54,166.19A13,13,0,0,0,124,189.82c-4.12-2.25-7-7.2-7-13s2.86-10.71,7-13l-.55.07c2-2.14,2.66-5.57,2.2-9.56-5.35,2-13.65-2.5-19.7-11.13a33.35,33.35,0,0,1-6-14.77,6.62,6.62,0,0,0-1.85,1c-5.11,4.13-3.74,15.35,3.06,25.05C106,161.62,112.44,165.91,117.54,166.19Z\"/>\r\n    <path class=\"cls-3\" d=\"M214.89,187.77a9,9,0,0,0-3.17,1.56l5.15,1.41a7.26,7.26,0,0,1,2.74.75l.1,0v0a10.73,10.73,0,0,1,5.24,6.9c1.38,5.26-.91,10.42-5.13,11.53s-8.76-2.26-10.14-7.52a11.91,11.91,0,0,1-.36-4l-2-.55a16.48,16.48,0,0,0,.46,5.89c1.92,7.31,8.23,12,14.1,10.45s9.05-8.72,7.13-16S220.75,186.23,214.89,187.77Z\"/>\r\n    <path class=\"cls-3\" d=\"M241.24,187.84a7.16,7.16,0,0,0-2,2h4a4.84,4.84,0,0,1,2.15,0h.08v0a8.31,8.31,0,0,1,5.19,4.53c2.06,4,1.41,8.84-1.44,10.71s-6.83.11-8.89-3.93a11.13,11.13,0,0,1-1.06-3.23h-1.59a15.43,15.43,0,0,0,1.51,4.77c2.86,5.62,8.39,8.07,12.35,5.47s4.86-9.27,2-14.89S245.21,185.24,241.24,187.84Z\"/>\r\n    <path class=\"cls-3\" d=\"M113.47,199.33l-.15-.08,0,.16Z\"/>\r\n    <path class=\"cls-3\" d=\"M127.09,189.57h0c-.43,0-.85,0-1.27,0,.46.1.92.22,1.34.35C127.13,189.83,127.11,189.7,127.09,189.57Z\"/>\r\n    <path class=\"cls-3\" d=\"M175.66,198.13a46.22,46.22,0,0,1-11.45-3.36,18.57,18.57,0,0,0-2.15-13.94,23.81,23.81,0,0,1,.41,4.4,23.38,23.38,0,0,1-1.39,8l-2,4.07c-3.25,5.21-8.53,8.61-14.51,8.61-5.81,0-11-3.2-14.24-8.15a14.93,14.93,0,0,1-8.22,2.89A16.86,16.86,0,0,1,118.4,209c-3.69,4.8-8.52,7.28-10.78,5.54s-1.11-7,2.58-11.84a22.3,22.3,0,0,1,1.83-2.09,17.52,17.52,0,0,0-5.22,4.2c-4.53,5.29-5.35,12-1.84,15s10,1.14,14.55-4.15a15.65,15.65,0,0,0,4-9.47,22.35,22.35,0,0,0,3.55.29,20.84,20.84,0,0,0,7.26-1.26,18.77,18.77,0,0,0,27.57-5,32.44,32.44,0,0,0,14.16,5.85c14.59,2.33,28.05-5.07,31.22-16.73C201.38,196.54,189.05,200.26,175.66,198.13Z\"/>\r\n    <ellipse class=\"cls-3\" cx=\"215.32\" cy=\"127.89\" rx=\"2.52\" ry=\"6.13\"/>\r\n    <ellipse class=\"cls-3\" cx=\"231.29\" cy=\"127.89\" rx=\"2.52\" ry=\"6.13\"/>\r\n    <ellipse class=\"cls-3\" cx=\"231.29\" cy=\"148.05\" rx=\"12.93\" ry=\"6.16\"/>\r\n    <path class=\"cls-3\" d=\"M238.92,169.46c.53-1.78-.37-3.77-2.21-5.49a20.77,20.77,0,0,1-6.9-1.13,10.48,10.48,0,0,1-2.19,1c0-.23-.06-.46-.11-.69a4.15,4.15,0,0,1-.3.82,22,22,0,0,1-6.29.86c-.84,0-1.66,0-2.45-.12a4.19,4.19,0,0,0,.06.43,17.57,17.57,0,0,1-7.09-5.76,12.2,12.2,0,0,0,3.61,6.43,12.67,12.67,0,0,0,6.2,3.47,16.09,16.09,0,0,0,5.76,3C232.62,174,238,172.7,238.92,169.46Z\"/>\r\n    <path class=\"cls-3\" d=\"M239.65,114.37l4.58,9L246.63,134h4.25l3.22-7.87V117s-2.6-3.94-9.89-4.22S239.65,114.37,239.65,114.37Z\"/>\r\n    <path class=\"cls-3\" d=\"M217.08,108.22c8.27,0,15.47,1.55,19.15,3.84-.85-4.28-9.87-7.64-20.87-7.64-11.57,0-20.95,3.72-20.95,8.31H197C200.26,110.08,208,108.22,217.08,108.22Z\"/>\r\n    <path class=\"cls-3\" d=\"M247,139.8a7,7,0,0,1,2.39.88c4.52,2.73,5.08,10.08,1.25,16.41a17.85,17.85,0,0,1-3.86,4.51,14,14,0,0,0,8.12-5.18c4.14-5.28,4-12.14-.33-15.32A9.11,9.11,0,0,0,247,139.8Z\"/>\r\n    <path class=\"cls-3\" d=\"M130.07,157.21c3.49.46,5.43-.64,8.85-1.13a22.54,22.54,0,0,1,4.15-.2,18.17,18.17,0,0,0,2.69.71,7.66,7.66,0,0,0,3.86,2.64,34.58,34.58,0,0,0,8.32,1.79,8.93,8.93,0,1,0,15-8.27l9.07-4.71v-4.28s-10,3-12.49,4.28c-1.55.8-8-.44-12.64-1.5a19,19,0,0,0-3.34-.8l-1.73-.44-.89.21-.86,0c-4.11,0-7.73,1-9.86,2.55l-.44.1-10.94,6.77c-.82.28-1.83.71-1.8,1.15C127.06,156.8,129.72,157.17,130.07,157.21Z\"/>\r\n    <path class=\"cls-3\" d=\"M174.71,128.23l2.09,5.67s3.1,3.74,4.41,4.28,8.91-.62,8.91-.62l2.69-20.41c-3.31-.31-3-3.22-9.52-4.24a26,26,0,0,0-12.55,1.51l2,6.9Z\"/>\r\n</svg>");

/***/ }),
/* 15 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(74);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 16 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<g>\r\n\t<polygon fill=\"currentColor\" points=\"698.6,807.7 943.6,561.3 943.6,1375.8 1113.4,1375.8 1113.4,565.1 1360,807.7 1470.9,696.8 1026.6,256.6 \r\n\t\t915.8,367.5 915.8,367.5 587.8,696.8 \t\"/>\r\n\t<polygon fill=\"currentColor\" points=\"1582.5,1279.1 1582.5,1595.9 465.5,1595.9 465.5,1279.1 257.9,1279.1 257.9,1791.4 278.6,1791.4 511,1791.4 \r\n\t\t1537,1791.4 1790.1,1791.4 1790.1,1550.4 1790.1,1279.1 \t\"/>\r\n</g>\r\n</svg>\r\n");

/***/ }),
/* 17 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(99);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 18 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 19 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 20 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 21 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 22 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 23 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(107);
} else {}

/***/ }),
/* 24 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(56);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 25 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(57);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 26 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(60);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 27 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(63);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(64);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 29 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(71);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 30 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(72);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 31 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(77);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 32 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(78);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 33 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(79);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 34 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(80);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 35 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(81);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 36 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(82);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 37 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(83);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 38 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(84);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 39 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactCollapse;

/***/ }),
/* 40 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(85);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 41 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(86);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 42 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(87);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 43 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(88);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 44 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(89);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 45 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(92);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 46 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(100);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 47 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(101);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 48 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(104);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 49 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(105);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 50 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(106);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 51 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(52);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 52 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extensions-app * {\n  box-sizing: border-box;\n}\n.extensions-app .dnn-persona-bar-page-header button {\n  margin-left: 12px;\n}\n.extensions-app .dnn-persona-bar-page.hidden {\n  display: none;\n}\n.extensions-app .svg-active > svg {\n  fill: #1E88C3 !important;\n}\n.extensions-app .dnn-ui-common-tooltip .tooltip-text {\n  max-width: 255px;\n  text-align: center;\n  padding: 7px 15px;\n}\n.extensions-app .row-opened {\n  border-top: 2px solid #1E88C3 !important;\n  border-bottom: 2px solid #1E88C3 !important;\n}\n.extension-form {\n  padding: 40px 25px 25px;\n}\n.extension-form .dnn-grid-cell {\n  padding: 10px 25px 15px;\n  box-sizing: border-box;\n}\n.extension-form .dnn-grid-cell.modal-footer {\n  text-align: center;\n}\n.extension-form .dnn-grid-cell.modal-footer button {\n  min-width: 100px;\n  margin-right: 10px;\n}\n.extension-form .dnn-grid-cell .version-dropdown {\n  width: 31.1%;\n  box-sizing: border-box;\n  margin-right: 8px;\n  vertical-align: bottom;\n}\n.extension-form .dnn-grid-cell .version-dropdown:last-child {\n  margin-right: 0;\n}\n.extension-form .dnn-grid-system.with-right-border.bottom-half {\n  padding-top: 25px;\n}\n.extension-form .dnn-grid-system.with-right-border.top-half {\n  padding-bottom: 25px;\n}\n.extension-form .dnn-grid-system.with-right-border > .dnn-grid-cell:first-child {\n  border-right: 1px solid #C8C8C8;\n}\n", ""]);



/***/ }),
/* 53 */
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
/* 54 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(55);

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
/* 55 */
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
/* 56 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3oZpe0sXNMbh6iL5YggGy6 {\n  border-bottom: 1px solid #C8C8C8;\n}\n._3oZpe0sXNMbh6iL5YggGy6 > .dnn-grid-cell {\n  padding: 0 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"extensionHeader": "_3oZpe0sXNMbh6iL5YggGy6"
};

/***/ }),
/* 57 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._28HcX_98ot51f_xsb_hA-a {\n  border-bottom: 1px solid #C8C8C8;\n}\n._28HcX_98ot51f_xsb_hA-a .extension-action {\n  width: 16px;\n  height: 16px;\n  float: right;\n  cursor: pointer;\n  margin-right: 5px;\n}\n._28HcX_98ot51f_xsb_hA-a .dnn-grid-cell > img {\n  max-width: 100%;\n}\n._28HcX_98ot51f_xsb_hA-a .package-name {\n  display: inline-block;\n  word-break: keep-all;\n  white-space: normal;\n  max-width: 100%;\n  word-wrap: break-word;\n  vertical-align: top;\n  font-weight: bold;\n  margin-bottom: 3px;\n}\n._28HcX_98ot51f_xsb_hA-a .in-use {\n  color: #1E88C3;\n  cursor: pointer;\n}\n._28HcX_98ot51f_xsb_hA-a > .dnn-grid-cell {\n  padding: 0 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"extensionDetailRow": "_28HcX_98ot51f_xsb_hA-a"
};

/***/ }),
/* 58 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(59);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 59 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.modepanel-content-wrapper {\n  padding: 0 0 20px 0;\n}\n.modepanel-content-wrapper * {\n  box-sizing: border-box;\n}\n.modepanel-content-wrapper .modepanel-content-title {\n  float: left;\n  font-weight: bold;\n  text-transform: uppercase;\n  width: 100%;\n}\n.modepanel-content-wrapper .modepanel-content-filter {\n  border-bottom: 1px solid #C8C8C8;\n  float: left;\n  width: 100%;\n  margin: 20px 0;\n}\n.modepanel-content-wrapper .modepanel-content-detail-wrapper .usage-detail {\n  height: 200px;\n  border: 1px solid #C8C8C8;\n  float: left;\n  width: 100%;\n  overflow-y: auto;\n  margin: 10px 0;\n  padding: 15px 20px;\n  box-sizing: border-box;\n}\n.modepanel-content-wrapper .modepanel-content-detail-wrapper .usage-detail .usage-detail-taburl {\n  margin-bottom: 10px;\n}\n.modepanel-content-wrapper .modepanel-content-detail-wrapper .usage-detail .usage-detail-taburl a {\n  text-decoration: none;\n  color: #21A3DA;\n}\n.modepanel-content-wrapper .button-box {\n  float: left;\n  width: 100%;\n  text-align: center;\n  margin-top: 15px;\n}\n.modepanel-content-wrapper label {\n  float: left;\n  margin-right: 15px;\n  font-weight: bold;\n}\n", ""]);



/***/ }),
/* 60 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "._3kJ52JOsrdOOG57zRXrbq3 {\n  padding: 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"extensionList": "_3kJ52JOsrdOOG57zRXrbq3"
};

/***/ }),
/* 61 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(62);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 62 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extension-list .filter-dropdown {\n  width: auto;\n}\n.extension-list .filter-dropdown .dropdown-tooltip-container {\n  width: 275px;\n}\n.extension-list .filter-dropdown .dropdown-tooltip-container .dnn-dropdown .collapsible-label {\n  white-space: nowrap;\n}\n.extension-list > .filter-section {\n  float: none;\n}\n.extension-list .loading-extensions {\n  width: 550px;\n  margin: 58px auto 100px auto;\n}\n.extension-list .loading-extensions h2 {\n  font-size: 28px;\n  text-align: center;\n  color: #333;\n}\n.extension-list .loading-extensions p {\n  text-align: center;\n  font-size: 14px;\n  color: #808080;\n  margin-top: 13px;\n}\n", ""]);



/***/ }),
/* 63 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1Z-xvFkLZ6FZ9mtOArdlAf {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1Z-xvFkLZ6FZ9mtOArdlAf > .dnn-grid-cell {\n  padding: 0 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"extensionHeader": "_1Z-xvFkLZ6FZ9mtOArdlAf"
};

/***/ }),
/* 64 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2tpXMHg7pj1Qdu9WV7WrJn {\n  border-bottom: 1px solid #C8C8C8;\n}\n._2tpXMHg7pj1Qdu9WV7WrJn .install-download-button {\n  float: right;\n  margin-right: 5px;\n}\n._2tpXMHg7pj1Qdu9WV7WrJn .download-button {\n  float: right;\n}\n._2tpXMHg7pj1Qdu9WV7WrJn img {\n  width: 100%;\n}\n._2tpXMHg7pj1Qdu9WV7WrJn .package-name {\n  display: inline-block;\n  word-break: keep-all;\n  white-space: normal;\n  max-width: 100%;\n  word-wrap: break-word;\n  vertical-align: top;\n  font-weight: bold;\n  margin-bottom: 3px;\n}\n._2tpXMHg7pj1Qdu9WV7WrJn > .dnn-grid-cell {\n  padding: 0 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"extensionDetailRow": "_2tpXMHg7pj1Qdu9WV7WrJn"
};

/***/ }),
/* 65 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(66);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 66 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "", ""]);



/***/ }),
/* 67 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(68);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 68 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extension-list {\n  min-height: 550px;\n}\n.extension-list > .filter-section {\n  float: none;\n}\n.extension-list .loading-extensions {\n  width: 550px;\n  margin: 58px auto 100px auto;\n}\n.extension-list .loading-extensions h2 {\n  font-size: 28px;\n  text-align: center;\n  color: #333;\n}\n.extension-list .loading-extensions p {\n  text-align: center;\n  font-size: 14px;\n  color: #808080;\n  margin-top: 13px;\n}\n", ""]);



/***/ }),
/* 69 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(70);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 70 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#extensions-container .extensions-app .dnn-persona-bar-page-body .collapse-section {\n  padding: 20px 25px;\n  font-weight: bold;\n  cursor: pointer;\n  color: #6F7273;\n}\n", ""]);



/***/ }),
/* 71 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "._26fEWzZ5coxK40UKU6c678 .dnn-switch-container {\n  width: 100%;\n}\n._26fEWzZ5coxK40UKU6c678 .dnn-switch-container .dnn-switch {\n  float: right;\n}\n", ""]);

// Exports
exports.locals = {
	"editAuthenticationSystem": "_26fEWzZ5coxK40UKU6c678"
};

/***/ }),
/* 72 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extensions-app .extension-form ._1QNubhbUBvzw3H5HbQ7Mn7 {\n  padding: 0;\n  margin-top: 32px;\n}\n.extensions-app .extension-form ._1QNubhbUBvzw3H5HbQ7Mn7 .selector-box {\n  padding: 0;\n  height: 225px;\n}\n.extensions-app .extension-form ._1QNubhbUBvzw3H5HbQ7Mn7 .selector-box h6 {\n  margin-bottom: 5px;\n}\n.extensions-app .extension-form ._1QNubhbUBvzw3H5HbQ7Mn7 .selector-box ul li {\n  padding: 15px 20px;\n}\n.extensions-app .extension-form ._1QNubhbUBvzw3H5HbQ7Mn7 .selector-box ul li:hover {\n  background-color: #EFF0F0;\n  cursor: pointer;\n}\n.extensions-app .extension-form ._1QNubhbUBvzw3H5HbQ7Mn7 .selector-box ul li.selected {\n  color: #21A3DA;\n}\n.extensions-app .extension-form ._1QNubhbUBvzw3H5HbQ7Mn7 .selector-controls {\n  padding: 0 15px;\n  margin-top: 21px;\n}\n.extensions-app .extension-form ._1QNubhbUBvzw3H5HbQ7Mn7 .selector-controls .move-item {\n  width: 30px;\n  height: 30px;\n  padding: 7px;\n  border: 1px solid #c8c8c8;\n}\n.extensions-app .extension-form ._1QNubhbUBvzw3H5HbQ7Mn7 .selector-controls .move-item.double-right {\n  margin-top: 105px;\n}\n", ""]);

// Exports
exports.locals = {
	"assignedSelector": "_1QNubhbUBvzw3H5HbQ7Mn7"
};

/***/ }),
/* 73 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extensions-app .extension-form .module-controls {\n  padding: 10px;\n}\n.extensions-app .extension-form .module-controls .header-container .box-title {\n  text-transform: none;\n  font-size: 13px;\n  font-weight: bold;\n  margin-bottom: 10px;\n}\n.extensions-app .extension-form .module-controls .module-controls-table {\n  padding: 0;\n  border: 1px solid #C8C8C8;\n  margin-top: 25px;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .module-control-row:first-child {\n  border: none;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .module-control-title-header,\n.extensions-app .extension-form .module-controls .module-controls-table .module-control-source-header {\n  border-top: 1px solid #C8C8C8;\n  text-transform: uppercase;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .edit-module-control > div {\n  float: left;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .edit-module-control-box {\n  padding: 0;\n  margin-top: 20px;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .edit-module-control-box .dnn-dropdown-with-error .dropdown-tooltip-container {\n  margin-bottom: 32px;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .edit-module-control-box .dnn-switch-container {\n  width: 100%;\n  margin-bottom: 32px;\n  padding-top: 15px;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .edit-module-control-box .dnn-switch-container .switch-label {\n  max-width: 50%;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .edit-module-control-box .dnn-switch-container .dnn-switch {\n  float: right;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .add-control-box > div {\n  float: left;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .module-control-row {\n  padding: 10px 15px;\n  border-top: 1px solid #C8C8C8;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .module-control-row .module-control-title,\n.extensions-app .extension-form .module-controls .module-controls-table .module-control-row .module-control-source {\n  padding: 0;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .module-control-row .action-buttons {\n  padding: 0;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .module-control-row .action-buttons > div {\n  float: right;\n  cursor: pointer;\n}\n.extensions-app .extension-form .module-controls .module-controls-table .module-control-row .action-buttons > div > svg {\n  width: 15px;\n  height: 15px;\n}\n", ""]);



/***/ }),
/* 74 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extensions-app .module-definitions .module-definitions-table {\n  padding: 0;\n  border: 1px solid #C8C8C8;\n  margin-top: 25px;\n}\n.extensions-app .module-definitions .module-definitions-table .module-definition-row:first-child {\n  border: none;\n}\n.extensions-app .module-definitions .add-module-definition-box {\n  padding: 15px 25px;\n}\n", ""]);



/***/ }),
/* 75 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(76);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 76 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extension-form .module-definition-row {\n  padding: 10px 15px;\n  border-top: 1px solid #C8C8C8;\n}\n.extension-form .module-definition-row .module-definition-name {\n  padding: 0;\n}\n.extension-form .module-definition-row .edit-module-definition > div {\n  float: left;\n}\n.extension-form .module-definition-row .edit-module-definition .edit-module-definition-box {\n  padding: 0;\n  margin-top: 20px;\n}\n.extension-form .module-definition-row .action-buttons {\n  padding: 0;\n}\n.extension-form .module-definition-row .action-buttons > div {\n  float: right;\n  cursor: pointer;\n}\n.extension-form .module-definition-row .action-buttons > div > svg {\n  margin-left: 5px;\n  width: 15px;\n  height: 15px;\n}\n", ""]);



/***/ }),
/* 77 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3qNM9H-_KXBc6PTORGlhd6 .box-title {\n  font-size: 15px;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .premium-module .dnn-switch-container {\n  width: 50%;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .premium-module .dnn-switch-container .dnn-switch {\n  float: right;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .module-definitions .header-container {\n  padding: 0;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .module-definitions .header-container .box-title {\n  float: left;\n  margin-bottom: 20px;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .module-definitions .header-container a.add-button {\n  float: right;\n  cursor: pointer;\n  color: #959695;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .module-definitions .header-container a.add-button.add-active,\n._3qNM9H-_KXBc6PTORGlhd6 .module-definitions .header-container a.add-button:hover {\n  color: #1E88C3;\n  font-weight: bold;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .module-definitions .header-container a.add-button.add-active svg,\n._3qNM9H-_KXBc6PTORGlhd6 .module-definitions .header-container a.add-button:hover svg {\n  fill: #1E88C3;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .module-definitions .header-container a.add-button svg {\n  width: 16px;\n  height: 16px;\n  fill: #959695;\n  float: left;\n  margin-right: 5px;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .with-right-border .dnn-switch-container {\n  width: 100%;\n  margin-bottom: 32px;\n  padding-top: 15px;\n}\n._3qNM9H-_KXBc6PTORGlhd6 .with-right-border .dnn-switch-container .dnn-switch {\n  float: right;\n}\n", ""]);

// Exports
exports.locals = {
	"editModule": "_3qNM9H-_KXBc6PTORGlhd6"
};

/***/ }),
/* 78 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n", ""]);



/***/ }),
/* 79 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extensions-app ._1tBUwl63kugrawQ_bX14r3 .js-library-info-table {\n  border: 1px solid #C8C8C8;\n  padding: 0;\n}\n.extensions-app ._1tBUwl63kugrawQ_bX14r3 .js-library-info-table .js-library-info-row .js-library-info-name {\n  padding: 0;\n}\n.extensions-app ._1tBUwl63kugrawQ_bX14r3 .js-library-info-table .js-library-info-row .js-library-info-version {\n  text-align: right;\n  padding: 0;\n}\n.extensions-app ._1tBUwl63kugrawQ_bX14r3 .js-library-info-table .js-library-info-name-header {\n  border-bottom: 1px solid #C8C8C8;\n}\n.extensions-app ._1tBUwl63kugrawQ_bX14r3 .js-library-info-table .js-library-info-version-header {\n  border-bottom: 1px solid #C8C8C8;\n}\n.extensions-app ._1tBUwl63kugrawQ_bX14r3 .dnn-label label {\n  float: left;\n}\n.extensions-app ._1tBUwl63kugrawQ_bX14r3 .jslibrary-table-separator {\n  margin: 15px 0;\n  float: left;\n  width: 100%;\n}\n", ""]);

// Exports
exports.locals = {
	"editJSLibrary": "_1tBUwl63kugrawQ_bX14r3"
};

/***/ }),
/* 80 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n", ""]);



/***/ }),
/* 81 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n", ""]);



/***/ }),
/* 82 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3D3HvOWafPEY5Ixb4U-ygC .dnn-switch-container {\n  width: 100%;\n}\n._3D3HvOWafPEY5Ixb4U-ygC .dnn-switch-container .dnn-switch {\n  float: right;\n}\n", ""]);

// Exports
exports.locals = {
	"editSkinObj": "_3D3HvOWafPEY5Ixb4U-ygC"
};

/***/ }),
/* 83 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._22apsDvbaX4z5qMJK4CHn- .extension-type {\n  margin-bottom: 34px;\n}\n._22apsDvbaX4z5qMJK4CHn- .version-dropdown {\n  width: 30%;\n  box-sizing: border-box;\n  margin-right: 5%;\n  vertical-align: bottom;\n}\n._22apsDvbaX4z5qMJK4CHn- .version-dropdown:last-child {\n  margin-right: 0;\n}\n", ""]);

// Exports
exports.locals = {
	"basicPackageInformation": "_22apsDvbaX4z5qMJK4CHn-"
};

/***/ }),
/* 84 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3PGrqkxHWNUTbpReJIM-00 {\n  box-sizing: border-box;\n}\n._3PGrqkxHWNUTbpReJIM-00 .dnn-grid-cell > hr {\n  border: none;\n  height: 1px;\n  color: #C8C8C8;\n  background-color: #C8C8C8;\n}\n._3PGrqkxHWNUTbpReJIM-00 .dnn-grid-cell .extension-type {\n  margin-bottom: 32px;\n}\n._3PGrqkxHWNUTbpReJIM-00 .dnn-grid-cell .version-dropdown {\n  width: 30%;\n  box-sizing: border-box;\n  margin-right: 5%;\n  vertical-align: bottom;\n}\n._3PGrqkxHWNUTbpReJIM-00 .dnn-grid-cell .version-dropdown:last-child {\n  margin-right: 0;\n}\n._3PGrqkxHWNUTbpReJIM-00 .dnn-grid-cell.add-mode {\n  padding: 0;\n}\n._3PGrqkxHWNUTbpReJIM-00 .new-extension-box {\n  padding: 25px 45px;\n}\n._3PGrqkxHWNUTbpReJIM-00 .box-title-container {\n  padding-top: 0;\n  padding-bottom: 0;\n}\n._3PGrqkxHWNUTbpReJIM-00 .box-title-container .box-title {\n  text-transform: uppercase;\n  font-weight: bold;\n  font-size: 14px;\n  margin-bottom: 0;\n  margin-top: 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"newExtensionModal": "_3PGrqkxHWNUTbpReJIM-00"
};

/***/ }),
/* 85 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1jxmWIlb7Ard2L2jnc-lkn {\n  position: relative;\n}\n._1jxmWIlb7Ard2L2jnc-lkn .dnn-dropdown-with-error {\n  width: 100%;\n  margin-bottom: 32px;\n}\n._1jxmWIlb7Ard2L2jnc-lkn .add-new-folder-box {\n  position: absolute;\n  z-index: 50;\n  background: white;\n  -webkit-box-shadow: 0px 0px 20px 0px rgba(0, 0, 0, 0.2);\n  -moz-box-shadow: 0px 0px 20px 0px rgba(0, 0, 0, 0.2);\n  box-shadow: 0px 0px 20px 0px rgba(0, 0, 0, 0.2);\n}\n._1jxmWIlb7Ard2L2jnc-lkn .add-new-folder-box .new-folder-title {\n  font-size: 15px;\n  font-weight: bold;\n}\n._1jxmWIlb7Ard2L2jnc-lkn .add-new-folder-box .new-folder-buttons {\n  text-align: center;\n}\n._1jxmWIlb7Ard2L2jnc-lkn .add-new-folder-box .new-folder-buttons button:first-child {\n  margin-right: 10px;\n}\n._1jxmWIlb7Ard2L2jnc-lkn .folder-dropdown.hidden {\n  opacity: 0;\n}\n", ""]);

// Exports
exports.locals = {
	"folderDropdown": "_1jxmWIlb7Ard2L2jnc-lkn"
};

/***/ }),
/* 86 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2lmyDgm66DSn3Kib-Kvw0p {\n  padding: 0;\n}\n._2lmyDgm66DSn3Kib-Kvw0p .dnn-radio-buttons {\n  margin-top: 32px;\n}\n._2lmyDgm66DSn3Kib-Kvw0p .dnn-radio-buttons > label {\n  font-weight: bold;\n}\n._2lmyDgm66DSn3Kib-Kvw0p .dnn-switch-container {\n  width: 100%;\n  margin-top: -11px;\n}\n._2lmyDgm66DSn3Kib-Kvw0p .dnn-switch-container .dnn-switch {\n  float: right;\n}\n", ""]);

// Exports
exports.locals = {
	"fromControl": "_2lmyDgm66DSn3Kib-Kvw0p"
};

/***/ }),
/* 87 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._696Dmo--EzMz5VxtmZ1ut {\n  padding: 0;\n}\n._696Dmo--EzMz5VxtmZ1ut .dnn-radio-buttons {\n  margin-top: 32px;\n}\n._696Dmo--EzMz5VxtmZ1ut .dnn-radio-buttons > label {\n  font-weight: bold;\n}\n._696Dmo--EzMz5VxtmZ1ut .dnn-switch-container {\n  width: 100%;\n  margin-top: -11px;\n}\n._696Dmo--EzMz5VxtmZ1ut .dnn-switch-container .dnn-switch {\n  float: right;\n}\n", ""]);

// Exports
exports.locals = {
	"fromManifest": "_696Dmo--EzMz5VxtmZ1ut"
};

/***/ }),
/* 88 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1YgBadr6ihwfo2C32VtGjX {\n  padding: 0;\n}\n._1YgBadr6ihwfo2C32VtGjX .dnn-radio-buttons {\n  margin-top: 32px;\n}\n._1YgBadr6ihwfo2C32VtGjX .dnn-radio-buttons > label {\n  font-weight: bold;\n}\n._1YgBadr6ihwfo2C32VtGjX .dnn-switch-container {\n  width: 100%;\n  margin-top: -11px;\n}\n._1YgBadr6ihwfo2C32VtGjX .dnn-switch-container .dnn-switch {\n  float: right;\n}\n", ""]);

// Exports
exports.locals = {
	"fromNew": "_1YgBadr6ihwfo2C32VtGjX"
};

/***/ }),
/* 89 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1o5-ML7I38Hsao6esBL70Q {\n  box-sizing: border-box;\n}\n._1o5-ML7I38Hsao6esBL70Q * {\n  box-sizing: border-box;\n}\n._1o5-ML7I38Hsao6esBL70Q .box-title {\n  margin-bottom: 25px;\n  text-transform: uppercase;\n}\n._1o5-ML7I38Hsao6esBL70Q .new-module-box {\n  padding: 25px;\n}\n._1o5-ML7I38Hsao6esBL70Q .new-module-box .create-new-module-dropdown {\n  padding-bottom: 45px;\n  border-bottom: 1px solid #C8C8C8;\n}\n._1o5-ML7I38Hsao6esBL70Q .new-module-box .new-module-dropdown-container .dnn-ui-common-button {\n  margin-left: auto;\n  margin-right: auto;\n  display: block;\n  margin-top: 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"newModuleModal": "_1o5-ML7I38Hsao6esBL70Q"
};

/***/ }),
/* 90 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(91);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 91 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extension-install-logs > h6 {\n  margin-bottom: 10px;\n  text-transform: uppercase;\n  display: inline-block;\n}\n.extension-install-logs > p {\n  margin-bottom: 25px;\n}\n.extension-install-logs .install-error-tooltip {\n  display: inline-block;\n  margin-left: 10px;\n  position: relative;\n  top: 5px;\n}\n.extension-install-logs .package-installation-report {\n  padding: 25px;\n}\n.extension-install-logs .package-installation-report p {\n  word-wrap: break-word;\n}\n.extension-install-logs .package-installation-report p.failure,\n.extension-install-logs .package-installation-report p.error {\n  color: #EA2134;\n  font-weight: bold;\n}\n.extension-install-logs .package-installation-report p.startjob,\n.extension-install-logs .package-installation-report p.endjob {\n  color: #000;\n  font-weight: bold;\n}\n", ""]);



/***/ }),
/* 92 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3AB740VT1v1WPjY6HyKV8E > .package-information-header > h6 {\n  margin-bottom: 10px;\n  text-transform: uppercase;\n}\n._3AB740VT1v1WPjY6HyKV8E > .package-information-header > p {\n  margin-bottom: 25px;\n}\n", ""]);

// Exports
exports.locals = {
	"pkgInformationBox": "_3AB740VT1v1WPjY6HyKV8E"
};

/***/ }),
/* 93 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(94);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 94 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, ".extensions-app .extension-form .release-notes {\n  padding: 50px;\n}\n.extensions-app .release-notes .read-only-release-notes {\n  padding: 25px;\n}\n.extensions-app .release-notes > h6 {\n  margin-bottom: 10px;\n  text-transform: uppercase;\n}\n.extensions-app .release-notes > p {\n  margin-bottom: 25px;\n}\n.extensions-app .release-notes .dnn-multi-line-input-with-error .dnn-label {\n  text-transform: uppercase;\n  padding-bottom: 5px;\n}\n", ""]);



/***/ }),
/* 95 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(96);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 96 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, ".extensions-app .extension-form .extension-license {\n  padding: 50px;\n}\n.extensions-app .extension-license {\n  padding: 50px;\n}\n.extensions-app .extension-license > h6 {\n  margin-bottom: 10px;\n  text-transform: uppercase;\n}\n.extensions-app .extension-license > p {\n  margin-bottom: 25px;\n}\n.extensions-app .extension-license .read-only-license {\n  padding: 25px;\n}\n.extensions-app .extension-license .dnn-multi-line-input-with-error .dnn-label {\n  text-transform: uppercase;\n  padding-bottom: 5px;\n}\n.extensions-app .install-extension-box .extension-license .dnn-checkbox-container {\n  float: none;\n  text-align: center;\n}\n", ""]);



/***/ }),
/* 97 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<polygon fill=\"currentColor\" points=\"1793.7,585.8 1623.9,416.1 756.6,1283.4 424.4,951.3 254.6,1121 756.6,1623 926.4,1453.2 926.4,1453.2 \"/>\r\n</svg>\r\n");

/***/ }),
/* 98 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<g>\r\n\t<path fill=\"currentColor\" d=\"M662.6,1565.4L483,1385.8l360.6-360.6L480.1,661.8l182.6-182.6l363.5,363.4l359-359l179.6,179.6l-359,359l362,362\r\n\t\tl-182.6,182.6l-362-362L662.6,1565.4z\"/>\r\n</g>\r\n</svg>\r\n");

/***/ }),
/* 99 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.dnn-package-upload {\n  float: left;\n  width: 100%;\n  height: 215px;\n  background: #FBFCFC;\n  outline: 1px solid #959695;\n  position: relative;\n  border: 12px solid #FFFFFF;\n  box-sizing: content-box;\n}\n.dnn-package-upload * {\n  box-sizing: content-box;\n}\n.dnn-package-upload.viewing-log {\n  outline: none;\n  border: none;\n}\n.dnn-package-upload .dropzone-container {\n  position: absolute;\n  width: 100%;\n  height: 100%;\n  z-index: 3;\n  background-color: #EFF0F0;\n}\n.dnn-package-upload .no-valid-manifest {\n  background-color: #FBFCFC;\n  position: absolute;\n  top: -13px;\n  left: -13px;\n  z-index: 3;\n  padding: 45px 25px;\n  text-align: center;\n}\n.dnn-package-upload .no-valid-manifest > p {\n  margin-bottom: 25px;\n}\n.dnn-package-upload .no-valid-manifest .dnn-radio-buttons {\n  margin: 0 auto;\n  float: none;\n  width: auto;\n  display: table;\n}\n.dnn-package-upload.uploading .dropzone-container {\n  background-color: transparent;\n}\n.dnn-package-upload.uploading .dropzone-container .overlay {\n  opacity: 0;\n}\n.dnn-package-upload.uploading .dropzone-container .overlay.hover {\n  opacity: 1;\n}\n.dnn-package-upload.uploading .dropzone-container .overlay:hover {\n  opacity: 1;\n  background: rgba(30, 136, 195, 0.9);\n}\n.dnn-package-upload.already-installed .dropzone-container {\n  z-index: 1;\n}\n.dnn-package-upload .overlay {\n  position: absolute;\n  background: rgba(30, 136, 195, 0);\n  top: -12px;\n  bottom: -12px;\n  left: -12px;\n  right: -12px;\n  text-align: center;\n  transition: 0.18s;\n  z-index: 1;\n}\n.dnn-package-upload .overlay.hover {\n  background: rgba(30, 136, 195, 0.6);\n  opacity: 1;\n}\n.dnn-package-upload .overlay.hover * {\n  pointer-events: none;\n}\n.dnn-package-upload .overlay.hover span {\n  color: white;\n}\n.dnn-package-upload .overlay.hover .buttons .button.upload {\n  color: white;\n}\n.dnn-package-upload .overlay:hover {\n  background: rgba(30, 136, 195, 0.6);\n}\n.dnn-package-upload .overlay:hover span {\n  color: white;\n}\n.dnn-package-upload .overlay:hover .buttons:hover .button {\n  color: #C8C8C8;\n}\n.dnn-package-upload .overlay:hover .buttons:hover .button:hover {\n  color: white;\n}\n.dnn-package-upload .overlay:hover .buttons .button {\n  color: white;\n}\n.dnn-package-upload .overlay span {\n  text-align: center;\n  margin-top: 25px;\n  float: left;\n  width: 100%;\n  box-sizing: border-box;\n  color: #6F7273;\n  transition: 0.18s;\n  font-size: 14px;\n}\n.dnn-package-upload .overlay .buttons {\n  margin: auto;\n  height: 32px;\n  margin-top: 60px;\n  width: 201px;\n  transition: 0.18s;\n}\n.dnn-package-upload .overlay .buttons .button {\n  float: left;\n  width: 32px;\n  height: 32px;\n  padding: 0 17.5px;\n  box-sizing: content-box;\n  color: #C8C8C8;\n  cursor: pointer;\n  position: relative;\n  overflow: hidden;\n}\n.dnn-package-upload .overlay .buttons .button input[type=\"file\"] {\n  position: absolute;\n  top: -100px;\n  bottom: -100px;\n  left: -100px;\n  right: -100px;\n  opacity: 0;\n  cursor: pointer;\n  min-height: 200px;\n}\n.dnn-package-upload .overlay .buttons .button svg {\n  width: 32px;\n  height: 32px;\n}\n.dnn-package-upload .overlay .buttons .button:first-of-type {\n  margin-left: 0px;\n}\n.dnn-package-upload .file-upload-container {\n  position: absolute;\n  top: -12px;\n  bottom: -12px;\n  left: -12px;\n  right: -12px;\n  background: white;\n  padding: 14px 22px;\n  z-index: 2;\n}\n.dnn-package-upload .file-upload-container h4 {\n  margin-bottom: 8px;\n  color: #46292B;\n}\n.dnn-package-upload .file-upload-container .textarea-container {\n  float: left;\n  width: 100%;\n  height: 90px;\n  border: 1px solid #959695;\n  position: relative;\n}\n.dnn-package-upload .file-upload-container .textarea-container textarea {\n  resize: none;\n  width: 100%;\n  height: 64px;\n  border: none;\n  outline: none !important;\n  padding: 10px;\n  box-sizing: border-box;\n  color: #46292B;\n}\n.dnn-package-upload .file-upload-container span {\n  float: right;\n  margin-right: 10px;\n  color: #1E88C3;\n}\n.dnn-package-upload .file-upload-container .drop-down {\n  width: 100%;\n  float: left;\n  height: 30px;\n  border: 1px solid #959695;\n  margin-bottom: 16px;\n  position: relative;\n}\n.dnn-package-upload .file-upload-container .drop-down:after {\n  content: \"\";\n  position: absolute;\n  right: 8px;\n  top: 12px;\n  width: 0;\n  height: 0;\n  pointer-events: none;\n  border-left: 6px solid transparent;\n  border-right: 6px solid transparent;\n  border-top: 6px solid #6F7273;\n}\n.install-failure-logs {\n  z-index: 10000;\n  background-color: white;\n}\n.install-failure-logs .package-installation-report {\n  padding: 25px;\n}\n.install-failure-logs .package-installation-report p {\n  word-wrap: break-word;\n}\n.install-failure-logs .package-installation-report p.failure {\n  color: #EA2134;\n  font-weight: bold;\n}\n.install-failure-logs .package-installation-report p.startjob,\n.install-failure-logs .package-installation-report p.endjob {\n  color: #000;\n  font-weight: bold;\n}\n.install-failure-logs .package-installation-report .logs-unknown-error ul {\n  padding-left: 40px;\n  display: block;\n  margin-top: 10px;\n}\n.dnn-package-upload .file-upload-container.dnn-upload-bar {\n  padding: 12px;\n  transition: opacity 0.4s 0.6s;\n}\n.dnn-package-upload .file-upload-container.dnn-upload-bar .upload-bar-container {\n  color: #1E88C3;\n  float: left;\n  width: 100%;\n  height: 100%;\n  border: 1px solid #1E88C3;\n  background: #f4f6f7;\n  padding: 5px 20px;\n  box-sizing: border-box;\n}\n.dnn-package-upload .file-upload-container.dnn-upload-bar .upload-bar-container .upload-file-name {\n  margin-bottom: 10px;\n}\n.dnn-package-upload .file-upload-container.dnn-upload-bar .upload-bar-container .upload-icon {\n  width: 42px;\n  height: 42px;\n  margin: auto;\n}\n.dnn-package-upload .file-upload-container.dnn-upload-bar .upload-bar-container .upload-icon svg {\n  margin-top: 8px;\n  margin-bottom: 3px;\n}\n.dnn-package-upload .file-upload-container.dnn-upload-bar .upload-bar-container h4 {\n  color: #1E88C3;\n  margin: auto;\n  text-align: center;\n  font-size: 14px;\n  margin: 15px 10px 10px;\n}\n.dnn-package-upload .file-upload-container.dnn-upload-bar .upload-bar-container .upload-bar-box {\n  position: relative;\n  float: left;\n  width: 100%;\n  height: 4px;\n  background: #C8C8C8;\n  margin-top: 5px;\n}\n.dnn-package-upload .file-upload-container.dnn-upload-bar .upload-bar-container .upload-bar-box .upload-bar {\n  width: 0;\n  float: left;\n  background: #1E88C3;\n  height: 100%;\n  left: 0px;\n  transition: 0.1s;\n}\n.dnn-package-upload .file-upload-container.upload-error {\n  z-index: 3;\n}\n.dnn-package-upload .file-upload-container.upload-error .upload-bar-container {\n  border: 1px solid #EA2134;\n  color: #EA2134;\n}\n.dnn-package-upload .file-upload-container.upload-error .upload-bar-container h4 {\n  color: #EA2134;\n}\n.dnn-package-upload .file-upload-container.upload-error .upload-bar-container .view-log-or-try-again {\n  text-align: center;\n}\n.dnn-package-upload .file-upload-container.upload-error .upload-bar-container .view-log-or-try-again span {\n  float: none;\n  color: #1E88C3;\n  cursor: pointer;\n  margin-left: 10px;\n}\n.dnn-package-upload .file-upload-container.upload-error .upload-bar-container .upload-bar-box {\n  background: #EA2134;\n}\n.dnn-package-upload .already-installed {\n  position: absolute;\n  top: -12px;\n  bottom: -12px;\n  left: -12px;\n  right: -12px;\n  background: white;\n  padding: 14px 22px;\n  z-index: 4;\n}\n.dnn-package-upload .already-installed .already-installed-container {\n  color: #959695;\n  float: left;\n  width: 100%;\n  height: 100%;\n  border: 1px solid #959695;\n  background: #EFF0F0;\n  padding: 5px 20px;\n  box-sizing: border-box;\n}\n.dnn-package-upload .already-installed .already-installed-container .upload-file-name {\n  margin-bottom: 10px;\n}\n.dnn-package-upload .already-installed .already-installed-container .upload-icon {\n  width: 42px;\n  margin: auto;\n}\n.dnn-package-upload .already-installed .already-installed-container .upload-icon svg {\n  margin-top: 8px;\n  margin-bottom: 3px;\n  width: 42px;\n  height: 42px;\n}\n.dnn-package-upload .already-installed .already-installed-container h4 {\n  color: #959695;\n  margin: auto;\n  text-align: center;\n  font-size: 12px;\n  margin: 10px 10px 15px;\n}\n.dnn-package-upload .already-installed .already-installed-container .repair-or-install {\n  text-align: center;\n}\n.dnn-package-upload .already-installed .already-installed-container .repair-or-install span {\n  color: #1E88C3;\n  cursor: pointer;\n}\n", ""]);



/***/ }),
/* 100 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._32Jdz9hXwZGU_hutbmJvlk {\n  box-sizing: border-box;\n}\n._32Jdz9hXwZGU_hutbmJvlk .install-extension-box {\n  padding: 25px 45px;\n}\n._32Jdz9hXwZGU_hutbmJvlk .install-extension-box .box-title {\n  font-size: 15px;\n  text-transform: uppercase;\n  margin-bottom: 25px;\n}\n._32Jdz9hXwZGU_hutbmJvlk .install-extension-box .upload-package-box.dnn-grid-cell {\n  padding: 0;\n  margin: 15px auto 25px;\n}\n._32Jdz9hXwZGU_hutbmJvlk .install-extension-box .dnn-checkbox-container {\n  float: left;\n  margin-top: 25px;\n  margin-bottom: 25px;\n}\n._32Jdz9hXwZGU_hutbmJvlk .install-extension-box .modal-pagination {\n  text-align: center;\n}\n._32Jdz9hXwZGU_hutbmJvlk .install-extension-box .package-already-exists > p {\n  padding: 0 25px;\n}\n._32Jdz9hXwZGU_hutbmJvlk .install-extension-box .package-already-exists > p.package-exists-help-text {\n  margin-bottom: 15px;\n}\n._32Jdz9hXwZGU_hutbmJvlk .install-extension-box .package-already-exists > p.package-exists-error {\n  margin-bottom: 15px;\n}\n", ""]);

// Exports
exports.locals = {
	"installExtensionModal": "_32Jdz9hXwZGU_hutbmJvlk"
};

/***/ }),
/* 101 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "._3PStCy7pXjaO7IAwTtdTwd .dnn-switch-container {\n  width: 100%;\n}\n._3PStCy7pXjaO7IAwTtdTwd .dnn-switch-container .dnn-switch {\n  float: right;\n}\n", ""]);

// Exports
exports.locals = {
	"editAuthenticationSystem": "_3PStCy7pXjaO7IAwTtdTwd"
};

/***/ }),
/* 102 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(103);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(7)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 103 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, ".module-settings {\n  padding: 15px 50px;\n}\n.module-settings .actions-row {\n  margin-top: 20px;\n  text-align: center;\n}\n.module-settings .actions-row button[role=\"primary\"] {\n  margin-left: 10px;\n}\n", ""]);



/***/ }),
/* 104 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.extensions-app ._2f5H024uZl1Vx9M6McgnE3 {\n  box-sizing: border-box;\n  padding: 0;\n}\n.extensions-app ._2f5H024uZl1Vx9M6McgnE3 * {\n  box-sizing: border-box;\n}\n.extensions-app ._2f5H024uZl1Vx9M6McgnE3 .box-title {\n  margin-bottom: 25px;\n  text-transform: uppercase;\n}\n.extensions-app ._2f5H024uZl1Vx9M6McgnE3 .dnn-tabs.react-tabs.primary > ul li {\n  padding: 17px 28px;\n  margin-bottom: -3px;\n}\n.extensions-app ._2f5H024uZl1Vx9M6McgnE3 .dnn-tabs.react-tabs.primary > ul li > span {\n  float: left;\n}\n.extensions-app ._2f5H024uZl1Vx9M6McgnE3 .dnn-tabs.react-tabs.primary > ul li .dnn-ui-common-tooltip {\n  float: right;\n  margin-left: 5px;\n}\n.extensions-app ._2f5H024uZl1Vx9M6McgnE3 .dnn-tabs.react-tabs.primary > ul li .dnn-ui-common-tooltip svg {\n  width: 16px;\n  height: 16px;\n}\n.extensions-app ._2f5H024uZl1Vx9M6McgnE3 .package-information-box .create-new-module-dropdown {\n  padding-bottom: 45px;\n  border-bottom: 1px solid #C8C8C8;\n}\n", ""]);

// Exports
exports.locals = {
	"editExtension": "_2f5H024uZl1Vx9M6McgnE3"
};

/***/ }),
/* 105 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .no-padding {\n  padding: 0;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .box-title {\n  font-size: 13px;\n  text-transform: uppercase;\n  font-weight: bold;\n  margin-bottom: 16px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .box-subtitle {\n  margin-bottom: 32px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .package-manifest-step {\n  padding: 0;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .package-manifest-step .package-manifest-info .existing-manifest-dropdown {\n  width: 50%;\n  margin-bottom: 32px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .package-manifest-step .package-manifest-info .package-manifest-header {\n  margin-top: 25px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .package-manifest-step .package-manifest-info .dnn-switch-container {\n  width: 50%;\n  margin-bottom: 16px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .package-manifest-step .package-manifest-info .dnn-switch-container .dnn-switch {\n  float: right;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-files-step .create-package-folder {\n  padding: 0 15px 0 0;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-files-step .create-package-folder .create-package-folder-input {\n  width: 100%;\n  margin: 0;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-files-step .create-package-folder .refresh-file-list-button {\n  padding: 0 0 0 15px;\n  position: relative;\n  height: 58px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-files-step .create-package-folder .refresh-file-list-button .dnn-ui-common-button {\n  position: absolute;\n  width: 100%;\n  bottom: 4px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-files-step .package-files-container {\n  margin-top: 32px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-files-step .package-files-container .package-files {\n  height: 250px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-assemblies-step .package-assemblies-container .package-assemblies {\n  height: 250px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-manifest-step .package-manifest-container .package-manifest {\n  width: 100%;\n  height: 250px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .create-package-step .using-existing-manifest {\n  margin-top: 25px;\n  margin-bottom: 40px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .create-package-step .using-existing-manifest .dnn-switch-container {\n  width: 50%;\n  margin-bottom: 16px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .create-package-step .using-existing-manifest .dnn-switch-container .dnn-switch {\n  float: right;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .create-package-step .with-right-border {\n  margin-top: 25px;\n  margin-bottom: 40px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .create-package-step .with-right-border > div.dnn-grid-cell {\n  padding: 0;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .create-package-step .with-right-border > div .left-side {\n  padding: 25px 25px 0 0;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .create-package-step .with-right-border > div .right-side {\n  padding: 25px 0 0 25px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .create-package-step .with-right-border > div .dnn-switch-container {\n  width: 100%;\n  margin-bottom: 16px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .create-package-step .with-right-border > div .dnn-switch-container .dnn-switch {\n  float: right;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-logs-step .package-logs-container {\n  margin-top: 32px;\n}\n._18Z9_BBcYdBX-wuuFyOWrs .create-package-wizard .review-logs-step .package-logs-container .package-creation-report {\n  padding: 25px 45px;\n}\n", ""]);

// Exports
exports.locals = {
	"createPackage": "_18Z9_BBcYdBX-wuuFyOWrs"
};

/***/ }),
/* 106 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(6)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._22rwNkED6wFw6phRb5JL2u .dnn-persona-bar-page-body .delete-extension-box .dnn-grid-system.with-right-border.top-half {\n  padding-bottom: 0;\n}\n._22rwNkED6wFw6phRb5JL2u .dnn-persona-bar-page-body .delete-extension-box .release-notes,\n._22rwNkED6wFw6phRb5JL2u .dnn-persona-bar-page-body .delete-extension-box .extension-license {\n  padding: 10px 25px 15px;\n}\n._22rwNkED6wFw6phRb5JL2u .dnn-persona-bar-page-body .delete-extension-box .delete-files-box {\n  text-align: center;\n  margin-top: 25px;\n}\n", ""]);

// Exports
exports.locals = {
	"DeleteExtension": "_22rwNkED6wFw6phRb5JL2u"
};

/***/ }),
/* 107 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);

// EXTERNAL MODULE: external "window.dnn.nodeModules.React"
var external_window_dnn_nodeModules_React_ = __webpack_require__(0);
var external_window_dnn_nodeModules_React_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_React_);

// EXTERNAL MODULE: D:/a/1/s/node_modules/prop-types/index.js
var prop_types = __webpack_require__(2);
var prop_types_default = /*#__PURE__*/__webpack_require__.n(prop_types);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactRedux"
var external_window_dnn_nodeModules_ReactRedux_ = __webpack_require__(5);

// EXTERNAL MODULE: ./src/constants/actionTypes/index.js + 7 modules
var actionTypes = __webpack_require__(3);

// CONCATENATED MODULE: ./src/actions/pagination.js

var paginationActions = {
  loadTab: function loadTab(index, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["f" /* pagination */].LOAD_TAB_DATA,
        payload: {
          index: index
        }
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  }
};
/* harmony default export */ var pagination = (paginationActions);
// CONCATENATED MODULE: ./src/actions/visiblePanel.js

var visiblePanelActions = {
  selectPanel: function selectPanel(panel) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["g" /* visiblePanel */].SELECT_PANEL,
        payload: {
          selectedPage: panel
        }
      });
    };
  }
};
/* harmony default export */ var visiblePanel = (visiblePanelActions);
// EXTERNAL MODULE: ./src/utils/index.jsx
var utils = __webpack_require__(4);

// CONCATENATED MODULE: ./src/services/extensionService.js
function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }



function mapPackageInformation(extensionBeingUpdated, addMode) {
  var _extensionBeingUpdated = {
    Name: extensionBeingUpdated.name.value,
    FriendlyName: extensionBeingUpdated.friendlyName.value,
    Description: extensionBeingUpdated.description.value,
    Version: extensionBeingUpdated.version.value,
    Owner: extensionBeingUpdated.owner.value,
    Url: extensionBeingUpdated.url.value,
    Organization: extensionBeingUpdated.organization.value,
    Email: extensionBeingUpdated.email.value,
    License: extensionBeingUpdated.releaseNotes && extensionBeingUpdated.license.value,
    ReleaseNotes: extensionBeingUpdated.releaseNotes && extensionBeingUpdated.releaseNotes.value
  };

  if (addMode) {
    delete _extensionBeingUpdated.License;
    delete _extensionBeingUpdated.ReleaseNotes;
    _extensionBeingUpdated.PackageType = extensionBeingUpdated.packageType.value;
  }

  return _extensionBeingUpdated;
}

function serializeQueryStringParameters(obj) {
  var s = [];

  for (var p in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, p)) {
      s.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
    }
  }

  return s.join("&");
}

var extensionService_ExtensionService =
/*#__PURE__*/
function () {
  function ExtensionService() {
    _classCallCheck(this, ExtensionService);
  }

  _createClass(ExtensionService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = utils["a" /* default */].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getInstalledPackages",
    value: function getInstalledPackages(type, callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetInstalledPackages?packageType=" + type, {}, callback);
    }
  }, {
    key: "getAvailablePackages",
    value: function getAvailablePackages(type, callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetAvailablePackages?packageType=" + type, {}, callback);
    }
  }, {
    key: "getPackageTypes",
    value: function getPackageTypes(callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetPackageTypes", {}, callback);
    }
  }, {
    key: "getAvailablePackageTypes",
    value: function getAvailablePackageTypes(callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetAvailablePackageTypes", {}, callback);
    }
  }, {
    key: "updateExtension",
    value: function updateExtension(extensionBeingUpdated, editorActions, callback) {
      var sf = this.getServiceFramework("Extensions");
      var payload = {
        packageId: extensionBeingUpdated.packageId.value,
        portalId: utils["a" /* default */].settings.portalId,
        settings: mapPackageInformation(extensionBeingUpdated),
        editorActions: editorActions
      };
      sf.post("SavePackageSettings", payload, callback);
    }
  }, {
    key: "createNewExtension",
    value: function createNewExtension(extensionBeingUpdated, editorActions, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      var payload = {
        packageId: -1,
        portalId: -1,
        settings: mapPackageInformation(extensionBeingUpdated, true),
        editorActions: editorActions
      };
      sf.post("CreateExtension", payload, callback, errorCallback);
    }
  }, {
    key: "downloadPackage",
    value: function downloadPackage(PackageType, FileName, callback) {
      var sf = this.getServiceFramework("Extensions");
      var payload = {
        PackageType: PackageType,
        FileName: FileName
      };
      sf.get("DownloadPackage", payload, callback);
    }
  }, {
    key: "installAvailablePackage",
    value: function installAvailablePackage(PackageType, FileName, callback) {
      var sf = this.getServiceFramework("Extensions");
      var payload = {
        PackageType: PackageType,
        FileName: FileName
      };
      sf.post("InstallAvailablePackage", payload, callback);
    }
  }, {
    key: "deletePackage",
    value: function deletePackage(payload, callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("DeletePackage", payload, callback);
    }
  }, {
    key: "parsePackage",
    value: function parsePackage(file, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      var formData = new FormData();
      formData.append("POSTFILE", file);
      sf.postfile("ParsePackage", formData, callback, errorCallback);
    }
  }, {
    key: "deployAvailablePackage",
    value: function deployAvailablePackage(cultureCode, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("ParseLanguagePackage?cultureCode=" + cultureCode, {}, callback, errorCallback);
    }
  }, {
    key: "installPackage",
    value: function installPackage(file, callback) {
      var sf = this.getServiceFramework("Extensions");
      var formData = new FormData();
      formData.append("POSTFILE", file);
      sf.postfile("InstallPackage", formData, callback);
    }
  }, {
    key: "createNewModule",
    value: function createNewModule(payload, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("CreateModule", payload, callback, errorCallback);
    }
  }, {
    key: "getPackageSettings",
    value: function getPackageSettings(packageId, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      var parameters = {
        siteId: utils["a" /* default */].settings.portalId,
        packageId: packageId
      };
      sf.get("GetPackageSettings?" + serializeQueryStringParameters(parameters), {}, callback, errorCallback);
    }
  }, {
    key: "getModuleCategories",
    value: function getModuleCategories(callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("getModuleCategories", {}, callback, errorCallback);
    }
  }, {
    key: "getDesktopModulePermissions",
    value: function getDesktopModulePermissions(desktopModuleId, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetDesktopModulePermissions", {
        desktopModuleId: desktopModuleId
      }, callback, errorCallback);
    }
  }, {
    key: "saveDesktopModulePermissions",
    value: function saveDesktopModulePermissions(permissions, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("saveDesktopModulePermissions", {
        permissions: permissions
      }, callback, errorCallback);
    }
  }, {
    key: "parseAvailablePackage",
    value: function parseAvailablePackage(FileName, PackageType, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("ParsePackageFile", {
        FileName: FileName,
        PackageType: PackageType
      }, callback, errorCallback);
    }
  }, {
    key: "getLocaleList",
    value: function getLocaleList(callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetLanguagesList", {}, callback, errorCallback);
    }
  }, {
    key: "getLocalePackagesList",
    value: function getLocalePackagesList(callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetAllPackagesListExceptLangPacks", {}, callback, errorCallback);
    }
  }, {
    key: "getPackageUsageFilter",
    value: function getPackageUsageFilter(callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetPackageUsageFilter", {}, callback);
    }
  }, {
    key: "getPackageUsage",
    value: function getPackageUsage(portalId, packageId, callback) {
      var sf = this.getServiceFramework("Extensions");
      var parameters = {
        portalId: portalId,
        packageId: packageId
      };
      sf.get("GetPackageUsage?" + serializeQueryStringParameters(parameters), {}, callback);
    }
  }]);

  return ExtensionService;
}();

var extensionService = new extensionService_ExtensionService();
/* harmony default export */ var services_extensionService = (extensionService);
// CONCATENATED MODULE: ./src/services/installationService.js
function installationService_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function installationService_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function installationService_createClass(Constructor, protoProps, staticProps) { if (protoProps) installationService_defineProperties(Constructor.prototype, protoProps); if (staticProps) installationService_defineProperties(Constructor, staticProps); return Constructor; }



var installationService_InstallationService =
/*#__PURE__*/
function () {
  function InstallationService() {
    installationService_classCallCheck(this, InstallationService);
  }

  installationService_createClass(InstallationService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = utils["a" /* default */].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "parsePackage",
    value: function parsePackage(file, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      var formData = new FormData();
      formData.append("POSTFILE", file);
      sf.postfile("ParsePackage", formData, callback, errorCallback);
    }
  }, {
    key: "installPackage",
    value: function installPackage(file, legacyType, isPortalPackage, callback) {
      var sf = this.getServiceFramework("Extensions");
      var formData = new FormData();
      formData.append("POSTFILE", file);

      if (legacyType && isPortalPackage) {
        sf.postfile("InstallPackage?legacySkin=" + legacyType + "&isPortalPackage=" + isPortalPackage, formData, callback, utils["a" /* default */].utilities.notifyError);
      } else if (legacyType) {
        sf.postfile("InstallPackage?legacySkin=" + legacyType, formData, callback, utils["a" /* default */].utilities.notifyError);
      } else if (isPortalPackage) {
        sf.postfile("InstallPackage?isPortalPackage=" + isPortalPackage, formData, callback, utils["a" /* default */].utilities.notifyError);
      } else {
        sf.postfile("InstallPackage", formData, callback, utils["a" /* default */].utilities.notifyError);
      }
    }
  }]);

  return InstallationService;
}();

var installationService = new installationService_InstallationService();
/* harmony default export */ var services_installationService = (installationService);
// CONCATENATED MODULE: ./src/services/folderService.js
function folderService_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function folderService_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function folderService_createClass(Constructor, protoProps, staticProps) { if (protoProps) folderService_defineProperties(Constructor.prototype, protoProps); if (staticProps) folderService_defineProperties(Constructor, staticProps); return Constructor; }



function folderService_serializeQueryStringParameters(obj) {
  var s = [];

  for (var p in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, p)) {
      s.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
    }
  }

  return s.join("&");
}

var folderService_FolderService =
/*#__PURE__*/
function () {
  function FolderService() {
    folderService_classCallCheck(this, FolderService);
  }

  folderService_createClass(FolderService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = utils["a" /* default */].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getOwnerFolders",
    value: function getOwnerFolders(callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetOwnerFolders", {}, callback, errorCallback);
    }
  }, {
    key: "getModuleFolders",
    value: function getModuleFolders(ownerFolder, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetModuleFolders?ownerFolder=" + ownerFolder, {}, callback, errorCallback);
    }
  }, {
    key: "getModuleFiles",
    value: function getModuleFiles(parameters, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetModuleFiles?" + folderService_serializeQueryStringParameters(parameters), {}, callback, errorCallback);
    }
  }, {
    key: "createNewFolder",
    value: function createNewFolder(payload, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("CreateFolder?" + folderService_serializeQueryStringParameters(payload), {}, callback, errorCallback);
    }
  }]);

  return FolderService;
}();

var folderService = new folderService_FolderService();
/* harmony default export */ var services_folderService = (folderService);
// CONCATENATED MODULE: ./src/services/moduleDefinitionService.js
function moduleDefinitionService_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function moduleDefinitionService_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function moduleDefinitionService_createClass(Constructor, protoProps, staticProps) { if (protoProps) moduleDefinitionService_defineProperties(Constructor.prototype, protoProps); if (staticProps) moduleDefinitionService_defineProperties(Constructor, staticProps); return Constructor; }

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }



function moduleDefinitionService_errorCallback(message) {
  utils["a" /* default */].utilities.notifyError(_typeof(message) === "object" ? message.Message : message);
}

var moduleDefinitionService_ModuleDefinitionService =
/*#__PURE__*/
function () {
  function ModuleDefinitionService() {
    moduleDefinitionService_classCallCheck(this, ModuleDefinitionService);
  }

  moduleDefinitionService_createClass(ModuleDefinitionService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = utils["a" /* default */].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getSourceFolders",
    value: function getSourceFolders(callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetSourceFolders", {}, callback, moduleDefinitionService_errorCallback);
    }
  }, {
    key: "getSourceFiles",
    value: function getSourceFiles(_root, callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetSourceFiles?root=" + _root, {}, callback, moduleDefinitionService_errorCallback);
    }
  }, {
    key: "getControlIcons",
    value: function getControlIcons(controlPath, callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("LoadIcons?controlPath=" + controlPath, {}, callback, moduleDefinitionService_errorCallback);
    }
  }]);

  return ModuleDefinitionService;
}();

var moduleDefinitionService = new moduleDefinitionService_ModuleDefinitionService();
/* harmony default export */ var services_moduleDefinitionService = (moduleDefinitionService);
// CONCATENATED MODULE: ./src/services/createPackage.js
function createPackage_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function createPackage_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function createPackage_createClass(Constructor, protoProps, staticProps) { if (protoProps) createPackage_defineProperties(Constructor.prototype, protoProps); if (staticProps) createPackage_defineProperties(Constructor, staticProps); return Constructor; }

function createPackage_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { createPackage_typeof = function _typeof(obj) { return typeof obj; }; } else { createPackage_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return createPackage_typeof(obj); }



function createPackage_errorCallback(message) {
  utils["a" /* default */].utilities.notifyError(createPackage_typeof(message) === "object" ? message.Message : message);
}

var createPackage_CreatePackageService =
/*#__PURE__*/
function () {
  function CreatePackageService() {
    createPackage_classCallCheck(this, CreatePackageService);
  }

  createPackage_createClass(CreatePackageService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = utils["a" /* default */].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getPackageManifest",
    value: function getPackageManifest(packageId, callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.get("GetPackageManifest?packageId=" + packageId, {}, callback, createPackage_errorCallback);
    }
  }, {
    key: "createManifest",
    value: function createManifest(payload, callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("CreateManifest", payload, callback, createPackage_errorCallback);
    }
  }, {
    key: "generateManifestPreview",
    value: function generateManifestPreview(payload, callback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("CreateNewManifest", payload, callback, createPackage_errorCallback);
    }
  }, {
    key: "createPackage",
    value: function createPackage(payload, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("CreatePackage", payload, callback, errorCallback);
    }
  }, {
    key: "refreshPackageFiles",
    value: function refreshPackageFiles(payload, callback, errorCallback) {
      var sf = this.getServiceFramework("Extensions");
      sf.post("RefreshPackageFiles", payload, callback, errorCallback);
    }
  }]);

  return CreatePackageService;
}();

var createPackageService = new createPackage_CreatePackageService();
/* harmony default export */ var services_createPackage = (createPackageService);
// CONCATENATED MODULE: ./src/services/index.js






// CONCATENATED MODULE: ./src/utils/helperFunctions.js
function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function getValidateRequired(key) {
  switch (key) {
    case "friendlyName":
      return true;

    default:
      return false;
  }
}

function getTabMapping(key) {
  switch (key) {
    case "license":
      return 3;

    case "releaseNotes":
      return 4;

    default:
      return 0;
  }
}

function validationMapExtensionBeingEdited(extensionBeingEdited) {
  var _extensionBeingEdited = _extends({}, extensionBeingEdited);

  Object.keys(_extensionBeingEdited).forEach(function (key) {
    var validateRequired = getValidateRequired(key);
    var tabMapping = getTabMapping(key);
    _extensionBeingEdited[key] = {
      value: _extensionBeingEdited[key] && !Object.prototype.hasOwnProperty.call(_extensionBeingEdited[key], "value") ? _extensionBeingEdited[key] : _extensionBeingEdited[key] && _extensionBeingEdited[key].value,
      validateRequired: validateRequired,
      tabMapping: tabMapping,
      error: false
    };
  });
  return _extensionBeingEdited;
}
function getVersionDropdownValues() {
  var values = [];

  for (var i = 0; i < 100; i++) {
    values.push({
      label: formatVersionNumber(i),
      value: i
    });
  }

  return values;
}
function formatVersionNumber(n) {
  return n > 9 ? "" + n : "0" + n;
}
// CONCATENATED MODULE: ./src/localization/index.jsx

var resx = {
  get: function get(key) {
    var moduleName = utils["a" /* default */].moduleName;
    return utils["a" /* default */].utilities.getResx(moduleName, key);
  }
};
/* harmony default export */ var localization = (resx);
// CONCATENATED MODULE: ./src/actions/extension.js
function extension_extends() { extension_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return extension_extends.apply(this, arguments); }







function extension_errorCallback(error) {
  var message = utils["a" /* default */].utilities.getResx("SharedResources", "GenericErrorMessage.Error");

  if (error.Error) {
    message = error.Error;
  } else if (error.Message) {
    message = error.Message;
  } else if (typeof error === "string") {
    message = error;
  }

  utils["a" /* default */].utilities.notifyError(message);
}

function valueMapExtensionBeingEdited(extensionBeingEdited) {
  var _extensionBeingEdited = extension_extends({}, extensionBeingEdited);

  Object.keys(_extensionBeingEdited).forEach(function (key) {
    _extensionBeingEdited[key] = _extensionBeingEdited[key].value;
  });
  return _extensionBeingEdited;
}

var extensionActions = {
  getInstalledPackages: function getInstalledPackages(type, callback) {
    return function (dispatch) {
      services_extensionService.getInstalledPackages(type, function (payload) {
        dispatch({
          type: actionTypes["b" /* extension */].RETRIEVED_INSTALLED_PACKAGES,
          payload: {
            Results: payload.Results,
            selectedInstalledPackageType: type
          }
        });

        if (callback) {
          callback();
        }
      }, extension_errorCallback);
    };
  },
  getAvailablePackages: function getAvailablePackages(type, callback) {
    return function (dispatch) {
      services_extensionService.getAvailablePackages(type, function (payload) {
        dispatch({
          type: actionTypes["b" /* extension */].RETRIEVED_AVAILABLE_PACKAGES,
          payload: {
            Results: payload.Results && payload.Results[0] && payload.Results[0].ValidPackages,
            selectedAvailablePackageType: type
          }
        });

        if (callback) {
          callback();
        }
      }, extension_errorCallback);
    };
  },
  getPackageTypes: function getPackageTypes(callback) {
    return function (dispatch) {
      services_extensionService.getPackageTypes(function (payload) {
        dispatch({
          type: actionTypes["b" /* extension */].RETRIEVED_INSTALLED_PACKAGE_TYPES,
          payload: payload
        });

        if (callback) {
          callback();
        }
      }, extension_errorCallback);
    };
  },
  getAvailablePackageTypes: function getAvailablePackageTypes(callback) {
    return function (dispatch) {
      services_extensionService.getAvailablePackageTypes(function (payload) {
        dispatch({
          type: actionTypes["b" /* extension */].RETRIEVED_AVAILABLE_PACKAGE_TYPES,
          payload: payload
        });

        if (callback) {
          callback();
        }
      }, extension_errorCallback);
    };
  },
  updateExtension: function updateExtension(updatedExtension, editorActions, index, callback) {
    return function (dispatch) {
      services_extensionService.updateExtension(updatedExtension, editorActions, function (data) {
        dispatch({
          type: actionTypes["b" /* extension */].UPDATED_EXTENSION_BEING_EDITED,
          payload: {
            extensionBeingEdited: validationMapExtensionBeingEdited(data.PackageDetail)
          }
        });
        dispatch({
          type: actionTypes["b" /* extension */].UPDATED_EXTENSION,
          payload: {
            index: index,
            updatedExtension: valueMapExtensionBeingEdited(updatedExtension)
          }
        });
        utils["a" /* default */].utilities.notify(localization.get("EditExtension_Notify.Success"));

        if (callback) {
          utils["a" /* default */].utilities.throttleExecution(callback);
        }
      }, extension_errorCallback);
    };
  },
  downloadPackage: function downloadPackage(packageType, packageName, callback) {
    return function () {
      services_extensionService.downloadPackage(packageType, packageName, function (data) {
        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  installAvailablePackage: function installAvailablePackage(packageType, packageName, newExtension, callback) {
    return function (dispatch) {
      services_extensionService.installAvailablePackage(packageType, packageName, function (data) {
        if (data.success) {
          var _newExtension = JSON.parse(JSON.stringify(newExtension));

          _newExtension.packageId = data.newPackageId;
          dispatch({
            type: actionTypes["b" /* extension */].INSTALLED_EXTENSION,
            payload: {
              PackageInfo: _newExtension,
              logs: data.logs
            }
          });
        }

        if (!data.success) {
          dispatch({
            type: actionTypes["d" /* installation */].SET_FAILED_INSTALLATION_LOGS
          });
        }

        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  parseAvailablePackage: function parseAvailablePackage(packageType, fileName, callback) {
    return function (dispatch) {
      services_extensionService.parseAvailablePackage(packageType, fileName, function (data) {
        dispatch({
          type: actionTypes["d" /* installation */].PARSED_INSTALLATION_PACKAGE,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  deletePackage: function deletePackage(payload, index, callback) {
    return function (dispatch) {
      services_extensionService.deletePackage(payload, function (data) {
        dispatch({
          type: actionTypes["b" /* extension */].DELETED_EXTENSION,
          payload: {
            index: index
          }
        });

        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  setPackageBeingDeleted: function setPackageBeingDeleted(extensionBeingDeleted, extensionBeingDeletedIndex, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* extension */].SET_EXTENSION_BEING_DELETED,
        payload: {
          extensionBeingDeleted: extensionBeingDeleted,
          extensionBeingDeletedIndex: extensionBeingDeletedIndex
        }
      });

      if (callback) {
        callback();
      }
    };
  },
  toggleDeleteFiles: function toggleDeleteFiles(callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* extension */].TOGGLE_DELETE_EXTENSION_FILES
      });

      if (callback) {
        callback();
      }
    };
  },
  createNewModule: function createNewModule(payload, shouldAppend, callback) {
    return function (dispatch) {
      services_extensionService.createNewModule(payload, function (data) {
        if (shouldAppend) {
          dispatch({
            type: actionTypes["b" /* extension */].CREATED_NEW_MODULE,
            payload: data
          });
        }

        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  addExtension: function addExtension(parameters, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* extension */].EDIT_EXTENSION,
        payload: {
          extensionBeingEdited: parameters,
          extensionBeingEditedIndex: -1
        }
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  createNewExtension: function createNewExtension(extensionBeingAdded, editorActions, index, callback) {
    return function (dispatch) {
      services_extensionService.createNewExtension(extensionBeingAdded, editorActions, function (data) {
        var _extensionBeingAdded = JSON.parse(JSON.stringify(extensionBeingAdded));

        _extensionBeingAdded.packageId = {};
        _extensionBeingAdded.packageId.value = data.PackageId;
        dispatch({
          type: actionTypes["b" /* extension */].ADDED_NEW_EXTENSION,
          payload: {
            PackageInfo: valueMapExtensionBeingEdited(_extensionBeingAdded)
          }
        });

        if (callback) {
          callback();
        }
      }, extension_errorCallback);
    };
  },
  editExtension: function editExtension(parameters, extensionBeingEditedIndex, callback) {
    return function (dispatch) {
      services_extensionService.getPackageSettings(parameters, function (data) {
        dispatch({
          type: actionTypes["b" /* extension */].EDIT_EXTENSION,
          payload: {
            extensionBeingEdited: data,
            extensionBeingEditedIndex: extensionBeingEditedIndex
          }
        });

        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  toggleTriedToSave: function toggleTriedToSave() {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* extension */].TOGGLE_TRIED_TO_SAVE
      });
    };
  },
  updateExtensionBeingEdited: function updateExtensionBeingEdited(extensionBeingEdited, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* extension */].UPDATED_EXTENSION_BEING_EDITED,
        payload: {
          extensionBeingEdited: extensionBeingEdited
        }
      });

      if (callback) {
        setTimeout(function () {
          //let JS propagate
          callback();
        }, 0);
      }
    };
  },
  toggleTabError: function toggleTabError(tabIndex, action, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* extension */].TOGGLE_TAB_ERROR,
        payload: {
          tabIndex: tabIndex,
          action: action
        }
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  getModuleCategories: function getModuleCategories(callback) {
    return function (dispatch) {
      services_extensionService.getModuleCategories(function (data) {
        dispatch({
          type: actionTypes["b" /* extension */].RETRIEVED_MODULE_CATEGORIES,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  selectEditingTab: function selectEditingTab(index, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["b" /* extension */].SELECT_EDITING_TAB,
        payload: index
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  getLocaleList: function getLocaleList(callback) {
    return function (dispatch) {
      services_extensionService.getLocaleList(function (data) {
        dispatch({
          type: actionTypes["b" /* extension */].RETRIEVED_LOCALE_LIST,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  getLocalePackageList: function getLocalePackageList(callback) {
    return function (dispatch) {
      services_extensionService.getLocalePackagesList(function (data) {
        dispatch({
          type: actionTypes["b" /* extension */].RETRIEVED_PACKAGE_LOCALE_LIST,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  deployAvailablePackage: function deployAvailablePackage(_package, updatedPackageIndex, callback) {
    return function () {
      services_extensionService.deployAvailablePackage(_package.description, function (data) {
        if (callback) {
          callback(data);
        }
      }, extension_errorCallback);
    };
  },
  GetPackageUsageFilter: function GetPackageUsageFilter(callback) {
    return function (dispatch) {
      services_extensionService.getPackageUsageFilter(function (payload) {
        dispatch({
          type: actionTypes["b" /* extension */].RETRIEVED_PACKAGE_USAGE_FILTER,
          payload: payload
        });

        if (callback) {
          callback();
        }
      }, extension_errorCallback);
    };
  },
  getPackageUsage: function getPackageUsage(portalId, packageId, callback) {
    return function (dispatch) {
      services_extensionService.getPackageUsage(portalId, packageId, function (data) {
        dispatch({
          type: actionTypes["b" /* extension */].RETRIEVED_PACKAGE_USAGE,
          payload: data
        });

        if (callback) {
          callback();
        }
      }, extension_errorCallback);
    };
  }
};
/* harmony default export */ var extension = (extensionActions);
// CONCATENATED MODULE: ./src/actions/installation.js


var installationActions = {
  parsePackage: function parsePackage(file, callback, errorCallback) {
    return function (dispatch) {
      services_installationService.parsePackage(file, function (data) {
        dispatch({
          type: actionTypes["d" /* installation */].PARSED_INSTALLATION_PACKAGE,
          payload: JSON.parse(data)
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  navigateWizard: function navigateWizard(wizardStep, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["d" /* installation */].GO_TO_WIZARD_STEP,
        payload: {
          wizardStep: wizardStep
        }
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  setInstallingAvailablePackage: function setInstallingAvailablePackage(FileName, PackageType, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["d" /* installation */].INSTALLING_AVAILABLE_PACKAGE,
        payload: {
          PackageType: PackageType,
          FileName: FileName
        }
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  notInstallingAvailablePackage: function notInstallingAvailablePackage(callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["d" /* installation */].NOT_INSTALLING_AVAILABLE_PACKAGE
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  installExtension: function installExtension(file, newExtension, legacyType, isPortalPackage, callback, addToList) {
    var _newExtension = JSON.parse(JSON.stringify(newExtension));

    return function (dispatch) {
      services_installationService.installPackage(file, legacyType, isPortalPackage, function (data) {
        dispatch({
          type: actionTypes["d" /* installation */].INSTALLED_EXTENSION_LOGS,
          payload: JSON.parse(data)
        });

        if (addToList) {
          _newExtension.packageId = JSON.parse(data).newPackageId;
          _newExtension.inUse = "No";
          dispatch({
            type: actionTypes["b" /* extension */].INSTALLED_EXTENSION,
            payload: {
              PackageInfo: _newExtension,
              logs: JSON.parse(data).logs
            }
          });
        }

        if (callback) {
          callback(data);
        }
      });
    };
  },
  clearParsedInstallationPackage: function clearParsedInstallationPackage(callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["d" /* installation */].CLEAR_PARSED_INSTALLATION_PACKAGE
      });

      if (callback) {
        callback();
      }
    };
  },
  toggleAcceptLicense: function toggleAcceptLicense(value, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["d" /* installation */].TOGGLE_ACCEPT_LICENSE,
        payload: value
      });

      if (callback) {
        callback();
      }
    };
  },
  setViewingLog: function setViewingLog(value, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["d" /* installation */].TOGGLE_VIEWING_LOG,
        payload: value
      });

      if (callback) {
        callback();
      }
    };
  },
  setIsPortalPackage: function setIsPortalPackage(value, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["d" /* installation */].SET_IS_PORTAL_PACKAGE,
        payload: value
      });

      if (callback) {
        callback();
      }
    };
  }
};
/* harmony default export */ var installation = (installationActions);
// CONCATENATED MODULE: ./src/actions/folder.js




function folder_errorCallback(message) {
  utils["a" /* default */].utilities.notifyError(message);
}

var folderActions = {
  getOwnerFolders: function getOwnerFolders(callback) {
    return function (dispatch) {
      services_folderService.getOwnerFolders(function (data) {
        dispatch({
          type: actionTypes["c" /* folder */].RETRIEVED_OWNER_FOLDERS,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      }, folder_errorCallback);
    };
  },
  getModuleFolders: function getModuleFolders(type, callback) {
    return function (dispatch) {
      services_folderService.getModuleFolders(type, function (data) {
        dispatch({
          type: actionTypes["c" /* folder */].RETRIEVED_MODULE_FOLDERS,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      }, folder_errorCallback);
    };
  },
  getModuleFiles: function getModuleFiles(parameters, callback) {
    return function (dispatch) {
      services_folderService.getModuleFiles(parameters, function (data) {
        dispatch({
          type: actionTypes["c" /* folder */].RETRIEVED_MODULE_FILES,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      }, folder_errorCallback);
    };
  },
  createFolder: function createFolder(parameters, type, callback) {
    return function (dispatch) {
      services_folderService.createNewFolder(parameters, function () {
        dispatch({
          type: type === "moduleFolder" ? actionTypes["c" /* folder */].CREATED_NEW_MODULE_FOLDER : actionTypes["c" /* folder */].CREATED_NEW_OWNER_FOLDER,
          payload: {
            value: parameters[type]
          }
        });

        if (callback) {
          callback(parameters);
        }
      }, folder_errorCallback);
    };
  }
};
/* harmony default export */ var folder = (folderActions);
// CONCATENATED MODULE: ./src/actions/moduleDefinition.js


var moduleDefinitionActions = {
  setFormDirt: function setFormDirt(value, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["e" /* moduleDefinition */].SET_FORM_DIRT,
        payload: value
      });

      if (typeof callback === "function") {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  setControlFormDirt: function setControlFormDirt(value, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["e" /* moduleDefinition */].SET_CONTROL_FORM_DIRT,
        payload: value
      });

      if (typeof callback === "function") {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  getSourceFolders: function getSourceFolders(callback) {
    return function (dispatch) {
      services_moduleDefinitionService.getSourceFolders(function (data) {
        dispatch({
          type: actionTypes["e" /* moduleDefinition */].RETRIEVED_SOURCE_FOLDERS,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getSourceFiles: function getSourceFiles(_root, callback) {
    return function (dispatch) {
      services_moduleDefinitionService.getSourceFiles(_root, function (data) {
        dispatch({
          type: actionTypes["e" /* moduleDefinition */].RETRIEVED_SOURCE_FILES,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getControlIcons: function getControlIcons(controlPath, callback) {
    return function (dispatch) {
      services_moduleDefinitionService.getControlIcons(controlPath, function (data) {
        dispatch({
          type: actionTypes["e" /* moduleDefinition */].RETRIEVED_CONTROL_ICONS,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      });
    };
  }
};
/* harmony default export */ var actions_moduleDefinition = (moduleDefinitionActions);
// CONCATENATED MODULE: ./src/actions/createPackage.js


var createPackageActions = {
  getPackageManifest: function getPackageManifest(packageId, callback) {
    return function (dispatch) {
      services_createPackage.getPackageManifest(packageId, function (data) {
        dispatch({
          type: actionTypes["a" /* createPackage */].RETRIEVED_PACKAGE_MANIFEST,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updatePackagePayload: function updatePackagePayload(packagePayload, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["a" /* createPackage */].UPDATED_PACKAGE_PAYLOAD,
        payload: packagePayload
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  updatePackageManifest: function updatePackageManifest(packageManifest, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["a" /* createPackage */].UPDATED_PACKAGE_MANIFEST,
        payload: packageManifest
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  createManifest: function createManifest(payload, callback) {
    return function (dispatch) {
      services_createPackage.createManifest(payload, function (data) {
        dispatch({
          type: actionTypes["a" /* createPackage */].CREATED_PACKAGE_MANIFEST,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  generateManifestPreview: function generateManifestPreview(payload, callback) {
    return function (dispatch) {
      services_createPackage.generateManifestPreview(payload, function (data) {
        dispatch({
          type: actionTypes["a" /* createPackage */].RETRIEVED_GENERATED_MANIFEST,
          payload: data
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  createPackage: function createPackage(payload, callback, errorCallback) {
    return function (dispatch) {
      services_createPackage.createPackage(payload, function (data) {
        dispatch({
          type: actionTypes["a" /* createPackage */].CREATED_PACKAGE,
          payload: data
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
  },
  goToWizardStep: function goToWizardStep(step, callback) {
    return function (dispatch) {
      dispatch({
        type: actionTypes["a" /* createPackage */].GO_TO_STEP,
        payload: step
      });

      if (callback) {
        setTimeout(function () {
          callback();
        }, 0);
      }
    };
  },
  refreshPackageFiles: function refreshPackageFiles(payload, callback, errorCallback) {
    return function (dispatch) {
      services_createPackage.refreshPackageFiles(payload, function (data) {
        dispatch({
          type: actionTypes["a" /* createPackage */].REFRESH_PACKAGE_FILES,
          payload: data
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
};
/* harmony default export */ var actions_createPackage = (createPackageActions);
// CONCATENATED MODULE: ./src/actions/index.js








// EXTERNAL MODULE: external "window.dnn.nodeModules.CommonComponents"
var external_window_dnn_nodeModules_CommonComponents_ = __webpack_require__(1);

// EXTERNAL MODULE: ./src/components/Body/InstalledExtensions/common/ExtensionColumnSizes.js
var ExtensionColumnSizes = __webpack_require__(8);
var ExtensionColumnSizes_default = /*#__PURE__*/__webpack_require__.n(ExtensionColumnSizes);

// EXTERNAL MODULE: ./src/components/Body/InstalledExtensions/common/ExtensionHeader/style.less
var style = __webpack_require__(24);
var style_default = /*#__PURE__*/__webpack_require__.n(style);

// CONCATENATED MODULE: ./src/components/Body/InstalledExtensions/common/ExtensionHeader/index.jsx






var ExtensionHeader_ExtensionHeader = function ExtensionHeader() {
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: style_default.a.extensionHeader,
      columnSize: 100,
      style: {
        padding: "20px 20px 5px"
      }
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: ExtensionColumnSizes_default.a[0]
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: ExtensionColumnSizes_default.a[1],
      style: {
        padding: "0 35px"
      }
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("Extension.Header"))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: ExtensionColumnSizes_default.a[2]
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("Version.Header"))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: ExtensionColumnSizes_default.a[3]
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("InUse.Header"))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: ExtensionColumnSizes_default.a[4]
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("Upgrade.Header"))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: ExtensionColumnSizes_default.a[5]
    }))
  );
};

/* harmony default export */ var common_ExtensionHeader = (ExtensionHeader_ExtensionHeader);
// EXTERNAL MODULE: ./src/components/Body/InstalledExtensions/common/ExtensionDetailRow/style.less
var ExtensionDetailRow_style = __webpack_require__(25);
var ExtensionDetailRow_style_default = /*#__PURE__*/__webpack_require__.n(ExtensionDetailRow_style);

// EXTERNAL MODULE: ./src/components/Body/InstalledExtensions/common/InUseModal/style.less
var InUseModal_style = __webpack_require__(58);

// CONCATENATED MODULE: ./src/components/Body/InstalledExtensions/common/InUseModal/index.jsx
function InUseModal_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { InUseModal_typeof = function _typeof(obj) { return typeof obj; }; } else { InUseModal_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return InUseModal_typeof(obj); }

function InUseModal_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function InUseModal_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function InUseModal_createClass(Constructor, protoProps, staticProps) { if (protoProps) InUseModal_defineProperties(Constructor.prototype, protoProps); if (staticProps) InUseModal_defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (InUseModal_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }








var modalStyles = {
  overlay: {
    zIndex: "99999",
    backgroundColor: "rgba(0, 0, 0, 0)"
  },
  content: {
    top: "250px",
    left: "220px",
    position: "absolute",
    padding: "30px 30px 30px 30px",
    borderRadius: 0,
    border: "none",
    width: "550px",
    height: "380px",
    backgroundColor: "#FFF",
    userSelect: "none",
    WebkitUserSelect: "none",
    MozUserSelect: "none",
    MsUserSelect: "none",
    boxShadow: "0 0 20px 0 rgba(0, 0, 0, .2)"
  }
};

var InUseModal_InUseModal =
/*#__PURE__*/
function (_Component) {
  _inherits(InUseModal, _Component);

  function InUseModal() {
    var _this;

    InUseModal_classCallCheck(this, InUseModal);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(InUseModal).call(this));
    _this.state = {
      usageFilter: undefined,
      portalId: -1
    };
    return _this;
  }

  InUseModal_createClass(InUseModal, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props,
          state = this.state;

      if (props.usageFilter) {
        this.setState({
          usageFilter: props.usageFilter
        });

        if (state.portalId === -1) {
          var portalId = props.usageFilter.find(function (p) {
            return p.IsCurrentPortal === true;
          }).PortalID;
          this.setState({
            usageFilter: props.usageFilter,
            portalId: portalId
          });
          props.dispatch(extension.getPackageUsage(portalId, props.packageId));
        }

        return;
      }

      props.dispatch(extension.GetPackageUsageFilter());
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      var state = this.state;

      if (state.portalId === -1) {
        var portalId = props.usageFilter.find(function (p) {
          return p.IsCurrentPortal === true;
        }).PortalID;
        this.setState({
          usageFilter: props.usageFilter,
          portalId: portalId
        });
        props.dispatch(extension.getPackageUsage(portalId, props.packageId));
      }
    }
  }, {
    key: "onClose",
    value: function onClose() {
      var props = this.props;
      props.onClose();
    }
  }, {
    key: "getPortalOptions",
    value: function getPortalOptions() {
      var props = this.props;
      var options = [];

      if (props.usageFilter !== undefined) {
        options = props.usageFilter.map(function (portal) {
          return {
            label: portal.PortalName,
            value: portal.PortalID
          };
        });
      }

      return options;
    }
  }, {
    key: "onSelectPortal",
    value: function onSelectPortal(event) {
      var props = this.props;
      this.setState({
        portalId: event.value
      });
      props.dispatch(extension.getPackageUsage(event.value, props.packageId));
    }
  }, {
    key: "getCurrentUsageFilter",
    value: function getCurrentUsageFilter() {
      var props = this.props,
          state = this.state;

      if (props.usageFilter && state.portalId !== -1) {
        return props.usageFilter.find(function (p) {
          return p.PortalID === state.portalId;
        }).PortalName;
      } else return null;
    }
  }, {
    key: "getUsageDetailSubject",
    value: function getUsageDetailSubject() {
      var props = this.props;

      if (props.tabUrls) {
        if (props.tabUrls.length > 0) {
          return localization.get("ModuleUsageDetail").replace("{0}", props.tabUrls.length).replace("{1}", this.getCurrentUsageFilter());
        } else return localization.get("NoModuleUsageDetail").replace("{0}", this.getCurrentUsageFilter());
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "renderUsageDetail",
    value: function renderUsageDetail() {
      var props = this.props;

      if (props.tabUrls) {
        return props.tabUrls.map(function (item, i) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("div", {
              key: i,
              className: "usage-detail-taburl",
              dangerouslySetInnerHTML: {
                __html: item.TabUrl
              }
            })
          );
        });
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Modal"], {
          fixedHeight: props.fixedHeight,
          isOpen: props.isOpened,
          style: modalStyles
        }, props.fixedHeight &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "modepanel-content-wrapper",
          style: {
            height: "calc(100% - 100px)"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "modepanel-content-title"
        }, localization.get("ModuleUsageTitle").replace("{0}", props.packageName)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "modepanel-content-filter"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          style: {
            width: "auto",
            marginTop: "8px"
          },
          label: localization.get("PagesFromSite")
        }), props.usageFilter &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          enabled: props.usageFilter.length > 1,
          value: state.portalId,
          style: {
            width: "150px"
          },
          options: this.getPortalOptions(),
          withBorder: false,
          onSelect: this.onSelectPortal.bind(this)
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "modepanel-content-detail-wrapper"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
          label: this.getUsageDetailSubject()
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "usage-detail"
        }, this.renderUsageDetail())),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "button-box"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: this.onClose.bind(this)
        }, localization.get("Close")))))
      );
    }
  }]);

  return InUseModal;
}(external_window_dnn_nodeModules_React_["Component"]);

InUseModal_InUseModal.PropTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  usageFilter: prop_types_default.a.array,
  tabUrls: prop_types_default.a.array,
  packageName: prop_types_default.a.string,
  packageId: prop_types_default.a.number,
  fixedHeight: prop_types_default.a.number,
  collapsibleWidth: prop_types_default.a.number,
  collapsibleHeight: prop_types_default.a.number,
  keepCollapsedContent: prop_types_default.a.bool,
  scrollAreaStyle: prop_types_default.a.object,
  isOpened: prop_types_default.a.bool,
  onClose: prop_types_default.a.func.isRequired,
  isHost: prop_types_default.a.bool
};

function mapStateToProps(state) {
  return {
    usageFilter: state.extension.usageFilter,
    tabUrls: state.extension.tabUrls
  };
}

/* harmony default export */ var common_InUseModal = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(mapStateToProps)(InUseModal_InUseModal));
// CONCATENATED MODULE: ./src/components/Body/InstalledExtensions/common/ExtensionDetailRow/index.jsx
function ExtensionDetailRow_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { ExtensionDetailRow_typeof = function _typeof(obj) { return typeof obj; }; } else { ExtensionDetailRow_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return ExtensionDetailRow_typeof(obj); }

function ExtensionDetailRow_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function ExtensionDetailRow_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function ExtensionDetailRow_createClass(Constructor, protoProps, staticProps) { if (protoProps) ExtensionDetailRow_defineProperties(Constructor.prototype, protoProps); if (staticProps) ExtensionDetailRow_defineProperties(Constructor, staticProps); return Constructor; }

function ExtensionDetailRow_possibleConstructorReturn(self, call) { if (call && (ExtensionDetailRow_typeof(call) === "object" || typeof call === "function")) { return call; } return ExtensionDetailRow_assertThisInitialized(self); }

function ExtensionDetailRow_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function ExtensionDetailRow_getPrototypeOf(o) { ExtensionDetailRow_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return ExtensionDetailRow_getPrototypeOf(o); }

function ExtensionDetailRow_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) ExtensionDetailRow_setPrototypeOf(subClass, superClass); }

function ExtensionDetailRow_setPrototypeOf(o, p) { ExtensionDetailRow_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return ExtensionDetailRow_setPrototypeOf(o, p); }








var ExtensionDetailRow_ExtensionDetailRow =
/*#__PURE__*/
function (_Component) {
  ExtensionDetailRow_inherits(ExtensionDetailRow, _Component);

  function ExtensionDetailRow() {
    var _this;

    ExtensionDetailRow_classCallCheck(this, ExtensionDetailRow);

    _this = ExtensionDetailRow_possibleConstructorReturn(this, ExtensionDetailRow_getPrototypeOf(ExtensionDetailRow).call(this));
    _this.state = {
      inUseModalOpen: false,
      usagePackageName: null,
      usagePackageId: -1
    };
    return _this;
  }

  ExtensionDetailRow_createClass(ExtensionDetailRow, [{
    key: "toggleInUseModal",
    value: function toggleInUseModal(packageName, packageId) {
      var nextState = !this.state.inUseModalOpen;
      window.dnn.stopEscapeFromClosingPB = nextState;
      this.setState({
        inUseModalOpen: nextState,
        usagePackageName: packageName,
        usagePackageId: packageId
      });
    }
  }, {
    key: "getInUseDisplay",
    value: function getInUseDisplay(packageName, packageId) {
      var props = this.props;

      if (props._package.inUse === "Yes") {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("div", {
            className: "in-use",
            onClick: this.toggleInUseModal.bind(this, packageName, packageId)
          }, props._package.inUse)
        );
      } else {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("p", null, props._package.inUse)
        );
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: ExtensionDetailRow_style_default.a.extensionDetailRow,
          columnSize: 100,
          style: {
            padding: "20px 0 20px 20px"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: ExtensionColumnSizes_default.a[0],
          style: {
            padding: 0
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("img", {
          src: props._package.packageIcon && props._package.packageIcon.replace("~", ""),
          alt: props._package.friendlyName
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: ExtensionColumnSizes_default.a[1],
          style: {
            padding: "0 35px"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          className: "package-name"
        }, props._package.friendlyName),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          dangerouslySetInnerHTML: {
            __html: props._package.description
          }
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: ExtensionColumnSizes_default.a[2]
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", null, props._package.version)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: ExtensionColumnSizes_default.a[3]
        }, this.getInUseDisplay(props._package.friendlyName, props._package.packageId)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: ExtensionColumnSizes_default.a[4]
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("a", {
          href: props._package.url,
          target: "_blank",
          rel: "noopener noreferrer",
          "aria-label": "Update"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("img", {
          src: props._package.upgradeIndicator,
          alt: "Update"
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: ExtensionColumnSizes_default.a[5],
          style: {
            paddingRight: 0
          }
        }, props._package.canDelete && props.isHost &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "extension-action",
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].TrashIcon
          },
          onClick: props.onDelete
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "extension-action",
          onClick: props.onEdit.bind(this, props._package.packageId),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].EditIcon
          }
        }))), state.inUseModalOpen &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_InUseModal, {
          isHost: props.isHost,
          packageId: state.usagePackageId,
          packageName: state.usagePackageName,
          fixedHeight: 500,
          isOpened: state.inUseModalOpen,
          onClose: this.toggleInUseModal.bind(this)
        }))
      );
    }
  }]);

  return ExtensionDetailRow;
}(external_window_dnn_nodeModules_React_["Component"]);

ExtensionDetailRow_ExtensionDetailRow.propTypes = {
  _package: prop_types_default.a.object,
  onEdit: prop_types_default.a.func,
  onDelete: prop_types_default.a.func,
  isHost: prop_types_default.a.bool
};
/* harmony default export */ var common_ExtensionDetailRow = (ExtensionDetailRow_ExtensionDetailRow);
// EXTERNAL MODULE: ./src/components/Body/InstalledExtensions/ExtensionList/style.less
var ExtensionList_style = __webpack_require__(26);
var ExtensionList_style_default = /*#__PURE__*/__webpack_require__.n(ExtensionList_style);

// CONCATENATED MODULE: ./src/components/Body/InstalledExtensions/ExtensionList/index.jsx
function ExtensionList_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { ExtensionList_typeof = function _typeof(obj) { return typeof obj; }; } else { ExtensionList_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return ExtensionList_typeof(obj); }

function ExtensionList_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function ExtensionList_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function ExtensionList_createClass(Constructor, protoProps, staticProps) { if (protoProps) ExtensionList_defineProperties(Constructor.prototype, protoProps); if (staticProps) ExtensionList_defineProperties(Constructor, staticProps); return Constructor; }

function ExtensionList_possibleConstructorReturn(self, call) { if (call && (ExtensionList_typeof(call) === "object" || typeof call === "function")) { return call; } return ExtensionList_assertThisInitialized(self); }

function ExtensionList_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function ExtensionList_getPrototypeOf(o) { ExtensionList_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return ExtensionList_getPrototypeOf(o); }

function ExtensionList_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) ExtensionList_setPrototypeOf(subClass, superClass); }

function ExtensionList_setPrototypeOf(o, p) { ExtensionList_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return ExtensionList_setPrototypeOf(o, p); }








var ExtensionList_ExtensionList =
/*#__PURE__*/
function (_Component) {
  ExtensionList_inherits(ExtensionList, _Component);

  function ExtensionList() {
    ExtensionList_classCallCheck(this, ExtensionList);

    return ExtensionList_possibleConstructorReturn(this, ExtensionList_getPrototypeOf(ExtensionList).call(this));
  }

  ExtensionList_createClass(ExtensionList, [{
    key: "render",
    value: function render() {
      var _this = this;

      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: ExtensionList_style_default.a.extensionList,
          style: {
            padding: "5px 20px"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_ExtensionHeader, null), props.packages.map(function (_package, index) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(common_ExtensionDetailRow, {
              _package: _package,
              isHost: props.isHost,
              onEdit: props.onEdit.bind(_this, index),
              onDelete: props.onDelete.bind(_this, _package, index),
              key: index
            })
          );
        }))
      );
    }
  }]);

  return ExtensionList;
}(external_window_dnn_nodeModules_React_["Component"]);

ExtensionList_ExtensionList.propTypes = {
  label: prop_types_default.a.string,
  packages: prop_types_default.a.array,
  collapsed: prop_types_default.a.bool,
  onChange: prop_types_default.a.func,
  onEdit: prop_types_default.a.func,
  isHost: prop_types_default.a.bool
};
/* harmony default export */ var InstalledExtensions_ExtensionList = (ExtensionList_ExtensionList);
// EXTERNAL MODULE: ./src/components/Body/InstalledExtensions/style.less
var InstalledExtensions_style = __webpack_require__(61);

// CONCATENATED MODULE: ./src/components/Body/InstalledExtensions/index.jsx
function InstalledExtensions_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { InstalledExtensions_typeof = function _typeof(obj) { return typeof obj; }; } else { InstalledExtensions_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return InstalledExtensions_typeof(obj); }

function InstalledExtensions_extends() { InstalledExtensions_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return InstalledExtensions_extends.apply(this, arguments); }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function InstalledExtensions_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function InstalledExtensions_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function InstalledExtensions_createClass(Constructor, protoProps, staticProps) { if (protoProps) InstalledExtensions_defineProperties(Constructor.prototype, protoProps); if (staticProps) InstalledExtensions_defineProperties(Constructor, staticProps); return Constructor; }

function InstalledExtensions_possibleConstructorReturn(self, call) { if (call && (InstalledExtensions_typeof(call) === "object" || typeof call === "function")) { return call; } return InstalledExtensions_assertThisInitialized(self); }

function InstalledExtensions_getPrototypeOf(o) { InstalledExtensions_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return InstalledExtensions_getPrototypeOf(o); }

function InstalledExtensions_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function InstalledExtensions_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) InstalledExtensions_setPrototypeOf(subClass, superClass); }

function InstalledExtensions_setPrototypeOf(o, p) { InstalledExtensions_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return InstalledExtensions_setPrototypeOf(o, p); }










var InstalledExtensions_InstalledExtensions =
/*#__PURE__*/
function (_Component) {
  InstalledExtensions_inherits(InstalledExtensions, _Component);

  function InstalledExtensions() {
    var _this;

    InstalledExtensions_classCallCheck(this, InstalledExtensions);

    _this = InstalledExtensions_possibleConstructorReturn(this, InstalledExtensions_getPrototypeOf(InstalledExtensions).call(this));
    _this.handleSelect = _this.handleSelect.bind(InstalledExtensions_assertThisInitialized(_this));
    _this.state = {
      loading: false
    };
    return _this;
  }

  InstalledExtensions_createClass(InstalledExtensions, [{
    key: "checkIfPackageTypesEmpty",
    value: function checkIfPackageTypesEmpty(props) {
      return !props.installedPackageTypes || props.installedPackageTypes.length === 0;
    }
  }, {
    key: "checkIfInstalledPackagesEmpty",
    value: function checkIfInstalledPackagesEmpty(props) {
      return !props.installedPackages || props.installedPackages.length === 0;
    }
  }, {
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;

      if (this.checkIfPackageTypesEmpty(props)) {
        props.dispatch(extension.getPackageTypes());
      }
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      var _this2 = this;

      if (!this.checkIfPackageTypesEmpty(props) && this.checkIfInstalledPackagesEmpty(props) && props.selectedInstalledPackageType === "") {
        this.setState({
          loading: true
        }, function () {
          props.dispatch(extension.getInstalledPackages(props.installedPackageTypes[0].Type, function () {
            _this2.setState({
              loading: false
            });
          }));
        });
      }
    }
  }, {
    key: "handleSelect",
    value: function handleSelect(index
    /*, last*/
    ) {
      var props = this.props;
      props.dispatch(pagination.loadTab(index)); //index acts as scopeTypeId
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      this.setState(_defineProperty({}, key, event.target.value));
    }
  }, {
    key: "onFilterSelect",
    value: function onFilterSelect(option) {
      var _this3 = this;

      var props = this.props;
      this.setState({
        loading: true
      }, function () {
        props.dispatch(extension.getInstalledPackages(option.value, function () {
          _this3.setState({
            loading: false
          });
        }));
      });
    }
  }, {
    key: "onDelete",
    value: function onDelete(_package, index) {
      var props = this.props;
      props.dispatch(extension.setPackageBeingDeleted(InstalledExtensions_extends({
        packageType: props.selectedInstalledPackageType
      }, _package), index, function () {
        props.dispatch(visiblePanel.selectPanel(6));
      }));
    }
  }, {
    key: "renderLoading",
    value: function renderLoading() {
      /* eslint-disable react/no-danger */
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "loading-extensions"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h2", null, localization.get("Loading")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", null, localization.get("Loading.Tooltip")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          dangerouslySetInnerHTML: {
            __html: __webpack_require__(14).default
          }
        }))
      );
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "extension-list"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "collapse-section filter-section"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          className: "filter-dropdown",
          onSelect: this.onFilterSelect.bind(this),
          options: props.installedPackageTypes && props.installedPackageTypes.map(function (_package) {
            return {
              label: _package.DisplayName,
              value: _package.Type
            };
          }),
          label: localization.get("Showing.Label"),
          value: props.selectedInstalledPackageType,
          labelType: "inline"
        })), state.loading && this.renderLoading(), props.installedPackages && props.installedPackages.length > 0 && !state.loading &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(InstalledExtensions_ExtensionList, {
          packages: props.installedPackages,
          isHost: props.isHost,
          onEdit: props.onEdit.bind(this),
          onDelete: this.onDelete.bind(this)
        }))
      );
    }
  }]);

  return InstalledExtensions;
}(external_window_dnn_nodeModules_React_["Component"]);

InstalledExtensions_InstalledExtensions.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  installedPackages: prop_types_default.a.array,
  installedPackageTypes: prop_types_default.a.array,
  isHost: prop_types_default.a.bool,
  tabIndex: prop_types_default.a.number
};

function InstalledExtensions_mapStateToProps(state) {
  return {
    installedPackageTypes: state.extension.installedPackageTypes,
    installedPackages: state.extension.installedPackages,
    selectedInstalledPackageType: state.extension.selectedInstalledPackageType,
    tabIndex: state.pagination.tabIndex
  };
}

/* harmony default export */ var Body_InstalledExtensions = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(InstalledExtensions_mapStateToProps)(InstalledExtensions_InstalledExtensions));
// EXTERNAL MODULE: ./src/components/Body/AvailableExtensions/common/ExtensionColumnSizes.js
var common_ExtensionColumnSizes = __webpack_require__(10);
var common_ExtensionColumnSizes_default = /*#__PURE__*/__webpack_require__.n(common_ExtensionColumnSizes);

// EXTERNAL MODULE: ./src/components/Body/AvailableExtensions/common/ExtensionHeader/style.less
var ExtensionHeader_style = __webpack_require__(27);
var ExtensionHeader_style_default = /*#__PURE__*/__webpack_require__.n(ExtensionHeader_style);

// CONCATENATED MODULE: ./src/components/Body/AvailableExtensions/common/ExtensionHeader/index.jsx






var common_ExtensionHeader_ExtensionHeader = function ExtensionHeader() {
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: ExtensionHeader_style_default.a.extensionHeader,
      columnSize: 100,
      style: {
        padding: "20px 20px 5px"
      }
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: common_ExtensionColumnSizes_default.a[0]
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: common_ExtensionColumnSizes_default.a[1],
      style: {
        padding: "0 35px"
      }
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("Extension.Header"))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: common_ExtensionColumnSizes_default.a[2]
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("Version.Header"))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: common_ExtensionColumnSizes_default.a[3]
    }))
  );
};

/* harmony default export */ var AvailableExtensions_common_ExtensionHeader = (common_ExtensionHeader_ExtensionHeader);
// EXTERNAL MODULE: ./src/components/Body/AvailableExtensions/common/ExtensionDetailRow/style.less
var common_ExtensionDetailRow_style = __webpack_require__(28);
var common_ExtensionDetailRow_style_default = /*#__PURE__*/__webpack_require__.n(common_ExtensionDetailRow_style);

// CONCATENATED MODULE: ./src/components/Body/AvailableExtensions/common/ExtensionDetailRow/index.jsx
var ExtensionDetailRow_this = undefined;








/* eslint-disable react/no-danger */

var common_ExtensionDetailRow_ExtensionDetailRow = function ExtensionDetailRow(_ref) {
  var _package = _ref._package,
      type = _ref.type,
      onInstall = _ref.onInstall,
      onDeploy = _ref.onDeploy,
      doingOperation = _ref.doingOperation;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: common_ExtensionDetailRow_style_default.a.extensionDetailRow,
      columnSize: 100,
      style: {
        padding: "20px 0 20px 20px"
      }
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: common_ExtensionColumnSizes_default.a[0],
      style: {
        padding: 0
      }
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("img", {
      src: _package.packageIcon.replace("~", ""),
      alt: _package.friendlyName
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: common_ExtensionColumnSizes_default.a[1],
      style: {
        padding: "0 35px"
      }
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("span", {
      className: "package-name"
    }, _package.friendlyName),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", {
      dangerouslySetInnerHTML: {
        __html: _package.description
      }
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: common_ExtensionColumnSizes_default.a[2]
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", null, _package.version)),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: common_ExtensionColumnSizes_default.a[3],
      style: {
        paddingRight: 0
      }
    }, _package.fileName &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("form", {
      action: utils["a" /* default */].siteRoot + "API/PersonaBar/Extensions/DownloadPackage",
      method: "GET",
      target: "_blank"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("input", {
      type: "hidden",
      name: "FileName",
      value: _package.fileName
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("input", {
      type: "hidden",
      name: "PackageType",
      value: type
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("button", {
      className: "dnn-ui-common-button download-button",
      type: "submit",
      role: "secondary"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
      text: localization.get("Download.Button"),
      maxWidth: 60
    }))), !_package.fileName &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("form", {
      action: utils["a" /* default */].siteRoot + "API/PersonaBar/Extensions/DownloadLanguagePackage",
      method: "GET",
      target: "_blank"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("input", {
      type: "hidden",
      name: "CultureCode",
      value: _package.description
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("button", {
      className: "dnn-ui-common-button download-button",
      type: "submit",
      role: "secondary"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
      text: localization.get("Download.Button"),
      maxWidth: 60
    }))), !_package.fileName &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      className: "install-download-button",
      disabled: doingOperation,
      onClick: onDeploy.bind(ExtensionDetailRow_this, _package)
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
      text: localization.get("Deploy.Button"),
      maxWidth: 60
    })), _package.fileName &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      className: "install-download-button",
      disabled: doingOperation,
      onClick: onInstall.bind(ExtensionDetailRow_this, _package.fileName)
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
      text: localization.get("Install.Button"),
      maxWidth: 60
    }))))
  );
};

common_ExtensionDetailRow_ExtensionDetailRow.propTypes = {
  _package: prop_types_default.a.object,
  type: prop_types_default.a.string,
  onDownload: prop_types_default.a.func,
  onInstall: prop_types_default.a.func,
  onDeploy: prop_types_default.a.func,
  doingOperation: prop_types_default.a.bool
};
/* harmony default export */ var AvailableExtensions_common_ExtensionDetailRow = (common_ExtensionDetailRow_ExtensionDetailRow);
// EXTERNAL MODULE: ./src/components/Body/AvailableExtensions/ExtensionList/style.less
var AvailableExtensions_ExtensionList_style = __webpack_require__(65);

// CONCATENATED MODULE: ./src/components/Body/AvailableExtensions/ExtensionList/index.jsx
function AvailableExtensions_ExtensionList_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { AvailableExtensions_ExtensionList_typeof = function _typeof(obj) { return typeof obj; }; } else { AvailableExtensions_ExtensionList_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return AvailableExtensions_ExtensionList_typeof(obj); }

function AvailableExtensions_ExtensionList_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function AvailableExtensions_ExtensionList_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function AvailableExtensions_ExtensionList_createClass(Constructor, protoProps, staticProps) { if (protoProps) AvailableExtensions_ExtensionList_defineProperties(Constructor.prototype, protoProps); if (staticProps) AvailableExtensions_ExtensionList_defineProperties(Constructor, staticProps); return Constructor; }

function AvailableExtensions_ExtensionList_possibleConstructorReturn(self, call) { if (call && (AvailableExtensions_ExtensionList_typeof(call) === "object" || typeof call === "function")) { return call; } return AvailableExtensions_ExtensionList_assertThisInitialized(self); }

function AvailableExtensions_ExtensionList_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function AvailableExtensions_ExtensionList_getPrototypeOf(o) { AvailableExtensions_ExtensionList_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return AvailableExtensions_ExtensionList_getPrototypeOf(o); }

function AvailableExtensions_ExtensionList_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) AvailableExtensions_ExtensionList_setPrototypeOf(subClass, superClass); }

function AvailableExtensions_ExtensionList_setPrototypeOf(o, p) { AvailableExtensions_ExtensionList_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return AvailableExtensions_ExtensionList_setPrototypeOf(o, p); }








var AvailableExtensions_ExtensionList_ExtensionList =
/*#__PURE__*/
function (_Component) {
  AvailableExtensions_ExtensionList_inherits(ExtensionList, _Component);

  function ExtensionList() {
    AvailableExtensions_ExtensionList_classCallCheck(this, ExtensionList);

    return AvailableExtensions_ExtensionList_possibleConstructorReturn(this, AvailableExtensions_ExtensionList_getPrototypeOf(ExtensionList).call(this));
  }

  AvailableExtensions_ExtensionList_createClass(ExtensionList, [{
    key: "render",
    value: function render() {
      var _this = this;

      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          style: {
            padding: "5px 20px"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(AvailableExtensions_common_ExtensionHeader, null), props.packages.map(function (_package, index) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(AvailableExtensions_common_ExtensionDetailRow, {
              _package: _package,
              type: props.type,
              onInstall: props.onInstall.bind(_this),
              doingOperation: props.doingOperation,
              onDeploy: props.onDeploy.bind(_this, index),
              key: index
            })
          );
        }))
      );
    }
  }]);

  return ExtensionList;
}(external_window_dnn_nodeModules_React_["Component"]);

AvailableExtensions_ExtensionList_ExtensionList.propTypes = {
  label: prop_types_default.a.string,
  packages: prop_types_default.a.array,
  collapsed: prop_types_default.a.bool,
  onChange: prop_types_default.a.func,
  type: prop_types_default.a.string,
  onDownload: prop_types_default.a.func,
  onInstall: prop_types_default.a.func,
  onDeploy: prop_types_default.a.func,
  doingOperation: prop_types_default.a.bool
};
/* harmony default export */ var AvailableExtensions_ExtensionList = (AvailableExtensions_ExtensionList_ExtensionList);
// EXTERNAL MODULE: ./src/components/Body/AvailableExtensions/style.less
var AvailableExtensions_style = __webpack_require__(67);

// CONCATENATED MODULE: ./src/components/Body/AvailableExtensions/index.jsx
function AvailableExtensions_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { AvailableExtensions_typeof = function _typeof(obj) { return typeof obj; }; } else { AvailableExtensions_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return AvailableExtensions_typeof(obj); }

function AvailableExtensions_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function AvailableExtensions_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function AvailableExtensions_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function AvailableExtensions_createClass(Constructor, protoProps, staticProps) { if (protoProps) AvailableExtensions_defineProperties(Constructor.prototype, protoProps); if (staticProps) AvailableExtensions_defineProperties(Constructor, staticProps); return Constructor; }

function AvailableExtensions_possibleConstructorReturn(self, call) { if (call && (AvailableExtensions_typeof(call) === "object" || typeof call === "function")) { return call; } return AvailableExtensions_assertThisInitialized(self); }

function AvailableExtensions_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function AvailableExtensions_getPrototypeOf(o) { AvailableExtensions_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return AvailableExtensions_getPrototypeOf(o); }

function AvailableExtensions_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) AvailableExtensions_setPrototypeOf(subClass, superClass); }

function AvailableExtensions_setPrototypeOf(o, p) { AvailableExtensions_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return AvailableExtensions_setPrototypeOf(o, p); }










var AvailableExtensions_AvailableExtensions =
/*#__PURE__*/
function (_Component) {
  AvailableExtensions_inherits(AvailableExtensions, _Component);

  function AvailableExtensions() {
    var _this;

    AvailableExtensions_classCallCheck(this, AvailableExtensions);

    _this = AvailableExtensions_possibleConstructorReturn(this, AvailableExtensions_getPrototypeOf(AvailableExtensions).call(this));
    _this.state = {
      doingOperation: false,
      loading: false
    };
    return _this;
  }

  AvailableExtensions_createClass(AvailableExtensions, [{
    key: "checkIfAvailablePackageTypesEmpty",
    value: function checkIfAvailablePackageTypesEmpty(props) {
      return !props.availablePackageTypes || props.availablePackageTypes.length === 0;
    }
  }, {
    key: "checkIfAvailablePackagesEmpty",
    value: function checkIfAvailablePackagesEmpty(props) {
      return !props.availablePackages || props.availablePackages.length === 0;
    }
  }, {
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (!this.checkIfAvailablePackageTypesEmpty(props) && this.checkIfAvailablePackagesEmpty(props) && props.selectedAvailablePackageType === "") {
        this.setState({
          loading: true
        }, function () {
          props.dispatch(extension.getAvailablePackages(props.availablePackageTypes[0].Type, function () {
            _this2.setState({
              loading: false
            });
          }));
        });
      }
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      if (props.availablePackages && props.availablePackages.length > 0) {
        this.setState({
          loading: false
        });
      }
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      this.setState(AvailableExtensions_defineProperty({}, key, event.target.value));
    }
  }, {
    key: "onSelectChange",
    value: function onSelectChange(option) {
      this.setState({
        selectValue: option.value
      });
    }
  }, {
    key: "onFilterSelect",
    value: function onFilterSelect(option) {
      var _this3 = this;

      var props = this.props;
      this.setState({
        loading: true
      }, function () {
        props.dispatch(extension.getAvailablePackages(option.value, function () {
          _this3.setState({
            loading: false
          });
        }));
      });
    }
  }, {
    key: "onInstall",
    value: function onInstall(name, event) {
      var _this4 = this;

      if (event) {
        event.preventDefault();
      }

      var props = this.props;
      this.setDoingOperation(true);
      props.dispatch(extension.parseAvailablePackage(name, props.selectedAvailablePackageType, function () {
        _this4.setDoingOperation(false);

        props.dispatch(installation.setInstallingAvailablePackage(name, props.selectedAvailablePackageType, function () {
          props.dispatch(installation.navigateWizard(1, function () {
            props.dispatch(visiblePanel.selectPanel(3));
          }));
        }));
      }));
    }
  }, {
    key: "setDoingOperation",
    value: function setDoingOperation(doingOperation) {
      this.setState({
        doingOperation: doingOperation
      });
    }
  }, {
    key: "onDeploy",
    value: function onDeploy(index, _package, event) {
      var _this5 = this;

      if (event) {
        event.preventDefault();
      }

      var props = this.props;
      this.setDoingOperation(true);
      props.dispatch(extension.deployAvailablePackage(_package, index, function (data) {
        props.dispatch(extension.getAvailablePackages(props.selectedAvailablePackageType, function () {
          var packageToInstall = _this5.props.availablePackages.find(function (_package) {
            return _package.name === data.name;
          });

          _this5.onInstall(packageToInstall.fileName);
        }));
      }));
    }
  }, {
    key: "renderLoading",
    value: function renderLoading() {
      /* eslint-disable react/no-danger */
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "loading-extensions"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h2", null, localization.get("Loading")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", null, localization.get("Loading.Tooltip")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          dangerouslySetInnerHTML: {
            __html: __webpack_require__(14).default
          }
        }))
      );
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "extension-list"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "collapse-section filter-section"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          className: "filter-dropdown",
          onSelect: this.onFilterSelect.bind(this),
          options: props.availablePackageTypes && props.availablePackageTypes.map(function (_package) {
            return {
              label: _package.DisplayName,
              value: _package.Type
            };
          }),
          label: localization.get("Showing.Label"),
          value: props.selectedAvailablePackageType,
          labelType: "inline"
        })), state.loading && this.renderLoading(), props.availablePackages && props.availablePackages.length > 0 && !state.loading &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(AvailableExtensions_ExtensionList, {
          packages: props.availablePackages,
          doingOperation: this.state.doingOperation,
          type: props.selectedAvailablePackageType,
          onInstall: this.onInstall.bind(this),
          onDeploy: this.onDeploy.bind(this)
        }))
      );
    }
  }]);

  return AvailableExtensions;
}(external_window_dnn_nodeModules_React_["Component"]);

AvailableExtensions_AvailableExtensions.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  availablePackages: prop_types_default.a.array,
  tabIndex: prop_types_default.a.number
};

function AvailableExtensions_mapStateToProps(state) {
  return {
    availablePackageTypes: state.extension.availablePackageTypes,
    availablePackages: state.extension.availablePackages,
    selectedAvailablePackageType: state.extension.selectedAvailablePackageType,
    tabIndex: state.pagination.tabIndex
  };
}

/* harmony default export */ var Body_AvailableExtensions = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(AvailableExtensions_mapStateToProps)(AvailableExtensions_AvailableExtensions));
// CONCATENATED MODULE: ./src/components/Body/packageCustomSettings.js
var ModuleCustomSettings = {
  moduleName: "",
  folderName: "",
  category: "",
  businessController: "",
  dependencies: "",
  hostPermissions: "",
  portable: false,
  searchable: false,
  shareable: 2,
  upgradeable: false,
  premiumModule: false
};
var packageCustomSettings_CoreLanguagePack = {
  languageId: 1
};
var packageCustomSettings_ExtensionLanguagePack = {
  dependentPackageId: 1
};
// EXTERNAL MODULE: ./src/components/Body/style.less
var Body_style = __webpack_require__(69);

// CONCATENATED MODULE: ./src/components/Body/index.jsx
function Body_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { Body_typeof = function _typeof(obj) { return typeof obj; }; } else { Body_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return Body_typeof(obj); }

function Body_extends() { Body_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return Body_extends.apply(this, arguments); }

function Body_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function Body_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function Body_createClass(Constructor, protoProps, staticProps) { if (protoProps) Body_defineProperties(Constructor.prototype, protoProps); if (staticProps) Body_defineProperties(Constructor, staticProps); return Constructor; }

function Body_possibleConstructorReturn(self, call) { if (call && (Body_typeof(call) === "object" || typeof call === "function")) { return call; } return Body_assertThisInitialized(self); }

function Body_getPrototypeOf(o) { Body_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return Body_getPrototypeOf(o); }

function Body_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function Body_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) Body_setPrototypeOf(subClass, superClass); }

function Body_setPrototypeOf(o, p) { Body_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return Body_setPrototypeOf(o, p); }













var Body_newExtension = {
  packageType: "",
  name: "",
  friendlyName: "",
  description: "",
  inUse: "",
  upgradeUrl: "",
  packageIcon: "",
  license: "",
  owner: "",
  organization: "",
  url: "",
  email: "",
  version: "0.0.0"
};

var Body_Body =
/*#__PURE__*/
function (_Component) {
  Body_inherits(Body, _Component);

  function Body() {
    var _this;

    Body_classCallCheck(this, Body);

    _this = Body_possibleConstructorReturn(this, Body_getPrototypeOf(Body).call(this));
    _this.handleSelect = _this.handleSelect.bind(Body_assertThisInitialized(_this));
    _this.state = {};
    return _this;
  }

  Body_createClass(Body, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;
      this.isHost = utils["a" /* default */].settings.isHost;

      if (!props.locales || props.locales.length === 0) {
        props.dispatch(extension.getLocaleList());
      }

      if (!props.localePackages || props.localePackages.length === 0) {
        props.dispatch(extension.getLocalePackageList());
      }
    }
  }, {
    key: "handleSelect",
    value: function handleSelect(index
    /*, last*/
    ) {
      var props = this.props;
      props.dispatch(pagination.loadTab(index)); //index acts as scopeTypeId

      this.setState({});
    }
  }, {
    key: "selectPanel",
    value: function selectPanel(panel, event) {
      if (event) {
        event.preventDefault();
      }

      var props = this.props;
      props.dispatch(installation.setIsPortalPackage(false, function () {
        props.dispatch(visiblePanel.selectPanel(panel));
      }));
    }
  }, {
    key: "onEditExtension",
    value: function onEditExtension(extensionBeingEditedIndex, packageId) {
      var _this2 = this;

      var props = this.props;
      props.dispatch(extension.editExtension(packageId, extensionBeingEditedIndex, function () {
        _this2.selectPanel(4);
      }));
    }
  }, {
    key: "createExtension",
    value: function createExtension() {
      var _this3 = this;

      var props = this.props;

      var _newExtension = Body_extends(Body_newExtension, ModuleCustomSettings);

      _newExtension = Body_extends(Body_newExtension, packageCustomSettings_CoreLanguagePack);
      _newExtension = Body_extends(Body_newExtension, packageCustomSettings_ExtensionLanguagePack);
      _newExtension = Body_extends(Body_newExtension, {
        locales: props.locales
      });
      _newExtension = Body_extends(Body_newExtension, {
        packages: props.localePackages
      });
      props.dispatch(extension.addExtension(validationMapExtensionBeingEdited(_newExtension), function () {
        _this3.selectPanel(2);
      }));
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "extension-body"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: localization.get("ExtensionsLabel")
        }, this.isHost &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          size: "large",
          onClick: this.selectPanel.bind(this, 3)
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
          text: localization.get("ExtensionInstall.Action"),
          maxWidth: 120
        })), this.isHost &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          size: "large",
          onClick: this.createExtension.bind(this)
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
          text: localization.get("CreateExtension.Action"),
          maxWidth: 120
        })), this.isHost &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          size: "large",
          onClick: this.selectPanel.bind(this, 1)
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["TextOverflowWrapper"], {
          text: localization.get("CreateModule.Action"),
          maxWidth: 120
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], null, this.isHost &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DnnTabs"], {
          onSelect: this.handleSelect,
          selectedIndex: props.tabIndex,
          tabHeaders: [localization.get("InstalledExtensions"), localization.get("AvailableExtensions")]
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(Body_InstalledExtensions, {
          isHost: this.isHost,
          onEdit: this.onEditExtension.bind(this),
          onCancel: this.selectPanel.bind(this, 0)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(Body_AvailableExtensions, null)), !this.isHost &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(Body_InstalledExtensions, {
          isHost: this.isHost,
          onEdit: this.onEditExtension.bind(this),
          onCancel: this.selectPanel.bind(this, 0)
        })))
      );
    }
  }]);

  return Body;
}(external_window_dnn_nodeModules_React_["Component"]);

Body_Body.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  installedPackages: prop_types_default.a.array,
  tabIndex: prop_types_default.a.number
};

function Body_mapStateToProps(state) {
  return {
    installedPackages: state.extension.installedPackages,
    selectedInstalledPackageType: state.extension.selectedInstalledPackageType,
    tabIndex: state.pagination.tabIndex,
    locales: state.extension.locales,
    localePackages: state.extension.localePackages
  };
}

/* harmony default export */ var components_Body = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(Body_mapStateToProps)(Body_Body));
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/AuthenticationSystem/style.less
var AuthenticationSystem_style = __webpack_require__(29);
var AuthenticationSystem_style_default = /*#__PURE__*/__webpack_require__.n(AuthenticationSystem_style);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/AuthenticationSystem/index.jsx
function AuthenticationSystem_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { AuthenticationSystem_typeof = function _typeof(obj) { return typeof obj; }; } else { AuthenticationSystem_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return AuthenticationSystem_typeof(obj); }

function AuthenticationSystem_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function AuthenticationSystem_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function AuthenticationSystem_createClass(Constructor, protoProps, staticProps) { if (protoProps) AuthenticationSystem_defineProperties(Constructor.prototype, protoProps); if (staticProps) AuthenticationSystem_defineProperties(Constructor, staticProps); return Constructor; }

function AuthenticationSystem_possibleConstructorReturn(self, call) { if (call && (AuthenticationSystem_typeof(call) === "object" || typeof call === "function")) { return call; } return AuthenticationSystem_assertThisInitialized(self); }

function AuthenticationSystem_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function AuthenticationSystem_getPrototypeOf(o) { AuthenticationSystem_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return AuthenticationSystem_getPrototypeOf(o); }

function AuthenticationSystem_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) AuthenticationSystem_setPrototypeOf(subClass, superClass); }

function AuthenticationSystem_setPrototypeOf(o, p) { AuthenticationSystem_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return AuthenticationSystem_setPrototypeOf(o, p); }







var inputStyle = {
  width: "100%"
};

var AuthenticationSystem_AuthenticationSystem =
/*#__PURE__*/
function (_Component) {
  AuthenticationSystem_inherits(AuthenticationSystem, _Component);

  function AuthenticationSystem() {
    AuthenticationSystem_classCallCheck(this, AuthenticationSystem);

    return AuthenticationSystem_possibleConstructorReturn(this, AuthenticationSystem_getPrototypeOf(AuthenticationSystem).apply(this, arguments));
  }

  AuthenticationSystem_createClass(AuthenticationSystem, [{
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: AuthenticationSystem_style_default.a.editAuthenticationSystem + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditAuthSystem_Type.Label"),
          value: extensionBeingEdited.authenticationType.value,
          onChange: props.onChange.bind(this, "authenticationType"),
          tooltipMessage: localization.get("EditAuthSystem_Type.Tooltip"),
          style: inputStyle
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditAuthSystem_LoginCtrlSource.Label"),
          value: extensionBeingEdited.loginControlSource.value,
          onChange: props.onChange.bind(this, "loginControlSource"),
          tooltipMessage: localization.get("EditAuthSystem_LoginCtrlSource.Tooltip"),
          style: inputStyle
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditAuthSystem_LogoffCtrlSource.Label"),
          value: extensionBeingEdited.logoffControlSource.value,
          onChange: props.onChange.bind(this, "logoffControlSource"),
          tooltipMessage: localization.get("EditAuthSystem_LogoffCtrlSource.Tooltip"),
          style: inputStyle
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditAuthSystem_SettingsCtrlSource.Label"),
          value: extensionBeingEdited.settingsControlSource.value,
          onChange: props.onChange.bind(this, "settingsControlSource"),
          tooltipMessage: localization.get("EditAuthSystem_SettingsCtrlSource.Tooltip"),
          style: inputStyle,
          enabled: !props.disabled
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: extensionBeingEdited.enabled.value,
          onChange: props.onChange.bind(this, "enabled"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          label: localization.get("EditAuthSystem_Enabled.Label")
        }))), !props.actionButtonsDisabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this)
        }, props.primaryButtonText)))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return AuthenticationSystem;
}(external_window_dnn_nodeModules_React_["Component"]);

AuthenticationSystem_AuthenticationSystem.PropTypes = {
  onCancel: prop_types_default.a.func,
  onUpdateExtension: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  disabled: prop_types_default.a.func,
  primaryButtonText: prop_types_default.a.string,
  extensionBeingEdited: prop_types_default.a.object,
  extensionBeingEditedIndex: prop_types_default.a.number
};

function AuthenticationSystem_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex
  };
}

/* harmony default export */ var CustomSettings_AuthenticationSystem = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(AuthenticationSystem_mapStateToProps)(AuthenticationSystem_AuthenticationSystem));
// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactCustomScrollBars"
var external_window_dnn_nodeModules_ReactCustomScrollBars_ = __webpack_require__(9);

// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/Module/AssignedSelector/style.less
var AssignedSelector_style = __webpack_require__(30);
var AssignedSelector_style_default = /*#__PURE__*/__webpack_require__.n(AssignedSelector_style);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/Module/AssignedSelector/index.jsx
function AssignedSelector_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { AssignedSelector_typeof = function _typeof(obj) { return typeof obj; }; } else { AssignedSelector_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return AssignedSelector_typeof(obj); }

function AssignedSelector_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function AssignedSelector_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function AssignedSelector_createClass(Constructor, protoProps, staticProps) { if (protoProps) AssignedSelector_defineProperties(Constructor.prototype, protoProps); if (staticProps) AssignedSelector_defineProperties(Constructor, staticProps); return Constructor; }

function AssignedSelector_possibleConstructorReturn(self, call) { if (call && (AssignedSelector_typeof(call) === "object" || typeof call === "function")) { return call; } return AssignedSelector_assertThisInitialized(self); }

function AssignedSelector_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function AssignedSelector_getPrototypeOf(o) { AssignedSelector_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return AssignedSelector_getPrototypeOf(o); }

function AssignedSelector_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) AssignedSelector_setPrototypeOf(subClass, superClass); }

function AssignedSelector_setPrototypeOf(o, p) { AssignedSelector_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return AssignedSelector_setPrototypeOf(o, p); }








var AssignedSelector_AssignedSelector =
/*#__PURE__*/
function (_Component) {
  AssignedSelector_inherits(AssignedSelector, _Component);

  function AssignedSelector() {
    AssignedSelector_classCallCheck(this, AssignedSelector);

    return AssignedSelector_possibleConstructorReturn(this, AssignedSelector_getPrototypeOf(AssignedSelector).call(this));
  }

  AssignedSelector_createClass(AssignedSelector, [{
    key: "getPortalList",
    value: function getPortalList(list, type) {
      var _this = this;

      var props = this.props;
      return list.map(function (portal, index) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("li", {
            className: portal.selected ? "selected" : "",
            onClick: props.onClickOnPortal.bind(_this, index, type),
            key: index
          }, portal.name)
        );
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var assignedPortals = this.getPortalList(props.assignedPortals, "assignedPortals");
      var unassignedPortals = this.getPortalList(props.unassignedPortals, "unassignedPortals");
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: AssignedSelector_style_default.a.assignedSelector
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 45,
          className: "selector-box"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("EditModule_Unassigned.Label")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCustomScrollBars_["Scrollbars"], {
          style: {
            width: "100%",
            height: "100%",
            border: "1px solid #c8c8c8"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("ul", null, unassignedPortals))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 10,
          className: "selector-controls"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          href: "",
          className: "move-item single-right",
          onClick: props.moveItemsRight.bind(this),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].ArrowRightIcon
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          href: "",
          className: "move-item single-left",
          onClick: props.moveItemsLeft.bind(this),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].ArrowLeftIcon
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          href: "",
          className: "move-item double-right",
          onClick: props.moveAll.bind(this, "right"),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].DoubleArrowRightIcon
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          href: "",
          className: "move-item double-left",
          onClick: props.moveAll.bind(this),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].DoubleArrowLeftIcon
          }
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 45,
          className: "selector-box"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("EditModule_Assigned.Label")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCustomScrollBars_["Scrollbars"], {
          style: {
            width: "100%",
            height: "100%",
            border: "1px solid #c8c8c8"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("ul", null, assignedPortals))))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return AssignedSelector;
}(external_window_dnn_nodeModules_React_["Component"]);

AssignedSelector_AssignedSelector.PropTypes = {
  assignedPortals: prop_types_default.a.array,
  unassignedPortals: prop_types_default.a.array
};
/* harmony default export */ var Module_AssignedSelector = (AssignedSelector_AssignedSelector);
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/Module/ModuleDefinitions/ModuleDefinitionRow/Controls/style.less
var Controls_style = __webpack_require__(12);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/Module/ModuleDefinitions/ModuleDefinitionRow/Controls/ControlFields.jsx
function ControlFields_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { ControlFields_typeof = function _typeof(obj) { return typeof obj; }; } else { ControlFields_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return ControlFields_typeof(obj); }

function ControlFields_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function ControlFields_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function ControlFields_createClass(Constructor, protoProps, staticProps) { if (protoProps) ControlFields_defineProperties(Constructor.prototype, protoProps); if (staticProps) ControlFields_defineProperties(Constructor, staticProps); return Constructor; }

function ControlFields_possibleConstructorReturn(self, call) { if (call && (ControlFields_typeof(call) === "object" || typeof call === "function")) { return call; } return ControlFields_assertThisInitialized(self); }

function ControlFields_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function ControlFields_getPrototypeOf(o) { ControlFields_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return ControlFields_getPrototypeOf(o); }

function ControlFields_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) ControlFields_setPrototypeOf(subClass, superClass); }

function ControlFields_setPrototypeOf(o, p) { ControlFields_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return ControlFields_setPrototypeOf(o, p); }






var ControlFields_inputStyle = {
  width: "100%"
};

var ControlFields_ControlFields =
/*#__PURE__*/
function (_Component) {
  ControlFields_inherits(ControlFields, _Component);

  function ControlFields() {
    ControlFields_classCallCheck(this, ControlFields);

    return ControlFields_possibleConstructorReturn(this, ControlFields_getPrototypeOf(ControlFields).call(this));
  }

  ControlFields_createClass(ControlFields, [{
    key: "onSelect",
    value: function onSelect(key, option) {
      var props = this.props;
      props.onChange(key, option.value);
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "edit-module-control-box"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("AddModuleControl_Key.Label"),
          style: ControlFields_inputStyle,
          tooltipMessage: localization.get("AddModuleControl_Key.HelpText"),
          value: props.controlBeingEdited.key,
          onChange: props.onChange.bind(this, "key")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("AddModuleControl_Title.Label"),
          tooltipMessage: localization.get("AddModuleControl_Title.HelpText"),
          style: ControlFields_inputStyle,
          value: props.controlBeingEdited.title,
          onChange: props.onChange.bind(this, "title")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("AddModuleControl_SourceFolder.Label"),
          tooltipMessage: localization.get("AddModuleControl_SourceFolder.HelpText"),
          style: ControlFields_inputStyle,
          options: props.sourceFolders.map(function (folder) {
            return {
              label: folder.Value,
              value: folder.Value
            };
          }),
          onSelect: props.onSelectSourceFolder.bind(this),
          value: props.selectedSourceFolder
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("AddModuleControl_Source.Label") + "*",
          tooltipMessage: localization.get("AddModuleControl_Source.HelpText"),
          style: ControlFields_inputStyle,
          options: props.sourceFiles.map(function (file) {
            return {
              label: file.Value,
              value: file.Value
            };
          }),
          value: props.controlBeingEdited.source,
          error: props.triedToSave && props.error.source,
          onSelect: this.onSelect.bind(this, "source")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("AddModuleControl_Type.Label"),
          tooltipMessage: localization.get("AddModuleControl_Type.HelpText"),
          style: ControlFields_inputStyle,
          options: [{
            label: "Theme Object",
            value: -2
          }, {
            label: "Anonymous",
            value: -1
          }, {
            label: "View",
            value: 0
          }, {
            label: "Edit",
            value: 1
          }, {
            label: "Admin",
            value: 2
          }, {
            label: "Host",
            value: 3
          }],
          value: props.controlBeingEdited.type,
          onSelect: this.onSelect.bind(this, "type")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("AddModuleControl_Icon.Label"),
          tooltipMessage: localization.get("AddModuleControl_Icon.HelpText"),
          style: ControlFields_inputStyle,
          options: props.icons.map(function (icon) {
            return {
              label: icon.Value,
              value: icon.Value
            };
          }),
          value: props.controlBeingEdited.icon,
          onSelect: this.onSelect.bind(this, "icon")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: props.controlBeingEdited.supportPopups,
          label: localization.get("AddModuleControl_SupportsPopups.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          tooltipMessage: localization.get("AddModuleControl_SupportsPopups.HelpText"),
          onChange: props.onChange.bind(this, "supportPopups")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("AddModuleControl_ViewOrder.Label"),
          tooltipMessage: localization.get("AddModuleControl_ViewOrder.HelpText"),
          style: ControlFields_inputStyle,
          value: props.controlBeingEdited.order,
          onChange: props.onChange.bind(this, "order")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("AddModuleControl_HelpURL.Label"),
          tooltipMessage: localization.get("AddModuleControl_HelpURL.HelpText"),
          style: ControlFields_inputStyle,
          value: props.controlBeingEdited.helpUrl,
          onChange: props.onChange.bind(this, "helpUrl")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          label: localization.get("AddModuleControl_SupportsPartialRendering.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          tooltipMessage: localization.get("AddModuleControl_SupportsPartialRendering.HelpText"),
          value: props.controlBeingEdited.supportPartialRendering,
          onChange: props.onChange.bind(this, "supportPartialRendering")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("AddModuleControl_Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this)
        }, localization.get("AddModuleControl_Update.Button"))))
      );
    }
  }]);

  return ControlFields;
}(external_window_dnn_nodeModules_React_["Component"]);

ControlFields_ControlFields.PropTypes = {
  moduleDefinition: prop_types_default.a.object,
  moduleDefinitionBeingEdited: prop_types_default.a.object,
  onCancel: prop_types_default.a.func,
  onSave: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  onEdit: prop_types_default.a.func,
  error: prop_types_default.a.object,
  triedToSave: prop_types_default.a.bool
};
/* harmony default export */ var Controls_ControlFields = (ControlFields_ControlFields);
// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/Module/ModuleDefinitions/ModuleDefinitionRow/Controls/ControlRow.jsx
function ControlRow_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { ControlRow_typeof = function _typeof(obj) { return typeof obj; }; } else { ControlRow_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return ControlRow_typeof(obj); }

function ControlRow_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function ControlRow_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function ControlRow_createClass(Constructor, protoProps, staticProps) { if (protoProps) ControlRow_defineProperties(Constructor.prototype, protoProps); if (staticProps) ControlRow_defineProperties(Constructor, staticProps); return Constructor; }

function ControlRow_possibleConstructorReturn(self, call) { if (call && (ControlRow_typeof(call) === "object" || typeof call === "function")) { return call; } return ControlRow_assertThisInitialized(self); }

function ControlRow_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function ControlRow_getPrototypeOf(o) { ControlRow_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return ControlRow_getPrototypeOf(o); }

function ControlRow_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) ControlRow_setPrototypeOf(subClass, superClass); }

function ControlRow_setPrototypeOf(o, p) { ControlRow_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return ControlRow_setPrototypeOf(o, p); }







var ControlRow_ControlRow =
/*#__PURE__*/
function (_Component) {
  ControlRow_inherits(ControlRow, _Component);

  function ControlRow() {
    var _this;

    ControlRow_classCallCheck(this, ControlRow);

    _this = ControlRow_possibleConstructorReturn(this, ControlRow_getPrototypeOf(ControlRow).call(this));
    _this.state = {
      isOpened: false
    };
    return _this;
  }
  /* eslint-disable react/no-danger */


  ControlRow_createClass(ControlRow, [{
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "module-control-row" + (props.isEditMode ? " row-opened" : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 15,
          className: "module-control-title"
        }, props.moduleControl.title),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 70,
          className: "module-control-source"
        }, props.moduleControl.source),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 15,
          className: "action-buttons"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          onClick: props.onDelete.bind(this),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].TrashIcon
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          onClick: props.onEdit.bind(this),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].EditIcon
          },
          className: props.isEditMode ? "svg-active" : ""
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
          isOpened: props.isEditMode,
          style: {
            float: "left"
          },
          className: "edit-module-control"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(Controls_ControlFields, props)))
      );
    }
  }]);

  return ControlRow;
}(external_window_dnn_nodeModules_React_["Component"]);

ControlRow_ControlRow.propTypes = {
  moduleDefinition: prop_types_default.a.object,
  moduleDefinitionBeingEdited: prop_types_default.a.object,
  onCancel: prop_types_default.a.func,
  onSave: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  onEdit: prop_types_default.a.func
};
/* harmony default export */ var Controls_ControlRow = (ControlRow_ControlRow);
// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/Module/ModuleDefinitions/ModuleDefinitionRow/Controls/index.jsx
function Controls_extends() { Controls_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return Controls_extends.apply(this, arguments); }

function Controls_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { Controls_typeof = function _typeof(obj) { return typeof obj; }; } else { Controls_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return Controls_typeof(obj); }

function Controls_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function Controls_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function Controls_createClass(Constructor, protoProps, staticProps) { if (protoProps) Controls_defineProperties(Constructor.prototype, protoProps); if (staticProps) Controls_defineProperties(Constructor, staticProps); return Constructor; }

function Controls_possibleConstructorReturn(self, call) { if (call && (Controls_typeof(call) === "object" || typeof call === "function")) { return call; } return Controls_assertThisInitialized(self); }

function Controls_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function Controls_getPrototypeOf(o) { Controls_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return Controls_getPrototypeOf(o); }

function Controls_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) Controls_setPrototypeOf(subClass, superClass); }

function Controls_setPrototypeOf(o, p) { Controls_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return Controls_setPrototypeOf(o, p); }













function getSourceFolder(str) {
  return str.substr(0, str.lastIndexOf("/"));
}

function getFileName(str) {
  return str.substr(str.lastIndexOf("/") + 1, str.length - 1);
}

var Controls_Controls =
/*#__PURE__*/
function (_Component) {
  Controls_inherits(Controls, _Component);

  function Controls() {
    var _this;

    Controls_classCallCheck(this, Controls);

    _this = Controls_possibleConstructorReturn(this, Controls_getPrototypeOf(Controls).call(this));
    _this.state = {
      editMode: false,
      controlBeingEdited: {},
      controlBeingEditedIndex: -1,
      error: {
        source: true
      },
      selectedSourceFolder: "Admin/Skins/"
    };
    return _this;
  }

  Controls_createClass(Controls, [{
    key: "getNewControlKeys",
    value: function getNewControlKeys() {
      var props = this.props;
      return {
        id: -1,
        definitionId: props.moduleDefinitionId,
        key: "",
        title: "",
        source: "Admin/Skins/",
        type: -1,
        order: -1,
        icon: "",
        helpUrl: "",
        supportPopups: true,
        supportPartialRendering: true
      };
    }
  }, {
    key: "confirmAction",
    value: function confirmAction(callback) {
      var props = this.props;

      if (props.controlFormIsDirty) {
        utils["a" /* default */].utilities.confirm(localization.get("UnsavedChanges.HelpText"), localization.get("UnsavedChanges.Confirm"), localization.get("UnsavedChanges.Cancel"), function () {
          callback();
          props.dispatch(actions_moduleDefinition.setControlFormDirt(false));
        });
      } else {
        callback();
      }
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var state = this.state,
          props = this.props;
      var value = Controls_typeof(event) === "object" ? event.target.value : event;
      var controlBeingEdited = state.controlBeingEdited,
          error = state.error;
      controlBeingEdited[key] = value;

      if (value === "<None Specified>" && key === "source") {
        error[key] = true;
      } else {
        error[key] = false;
      }

      this.setState({
        controlBeingEdited: controlBeingEdited,
        error: error
      });

      if (!props.controlFormIsDirty) {
        props.dispatch(actions_moduleDefinition.setControlFormDirt(true));
      }

      if (!props.formIsDirty) {
        props.dispatch(actions_moduleDefinition.setFormDirt(true));
      }
    }
  }, {
    key: "onDelete",
    value: function onDelete(controlId) {
      var _this2 = this;

      utils["a" /* default */].utilities.confirm("Are you sure you want to delete this module definition?", "Yes", "No", function () {
        var props = _this2.props;
        var extensionBeingUpdated = JSON.parse(JSON.stringify(props.extensionBeingEdited));
        var actions = {
          deletemodulecontrol: controlId.toString()
        };
        props.dispatch(extension.updateExtension(extensionBeingUpdated, actions, props.extensionBeingEditedIndex, function () {
          utils["a" /* default */].utilities.notify(localization.get("UpdateComplete"));
          props.onControlSave();
        }));
      });
    }
  }, {
    key: "onSave",
    value: function onSave() {
      var _this3 = this;

      var props = this.props,
          state = this.state;
      var triedToSave = state.triedToSave,
          error = state.error;
      triedToSave = true;
      this.setState({
        triedToSave: triedToSave
      });
      var errorCount = 0;
      Object.keys(error).forEach(function (key) {
        if (error[key]) {
          errorCount++;
        }
      });

      if (errorCount > 0) {
        return;
      }

      var extensionBeingUpdated = JSON.parse(JSON.stringify(props.extensionBeingEdited));
      var controls = JSON.parse(JSON.stringify(props.moduleControls));

      if (state.controlBeingEdited.id > -1) {
        controls[state.controlBeingEditedIndex] = state.controlBeingEdited;
      } else {
        controls.push(state.controlBeingEdited);
      }

      state.controlBeingEdited.definitionId = props.moduleDefinitionId;
      var actions = {
        savemodulecontrol: JSON.stringify(state.controlBeingEdited)
      };
      props.dispatch(extension.updateExtension(extensionBeingUpdated, actions, props.extensionBeingEditedIndex, function () {
        utils["a" /* default */].utilities.notify(localization.get("UpdateComplete"));
        props.onControlSave();
        props.dispatch(actions_moduleDefinition.setControlFormDirt(false, function () {
          _this3.exitEditMode();
        }));
      }));
    }
  }, {
    key: "onEdit",
    value: function onEdit(controlBeingEdited, controlBeingEditedIndex) {
      var _this4 = this;

      this.confirmAction(function () {
        var props = _this4.props;
        var sourceFolder = getSourceFolder(controlBeingEdited.source) || "Admin/Skins/";
        props.dispatch(actions_moduleDefinition.getSourceFolders());
        props.dispatch(actions_moduleDefinition.getSourceFiles(sourceFolder, function () {
          props.dispatch(actions_moduleDefinition.getControlIcons(controlBeingEdited.source, function () {
            _this4.setState({
              editMode: true,
              controlBeingEdited: controlBeingEdited,
              controlBeingEditedIndex: controlBeingEditedIndex,
              selectedSourceFolder: sourceFolder,
              error: {
                source: getFileName(controlBeingEdited.source) === ""
              }
            });
          }));
        }));
      });
    }
  }, {
    key: "exitEditMode",
    value: function exitEditMode() {
      var _this5 = this;

      this.confirmAction(function () {
        _this5.setState({
          controlBeingEdited: _this5.getNewControlKeys(),
          controlBeingEditedIndex: null,
          editMode: false,
          triedToSave: false
        });
      });
    }
  }, {
    key: "onSelectSourceFolder",
    value: function onSelectSourceFolder(option) {
      var _this6 = this;

      var props = this.props;
      this.setState({
        selectedSourceFolder: option.value
      });
      props.dispatch(actions_moduleDefinition.getSourceFiles(option.value, function () {
        if (props.sourceFolders.length > 0) {
          props.dispatch(actions_moduleDefinition.getControlIcons(option.value), function () {
            _this6.setState({
              selectedSourceFolder: option.value
            });
          });
        }
      }));
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var _this7 = this;

      var props = this.props,
          state = this.state;
      var moduleControls = props.moduleControls && props.moduleControls.map(function (moduleControl, index) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(Controls_ControlRow, {
            moduleControl: moduleControl,
            controlBeingEdited: state.controlBeingEdited,
            onChange: _this7.onChange.bind(_this7),
            onSave: _this7.onSave.bind(_this7),
            onDelete: _this7.onDelete.bind(_this7, moduleControl.id, index),
            sourceFolders: props.sourceFolders,
            icons: props.icons,
            error: state.error,
            triedToSave: state.triedToSave,
            sourceFiles: props.sourceFiles,
            onSelectSourceFolder: _this7.onSelectSourceFolder.bind(_this7),
            selectedSourceFolder: state.selectedSourceFolder,
            isEditMode: state.controlBeingEditedIndex === index,
            onCancel: _this7.exitEditMode.bind(_this7) // Set definition being edited as null.
            ,
            onEdit: _this7.onEdit.bind(_this7, Controls_extends({}, moduleControl), index),
            key: index
          })
        );
      });
      var isAddMode = state.editMode && state.controlBeingEditedIndex === -1;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "module-controls"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "header-container"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h3", {
          className: "box-title"
        }, localization.get("ModuleDefinitions_ModuleControls.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("a", {
          className: "add-button" + (isAddMode ? " add-active" : ""),
          onClick: this.onEdit.bind(this, this.getNewControlKeys(), -1)
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].AddIcon
          }
        }), " ", localization.get("EditModule_Add.Button"))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          style: {
            padding: 0
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("hr", null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "module-controls-table"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
          isOpened: isAddMode,
          style: {
            float: "left"
          },
          className: "add-control-box" + (isAddMode ? " row-opened" : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(Controls_ControlFields, {
          controlBeingEdited: state.controlBeingEdited,
          onChange: this.onChange.bind(this),
          onSave: this.onSave.bind(this),
          onDelete: this.onDelete.bind(this),
          error: state.error,
          triedToSave: state.triedToSave,
          sourceFolders: props.sourceFolders,
          icons: props.icons,
          sourceFiles: props.sourceFiles,
          onSelectSourceFolder: this.onSelectSourceFolder.bind(this),
          selectedSourceFolder: state.selectedSourceFolder,
          onCancel: this.exitEditMode.bind(this)
          /* Set definition being edited as null. */

        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 15,
          className: "module-control-title-header"
        }, localization.get("AddModuleControl_Title.Label")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 85,
          className: "module-control-source-header"
        }, localization.get("AddModuleControl_Source.Label")), moduleControls))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return Controls;
}(external_window_dnn_nodeModules_React_["Component"]);

Controls_Controls.propTypes = {
  dispatch: prop_types_default.a.func.isRequired
};

function Controls_mapStateToProps(state) {
  return {
    sourceFolders: state.moduleDefinition.sourceFolders,
    sourceFiles: state.moduleDefinition.sourceFiles,
    formIsDirty: state.moduleDefinition.formIsDirty,
    controlFormIsDirty: state.moduleDefinition.controlFormIsDirty,
    icons: state.moduleDefinition.icons
  };
}

/* harmony default export */ var ModuleDefinitionRow_Controls = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(Controls_mapStateToProps)(Controls_Controls));
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/Module/ModuleDefinitions/style.less
var ModuleDefinitions_style = __webpack_require__(15);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/Module/ModuleDefinitions/DefinitionFields.jsx
function DefinitionFields_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { DefinitionFields_typeof = function _typeof(obj) { return typeof obj; }; } else { DefinitionFields_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return DefinitionFields_typeof(obj); }

function DefinitionFields_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function DefinitionFields_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function DefinitionFields_createClass(Constructor, protoProps, staticProps) { if (protoProps) DefinitionFields_defineProperties(Constructor.prototype, protoProps); if (staticProps) DefinitionFields_defineProperties(Constructor, staticProps); return Constructor; }

function DefinitionFields_possibleConstructorReturn(self, call) { if (call && (DefinitionFields_typeof(call) === "object" || typeof call === "function")) { return call; } return DefinitionFields_assertThisInitialized(self); }

function DefinitionFields_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function DefinitionFields_getPrototypeOf(o) { DefinitionFields_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return DefinitionFields_getPrototypeOf(o); }

function DefinitionFields_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) DefinitionFields_setPrototypeOf(subClass, superClass); }

function DefinitionFields_setPrototypeOf(o, p) { DefinitionFields_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return DefinitionFields_setPrototypeOf(o, p); }







var DefinitionFields_DefinitionFields =
/*#__PURE__*/
function (_Component) {
  DefinitionFields_inherits(DefinitionFields, _Component);

  function DefinitionFields() {
    DefinitionFields_classCallCheck(this, DefinitionFields);

    return DefinitionFields_possibleConstructorReturn(this, DefinitionFields_getPrototypeOf(DefinitionFields).call(this));
  }
  /* eslint-disable react/no-danger */


  DefinitionFields_createClass(DefinitionFields, [{
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("ModuleDefinitions_DefinitionName.Label") + "*",
          tooltipMessage: localization.get("ModuleDefinitions_DefinitionName.HelpText"),
          onChange: props.onChange.bind(this, "name"),
          enabled: !props.isEditMode,
          error: props.error.name && props.triedToSave,
          value: props.moduleDefinitionBeingEdited.name
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("ModuleDefinitions_DefaultCacheTime.Label"),
          tooltipMessage: localization.get("ModuleDefinitions_DefaultCacheTime.HelpText"),
          onChange: props.onChange.bind(this, "cacheTime"),
          value: props.moduleDefinitionBeingEdited.cacheTime
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("ModuleDefinitions_FriendlyName.Label") + "*",
          tooltipMessage: localization.get("ModuleDefinitions_FriendlyName.HelpText"),
          error: props.error.friendlyName && props.triedToSave,
          onChange: props.onChange.bind(this, "friendlyName"),
          value: props.moduleDefinitionBeingEdited.friendlyName
        })))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return DefinitionFields;
}(external_window_dnn_nodeModules_React_["Component"]);

DefinitionFields_DefinitionFields.propTypes = {
  onChange: prop_types_default.a.func,
  error: prop_types_default.a.object,
  triedToSave: prop_types_default.a.bool,
  isEditMode: prop_types_default.a.bool,
  moduleDefinitionBeingEdited: prop_types_default.a.object
};
/* harmony default export */ var ModuleDefinitions_DefinitionFields = (DefinitionFields_DefinitionFields);
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/Module/ModuleDefinitions/ModuleDefinitionRow/style.less
var ModuleDefinitionRow_style = __webpack_require__(75);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/Module/ModuleDefinitions/ModuleDefinitionRow/index.jsx
function ModuleDefinitionRow_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { ModuleDefinitionRow_typeof = function _typeof(obj) { return typeof obj; }; } else { ModuleDefinitionRow_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return ModuleDefinitionRow_typeof(obj); }

function ModuleDefinitionRow_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function ModuleDefinitionRow_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function ModuleDefinitionRow_createClass(Constructor, protoProps, staticProps) { if (protoProps) ModuleDefinitionRow_defineProperties(Constructor.prototype, protoProps); if (staticProps) ModuleDefinitionRow_defineProperties(Constructor, staticProps); return Constructor; }

function ModuleDefinitionRow_possibleConstructorReturn(self, call) { if (call && (ModuleDefinitionRow_typeof(call) === "object" || typeof call === "function")) { return call; } return ModuleDefinitionRow_assertThisInitialized(self); }

function ModuleDefinitionRow_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function ModuleDefinitionRow_getPrototypeOf(o) { ModuleDefinitionRow_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return ModuleDefinitionRow_getPrototypeOf(o); }

function ModuleDefinitionRow_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) ModuleDefinitionRow_setPrototypeOf(subClass, superClass); }

function ModuleDefinitionRow_setPrototypeOf(o, p) { ModuleDefinitionRow_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return ModuleDefinitionRow_setPrototypeOf(o, p); }









var ModuleDefinitionRow_ModuleDefinitionRow =
/*#__PURE__*/
function (_Component) {
  ModuleDefinitionRow_inherits(ModuleDefinitionRow, _Component);

  function ModuleDefinitionRow() {
    var _this;

    ModuleDefinitionRow_classCallCheck(this, ModuleDefinitionRow);

    _this = ModuleDefinitionRow_possibleConstructorReturn(this, ModuleDefinitionRow_getPrototypeOf(ModuleDefinitionRow).call(this));
    _this.state = {
      isOpened: false
    };
    return _this;
  }
  /* eslint-disable react/no-danger */


  ModuleDefinitionRow_createClass(ModuleDefinitionRow, [{
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "module-definition-row " + (props.isEditMode ? " row-opened" : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 85,
          className: "module-definition-name"
        }, props.moduleDefinition.name),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 15,
          className: "action-buttons"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          onClick: props.onDelete.bind(this),
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].TrashIcon
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          onClick: props.onEdit.bind(this),
          className: props.isEditMode ? "svg-active" : "",
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].EditIcon
          }
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
          isOpened: props.isEditMode,
          style: {
            float: "left"
          },
          className: "edit-module-definition"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "edit-module-definition-box"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(ModuleDefinitions_DefinitionFields, {
          error: props.error,
          triedToSave: props.triedToSave,
          onChange: props.onChange.bind(this),
          isEditMode: true,
          moduleDefinitionBeingEdited: props.moduleDefinitionBeingEdited
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "module-controls"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(ModuleDefinitionRow_Controls, {
          moduleControls: props.moduleDefinitionBeingEdited.controls,
          extensionBeingEdited: props.extensionBeingEdited,
          extensionBeingEditedIndex: props.extensionBeingEditedIndex,
          onChange: props.onChange.bind(this, "controls"),
          onControlSave: props.onSave.bind(this),
          moduleDefinitionId: props.moduleDefinitionBeingEdited.id
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("ModuleDefinitions_Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          disabled: props.controlFormIsDirty,
          onClick: props.onSave.bind(this)
        }, localization.get("ModuleDefinitions_Save.Button"))))))
      );
    }
  }]);

  return ModuleDefinitionRow;
}(external_window_dnn_nodeModules_React_["Component"]);

ModuleDefinitionRow_ModuleDefinitionRow.propTypes = {
  moduleDefinition: prop_types_default.a.object,
  moduleDefinitionBeingEdited: prop_types_default.a.object,
  onCancel: prop_types_default.a.func,
  onSave: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  onEdit: prop_types_default.a.func,
  error: prop_types_default.a.object
};
/* harmony default export */ var ModuleDefinitions_ModuleDefinitionRow = (ModuleDefinitionRow_ModuleDefinitionRow);
// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/Module/ModuleDefinitions/index.jsx
function ModuleDefinitions_extends() { ModuleDefinitions_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return ModuleDefinitions_extends.apply(this, arguments); }

function ModuleDefinitions_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { ModuleDefinitions_typeof = function _typeof(obj) { return typeof obj; }; } else { ModuleDefinitions_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return ModuleDefinitions_typeof(obj); }

function ModuleDefinitions_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function ModuleDefinitions_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function ModuleDefinitions_createClass(Constructor, protoProps, staticProps) { if (protoProps) ModuleDefinitions_defineProperties(Constructor.prototype, protoProps); if (staticProps) ModuleDefinitions_defineProperties(Constructor, staticProps); return Constructor; }

function ModuleDefinitions_possibleConstructorReturn(self, call) { if (call && (ModuleDefinitions_typeof(call) === "object" || typeof call === "function")) { return call; } return ModuleDefinitions_assertThisInitialized(self); }

function ModuleDefinitions_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function ModuleDefinitions_getPrototypeOf(o) { ModuleDefinitions_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return ModuleDefinitions_getPrototypeOf(o); }

function ModuleDefinitions_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) ModuleDefinitions_setPrototypeOf(subClass, superClass); }

function ModuleDefinitions_setPrototypeOf(o, p) { ModuleDefinitions_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return ModuleDefinitions_setPrototypeOf(o, p); }














var ModuleDefinitions_ModuleDefinitions =
/*#__PURE__*/
function (_Component) {
  ModuleDefinitions_inherits(ModuleDefinitions, _Component);

  function ModuleDefinitions() {
    var _this;

    ModuleDefinitions_classCallCheck(this, ModuleDefinitions);

    _this = ModuleDefinitions_possibleConstructorReturn(this, ModuleDefinitions_getPrototypeOf(ModuleDefinitions).call(this));
    _this.state = {
      moduleDefinitionBeingEdited: {},
      moduleDefinitionBeingEditedIndex: null,
      error: {
        name: false,
        friendlyName: false
      },
      triedToSave: false,
      editMode: false
    };
    return _this;
  }

  ModuleDefinitions_createClass(ModuleDefinitions, [{
    key: "getNewModuleDefinition",
    value: function getNewModuleDefinition() {
      var props = this.props;
      return {
        id: -1,
        desktopModuleId: props.desktopModuleId,
        name: "",
        friendlyName: "",
        cacheTime: 0
      };
    }
  }, {
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      this.setState({
        moduleDefinitionBeingEdited: this.getNewModuleDefinition()
      });
    }
  }, {
    key: "confirmAction",
    value: function confirmAction(callback) {
      var props = this.props;

      if (props.formIsDirty) {
        utils["a" /* default */].utilities.confirm(localization.get("UnsavedChanges.HelpText"), localization.get("UnsavedChanges.Confirm"), localization.get("UnsavedChanges.Cancel"), function () {
          callback();
          props.dispatch(actions_moduleDefinition.setFormDirt(false));

          if (props.controlFormIsDirty) {
            props.dispatch(actions_moduleDefinition.setControlFormDirt(false));
          }
        });
      } else {
        callback();
      }
    }
  }, {
    key: "resetError",
    value: function resetError() {
      return {
        name: false,
        friendlyName: false
      };
    }
  }, {
    key: "exitEditMode",
    value: function exitEditMode() {
      var _this2 = this;

      this.confirmAction(function () {
        _this2.setState({
          moduleDefinitionBeingEdited: _this2.getNewModuleDefinition(),
          error: _this2.resetError(),
          moduleDefinitionBeingEditedIndex: null,
          editMode: false,
          triedToSave: false
        });
      });
    }
  }, {
    key: "_onEditModuleDefinition",
    value: function _onEditModuleDefinition(moduleDefinitionBeingEdited, moduleDefinitionBeingEditedIndex) {
      this.setState({
        editMode: true,
        error: {
          name: moduleDefinitionBeingEdited.name === "",
          friendlyName: moduleDefinitionBeingEdited.friendlyName === ""
        },
        moduleDefinitionBeingEdited: moduleDefinitionBeingEdited,
        moduleDefinitionBeingEditedIndex: moduleDefinitionBeingEditedIndex
      });
    }
  }, {
    key: "onEditModuleDefinition",
    value: function onEditModuleDefinition(moduleDefinitionBeingEdited, moduleDefinitionBeingEditedIndex) {
      var _this3 = this;

      this.confirmAction(function () {
        _this3._onEditModuleDefinition(moduleDefinitionBeingEdited, moduleDefinitionBeingEditedIndex);
      });
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var props = this.props,
          state = this.state;
      var value = ModuleDefinitions_typeof(event) === "object" ? event.target.value : event;
      var moduleDefinitionBeingEdited = state.moduleDefinitionBeingEdited,
          error = state.error;
      moduleDefinitionBeingEdited[key] = value;

      if (value === "" && (key === "friendlyName" || key === "name")) {
        error[key] = true;
      } else {
        error[key] = false;
      }

      this.setState({
        moduleDefinitionBeingEdited: moduleDefinitionBeingEdited,
        error: error
      });

      if (!props.formIsDirty) {
        props.dispatch(actions_moduleDefinition.setFormDirt(true));
      }
    }
  }, {
    key: "onSave",
    value: function onSave() {
      var _this4 = this;

      var props = this.props,
          state = this.state;
      var triedToSave = state.triedToSave,
          error = state.error;
      triedToSave = true;
      this.setState({
        triedToSave: triedToSave
      });
      var errorCount = 0;
      Object.keys(error).forEach(function (key) {
        if (error[key]) {
          errorCount++;
        }
      });

      if (errorCount > 0) {
        return;
      }

      var extensionBeingUpdated = JSON.parse(JSON.stringify(props.extensionBeingEdited));
      var moduleDefinitions = extensionBeingUpdated.moduleDefinitions.value;

      if (state.moduleDefinitionBeingEditedIndex > -1) {
        moduleDefinitions[state.moduleDefinitionBeingEditedIndex] = state.moduleDefinitionBeingEdited;
      } else {
        moduleDefinitions.push(state.moduleDefinitionBeingEdited);
      }

      state.moduleDefinitionBeingEdited.desktopModuleId = props.extensionBeingEdited.desktopModuleId.value;
      var actions = {
        savedefinition: JSON.stringify(state.moduleDefinitionBeingEdited)
      };
      props.dispatch(extension.updateExtension(extensionBeingUpdated, actions, props.extensionBeingEditedIndex, function () {
        props.dispatch(actions_moduleDefinition.setFormDirt(false, function () {
          _this4.exitEditMode();
        }));
      }));
    }
  }, {
    key: "onControlSave",
    value: function onControlSave() {
      var _this5 = this;

      var props = this.props,
          state = this.state;
      var extensionBeingUpdated = JSON.parse(JSON.stringify(props.extensionBeingEdited));
      state.moduleDefinitionBeingEdited.desktopModuleId = props.extensionBeingEdited.desktopModuleId.value;
      var actions = {
        savedefinition: JSON.stringify(state.moduleDefinitionBeingEdited)
      };
      props.dispatch(extension.updateExtension(extensionBeingUpdated, actions, props.extensionBeingEditedIndex, function () {
        props.dispatch(actions_moduleDefinition.setFormDirt(false, function () {
          _this5.exitEditMode();
        }));
      }));
    }
  }, {
    key: "onDelete",
    value: function onDelete(definitionId) {
      var _this6 = this;

      utils["a" /* default */].utilities.confirm("Are you sure you want to delete this module definition?", "Yes", "No", function () {
        var props = _this6.props;
        var extensionBeingUpdated = JSON.parse(JSON.stringify(props.extensionBeingEdited));
        var actions = {
          deletedefinition: definitionId.toString()
        };
        props.dispatch(extension.updateExtension(extensionBeingUpdated, actions, props.extensionBeingEditedIndex, function () {
          utils["a" /* default */].utilities.notify(localization.get("UpdateComplete"));
        }));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var _this7 = this;

      var props = this.props,
          state = this.state;
      var moduleDefinitions = props.extensionBeingEdited.moduleDefinitions.value.map(function (moduleDefinition, index) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(ModuleDefinitions_ModuleDefinitionRow, {
            moduleDefinition: moduleDefinition,
            moduleDefinitionBeingEdited: state.moduleDefinitionBeingEdited,
            extensionBeingEdited: props.extensionBeingEdited,
            extensionBeingEditedIndex: props.extensionBeingEditedIndex,
            onChange: _this7.onChange.bind(_this7),
            onSave: _this7.onControlSave.bind(_this7),
            error: state.error,
            triedToSave: state.triedToSave,
            controlFormIsDirty: props.controlFormIsDirty,
            onDelete: _this7.onDelete.bind(_this7, moduleDefinition.id, index),
            isEditMode: state.moduleDefinitionBeingEditedIndex === index,
            onCancel: _this7.exitEditMode.bind(_this7) // Set definition being edited as null.
            ,
            onEdit: _this7.onEditModuleDefinition.bind(_this7, ModuleDefinitions_extends({}, moduleDefinition), index),
            key: index
          })
        );
      });
      var isAddMode = state.editMode && state.moduleDefinitionBeingEditedIndex === -1;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "module-definitions"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "header-container"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h3", {
          className: "box-title"
        }, localization.get("EditModule_ModuleDefinitions.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("a", {
          className: "add-button" + (isAddMode ? " add-active" : ""),
          onClick: this.onEditModuleDefinition.bind(this, this.getNewModuleDefinition(), -1)
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].AddIcon
          },
          className: isAddMode ? "svg-active" : ""
        }), " ", localization.get("EditModule_Add.Button"))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          style: {
            padding: 0
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("hr", null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "module-definitions-table"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Collapsible"], {
          isOpened: isAddMode,
          fixedHeight: 300,
          style: {
            float: "left"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "add-module-definition-box" + (isAddMode ? " row-opened" : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(ModuleDefinitions_DefinitionFields, {
          onChange: this.onChange.bind(this),
          onControlSave: this.onSave.bind(this),
          error: state.error,
          triedToSave: state.triedToSave,
          isEditMode: false,
          moduleDefinitionBeingEdited: state.moduleDefinitionBeingEdited
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: this.exitEditMode.bind(this)
        }, localization.get("ModuleDefinitions_Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.onSave.bind(this)
        }, localization.get("ModuleDefinitions_Save.Button"))))), moduleDefinitions))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return ModuleDefinitions;
}(external_window_dnn_nodeModules_React_["Component"]);

ModuleDefinitions_ModuleDefinitions.propTypes = {
  dispatch: prop_types_default.a.func.isRequired
};

function ModuleDefinitions_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    controlFormIsDirty: state.moduleDefinition.controlFormIsDirty
  };
}

/* harmony default export */ var Module_ModuleDefinitions = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(ModuleDefinitions_mapStateToProps)(ModuleDefinitions_ModuleDefinitions));
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/Module/style.less
var Module_style = __webpack_require__(31);
var Module_style_default = /*#__PURE__*/__webpack_require__.n(Module_style);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/Module/index.jsx
function Module_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { Module_typeof = function _typeof(obj) { return typeof obj; }; } else { Module_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return Module_typeof(obj); }

function Module_extends() { Module_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return Module_extends.apply(this, arguments); }

function Module_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function Module_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function Module_createClass(Constructor, protoProps, staticProps) { if (protoProps) Module_defineProperties(Constructor.prototype, protoProps); if (staticProps) Module_defineProperties(Constructor, staticProps); return Constructor; }

function Module_possibleConstructorReturn(self, call) { if (call && (Module_typeof(call) === "object" || typeof call === "function")) { return call; } return Module_assertThisInitialized(self); }

function Module_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function Module_getPrototypeOf(o) { Module_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return Module_getPrototypeOf(o); }

function Module_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) Module_setPrototypeOf(subClass, superClass); }

function Module_setPrototypeOf(o, p) { Module_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return Module_setPrototypeOf(o, p); }









var Module_inputStyle = {
  width: "100%"
};

var Module_Module =
/*#__PURE__*/
function (_Component) {
  Module_inherits(Module, _Component);

  function Module() {
    Module_classCallCheck(this, Module);

    return Module_possibleConstructorReturn(this, Module_getPrototypeOf(Module).apply(this, arguments));
  }

  Module_createClass(Module, [{
    key: "onClickOnPortal",
    value: function onClickOnPortal(index, type) {
      var props = this.props;

      var _extensionBeingEdited = JSON.parse(JSON.stringify(props.extensionBeingEdited));

      _extensionBeingEdited[type].value[index].selected = !_extensionBeingEdited[type].value[index].selected;
      props.onAssignedPortalsChange(type, _extensionBeingEdited[type].value);
    }
  }, {
    key: "moveItemsLeft",
    value: function moveItemsLeft() {
      var props = this.props;
      var assignedPortals = JSON.parse(JSON.stringify(props.extensionBeingEdited.assignedPortals.value));
      var unassignedPortals = JSON.parse(JSON.stringify(props.extensionBeingEdited.unassignedPortals.value));
      var itemsToStay = [],
          itemsToMove = [];
      var selectedCount = 0;
      assignedPortals.forEach(function (portal) {
        var selected = portal.selected;
        delete portal.selected;

        if (selected) {
          selectedCount++;
          itemsToMove.push(portal);
        } else {
          itemsToStay.push(portal);
        }
      });

      if (selectedCount > 0) {
        props.onAssignedPortalsChange("unassignedPortals", unassignedPortals.concat(itemsToMove), function () {
          props.onAssignedPortalsChange("assignedPortals", itemsToStay);
        });
      }
    }
  }, {
    key: "moveItemsRight",
    value: function moveItemsRight() {
      var props = this.props;
      var unassignedPortals = JSON.parse(JSON.stringify(props.extensionBeingEdited.unassignedPortals.value));
      var assignedPortals = JSON.parse(JSON.stringify(props.extensionBeingEdited.assignedPortals.value));
      var itemsToStay = [],
          itemsToMove = [];
      var selectedCount = 0;
      unassignedPortals.forEach(function (portal) {
        var selected = portal.selected;
        delete portal.selected;

        if (selected) {
          selectedCount++;
          itemsToMove.push(portal);
        } else {
          itemsToStay.push(portal);
        }
      });

      if (selectedCount > 0) {
        props.onAssignedPortalsChange("assignedPortals", assignedPortals.concat(itemsToMove), function () {
          props.onAssignedPortalsChange("unassignedPortals", itemsToStay);
        });
      }
    }
  }, {
    key: "moveAll",
    value: function moveAll(direction) {
      var props = this.props;
      var assignedPortals = JSON.parse(JSON.stringify(props.extensionBeingEdited.assignedPortals.value));
      var unassignedPortals = JSON.parse(JSON.stringify(props.extensionBeingEdited.unassignedPortals.value));

      switch (direction) {
        case "right":
          props.onAssignedPortalsChange("assignedPortals", assignedPortals.concat(unassignedPortals), function () {
            props.onAssignedPortalsChange("unassignedPortals", []);
          });
          break;

        default:
          props.onAssignedPortalsChange("unassignedPortals", unassignedPortals.concat(assignedPortals), function () {
            props.onAssignedPortalsChange("assignedPortals", []);
          });
          break;
      }
    }
  }, {
    key: "onSelect",
    value: function onSelect(key, option) {
      var props = this.props;
      props.onChange(key, option.value);
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: Module_style_default.a.editModule + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null, !props.isAddMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditModule_ModuleName.Label"),
          tooltipMessage: localization.get("EditModule_ModuleName.HelpText"),
          style: Module_inputStyle,
          onChange: props.onChange.bind(this, "moduleName"),
          value: extensionBeingEdited.moduleName.value,
          enabled: false
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("EditModule_ModuleCategory.Label"),
          tooltipMessage: localization.get("EditModule_ModuleCategory.HelpText"),
          options: props.moduleCategories.map(function (category) {
            return {
              label: category.replace("&lt;", "<").replace("&gt;", ">"),
              value: category
            };
          }),
          value: extensionBeingEdited.category.value,
          onSelect: this.onSelect.bind(this, "category"),
          style: Module_extends({
            marginBottom: 32
          }, Module_inputStyle)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditModule_Dependencies.Label"),
          tooltipMessage: localization.get("EditModule_Dependencies.HelpText"),
          value: extensionBeingEdited.dependencies.value,
          onChange: props.onChange.bind(this, "dependencies"),
          style: Module_inputStyle
        }), props.isAddMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("EditModule_ModuleSharing.Label"),
          tooltipMessage: localization.get("EditModule_ModuleSharing.HelpText"),
          options: [{
            label: localization.get("EditModule_ModuleSharingUnknown.Label"),
            value: 0
          }, {
            label: localization.get("EditModule_ModuleSharingUnsupported.Label"),
            value: 1
          }, {
            label: localization.get("EditModule_ModuleSharingSupported.Label"),
            value: 2
          }],
          value: extensionBeingEdited.shareable.value,
          onSelect: this.onSelect.bind(this, "shareable"),
          style: Module_extends({
            marginBottom: 32
          }, Module_inputStyle)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: extensionBeingEdited.portable.value,
          readOnly: true,
          className: "full-width",
          label: localization.get("EditModule_IsPortable.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          tooltipMessage: localization.get("EditModule_IsPortable.HelpText")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: extensionBeingEdited.upgradeable.value,
          readOnly: true,
          className: "full-width",
          label: localization.get("EditModule_IsUpgradable.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          tooltipMessage: localization.get("EditModule_IsUpgradable.HelpText")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditModule_FolderName.Label"),
          tooltipMessage: localization.get("EditModule_FolderName.HelpText"),
          value: extensionBeingEdited.folderName.value,
          onChange: props.onChange.bind(this, "folderName"),
          style: Module_inputStyle
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditModule_BusinessControllerClass.Label"),
          tooltipMessage: localization.get("EditModule_BusinessControllerClass.HelpText"),
          onChange: props.onChange.bind(this, "businessController"),
          value: extensionBeingEdited.businessController.value,
          style: Module_inputStyle
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditModule_Permissions.Label"),
          value: extensionBeingEdited.hostPermissions.value,
          onChange: props.onChange.bind(this, "hostPermissions"),
          tooltipMessage: localization.get("EditModule_Permissions.HelpText"),
          style: Module_inputStyle
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: extensionBeingEdited.searchable.value,
          readOnly: true,
          className: "full-width",
          label: localization.get("EditModule_IsSearchable.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          tooltipMessage: localization.get("EditModule_IsSearchable.HelpText")
        }), props.isAddMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: extensionBeingEdited.premiumModule.value,
          label: localization.get("EditModule_IsPremiumModule.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          onChange: props.onChange.bind(this, "premiumModule"),
          tooltipMessage: localization.get("EditModule_IsPremiumModule.HelpText")
        }), !props.isAddMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("EditModule_ModuleSharing.Label"),
          tooltipMessage: localization.get("EditModule_ModuleSharing.HelpText"),
          options: [{
            label: "Unknown",
            value: 0
          }, {
            label: "Unsupported",
            value: 1
          }, {
            label: "Supported",
            value: 2
          }],
          value: extensionBeingEdited.shareable.value,
          onSelect: this.onSelect.bind(this, "shareable"),
          style: Module_inputStyle
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("hr", null)), !props.isAddMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "premium-module"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h3", {
          className: "box-title"
        }, localization.get("EditModule_PremiumModuleAssignment.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: extensionBeingEdited.premiumModule.value,
          label: localization.get("EditModule_IsPremiumModule.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          onChange: props.onChange.bind(this, "premiumModule"),
          tooltipMessage: localization.get("EditModule_IsPremiumModule.HelpText")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(Module_AssignedSelector, {
          assignedPortals: extensionBeingEdited.assignedPortals.value,
          unassignedPortals: extensionBeingEdited.unassignedPortals.value,
          onClickOnPortal: this.onClickOnPortal.bind(this),
          moveItemsLeft: this.moveItemsLeft.bind(this),
          moveItemsRight: this.moveItemsRight.bind(this),
          moveAll: this.moveAll.bind(this),
          onChange: props.onChange.bind(this)
        })), !props.isAddMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(Module_ModuleDefinitions, {
          extensionBeingEdited: extensionBeingEdited,
          extensionBeingEditedIndex: props.extensionBeingEditedIndex,
          onSave: props.onChange.bind(this, "moduleDefinitions")
        }), !props.actionButtonsDisabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          disabled: props.formIsDirty || props.controlFormIsDirty,
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          disabled: props.formIsDirty || props.controlFormIsDirty,
          onClick: props.onSave.bind(this)
        }, localization.get("Save.Button"))))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return Module;
}(external_window_dnn_nodeModules_React_["Component"]);

Module_Module.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  onCancel: prop_types_default.a.func,
  onSave: prop_types_default.a.func,
  onUpdateExtension: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  disabled: prop_types_default.a.func,
  primaryButtonText: prop_types_default.a.string,
  extensionBeingEdited: prop_types_default.a.object,
  isAddMode: prop_types_default.a.object
};

function Module_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex,
    formIsDirty: state.moduleDefinition.formIsDirty,
    controlFormIsDirty: state.moduleDefinition.controlFormIsDirty,
    moduleCategories: state.extension.moduleCategories
  };
}

/* harmony default export */ var CustomSettings_Module = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(Module_mapStateToProps)(Module_Module));
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/Container/style.less
var Container_style = __webpack_require__(32);
var Container_style_default = /*#__PURE__*/__webpack_require__.n(Container_style);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/Container/index.jsx
function Container_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { Container_typeof = function _typeof(obj) { return typeof obj; }; } else { Container_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return Container_typeof(obj); }

function Container_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function Container_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function Container_createClass(Constructor, protoProps, staticProps) { if (protoProps) Container_defineProperties(Constructor.prototype, protoProps); if (staticProps) Container_defineProperties(Constructor, staticProps); return Constructor; }

function Container_possibleConstructorReturn(self, call) { if (call && (Container_typeof(call) === "object" || typeof call === "function")) { return call; } return Container_assertThisInitialized(self); }

function Container_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function Container_getPrototypeOf(o) { Container_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return Container_getPrototypeOf(o); }

function Container_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) Container_setPrototypeOf(subClass, superClass); }

function Container_setPrototypeOf(o, p) { Container_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return Container_setPrototypeOf(o, p); }







var Container_inputStyle = {
  width: "100%"
};

var Container_Container =
/*#__PURE__*/
function (_Component) {
  Container_inherits(Container, _Component);

  function Container() {
    Container_classCallCheck(this, Container);

    return Container_possibleConstructorReturn(this, Container_getPrototypeOf(Container).apply(this, arguments));
  }

  Container_createClass(Container, [{
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: Container_style_default.a.editContainer + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditContainer_ThemePackageName.Label"),
          value: extensionBeingEdited.themePackageName.value,
          onChange: props.onChange.bind(this, "themePackageName"),
          enabled: !props.disabled,
          tooltipMessage: localization.get("EditContainer_ThemePackageName.HelpText"),
          style: Container_inputStyle
        })), !props.actionButtonsDisabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this)
        }, props.primaryButtonText)))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return Container;
}(external_window_dnn_nodeModules_React_["Component"]);

Container_Container.propTypes = {
  onCancel: prop_types_default.a.func,
  onUpdateExtension: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  disabled: prop_types_default.a.func,
  primaryButtonText: prop_types_default.a.string
};

function Container_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex
  };
}

/* harmony default export */ var CustomSettings_Container = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(Container_mapStateToProps)(Container_Container));
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/JavascriptLibrary/style.less
var JavascriptLibrary_style = __webpack_require__(33);
var JavascriptLibrary_style_default = /*#__PURE__*/__webpack_require__.n(JavascriptLibrary_style);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/JavascriptLibrary/index.jsx
function JavascriptLibrary_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { JavascriptLibrary_typeof = function _typeof(obj) { return typeof obj; }; } else { JavascriptLibrary_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return JavascriptLibrary_typeof(obj); }

function JavascriptLibrary_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function JavascriptLibrary_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function JavascriptLibrary_createClass(Constructor, protoProps, staticProps) { if (protoProps) JavascriptLibrary_defineProperties(Constructor.prototype, protoProps); if (staticProps) JavascriptLibrary_defineProperties(Constructor, staticProps); return Constructor; }

function JavascriptLibrary_possibleConstructorReturn(self, call) { if (call && (JavascriptLibrary_typeof(call) === "object" || typeof call === "function")) { return call; } return JavascriptLibrary_assertThisInitialized(self); }

function JavascriptLibrary_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function JavascriptLibrary_getPrototypeOf(o) { JavascriptLibrary_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return JavascriptLibrary_getPrototypeOf(o); }

function JavascriptLibrary_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) JavascriptLibrary_setPrototypeOf(subClass, superClass); }

function JavascriptLibrary_setPrototypeOf(o, p) { JavascriptLibrary_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return JavascriptLibrary_setPrototypeOf(o, p); }







var JavascriptLibrary_inputStyle = {
  width: "100%"
};

var JavascriptLibrary_JavascriptLibrary =
/*#__PURE__*/
function (_Component) {
  JavascriptLibrary_inherits(JavascriptLibrary, _Component);

  function JavascriptLibrary() {
    JavascriptLibrary_classCallCheck(this, JavascriptLibrary);

    return JavascriptLibrary_possibleConstructorReturn(this, JavascriptLibrary_getPrototypeOf(JavascriptLibrary).apply(this, arguments));
  }

  JavascriptLibrary_createClass(JavascriptLibrary, [{
    key: "getNameVersionRows",
    value: function getNameVersionRows(list) {
      return list.map(function (item, i) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
            key: i,
            className: "js-library-info-row"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
            columnSize: 70,
            className: "js-library-info-name"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("p", null, item.name)),
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
            columnSize: 30,
            className: "js-library-info-version"
          },
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("p", null, item.version)))
        );
      });
    }
  }, {
    key: "getNameVersionTable",
    value: function getNameVersionTable(list, label, labelTooltip) {
      return [
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Label"], {
        label: label,
        tooltipMessage: labelTooltip,
        key: "first"
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement("hr", {
        className: "jslibrary-table-separator",
        key: "second"
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
        className: "js-library-info-table",
        key: "third"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
        className: "js-library-info-name-header",
        columnSize: 70
      }, localization.get("EditJavascriptLibrary_TableName.Header")),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
        className: "js-library-info-version-header",
        columnSize: 30
      }, localization.get("EditJavascriptLibrary_TableVersion.Header")), this.getNameVersionRows(list))];
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: JavascriptLibrary_style_default.a.editJSLibrary + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditJavascriptLibrary_LibraryName.Label"),
          value: extensionBeingEdited.name.value,
          tooltipMessage: localization.get("EditJavascriptLibrary_LibraryName.HelpText"),
          style: JavascriptLibrary_inputStyle,
          enabled: false
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditJavascriptLibrary_FileName.Label"),
          value: extensionBeingEdited.fileName.value,
          tooltipMessage: localization.get("EditJavascriptLibrary_FileName.HelpText"),
          style: JavascriptLibrary_inputStyle,
          enabled: false
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditJavascriptLibrary_PreferredLocation.Label"),
          value: extensionBeingEdited.location.value,
          tooltipMessage: localization.get("EditJavascriptLibrary_PreferredLocation.HelpText"),
          style: JavascriptLibrary_inputStyle,
          enabled: false
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditJavascriptLibrary_LibraryVersion.Label"),
          value: extensionBeingEdited.version.value,
          tooltipMessage: localization.get("EditJavascriptLibrary_LibraryVersion.HelpText"),
          style: JavascriptLibrary_inputStyle,
          enabled: false
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditJavascriptLibrary_ObjectName.Label"),
          value: extensionBeingEdited.objectName.value,
          tooltipMessage: localization.get("EditJavascriptLibrary_ObjectName.Label"),
          style: JavascriptLibrary_inputStyle,
          enabled: false
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditJavascriptLibrary_DefaultCDN.Label"),
          value: extensionBeingEdited.defaultCdn.value,
          tooltipMessage: localization.get("EditJavascriptLibrary_DefaultCDN.HelpText"),
          style: JavascriptLibrary_inputStyle,
          enabled: false
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditJavascriptLibrary_CustomCDN.Label"),
          value: extensionBeingEdited.customCdn.value,
          onChange: props.onChange.bind(this, "customCdn"),
          tooltipMessage: localization.get("EditJavascriptLibrary_CustomCDN.HelpText"),
          enabled: !props.disabled,
          style: JavascriptLibrary_inputStyle
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("hr", null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null, this.getNameVersionTable(extensionBeingEdited.dependencies.value, localization.get("EditJavascriptLibrary_DependsOn.Label"), localization.get("EditJavascriptLibrary_DependsOn.HelpText"))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null, this.getNameVersionTable(extensionBeingEdited.usedBy.value, localization.get("EditJavascriptLibrary_UsedBy.Label"), localization.get("EditJavascriptLibrary_UsedBy.HelpText")))), !props.actionButtonsDisabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary"
        }, props.primaryButtonText)))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return JavascriptLibrary;
}(external_window_dnn_nodeModules_React_["Component"]);

JavascriptLibrary_JavascriptLibrary.PropTypes = {
  onCancel: prop_types_default.a.func,
  onUpdateExtension: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  disabled: prop_types_default.a.func,
  primaryButtonText: prop_types_default.a.string
};

function JavascriptLibrary_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex
  };
}

/* harmony default export */ var CustomSettings_JavascriptLibrary = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(JavascriptLibrary_mapStateToProps)(JavascriptLibrary_JavascriptLibrary));
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/ExtensionLanguagePack/style.less
var ExtensionLanguagePack_style = __webpack_require__(34);
var ExtensionLanguagePack_style_default = /*#__PURE__*/__webpack_require__.n(ExtensionLanguagePack_style);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/ExtensionLanguagePack/index.jsx
function ExtensionLanguagePack_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { ExtensionLanguagePack_typeof = function _typeof(obj) { return typeof obj; }; } else { ExtensionLanguagePack_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return ExtensionLanguagePack_typeof(obj); }

function ExtensionLanguagePack_extends() { ExtensionLanguagePack_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return ExtensionLanguagePack_extends.apply(this, arguments); }

function ExtensionLanguagePack_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function ExtensionLanguagePack_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function ExtensionLanguagePack_createClass(Constructor, protoProps, staticProps) { if (protoProps) ExtensionLanguagePack_defineProperties(Constructor.prototype, protoProps); if (staticProps) ExtensionLanguagePack_defineProperties(Constructor, staticProps); return Constructor; }

function ExtensionLanguagePack_possibleConstructorReturn(self, call) { if (call && (ExtensionLanguagePack_typeof(call) === "object" || typeof call === "function")) { return call; } return ExtensionLanguagePack_assertThisInitialized(self); }

function ExtensionLanguagePack_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function ExtensionLanguagePack_getPrototypeOf(o) { ExtensionLanguagePack_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return ExtensionLanguagePack_getPrototypeOf(o); }

function ExtensionLanguagePack_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) ExtensionLanguagePack_setPrototypeOf(subClass, superClass); }

function ExtensionLanguagePack_setPrototypeOf(o, p) { ExtensionLanguagePack_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return ExtensionLanguagePack_setPrototypeOf(o, p); }







var ExtensionLanguagePack_inputStyle = {
  width: "100%"
};

var ExtensionLanguagePack_ExtensionLanguagePack =
/*#__PURE__*/
function (_Component) {
  ExtensionLanguagePack_inherits(ExtensionLanguagePack, _Component);

  function ExtensionLanguagePack() {
    ExtensionLanguagePack_classCallCheck(this, ExtensionLanguagePack);

    return ExtensionLanguagePack_possibleConstructorReturn(this, ExtensionLanguagePack_getPrototypeOf(ExtensionLanguagePack).apply(this, arguments));
  }

  ExtensionLanguagePack_createClass(ExtensionLanguagePack, [{
    key: "onSelect",
    value: function onSelect(key, option) {
      this.props.onChange(key, option.value);
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: ExtensionLanguagePack_style_default.a.editExtensionLanguagePack + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("EditExtensionLanguagePack_Language.Label"),
          options: extensionBeingEdited.locales.value.map(function (locale) {
            return {
              label: locale.name,
              value: locale.id
            };
          }),
          value: extensionBeingEdited.languageId.value,
          onSelect: this.onSelect.bind(this, "languageId"),
          enabled: !props.disabled,
          tooltipMessage: localization.get("EditExtensionLanguagePack_Language.HelpText"),
          style: ExtensionLanguagePack_extends({
            marginBottom: 32
          }, ExtensionLanguagePack_inputStyle)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("EditExtensionLanguagePack_Package.Label"),
          options: extensionBeingEdited.packages.value.map(function (_package) {
            return {
              label: _package.name,
              value: _package.id
            };
          }),
          value: extensionBeingEdited.dependentPackageId.value,
          enabled: !props.disabled,
          onSelect: this.onSelect.bind(this, "dependentPackageId"),
          tooltipMessage: localization.get("EditExtensionLanguagePack_Package.HelpText"),
          style: ExtensionLanguagePack_inputStyle
        })), !props.actionButtonsDisabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary"
        }, props.primaryButtonText)))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return ExtensionLanguagePack;
}(external_window_dnn_nodeModules_React_["Component"]);

ExtensionLanguagePack_ExtensionLanguagePack.propTypes = {
  onCancel: prop_types_default.a.func,
  onUpdateExtension: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  disabled: prop_types_default.a.func,
  primaryButtonText: prop_types_default.a.string
};

function ExtensionLanguagePack_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex
  };
}

/* harmony default export */ var CustomSettings_ExtensionLanguagePack = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(ExtensionLanguagePack_mapStateToProps)(ExtensionLanguagePack_ExtensionLanguagePack));
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/CoreLanguagePack/style.less
var CoreLanguagePack_style = __webpack_require__(35);
var CoreLanguagePack_style_default = /*#__PURE__*/__webpack_require__.n(CoreLanguagePack_style);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/CoreLanguagePack/index.jsx
function CoreLanguagePack_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { CoreLanguagePack_typeof = function _typeof(obj) { return typeof obj; }; } else { CoreLanguagePack_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return CoreLanguagePack_typeof(obj); }

function CoreLanguagePack_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function CoreLanguagePack_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function CoreLanguagePack_createClass(Constructor, protoProps, staticProps) { if (protoProps) CoreLanguagePack_defineProperties(Constructor.prototype, protoProps); if (staticProps) CoreLanguagePack_defineProperties(Constructor, staticProps); return Constructor; }

function CoreLanguagePack_possibleConstructorReturn(self, call) { if (call && (CoreLanguagePack_typeof(call) === "object" || typeof call === "function")) { return call; } return CoreLanguagePack_assertThisInitialized(self); }

function CoreLanguagePack_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function CoreLanguagePack_getPrototypeOf(o) { CoreLanguagePack_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return CoreLanguagePack_getPrototypeOf(o); }

function CoreLanguagePack_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) CoreLanguagePack_setPrototypeOf(subClass, superClass); }

function CoreLanguagePack_setPrototypeOf(o, p) { CoreLanguagePack_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return CoreLanguagePack_setPrototypeOf(o, p); }







var CoreLanguagePack_inputStyle = {
  width: "100%"
};

var CoreLanguagePack_CoreLanguagePack =
/*#__PURE__*/
function (_Component) {
  CoreLanguagePack_inherits(CoreLanguagePack, _Component);

  function CoreLanguagePack() {
    CoreLanguagePack_classCallCheck(this, CoreLanguagePack);

    return CoreLanguagePack_possibleConstructorReturn(this, CoreLanguagePack_getPrototypeOf(CoreLanguagePack).apply(this, arguments));
  }

  CoreLanguagePack_createClass(CoreLanguagePack, [{
    key: "onSelect",
    value: function onSelect(option) {
      this.props.onChange("languageId", option.value);
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: CoreLanguagePack_style_default.a.editCoreLanguagePack + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("EditExtensionLanguagePack_Language.Label"),
          options: extensionBeingEdited.locales.value.map(function (locale) {
            return {
              label: locale.name,
              value: locale.id
            };
          }),
          enabled: !props.disabled,
          value: extensionBeingEdited.languageId.value,
          onSelect: this.onSelect.bind(this),
          tooltipMessage: localization.get("EditExtensionLanguagePack_Language.HelpText"),
          style: CoreLanguagePack_inputStyle
        })), !props.actionButtonsDisabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary"
        }, props.primaryButtonText)))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return CoreLanguagePack;
}(external_window_dnn_nodeModules_React_["Component"]);

CoreLanguagePack_CoreLanguagePack.propTypes = {
  onCancel: prop_types_default.a.func,
  onUpdateExtension: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  disabled: prop_types_default.a.func,
  primaryButtonText: prop_types_default.a.string
};

function CoreLanguagePack_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex
  };
}

/* harmony default export */ var CustomSettings_CoreLanguagePack = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(CoreLanguagePack_mapStateToProps)(CoreLanguagePack_CoreLanguagePack));
// EXTERNAL MODULE: ./src/components/EditExtension/CustomSettings/SkinObject/style.less
var SkinObject_style = __webpack_require__(36);
var SkinObject_style_default = /*#__PURE__*/__webpack_require__.n(SkinObject_style);

// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/SkinObject/index.jsx
function SkinObject_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { SkinObject_typeof = function _typeof(obj) { return typeof obj; }; } else { SkinObject_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return SkinObject_typeof(obj); }

function SkinObject_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function SkinObject_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function SkinObject_createClass(Constructor, protoProps, staticProps) { if (protoProps) SkinObject_defineProperties(Constructor.prototype, protoProps); if (staticProps) SkinObject_defineProperties(Constructor, staticProps); return Constructor; }

function SkinObject_possibleConstructorReturn(self, call) { if (call && (SkinObject_typeof(call) === "object" || typeof call === "function")) { return call; } return SkinObject_assertThisInitialized(self); }

function SkinObject_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function SkinObject_getPrototypeOf(o) { SkinObject_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return SkinObject_getPrototypeOf(o); }

function SkinObject_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) SkinObject_setPrototypeOf(subClass, superClass); }

function SkinObject_setPrototypeOf(o, p) { SkinObject_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return SkinObject_setPrototypeOf(o, p); }







var SkinObject_inputStyle = {
  width: "100%"
};

var SkinObject_SkinObject =
/*#__PURE__*/
function (_Component) {
  SkinObject_inherits(SkinObject, _Component);

  function SkinObject() {
    SkinObject_classCallCheck(this, SkinObject);

    return SkinObject_possibleConstructorReturn(this, SkinObject_getPrototypeOf(SkinObject).apply(this, arguments));
  }

  SkinObject_createClass(SkinObject, [{
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: SkinObject_style_default.a.editSkinObj + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditSkinObject_ControlKey.Label"),
          value: extensionBeingEdited.controlKey.value,
          onChange: props.onChange.bind(this, "controlKey"),
          tooltipMessage: localization.get("EditSkinObject_ControlKey.HelpText"),
          style: SkinObject_inputStyle,
          enabled: !props.disabled
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditSkinObject_ControlSrc.Label"),
          value: extensionBeingEdited.controlSrc.value,
          onChange: props.onChange.bind(this, "controlSrc"),
          tooltipMessage: localization.get("EditSkinObject_ControlSrc.HelpText"),
          style: SkinObject_inputStyle,
          enabled: !props.disabled
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          label: localization.get("EditSkinObject_SupportsPartialRender.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          value: extensionBeingEdited.supportsPartialRendering.value,
          onChange: props.onChange.bind(this, "supportsPartialRendering"),
          tooltipMessage: localization.get("EditSkinObject_SupportsPartialRender.HelpText"),
          readOnly: props.disabled
        }))), !props.actionButtonsDisabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this)
        }, props.primaryButtonText)))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return SkinObject;
}(external_window_dnn_nodeModules_React_["Component"]);

SkinObject_SkinObject.propTypes = {
  onCancel: prop_types_default.a.func,
  onUpdateExtension: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  disabled: prop_types_default.a.func,
  primaryButtonText: prop_types_default.a.string
};

function SkinObject_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex
  };
}

/* harmony default export */ var CustomSettings_SkinObject = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(SkinObject_mapStateToProps)(SkinObject_SkinObject));
// CONCATENATED MODULE: ./src/components/EditExtension/CustomSettings/index.jsx
function CustomSettings_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { CustomSettings_typeof = function _typeof(obj) { return typeof obj; }; } else { CustomSettings_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return CustomSettings_typeof(obj); }

function CustomSettings_extends() { CustomSettings_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return CustomSettings_extends.apply(this, arguments); }

function CustomSettings_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function CustomSettings_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function CustomSettings_createClass(Constructor, protoProps, staticProps) { if (protoProps) CustomSettings_defineProperties(Constructor.prototype, protoProps); if (staticProps) CustomSettings_defineProperties(Constructor, staticProps); return Constructor; }

function CustomSettings_possibleConstructorReturn(self, call) { if (call && (CustomSettings_typeof(call) === "object" || typeof call === "function")) { return call; } return CustomSettings_assertThisInitialized(self); }

function CustomSettings_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function CustomSettings_getPrototypeOf(o) { CustomSettings_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return CustomSettings_getPrototypeOf(o); }

function CustomSettings_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) CustomSettings_setPrototypeOf(subClass, superClass); }

function CustomSettings_setPrototypeOf(o, p) { CustomSettings_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return CustomSettings_setPrototypeOf(o, p); }










var CustomSettings_CustomSettings =
/*#__PURE__*/
function (_Component) {
  CustomSettings_inherits(CustomSettings, _Component);

  function CustomSettings() {
    CustomSettings_classCallCheck(this, CustomSettings);

    return CustomSettings_possibleConstructorReturn(this, CustomSettings_getPrototypeOf(CustomSettings).apply(this, arguments));
  }

  CustomSettings_createClass(CustomSettings, [{
    key: "getExtensionSetting",
    value: function getExtensionSetting(props) {
      switch (props.type) {
        case "Auth_System":
          return !props.isAddMode &&
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_AuthenticationSystem, CustomSettings_extends({}, props, {
            className: props.isAddMode ? "add-mode" : ""
          }));

        case "SkinObject":
          return !props.isAddMode &&
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_SkinObject, CustomSettings_extends({}, props, {
            className: props.isAddMode ? "add-mode" : ""
          }));

        case "Skin":
        case "Container":
          return !props.isAddMode &&
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_Container, CustomSettings_extends({}, props, {
            className: props.isAddMode ? "add-mode" : ""
          }));

        case "ExtensionLanguagePack":
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_ExtensionLanguagePack, CustomSettings_extends({}, props, {
              className: props.isAddMode ? "add-mode" : ""
            }))
          );

        case "CoreLanguagePack":
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_CoreLanguagePack, CustomSettings_extends({}, props, {
              className: props.isAddMode ? "add-mode" : ""
            }))
          );

        case "JavaScript_Library":
          return !props.isAddMode &&
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_JavascriptLibrary, CustomSettings_extends({}, props, {
            className: props.isAddMode ? "add-mode" : ""
          }));

        case "Module":
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_Module, CustomSettings_extends({}, props, {
              className: props.isAddMode ? "add-mode" : ""
            }))
          );

        default:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("div", null)
          );
      }
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return this.getExtensionSetting(props);
    }
  }]);

  return CustomSettings;
}(external_window_dnn_nodeModules_React_["Component"]);

/* harmony default export */ var EditExtension_CustomSettings = (CustomSettings_CustomSettings);
// EXTERNAL MODULE: ./src/components/common/BasicPackageInformation/style.less
var BasicPackageInformation_style = __webpack_require__(37);
var BasicPackageInformation_style_default = /*#__PURE__*/__webpack_require__.n(BasicPackageInformation_style);

// CONCATENATED MODULE: ./src/components/common/BasicPackageInformation/index.jsx
var BasicPackageInformation_this = undefined;







var BasicPackageInformation_inputStyle = {
  width: "100%"
};

var BasicPackageInformation_BasicPackageInformation = function BasicPackageInformation(_ref) {
  var disabled = _ref.disabled,
      validationMapped = _ref.validationMapped,
      installedPackageTypes = _ref.installedPackageTypes,
      extensionData = _ref.extensionData,
      onVersionChange = _ref.onVersionChange,
      onChange = _ref.onChange,
      triedToSave = _ref.triedToSave,
      version = _ref.version,
      onPackageTypeSelect = _ref.onPackageTypeSelect,
      isAddMode = _ref.isAddMode;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
      className: BasicPackageInformation_style_default.a.basicPackageInformation + " with-right-border top-half"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("div", null,
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
      className: "extension-type",
      options: installedPackageTypes && installedPackageTypes.map(function (_package) {
        return {
          label: _package.Type.split("_").join("").split(/(?=[A-Z])/).join(" "),
          value: _package.Type
        };
      }),
      enabled: !disabled,
      tooltipMessage: localization.get("EditExtension_PackageType.HelpText"),
      label: localization.get("EditExtension_PackageType.Label"),
      onSelect: onPackageTypeSelect,
      value: !validationMapped ? extensionData.packageType : extensionData.packageType.value,
      style: BasicPackageInformation_inputStyle
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
      label: localization.get("EditExtension_PackageName.Label"),
      tooltipMessage: localization.get("EditExtension_PackageName.HelpText"),
      style: BasicPackageInformation_inputStyle,
      enabled: isAddMode,
      onChange: onChange && onChange.bind(BasicPackageInformation_this, "name"),
      className: "extension-package-name",
      value: !validationMapped ? extensionData.name : extensionData.name.value
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
      label: localization.get("EditExtension_PackageFriendlyName.Label") + "*",
      tooltipMessage: localization.get("EditExtension_PackageFriendlyName.HelpText"),
      value: !validationMapped ? extensionData.friendlyName : extensionData.friendlyName.value,
      style: BasicPackageInformation_inputStyle,
      className: "extension-package-friendly-name",
      error: extensionData.friendlyName.error && triedToSave,
      enabled: !disabled,
      onChange: onChange && onChange.bind(BasicPackageInformation_this, "friendlyName")
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("div", null,
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInputWithError"], {
      label: localization.get("EditExtension_PackageDescription.Label"),
      tooltipMessage: localization.get("EditExtension_PackageDescription.HelpText"),
      style: BasicPackageInformation_inputStyle,
      className: "extension-description",
      inputStyle: {
        marginBottom: 28,
        height: 123
      },
      value: !validationMapped ? extensionData.description : extensionData.description.value,
      enabled: !disabled,
      onChange: onChange && onChange.bind(BasicPackageInformation_this, "description")
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
      options: getVersionDropdownValues(),
      label: localization.get("EditExtension_PackageVersion.Label"),
      tooltipMessage: localization.get("EditExtension_PackageVersion.HelpText"),
      enabled: !disabled,
      defaultDropdownValue: formatVersionNumber(version[0]),
      onSelect: onVersionChange && onVersionChange.bind(BasicPackageInformation_this, 0),
      className: "version-dropdown",
      style: {
        position: "relative",
        top: 3
      }
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
      options: getVersionDropdownValues(),
      className: "version-dropdown",
      label: formatVersionNumber(version[1]),
      onSelect: onVersionChange && onVersionChange.bind(BasicPackageInformation_this, 1),
      enabled: !disabled
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
      options: getVersionDropdownValues(),
      label: formatVersionNumber(version[2]),
      className: "version-dropdown",
      onSelect: onVersionChange && onVersionChange.bind(BasicPackageInformation_this, 2),
      enabled: !disabled
    })))
  );
};

BasicPackageInformation_BasicPackageInformation.propTypes = {
  disabled: prop_types_default.a.bool,
  validationMapped: prop_types_default.a.bool,
  installedPackageTypes: prop_types_default.a.array,
  extensionData: prop_types_default.a.object,
  onVersionChange: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  triedToSave: prop_types_default.a.bool,
  version: prop_types_default.a.array,
  onPackageTypeSelect: prop_types_default.a.func,
  isAddMode: prop_types_default.a.bool
};
/* harmony default export */ var common_BasicPackageInformation = (BasicPackageInformation_BasicPackageInformation);
// EXTERNAL MODULE: ./src/components/NewExtensionModal/style.less
var NewExtensionModal_style = __webpack_require__(38);
var NewExtensionModal_style_default = /*#__PURE__*/__webpack_require__.n(NewExtensionModal_style);

// CONCATENATED MODULE: ./src/components/NewExtensionModal/index.jsx
function NewExtensionModal_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { NewExtensionModal_typeof = function _typeof(obj) { return typeof obj; }; } else { NewExtensionModal_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return NewExtensionModal_typeof(obj); }

function NewExtensionModal_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function NewExtensionModal_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function NewExtensionModal_createClass(Constructor, protoProps, staticProps) { if (protoProps) NewExtensionModal_defineProperties(Constructor.prototype, protoProps); if (staticProps) NewExtensionModal_defineProperties(Constructor, staticProps); return Constructor; }

function NewExtensionModal_possibleConstructorReturn(self, call) { if (call && (NewExtensionModal_typeof(call) === "object" || typeof call === "function")) { return call; } return NewExtensionModal_assertThisInitialized(self); }

function NewExtensionModal_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function NewExtensionModal_getPrototypeOf(o) { NewExtensionModal_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return NewExtensionModal_getPrototypeOf(o); }

function NewExtensionModal_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) NewExtensionModal_setPrototypeOf(subClass, superClass); }

function NewExtensionModal_setPrototypeOf(o, p) { NewExtensionModal_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return NewExtensionModal_setPrototypeOf(o, p); }










var NewExtensionModal_inputStyle = {
  width: "100%"
};

function NewExtensionModal_formatVersionNumber(n) {
  return n > 9 ? "" + n : "0" + n;
}

function getDropdownOptions() {
  var options = [];

  for (var i = 0; i < 100; i++) {
    options.push({
      label: NewExtensionModal_formatVersionNumber(i),
      value: i
    });
  }

  return options;
}

function deepCopy(object) {
  return JSON.parse(JSON.stringify(object));
}

var NewExtensionModal_NewExtensionModal =
/*#__PURE__*/
function (_Component) {
  NewExtensionModal_inherits(NewExtensionModal, _Component);

  function NewExtensionModal() {
    var _this;

    NewExtensionModal_classCallCheck(this, NewExtensionModal);

    _this = NewExtensionModal_possibleConstructorReturn(this, NewExtensionModal_getPrototypeOf(NewExtensionModal).call(this));
    _this.versionDropdownOptions = getDropdownOptions();
    return _this;
  }

  NewExtensionModal_createClass(NewExtensionModal, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;

      if (!props.moduleCategories || props.moduleCategories.length === 0) {
        props.dispatch(extension.getModuleCategories());
      }
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var props = this.props;
      var value = NewExtensionModal_typeof(event) === "object" ? event.target.value : event;

      var _extensionBeingEdited = JSON.parse(JSON.stringify(props.extensionBeingEdited));

      var field = _extensionBeingEdited[key];
      field.value = value;

      if (field.validateRequired && field.error) {
        field.error = false;
        this.toggleTriedToSave();
      }

      this.updateExtensionBeingEdited(_extensionBeingEdited);
    }
  }, {
    key: "onVersionChange",
    value: function onVersionChange(index, option) {
      var props = this.props;

      var _extensionBeingEdited = JSON.parse(JSON.stringify(props.extensionBeingEdited));

      if (_extensionBeingEdited.version) {
        var versionArray = _extensionBeingEdited.version.value.split(".");

        versionArray[index] = option.value;
        _extensionBeingEdited.version.value = versionArray.join(".");
      }

      this.updateExtensionBeingEdited(_extensionBeingEdited);
    }
  }, {
    key: "onPackageTypeSelect",
    value: function onPackageTypeSelect(option) {
      var value = option.value;
      var props = this.props;

      var _extensionBeingEdited = deepCopy(props.extensionBeingEdited);

      _extensionBeingEdited.packageType.value = value;
      this.updateExtensionBeingEdited(_extensionBeingEdited);
    }
  }, {
    key: "updateExtensionBeingEdited",
    value: function updateExtensionBeingEdited(extensionBeingEdited, callback) {
      var props = this.props;
      props.dispatch(extension.updateExtensionBeingEdited(extensionBeingEdited, callback));
    }
  }, {
    key: "parseEditorActions",
    value: function parseEditorActions(extension) {
      switch (extension.packageType.value.toLowerCase()) {
        case "module":
          return {
            category: extension.category.value,
            dependencies: extension.dependencies.value,
            hostPermissions: extension.hostPermissions.value,
            shareable: extension.shareable.value,
            premiumModule: extension.premiumModule.value,
            folderName: extension.folderName.value,
            businessController: extension.businessController.value
          };

        case "corelanguagepack":
          return {
            languageId: extension.languageId.value
          };

        case "extensionlanguagepack":
          return {
            languageId: extension.languageId.value,
            dependentPackageId: extension.dependentPackageId.value
          };

        default:
          return {};
      }
    }
  }, {
    key: "onSaveExtension",
    value: function onSaveExtension() {
      var _this2 = this;

      var props = this.props;
      var editorActions = this.parseEditorActions(props.extensionBeingEdited);
      props.dispatch(extension.createNewExtension(props.extensionBeingEdited, editorActions, props.extensionBeingEditedIndex, function () {
        if (props.tabIndex !== 0) {
          props.dispatch(pagination.loadTab(0));
        }

        if (props.selectedInstalledPackageType !== props.extensionBeingEdited.packageType.value) {
          props.dispatch(extension.getInstalledPackages(props.extensionBeingEdited.packageType.value));
        }

        _this2.selectPanel(0);
      }));
    }
  }, {
    key: "toggleTriedToSave",
    value: function toggleTriedToSave() {
      var props = this.props;
      props.dispatch(extension.toggleTriedToSave());
    }
  }, {
    key: "validateFields",
    value: function validateFields() {
      var props = this.props;
      var errorCount = 0;

      var _extensionBeingEdited = JSON.parse(JSON.stringify(props.extensionBeingEdited));

      Object.keys(_extensionBeingEdited).forEach(function (key) {
        var field = _extensionBeingEdited[key];

        if (field.validateRequired && field.value === "") {
          field.error = true;
          errorCount++;
        }
      });

      if (props.triedToSave === false) {
        this.toggleTriedToSave();
      }

      this.updateExtensionBeingEdited(_extensionBeingEdited);
      return errorCount === 0;
    }
  }, {
    key: "selectPanel",
    value: function selectPanel(panel, event) {
      if (event) {
        event.preventDefault();
      }

      var props = this.props;
      props.dispatch(visiblePanel.selectPanel(panel));
    }
  }, {
    key: "onSave",
    value: function onSave() {
      if (!this.validateFields()) {
        return;
      }

      this.onSaveExtension();
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      var version = extensionBeingEdited.version.value ? extensionBeingEdited.version.value.split(".") : [0, 0, 0];
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: NewExtensionModal_style_default.a.newExtensionModal
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: localization.get("CreateExtension.Action")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], {
          backToLinkProps: {
            text: localization.get("BackToExtensions"),
            onClick: props.onCancel.bind(this)
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "new-extension-box extension-form"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_BasicPackageInformation, {
          validationMapped: true,
          installedPackageTypes: props.installedPackageTypes,
          extensionData: extensionBeingEdited,
          onChange: this.onChange.bind(this),
          triedToSave: props.triedToSave,
          version: version,
          onPackageTypeSelect: this.onPackageTypeSelect.bind(this),
          onVersionChange: this.onVersionChange.bind(this),
          isAddMode: true
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("hr", null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "box-title-container"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h3", {
          className: "box-title"
        }, localization.get("EditExtension_OwnerDetails.Label"))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border bottom-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageOwner.Label"),
          tooltipMessage: localization.get("EditExtension_PackageOwner.HelpText"),
          style: NewExtensionModal_inputStyle,
          value: extensionBeingEdited.owner.value,
          onChange: this.onChange.bind(this, "owner")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageOrganization.Label"),
          tooltipMessage: localization.get("EditExtension_PackageOrganization.HelpText"),
          style: NewExtensionModal_inputStyle,
          inputStyle: {
            marginBottom: 0
          },
          value: extensionBeingEdited.organization.value,
          onChange: this.onChange.bind(this, "organization")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageURL.Label"),
          tooltipMessage: localization.get("EditExtension_PackageURL.HelpText"),
          style: NewExtensionModal_inputStyle,
          inputStyle: {
            marginBottom: 32
          },
          value: extensionBeingEdited.url.value,
          onChange: this.onChange.bind(this, "url")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageEmailAddress.Label"),
          tooltipMessage: localization.get("EditExtension_PackageEmailAddress.HelpText"),
          style: NewExtensionModal_inputStyle,
          inputStyle: {
            marginBottom: 32
          },
          value: extensionBeingEdited.email.value,
          onChange: this.onChange.bind(this, "email")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(EditExtension_CustomSettings, {
          type: extensionBeingEdited.packageType.value,
          onChange: this.onChange.bind(this),
          actionButtonsDisabled: true,
          isAddMode: true
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("CreateExtension_Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.onSave.bind(this)
        }, localization.get("CreateExtension_Save.Button"))))))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return NewExtensionModal;
}(external_window_dnn_nodeModules_React_["Component"]);

NewExtensionModal_NewExtensionModal.propTypes = {
  onCancel: prop_types_default.a.func
};

function NewExtensionModal_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    triedToSave: state.extension.triedToSave,
    installedPackageTypes: state.extension.installedPackageTypes,
    selectedInstalledPackageType: state.extension.selectedInstalledPackageType,
    moduleCategories: state.extension.moduleCategories,
    locales: state.extension.locales,
    localePackages: state.extension.localePackages,
    tabIndex: state.pagination.tabIndex
  };
}

/* harmony default export */ var components_NewExtensionModal = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(NewExtensionModal_mapStateToProps)(NewExtensionModal_NewExtensionModal));
// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactCollapse"
var external_window_dnn_nodeModules_ReactCollapse_ = __webpack_require__(39);
var external_window_dnn_nodeModules_ReactCollapse_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReactCollapse_);

// EXTERNAL MODULE: ./src/components/NewModuleModal/common/FolderDropdown/style.less
var FolderDropdown_style = __webpack_require__(40);
var FolderDropdown_style_default = /*#__PURE__*/__webpack_require__.n(FolderDropdown_style);

// CONCATENATED MODULE: ./src/components/NewModuleModal/common/FolderDropdown/index.jsx
function FolderDropdown_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { FolderDropdown_typeof = function _typeof(obj) { return typeof obj; }; } else { FolderDropdown_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return FolderDropdown_typeof(obj); }

function FolderDropdown_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function FolderDropdown_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function FolderDropdown_createClass(Constructor, protoProps, staticProps) { if (protoProps) FolderDropdown_defineProperties(Constructor.prototype, protoProps); if (staticProps) FolderDropdown_defineProperties(Constructor, staticProps); return Constructor; }

function FolderDropdown_possibleConstructorReturn(self, call) { if (call && (FolderDropdown_typeof(call) === "object" || typeof call === "function")) { return call; } return FolderDropdown_assertThisInitialized(self); }

function FolderDropdown_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function FolderDropdown_getPrototypeOf(o) { FolderDropdown_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return FolderDropdown_getPrototypeOf(o); }

function FolderDropdown_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) FolderDropdown_setPrototypeOf(subClass, superClass); }

function FolderDropdown_setPrototypeOf(o, p) { FolderDropdown_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return FolderDropdown_setPrototypeOf(o, p); }








var FolderDropdown_FolderDropdown =
/*#__PURE__*/
function (_Component) {
  FolderDropdown_inherits(FolderDropdown, _Component);

  function FolderDropdown() {
    var _this;

    FolderDropdown_classCallCheck(this, FolderDropdown);

    _this = FolderDropdown_possibleConstructorReturn(this, FolderDropdown_getPrototypeOf(FolderDropdown).call(this));
    _this.state = {
      addNewFolderOpen: false,
      newFolderValue: "",
      triedToSave: false
    };
    return _this;
  }

  FolderDropdown_createClass(FolderDropdown, [{
    key: "onOwnerFolderSelect",
    value: function onOwnerFolderSelect(option) {
      var props = this.props;

      if (option && option.value === "AddNewFolder") {
        this.setState({
          addNewFolderOpen: true
        });
      } else {
        props.onFolderSelect(option);
      }
    }
  }, {
    key: "closeAddNewFolderBox",
    value: function closeAddNewFolderBox() {
      this.setState({
        addNewFolderOpen: false,
        newFolderValue: ""
      });
    }
  }, {
    key: "onAddNewFolder",
    value: function onAddNewFolder() {
      var props = this.props,
          state = this.state;

      if (this.state.newFolderValue === "") {
        this.setState({
          triedToSave: true
        });
        return;
      }

      props.onAddNewFolder(state.newFolderValue, props.type, this.closeAddNewFolderBox.bind(this));
    }
  }, {
    key: "onFolderNameChange",
    value: function onFolderNameChange(event) {
      this.setState({
        newFolderValue: event.target.value,
        triedToSave: false
      });
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var folders = [{
        label: "[" + localization.get("CreateNewModule_NewFolder.Label") + "]",
        value: "AddNewFolder"
      }, {
        label: localization.get("CreateNewModule_NotSpecified.Label"),
        value: ""
      }].concat(props.folders);
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: FolderDropdown_style_default.a.folderDropdown,
          style: {
            padding: 0
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCollapse_default.a, {
          isOpened: state.addNewFolderOpen,
          className: "add-new-folder-box",
          fixedHeight: 250,
          style: {
            float: "left"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          style: {
            padding: "20px 10px"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h3", {
          className: "new-folder-title"
        }, localization.get("CreateNewModule_NewFolder.Label")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("CreateNewModule_FolderName.Label"),
          inputStyle: {
            marginBottom: 16
          },
          value: state.newFolderValue,
          onChange: this.onFolderNameChange.bind(this),
          error: state.newFolderValue === "" && state.triedToSave
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "new-folder-buttons"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: this.closeAddNewFolderBox.bind(this)
        }, localization.get("NewModule_Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.onAddNewFolder.bind(this)
        }, localization.get("NewModule_Create.Button"))))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          options: folders,
          tooltipMessage: props.tooltipMessage,
          label: props.label,
          className: "folder-dropdown" + (state.addNewFolderOpen ? " hidden" : ""),
          onSelect: this.onOwnerFolderSelect.bind(this),
          value: props.value,
          enabled: props.enabled,
          error: props.error
        }))
      );
    }
  }]);

  return FolderDropdown;
}(external_window_dnn_nodeModules_React_["Component"]);

FolderDropdown_FolderDropdown.propTypes = {
  onCancel: prop_types_default.a.func,
  folders: prop_types_default.a.array,
  onFolderSelect: prop_types_default.a.func,
  type: prop_types_default.a.string,
  tooltipMessage: prop_types_default.a.string,
  label: prop_types_default.a.string,
  enabled: prop_types_default.a.bool,
  error: prop_types_default.a.bool,
  onAddedNewFolder: prop_types_default.a.func
};
/* harmony default export */ var common_FolderDropdown = (FolderDropdown_FolderDropdown);
// CONCATENATED MODULE: ./src/components/NewModuleModal/common/helperFunctions.js
function helperFunctions_extends() { helperFunctions_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return helperFunctions_extends.apply(this, arguments); }

function validationMapNewModule(newModule, getValidateRequired) {
  var _newModule = helperFunctions_extends({}, newModule);

  Object.keys(_newModule).forEach(function (key) {
    var required = getValidateRequired(key);
    _newModule[key] = {
      value: _newModule[key],
      required: required,
      error: required ? !_newModule[key] : false
    };
  });
  return _newModule;
}
function valueMapNewModule(newModule) {
  var _newModule = helperFunctions_extends({}, newModule);

  Object.keys(_newModule).forEach(function (key) {
    _newModule[key] = _newModule[key].value;
  });
  return _newModule;
}
// EXTERNAL MODULE: ./src/components/NewModuleModal/FromControl/style.less
var FromControl_style = __webpack_require__(41);
var FromControl_style_default = /*#__PURE__*/__webpack_require__.n(FromControl_style);

// CONCATENATED MODULE: ./src/components/NewModuleModal/FromControl/index.jsx
function FromControl_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { FromControl_typeof = function _typeof(obj) { return typeof obj; }; } else { FromControl_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return FromControl_typeof(obj); }

function FromControl_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function FromControl_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function FromControl_createClass(Constructor, protoProps, staticProps) { if (protoProps) FromControl_defineProperties(Constructor.prototype, protoProps); if (staticProps) FromControl_defineProperties(Constructor, staticProps); return Constructor; }

function FromControl_possibleConstructorReturn(self, call) { if (call && (FromControl_typeof(call) === "object" || typeof call === "function")) { return call; } return FromControl_assertThisInitialized(self); }

function FromControl_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function FromControl_getPrototypeOf(o) { FromControl_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return FromControl_getPrototypeOf(o); }

function FromControl_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) FromControl_setPrototypeOf(subClass, superClass); }

function FromControl_setPrototypeOf(o, p) { FromControl_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return FromControl_setPrototypeOf(o, p); }








var FromControl_inputStyle = {
  width: "100%"
};

function FromControl_getValidateRequired(key) {
  switch (key) {
    case "moduleFolder":
    case "fileName":
    case "moduleName":
      return true;

    default:
      return false;
  }
}

var emptyNewModule = {
  type: 1,
  moduleFolder: "",
  ownerFolder: "",
  addPage: true,
  fileName: "",
  moduleName: "",
  description: ""
};

var FromControl_FromControl =
/*#__PURE__*/
function (_Component) {
  FromControl_inherits(FromControl, _Component);

  function FromControl() {
    var _this;

    FromControl_classCallCheck(this, FromControl);

    _this = FromControl_possibleConstructorReturn(this, FromControl_getPrototypeOf(FromControl).call(this));
    _this.state = {
      newModule: validationMapNewModule(emptyNewModule, FromControl_getValidateRequired),
      triedToSave: false
    };
    return _this;
  }

  FromControl_createClass(FromControl, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      this.props.retrieveOwnerAndModuleFolders();
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      if (props.moduleFiles.length > 0) {
        var newModule = this.state.newModule;
        newModule.fileName.value = props.moduleFiles[0].value;
        newModule.fileName.error = false;
        this.setState({
          newModule: newModule
        });
      }
    }
  }, {
    key: "onFolderSelect",
    value: function onFolderSelect(key, option) {
      var props = this.props,
          state = this.state;
      var _this$state = this.state,
          newModule = _this$state.newModule,
          triedToSave = _this$state.triedToSave;

      if (key === "ownerFolder") {
        newModule.fileName.value = "";
        newModule.fileName.error = true;
        newModule.moduleFolder.value = "";
        newModule.moduleFolder.error = true;
        triedToSave = false;
        props.onSelectOwnerFolder(option.value);
      }

      if (key === "moduleFolder" && option.value !== "") {
        newModule.fileName.value = "";
        newModule.fileName.error = true;
        triedToSave = false;
        props.onSelectModuleFolder({
          ownerFolder: state.newModule.ownerFolder.value,
          moduleFolder: option.value,
          type: 0
        });
      }

      if (newModule[key].required) {
        newModule[key].error = !option.value;
      }

      newModule[key].value = option.value;
      this.setState({
        newModule: newModule,
        triedToSave: triedToSave
      });
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var value = FromControl_typeof(event) === "object" ? event.target.value : event;
      var newModule = this.state.newModule;
      newModule[key].value = value;

      if (newModule[key].required) {
        newModule[key].error = !value;
      }

      this.setState({
        newModule: newModule
      });
    }
  }, {
    key: "validateFields",
    value: function validateFields() {
      var _this$state2 = this.state,
          triedToSave = _this$state2.triedToSave,
          newModule = _this$state2.newModule;
      var errorCount = 0;
      Object.keys(newModule).forEach(function (key) {
        if (newModule[key].error) {
          errorCount++;
        }
      });
      triedToSave = true;
      this.setState({
        newModule: newModule,
        triedToSave: triedToSave
      });
      return errorCount === 0;
    }
  }, {
    key: "onCreateNewModule",
    value: function onCreateNewModule() {
      var state = this.state,
          props = this.props;
      var triedToSave = state.triedToSave;
      triedToSave = true;

      if (!this.validateFields()) {
        return;
      }

      this.setState({
        triedToSave: triedToSave
      });
      props.onCreateNewModule(valueMapNewModule(state.newModule));
    }
  }, {
    key: "onAddedNewFolder",
    value: function onAddedNewFolder(data, type) {
      var newModule = this.state.newModule;

      if (type === "ownerFolder") {
        newModule.ownerFolder.value = data.ownerFolder;
        newModule.moduleFolder.value = "";
        newModule.moduleFolder.error = true;
        newModule.fileName.value = "";
        newModule.fileName.error = true;
        this.setState({
          triedToSave: false
        });
        this.props.onSelectOwnerFolder(data.ownerFolder);
      }

      if (type === "moduleFolder") {
        newModule.moduleFolder.value = data.moduleFolder;
        newModule.moduleFolder.error = false;
        newModule.fileName.value = "";
        newModule.fileName.error = true;
        this.setState({
          triedToSave: false
        });
        this.props.onSelectModuleFolder({
          ownerFolder: newModule.ownerFolder.value,
          moduleFolder: data.moduleFolder,
          type: 0
        });
      }

      this.setState({
        newModule: newModule
      });
    }
  }, {
    key: "onAddNewFolder",
    value: function onAddNewFolder(value, type, callback) {
      var newModule = this.state.newModule;
      var payload = {
        moduleFolder: type === "ownerFolder" ? "" : newModule.moduleFolder.value,
        ownerFolder: newModule.ownerFolder.value
      };
      payload[type] = value;
      this.props.onAddNewFolder(payload, type, callback);
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: FromControl_style_default.a.fromControl + " extension-form",
          style: {
            padding: 0
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          style: {
            paddingRight: 15
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_FolderDropdown, {
          folders: props.ownerFolders,
          label: localization.get("NewModule_OwnerFolder.Label"),
          type: "ownerFolder",
          tooltipMessage: localization.get("NewModule_OwnerFolder.HelpText"),
          onFolderSelect: this.onFolderSelect.bind(this, "ownerFolder"),
          value: state.newModule.ownerFolder.value,
          onAddNewFolder: this.onAddNewFolder.bind(this),
          onAddedNewFolder: this.onAddedNewFolder.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_FolderDropdown, {
          folders: props.moduleFolders,
          label: localization.get("NewModule_ModuleFolder.Label"),
          type: "moduleFolder",
          tooltipMessage: localization.get("NewModule_ModuleFolder.HelpText"),
          onFolderSelect: this.onFolderSelect.bind(this, "moduleFolder"),
          value: state.newModule.moduleFolder.value,
          onAddNewFolder: this.onAddNewFolder.bind(this),
          error: state.newModule.moduleFolder.error && state.triedToSave,
          onAddedNewFolder: this.onAddedNewFolder.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("NewModule_Resource.Label"),
          tooltipMessage: localization.get("NewModule_Resource.HelpText"),
          style: FromControl_inputStyle,
          options: props.moduleFiles,
          onSelect: this.onFolderSelect.bind(this, "fileName"),
          value: state.newModule.fileName.value,
          error: state.newModule.fileName.error && state.triedToSave
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          style: {
            paddingLeft: 15
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("NewModule_ModuleName.Label"),
          tooltipMessage: localization.get("NewModule_ModuleName.HelpText"),
          style: FromControl_inputStyle,
          inputStyle: {
            marginBottom: 32
          },
          value: state.newModule.moduleName.value,
          onChange: this.onChange.bind(this, "moduleName"),
          error: state.newModule.moduleName.error && state.triedToSave
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInputWithError"], {
          label: localization.get("NewModule_Description.Label"),
          tooltipMessage: localization.get("NewModule_Description.HelpText"),
          style: FromControl_inputStyle,
          inputStyle: {
            marginBottom: 32,
            height: 126
          },
          onChange: this.onChange.bind(this, "description"),
          value: state.newModule.description.value
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: state.newModule.addPage.value,
          tooltipMessage: localization.get("NewModule_AddTestPage.HelpText"),
          label: localization.get("NewModule_AddTestPage.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          onChange: this.onChange.bind(this, "addPage")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("NewModule_Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.onCreateNewModule.bind(this)
        }, localization.get("NewModule_Create.Button"))))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return FromControl;
}(external_window_dnn_nodeModules_React_["Component"]);

FromControl_FromControl.propTypes = {
  onCancel: prop_types_default.a.func,
  ownerFolders: prop_types_default.a.array,
  moduleFolders: prop_types_default.a.array,
  moduleFiles: prop_types_default.a.array,
  onCreateNewModule: prop_types_default.a.func,
  onSelectOwnerFolder: prop_types_default.a.func,
  onSelectModuleFolder: prop_types_default.a.func,
  onAddNewFolder: prop_types_default.a.func,
  retrieveOwnerAndModuleFolders: prop_types_default.a.func
};
/* harmony default export */ var NewModuleModal_FromControl = (FromControl_FromControl);
// EXTERNAL MODULE: ./src/components/NewModuleModal/FromManifest/style.less
var FromManifest_style = __webpack_require__(42);
var FromManifest_style_default = /*#__PURE__*/__webpack_require__.n(FromManifest_style);

// CONCATENATED MODULE: ./src/components/NewModuleModal/FromManifest/index.jsx
function FromManifest_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { FromManifest_typeof = function _typeof(obj) { return typeof obj; }; } else { FromManifest_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return FromManifest_typeof(obj); }

function FromManifest_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function FromManifest_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function FromManifest_createClass(Constructor, protoProps, staticProps) { if (protoProps) FromManifest_defineProperties(Constructor.prototype, protoProps); if (staticProps) FromManifest_defineProperties(Constructor, staticProps); return Constructor; }

function FromManifest_possibleConstructorReturn(self, call) { if (call && (FromManifest_typeof(call) === "object" || typeof call === "function")) { return call; } return FromManifest_assertThisInitialized(self); }

function FromManifest_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function FromManifest_getPrototypeOf(o) { FromManifest_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return FromManifest_getPrototypeOf(o); }

function FromManifest_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) FromManifest_setPrototypeOf(subClass, superClass); }

function FromManifest_setPrototypeOf(o, p) { FromManifest_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return FromManifest_setPrototypeOf(o, p); }








var dropdownStyle = {
  width: "100%",
  marginBottom: 64
};

function FromManifest_getValidateRequired(key) {
  switch (key) {
    case "moduleFolder":
    case "manifest":
      return true;

    default:
      return false;
  }
}

var FromManifest_emptyNewModule = {
  type: 2,
  moduleFolder: "",
  ownerFolder: "",
  addPage: true,
  manifest: "",
  moduleName: "",
  description: ""
};

var FromManifest_FromManifest =
/*#__PURE__*/
function (_Component) {
  FromManifest_inherits(FromManifest, _Component);

  function FromManifest() {
    var _this;

    FromManifest_classCallCheck(this, FromManifest);

    _this = FromManifest_possibleConstructorReturn(this, FromManifest_getPrototypeOf(FromManifest).call(this));
    _this.state = {
      newModule: validationMapNewModule(FromManifest_emptyNewModule, FromManifest_getValidateRequired),
      triedToSave: false
    };
    return _this;
  }

  FromManifest_createClass(FromManifest, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      this.props.retrieveOwnerAndModuleFolders();
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      if (props.moduleFiles.length > 0) {
        var newModule = this.state.newModule;
        newModule.manifest.value = props.moduleFiles[0].value;
        newModule.manifest.error = false;
        this.setState({
          newModule: newModule
        });
      }
    }
  }, {
    key: "onFolderSelect",
    value: function onFolderSelect(key, option) {
      var props = this.props,
          state = this.state;
      var _this$state = this.state,
          newModule = _this$state.newModule,
          triedToSave = _this$state.triedToSave;

      if (key === "ownerFolder") {
        newModule.manifest.value = "";
        newModule.manifest.error = true;
        newModule.moduleFolder.value = "";
        newModule.moduleFolder.error = true;
        triedToSave = false;
        props.onSelectOwnerFolder(option.value);
      }

      if (key === "moduleFolder" && option.value !== "") {
        newModule.manifest.value = "";
        newModule.manifest.error = true;
        triedToSave = false;
        props.onSelectModuleFolder({
          ownerFolder: state.newModule.ownerFolder.value,
          moduleFolder: option.value,
          type: 2 //.dnn files

        });
      }

      if (newModule[key].required) {
        newModule[key].error = !option.value;
      }

      newModule[key].value = option.value;
      this.setState({
        newModule: newModule,
        triedToSave: triedToSave
      });
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var value = FromManifest_typeof(event) === "object" ? event.target.value : event;
      var newModule = this.state.newModule;
      newModule[key].value = value;

      if (newModule[key].required) {
        newModule[key].error = !value;
      }

      this.setState({
        newModule: newModule
      });
    }
  }, {
    key: "validateFields",
    value: function validateFields() {
      var _this$state2 = this.state,
          triedToSave = _this$state2.triedToSave,
          newModule = _this$state2.newModule;
      var errorCount = 0;
      Object.keys(newModule).forEach(function (key) {
        if (newModule[key].error) {
          errorCount++;
        }
      });
      triedToSave = true;
      this.setState({
        newModule: newModule,
        triedToSave: triedToSave
      });
      return errorCount === 0;
    }
  }, {
    key: "onCreateNewModule",
    value: function onCreateNewModule() {
      var state = this.state,
          props = this.props;
      var triedToSave = state.triedToSave;
      triedToSave = true;

      if (!this.validateFields()) {
        return;
      }

      this.setState({
        triedToSave: triedToSave
      });
      props.onCreateNewModule(valueMapNewModule(state.newModule));
    }
  }, {
    key: "onAddedNewFolder",
    value: function onAddedNewFolder(data, type) {
      var newModule = this.state.newModule;

      if (type === "ownerFolder") {
        newModule.ownerFolder.value = data.ownerFolder;
        this.props.onSelectOwnerFolder(data.ownerFolder);

        if (newModule.moduleFolder.value !== "") {
          newModule.moduleFolder.value = "";
          newModule.moduleFolder.error = true;
        }
      }

      if (type === "moduleFolder") {
        newModule.moduleFolder.value = data.moduleFolder;
        newModule.moduleFolder.error = false;
        newModule.manifest.value = "";
        newModule.manifest.error = true;
        this.setState({
          triedToSave: false
        });
      }

      this.setState({
        newModule: newModule
      });
    }
  }, {
    key: "onAddNewFolder",
    value: function onAddNewFolder(value, type, callback) {
      var newModule = this.state.newModule;
      var payload = {
        moduleFolder: type === "ownerFolder" ? "" : newModule.moduleFolder.value,
        ownerFolder: newModule.ownerFolder.value
      };
      payload[type] = value;
      this.props.onAddNewFolder(payload, type, callback);
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: FromManifest_style_default.a.fromManifest + " extension-form",
          style: {
            padding: 0
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          style: {
            paddingRight: 15
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_FolderDropdown, {
          folders: props.ownerFolders,
          label: localization.get("NewModule_OwnerFolder.Label"),
          type: "ownerFolder",
          tooltipMessage: localization.get("NewModule_OwnerFolder.HelpText"),
          onFolderSelect: this.onFolderSelect.bind(this, "ownerFolder"),
          value: state.newModule.ownerFolder.value,
          onAddNewFolder: this.onAddNewFolder.bind(this),
          onAddedNewFolder: this.onAddedNewFolder.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_FolderDropdown, {
          folders: props.moduleFolders,
          label: localization.get("NewModule_ModuleFolder.Label"),
          type: "moduleFolder",
          tooltipMessage: localization.get("NewModule_ModuleFolder.HelpText"),
          onFolderSelect: this.onFolderSelect.bind(this, "moduleFolder"),
          value: state.newModule.moduleFolder.value,
          onAddNewFolder: this.onAddNewFolder.bind(this),
          error: state.newModule.moduleFolder.error && state.triedToSave,
          onAddedNewFolder: this.onAddedNewFolder.bind(this)
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          style: {
            paddingLeft: 15
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          label: localization.get("NewModule_Resource.Label"),
          tooltipMessage: localization.get("NewModule_Resource.HelpText"),
          style: dropdownStyle,
          options: props.moduleFiles,
          onSelect: this.onFolderSelect.bind(this, "manifest"),
          value: state.newModule.manifest.value,
          error: state.newModule.manifest.error && state.triedToSave
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: state.newModule.addPage.value,
          tooltipMessage: localization.get("NewModule_AddTestPage.HelpText"),
          label: localization.get("NewModule_AddTestPage.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          onChange: this.onChange.bind(this, "addPage")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("NewModule_Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.onCreateNewModule.bind(this)
        }, localization.get("NewModule_Create.Button"))))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return FromManifest;
}(external_window_dnn_nodeModules_React_["Component"]);

FromManifest_FromManifest.propTypes = {
  onCancel: prop_types_default.a.func,
  ownerFolders: prop_types_default.a.array,
  moduleFolders: prop_types_default.a.array,
  moduleFiles: prop_types_default.a.array,
  onCreateNewModule: prop_types_default.a.func,
  onSelectOwnerFolder: prop_types_default.a.func,
  onSelectModuleFolder: prop_types_default.a.func,
  onAddNewFolder: prop_types_default.a.func,
  retrieveOwnerAndModuleFolders: prop_types_default.a.func
};
/* harmony default export */ var NewModuleModal_FromManifest = (FromManifest_FromManifest);
// EXTERNAL MODULE: ./src/components/NewModuleModal/FromNew/style.less
var FromNew_style = __webpack_require__(43);
var FromNew_style_default = /*#__PURE__*/__webpack_require__.n(FromNew_style);

// CONCATENATED MODULE: ./src/components/NewModuleModal/FromNew/index.jsx
function FromNew_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { FromNew_typeof = function _typeof(obj) { return typeof obj; }; } else { FromNew_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return FromNew_typeof(obj); }

function FromNew_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function FromNew_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function FromNew_createClass(Constructor, protoProps, staticProps) { if (protoProps) FromNew_defineProperties(Constructor.prototype, protoProps); if (staticProps) FromNew_defineProperties(Constructor, staticProps); return Constructor; }

function FromNew_possibleConstructorReturn(self, call) { if (call && (FromNew_typeof(call) === "object" || typeof call === "function")) { return call; } return FromNew_assertThisInitialized(self); }

function FromNew_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function FromNew_getPrototypeOf(o) { FromNew_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return FromNew_getPrototypeOf(o); }

function FromNew_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) FromNew_setPrototypeOf(subClass, superClass); }

function FromNew_setPrototypeOf(o, p) { FromNew_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return FromNew_setPrototypeOf(o, p); }








var FromNew_inputStyle = {
  width: "100%"
};

function FromNew_getValidateRequired(key) {
  switch (key) {
    case "moduleFolder":
    case "fileName":
    case "moduleName":
    case "language":
      return true;

    default:
      return false;
  }
}

var FromNew_emptyNewModule = {
  type: 0,
  moduleFolder: "",
  ownerFolder: "",
  addPage: true,
  fileName: "",
  moduleName: "",
  description: "",
  language: "C#"
};

var FromNew_FromNew =
/*#__PURE__*/
function (_Component) {
  FromNew_inherits(FromNew, _Component);

  function FromNew() {
    var _this;

    FromNew_classCallCheck(this, FromNew);

    _this = FromNew_possibleConstructorReturn(this, FromNew_getPrototypeOf(FromNew).call(this));
    _this.state = {
      newModule: validationMapNewModule(FromNew_emptyNewModule, FromNew_getValidateRequired),
      triedToSave: false
    };
    return _this;
  }

  FromNew_createClass(FromNew, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      this.props.retrieveOwnerAndModuleFolders();
    }
  }, {
    key: "onFolderSelect",
    value: function onFolderSelect(key, option) {
      var props = this.props;
      var _this$state = this.state,
          newModule = _this$state.newModule,
          triedToSave = _this$state.triedToSave;

      if (key === "ownerFolder") {
        newModule.moduleFolder.value = "";
        newModule.moduleFolder.error = true;
        triedToSave = false;
        props.onSelectOwnerFolder(option.value);
      }

      if (newModule[key].required) {
        newModule[key].error = !option.value;
      }

      newModule[key].value = option.value;
      this.setState({
        newModule: newModule,
        triedToSave: triedToSave
      });
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var value = FromNew_typeof(event) === "object" ? event.target.value : event;
      var newModule = this.state.newModule;
      newModule[key].value = value;

      if (newModule[key].required) {
        newModule[key].error = !value;
      }

      this.setState({
        newModule: newModule
      });
    }
  }, {
    key: "validateFields",
    value: function validateFields() {
      var _this$state2 = this.state,
          triedToSave = _this$state2.triedToSave,
          newModule = _this$state2.newModule;
      var errorCount = 0;
      Object.keys(newModule).forEach(function (key) {
        if (newModule[key].error) {
          errorCount++;
        }
      });
      triedToSave = true;
      this.setState({
        newModule: newModule,
        triedToSave: triedToSave
      });
      return errorCount === 0;
    }
  }, {
    key: "onCreateNewModule",
    value: function onCreateNewModule() {
      var state = this.state,
          props = this.props;
      var triedToSave = state.triedToSave;
      triedToSave = true;

      if (!this.validateFields()) {
        return;
      }

      this.setState({
        triedToSave: triedToSave
      });
      props.onCreateNewModule(valueMapNewModule(state.newModule));
    }
  }, {
    key: "onAddedNewFolder",
    value: function onAddedNewFolder(data, type) {
      var newModule = this.state.newModule;

      if (type === "ownerFolder") {
        newModule.ownerFolder.value = data.ownerFolder;
        this.props.onSelectOwnerFolder(data.ownerFolder);

        if (newModule.moduleFolder.value !== "") {
          newModule.moduleFolder.value = "";
          newModule.moduleFolder.error = true;
        }

        this.setState({
          triedToSave: false
        });
      }

      if (type === "moduleFolder") {
        newModule.moduleFolder.value = data.moduleFolder;
        newModule.moduleFolder.error = false;
      }

      this.setState({
        newModule: newModule
      });
    }
  }, {
    key: "onAddNewFolder",
    value: function onAddNewFolder(value, type, callback) {
      var newModule = this.state.newModule;
      var payload = {
        moduleFolder: type === "ownerFolder" ? "" : newModule.moduleFolder.value,
        ownerFolder: newModule.ownerFolder.value
      };
      payload[type] = value;
      this.props.onAddNewFolder(payload, type, callback);
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: FromNew_style_default.a.fromNew + " extension-form",
          style: {
            padding: 0
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          style: {
            paddingRight: 15
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_FolderDropdown, {
          folders: props.ownerFolders,
          label: localization.get("NewModule_OwnerFolder.Label"),
          type: "ownerFolder",
          tooltipMessage: localization.get("NewModule_OwnerFolder.HelpText"),
          onFolderSelect: this.onFolderSelect.bind(this, "ownerFolder"),
          value: state.newModule.ownerFolder.value,
          onAddNewFolder: this.onAddNewFolder.bind(this),
          onAddedNewFolder: this.onAddedNewFolder.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_FolderDropdown, {
          folders: props.moduleFolders,
          label: localization.get("NewModule_ModuleFolder.Label"),
          type: "moduleFolder",
          tooltipMessage: localization.get("NewModule_ModuleFolder.HelpText"),
          onFolderSelect: this.onFolderSelect.bind(this, "moduleFolder"),
          value: state.newModule.moduleFolder.value,
          onAddNewFolder: this.onAddNewFolder.bind(this),
          error: state.newModule.moduleFolder.error && state.triedToSave,
          onAddedNewFolder: this.onAddedNewFolder.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("NewModule_FileName.Label"),
          tooltipMessage: localization.get("NewModule_FileName.HelpText"),
          style: FromNew_inputStyle,
          inputStyle: {
            marginBottom: 0
          },
          onChange: this.onChange.bind(this, "fileName"),
          value: state.newModule.fileName.value,
          error: state.newModule.fileName.error && state.triedToSave
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["RadioButtons"], {
          options: [{
            label: "Visual Basic",
            value: "Visual Basic"
          }, {
            label: "C Sharp",
            value: "C#"
          }],
          label: localization.get("NewModule_Language.Label"),
          value: state.newModule.language.value,
          buttonGroup: "language",
          tooltipMessage: localization.get("NewModule_Language.HelpText"),
          onChange: this.onChange.bind(this, "language"),
          float: "right"
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          style: {
            paddingLeft: 15
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("NewModule_ModuleName.Label"),
          tooltipMessage: localization.get("NewModule_ModuleName.HelpText"),
          style: FromNew_inputStyle,
          inputStyle: {
            marginBottom: 32
          },
          value: state.newModule.moduleName.value,
          onChange: this.onChange.bind(this, "moduleName"),
          error: state.newModule.moduleName.error && state.triedToSave
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInputWithError"], {
          label: localization.get("NewModule_Description.Label"),
          tooltipMessage: localization.get("NewModule_Description.HelpText"),
          style: FromNew_inputStyle,
          inputStyle: {
            marginBottom: 32,
            height: 126
          },
          onChange: this.onChange.bind(this, "description"),
          value: state.newModule.description.value
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          value: state.newModule.addPage.value,
          tooltipMessage: localization.get("NewModule_AddTestPage.HelpText"),
          label: localization.get("NewModule_AddTestPage.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          onChange: this.onChange.bind(this, "addPage")
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("NewModule_Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.onCreateNewModule.bind(this)
        }, localization.get("NewModule_Create.Button"))))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return FromNew;
}(external_window_dnn_nodeModules_React_["Component"]);

FromNew_FromNew.propTypes = {
  onCancel: prop_types_default.a.func,
  ownerFolders: prop_types_default.a.array,
  onCreateNewModule: prop_types_default.a.func,
  onSelectOwnerFolder: prop_types_default.a.func,
  onSelectModuleFolder: prop_types_default.a.func,
  onAddNewFolder: prop_types_default.a.func,
  retrieveOwnerAndModuleFolders: prop_types_default.a.func
};
/* harmony default export */ var NewModuleModal_FromNew = (FromNew_FromNew);
// EXTERNAL MODULE: ./src/components/NewModuleModal/style.less
var NewModuleModal_style = __webpack_require__(44);
var NewModuleModal_style_default = /*#__PURE__*/__webpack_require__.n(NewModuleModal_style);

// CONCATENATED MODULE: ./src/components/NewModuleModal/index.jsx
function NewModuleModal_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { NewModuleModal_typeof = function _typeof(obj) { return typeof obj; }; } else { NewModuleModal_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return NewModuleModal_typeof(obj); }

function NewModuleModal_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function NewModuleModal_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function NewModuleModal_createClass(Constructor, protoProps, staticProps) { if (protoProps) NewModuleModal_defineProperties(Constructor.prototype, protoProps); if (staticProps) NewModuleModal_defineProperties(Constructor, staticProps); return Constructor; }

function NewModuleModal_possibleConstructorReturn(self, call) { if (call && (NewModuleModal_typeof(call) === "object" || typeof call === "function")) { return call; } return NewModuleModal_assertThisInitialized(self); }

function NewModuleModal_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function NewModuleModal_getPrototypeOf(o) { NewModuleModal_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return NewModuleModal_getPrototypeOf(o); }

function NewModuleModal_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) NewModuleModal_setPrototypeOf(subClass, superClass); }

function NewModuleModal_setPrototypeOf(o, p) { NewModuleModal_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return NewModuleModal_setPrototypeOf(o, p); }











var NewModuleModal_inputStyle = {
  width: "100%"
};
var newModuleTypes = [{
  label: "New",
  value: 0
}, {
  label: "Control",
  value: 1
}, {
  label: "Manifest",
  value: 2
}];

var NewModuleModal_NewModuleModal =
/*#__PURE__*/
function (_Component) {
  NewModuleModal_inherits(NewModuleModal, _Component);

  function NewModuleModal() {
    var _this;

    NewModuleModal_classCallCheck(this, NewModuleModal);

    _this = NewModuleModal_possibleConstructorReturn(this, NewModuleModal_getPrototypeOf(NewModuleModal).call(this));
    _this.state = {
      selectedType: ""
    };
    return _this;
  }

  NewModuleModal_createClass(NewModuleModal, [{
    key: "onSelectOwnerFolder",
    value: function onSelectOwnerFolder(ownerFolder) {
      var props = this.props;
      props.dispatch(folder.getModuleFolders(ownerFolder));
    }
  }, {
    key: "onSelectModuleFolder",
    value: function onSelectModuleFolder(parameters) {
      var props = this.props;
      props.dispatch(folder.getModuleFiles(parameters));
    }
  }, {
    key: "retrieveOwnerAndModuleFolders",
    value: function retrieveOwnerAndModuleFolders() {
      this.props.dispatch(folder.getOwnerFolders());
      this.props.dispatch(folder.getModuleFolders(""));
    }
  }, {
    key: "onSelectNewModuleType",
    value: function onSelectNewModuleType(option) {
      this.setState({
        selectedType: option.value
      });
    }
  }, {
    key: "getCreateButtonActive",
    value: function getCreateButtonActive() {
      return this.state.selectedType !== 0 && this.state.selectedType !== 1 && this.state.selectedType !== 2;
    }
  }, {
    key: "onAddNewFolder",
    value: function onAddNewFolder(payload, type, callback) {
      var props = this.props;
      props.dispatch(folder.createFolder(payload, type, callback));
    }
  }, {
    key: "onCreateNewModule",
    value: function onCreateNewModule(payload) {
      var _this2 = this;

      var props = this.props;
      var shouldAppend = props.selectedInstalledPackageType === "Module";
      props.dispatch(extension.createNewModule(payload, shouldAppend, function (data) {
        if (data.NewPageUrl) {
          window.open(data.NewPageUrl);
        }

        if (!shouldAppend) {
          props.dispatch(extension.getInstalledPackages("Module"));
        }

        _this2.onCancel();
      }));
    }
  }, {
    key: "getCreateUI",
    value: function getCreateUI(selectedType) {
      var props = this.props;
      var ownerFolders = props.ownerFolders.map(function (folder) {
        return {
          value: folder,
          label: folder.split(/(?=[A-Z])/).join(" ")
        };
      });
      var moduleFolders = props.moduleFolders.map(function (folder) {
        return {
          value: folder,
          label: folder.split(/(?=[A-Z])/).join(" ")
        };
      });
      var moduleFiles = props.moduleFiles.map(function (file) {
        return {
          value: file,
          label: file
        };
      });

      switch (selectedType) {
        case 0:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(NewModuleModal_FromNew, {
              ownerFolders: ownerFolders,
              moduleFolders: moduleFolders,
              onCancel: this.onCancel.bind(this),
              onSelectOwnerFolder: this.onSelectOwnerFolder.bind(this),
              onAddNewFolder: this.onAddNewFolder.bind(this),
              onCreateNewModule: this.onCreateNewModule.bind(this),
              retrieveOwnerAndModuleFolders: this.retrieveOwnerAndModuleFolders.bind(this)
            })
          );

        case 1:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(NewModuleModal_FromControl, {
              ownerFolders: ownerFolders,
              moduleFolders: moduleFolders,
              moduleFiles: moduleFiles,
              onCancel: this.onCancel.bind(this),
              onSelectOwnerFolder: this.onSelectOwnerFolder.bind(this),
              onAddNewFolder: this.onAddNewFolder.bind(this),
              onSelectModuleFolder: this.onSelectModuleFolder.bind(this),
              onCreateNewModule: this.onCreateNewModule.bind(this),
              retrieveOwnerAndModuleFolders: this.retrieveOwnerAndModuleFolders.bind(this)
            })
          );

        case 2:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(NewModuleModal_FromManifest, {
              ownerFolders: ownerFolders,
              moduleFolders: moduleFolders,
              moduleFiles: moduleFiles,
              onCancel: this.onCancel.bind(this),
              onSelectOwnerFolder: this.onSelectOwnerFolder.bind(this),
              onAddNewFolder: this.onAddNewFolder.bind(this),
              onSelectModuleFolder: this.onSelectModuleFolder.bind(this),
              onCreateNewModule: this.onCreateNewModule.bind(this),
              retrieveOwnerAndModuleFolders: this.retrieveOwnerAndModuleFolders.bind(this)
            })
          );

        default:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("div", null)
          );
      }
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var props = this.props;
      props.dispatch(visiblePanel.selectPanel(0));
    }
  }, {
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: NewModuleModal_style_default.a.newModuleModal
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: localization.get("CreateModule.Action")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], {
          backToLinkProps: {
            text: localization.get("BackToExtensions"),
            onClick: this.onCancel.bind(this)
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "new-module-box extension-form"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          style: {
            marginBottom: 15,
            padding: 0
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "new-module-dropdown-container"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          className: "create-new-module-dropdown",
          options: newModuleTypes,
          tooltipMessage: localization.get("CreateNewModule.HelpText"),
          value: this.state.selectedType,
          onSelect: this.onSelectNewModuleType.bind(this),
          label: localization.get("CreateNewModuleFrom.Label"),
          style: NewModuleModal_inputStyle
        }), this.state.selectedType === "" &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: this.onCancel.bind(this)
        }, localization.get("NewModule_Cancel.Button")))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          style: {
            padding: 0
          }
        }, this.getCreateUI(this.state.selectedType)))))
      );
    }
  }]);

  return NewModuleModal;
}(external_window_dnn_nodeModules_React_["Component"]);

NewModuleModal_NewModuleModal.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  onCancel: prop_types_default.a.func,
  selectedInstalledPackageType: prop_types_default.a.string,
  ownerFolders: prop_types_default.a.array,
  moduleFolders: prop_types_default.a.array,
  moduleFiles: prop_types_default.a.array
};

function NewModuleModal_mapStateToProps(state) {
  return {
    selectedInstalledPackageType: state.extension.selectedInstalledPackageType,
    ownerFolders: state.folder.ownerFolders,
    moduleFolders: state.folder.moduleFolders,
    moduleFiles: state.folder.moduleFiles
  };
}

/* harmony default export */ var components_NewModuleModal = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(NewModuleModal_mapStateToProps)(NewModuleModal_NewModuleModal));
// EXTERNAL MODULE: ./src/components/installExtensionModal/InstallLog/style.less
var InstallLog_style = __webpack_require__(90);

// CONCATENATED MODULE: ./src/components/installExtensionModal/InstallLog/index.jsx
function InstallLog_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { InstallLog_typeof = function _typeof(obj) { return typeof obj; }; } else { InstallLog_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return InstallLog_typeof(obj); }

function InstallLog_extends() { InstallLog_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return InstallLog_extends.apply(this, arguments); }

function InstallLog_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function InstallLog_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function InstallLog_createClass(Constructor, protoProps, staticProps) { if (protoProps) InstallLog_defineProperties(Constructor.prototype, protoProps); if (staticProps) InstallLog_defineProperties(Constructor, staticProps); return Constructor; }

function InstallLog_possibleConstructorReturn(self, call) { if (call && (InstallLog_typeof(call) === "object" || typeof call === "function")) { return call; } return InstallLog_assertThisInitialized(self); }

function InstallLog_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function InstallLog_getPrototypeOf(o) { InstallLog_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return InstallLog_getPrototypeOf(o); }

function InstallLog_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) InstallLog_setPrototypeOf(subClass, superClass); }

function InstallLog_setPrototypeOf(o, p) { InstallLog_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return InstallLog_setPrototypeOf(o, p); }







var licenseBoxStyle = {
  border: "1px solid #c8c8c8",
  marginBottom: 25,
  height: 250,
  width: "100%"
};

var InstallLog_EditExtension =
/*#__PURE__*/
function (_Component) {
  InstallLog_inherits(EditExtension, _Component);

  function EditExtension() {
    InstallLog_classCallCheck(this, EditExtension);

    return InstallLog_possibleConstructorReturn(this, InstallLog_getPrototypeOf(EditExtension).apply(this, arguments));
  }

  InstallLog_createClass(EditExtension, [{
    key: "render",
    value: function render() {
      var props = this.props;
      var errorCount = props.logs.filter(function (log) {
        return log.Type === "Failure" || log.Type === "Error";
      }).length;
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          style: {
            padding: 50
          },
          className: "extension-install-logs" + (errorCount > 0 ? " with-error" : "")
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("InstallExtension_Logs.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
          messages: [localization.get("InstallationError")],
          type: "error",
          rendered: errorCount > 0,
          className: "install-error-tooltip"
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", null, localization.get("InstallExtension_Logs.HelpText")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCustomScrollBars_["Scrollbars"], {
          style: errorCount > 0 ? InstallLog_extends({
            borderBottom: "2px solid #EA2134"
          }, licenseBoxStyle) : licenseBoxStyle
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "package-installation-report"
        }, props.logs.map(function (log, i) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("p", {
              className: log.Type.toLowerCase(),
              key: i
            }, log.Type + " " + log.Description)
          );
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onDone.bind(this)
        }, localization.get("Done.Button"))))
      );
    }
  }]);

  return EditExtension;
}(external_window_dnn_nodeModules_React_["Component"]);

InstallLog_EditExtension.propTypes = {
  onDone: prop_types_default.a.func,
  logs: prop_types_default.a.array
};
/* harmony default export */ var InstallLog = (InstallLog_EditExtension);
// CONCATENATED MODULE: ./src/components/EditExtension/PackageInformation/helperFunctions.js
function helperFunctions_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { helperFunctions_typeof = function _typeof(obj) { return typeof obj; }; } else { helperFunctions_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return helperFunctions_typeof(obj); }

function PackageInformation_helperFunctions_extends() { PackageInformation_helperFunctions_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return PackageInformation_helperFunctions_extends.apply(this, arguments); }

function helperFunctions_formatVersionNumber(n) {
  return n > 9 ? "" + n : "0" + n;
}

function helperFunctions_getValidateRequired(key) {
  switch (key) {
    case "friendlyName":
      return true;

    default:
      return false;
  }
}

function helperFunctions_validationMapExtensionBeingEdited(extensionBeingEdited) {
  var _extensionBeingEdited = PackageInformation_helperFunctions_extends({}, extensionBeingEdited);

  Object.keys(_extensionBeingEdited).forEach(function (key) {
    var validateRequired = helperFunctions_getValidateRequired(key);
    _extensionBeingEdited[key] = {
      value: helperFunctions_typeof(_extensionBeingEdited[key]) !== "object" ? _extensionBeingEdited[key] : _extensionBeingEdited[key].value,
      validateRequired: validateRequired,
      error: false
    };
  });
  return _extensionBeingEdited;
}
function helperFunctions_valueMapExtensionBeingEdited(extensionBeingEdited) {
  var _extensionBeingEdited = PackageInformation_helperFunctions_extends({}, extensionBeingEdited);

  Object.keys(_extensionBeingEdited).forEach(function (key) {
    _extensionBeingEdited[key] = _extensionBeingEdited[key].value;
  });
  return _extensionBeingEdited;
}
function helperFunctions_getVersionDropdownValues() {
  var values = [];

  for (var i = 0; i < 100; i++) {
    values.push({
      label: helperFunctions_formatVersionNumber(i),
      value: i
    });
  }

  return values;
}
// EXTERNAL MODULE: ./src/components/EditExtension/PackageInformation/style.less
var PackageInformation_style = __webpack_require__(45);
var PackageInformation_style_default = /*#__PURE__*/__webpack_require__.n(PackageInformation_style);

// CONCATENATED MODULE: ./src/components/EditExtension/PackageInformation/index.jsx
function PackageInformation_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { PackageInformation_typeof = function _typeof(obj) { return typeof obj; }; } else { PackageInformation_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return PackageInformation_typeof(obj); }

function PackageInformation_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function PackageInformation_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function PackageInformation_createClass(Constructor, protoProps, staticProps) { if (protoProps) PackageInformation_defineProperties(Constructor.prototype, protoProps); if (staticProps) PackageInformation_defineProperties(Constructor, staticProps); return Constructor; }

function PackageInformation_possibleConstructorReturn(self, call) { if (call && (PackageInformation_typeof(call) === "object" || typeof call === "function")) { return call; } return PackageInformation_assertThisInitialized(self); }

function PackageInformation_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function PackageInformation_getPrototypeOf(o) { PackageInformation_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return PackageInformation_getPrototypeOf(o); }

function PackageInformation_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) PackageInformation_setPrototypeOf(subClass, superClass); }

function PackageInformation_setPrototypeOf(o, p) { PackageInformation_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return PackageInformation_setPrototypeOf(o, p); }







var PackageInformation_inputStyle = {
  width: "100%"
};

var PackageInformation_PackageInformation =
/*#__PURE__*/
function (_Component) {
  PackageInformation_inherits(PackageInformation, _Component);

  function PackageInformation() {
    var _this;

    PackageInformation_classCallCheck(this, PackageInformation);

    _this = PackageInformation_possibleConstructorReturn(this, PackageInformation_getPrototypeOf(PackageInformation).call(this));
    _this.state = {
      extensionBeingEdited: helperFunctions_validationMapExtensionBeingEdited({
        packageType: "",
        name: "",
        description: "",
        friendlyName: "",
        version: "9.0.0",
        owner: "",
        url: "",
        organization: "",
        email: ""
      }),
      triedToSave: false
    };
    return _this;
  }

  PackageInformation_createClass(PackageInformation, [{
    key: "render",
    value: function render() {
      var props = this.props;

      if (!props.extensionBeingEdited) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("p", null, "Empty")
        );
      }

      var extensionBeingEdited = props.extensionBeingEdited;
      var version = props.validationMapped ? extensionBeingEdited.version.value ? extensionBeingEdited.version.value.split(".") : [0, 0, 0] : extensionBeingEdited.version ? extensionBeingEdited.version.split(".") : [0, 0, 0];
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: PackageInformation_style_default.a.pkgInformationBox
        }, props.installationMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "package-information-header"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("InstallExtension_PackageInfo.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", null, localization.get("InstallExtension_PackageInfo.HelpText")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("hr", null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          style: {
            marginBottom: 15
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          tooltipMessage: localization.get("EditExtension_PackageType.HelpText"),
          label: localization.get("EditExtension_PackageType.Label"),
          defaultDropdownValue: !props.validationMapped ? extensionBeingEdited.packageType : extensionBeingEdited.packageType.value,
          style: PackageInformation_inputStyle,
          enabled: false
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border top-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageName.Label"),
          tooltipMessage: localization.get("EditExtension_PackageName.HelpText"),
          style: PackageInformation_inputStyle,
          enabled: false,
          value: !props.validationMapped ? extensionBeingEdited.name : extensionBeingEdited.name.value
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageFriendlyName.Label") + "*",
          tooltipMessage: localization.get("EditExtension_PackageFriendlyName.HelpText"),
          value: !props.validationMapped ? extensionBeingEdited.friendlyName : extensionBeingEdited.friendlyName.value,
          style: PackageInformation_inputStyle,
          error: extensionBeingEdited.friendlyName.error && props.triedToSave,
          enabled: !props.disabled,
          onChange: props.onChange && props.onChange.bind(this, "friendlyName")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageIconFile.Label"),
          tooltipMessage: localization.get("EditExtension_PackageIconFile.HelpText"),
          value: !props.validationMapped ? extensionBeingEdited.packageIcon : extensionBeingEdited.packageIcon.value,
          style: PackageInformation_inputStyle,
          inputStyle: {
            marginBottom: 0
          },
          enabled: !props.disabled,
          onChange: props.onChange && props.onChange.bind(this, "packageIcon")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInputWithError"], {
          label: localization.get("EditExtension_PackageDescription.Label"),
          tooltipMessage: localization.get("EditExtension_PackageDescription.HelpText"),
          style: PackageInformation_inputStyle,
          inputStyle: {
            marginBottom: 28,
            height: 123
          },
          value: !props.validationMapped ? extensionBeingEdited.description : extensionBeingEdited.description.value,
          enabled: !props.disabled,
          onChange: props.onChange && props.onChange.bind(this, "description")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
          options: helperFunctions_getVersionDropdownValues(),
          label: localization.get("EditExtension_PackageVersion.Label"),
          tooltipMessage: localization.get("EditExtension_PackageVersion.HelpText"),
          enabled: !props.disabled,
          defaultDropdownValue: helperFunctions_formatVersionNumber(version[0]),
          onSelect: props.onVersionChange && props.onVersionChange.bind(this, 0),
          className: "version-dropdown",
          style: {
            position: "relative",
            top: 3
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: helperFunctions_getVersionDropdownValues(),
          className: "version-dropdown",
          label: helperFunctions_formatVersionNumber(version[1]),
          onSelect: props.onVersionChange && props.onVersionChange.bind(this, 1),
          enabled: !props.disabled
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Dropdown"], {
          options: helperFunctions_getVersionDropdownValues(),
          label: helperFunctions_formatVersionNumber(version[2]),
          className: "version-dropdown",
          onSelect: props.onVersionChange && props.onVersionChange.bind(this, 2),
          enabled: !props.disabled
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("hr", null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
          className: "with-right-border bottom-half"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageOwner.Label"),
          tooltipMessage: localization.get("EditExtension_PackageOwner.HelpText"),
          style: PackageInformation_inputStyle,
          value: !props.validationMapped ? extensionBeingEdited.owner : extensionBeingEdited.owner.value,
          enabled: !props.disabled,
          onChange: props.onChange && props.onChange.bind(this, "owner")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageOrganization.Label"),
          tooltipMessage: localization.get("EditExtension_PackageOrganization.HelpText"),
          style: PackageInformation_inputStyle,
          inputStyle: {
            marginBottom: 0
          },
          value: !props.validationMapped ? extensionBeingEdited.organization : extensionBeingEdited.organization.value,
          enabled: !props.disabled,
          onChange: props.onChange && props.onChange.bind(this, "organization")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageURL.Label"),
          tooltipMessage: localization.get("EditExtension_PackageURL.HelpText"),
          style: PackageInformation_inputStyle,
          inputStyle: {
            marginBottom: 32
          },
          value: !props.validationMapped ? extensionBeingEdited.url : extensionBeingEdited.url.value,
          enabled: !props.disabled,
          onChange: props.onChange && props.onChange.bind(this, "url")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("EditExtension_PackageEmailAddress.Label"),
          tooltipMessage: localization.get("EditExtension_PackageEmailAddress.HelpText"),
          style: PackageInformation_inputStyle,
          inputStyle: {
            marginBottom: 32
          },
          value: !props.validationMapped ? extensionBeingEdited.email : extensionBeingEdited.email.value,
          enabled: !props.disabled,
          onChange: props.onChange && props.onChange.bind(this, "email")
        }))), !props.buttonsAreHidden &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")), (!props.disabled || props.installationMode) &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this)
        }, props.primaryButtonText)))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return PackageInformation;
}(external_window_dnn_nodeModules_React_["Component"]);

PackageInformation_PackageInformation.propTypes = {
  onCancel: prop_types_default.a.func,
  onPrimaryButtonClick: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  disabled: prop_types_default.a.bool,
  primaryButtonText: prop_types_default.a.string,
  triedToSave: prop_types_default.a.bool,
  validateFields: prop_types_default.a.func,
  validationMapped: prop_types_default.a.bool
};
/* harmony default export */ var EditExtension_PackageInformation = (PackageInformation_PackageInformation);
// EXTERNAL MODULE: ./src/components/EditExtension/ReleaseNotes/style.less
var ReleaseNotes_style = __webpack_require__(93);

// CONCATENATED MODULE: ./src/components/EditExtension/ReleaseNotes/index.jsx
function ReleaseNotes_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { ReleaseNotes_typeof = function _typeof(obj) { return typeof obj; }; } else { ReleaseNotes_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return ReleaseNotes_typeof(obj); }

function ReleaseNotes_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function ReleaseNotes_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function ReleaseNotes_createClass(Constructor, protoProps, staticProps) { if (protoProps) ReleaseNotes_defineProperties(Constructor.prototype, protoProps); if (staticProps) ReleaseNotes_defineProperties(Constructor, staticProps); return Constructor; }

function ReleaseNotes_possibleConstructorReturn(self, call) { if (call && (ReleaseNotes_typeof(call) === "object" || typeof call === "function")) { return call; } return ReleaseNotes_assertThisInitialized(self); }

function ReleaseNotes_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function ReleaseNotes_getPrototypeOf(o) { ReleaseNotes_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return ReleaseNotes_getPrototypeOf(o); }

function ReleaseNotes_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) ReleaseNotes_setPrototypeOf(subClass, superClass); }

function ReleaseNotes_setPrototypeOf(o, p) { ReleaseNotes_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return ReleaseNotes_setPrototypeOf(o, p); }







var ReleaseNotes_inputStyle = {
  width: "100%"
};
var releaseBoxStyle = {
  border: "1px solid #c8c8c8",
  marginBottom: 25,
  height: 250,
  width: "100%"
};

var ReleaseNotes_ReleaseNotes =
/*#__PURE__*/
function (_Component) {
  ReleaseNotes_inherits(ReleaseNotes, _Component);

  function ReleaseNotes() {
    ReleaseNotes_classCallCheck(this, ReleaseNotes);

    return ReleaseNotes_possibleConstructorReturn(this, ReleaseNotes_getPrototypeOf(ReleaseNotes).apply(this, arguments));
  }

  ReleaseNotes_createClass(ReleaseNotes, [{
    key: "render",
    value: function render() {
      var props = this.props;
      var value = props.value;
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "release-notes extension-form"
        }, props.installationMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("InstallExtension_ReleaseNotes.Header")), props.installationMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", null, localization.get("InstallExtension_ReleaseNotes.HelpText")), !props.readOnly &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInputWithError"], {
          label: !props.installationMode && localization.get("InstallExtension_ReleaseNotes.Header"),
          style: ReleaseNotes_inputStyle,
          enabled: !props.disabled,
          value: value,
          onChange: props.onChange && props.onChange.bind(this, "releaseNotes")
        }), props.readOnly &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCustomScrollBars_["Scrollbars"], {
          style: releaseBoxStyle
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "read-only-release-notes",
          dangerouslySetInnerHTML: {
            __html: value
          }
        })), !props.buttonsAreHidden &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")), (!props.disabled || props.installationMode) &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this)
        }, props.primaryButtonText)))
      );
    }
  }]);

  return ReleaseNotes;
}(external_window_dnn_nodeModules_React_["Component"]);

ReleaseNotes_ReleaseNotes.propTypes = {
  onCancel: prop_types_default.a.func,
  readOnly: prop_types_default.a.bool,
  onSave: prop_types_default.a.func,
  value: prop_types_default.a.string,
  onChange: prop_types_default.a.func,
  primaryButtonText: prop_types_default.a.string
};
/* harmony default export */ var EditExtension_ReleaseNotes = (ReleaseNotes_ReleaseNotes);
// EXTERNAL MODULE: ./src/components/EditExtension/License/style.less
var License_style = __webpack_require__(95);

// CONCATENATED MODULE: ./src/components/EditExtension/License/index.jsx
function License_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { License_typeof = function _typeof(obj) { return typeof obj; }; } else { License_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return License_typeof(obj); }

function License_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function License_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function License_createClass(Constructor, protoProps, staticProps) { if (protoProps) License_defineProperties(Constructor.prototype, protoProps); if (staticProps) License_defineProperties(Constructor, staticProps); return Constructor; }

function License_possibleConstructorReturn(self, call) { if (call && (License_typeof(call) === "object" || typeof call === "function")) { return call; } return License_assertThisInitialized(self); }

function License_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function License_getPrototypeOf(o) { License_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return License_getPrototypeOf(o); }

function License_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) License_setPrototypeOf(subClass, superClass); }

function License_setPrototypeOf(o, p) { License_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return License_setPrototypeOf(o, p); }







var License_inputStyle = {
  width: "100%"
};
var License_licenseBoxStyle = {
  border: "1px solid #c8c8c8",
  marginBottom: 25,
  height: 250,
  width: "100%"
};

var License_License =
/*#__PURE__*/
function (_Component) {
  License_inherits(License, _Component);

  function License() {
    License_classCallCheck(this, License);

    return License_possibleConstructorReturn(this, License_getPrototypeOf(License).apply(this, arguments));
  }

  License_createClass(License, [{
    key: "render",
    value: function render() {
      var props = this.props;
      var value = props.value;
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "extension-license extension-form"
        }, props.installationMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h6", null, localization.get("InstallExtension_License.Header")), props.installationMode &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          dangerouslySetInnerHTML: {
            __html: localization.get("InstallExtension_License.HelpText").replace("\\n", "<br/>")
          }
        }), !props.readOnly &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInputWithError"], {
          label: !props.installationMode && localization.get("InstallExtension_License.Header"),
          style: License_inputStyle,
          value: value,
          enabled: !props.disabled,
          onChange: props.onChange && props.onChange.bind(this, "license")
        }), props.readOnly &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCustomScrollBars_["Scrollbars"], {
          style: License_licenseBoxStyle
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "read-only-license",
          dangerouslySetInnerHTML: {
            __html: value
          }
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Checkbox"], {
          label: localization.get("InstallExtension_AcceptLicense.Label"),
          value: props.licenseAccepted,
          onChange: props.onToggleLicenseAccept
        }), !props.buttonsAreHidden &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")), !props.disabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")), (!props.disabled || props.installationMode) &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this),
          disabled: props.primaryButtonDisabled
        }, props.primaryButtonText)))
      );
    }
  }]);

  return License;
}(external_window_dnn_nodeModules_React_["Component"]);

License_License.propTypes = {
  onCancel: prop_types_default.a.func,
  readOnly: prop_types_default.a.bool,
  onSave: prop_types_default.a.func,
  value: prop_types_default.a.string,
  onChange: prop_types_default.a.func,
  primaryButtonText: prop_types_default.a.string,
  licenseAccepted: prop_types_default.a.bool,
  onToggleLicenseAccept: prop_types_default.a.func
};
License_License.defaultProps = {
  licenseAccepted: false
};
/* harmony default export */ var EditExtension_License = (License_License);
// CONCATENATED MODULE: ./src/components/installExtensionModal/FileUpload/UploadBar.jsx
function UploadBar_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { UploadBar_typeof = function _typeof(obj) { return typeof obj; }; } else { UploadBar_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return UploadBar_typeof(obj); }

function UploadBar_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function UploadBar_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function UploadBar_createClass(Constructor, protoProps, staticProps) { if (protoProps) UploadBar_defineProperties(Constructor.prototype, protoProps); if (staticProps) UploadBar_defineProperties(Constructor, staticProps); return Constructor; }

function UploadBar_possibleConstructorReturn(self, call) { if (call && (UploadBar_typeof(call) === "object" || typeof call === "function")) { return call; } return UploadBar_assertThisInitialized(self); }

function UploadBar_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function UploadBar_getPrototypeOf(o) { UploadBar_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return UploadBar_getPrototypeOf(o); }

function UploadBar_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) UploadBar_setPrototypeOf(subClass, superClass); }

function UploadBar_setPrototypeOf(o, p) { UploadBar_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return UploadBar_setPrototypeOf(o, p); }




var upload = __webpack_require__(16).default;

var checkmark = __webpack_require__(97).default;

var errorIcon = __webpack_require__(98).default;

var UploadBar_UploadBar =
/*#__PURE__*/
function (_Component) {
  UploadBar_inherits(UploadBar, _Component);

  function UploadBar() {
    var _this;

    UploadBar_classCallCheck(this, UploadBar);

    _this = UploadBar_possibleConstructorReturn(this, UploadBar_getPrototypeOf(UploadBar).call(this));
    _this.state = {
      percent: 0
    };
    _this.timeout = 20;
    _this.delta = 1.00;
    _this.setTimeout = null;
    return _this;
  }

  UploadBar_createClass(UploadBar, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      setTimeout(this.increase.bind(this), 100);
      this._isMounted = true;
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      this._isMounted = false;
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      if (props.uploadComplete) {
        clearTimeout(this.setTimeout);

        if (this._isMounted) {
          this.setState({
            percent: 100
          }, function () {});
        }
      }
    }
  }, {
    key: "increase",
    value: function increase() {
      var percent = this.state.percent;
      percent++;
      this.timeout *= this.delta;
      this.delta *= 1.00;

      if (percent <= 100) {
        if (this._isMounted) {
          this.setState({
            percent: percent
          });
        }
      }

      if (percent < 95) {
        this.setTimeout = setTimeout(this.increase.bind(this), this.timeout);
      }
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      /* eslint-disable react/no-danger */

      var percent = props.errorText ? 0 : this.state.percent;
      var text = props.uploadComplete ? props.uploadCompleteText : props.uploadingText;
      text = props.errorText ? props.errorText : text;
      var svg = props.uploadComplete ? checkmark : upload;
      svg = props.errorText ? errorIcon : svg;
      var className = "file-upload-container dnn-upload-bar" + (props.uploadComplete ? " complete" : "") + (props.errorText ? " upload-error" : "");
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: className
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "upload-bar-container"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "upload-file-name"
        }, this.props.fileName || "myImage.jpg"),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "upload-icon",
          dangerouslySetInnerHTML: {
            __html: svg
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h4", null, text), props.errorInPackage &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          className: "view-log-or-try-again"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          onClick: props.onViewLog.bind(this)
        }, props.viewLogText, " "), "or",
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          onClick: props.onTryAgain.bind(this)
        }, props.tryAgainText)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "upload-percent"
        }, percent + "%"),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "upload-bar-box"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "upload-bar",
          style: {
            width: percent + "%"
          }
        }))))
      );
    }
  }]);

  return UploadBar;
}(external_window_dnn_nodeModules_React_["Component"]);


UploadBar_UploadBar.propTypes = {
  errorText: prop_types_default.a.string.isRequired,
  fileName: prop_types_default.a.string.isRequired,
  uploadComplete: prop_types_default.a.bool.isRequired,
  uploadCompleteText: prop_types_default.a.string,
  uploadingText: prop_types_default.a.string,
  errorInPackage: prop_types_default.a.bool
};
UploadBar_UploadBar.defaultProps = {
  uploadCompleteText: "Upload Complete",
  uploadingText: "Uploading..."
};
// CONCATENATED MODULE: ./src/components/installExtensionModal/FileUpload/AlreadyInstalled.jsx
function AlreadyInstalled_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { AlreadyInstalled_typeof = function _typeof(obj) { return typeof obj; }; } else { AlreadyInstalled_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return AlreadyInstalled_typeof(obj); }

function AlreadyInstalled_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function AlreadyInstalled_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function AlreadyInstalled_createClass(Constructor, protoProps, staticProps) { if (protoProps) AlreadyInstalled_defineProperties(Constructor.prototype, protoProps); if (staticProps) AlreadyInstalled_defineProperties(Constructor, staticProps); return Constructor; }

function AlreadyInstalled_possibleConstructorReturn(self, call) { if (call && (AlreadyInstalled_typeof(call) === "object" || typeof call === "function")) { return call; } return AlreadyInstalled_assertThisInitialized(self); }

function AlreadyInstalled_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function AlreadyInstalled_getPrototypeOf(o) { AlreadyInstalled_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return AlreadyInstalled_getPrototypeOf(o); }

function AlreadyInstalled_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) AlreadyInstalled_setPrototypeOf(subClass, superClass); }

function AlreadyInstalled_setPrototypeOf(o, p) { AlreadyInstalled_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return AlreadyInstalled_setPrototypeOf(o, p); }





var AlreadyInstalled_AlreadyInstalled =
/*#__PURE__*/
function (_Component) {
  AlreadyInstalled_inherits(AlreadyInstalled, _Component);

  function AlreadyInstalled() {
    AlreadyInstalled_classCallCheck(this, AlreadyInstalled);

    return AlreadyInstalled_possibleConstructorReturn(this, AlreadyInstalled_getPrototypeOf(AlreadyInstalled).call(this));
  }

  AlreadyInstalled_createClass(AlreadyInstalled, [{
    key: "render",
    value: function render() {
      var props = this.props;
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "already-installed"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "already-installed-container"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "upload-file-name"
        }, this.props.fileName || "undefined"),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "upload-icon",
          dangerouslySetInnerHTML: {
            __html: external_window_dnn_nodeModules_CommonComponents_["SvgIcons"].ErrorStateIcon
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h4", {
          dangerouslySetInnerHTML: {
            __html: props.repairWarning
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          className: "repair-or-install"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          onClick: props.repairInstall.bind(this)
        }, "[", props.repairInstallText, "] "), props.orText,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", {
          onClick: props.cancelRepair.bind(this)
        }, " [", props.cancelInstallText, "]"))))
      );
    }
  }]);

  return AlreadyInstalled;
}(external_window_dnn_nodeModules_React_["Component"]);


AlreadyInstalled_AlreadyInstalled.propTypes = {
  errorText: prop_types_default.a.string,
  fileName: prop_types_default.a.string.isRequired,
  repairInstall: prop_types_default.a.func,
  cancelRepair: prop_types_default.a.func,
  repairWarning: prop_types_default.a.string,
  repairInstallText: prop_types_default.a.string,
  cancelInstallText: prop_types_default.a.string,
  orText: prop_types_default.a.string
};
AlreadyInstalled_AlreadyInstalled.defaultProps = {
  repairInstallText: "Repair Install",
  cancelInstallText: "Cancel",
  orText: "or",
  repairWarning: "Warning: You have selected to repair the installation of this package." + "<br/> This will cause the files in the package to overwrite all files that were previously installed."
};
// EXTERNAL MODULE: ./src/components/installExtensionModal/FileUpload/style.less
var FileUpload_style = __webpack_require__(17);

// CONCATENATED MODULE: ./src/components/installExtensionModal/FileUpload/LogDisplay.jsx
function LogDisplay_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { LogDisplay_typeof = function _typeof(obj) { return typeof obj; }; } else { LogDisplay_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return LogDisplay_typeof(obj); }

function LogDisplay_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function LogDisplay_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function LogDisplay_createClass(Constructor, protoProps, staticProps) { if (protoProps) LogDisplay_defineProperties(Constructor.prototype, protoProps); if (staticProps) LogDisplay_defineProperties(Constructor, staticProps); return Constructor; }

function LogDisplay_possibleConstructorReturn(self, call) { if (call && (LogDisplay_typeof(call) === "object" || typeof call === "function")) { return call; } return LogDisplay_assertThisInitialized(self); }

function LogDisplay_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function LogDisplay_getPrototypeOf(o) { LogDisplay_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return LogDisplay_getPrototypeOf(o); }

function LogDisplay_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) LogDisplay_setPrototypeOf(subClass, superClass); }

function LogDisplay_setPrototypeOf(o, p) { LogDisplay_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return LogDisplay_setPrototypeOf(o, p); }







var LogDisplay_licenseBoxStyle = {
  border: "1px solid #c8c8c8",
  borderBottom: "2px solid #EA2134",
  marginBottom: 25,
  height: 175,
  width: "100%"
};

var LogDisplay_LogDisplay =
/*#__PURE__*/
function (_Component) {
  LogDisplay_inherits(LogDisplay, _Component);

  function LogDisplay() {
    LogDisplay_classCallCheck(this, LogDisplay);

    return LogDisplay_possibleConstructorReturn(this, LogDisplay_getPrototypeOf(LogDisplay).apply(this, arguments));
  }

  LogDisplay_createClass(LogDisplay, [{
    key: "render",
    value: function render() {
      var props = this.props;
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          style: {
            padding: 0
          },
          className: "install-failure-logs"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCustomScrollBars_["Scrollbars"], {
          style: LogDisplay_licenseBoxStyle
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "package-installation-report"
        }, props.logs && props.logs.map(function (log, i) {
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("p", {
              className: log.Type.toLowerCase(),
              key: i
            }, log.Type + " " + log.Description)
          );
        }), !props.logs &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          className: "logs-unknown-error",
          dangerouslySetInnerHTML: {
            __html: localization.get("InstallExtension_UploadFailedUnknownLogs")
          }
        }))))
      );
    }
  }]);

  return LogDisplay;
}(external_window_dnn_nodeModules_React_["Component"]);

LogDisplay_LogDisplay.propTypes = {
  onDone: prop_types_default.a.func,
  logs: prop_types_default.a.array
};
/* harmony default export */ var FileUpload_LogDisplay = (LogDisplay_LogDisplay);
// CONCATENATED MODULE: ./src/components/installExtensionModal/FileUpload/index.jsx
function FileUpload_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { FileUpload_typeof = function _typeof(obj) { return typeof obj; }; } else { FileUpload_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return FileUpload_typeof(obj); }

function FileUpload_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function FileUpload_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function FileUpload_createClass(Constructor, protoProps, staticProps) { if (protoProps) FileUpload_defineProperties(Constructor.prototype, protoProps); if (staticProps) FileUpload_defineProperties(Constructor, staticProps); return Constructor; }

function FileUpload_possibleConstructorReturn(self, call) { if (call && (FileUpload_typeof(call) === "object" || typeof call === "function")) { return call; } return FileUpload_assertThisInitialized(self); }

function FileUpload_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function FileUpload_getPrototypeOf(o) { FileUpload_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return FileUpload_getPrototypeOf(o); }

function FileUpload_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) FileUpload_setPrototypeOf(subClass, superClass); }

function FileUpload_setPrototypeOf(o, p) { FileUpload_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return FileUpload_setPrototypeOf(o, p); }










var FileUpload_FileUpload =
/*#__PURE__*/
function (_Component) {
  FileUpload_inherits(FileUpload, _Component);

  function FileUpload(props) {
    var _this;

    FileUpload_classCallCheck(this, FileUpload);

    _this = FileUpload_possibleConstructorReturn(this, FileUpload_getPrototypeOf(FileUpload).call(this, props));
    _this.state = {
      text: localization.get("InstallExtension_FileUploadDefault"),
      draggedOver: false,
      isDragging: false,
      fileName: "",
      uploading: false,
      uploadComplete: false,
      errorText: "",
      errorInPackage: false,
      noManifest: false
    };
    return _this;
  }

  FileUpload_createClass(FileUpload, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      window.addEventListener("dragover", this.prevent);
      window.addEventListener("drop", this.prevent);
    }
  }, {
    key: "prevent",
    value: function prevent(e) {
      e.preventDefault();
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      window.removeEventListener("dragover", this.prevent);
      window.removeEventListener("drop", this.prevent);
    }
  }, {
    key: "onMouseEnter",
    value: function onMouseEnter(text) {
      this.setState({
        text: text
      });
    }
  }, {
    key: "onMouseLeave",
    value: function onMouseLeave() {
      this.setState({
        text: localization.get("InstallExtension_FileUploadDefault")
      });
    }
  }, {
    key: "onFileUpload",
    value: function onFileUpload(e) {
      this.uploadFile(e.target.files[0]);
    }
  }, {
    key: "getErrorCount",
    value: function getErrorCount(_package) {
      var count = 0;

      _package.forEach(function (key) {
        if (key.Type === "Failure") {
          count++;
        }
      });

      return count;
    }
  }, {
    key: "handleError",
    value: function handleError(error) {
      if (error && error.logs && !error.noManifest) {
        var errorCount = this.getErrorCount(error.logs);
        var errorText = typeof error === "string" ? error : errorCount > 0 ? localization.get("InstallExtension_UploadFailed") + errorCount + " " + localization.get("Errors") : localization.get("InstallExtension_UploadFailedUnknown");
        this.setState({
          uploading: true,
          errorText: errorText,
          errorInPackage: true,
          noManifest: false
        }, function () {});
      } else {
        var _errorText = localization.get("InstallExtension_UploadFailedUnknown");

        this.setState({
          uploading: true,
          errorText: _errorText,
          errorInPackage: true,
          noManifest: false
        });
      }
    }
  }, {
    key: "uploadFile",
    value: function uploadFile(file) {
      this.setState({
        fileName: file.name,
        errorInPackage: false
      });
      this.postFile(file);
    }
  }, {
    key: "postFile",
    value: function postFile(file) {
      var props = this.props;
      props.parsePackage(file, this.uploadComplete.bind(this), this.handleError.bind(this));
      this.setState({
        uploading: true,
        uploadComplete: false
      });
    }
  }, {
    key: "cancelRepair",
    value: function cancelRepair() {
      this.props.clearParsedInstallationPackage();
      this.setState({
        uploading: false,
        errorInPackage: false,
        errorText: "",
        noManifest: false
      });
    }
  }, {
    key: "repairInstall",
    value: function repairInstall() {
      var props = this.props;
      props.repairInstall();
    }
  }, {
    key: "uploadComplete",
    value: function uploadComplete(data) {
      var _this2 = this;

      if (data.noManifest && !this.props.alreadyInstalled) {
        this.setState({
          uploading: false,
          noManifest: true
        });
      } else {
        setTimeout(function () {
          _this2.setState({
            uploadComplete: true
          }, function () {
            if (data.alreadyInstalled) {
              _this2.setState({
                uploading: false,
                noManifest: false
              });
            }
          });
        }, 1000);
      }
    }
  }, {
    key: "onViewLog",
    value: function onViewLog() {
      this.props.toggleViewLog(true);
    }
  }, {
    key: "onDragOver",
    value: function onDragOver() {
      this.setState({
        draggedOver: true,
        text: localization.get("InstallExtension_FileUploadDragOver")
      });
    }
  }, {
    key: "onDragLeave",
    value: function onDragLeave() {
      this.setState({
        draggedOver: false,
        text: localization.get("InstallExtension_FileUploadDefault")
      });
    }
  }, {
    key: "onDrop",
    value: function onDrop(e) {
      e.preventDefault();
      this.uploadFile(e.dataTransfer.files[0]);
      this.onDragLeave();
    }
  }, {
    key: "render",
    value: function render() {
      /* eslint-disable quotes */
      var svg = __webpack_require__(16).default;

      var buttonsStyle = {
        width: 67
      };
      var className = "overlay" + (this.state.draggedOver ? " hover" : "");
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "dnn-package-upload" + (this.state.uploading ? " uploading" : "") + (this.props.alreadyInstalled ? " already-installed" : "") + (this.props.viewingLog ? " viewing-log" : "")
        }, (!this.state.uploading || this.state.uploadComplete) && !this.props.viewingLog &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "dropzone-container"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          id: "dropzoneId",
          className: className,
          onDragOver: this.onDragOver.bind(this),
          onDragLeave: this.onDragLeave.bind(this),
          onDrop: this.onDrop.bind(this)
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "buttons",
          style: buttonsStyle
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "button upload",
          onMouseEnter: this.onMouseEnter.bind(this, localization.get("InstallExtension_UploadAFile")),
          onMouseLeave: this.onMouseLeave.bind(this)
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          dangerouslySetInnerHTML: {
            __html: svg
          }
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("input", {
          type: "file",
          onChange: this.onFileUpload.bind(this),
          "aria-label": "File"
        }))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("span", null, this.state.text))), this.props.viewingLog &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(FileUpload_LogDisplay, {
          logs: this.props.parsedInstallationPackage && this.props.parsedInstallationPackage.logs
        }), this.state.uploading && !this.props.viewingLog &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(UploadBar_UploadBar, {
          uploadComplete: this.state.uploadComplete,
          errorText: this.state.errorText,
          errorInPackage: this.state.errorInPackage,
          fileName: this.state.fileName,
          onViewLog: this.onViewLog.bind(this),
          onTryAgain: this.cancelRepair.bind(this),
          viewLogText: localization.get("ViewErrorLog"),
          tryAgainText: localization.get("TryAgain"),
          uploadCompleteText: localization.get("InstallExtension_UploadComplete"),
          uploadingText: localization.get("InstallExtension_Uploading")
        }), this.state.noManifest &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "no-valid-manifest",
          style: {
            width: "calc(100% + 26px)",
            height: "calc(100% + 26px)"
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", null, localization.get("InvalidDNNManifest")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["RadioButtons"], {
          options: [{
            label: localization.get("CatalogSkin"),
            value: "Skin"
          }, {
            label: localization.get("Container"),
            value: "Container"
          }],
          buttonGroup: "selectedLegacyType",
          onChange: this.props.onSelectLegacyType.bind(this),
          value: this.props.selectedLegacyType
        })), this.props.alreadyInstalled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(AlreadyInstalled_AlreadyInstalled, {
          fileName: this.state.fileName,
          cancelRepair: this.cancelRepair.bind(this),
          repairInstall: this.repairInstall.bind(this)
        }))
      );
    }
  }]);

  return FileUpload;
}(external_window_dnn_nodeModules_React_["Component"]);


FileUpload_FileUpload.propTypes = {
  cancelInstall: prop_types_default.a.func,
  parsedInstallationPackage: prop_types_default.a.object,
  viewingLog: prop_types_default.a.bool,
  toggleViewLog: prop_types_default.a.func,
  clearParsedInstallationPackage: prop_types_default.a.func,
  onSelectLegacyType: prop_types_default.a.func,
  selectedLegacyType: prop_types_default.a.string,
  alreadyInstalled: prop_types_default.a.bool,
  //---OPTIONAL PROPS---
  buttons: prop_types_default.a.array
};
// EXTERNAL MODULE: ./src/components/installExtensionModal/style.less
var installExtensionModal_style = __webpack_require__(46);
var installExtensionModal_style_default = /*#__PURE__*/__webpack_require__.n(installExtensionModal_style);

// CONCATENATED MODULE: ./src/components/installExtensionModal/index.jsx
function installExtensionModal_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { installExtensionModal_typeof = function _typeof(obj) { return typeof obj; }; } else { installExtensionModal_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return installExtensionModal_typeof(obj); }

function installExtensionModal_extends() { installExtensionModal_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return installExtensionModal_extends.apply(this, arguments); }

function installExtensionModal_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function installExtensionModal_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function installExtensionModal_createClass(Constructor, protoProps, staticProps) { if (protoProps) installExtensionModal_defineProperties(Constructor.prototype, protoProps); if (staticProps) installExtensionModal_defineProperties(Constructor, staticProps); return Constructor; }

function installExtensionModal_possibleConstructorReturn(self, call) { if (call && (installExtensionModal_typeof(call) === "object" || typeof call === "function")) { return call; } return installExtensionModal_assertThisInitialized(self); }

function installExtensionModal_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function installExtensionModal_getPrototypeOf(o) { installExtensionModal_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return installExtensionModal_getPrototypeOf(o); }

function installExtensionModal_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) installExtensionModal_setPrototypeOf(subClass, superClass); }

function installExtensionModal_setPrototypeOf(o, p) { installExtensionModal_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return installExtensionModal_setPrototypeOf(o, p); }















var installExtensionModal_InstallExtensionModal =
/*#__PURE__*/
function (_Component) {
  installExtensionModal_inherits(InstallExtensionModal, _Component);

  function InstallExtensionModal(props) {
    var _this;

    installExtensionModal_classCallCheck(this, InstallExtensionModal);

    _this = installExtensionModal_possibleConstructorReturn(this, installExtensionModal_getPrototypeOf(InstallExtensionModal).call(this));
    _this.state = {
      package: null,
      wizardStep: 0,
      repairInstallChecked: false,
      selectedLegacyType: null,
      isPortalPackage: props.isPortalPackage
    };
    return _this;
  }

  installExtensionModal_createClass(InstallExtensionModal, [{
    key: "onPackageChange",
    value: function onPackageChange(event) {
      var files = event.target.files;

      if (files && files.length > 0) {
        this.setState({
          package: files[0]
        });
      }
    }
  }, {
    key: "goToStep",
    value: function goToStep(wizardStep) {
      var props = this.props;
      props.dispatch(installation.navigateWizard(wizardStep));
    }
  }, {
    key: "parsePackage",
    value: function parsePackage(file, callback, errorCallback) {
      var _this2 = this;

      if (!file) {
        utils["a" /* default */].utilities.notifyError(localization.get("InstallExtension_EmptyPackage.Error"));
        return;
      }

      var props = this.props;
      this.setState({
        package: file
      }, function () {
        props.dispatch(installation.parsePackage(file, function (data) {
          data = JSON.parse(data);

          if (!data.success) {
            if (errorCallback) {
              errorCallback(data);
            }
          }

          if (data.noManifest) {
            _this2.setState({
              selectedLegacyType: "Skin"
            });
          }

          if (data.alreadyInstalled) {
            _this2.setAlreadyInstalled(true);
          }

          if (callback) {
            callback(data);
          }
        }, function (data) {
          if (errorCallback) {
            errorCallback(data);
          }
        }));
      });
    }
  }, {
    key: "goToReleaseNotes",
    value: function goToReleaseNotes() {
      this.goToStep(2);
    }
  }, {
    key: "goToLicense",
    value: function goToLicense() {
      this.goToStep(3);
    }
  }, {
    key: "installPackage",
    value: function installPackage() {
      var _this3 = this;

      var props = this.props;
      this.setState({
        installingPackage: true
      });

      if (!props.installingAvailablePackage) {
        props.dispatch(installation.installExtension(this.state.package, props.parsedInstallationPackage, this.state.selectedLegacyType, this.state.isPortalPackage, function () {
          _this3.goToStep(4);

          _this3.setState({
            installingPackage: false
          });
        }, !props.parsedInstallationPackage.alreadyInstalled));
      } else {
        props.dispatch(extension.installAvailablePackage(props.availablePackage.PackageType, props.availablePackage.FileName, props.parsedInstallationPackage, function () {
          _this3.setState({
            installingPackage: false
          });

          _this3.goToStep(4);
        }));
      }
    }
  }, {
    key: "onCheckRepairInstall",
    value: function onCheckRepairInstall(value) {
      this.setState({
        repairInstallChecked: value
      });
    }
  }, {
    key: "cancelInstall",
    value: function cancelInstall() {
      var props = this.props;
      props.dispatch(installation.clearParsedInstallationPackage());
      props.dispatch(installation.notInstallingAvailablePackage());
      props.dispatch(installation.toggleAcceptLicense(false));
      this.goToStep(0);
      props.onCancel();

      if (props.backToReferrerFunc) {
        props.backToReferrerFunc();
      }
    }
  }, {
    key: "getResxFromLegacyType",
    value: function getResxFromLegacyType() {
      if (this.state.selectedLegacyType === "Skin") {
        return localization.get("CatalogSkin");
      } else {
        return localization.get("Container");
      }
    }
  }, {
    key: "getPackageInformationStep",
    value: function getPackageInformationStep() {
      var props = this.props;
      var parsedInstallationPackageCopy = utils["a" /* default */].utilities.getObjectCopy(props.parsedInstallationPackage);
      var parsedInstallationPackage = this.state.selectedLegacyType ? installExtensionModal_extends(parsedInstallationPackageCopy, {
        packageType: this.getResxFromLegacyType(),
        friendlyName: this.state.package && this.state.package.name.replace(".zip", ""),
        name: this.state.package && this.state.package.name.replace(".zip", ""),
        version: "1.0.0"
      }) : parsedInstallationPackageCopy;

      if (parsedInstallationPackage) {
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(EditExtension_PackageInformation, {
            extensionBeingEdited: parsedInstallationPackage,
            validationMapped: false,
            onCancel: this.cancelInstall.bind(this),
            installationMode: true,
            onSave: this.goToReleaseNotes.bind(this),
            primaryButtonText: localization.get("Next"),
            disabled: true
          })
        );
      }
    }
  }, {
    key: "endInstallation",
    value: function endInstallation() {
      var props = this.props;

      if (props.installingAvailablePackage) {
        props.dispatch(pagination.loadTab(0, function () {
          var _packageType = props.availablePackage && props.availablePackage.PackageType ? props.availablePackage.PackageType : "Module";

          props.dispatch(extension.getInstalledPackages(_packageType));
          props.dispatch(extension.getAvailablePackages(_packageType));
        }));
      } else {
        if (props.tabIndex !== 0) {
          props.dispatch(pagination.loadTab(0));
        }

        if (props.parsedInstallationPackage.packageType && props.parsedInstallationPackage.packageType !== props.selectedInstalledPackageType) {
          var _packageType = props.parsedInstallationPackage && props.parsedInstallationPackage.packageType ? props.parsedInstallationPackage.packageType : "Module";

          props.dispatch(extension.getInstalledPackages(_packageType));
        } else if (this.state.selectedLegacyType) {
          props.dispatch(extension.getInstalledPackages(this.state.selectedLegacyType));
        } else if (props.selectedInstalledPackageType) {
          props.dispatch(extension.getInstalledPackages(props.selectedInstalledPackageType));
        }
      }

      this.cancelInstall();
    }
  }, {
    key: "onToggleLicenseAccept",
    value: function onToggleLicenseAccept() {
      this.props.dispatch(installation.toggleAcceptLicense(!this.props.licenseAccepted));
    }
  }, {
    key: "toggleViewLog",
    value: function toggleViewLog(value) {
      this.props.dispatch(installation.setViewingLog(value));
    }
  }, {
    key: "clearParsedInstallationPackage",
    value: function clearParsedInstallationPackage() {
      this.setAlreadyInstalled(false);
      this.props.dispatch(installation.clearParsedInstallationPackage());
    }
  }, {
    key: "cancelInstallationOnUpload",
    value: function cancelInstallationOnUpload() {
      this.clearParsedInstallationPackage();
      this.props.onCancel();
      this.props.dispatch(installation.toggleAcceptLicense(false));

      if (this.props.backToReferrerFunc) {
        this.props.backToReferrerFunc();
      }
    }
  }, {
    key: "onSelectLegacyType",
    value: function onSelectLegacyType(value) {
      this.setState({
        selectedLegacyType: value
      });
    }
  }, {
    key: "setAlreadyInstalled",
    value: function setAlreadyInstalled(value, nextStepIfFalse) {
      this.setState({
        alreadyInstalled: value
      });

      if (nextStepIfFalse && !value) {
        this.goToStep(1);
      }
    }
  }, {
    key: "getSelectedLegacyTypeIsInstalled",
    value: function getSelectedLegacyTypeIsInstalled() {
      if (this.state.selectedLegacyType === "Skin") {
        return this.props.parsedInstallationPackage && this.props.parsedInstallationPackage.legacySkinInstalled;
      }

      if (this.state.selectedLegacyType === "Container") {
        return this.props.parsedInstallationPackage && this.props.parsedInstallationPackage.legacyContainerInstalled;
      }
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var wizardStep = props.wizardStep,
          legacyInstalled = props.parsedInstallationPackage && (props.parsedInstallationPackage.legacySkinInstalled || props.parsedInstallationPackage.legacyContainerInstalled),
          legacyTypeIsInstalled = this.getSelectedLegacyTypeIsInstalled();
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: installExtensionModal_style_default.a.installExtensionModal
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: localization.get("ExtensionInstall.Action")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], {
          backToLinkProps: {
            text: props.backToReferrer && props.backToReferrerText ? props.backToReferrerText : localization.get("BackToExtensions"),
            onClick: this.cancelInstall.bind(this)
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "install-extension-box extension-form"
        }, wizardStep === 0 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], null,
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("h3", {
          className: "box-title"
        }, localization.get("InstallExtension_UploadPackage.Header")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", null, localization.get("InstallExtension_UploadPackage.HelpText")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "upload-package-box"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(FileUpload_FileUpload, {
          parsePackage: this.parsePackage.bind(this),
          repairInstall: this.goToStep.bind(this, 1),
          cancelInstall: this.cancelInstall.bind(this),
          parsedInstallationPackage: props.parsedInstallationPackage,
          alreadyInstalled: this.state.alreadyInstalled,
          toggleViewLog: this.toggleViewLog.bind(this),
          clearParsedInstallationPackage: this.clearParsedInstallationPackage.bind(this),
          viewingLog: props.viewingLog,
          onSelectLegacyType: this.onSelectLegacyType.bind(this),
          selectedLegacyType: this.state.selectedLegacyType
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          onClick: !props.viewingLog ? this.cancelInstallationOnUpload.bind(this) : this.toggleViewLog.bind(this, false)
        }, localization.get("InstallExtension_Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          onClick: legacyInstalled ? this.setAlreadyInstalled.bind(this, legacyTypeIsInstalled, true) : this.goToStep.bind(this, 1),
          type: "primary",
          disabled: !props.parsedInstallationPackage || !props.parsedInstallationPackage.success
        }, localization.get("InstallExtension_Upload.Button")))), wizardStep === 1 && this.getPackageInformationStep(), wizardStep === 2 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(EditExtension_ReleaseNotes, {
          value: props.parsedInstallationPackage.releaseNotes,
          onCancel: this.cancelInstall.bind(this),
          onSave: this.goToLicense.bind(this),
          primaryButtonText: localization.get("Next.Button"),
          installationMode: true,
          readOnly: true,
          disabled: true
        }), wizardStep === 3 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(EditExtension_License, {
          value: props.parsedInstallationPackage.license,
          onCancel: this.cancelInstall.bind(this),
          installationMode: true,
          readOnly: true,
          onSave: this.installPackage.bind(this),
          primaryButtonText: localization.get("Next.Button"),
          disabled: true,
          primaryButtonDisabled: !props.licenseAccepted || this.state.installingPackage,
          licenseAccepted: props.licenseAccepted,
          onToggleLicenseAccept: this.onToggleLicenseAccept.bind(this)
        }), wizardStep === 4 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(InstallLog, {
          logs: props.installationLogs,
          onDone: this.endInstallation.bind(this),
          primaryButtonText: localization.get("Next.Button")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          className: "modal-pagination"
        }, "-- " + (props.wizardStep + 1) + " of 5 --", " "))))
      );
    }
  }]);

  return InstallExtensionModal;
}(external_window_dnn_nodeModules_React_["Component"]);

installExtensionModal_InstallExtensionModal.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  onCancel: prop_types_default.a.func,
  parsedInstallationPackage: prop_types_default.a.object,
  selectedInstalledPackageType: prop_types_default.a.string,
  wizardStep: prop_types_default.a.number,
  installationLogs: prop_types_default.a.array,
  installingAvailablePackage: prop_types_default.a.bool,
  availablePackage: prop_types_default.a.object,
  licenseAccepted: prop_types_default.a.bool,
  viewingLog: prop_types_default.a.bool,
  isPortalPackage: prop_types_default.a.bool,
  backToReferrer: prop_types_default.a.string,
  backToReferrerText: prop_types_default.a.string,
  backToReferrerFunc: prop_types_default.a.func
};

function installExtensionModal_mapStateToProps(state) {
  return {
    parsedInstallationPackage: state.installation.parsedInstallationPackage,
    selectedInstalledPackageType: state.extension.selectedInstalledPackageType,
    wizardStep: state.installation.installWizardStep,
    installationLogs: state.installation.installationLogs,
    installingAvailablePackage: state.installation.installingAvailablePackage,
    availablePackage: state.installation.availablePackage,
    licenseAccepted: state.installation.licenseAccepted,
    viewingLog: state.installation.viewingLog,
    isPortalPackage: state.installation.isPortalPackage
  };
}

/* harmony default export */ var installExtensionModal = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(installExtensionModal_mapStateToProps)(installExtensionModal_InstallExtensionModal));
// EXTERNAL MODULE: ./src/components/EditExtension/EditSettings/AuthenticationSystemSettings/style.less
var AuthenticationSystemSettings_style = __webpack_require__(47);
var AuthenticationSystemSettings_style_default = /*#__PURE__*/__webpack_require__.n(AuthenticationSystemSettings_style);

// CONCATENATED MODULE: ./src/components/EditExtension/EditSettings/AuthenticationSystemSettings/index.jsx
function AuthenticationSystemSettings_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { AuthenticationSystemSettings_typeof = function _typeof(obj) { return typeof obj; }; } else { AuthenticationSystemSettings_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return AuthenticationSystemSettings_typeof(obj); }

function AuthenticationSystemSettings_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function AuthenticationSystemSettings_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function AuthenticationSystemSettings_createClass(Constructor, protoProps, staticProps) { if (protoProps) AuthenticationSystemSettings_defineProperties(Constructor.prototype, protoProps); if (staticProps) AuthenticationSystemSettings_defineProperties(Constructor, staticProps); return Constructor; }

function AuthenticationSystemSettings_possibleConstructorReturn(self, call) { if (call && (AuthenticationSystemSettings_typeof(call) === "object" || typeof call === "function")) { return call; } return AuthenticationSystemSettings_assertThisInitialized(self); }

function AuthenticationSystemSettings_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function AuthenticationSystemSettings_getPrototypeOf(o) { AuthenticationSystemSettings_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return AuthenticationSystemSettings_getPrototypeOf(o); }

function AuthenticationSystemSettings_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) AuthenticationSystemSettings_setPrototypeOf(subClass, superClass); }

function AuthenticationSystemSettings_setPrototypeOf(o, p) { AuthenticationSystemSettings_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return AuthenticationSystemSettings_setPrototypeOf(o, p); }







var AuthenticationSystemSettings_inputStyle = {
  width: "100%"
};

var AuthenticationSystemSettings_AuthenticationSystemSettings =
/*#__PURE__*/
function (_Component) {
  AuthenticationSystemSettings_inherits(AuthenticationSystemSettings, _Component);

  function AuthenticationSystemSettings() {
    AuthenticationSystemSettings_classCallCheck(this, AuthenticationSystemSettings);

    return AuthenticationSystemSettings_possibleConstructorReturn(this, AuthenticationSystemSettings_getPrototypeOf(AuthenticationSystemSettings).apply(this, arguments));
  }

  AuthenticationSystemSettings_createClass(AuthenticationSystemSettings, [{
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: AuthenticationSystemSettings_style_default.a.editAuthenticationSystem + " extension-form"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "auth-system-site-settings"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("AuthSystemSiteSettings_AppId.Label"),
          value: extensionBeingEdited.appId.value,
          onChange: props.onChange.bind(this, "appId"),
          tooltipMessage: localization.get("AuthSystemSiteSettings_AppId.HelpText"),
          style: AuthenticationSystemSettings_inputStyle
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
          label: localization.get("AuthSystemSiteSettings_AppSecret.Label"),
          value: extensionBeingEdited.appSecret.value,
          onChange: props.onChange.bind(this, "appSecret"),
          tooltipMessage: localization.get("AuthSystemSiteSettings_AppSecret.HelpText"),
          style: AuthenticationSystemSettings_inputStyle
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 50,
          style: {
            padding: 0
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
          label: localization.get("AuthSystemSiteSettings_AppEnabled.Label"),
          onText: localization.get("SwitchOn"),
          offText: localization.get("SwitchOff"),
          value: extensionBeingEdited.appEnabled.value,
          onChange: props.onChange.bind(this, "appEnabled"),
          tooltipMessage: localization.get("AuthSystemSiteSettings_AppEnabled.HelpText")
        }))), !props.actionButtonsDisabled &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          columnSize: 100,
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: props.onSave.bind(this)
        }, localization.get("Save"))))
      );
    }
  }]);

  return AuthenticationSystemSettings;
}(external_window_dnn_nodeModules_React_["Component"]);

AuthenticationSystemSettings_AuthenticationSystemSettings.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  onChange: prop_types_default.a.func,
  extensionBeingEdited: prop_types_default.a.object,
  extensionBeingEditedIndex: prop_types_default.a.object
};

function AuthenticationSystemSettings_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex
  };
}

/* harmony default export */ var EditSettings_AuthenticationSystemSettings = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(AuthenticationSystemSettings_mapStateToProps)(AuthenticationSystemSettings_AuthenticationSystemSettings));
// EXTERNAL MODULE: ./src/components/EditExtension/EditSettings/ModuleSettings/style.less
var ModuleSettings_style = __webpack_require__(102);

// CONCATENATED MODULE: ./src/components/EditExtension/EditSettings/ModuleSettings/index.jsx
function ModuleSettings_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { ModuleSettings_typeof = function _typeof(obj) { return typeof obj; }; } else { ModuleSettings_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return ModuleSettings_typeof(obj); }

function ModuleSettings_extends() { ModuleSettings_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return ModuleSettings_extends.apply(this, arguments); }

function ModuleSettings_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function ModuleSettings_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function ModuleSettings_createClass(Constructor, protoProps, staticProps) { if (protoProps) ModuleSettings_defineProperties(Constructor.prototype, protoProps); if (staticProps) ModuleSettings_defineProperties(Constructor, staticProps); return Constructor; }

function ModuleSettings_possibleConstructorReturn(self, call) { if (call && (ModuleSettings_typeof(call) === "object" || typeof call === "function")) { return call; } return ModuleSettings_assertThisInitialized(self); }

function ModuleSettings_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function ModuleSettings_getPrototypeOf(o) { ModuleSettings_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return ModuleSettings_getPrototypeOf(o); }

function ModuleSettings_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) ModuleSettings_setPrototypeOf(subClass, superClass); }

function ModuleSettings_setPrototypeOf(o, p) { ModuleSettings_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return ModuleSettings_setPrototypeOf(o, p); }










var ModuleSettings_ModuleSettings =
/*#__PURE__*/
function (_Component) {
  ModuleSettings_inherits(ModuleSettings, _Component);

  function ModuleSettings(props) {
    var _this;

    ModuleSettings_classCallCheck(this, ModuleSettings);

    _this = ModuleSettings_possibleConstructorReturn(this, ModuleSettings_getPrototypeOf(ModuleSettings).call(this, props));
    _this.state = {
      permissions: JSON.parse(JSON.stringify(props.extensionBeingEdited.permissions.value)),
      desktopModuleId: props.extensionBeingEdited.desktopModuleId.value
    };
    return _this;
  }

  ModuleSettings_createClass(ModuleSettings, [{
    key: "onPermissionsChanged",
    value: function onPermissionsChanged(permissions) {
      var state = this.state;

      var newPermissions = ModuleSettings_extends({}, state.permissions, permissions);

      this.setState({
        permissions: newPermissions
      });
    }
  }, {
    key: "savePermissions",
    value: function savePermissions(closeOnSave) {
      var props = this.props,
          state = this.state;

      var permissions = ModuleSettings_extends({}, state.permissions);

      var extensionBeingUpdated = JSON.parse(JSON.stringify(props.extensionBeingEdited));
      extensionBeingUpdated.permissions.value = permissions;
      var actions = {
        permissions: JSON.stringify(permissions)
      };
      props.updateExtensionBeingEdited(extensionBeingUpdated);
      props.dispatch(extension.updateExtension(extensionBeingUpdated, actions, props.extensionBeingEditedIndex, function () {
        utils["a" /* default */].utilities.notify(localization.get("EditExtension_Notify.Success"));
      }));

      if (closeOnSave) {
        props.dispatch(visiblePanel.selectPanel(0));
        props.dispatch(extension.selectEditingTab(0));
      }
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "module-settings"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PermissionGrid"], {
          permissions: state.permissions,
          service: utils["a" /* default */].utilities.sf,
          onPermissionsChanged: this.onPermissionsChanged.bind(this)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "actions-row"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          onClick: props.onCancel.bind(this)
        }, localization.get("Cancel")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.savePermissions.bind(this, true)
        }, localization.get("EditModule_SaveAndClose.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.savePermissions.bind(this, false)
        }, localization.get("Save"))))
      );
    }
  }]);

  return ModuleSettings;
}(external_window_dnn_nodeModules_React_["Component"]);

ModuleSettings_ModuleSettings.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  onCancel: prop_types_default.a.func,
  updateExtensionBeingEdited: prop_types_default.a.func,
  extensionBeingEdited: prop_types_default.a.object,
  extensionBeingEditedIndex: prop_types_default.a.object
};

function ModuleSettings_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex
  };
}

/* harmony default export */ var EditSettings_ModuleSettings = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(ModuleSettings_mapStateToProps)(ModuleSettings_ModuleSettings));
// CONCATENATED MODULE: ./src/components/EditExtension/EditSettings/WrapWithContainer/index.jsx




var WrapWithContainer_WrapWithContainer = function WrapWithContainer(_ref) {
  var children = _ref.children;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "extension-form"
    }, children)
  );
};

WrapWithContainer_WrapWithContainer.propTypes = {
  children: prop_types_default.a.node
};
/* harmony default export */ var EditSettings_WrapWithContainer = (WrapWithContainer_WrapWithContainer);
// CONCATENATED MODULE: ./src/components/EditExtension/EditSettings/IFrameHandler.jsx
function IFrameHandler_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { IFrameHandler_typeof = function _typeof(obj) { return typeof obj; }; } else { IFrameHandler_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return IFrameHandler_typeof(obj); }

function IFrameHandler_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function IFrameHandler_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function IFrameHandler_createClass(Constructor, protoProps, staticProps) { if (protoProps) IFrameHandler_defineProperties(Constructor.prototype, protoProps); if (staticProps) IFrameHandler_defineProperties(Constructor, staticProps); return Constructor; }

function IFrameHandler_possibleConstructorReturn(self, call) { if (call && (IFrameHandler_typeof(call) === "object" || typeof call === "function")) { return call; } return IFrameHandler_assertThisInitialized(self); }

function IFrameHandler_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function IFrameHandler_getPrototypeOf(o) { IFrameHandler_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return IFrameHandler_getPrototypeOf(o); }

function IFrameHandler_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) IFrameHandler_setPrototypeOf(subClass, superClass); }

function IFrameHandler_setPrototypeOf(o, p) { IFrameHandler_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return IFrameHandler_setPrototypeOf(o, p); }





var IFrameHandler_IFrameHandler =
/*#__PURE__*/
function (_Component) {
  IFrameHandler_inherits(IFrameHandler, _Component);

  function IFrameHandler() {
    IFrameHandler_classCallCheck(this, IFrameHandler);

    return IFrameHandler_possibleConstructorReturn(this, IFrameHandler_getPrototypeOf(IFrameHandler).apply(this, arguments));
  }

  IFrameHandler_createClass(IFrameHandler, [{
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("iframe", {
          src: props.extensionBeingEdited.siteSettingsLink.value,
          style: {
            width: "100%",
            height: "750px"
          }
        })
      );
    }
  }]);

  return IFrameHandler;
}(external_window_dnn_nodeModules_React_["Component"]);

IFrameHandler_IFrameHandler.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  extensionBeingEdited: prop_types_default.a.object
};

function IFrameHandler_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex
  };
}

/* harmony default export */ var EditSettings_IFrameHandler = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(IFrameHandler_mapStateToProps)(IFrameHandler_IFrameHandler));
// CONCATENATED MODULE: ./src/components/EditExtension/EditSettings/index.jsx
function EditSettings_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { EditSettings_typeof = function _typeof(obj) { return typeof obj; }; } else { EditSettings_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return EditSettings_typeof(obj); }

function EditSettings_extends() { EditSettings_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return EditSettings_extends.apply(this, arguments); }

function EditSettings_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function EditSettings_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function EditSettings_createClass(Constructor, protoProps, staticProps) { if (protoProps) EditSettings_defineProperties(Constructor.prototype, protoProps); if (staticProps) EditSettings_defineProperties(Constructor, staticProps); return Constructor; }

function EditSettings_possibleConstructorReturn(self, call) { if (call && (EditSettings_typeof(call) === "object" || typeof call === "function")) { return call; } return EditSettings_assertThisInitialized(self); }

function EditSettings_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function EditSettings_getPrototypeOf(o) { EditSettings_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return EditSettings_getPrototypeOf(o); }

function EditSettings_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) EditSettings_setPrototypeOf(subClass, superClass); }

function EditSettings_setPrototypeOf(o, p) { EditSettings_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return EditSettings_setPrototypeOf(o, p); }












function checkIfKnownAuthenticationProvider(name) {
  switch (name) {
    case "DNN_FacebookAuthentication":
    case "DNN_GoogleAuthentication":
    case "DNN_LiveAuthentication":
    case "DNN_TwitterAuthentication":
      return true;

    default:
      return false;
  }
}

var EditSettings_EditSettings =
/*#__PURE__*/
function (_Component) {
  EditSettings_inherits(EditSettings, _Component);

  function EditSettings() {
    EditSettings_classCallCheck(this, EditSettings);

    return EditSettings_possibleConstructorReturn(this, EditSettings_getPrototypeOf(EditSettings).apply(this, arguments));
  }

  EditSettings_createClass(EditSettings, [{
    key: "getSettingsEditor",
    value: function getSettingsEditor(props) {
      switch (props.type) {
        case "Module":
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(EditSettings_ModuleSettings, props)
          );

        case "Auth_System":
          if (checkIfKnownAuthenticationProvider(props.extensionBeingEdited.name.value)) {
            return (
              /*#__PURE__*/
              external_window_dnn_nodeModules_React_default.a.createElement(EditSettings_AuthenticationSystemSettings, props)
            );
          } else {
            return (
              /*#__PURE__*/
              external_window_dnn_nodeModules_React_default.a.createElement(EditSettings_IFrameHandler, props)
            );
          }

        case "Container":
        case "Skin":
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(EditSettings_WrapWithContainer, null,
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_Container, EditSettings_extends({}, props, {
              disabled: true
            })))
          );

        case "JavaScript_Library":
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(EditSettings_WrapWithContainer, null,
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_JavascriptLibrary, EditSettings_extends({}, props, {
              disabled: true
            })))
          );

        case "SkinObject":
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(EditSettings_WrapWithContainer, null,
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_SkinObject, EditSettings_extends({}, props, {
              disabled: true
            })))
          );

        case "CoreLanguagePack":
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(EditSettings_WrapWithContainer, null,
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_CoreLanguagePack, EditSettings_extends({}, props, {
              disabled: true
            })))
          );

        case "ExtensionLanguagePack":
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(EditSettings_WrapWithContainer, null,
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CustomSettings_ExtensionLanguagePack, EditSettings_extends({}, props, {
              disabled: true
            })))
          );

        default:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(EditSettings_IFrameHandler, props)
          );
      }
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return this.getSettingsEditor(props);
    }
  }]);

  return EditSettings;
}(external_window_dnn_nodeModules_React_["Component"]);

/* harmony default export */ var EditExtension_EditSettings = (EditSettings_EditSettings);
// EXTERNAL MODULE: ./src/components/EditExtension/style.less
var EditExtension_style = __webpack_require__(48);
var EditExtension_style_default = /*#__PURE__*/__webpack_require__.n(EditExtension_style);

// CONCATENATED MODULE: ./src/components/EditExtension/index.jsx
function EditExtension_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { EditExtension_typeof = function _typeof(obj) { return typeof obj; }; } else { EditExtension_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return EditExtension_typeof(obj); }

function EditExtension_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function EditExtension_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function EditExtension_createClass(Constructor, protoProps, staticProps) { if (protoProps) EditExtension_defineProperties(Constructor.prototype, protoProps); if (staticProps) EditExtension_defineProperties(Constructor, staticProps); return Constructor; }

function EditExtension_possibleConstructorReturn(self, call) { if (call && (EditExtension_typeof(call) === "object" || typeof call === "function")) { return call; } return EditExtension_assertThisInitialized(self); }

function EditExtension_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function EditExtension_getPrototypeOf(o) { EditExtension_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return EditExtension_getPrototypeOf(o); }

function EditExtension_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) EditExtension_setPrototypeOf(subClass, superClass); }

function EditExtension_setPrototypeOf(o, p) { EditExtension_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return EditExtension_setPrototypeOf(o, p); }















var EditExtension_EditExtension =
/*#__PURE__*/
function (_Component) {
  EditExtension_inherits(EditExtension, _Component);

  function EditExtension() {
    var _this;

    EditExtension_classCallCheck(this, EditExtension);

    _this = EditExtension_possibleConstructorReturn(this, EditExtension_getPrototypeOf(EditExtension).call(this));
    _this.state = {
      extensionBeingEdited: {
        type: "",
        name: "",
        description: "",
        friendlyName: "",
        version: "9.0.0",
        owner: "",
        url: "",
        organization: "",
        email: ""
      },
      selectedTabIndex: 0
    };
    return _this;
  }

  EditExtension_createClass(EditExtension, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;
      this.isHost = utils["a" /* default */].settings.isHost;

      if ((!props.moduleCategories || props.moduleCategories.length === 0) && this.isHost) {
        props.dispatch(extension.getModuleCategories());
      }
    }
  }, {
    key: "onVersionChange",
    value: function onVersionChange(index, option) {
      var props = this.props;

      var _extensionBeingEdited = JSON.parse(JSON.stringify(props.extensionBeingEdited));

      if (_extensionBeingEdited.version && _extensionBeingEdited.version.value) {
        var versionArray = _extensionBeingEdited.version.value.split(".");

        versionArray[index] = option.value;
        _extensionBeingEdited.version.value = versionArray.join(".");
      }

      this.updateExtensionBeingEdited(_extensionBeingEdited);
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var props = this.props;
      var value = EditExtension_typeof(event) === "object" ? event.target.value : event;

      var _extensionBeingEdited = JSON.parse(JSON.stringify(props.extensionBeingEdited));

      var field = _extensionBeingEdited[key];
      field.value = value;

      if (field.validateRequired && field.error) {
        field.error = false;
        this.toggleTriedToSave();
        this.toggleTabError(field.tabMapping, "remove");
      }

      this.updateExtensionBeingEdited(_extensionBeingEdited);
    }
  }, {
    key: "onAssignedPortalsChange",
    value: function onAssignedPortalsChange(key, value, callback) {
      var props = this.props;

      var _extensionBeingEdited = JSON.parse(JSON.stringify(props.extensionBeingEdited));

      _extensionBeingEdited[key].value = value;
      this.updateExtensionBeingEdited(_extensionBeingEdited, callback);
    }
  }, {
    key: "updateExtensionBeingEdited",
    value: function updateExtensionBeingEdited(extensionBeingEdited, callback) {
      var props = this.props;
      props.dispatch(extension.updateExtensionBeingEdited(extensionBeingEdited, callback));
    }
  }, {
    key: "getAuthSystemCustomSettings",
    value: function getAuthSystemCustomSettings(extension) {
      var payload = {
        loginControlSource: extension.loginControlSource.value,
        logoffControlSource: extension.logoffControlSource.value,
        enabled: extension.enabled.value,
        settingsControlSource: extension.settingsControlSource.value,
        authenticationType: extension.authenticationType.value,
        appId: extension.appId.value,
        appSecret: extension.appSecret.value,
        appEnabled: extension.appEnabled.value
      };

      if (extension.name.value === "DNNPro_ActiveDirectoryAuthentication") {
        delete payload.appId;
        delete payload.appSecret;
        delete payload.appEnabled;
      }

      return payload;
    }
  }, {
    key: "parseEditorActions",
    value: function parseEditorActions(extension) {
      switch (extension.packageType.value.toLowerCase()) {
        case "module":
          return extension.desktopModuleId ? {
            category: extension.category.value,
            dependencies: extension.dependencies.value,
            hostPermissions: extension.hostPermissions.value,
            shareable: extension.shareable.value,
            premiumModule: extension.premiumModule.value,
            assignPortal: JSON.stringify(extension.assignedPortals.value),
            unassignPortal: JSON.stringify(extension.unassignedPortals.value),
            folderName: extension.folderName.value,
            businessController: extension.businessController.value
          } : null;

        case "auth_system":
          return this.getAuthSystemCustomSettings(extension);

        case "javascript_library":
          return {
            customCdn: extension.customCdn.value
          };

        case "skin":
        case "container":
          return {
            themePackageName: extension.themePackageName.value
          };

        case "skinobject":
          return {
            controlKey: extension.controlKey.value,
            controlSrc: extension.controlSrc.value,
            supportsPartialRendering: extension.supportsPartialRendering.value
          };

        case "corelanguagepack":
          return {
            languageId: extension.languageId.value
          };

        case "extensionlanguagepack":
          return {
            languageId: extension.languageId.value,
            dependentPackageId: extension.dependentPackageId.value
          };

        default:
          return {};
      }
    }
  }, {
    key: "onSaveExtension",
    value: function onSaveExtension(close) {
      var _this2 = this;

      var props = this.props;
      var editorActions = this.parseEditorActions(props.extensionBeingEdited);
      props.dispatch(extension.updateExtension(props.extensionBeingEdited, editorActions, props.extensionBeingEditedIndex, function () {
        if (close) {
          _this2.selectPanel(0);

          props.dispatch(extension.selectEditingTab(0));
        }
      }));
    }
  }, {
    key: "toggleTriedToSave",
    value: function toggleTriedToSave() {
      var props = this.props;
      props.dispatch(extension.toggleTriedToSave());
    }
  }, {
    key: "toggleTabError",
    value: function toggleTabError(tabWithError, action) {
      var props = this.props;
      props.dispatch(extension.toggleTabError(tabWithError, action));
    }
  }, {
    key: "validateFields",
    value: function validateFields() {
      var _this3 = this;

      var props = this.props;
      var errorCount = 0;

      var _extensionBeingEdited = JSON.parse(JSON.stringify(props.extensionBeingEdited));

      Object.keys(_extensionBeingEdited).forEach(function (key) {
        var field = _extensionBeingEdited[key];

        if (field.validateRequired && field.value === "") {
          field.error = true;
          errorCount++;

          _this3.toggleTabError(field.tabMapping);
        }
      });

      if (props.triedToSave === false) {
        this.toggleTriedToSave();
      }

      this.updateExtensionBeingEdited(_extensionBeingEdited);
      return errorCount === 0;
    }
  }, {
    key: "selectPanel",
    value: function selectPanel(panel, event) {
      if (event) {
        event.preventDefault();
      }

      var props = this.props;
      props.dispatch(visiblePanel.selectPanel(panel));
    }
  }, {
    key: "_getTabHeaders",
    value: function _getTabHeaders() {
      var tabHeaders = [localization.get("EditExtension_PackageInformation.TabHeader"), localization.get("EditExtension_ExtensionSettings.TabHeader"), localization.get("EditExtension_SiteSettings.TabHeader"), localization.get("EditExtension_License.TabHeader"), localization.get("EditExtension_ReleaseNotes.TabHeader")];
      var siteSettingIndex = 2;

      if (!this.isHost || !this.getExtensionSettingTabVisible(this.props.extensionBeingEdited.packageType.value)) {
        tabHeaders.splice(1, 1);
        siteSettingIndex = 1;
      }

      if (!this.getSiteSettingTabVisible(this.props.extensionBeingEdited.packageType.value)) {
        tabHeaders.splice(siteSettingIndex, 1);
      }

      return tabHeaders;
    }
  }, {
    key: "getTabHeaders",
    value: function getTabHeaders() {
      var props = this.props;

      var tabHeaders = this._getTabHeaders();

      return tabHeaders.map(function (tabHeader, index) {
        var hasError = props.tabsWithError.indexOf(index) > -1;
        return (
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement("span", {
            key: index
          }, tabHeader, " ",
          /*#__PURE__*/
          external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Tooltip"], {
            type: "error",
            rendered: hasError,
            messages: [localization.get("EditExtensions_TabHasError")]
          }))
        );
      });
    }
  }, {
    key: "onSave",
    value: function onSave(close) {
      if (!this.validateFields()) {
        return;
      }

      this.onSaveExtension(close === true);
    }
  }, {
    key: "confirmAction",
    value: function confirmAction(callback) {
      var props = this.props;

      if (props.moduleDefinitionFormIsDirty) {
        this.setState({});
        utils["a" /* default */].utilities.confirm(localization.get("UnsavedChanges.HelpText"), localization.get("UnsavedChanges.Confirm"), localization.get("UnsavedChanges.Cancel"), function () {
          callback();
          props.dispatch(actions_moduleDefinition.setFormDirt(false));
        });
      } else {
        callback();
      }
    }
  }, {
    key: "onTabSelect",
    value: function onTabSelect(index) {
      var _this4 = this;

      this.confirmAction(function () {
        var props = _this4.props;
        props.dispatch(extension.selectEditingTab(index));
      });
    }
  }, {
    key: "startCreatePackageWizard",
    value: function startCreatePackageWizard() {
      var _this5 = this;

      var props = this.props;
      props.dispatch(actions_createPackage.getPackageManifest(props.extensionBeingEdited.packageId.value, function () {
        _this5.selectPanel(5);
      }));
    }
  }, {
    key: "onCancel",
    value: function onCancel(event) {
      if (event) {
        event.preventDefault();
      }

      var props = this.props;
      props.dispatch(extension.selectEditingTab(0));
      this.selectPanel(0);
    }
  }, {
    key: "getExtensionSettingTabVisible",
    value: function getExtensionSettingTabVisible(type) {
      switch (type) {
        case "Auth_System":
        case "SkinObject":
        case "Skin":
        case "Container":
        case "ExtensionLanguagePack":
        case "CoreLanguagePack":
        case "JavaScript_Library":
          return true;

        case "Module":
          return this.props.extensionBeingEdited.desktopModuleId ? true : false;

        default:
          return false;
      }
    }
  }, {
    key: "getSiteSettingTabVisible",
    value: function getSiteSettingTabVisible(type) {
      switch (type) {
        case "SkinObject":
        case "Skin":
        case "Container":
        case "ExtensionLanguagePack":
        case "CoreLanguagePack":
        case "JavaScript_Library":
          return !this.isHost;

        case "Auth_System":
          return true;

        case "Module":
          return this.props.extensionBeingEdited.desktopModuleId ? true : false;

        default:
          return false;
      }
    }
  }, {
    key: "getTabUI",
    value: function getTabUI() {
      var props = this.props,
          extensionBeingEdited = props.extensionBeingEdited;
      var allTabs = [
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
        key: "first",
        className: "package-information-box extension-form"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(EditExtension_PackageInformation, {
        onSave: this.onSave.bind(this),
        validationMapped: true,
        extensionBeingEdited: extensionBeingEdited,
        onVersionChange: this.onVersionChange.bind(this),
        onCancel: this.onCancel.bind(this),
        validateFields: this.validateFields.bind(this),
        onChange: this.onChange.bind(this),
        disabled: !this.isHost,
        updateExtensionBeingEdited: this.updateExtensionBeingEdited.bind(this),
        triedToSave: props.triedToSave,
        toggleTriedToSave: this.toggleTriedToSave.bind(this),
        primaryButtonText: localization.get("Save.Button")
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
        key: "second",
        className: "extension-form"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(EditExtension_CustomSettings, {
        type: extensionBeingEdited.packageType.value,
        primaryButtonText: localization.get("Save.Button"),
        onChange: this.onChange.bind(this),
        onCancel: this.onCancel.bind(this),
        onSave: this.onSave.bind(this),
        onAssignedPortalsChange: this.onAssignedPortalsChange.bind(this)
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
        key: "third"
      },
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(EditExtension_EditSettings, {
        type: extensionBeingEdited.packageType.value,
        onChange: this.onChange.bind(this),
        onCancel: this.onCancel.bind(this),
        onSave: this.onSave.bind(this),
        extensionBeingEdited: extensionBeingEdited,
        updateExtensionBeingEdited: this.updateExtensionBeingEdited.bind(this)
      })),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(EditExtension_License, {
        key: "fourth",
        value: extensionBeingEdited.license.value,
        onChange: this.onChange.bind(this),
        disabled: !this.isHost,
        onCancel: this.onCancel.bind(this),
        onSave: this.onSave.bind(this),
        primaryButtonText: localization.get("Save.Button")
      }),
      /*#__PURE__*/
      external_window_dnn_nodeModules_React_default.a.createElement(EditExtension_ReleaseNotes, {
        key: "fifth",
        value: extensionBeingEdited.releaseNotes.value,
        onChange: this.onChange.bind(this),
        onCancel: this.onCancel.bind(this),
        disabled: !this.isHost,
        onSave: this.onSave.bind(this),
        primaryButtonText: localization.get("Save.Button")
      })];
      var siteSettingIndex = 2;

      if (!this.isHost || !this.getExtensionSettingTabVisible(extensionBeingEdited.packageType.value)) {
        allTabs.splice(1, 1);
        siteSettingIndex = 1;
      }

      if (!this.getSiteSettingTabVisible(extensionBeingEdited.packageType.value)) {
        allTabs.splice(siteSettingIndex, 1);
      }

      return allTabs;
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingEdited = props.extensionBeingEdited;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: EditExtension_style_default.a.editExtension
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: extensionBeingEdited.friendlyName.value + " " + localization.get("Extension.Header")
        }, this.isHost &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          size: "large",
          onClick: this.startCreatePackageWizard.bind(this)
        }, localization.get("EditExtension_CreatePackage.Button"))),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], {
          backToLinkProps: {
            text: localization.get("BackToExtensions"),
            onClick: this.onCancel.bind(this)
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DnnTabs"], {
          tabHeaders: this.getTabHeaders(),
          onSelect: this.onTabSelect.bind(this),
          selectedIndex: props.selectedEditingTab,
          type: "primary"
        }, this.getTabUI())))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return EditExtension;
}(external_window_dnn_nodeModules_React_["Component"]);

EditExtension_EditExtension.propTypes = {
  onCancel: prop_types_default.a.func,
  onUpdateExtension: prop_types_default.a.func,
  disabled: prop_types_default.a.func,
  extensionBeingEdited: prop_types_default.a.object,
  packageBeingEditedSettings: prop_types_default.a.object
};

function EditExtension_mapStateToProps(state) {
  return {
    extensionBeingEdited: state.extension.extensionBeingEdited,
    extensionBeingEditedIndex: state.extension.extensionBeingEditedIndex,
    packageBeingEditedSettings: state.extension.packageBeingEditedSettings,
    moduleDefinitionFormIsDirty: state.moduleDefinition.formIsDirty,
    triedToSave: state.extension.triedToSave,
    tabsWithError: state.extension.tabsWithError,
    moduleCategories: state.extension.moduleCategories,
    selectedEditingTab: state.extension.tabBeingEdited
  };
}

/* harmony default export */ var components_EditExtension = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(EditExtension_mapStateToProps)(EditExtension_EditExtension));
// CONCATENATED MODULE: ./src/components/CreatePackageModal/StepOne.jsx
var StepOne_this = undefined;







var StepOne_StepOne = function StepOne(_ref) {
  var packageManifest = _ref.packageManifest,
      version = _ref.version,
      installedPackageTypes = _ref.installedPackageTypes,
      onNext = _ref.onNext,
      onCancel = _ref.onCancel,
      useExistingManifest = _ref.useExistingManifest,
      onChange = _ref.onChange,
      hasManifests = _ref.hasManifests,
      manifestDropdown = _ref.manifestDropdown,
      selectedManifest = _ref.selectedManifest,
      onSelect = _ref.onSelect,
      reviewManifest = _ref.reviewManifest;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "package-manifest-step"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(common_BasicPackageInformation, {
      extensionData: packageManifest,
      validationMapped: false,
      disabled: true,
      version: version,
      installedPackageTypes: installedPackageTypes
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "package-manifest-info"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("hr", null),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", {
      className: "box-title package-manifest-header"
    }, localization.get("CreatePackage_PackageManifest.Header")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", {
      className: "box-subtitle"
    }, localization.get("CreatePackage_PackageManifest.HelpText")), hasManifests &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "no-padding"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
      className: "existing-manifest-switch",
      label: localization.get("CreatePackage_UseExistingManifest.Label"),
      onText: localization.get("SwitchOn"),
      offText: localization.get("SwitchOff"),
      onChange: onChange.bind(StepOne_this, "useExistingManifest"),
      value: useExistingManifest
    })), useExistingManifest &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "no-padding"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["DropdownWithError"], {
      className: "existing-manifest-dropdown",
      options: manifestDropdown,
      label: localization.get("CreatePackage_ManifestFile.Label"),
      onSelect: onSelect.bind(StepOne_this, "selectedManifest"),
      value: selectedManifest
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "no-padding"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
      className: "review-manifest-switch",
      label: localization.get("CreatePackage_ReviewManifest.Label"),
      onText: localization.get("SwitchOn"),
      offText: localization.get("SwitchOff"),
      onChange: onChange.bind(StepOne_this, "reviewManifest"),
      value: reviewManifest
    }))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "modal-footer"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onCancel
    }, localization.get("Cancel.Button")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "primary",
      onClick: onNext
    }, localization.get("Next.Button"))))
  );
};

StepOne_StepOne.propTypes = {
  packageManifest: prop_types_default.a.object,
  version: prop_types_default.a.array,
  installedPackageTypes: prop_types_default.a.array,
  onNext: prop_types_default.a.func,
  onCancel: prop_types_default.a.func,
  useExistingManifest: prop_types_default.a.bool,
  onChange: prop_types_default.a.func,
  hasManifests: prop_types_default.a.bool,
  manifestDropdown: prop_types_default.a.array,
  selectedManifest: prop_types_default.a.bool,
  onSelect: prop_types_default.a.func,
  reviewManifest: prop_types_default.a.bool
};
/* harmony default export */ var CreatePackageModal_StepOne = (StepOne_StepOne);
// CONCATENATED MODULE: ./src/components/CreatePackageModal/StepTwo.jsx
var StepTwo_this = undefined;






var StepTwo_StepTwo = function StepTwo(_ref) {
  var packageManifest = _ref.packageManifest,
      onCancel = _ref.onCancel,
      onNext = _ref.onNext,
      onBasePathChange = _ref.onBasePathChange,
      onPrevious = _ref.onPrevious,
      onFileOrAssemblyChange = _ref.onFileOrAssemblyChange,
      onRefresh = _ref.onRefresh;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "review-files-step"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", {
      className: "box-title"
    }, localization.get("CreatePackage_ChooseFiles.Label")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", {
      className: "box-subtitle"
    }, localization.get("CreatePackage_ChooseFiles.HelpText")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "no-padding create-package-folder"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: 70,
      className: "no-padding create-package-folder-input-container"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
      value: packageManifest.basePath,
      onChange: onBasePathChange.bind(StepTwo_this),
      className: "create-package-folder-input",
      inputStyle: {
        marginBottom: 0
      },
      label: localization.get("CreatePackage_Folder.Label")
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      columnSize: 30,
      className: "no-padding refresh-file-list-button"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onRefresh
    }, localization.get("CreatePackage_RefreshFileList.Button")))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "package-files-container no-padding"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInput"], {
      className: "package-files",
      value: packageManifest.files.join("\n"),
      onChange: onFileOrAssemblyChange.bind(StepTwo_this, "files")
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "modal-footer"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onCancel
    }, localization.get("Cancel.Button")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onPrevious
    }, localization.get("Previous.Button")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "primary",
      onClick: onNext
    }, localization.get("Next.Button"))))
  );
};

StepTwo_StepTwo.propTypes = {
  packageManifest: prop_types_default.a.object,
  onCancel: prop_types_default.a.func,
  onNext: prop_types_default.a.func,
  onBasePathChange: prop_types_default.a.func,
  onPrevious: prop_types_default.a.func,
  onFileOrAssemblyChange: prop_types_default.a.func,
  onRefresh: prop_types_default.a.func
};
/* harmony default export */ var CreatePackageModal_StepTwo = (StepTwo_StepTwo);
// CONCATENATED MODULE: ./src/components/CreatePackageModal/StepThree.jsx
var StepThree_this = undefined;






var StepThree_StepThree = function StepThree(_ref) {
  var packageManifest = _ref.packageManifest,
      onCancel = _ref.onCancel,
      onNext = _ref.onNext,
      onPrevious = _ref.onPrevious,
      onFileOrAssemblyChange = _ref.onFileOrAssemblyChange;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "review-assemblies-step"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", {
      className: "box-title"
    }, localization.get("CreatePackage_ChooseAssemblies.Label")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", {
      className: "box-subtitle"
    }, localization.get("CreatePackage_ChooseAssemblies.HelpText")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "package-assemblies-container no-padding"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInput"], {
      className: "package-assemblies",
      value: packageManifest.assemblies.join("\n"),
      onChange: onFileOrAssemblyChange.bind(StepThree_this, "assemblies")
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "modal-footer"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onCancel
    }, localization.get("Cancel.Button")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onPrevious
    }, localization.get("Previous.Button")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "primary",
      onClick: onNext
    }, localization.get("Next.Button"))))
  );
};

StepThree_StepThree.propTypes = {
  packageManifest: prop_types_default.a.object,
  onCancel: prop_types_default.a.func,
  onNext: prop_types_default.a.func,
  onPrevious: prop_types_default.a.func,
  onFileOrAssemblyChange: prop_types_default.a.func
};
/* harmony default export */ var CreatePackageModal_StepThree = (StepThree_StepThree);
// CONCATENATED MODULE: ./src/components/CreatePackageModal/StepFour.jsx
var StepFour_this = undefined;






var StepFour_StepFour = function StepFour(_ref) {
  var onCancel = _ref.onCancel,
      onNext = _ref.onNext,
      onChange = _ref.onChange,
      selectedManifest = _ref.selectedManifest,
      onPrevious = _ref.onPrevious;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "review-manifest-step"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", {
      className: "box-title"
    }, localization.get("CreatePackage_CreateManifest.Label")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", {
      className: "box-subtitle"
    }, localization.get("CreatePackage_CreateManifest.HelpText")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "package-manifest-container no-padding"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["MultiLineInput"], {
      className: "package-manifest",
      value: selectedManifest,
      onChange: onChange.bind(StepFour_this, "selectedManifest")
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "modal-footer"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onCancel
    }, localization.get("Cancel.Button")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onPrevious
    }, localization.get("Previous.Button")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "primary",
      onClick: onNext
    }, localization.get("Next.Button"))))
  );
};

StepFour_StepFour.propTypes = {
  onCancel: prop_types_default.a.func,
  onNext: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  selectedManifest: prop_types_default.a.string,
  onPrevious: prop_types_default.a.func
};
/* harmony default export */ var CreatePackageModal_StepFour = (StepFour_StepFour);
// CONCATENATED MODULE: ./src/components/CreatePackageModal/StepFive.jsx
var StepFive_this = undefined;





var StepFive_inputStyle = {
  width: "100%"
};

var StepFive_StepFive = function StepFive(_ref) {
  var onNext = _ref.onNext,
      onCancel = _ref.onCancel,
      onChange = _ref.onChange,
      createPackage = _ref.createPackage,
      createManifest = _ref.createManifest,
      manifestName = _ref.manifestName,
      useExistingManifest = _ref.useExistingManifest,
      archiveName = _ref.archiveName,
      onPrevious = _ref.onPrevious;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "create-package-step"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", {
      className: "box-title"
    }, localization.get("CreatePackage_CreatePackage.Label")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", null, localization.get("CreatePackage_FinalStep.HelpText")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", null, localization.get("CreatePackage_FinalStep.HelpTextTwo")), !useExistingManifest &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridSystem"], {
      className: "with-right-border top-half"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("div", {
      className: "left-side"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
      className: "create-manifest-switch",
      label: localization.get("CreatePackage_CreateManifestFile.Label"),
      onText: localization.get("SwitchOn"),
      offText: localization.get("SwitchOff"),
      onChange: onChange.bind(StepFive_this, "createManifest"),
      value: createManifest
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
      label: localization.get("CreatePackage_ManifestFileName.Label"),
      tooltipMessage: localization.get("CreatePackage_ManifestFileName.HelpText"),
      style: StepFive_inputStyle,
      onChange: onChange.bind(StepFive_this, "manifestName"),
      className: "extension-package-name",
      value: manifestName
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("div", {
      className: "right-side"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
      className: "review-manifest-switch",
      label: localization.get("CreatePackage_CreatePackage.Label"),
      onText: localization.get("SwitchOn"),
      offText: localization.get("SwitchOff"),
      onChange: onChange.bind(StepFive_this, "createPackage"),
      value: createPackage
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
      label: localization.get("CreatePackage_ArchiveFileName.Label"),
      tooltipMessage: localization.get("CreatePackage_ArchiveFileName.HelpText"),
      style: StepFive_inputStyle,
      className: "extension-package-name",
      onChange: onChange.bind(StepFive_this, "archiveName"),
      value: archiveName
    }))), useExistingManifest &&
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "no-padding using-existing-manifest"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Switch"], {
      className: "review-manifest-switch",
      label: localization.get("CreatePackage_CreatePackage.Label"),
      onText: localization.get("SwitchOn"),
      offText: localization.get("SwitchOff"),
      onChange: onChange.bind(StepFive_this, "createPackage"),
      value: createPackage
    }),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["SingleLineInputWithError"], {
      label: localization.get("CreatePackage_ArchiveFileName.Label"),
      tooltipMessage: localization.get("CreatePackage_ArchiveFileName.HelpText"),
      style: StepFive_inputStyle,
      className: "extension-package-name",
      onChange: onChange.bind(StepFive_this, "archiveName"),
      value: archiveName
    })),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "modal-footer"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onCancel
    }, localization.get("Cancel.Button")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "secondary",
      onClick: onPrevious
    }, localization.get("Previous.Button")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "primary",
      disabled: !createPackage && !createManifest,
      onClick: onNext
    }, localization.get("Next.Button"))))
  );
};

StepFive_StepFive.propTypes = {
  onNext: prop_types_default.a.func,
  onCancel: prop_types_default.a.func,
  onChange: prop_types_default.a.func,
  createPackage: prop_types_default.a.bool,
  createManifest: prop_types_default.a.bool,
  manifestName: prop_types_default.a.string,
  useExistingManifest: prop_types_default.a.bool,
  archiveName: prop_types_default.a.string,
  onPrevious: prop_types_default.a.func
};
/* harmony default export */ var CreatePackageModal_StepFive = (StepFive_StepFive);
// CONCATENATED MODULE: ./src/components/CreatePackageModal/StepSix.jsx





var packageCreationStyle = {
  border: "1px solid #c8c8c8",
  marginBottom: 25,
  height: 250,
  width: "100%"
};

var StepSix_StepSix = function StepSix(_ref) {
  var onClose = _ref.onClose,
      logs = _ref.logs;
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "review-logs-step"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("h6", {
      className: "box-title"
    }, localization.get("CreatePackage_ChooseFiles.Label")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("p", {
      className: "box-subtitle"
    }, localization.get("CreatePackage_ChooseFiles.HelpText")),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "package-logs-container no-padding"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactCustomScrollBars_["Scrollbars"], {
      style: packageCreationStyle
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("div", {
      className: "package-creation-report"
    }, logs.map(function (log, i) {
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("p", {
          key: i
        }, log)
      );
    })))),
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
      className: "modal-footer"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
      type: "primary",
      onClick: onClose
    }, localization.get("Done.Button"))))
  );
};

StepSix_StepSix.propTypes = {
  onClose: prop_types_default.a.func,
  logs: prop_types_default.a.array
};
/* harmony default export */ var CreatePackageModal_StepSix = (StepSix_StepSix);
// EXTERNAL MODULE: ./src/components/CreatePackageModal/style.less
var CreatePackageModal_style = __webpack_require__(49);
var CreatePackageModal_style_default = /*#__PURE__*/__webpack_require__.n(CreatePackageModal_style);

// CONCATENATED MODULE: ./src/components/CreatePackageModal/index.jsx
function CreatePackageModal_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { CreatePackageModal_typeof = function _typeof(obj) { return typeof obj; }; } else { CreatePackageModal_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return CreatePackageModal_typeof(obj); }

function CreatePackageModal_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function CreatePackageModal_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function CreatePackageModal_createClass(Constructor, protoProps, staticProps) { if (protoProps) CreatePackageModal_defineProperties(Constructor.prototype, protoProps); if (staticProps) CreatePackageModal_defineProperties(Constructor, staticProps); return Constructor; }

function CreatePackageModal_possibleConstructorReturn(self, call) { if (call && (CreatePackageModal_typeof(call) === "object" || typeof call === "function")) { return call; } return CreatePackageModal_assertThisInitialized(self); }

function CreatePackageModal_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function CreatePackageModal_getPrototypeOf(o) { CreatePackageModal_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return CreatePackageModal_getPrototypeOf(o); }

function CreatePackageModal_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) CreatePackageModal_setPrototypeOf(subClass, superClass); }

function CreatePackageModal_setPrototypeOf(o, p) { CreatePackageModal_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return CreatePackageModal_setPrototypeOf(o, p); }

function CreatePackageModal_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function CreatePackageModal_extends() { CreatePackageModal_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return CreatePackageModal_extends.apply(this, arguments); }
















function mapToManifestPayload(payload, manifest) {
  return CreatePackageModal_extends(manifest, {
    archiveName: payload.archiveName,
    files: manifest.files || [],
    assemblies: manifest.assemblies || [],
    manifests: payload.selectedManifest || {}
  });
}

function mapToPackagePayload(payload, manifest) {
  return CreatePackageModal_extends(manifest, {
    archiveName: payload.archiveName,
    manifestName: payload.manifestName,
    manifests: CreatePackageModal_defineProperty({}, payload.selectedManifestKey, payload.selectedManifest)
  });
}

function mapToFileRequestPayload(manifest) {
  return CreatePackageModal_extends(manifest, {
    packageFolder: manifest.basePath,
    includeSource: true,
    includeAppCode: true
  });
}

function CreatePackageModal_deepCopy(object) {
  return JSON.parse(JSON.stringify(object));
}

var CreatePackageModal_CreatePackage =
/*#__PURE__*/
function (_Component) {
  CreatePackageModal_inherits(CreatePackage, _Component);

  function CreatePackage() {
    var _this;

    CreatePackageModal_classCallCheck(this, CreatePackage);

    _this = CreatePackageModal_possibleConstructorReturn(this, CreatePackageModal_getPrototypeOf(CreatePackage).call(this));
    _this.state = {
      useExistingManifest: false,
      selectedManifest: "",
      selectedManifestKey: "",
      archiveName: "",
      manifestName: "",
      createManifest: true,
      createPackage: true,
      reviewManifest: true
    };
    return _this;
  }

  CreatePackageModal_createClass(CreatePackage, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      this.goToStep(0);
      var props = this.props;

      var _packagePayload = CreatePackageModal_deepCopy(props.packagePayload);

      _packagePayload.archiveName = props.packageManifest.name + "_" + props.packageManifest.version + "_Install.zip";
      _packagePayload.manifestName = props.packageManifest.name + ".dnn";
      props.dispatch(actions_createPackage.updatePackagePayload(_packagePayload));
    }
  }, {
    key: "goToStep",
    value: function goToStep(step) {
      var props = this.props;
      props.dispatch(actions_createPackage.goToWizardStep(step));
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var value = CreatePackageModal_typeof(event) === "object" ? event.target.value : event;
      var props = this.props;

      var _packagePayload = CreatePackageModal_deepCopy(props.packagePayload);

      if (key === "useExistingManifest") {
        if (!_packagePayload.selectedManifest && value) {
          var selectedManifestKey = Object.keys(props.packageManifest.manifests)[0];
          _packagePayload.selectedManifest = props.packageManifest.manifests[selectedManifestKey];
          _packagePayload.selectedManifestKey = selectedManifestKey;
        } else {
          _packagePayload.selectedManifest = null;
          _packagePayload.selectedManifestKey = "default";
        }
      }

      _packagePayload[key] = value;
      props.dispatch(actions_createPackage.updatePackagePayload(_packagePayload));
    }
  }, {
    key: "onSelect",
    value: function onSelect(key, option) {
      var value = option.value;
      var props = this.props;

      var _packagePayload = CreatePackageModal_deepCopy(props.packagePayload);

      _packagePayload[key] = value;
      _packagePayload.selectedManifestKey = option.label;
      props.dispatch(actions_createPackage.updatePackagePayload(_packagePayload));
    }
  }, {
    key: "getManifestDropdown",
    value: function getManifestDropdown(manifests) {
      return Object.keys(manifests).map(function (key) {
        return {
          label: key,
          value: manifests[key]
        };
      });
    }
  }, {
    key: "goToStepTwo",
    value: function goToStepTwo() {
      var packagePayload = this.props.packagePayload;

      if (packagePayload.useExistingManifest) {
        this.goToStepFour();
      } else {
        this.goToStep(1);
      }
    }
  }, {
    key: "goToStepFour",
    value: function goToStepFour() {
      var _this2 = this;

      var props = this.props;

      if (props.packagePayload.reviewManifest) {
        props.dispatch(actions_createPackage.generateManifestPreview(mapToManifestPayload(CreatePackageModal_deepCopy(props.packagePayload), CreatePackageModal_deepCopy(props.packageManifest)), function () {
          _this2.goToStep(3);
        }));
      } else {
        this.goToStep(4);
      }
    }
  }, {
    key: "createPackage",
    value: function createPackage() {
      var _this3 = this;

      var props = this.props;

      if (props.packagePayload.createManifest) {
        props.dispatch(actions_createPackage.createManifest(mapToManifestPayload(CreatePackageModal_deepCopy(props.packagePayload), CreatePackageModal_deepCopy(props.packageManifest)), function () {
          if (!props.packagePayload.createPackage) {
            utils["a" /* default */].utilities.notify("Manifest successfully created.");
            setTimeout(function () {
              props.onCancel();
            }, 1500);
          }
        }));
      }

      if (props.packagePayload.createPackage) {
        props.dispatch(actions_createPackage.createPackage(mapToPackagePayload(CreatePackageModal_deepCopy(props.packagePayload), CreatePackageModal_deepCopy(props.packageManifest)), function () {
          _this3.goToStep(5);
        }, function (data) {
          utils["a" /* default */].utilities.notifyError(JSON.parse(data.responseText).Message, 5000);
        }));
      }
    }
  }, {
    key: "onStepBack",
    value: function onStepBack() {
      var props = this.props;

      switch (props.createPackageStep) {
        case 1:
          this.goToStep(0);
          break;

        case 2:
          this.goToStep(1);
          break;

        case 3:
          if (props.packagePayload.useExistingManifest) {
            this.goToStep(0);
          } else {
            this.goToStep(2);
          }

          break;

        case 4:
          if (props.packagePayload.reviewManifest) {
            this.goToStep(3);
          } else if (props.packagePayload.useExistingManifest) {
            this.goToStep(0);
          } else {
            this.goToStep(2);
          }

          break;

        default:
          break;
      }
    }
  }, {
    key: "onFileOrAssemblyChange",
    value: function onFileOrAssemblyChange(key, event) {
      var props = this.props;
      var value = CreatePackageModal_typeof(event) === "object" ? event.target.value : event;

      var _fileValue = value.split("\n");

      var packageManifest = CreatePackageModal_deepCopy(props.packageManifest);
      packageManifest[key] = _fileValue;
      props.dispatch(actions_createPackage.updatePackageManifest(packageManifest));
    }
  }, {
    key: "onBasePathChange",
    value: function onBasePathChange(event) {
      var value = CreatePackageModal_typeof(event) === "object" ? event.target.value : event;
      var props = this.props;
      var packageManifest = CreatePackageModal_deepCopy(props.packageManifest);
      packageManifest.basePath = value;
      props.dispatch(actions_createPackage.updatePackageManifest(packageManifest));
    }
  }, {
    key: "onRefresh",
    value: function onRefresh() {
      var props = this.props;
      props.dispatch(actions_createPackage.refreshPackageFiles(mapToFileRequestPayload(CreatePackageModal_deepCopy(props.packageManifest)), function () {}, function (data) {
        utils["a" /* default */].utilities.notifyError(JSON.parse(data.responseText).Message);
      }));
    }
  }, {
    key: "getCurrentWizardUI",
    value: function getCurrentWizardUI(step) {
      var props = this.props;
      var packageManifest = props.packageManifest,
          installedPackageTypes = props.installedPackageTypes,
          packagePayload = props.packagePayload;
      var version = packageManifest.version ? packageManifest.version.split(".") : [0, 0, 0];
      var manifestDropdown = this.getManifestDropdown(props.packageManifest.manifests);

      switch (step) {
        case 0:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CreatePackageModal_StepOne, {
              packageManifest: packageManifest,
              version: version,
              installedPackageTypes: installedPackageTypes,
              useExistingManifest: packagePayload.useExistingManifest,
              onNext: this.goToStepTwo.bind(this),
              onChange: this.onChange.bind(this),
              onCancel: props.onCancel.bind(this),
              manifestDropdown: manifestDropdown,
              selectedManifest: packagePayload.selectedManifest,
              onSelect: this.onSelect.bind(this),
              reviewManifest: packagePayload.reviewManifest,
              hasManifests: Object.keys(packageManifest.manifests).length > 0
            })
          );

        case 1:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CreatePackageModal_StepTwo, {
              packageManifest: packageManifest,
              onNext: this.goToStep.bind(this, 2),
              onRefresh: this.onRefresh.bind(this),
              onBasePathChange: this.onBasePathChange.bind(this),
              onFileOrAssemblyChange: this.onFileOrAssemblyChange.bind(this),
              onCancel: props.onCancel.bind(this),
              onPrevious: this.onStepBack.bind(this)
            })
          );

        case 2:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CreatePackageModal_StepThree, {
              packageManifest: packageManifest,
              onNext: this.goToStepFour.bind(this),
              onBasePathChange: this.onBasePathChange.bind(this),
              onFileOrAssemblyChange: this.onFileOrAssemblyChange.bind(this),
              onCancel: props.onCancel.bind(this),
              onPrevious: this.onStepBack.bind(this)
            })
          );

        case 3:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CreatePackageModal_StepFour, {
              packageManifest: packageManifest,
              selectedManifest: packagePayload.selectedManifest,
              onNext: this.goToStep.bind(this, 4),
              onChange: this.onChange.bind(this),
              onPrevious: this.onStepBack.bind(this),
              onCancel: props.onCancel.bind(this)
            })
          );

        case 4:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CreatePackageModal_StepFive, {
              onNext: this.createPackage.bind(this),
              onCancel: props.onCancel.bind(this),
              onChange: this.onChange.bind(this),
              createPackage: packagePayload.createPackage,
              createManifest: packagePayload.createManifest,
              manifestName: packagePayload.manifestName,
              archiveName: packagePayload.archiveName,
              useExistingManifest: packagePayload.useExistingManifest,
              onPrevious: this.onStepBack.bind(this)
            })
          );

        case 5:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement(CreatePackageModal_StepSix, {
              onClose: props.onCancel.bind(this),
              logs: props.createdPackage
            })
          );

        default:
          return (
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("p", null, "Oops, something went wrong. Click ",
            /*#__PURE__*/
            external_window_dnn_nodeModules_React_default.a.createElement("a", {
              href: "javascript:void(0)",
              onClick: props.onCancel.bind(this)
            }, " here "), " to go back")
          );
      }
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var createPackageStep = props.createPackageStep,
          packageManifest = props.packageManifest;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: CreatePackageModal_style_default.a.createPackage
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: localization.get("CreatePackage_Header.Header")
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], {
          backToLinkProps: {
            text: localization.get("BackToEditExtension").replace("{0}", packageManifest.friendlyName),
            onClick: props.onCancel.bind(this)
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "extension-form create-package-wizard"
        }, this.getCurrentWizardUI(createPackageStep))))
      ); // <p className="modal-pagination"> --1 of 2 -- </p>
    }
  }]);

  return CreatePackage;
}(external_window_dnn_nodeModules_React_["Component"]);

CreatePackageModal_CreatePackage.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  onCancel: prop_types_default.a.func,
  packageManifest: prop_types_default.a.object,
  installedPackageTypes: prop_types_default.a.array,
  createPackageStep: prop_types_default.a.number,
  packagePayload: prop_types_default.a.object
};

function CreatePackageModal_mapStateToProps(state) {
  return {
    installedPackageTypes: state.extension.installedPackageTypes,
    createdPackage: state.createPackage.createdPackage,
    packageManifest: state.createPackage.packageManifest,
    packagePayload: state.createPackage.packagePayload,
    createPackageStep: state.createPackage.currentStep
  };
}

/* harmony default export */ var CreatePackageModal = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(CreatePackageModal_mapStateToProps)(CreatePackageModal_CreatePackage));
// EXTERNAL MODULE: ./src/components/DeleteExtension/style.less
var DeleteExtension_style = __webpack_require__(50);
var DeleteExtension_style_default = /*#__PURE__*/__webpack_require__.n(DeleteExtension_style);

// CONCATENATED MODULE: ./src/components/DeleteExtension/index.jsx
function DeleteExtension_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { DeleteExtension_typeof = function _typeof(obj) { return typeof obj; }; } else { DeleteExtension_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return DeleteExtension_typeof(obj); }

function DeleteExtension_classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function DeleteExtension_defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function DeleteExtension_createClass(Constructor, protoProps, staticProps) { if (protoProps) DeleteExtension_defineProperties(Constructor.prototype, protoProps); if (staticProps) DeleteExtension_defineProperties(Constructor, staticProps); return Constructor; }

function DeleteExtension_possibleConstructorReturn(self, call) { if (call && (DeleteExtension_typeof(call) === "object" || typeof call === "function")) { return call; } return DeleteExtension_assertThisInitialized(self); }

function DeleteExtension_assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function DeleteExtension_getPrototypeOf(o) { DeleteExtension_getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return DeleteExtension_getPrototypeOf(o); }

function DeleteExtension_inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) DeleteExtension_setPrototypeOf(subClass, superClass); }

function DeleteExtension_setPrototypeOf(o, p) { DeleteExtension_setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return DeleteExtension_setPrototypeOf(o, p); }











var DeleteExtension_DeleteExtension =
/*#__PURE__*/
function (_Component) {
  DeleteExtension_inherits(DeleteExtension, _Component);

  function DeleteExtension() {
    DeleteExtension_classCallCheck(this, DeleteExtension);

    return DeleteExtension_possibleConstructorReturn(this, DeleteExtension_getPrototypeOf(DeleteExtension).call(this));
  }

  DeleteExtension_createClass(DeleteExtension, [{
    key: "onToggleDeleteFiles",
    value: function onToggleDeleteFiles() {
      this.props.dispatch(extension.toggleDeleteFiles());
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      this.props.dispatch(visiblePanel.selectPanel(0));
      this.props.dispatch(extension.setPackageBeingDeleted({}, -1)); //clear

      if (this.props.deleteExtensionFiles) {
        this.onToggleDeleteFiles();
      }
    }
  }, {
    key: "onDelete",
    value: function onDelete() {
      var _this = this;

      var props = this.props;
      utils["a" /* default */].utilities.confirm(localization.get("DeleteExtension.Warning").replace("{0}", props.extensionBeingDeleted.friendlyName), localization.get("DeleteExtension.Confirm"), localization.get("DeleteExtension.Cancel"), function () {
        props.dispatch(extension.deletePackage({
          id: props.extensionBeingDeleted.packageId,
          deleteFiles: props.deleteExtensionFiles
        }, props.extensionBeingDeletedIndex, _this.onCancel.bind(_this)));
      });
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var extensionBeingDeleted = props.extensionBeingDeleted;
      var version = extensionBeingDeleted.version.value ? extensionBeingDeleted.version.value.split(".") : [0, 0, 0];
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: DeleteExtension_style_default.a.DeleteExtension
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageHeader"], {
          title: localization.get("DeleteExtension.Action").replace("{0}", extensionBeingDeleted.friendlyName)
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPageBody"], {
          backToLinkProps: {
            text: localization.get("BackToExtensions"),
            onClick: this.onCancel.bind(this)
          }
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "delete-extension-box extension-form"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(common_BasicPackageInformation, {
          extensionData: extensionBeingDeleted,
          installedPackageTypes: props.installedPackageTypes,
          validationMapped: false,
          installationMode: true,
          primaryButtonText: localization.get("Next"),
          version: version,
          disabled: true,
          isAddMode: false,
          buttonsAreHidden: true
        }),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "delete-files-box"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Checkbox"], {
          label: localization.get("DeleteFiles.Label"),
          value: props.deleteExtensionFiles,
          labelPlace: "left",
          onChange: this.onToggleDeleteFiles.bind(this),
          tooltipMessage: localization.get("DeleteFiles.HelpText")
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["GridCell"], {
          className: "modal-footer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "secondary",
          onClick: this.onCancel.bind(this)
        }, localization.get("Cancel.Button")),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["Button"], {
          type: "primary",
          onClick: this.onDelete.bind(this)
        }, localization.get("Delete.Button"))))))
      );
    }
  }]);

  return DeleteExtension;
}(external_window_dnn_nodeModules_React_["Component"]);

DeleteExtension_DeleteExtension.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  extensionBeingDeleted: prop_types_default.a.object,
  deleteExtensionFiles: prop_types_default.a.bool,
  selectedInstalledPackageType: prop_types_default.a.string
};
DeleteExtension_DeleteExtension.defaultProps = {
  deleteExtensionFiles: false
};

function DeleteExtension_mapStateToProps(state) {
  return {
    selectedInstalledPackageType: state.extension.selectedInstalledPackageType,
    extensionBeingDeleted: state.extension.extensionBeingDeleted,
    extensionBeingDeletedIndex: state.extension.extensionBeingDeletedIndex,
    deleteExtensionFiles: state.extension.deleteExtensionFiles,
    installedPackageTypes: state.extension.installedPackageTypes
  };
}

/* harmony default export */ var components_DeleteExtension = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(DeleteExtension_mapStateToProps)(DeleteExtension_DeleteExtension));
// CONCATENATED MODULE: ./src/components/App.jsx
function App_typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { App_typeof = function _typeof(obj) { return typeof obj; }; } else { App_typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return App_typeof(obj); }

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

  function App() {
    var _this;

    App_classCallCheck(this, App);

    _this = App_possibleConstructorReturn(this, App_getPrototypeOf(App).call(this));
    _this.state = {
      extensionBeingEdited: {}
    };
    return _this;
  }

  App_createClass(App, [{
    key: "selectPanel",
    value: function selectPanel(panel, event) {
      if (event) {
        event.preventDefault();
      }

      var props = this.props;
      this.setState({
        referrer: "",
        referrerText: "",
        backToReferrerFunc: null
      }, function () {
        props.dispatch(visiblePanel.selectPanel(panel));
      });
    }
  }, {
    key: "backToReferrer",
    value: function backToReferrer(callback) {
      var _this2 = this;

      if (typeof callback === "function") {
        callback();
      }

      setTimeout(function () {
        _this2.setState({
          referrer: "",
          referrerText: "",
          backToReferrerFunc: null
        });
      }, 750);
    }
  }, {
    key: "updateReferrerInfo",
    value: function updateReferrerInfo(event) {
      this.setState({
        referrer: event.referrer,
        referrerText: event.referrerText,
        backToReferrerFunc: this.backToReferrer.bind(this, event.backToReferrerFunc)
      });
    }
  }, {
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this3 = this;

      var props = this.props;
      document.addEventListener("installPortalTheme", function (e) {
        props.dispatch(installation.setIsPortalPackage(true, function () {
          _this3.selectPanel(3);
        }));

        _this3.updateReferrerInfo(e);
      }, false);

      if (utils["a" /* default */].settings.installPortalTheme) {
        props.dispatch(installation.setIsPortalPackage(true, function () {
          _this3.selectPanel(3);
        }));
        this.updateReferrerInfo(utils["a" /* default */].settings);
      }
    }
    /* End Extension CRUD methods */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement("div", {
          className: "extensions-app personaBar-mainContainer"
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 0,
          className: props.selectedPage !== 0 ? "hidden" : ""
        },
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_Body, null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 1
        }, props.selectedPage === 1 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_NewModuleModal, null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 2
        }, props.selectedPage === 2 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_NewExtensionModal, {
          onCancel: this.selectPanel.bind(this, 0)
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 3
        }, props.selectedPage === 3 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(installExtensionModal, {
          backToReferrer: state.referrer,
          backToReferrerText: state.referrerText,
          backToReferrerFunc: state.backToReferrerFunc,
          onCancel: this.selectPanel.bind(this, 0)
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 4
        }, props.selectedPage === 4 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_EditExtension, null)),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 5
        }, props.selectedPage === 5 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(CreatePackageModal, {
          onCancel: this.selectPanel.bind(this, 4)
        })),
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_CommonComponents_["PersonaBarPage"], {
          isOpen: props.selectedPage === 6
        }, props.selectedPage === 6 &&
        /*#__PURE__*/
        external_window_dnn_nodeModules_React_default.a.createElement(components_DeleteExtension, {
          onCancel: this.selectPanel.bind(this, 0)
        })))
      );
    }
  }]);

  return App;
}(external_window_dnn_nodeModules_React_["Component"]);

App_App.propTypes = {
  dispatch: prop_types_default.a.func.isRequired,
  selectedPage: prop_types_default.a.number,
  selectedPageVisibleIndex: prop_types_default.a.number,
  packageTypes: prop_types_default.a.array,
  installedPackages: prop_types_default.a.array
};

function App_mapStateToProps(state) {
  return {
    packageTypes: state.extension.packageTypes,
    installedPackages: state.extension.installedPackages,
    selectedPage: state.visiblePanel.selectedPage,
    selectedPageVisibleIndex: state.visiblePanel.selectedPageVisibleIndex
  };
}

/* harmony default export */ var components_App = (Object(external_window_dnn_nodeModules_ReactRedux_["connect"])(App_mapStateToProps)(App_App));
// CONCATENATED MODULE: ./src/containers/Root.prod.js



var Root_prod_Root = function Root() {
  return (
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement("div", {
      className: "boilerplate-root"
    },
    /*#__PURE__*/
    external_window_dnn_nodeModules_React_default.a.createElement(components_App, null))
  );
};

/* harmony default export */ var Root_prod = __webpack_exports__["default"] = (Root_prod_Root);

/***/ }),
/* 108 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);

// EXTERNAL MODULE: external "window.dnn.nodeModules.React"
var external_window_dnn_nodeModules_React_ = __webpack_require__(0);
var external_window_dnn_nodeModules_React_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_React_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactDOM"
var external_window_dnn_nodeModules_ReactDOM_ = __webpack_require__(18);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReactRedux"
var external_window_dnn_nodeModules_ReactRedux_ = __webpack_require__(5);

// EXTERNAL MODULE: ./src/utils/index.jsx
var utils = __webpack_require__(4);

// CONCATENATED MODULE: ./src/globals/application.js

var extensions = {
  init: function init() {
    // This setting is required and define the public path
    // to allow the web application to download assets on demand
    // eslint-disable-next-line no-undef
    // __webpack_public_path__ = options.publicPath;
    var options = window.dnn.initExtensions();
    utils["a" /* default */].init(options); // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production

    __webpack_require__(51);
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
/* harmony default export */ var application = (extensions);
// EXTERNAL MODULE: external "window.dnn.nodeModules.Redux"
var external_window_dnn_nodeModules_Redux_ = __webpack_require__(11);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxThunk"
var external_window_dnn_nodeModules_ReduxThunk_ = __webpack_require__(13);
var external_window_dnn_nodeModules_ReduxThunk_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxThunk_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxImmutableStateInvariant"
var external_window_dnn_nodeModules_ReduxImmutableStateInvariant_ = __webpack_require__(19);
var external_window_dnn_nodeModules_ReduxImmutableStateInvariant_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxImmutableStateInvariant_);

// EXTERNAL MODULE: ./src/constants/actionTypes/index.js + 7 modules
var actionTypes = __webpack_require__(3);

// CONCATENATED MODULE: ./src/reducers/paginationReducer.js
function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function pagination() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    tabIndex: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["f" /* pagination */].LOAD_TAB_DATA:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        tabIndex: action.payload.index
      });

    default:
      return state;
  }
}
// CONCATENATED MODULE: ./src/reducers/visiblePanelReducer.js
function visiblePanelReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { visiblePanelReducer_defineProperty(target, key, source[key]); }); } return target; }

function visiblePanelReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function visiblePanel() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    selectedPage: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["g" /* visiblePanel */].SELECT_PANEL:
      return visiblePanelReducer_objectSpread2(visiblePanelReducer_objectSpread2({}, state), {}, {
        selectedPage: action.payload.selectedPage
      });

    default:
      return state;
  }
}
// CONCATENATED MODULE: ./src/reducers/extensionReducer.js
function extensionReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { extensionReducer_defineProperty(target, key, source[key]); }); } return target; }

function extensionReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }

function _toConsumableArray(arr) { return _arrayWithoutHoles(arr) || _iterableToArray(arr) || _nonIterableSpread(); }

function _nonIterableSpread() { throw new TypeError("Invalid attempt to spread non-iterable instance"); }

function _iterableToArray(iter) { if (Symbol.iterator in Object(iter) || Object.prototype.toString.call(iter) === "[object Arguments]") return Array.from(iter); }

function _arrayWithoutHoles(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = new Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }



function getAvailablePackageTypes(installedTypes) {
  return installedTypes.filter(function (type) {
    return type.HasAvailablePackages === true;
  });
}

function addToModuleList(value, list) {
  return list.concat(value).sort(function (a, b) {
    if (a.friendlyName.toLowerCase() < b.friendlyName.toLowerCase()) return -1;
    if (a.friendlyName.toLowerCase() > b.friendlyName.toLowerCase()) return 1;
    return 0;
  });
}

function getValidateRequired(key) {
  switch (key) {
    case "friendlyName":
      return true;

    default:
      return false;
  }
}

function getTabMapping(key) {
  switch (key) {
    case "license":
      return 3;

    case "releaseNotes":
      return 4;

    default:
      return 0;
  }
}

function validationMapExtensionBeingEdited(extensionBeingEdited) {
  var _extensionBeingEdited = _extends({}, extensionBeingEdited);

  Object.keys(_extensionBeingEdited).forEach(function (key) {
    var validateRequired = getValidateRequired(key);
    var tabMapping = getTabMapping(key);
    _extensionBeingEdited[key] = {
      value: _extensionBeingEdited[key] && !Object.prototype.hasOwnProperty.call(_extensionBeingEdited[key], "value") ? _extensionBeingEdited[key] : _extensionBeingEdited[key] && _extensionBeingEdited[key].value,
      validateRequired: validateRequired,
      tabMapping: tabMapping,
      error: false
    };
  });
  return _extensionBeingEdited;
}

function removeRecordFromArray(arr, index) {
  return [].concat(_toConsumableArray(arr.slice(0, index)), _toConsumableArray(arr.slice(index + 1)));
}

function extension() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    installedPackages: [],
    availablePackages: [],
    installedPackageTypes: [],
    availablePackageTypes: [],
    packageBeingEditedSettings: {},
    extensionBeingEdited: {},
    extensionBeingEditedIndex: -1,
    selectedInstalledPackageType: "",
    selectedAvailablePackageType: "",
    triedToSave: false,
    tabsWithError: [],
    moduleCategories: [],
    tabBeingEdited: 0,
    locales: [],
    localePackages: [],
    extensionBeingDeleted: {},
    extensionBeingDeletedIndex: -1,
    deleteExtensionFiles: false
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["b" /* extension */].RETRIEVED_INSTALLED_PACKAGES:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        installedPackages: action.payload.Results,
        selectedInstalledPackageType: action.payload.selectedInstalledPackageType
      });

    case actionTypes["b" /* extension */].RETRIEVED_AVAILABLE_PACKAGES:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        availablePackages: action.payload.Results,
        selectedAvailablePackageType: action.payload.selectedAvailablePackageType
      });

    case actionTypes["b" /* extension */].UPDATED_EXTENSION:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        installedPackages: [].concat(_toConsumableArray(state.installedPackages.slice(0, action.payload.index)), [action.payload.updatedExtension], _toConsumableArray(state.installedPackages.slice(action.payload.index + 1)))
      });

    case actionTypes["b" /* extension */].DELETED_EXTENSION:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        installedPackages: removeRecordFromArray(state.installedPackages, action.payload.index),
        deleteExtensionFiles: false
      });

    case actionTypes["b" /* extension */].SET_EXTENSION_BEING_DELETED:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        extensionBeingDeleted: action.payload.extensionBeingDeleted,
        extensionBeingDeletedIndex: action.payload.extensionBeingDeletedIndex
      });

    case actionTypes["b" /* extension */].TOGGLE_DELETE_EXTENSION_FILES:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        deleteExtensionFiles: !state.deleteExtensionFiles
      });

    case actionTypes["b" /* extension */].EDIT_EXTENSION:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        extensionBeingEdited: validationMapExtensionBeingEdited(action.payload.extensionBeingEdited),
        extensionBeingEditedIndex: action.payload.extensionBeingEditedIndex
      });

    case actionTypes["b" /* extension */].TOGGLE_TRIED_TO_SAVE:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        triedToSave: !state.triedToSave
      });

    case actionTypes["b" /* extension */].TOGGLE_TAB_ERROR:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        tabsWithError: action.payload.action === "remove" ? removeRecordFromArray(state.tabsWithError, state.tabsWithError.indexOf(action.payload.tabIndex)) : state.tabsWithError.indexOf(action.payload.tabIndex) < 0 ? state.tabsWithError.concat(action.payload.tabIndex) : state.tabsWithError
      });

    case actionTypes["b" /* extension */].UPDATED_EXTENSION_BEING_EDITED:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        extensionBeingEdited: action.payload.extensionBeingEdited
      });

    case actionTypes["b" /* extension */].RETRIEVED_INSTALLED_PACKAGE_TYPES:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        installedPackageTypes: action.payload.Results,
        availablePackageTypes: getAvailablePackageTypes(action.payload.Results)
      });

    case actionTypes["b" /* extension */].RETRIEVED_AVAILABLE_PACKAGE_TYPES:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        availablePackageTypes: action.payload.Results
      });

    case actionTypes["b" /* extension */].ADDED_NEW_EXTENSION:
    case actionTypes["b" /* extension */].INSTALLED_EXTENSION:
    case actionTypes["b" /* extension */].CREATED_NEW_MODULE:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        installedPackages: addToModuleList(action.payload.PackageInfo, state.installedPackages)
      });

    case actionTypes["b" /* extension */].RETRIEVED_MODULE_CATEGORIES:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        moduleCategories: action.payload
      });

    case actionTypes["b" /* extension */].SELECT_EDITING_TAB:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        tabBeingEdited: action.payload
      });

    case actionTypes["b" /* extension */].RETRIEVED_LOCALE_LIST:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        locales: action.payload
      });

    case actionTypes["b" /* extension */].RETRIEVED_PACKAGE_LOCALE_LIST:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        localePackages: action.payload
      });

    case actionTypes["b" /* extension */].DEPLOYED_AVAILABLE_PACKAGE:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        availablePackages: [].concat(_toConsumableArray(state.availablePackages.slice(0, action.payload.updatedPackageIndex)), [action.payload.updatedPackage], _toConsumableArray(state.availablePackages.slice(action.payload.updatedPackageIndex + 1)))
      });

    case actionTypes["b" /* extension */].RETRIEVED_PACKAGE_USAGE_FILTER:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        usageFilter: action.payload.Results
      });

    case actionTypes["b" /* extension */].RETRIEVED_PACKAGE_USAGE:
      return extensionReducer_objectSpread2(extensionReducer_objectSpread2({}, state), {}, {
        tabUrls: action.payload.Results
      });

    default:
      return state;
  }
}
// CONCATENATED MODULE: ./src/reducers/installationReducer.js
function installationReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { installationReducer_defineProperty(target, key, source[key]); }); } return target; }

function installationReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function installation() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    parsedInstallationPackage: null,
    installWizardStep: 0,
    installationLogs: [],
    availablePackage: {},
    licenseAccepted: false,
    installingAvailablePackage: false,
    viewingLog: false
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["d" /* installation */].PARSED_INSTALLATION_PACKAGE:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        parsedInstallationPackage: action.payload
      });

    case actionTypes["d" /* installation */].CLEAR_PARSED_INSTALLATION_PACKAGE:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        parsedInstallationPackage: null
      });

    case actionTypes["d" /* installation */].GO_TO_WIZARD_STEP:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        installWizardStep: action.payload.wizardStep
      });

    case actionTypes["d" /* installation */].INSTALLED_EXTENSION_LOGS:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        installationLogs: action.payload.logs
      });

    case actionTypes["b" /* extension */].INSTALLED_EXTENSION:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        installationLogs: action.payload.logs
      });

    case actionTypes["d" /* installation */].NOT_INSTALLING_AVAILABLE_PACKAGE:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        availablePackage: {},
        installingAvailablePackage: false
      });

    case actionTypes["d" /* installation */].INSTALLING_AVAILABLE_PACKAGE:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        availablePackage: action.payload,
        installingAvailablePackage: true
      });

    case actionTypes["d" /* installation */].SET_FAILED_INSTALLATION_LOGS:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        installationLogs: ["Oops, something went wrong and the installation failed. Please try the installation again."]
      });

    case actionTypes["d" /* installation */].TOGGLE_ACCEPT_LICENSE:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        licenseAccepted: action.payload
      });

    case actionTypes["d" /* installation */].TOGGLE_VIEWING_LOG:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        viewingLog: action.payload
      });

    case actionTypes["d" /* installation */].SET_IS_PORTAL_PACKAGE:
      return installationReducer_objectSpread2(installationReducer_objectSpread2({}, state), {}, {
        isPortalPackage: action.payload
      });

    default:
      return state;
  }
}
// CONCATENATED MODULE: ./src/reducers/folderReducer.js
function folderReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { folderReducer_defineProperty(target, key, source[key]); }); } return target; }

function folderReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



function addToFolders(value, folders) {
  return folders.concat(value).sort();
}

function folder() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    ownerFolders: [],
    moduleFolders: [],
    moduleFiles: []
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["c" /* folder */].RETRIEVED_OWNER_FOLDERS:
      return folderReducer_objectSpread2(folderReducer_objectSpread2({}, state), {}, {
        ownerFolders: action.payload
      });

    case actionTypes["c" /* folder */].RETRIEVED_MODULE_FOLDERS:
      return folderReducer_objectSpread2(folderReducer_objectSpread2({}, state), {}, {
        moduleFolders: action.payload,
        moduleFiles: []
      });

    case actionTypes["c" /* folder */].RETRIEVED_MODULE_FILES:
      return folderReducer_objectSpread2(folderReducer_objectSpread2({}, state), {}, {
        moduleFiles: action.payload
      });

    case actionTypes["c" /* folder */].CREATED_NEW_MODULE_FOLDER:
      return folderReducer_objectSpread2(folderReducer_objectSpread2({}, state), {}, {
        moduleFolders: addToFolders(action.payload.value, state.moduleFolders)
      });

    case actionTypes["c" /* folder */].CREATED_NEW_OWNER_FOLDER:
      return folderReducer_objectSpread2(folderReducer_objectSpread2({}, state), {}, {
        ownerFolders: addToFolders(action.payload.value, state.ownerFolders)
      });

    default:
      return state;
  }
}
// CONCATENATED MODULE: ./src/reducers/moduleDefinitionReducer.js
function moduleDefinitionReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { moduleDefinitionReducer_defineProperty(target, key, source[key]); }); } return target; }

function moduleDefinitionReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function moduleDefinition() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    formIsDirty: false,
    controlFormIsDirty: false,
    sourceFolders: [],
    sourceFiles: [],
    icons: []
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["e" /* moduleDefinition */].SET_FORM_DIRT:
      return moduleDefinitionReducer_objectSpread2(moduleDefinitionReducer_objectSpread2({}, state), {}, {
        formIsDirty: action.payload
      });

    case actionTypes["e" /* moduleDefinition */].SET_CONTROL_FORM_DIRT:
      return moduleDefinitionReducer_objectSpread2(moduleDefinitionReducer_objectSpread2({}, state), {}, {
        controlFormIsDirty: action.payload
      });

    case actionTypes["e" /* moduleDefinition */].RETRIEVED_SOURCE_FOLDERS:
      return moduleDefinitionReducer_objectSpread2(moduleDefinitionReducer_objectSpread2({}, state), {}, {
        sourceFolders: action.payload
      });

    case actionTypes["e" /* moduleDefinition */].RETRIEVED_SOURCE_FILES:
      return moduleDefinitionReducer_objectSpread2(moduleDefinitionReducer_objectSpread2({}, state), {}, {
        sourceFiles: action.payload
      });

    case actionTypes["e" /* moduleDefinition */].RETRIEVED_CONTROL_ICONS:
      return moduleDefinitionReducer_objectSpread2(moduleDefinitionReducer_objectSpread2({}, state), {}, {
        icons: action.payload
      });

    default:
      return state;
  }
}
// CONCATENATED MODULE: ./src/reducers/createPackageReducer.js
function createPackageReducer_extends() { createPackageReducer_extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return createPackageReducer_extends.apply(this, arguments); }

function createPackageReducer_objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { createPackageReducer_defineProperty(target, key, source[key]); }); } return target; }

function createPackageReducer_defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


var newPackagePayload = {
  useExistingManifest: false,
  selectedManifest: "",
  selectedManifestKey: "",
  archiveName: "",
  manifestName: "",
  createManifest: true,
  createPackage: true,
  reviewManifest: true
};
function createPackage() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    packageManifest: {},
    createdManifest: {},
    createdPackage: {},
    packagePayload: newPackagePayload,
    currentStep: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case actionTypes["a" /* createPackage */].RETRIEVED_PACKAGE_MANIFEST:
      return createPackageReducer_objectSpread2(createPackageReducer_objectSpread2({}, state), {}, {
        packageManifest: action.payload,
        packagePayload: newPackagePayload
      });

    case actionTypes["a" /* createPackage */].CREATED_PACKAGE_MANIFEST:
      return createPackageReducer_objectSpread2(createPackageReducer_objectSpread2({}, state), {}, {
        createdManifest: action.payload
      });

    case actionTypes["a" /* createPackage */].CREATED_PACKAGE:
      return createPackageReducer_objectSpread2(createPackageReducer_objectSpread2({}, state), {}, {
        createdPackage: action.payload.Logs
      });

    case actionTypes["a" /* createPackage */].GO_TO_STEP:
      return createPackageReducer_objectSpread2(createPackageReducer_objectSpread2({}, state), {}, {
        currentStep: action.payload
      });

    case actionTypes["a" /* createPackage */].UPDATED_PACKAGE_MANIFEST:
      return createPackageReducer_objectSpread2(createPackageReducer_objectSpread2({}, state), {}, {
        packageManifest: action.payload
      });

    case actionTypes["a" /* createPackage */].UPDATED_PACKAGE_PAYLOAD:
      return createPackageReducer_objectSpread2(createPackageReducer_objectSpread2({}, state), {}, {
        packagePayload: action.payload
      });

    case actionTypes["a" /* createPackage */].RETRIEVED_GENERATED_MANIFEST:
      return createPackageReducer_objectSpread2(createPackageReducer_objectSpread2({}, state), {}, {
        packagePayload: createPackageReducer_extends(JSON.parse(JSON.stringify(state.packagePayload)), {
          selectedManifest: action.payload.Content
        })
      });

    case actionTypes["a" /* createPackage */].REFRESH_PACKAGE_FILES:
      return createPackageReducer_objectSpread2(createPackageReducer_objectSpread2({}, state), {}, {
        packageManifest: createPackageReducer_extends(JSON.parse(JSON.stringify(state.packageManifest)), {
          files: action.payload
        })
      });

    default:
      return state;
  }
}
// CONCATENATED MODULE: ./src/reducers/rootReducer.js








var rootReducer = Object(external_window_dnn_nodeModules_Redux_["combineReducers"])({
  pagination: pagination,
  visiblePanel: visiblePanel,
  extension: extension,
  installation: installation,
  folder: folder,
  moduleDefinition: moduleDefinition,
  createPackage: createPackage
});
/* harmony default export */ var reducers_rootReducer = (rootReducer);
// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevTools"
var external_window_dnn_nodeModules_ReduxDevTools_ = __webpack_require__(20);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsLogMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_ = __webpack_require__(21);
var external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_ReduxDevToolsLogMonitor_);

// EXTERNAL MODULE: external "window.dnn.nodeModules.ReduxDevToolsDockMonitor"
var external_window_dnn_nodeModules_ReduxDevToolsDockMonitor_ = __webpack_require__(22);
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





/* eslint-disable no-undef */

var IS_PRODUCTION = "production" === "production";
function configureStore(initialState) {
  var store = Object(external_window_dnn_nodeModules_Redux_["createStore"])(reducers_rootReducer, initialState, Object(external_window_dnn_nodeModules_Redux_["compose"])(IS_PRODUCTION ? Object(external_window_dnn_nodeModules_Redux_["applyMiddleware"])(external_window_dnn_nodeModules_ReduxThunk_default.a) : Object(external_window_dnn_nodeModules_Redux_["applyMiddleware"])(external_window_dnn_nodeModules_ReduxThunk_default.a, external_window_dnn_nodeModules_ReduxImmutableStateInvariant_default()()), DevTools.instrument()));
  return store;
}
// EXTERNAL MODULE: ./src/containers/Root.js
var Root = __webpack_require__(23);
var Root_default = /*#__PURE__*/__webpack_require__.n(Root);

// CONCATENATED MODULE: ./src/main.jsx






var main_store = configureStore();
application.dispatch = main_store.dispatch;
var appContainer = document.getElementById("extensions-container");
application.init();
Object(external_window_dnn_nodeModules_ReactDOM_["render"])(
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(external_window_dnn_nodeModules_ReactRedux_["Provider"], {
  store: main_store
},
/*#__PURE__*/
external_window_dnn_nodeModules_React_default.a.createElement(Root_default.a, null)), appContainer);

/***/ })
/******/ ]);
//# sourceMappingURL=extensions-bundle.js.map