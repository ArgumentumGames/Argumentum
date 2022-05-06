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
/******/ 	return __webpack_require__(__webpack_require__.s = 19);
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
  module.exports = __webpack_require__(21)();
}


/***/ }),
/* 2 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.CommonComponents;

/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (true) {
  module.exports = __webpack_require__(20);
} else { var jsFeaturesPresent, evalAllowed; }


/***/ }),
/* 4 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();


var Localization = {
  get: function get(key) {
    var moduleName = "SiteGroups";
    return utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].getResx(moduleName, key);
  }
};
var _default = Localization;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Localization, "Localization", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\localization\\index.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\localization\\index.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 5 */
/***/ (function(module, exports) {

module.exports = function(originalModule) {
	if (!originalModule.webpackPolyfill) {
		var module = Object.create(originalModule);
		// module.parent = undefined by default
		if (!module.children) module.children = [];
		Object.defineProperty(module, "loaded", {
			enumerable: true,
			get: function() {
				return module.l;
			}
		});
		Object.defineProperty(module, "id", {
			enumerable: true,
			get: function() {
				return module.i;
			}
		});
		Object.defineProperty(module, "exports", {
			enumerable: true
		});
		module.webpackPolyfill = 1;
	}
	return module;
};


/***/ }),
/* 6 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

var _default = _extends({}, window.dnn.utility);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\utils\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 7 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }



var SiteGroupsService =
/*#__PURE__*/
function () {
  function SiteGroupsService() {
    _classCallCheck(this, SiteGroupsService);
  }

  _createClass(SiteGroupsService, [{
    key: "getServiceFramework",
    value: function getServiceFramework() {
      var sf = utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = "SiteGroups";
      return sf;
    }
  }, {
    key: "getSiteGroups",
    value: function getSiteGroups() {
      var sf = this.getServiceFramework();
      return sf.get("GetSiteGroups", {});
    }
  }, {
    key: "getUnassignedSites",
    value: function getUnassignedSites() {
      var sf = this.getServiceFramework();
      return sf.get("GetAvailablePortals", {});
    }
  }, {
    key: "save",
    value: function save(siteGroup) {
      var sf = this.getServiceFramework();
      return sf.post("Save", siteGroup);
    }
  }, {
    key: "delete",
    value: function _delete(siteGroupId) {
      var sf = this.getServiceFramework();
      return sf.post("Delete?groupId=" + siteGroupId, {});
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return SiteGroupsService;
}();

var siteGroupsService = new SiteGroupsService();
var _default = siteGroupsService;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(SiteGroupsService, "SiteGroupsService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\services\\SiteGroupsService.js");
  reactHotLoader.register(siteGroupsService, "siteGroupsService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\services\\SiteGroupsService.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\services\\SiteGroupsService.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 8 */
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
/* 9 */
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

var	fixUrls = __webpack_require__(24);

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
/* 10 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SiteGroupRow; });
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _localization__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _Editor__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(16);
/* harmony import */ var _Row_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(27);
/* harmony import */ var _Row_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_Row_less__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }








var SiteGroupRow =
/*#__PURE__*/
function (_React$Component) {
  _inherits(SiteGroupRow, _React$Component);

  function SiteGroupRow(props) {
    var _this;

    _classCallCheck(this, SiteGroupRow);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(SiteGroupRow).call(this, props));
    _this.state = {
      PortalGroupName: props.group.PortalGroupName || "",
      AuthenticationDomain: props.group.AuthenticationDomain,
      Description: props.group.Description,
      Portals: JSON.parse(JSON.stringify(props.group.Portals)) || [],
      UnassignedSites: JSON.parse(JSON.stringify(props.unassignedSites || [])).filter(function (site) {
        return site.PortalId !== _this.props.group.MasterPortal.PortalId;
      }),
      errors: {
        groupName: false
      }
    };
    _this.submitted = false;
    return _this;
  }

  _createClass(SiteGroupRow, [{
    key: "componentWillReceiveProps",
    value: function componentWillReceiveProps(newProps) {
      this.resetState(newProps);
    }
  }, {
    key: "clickOnPortal",
    value: function clickOnPortal(index, type) {
      if (type === "assignedPortals") {
        var p = JSON.parse(JSON.stringify(this.state.Portals));
        p[index].selected = !p[index].selected;
        this.setState({
          Portals: p
        });
      } else {
        var _p = JSON.parse(JSON.stringify(this.state.UnassignedSites));

        _p[index].selected = !_p[index].selected;
        this.setState({
          UnassignedSites: _p
        });
      }
    }
  }, {
    key: "moveItemsLeft",
    value: function moveItemsLeft() {
      var _this2 = this;

      var assignedPortals = JSON.parse(JSON.stringify(this.state.Portals));
      var unassignedPortals = JSON.parse(JSON.stringify(this.state.UnassignedSites)).filter(function (site) {
        return site.PortalId !== _this2.props.group.MasterPortal.PortalId;
      });
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
        this.setState({
          UnassignedSites: unassignedPortals.concat(itemsToMove),
          Portals: itemsToStay
        });
      }
    }
  }, {
    key: "moveItemsRight",
    value: function moveItemsRight() {
      var assignedPortals = JSON.parse(JSON.stringify(this.state.Portals));
      var unassignedPortals = JSON.parse(JSON.stringify(this.state.UnassignedSites));
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
        this.setState({
          UnassignedSites: itemsToStay,
          Portals: assignedPortals.concat(itemsToMove)
        });
      }
    }
  }, {
    key: "moveAll",
    value: function moveAll(direction) {
      var assignedPortals = JSON.parse(JSON.stringify(this.state.Portals));
      var unassignedPortals = JSON.parse(JSON.stringify(this.state.UnassignedSites));

      switch (direction) {
        case "right":
          this.setState({
            UnassignedSites: [],
            Portals: assignedPortals.concat(unassignedPortals)
          });
          break;

        default:
          this.setState({
            UnassignedSites: unassignedPortals.concat(assignedPortals),
            Portals: []
          });
          break;
      }
    }
  }, {
    key: "sortPortals",
    value: function sortPortals(a, b) {
      return a.PortalName < b.PortalName ? -1 : 1;
    }
  }, {
    key: "isNew",
    value: function isNew() {
      return this.props.group.PortalGroupId === -1;
    }
  }, {
    key: "save",
    value: function save() {
      this.submitted = true;
      if (this.isValid()) this.props.onSave(this.result());
    }
  }, {
    key: "cancel",
    value: function cancel() {
      this.resetState(this.props);
      this.props.onCancelEditing();
    }
  }, {
    key: "resetState",
    value: function resetState(props) {
      this.setState({
        PortalGroupName: props.group.PortalGroupName || "",
        AuthenticationDomain: props.group.AuthenticationDomain,
        Description: props.group.Description,
        Portals: JSON.parse(JSON.stringify(props.group.Portals)),
        UnassignedSites: JSON.parse(JSON.stringify(props.unassignedSites)),
        errors: {
          groupName: false,
          authenticationDomain: false
        }
      });
    }
  }, {
    key: "isValid",
    value: function isValid() {
      var valid = true;

      if (this.submitted) {
        var PortalGroupName = this.state.PortalGroupName;
        var errors = this.state.errors;
        errors.groupName = false;

        if (PortalGroupName === "") {
          errors.groupName = true;
          valid = false;
        }

        this.setState({
          errors: errors
        });
      }

      return valid;
    }
  }, {
    key: "result",
    value: function result() {
      return {
        PortalGroup: {
          PortalGroupId: this.props.group.PortalGroupId,
          AuthenticationDomain: this.state.AuthenticationDomain,
          PortalGroupName: this.state.PortalGroupName,
          MasterPortal: this.props.group.MasterPortal,
          Portals: this.state.Portals,
          Description: this.state.Description
        },
        UnassignedSites: this.state.UnassignedSites
      };
    }
  }, {
    key: "render",
    value: function render() {
      var _this3 = this;

      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "row " + this.props.isOpened
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "rowHeader"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 45
        }, this.props.group.PortalGroupName),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 45
        }, this.props.group.MasterPortal.PortalName),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 10
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["IconButton"], {
          type: "Edit",
          className: "edit-icon " + !this.props.isOpened,
          onClick: function onClick() {
            return _this3.props.isOpened ? _this3.props.onCancelEditing() : _this3.props.onEditGroup(_this3.props.group);
          },
          title: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("Edit.Button")
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Collapsible"], {
          isOpened: this.props.isOpened
        }, this.props.isOpened &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_Editor__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
          group: this.props.group,
          portalGroupName: this.state.PortalGroupName,
          authenticationDomain: this.state.AuthenticationDomain,
          description: this.state.Description,
          errors: this.state.errors,
          unassignedSites: this.state.UnassignedSites,
          portals: this.state.Portals,
          isNew: this.isNew(),
          onCancel: function onCancel() {
            return _this3.cancel();
          },
          onDeleteGroup: function onDeleteGroup(group) {
            return _this3.props.onDeleteGroup(group);
          },
          onSave: function onSave() {
            return _this3.save();
          },
          onAuthenticationDomainChanged: function onAuthenticationDomainChanged(value) {
            _this3.setState({
              AuthenticationDomain: value
            });
          },
          onDescriptionChanged: function onDescriptionChanged(value) {
            _this3.setState({
              Description: value
            });
          },
          onGroupNameChanged: function onGroupNameChanged(value) {
            _this3.setState({
              PortalGroupName: value
            });

            _this3.isValid();
          },
          onClickOnPortal: function onClickOnPortal(i, t) {
            return _this3.clickOnPortal(i, t);
          },
          onMoveItemsLeft: function onMoveItemsLeft() {
            return _this3.moveItemsLeft();
          },
          onMoveItemsRight: function onMoveItemsRight() {
            return _this3.moveItemsRight();
          },
          onMoveAll: function onMoveAll(d) {
            return _this3.moveAll(d);
          }
        })))
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return SiteGroupRow;
}(react__WEBPACK_IMPORTED_MODULE_1___default.a.Component);


SiteGroupRow.propTypes = {
  unassignedSites: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  group: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.object,
  onDeleteGroup: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onSave: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onEditGroup: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onCancelEditing: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  isOpened: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.bool
};
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(SiteGroupRow, "SiteGroupRow", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\components\\Row.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 11 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactCustomScrollBars;

/***/ }),
/* 12 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 13 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SiteGroupApp; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _NewGroup__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(14);
/* harmony import */ var _Table__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(15);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _services_SiteGroupsService__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(7);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(6);
/* harmony import */ var _localization__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(4);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }









var SiteGroupApp =
/*#__PURE__*/
function (_React$Component) {
  _inherits(SiteGroupApp, _React$Component);

  function SiteGroupApp(props) {
    var _this;

    _classCallCheck(this, SiteGroupApp);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(SiteGroupApp).call(this, props));
    _this.state = {
      groups: [],
      currentGroup: null,
      unassignedSites: []
    };
    return _this;
  }

  _createClass(SiteGroupApp, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      this.loadState();
    }
  }, {
    key: "loadState",
    value: function loadState() {
      var self = this;
      _services_SiteGroupsService__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].getSiteGroups().then(function (groups) {
        return self.setState({
          groups: groups
        });
      });
      _services_SiteGroupsService__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].getUnassignedSites().then(function (sites) {
        return self.setState({
          unassignedSites: sites
        });
      });
    }
  }, {
    key: "editNewGroup",
    value: function editNewGroup(id) {
      var site = this.state.unassignedSites.find(function (s) {
        return s.PortalId === id;
      });
      this.setState({
        currentGroup: {
          PortalGroupId: -1,
          MasterPortal: site,
          PortalGroupName: site.PortalName + " " + _localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Group"),
          AuthenticationDomain: "",
          Portals: []
        }
      });
    }
  }, {
    key: "save",
    value: function save(r) {
      var self = this;
      var group = r.PortalGroup;
      var unassignedSites = r.UnassignedSites;
      var isNewGroup = group.PortalGroupId === -1;
      _services_SiteGroupsService__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].save(group).then(function (id) {
        if (isNewGroup) group.PortalGroupId = id;
        var groups = (isNewGroup ? self.state.groups : self.state.groups.filter(function (g) {
          return g.PortalGroupId !== group.PortalGroupId;
        })).concat([group]).sort(function (a, b) {
          return a.PortalGroupName < b.PortalGroupName ? -1 : 1;
        });
        self.setState({
          unassignedSites: unassignedSites,
          currentGroup: null,
          groups: groups
        });
      });
    }
  }, {
    key: "deleteGroup",
    value: function deleteGroup(group) {
      var self = this;
      _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].confirm(_localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("DeleteGroup.Confirm"), _localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Delete"), _localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Cancel"), function () {
        _services_SiteGroupsService__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].delete(group.PortalGroupId).then(function () {
          self.setState({
            unassignedSites: self.state.unassignedSites.concat(group.Portals).concat([group.MasterPortal]).sort(function (a, b) {
              return a.PortalName < b.PortalName ? -1 : 1;
            }),
            groups: self.state.groups.filter(function (g) {
              return g.PortalGroupId !== group.PortalGroupId;
            }),
            currentGroup: null
          });
        });
      });
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPageHeader"], {
          title: _localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("nav_SiteGroups")
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_NewGroup__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"], {
          unassignedSites: this.state.currentGroup ? [] : this.state.unassignedSites,
          disabled: this.state.currentGroup != null,
          onNewGroup: function onNewGroup(siteId) {
            return _this2.editNewGroup(Number(siteId));
          }
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_Table__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"], {
          groups: this.state.groups,
          unassignedSites: this.state.unassignedSites,
          currentGroup: this.state.currentGroup,
          onEditGroup: function onEditGroup(group) {
            return _this2.setState({
              currentGroup: group
            });
          },
          onCancelEditing: function onCancelEditing() {
            return _this2.setState({
              currentGroup: null
            });
          },
          onDeleteGroup: function onDeleteGroup(group) {
            return _this2.deleteGroup(group);
          },
          onSave: function onSave(result) {
            return _this2.save(result);
          }
        }))
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return SiteGroupApp;
}(react__WEBPACK_IMPORTED_MODULE_0___default.a.Component);


;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(SiteGroupApp, "SiteGroupApp", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\components\\App.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 14 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return NewSiteGroup; });
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _localization__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }






var NewSiteGroup =
/*#__PURE__*/
function (_React$Component) {
  _inherits(NewSiteGroup, _React$Component);

  function NewSiteGroup(props) {
    var _this;

    _classCallCheck(this, NewSiteGroup);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(NewSiteGroup).call(this, props));
    _this.state = {
      siteId: null
    };
    return _this;
  }

  _createClass(NewSiteGroup, [{
    key: "onNew",
    value: function onNew() {
      this.props.onNewGroup(this.state.siteId);
      this.setState({
        siteId: null
      });
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      if (this.props.disabled && this.state.sitedId) this.setState({
        siteId: null
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          style: {
            float: "right"
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("select", {
          name: "sites",
          style: {
            padding: "12px",
            marginRight: "12px"
          },
          onChange: function onChange(e) {
            return _this2.setState({
              siteId: e.target.value === "NoSiteSelected" ? null : e.target.value
            });
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("option", {
          value: "NoSiteSelected"
        }, _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("ChooseASite.Label")), (this.props.unassignedSites || []).map(function (site) {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("option", {
              key: site.PortalId.toString(),
              value: site.PortalId
            }, site.PortalName)
          );
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "primary",
          size: "large",
          disabled: !this.state.siteId || this.props.disabled,
          onClick: function onClick() {
            return _this2.onNew();
          }
        }, _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("NewSiteGroup.Button")))
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return NewSiteGroup;
}(react__WEBPACK_IMPORTED_MODULE_1___default.a.Component);


NewSiteGroup.propTypes = {
  unassignedSites: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  onNewGroup: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  disabled: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.bool
};
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(NewSiteGroup, "NewSiteGroup", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\components\\NewGroup.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 15 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SiteGroupsTable; });
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _localization__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(4);
/* harmony import */ var _Row__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(10);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _Table_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(29);
/* harmony import */ var _Table_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_Table_less__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }







var tableFields = [{
  name: "GroupName",
  width: 45
}, {
  name: "MasterSite",
  width: 45
}, {
  name: "",
  width: 10
}];

var SiteGroupsTable =
/*#__PURE__*/
function (_React$Component) {
  _inherits(SiteGroupsTable, _React$Component);

  function SiteGroupsTable(props) {
    var _this;

    _classCallCheck(this, SiteGroupsTable);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(SiteGroupsTable).call(this, props));
    _this.state = {
      renderIndex: -1
    };
    return _this;
  }

  _createClass(SiteGroupsTable, [{
    key: "renderHeader",
    value: function renderHeader() {
      var tableHeaders = tableFields.map(function (field) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
            key: field.name,
            columnSize: field.width,
            style: {
              fontWeight: "bolder"
            }
          }, field.name !== "" ?
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get(field.name + ".Header")) :
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, "\xA0"))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          id: "sitegroups-header-row",
          className: "sitegroups-header-row"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderList",
    value: function renderList() {
      var _this2 = this;

      var existingGroupRows = (this.props.groups || []).map(function (group) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_Row__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"], {
            group: group,
            key: "PG_" + group.PortalGroupId.toString(),
            isOpened: _this2.props.currentGroup && _this2.props.currentGroup.PortalGroupId === group.PortalGroupId || false,
            unassignedSites: (_this2.props.unassignedSites || []).filter(function (site) {
              return site.PortalId !== group.MasterPortal.PortalId;
            }),
            onEditGroup: function onEditGroup(g) {
              return _this2.props.onEditGroup(g);
            },
            onSave: function onSave(g) {
              return _this2.props.onSave(g);
            },
            onCancelEditing: function onCancelEditing() {
              return _this2.props.onCancelEditing();
            },
            onDeleteGroup: function onDeleteGroup(g) {
              return _this2.props.onDeleteGroup(g);
            }
          })
        );
      });
      var newRow = this.props.currentGroup && this.props.currentGroup.PortalGroupId === -1 &&
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_Row__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"], {
        isOpened: true,
        key: "PG_new",
        group: this.props.currentGroup,
        unassignedSites: (this.props.unassignedSites || []).filter(function (site) {
          return site.PortalId !== _this2.props.currentGroup.MasterPortal.PortalId;
        }),
        onEditGroup: function onEditGroup(g) {
          return _this2.props.onEditGroup(g);
        },
        onSave: function onSave(g) {
          return _this2.props.onSave(g);
        },
        onCancelEditing: function onCancelEditing() {
          return _this2.props.onCancelEditing();
        },
        onDeleteGroup: function onDeleteGroup(g) {
          return _this2.props.onDeleteGroup(g);
        }
      });
      var rows = newRow ? [newRow].concat(existingGroupRows) : existingGroupRows;

      if (rows.length > 0) {
        return rows;
      } else {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
            className: "no-sitegroups-row"
          }, _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("NoData"))
        );
      }
    }
  }, {
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "sitegroups-list-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "container"
        }, this.renderHeader(), this.renderList()))
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return SiteGroupsTable;
}(react__WEBPACK_IMPORTED_MODULE_1___default.a.Component);


SiteGroupsTable.propTypes = {
  groups: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  unassignedSites: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  currentGroup: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.object,
  onEditGroup: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onDeleteGroup: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onCancelEditing: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onSave: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func
};
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(tableFields, "tableFields", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\components\\Table.jsx");
  reactHotLoader.register(SiteGroupsTable, "SiteGroupsTable", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\components\\Table.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 16 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return SiteGroupEditor; });
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _localization__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _AssignedSelector__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(17);
/* harmony import */ var _Editor_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(25);
/* harmony import */ var _Editor_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_Editor_less__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }








var SiteGroupEditor =
/*#__PURE__*/
function (_React$Component) {
  _inherits(SiteGroupEditor, _React$Component);

  function SiteGroupEditor(props) {
    _classCallCheck(this, SiteGroupEditor);

    return _possibleConstructorReturn(this, _getPrototypeOf(SiteGroupEditor).call(this, props));
  }

  _createClass(SiteGroupEditor, [{
    key: "render",
    value: function render() {
      var _this = this;

      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "sitegroup-details-editor"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridSystem"], {
          numberOfColums: 2
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "editor-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          value: this.props.group.MasterPortal.PortalName,
          enabled: false,
          label: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("MasterSite.Label"),
          tooltipMessage: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("MasterSite.Help"),
          inputStyle: {
            marginBottom: 15
          },
          tabIndex: 1
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          value: this.props.portalGroupName,
          enabled: true,
          onChange: function onChange(e) {
            return _this.props.onGroupNameChanged(e.target.value);
          },
          maxLength: 50,
          error: this.props.errors.groupName,
          label: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("GroupName.Label"),
          tooltipMessage: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("GroupName.Help"),
          errorMessage: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("GroupName.Required"),
          autoComplete: "off",
          inputStyle: {
            marginBottom: 15
          },
          tabIndex: 2
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "editor-container  right-column"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          value: this.props.authenticationDomain,
          enabled: true,
          onChange: function onChange(e) {
            return _this.props.onAuthenticationDomainChanged(e.target.value);
          },
          maxLength: 50,
          label: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("AuthenticationDomain.Label"),
          tooltipMessage: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("AuthenticationDomain.Help"),
          autoComplete: "off",
          inputStyle: {
            marginBottom: 15
          },
          tabIndex: 3
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "editor-row divider"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["MultiLineInputWithError"], {
          value: this.props.description,
          enabled: true,
          onChange: function onChange(e) {
            return _this.props.onDescriptionChanged(e.target.value);
          },
          maxLength: 50,
          label: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("Description.Label"),
          tooltipMessage: _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("Description.Help"),
          autoComplete: "off",
          inputStyle: {
            marginBottom: 15
          },
          tabIndex: 4
        })))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "selector-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_AssignedSelector__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
          assignedPortals: this.props.portals,
          unassignedPortals: this.props.unassignedSites,
          onClickOnPortal: function onClickOnPortal(i, t) {
            return _this.props.onClickOnPortal(i, t);
          },
          moveItemsLeft: function moveItemsLeft() {
            return _this.props.onMoveItemsLeft();
          },
          moveItemsRight: function moveItemsRight() {
            return _this.props.onMoveItemsRight();
          },
          moveAll: function moveAll(direction) {
            return _this.props.onMoveAll(direction);
          }
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "buttons-box"
        }, !this.props.isNew &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "secondary",
          onClick: function onClick() {
            return _this.props.onDeleteGroup(_this.props.group);
          }
        }, _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("Delete.Button")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "secondary",
          onClick: function onClick() {
            return _this.props.onCancel();
          }
        }, _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("Cancel.Button")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "primary",
          onClick: function onClick() {
            return _this.props.onSave();
          }
        }, _localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("Save.Button"))))
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return SiteGroupEditor;
}(react__WEBPACK_IMPORTED_MODULE_1___default.a.Component);


SiteGroupEditor.propTypes = {
  portalGroupName: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  errors: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.object,
  authenticationDomain: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  description: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  portals: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  group: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.object,
  unassignedSites: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  onCancel: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onDeleteGroup: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onSave: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onGroupNameChanged: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onDescriptionChanged: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onAuthenticationDomainChanged: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onClickOnPortal: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onMoveItemsLeft: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onMoveItemsRight: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  onMoveAll: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  isNew: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.bool
};
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(SiteGroupEditor, "SiteGroupEditor", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\components\\Editor.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 17 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_custom_scrollbars__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(11);
/* harmony import */ var react_custom_scrollbars__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_custom_scrollbars__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(4);
/* harmony import */ var _AssignedSelector_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(18);
/* harmony import */ var _AssignedSelector_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_AssignedSelector_less__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }








var AssignedSelector =
/*#__PURE__*/
function (_React$Component) {
  _inherits(AssignedSelector, _React$Component);

  function AssignedSelector(props) {
    _classCallCheck(this, AssignedSelector);

    return _possibleConstructorReturn(this, _getPrototypeOf(AssignedSelector).call(this, props));
  }

  _createClass(AssignedSelector, [{
    key: "getPortalList",
    value: function getPortalList(list, type) {
      var _this = this;

      return list.map(function (portal, index) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("li", {
            className: portal.selected ? "selected" : "",
            key: portal.PortalId.toString(),
            onClick: function onClick() {
              return _this.props.onClickOnPortal(index, type);
            }
          }, portal.PortalName)
        );
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      var assignedPortals = this.getPortalList(this.props.assignedPortals, "assignedPortals");
      var unassignedPortals = this.getPortalList(this.props.unassignedPortals, "unassignedPortals");
      var leftSelected = this.props.unassignedPortals.find(function (p) {
        return p.selected;
      }) != null;
      var rightSelected = this.props.assignedPortals.find(function (p) {
        return p.selected;
      }) != null;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          className: _AssignedSelector_less__WEBPACK_IMPORTED_MODULE_5___default.a.assignedSelector
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 45,
          className: "selector-box"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("h6", null, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("EditModule_Unassigned.Label")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(react_custom_scrollbars__WEBPACK_IMPORTED_MODULE_2__["Scrollbars"], {
          style: {
            width: "100%",
            height: "100%",
            border: "1px solid #c8c8c8"
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("ul", null, unassignedPortals))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 10,
          className: "selector-controls"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          href: "",
          className: "move-item single-right" + (leftSelected ? " enabled" : ""),
          onClick: function onClick() {
            return _this2.props.moveItemsRight();
          },
          dangerouslySetInnerHTML: {
            __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SvgIcons"].ArrowRightIcon
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          href: "",
          className: "move-item single-left" + (rightSelected ? " enabled" : ""),
          onClick: function onClick() {
            return _this2.props.moveItemsLeft();
          },
          dangerouslySetInnerHTML: {
            __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SvgIcons"].ArrowLeftIcon
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          href: "",
          className: "move-item double-right" + (unassignedPortals.length > 0 ? " enabled" : ""),
          onClick: function onClick() {
            return _this2.props.moveAll("right");
          },
          dangerouslySetInnerHTML: {
            __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SvgIcons"].DoubleArrowRightIcon
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          href: "",
          className: "move-item double-left" + (assignedPortals.length > 0 ? " enabled" : ""),
          onClick: function onClick() {
            return _this2.props.moveAll("left");
          },
          dangerouslySetInnerHTML: {
            __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SvgIcons"].DoubleArrowLeftIcon
          }
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 45,
          className: "selector-box"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("h6", null, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("EditModule_Assigned.Label")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(react_custom_scrollbars__WEBPACK_IMPORTED_MODULE_2__["Scrollbars"], {
          style: {
            width: "100%",
            height: "100%",
            border: "1px solid #c8c8c8"
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("ul", null, assignedPortals))))
      );
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return AssignedSelector;
}(react__WEBPACK_IMPORTED_MODULE_1___default.a.Component);

AssignedSelector.propTypes = {
  assignedPortals: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  unassignedPortals: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  onClickOnPortal: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  moveItemsRight: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  moveItemsLeft: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  moveAll: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func
};
var _default = AssignedSelector;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(AssignedSelector, "AssignedSelector", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\components\\AssignedSelector.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\components\\AssignedSelector.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 18 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(23);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(9)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 19 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(12);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _components_App__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(13);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();




var appContainer = document.getElementById("sitegroups-container");
Object(react_dom__WEBPACK_IMPORTED_MODULE_1__["render"])(
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_components_App__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"], null), appContainer);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(appContainer, "appContainer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteGroups.Web\\src\\main.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(5)(module)))

/***/ }),
/* 20 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
function _interopDefault(e){return e&&"object"==typeof e&&"default"in e?e.default:e}Object.defineProperty(exports,"__esModule",{value:!0});var React=_interopDefault(__webpack_require__(0));function AppContainer(e){return React.Children.only(e.children)}var hot_prod=function(){return function(e){return e}},areComponentsEqual=function(e,n){return e===n},setConfig=function(){},cold=function(e){return e},configureComponent=function(){};exports.AppContainer=AppContainer,exports.hot=hot_prod,exports.areComponentsEqual=areComponentsEqual,exports.setConfig=setConfig,exports.cold=cold,exports.configureComponent=configureComponent;


/***/ }),
/* 21 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(22);

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
/* 22 */
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
/* 23 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(8)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr {\n  padding: 0;\n  margin-top: 32px;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-box {\n  padding: 0;\n  height: 225px;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-box h6 {\n  margin-bottom: 5px;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-box ul li {\n  padding: 15px 20px;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-box ul li:hover {\n  background-color: #EFF0F0;\n  cursor: pointer;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-box ul li.selected {\n  background-color: #21A3DA;\n  color: #fff;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-controls {\n  padding: 0 22px;\n  margin-top: 21px;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-controls .move-item {\n  width: 30px;\n  height: 30px;\n  padding: 7px;\n  border: 1px solid #c8c8c8;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-controls .move-item.double-right {\n  margin-top: 105px;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-controls .move-item.enabled {\n  border-color: #21A3DA;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-controls .move-item.enabled svg path {\n  fill: #21A3DA;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-controls .move-item.enabled svg g polygon {\n  fill: #21A3DA;\n}\n.sitegroup-details-editor ._1w3RqW2RengAM2eVieeDVr .selector-controls .move-item.enabled:hover {\n  background-color: #EFF0F0;\n  cursor: pointer;\n}\n", ""]);

// Exports
exports.locals = {
	"assignedSelector": "_1w3RqW2RengAM2eVieeDVr"
};

/***/ }),
/* 24 */
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
/* 25 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(26);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(9)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 26 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(8)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.sitegroup-details-editor {\n  padding: 10px 0;\n  background-color: #FFFFFF;\n  display: block;\n  float: left;\n}\n.sitegroup-details-editor .selector-container {\n  padding: 0 25px;\n}\n.sitegroup-details-editor .editor-container {\n  float: left;\n  padding: 10px 25px;\n  width: 100%;\n  box-sizing: border-box;\n}\n.sitegroup-details-editor .editor-container.right-column {\n  border-left: 1px solid #C8C8C8;\n}\n.sitegroup-details-editor .editor-container .title-row {\n  width: 100%;\n  float: left;\n  font-weight: bold;\n  text-transform: uppercase;\n  margin-bottom: 10px;\n}\n.sitegroup-details-editor .editor-container .status-row {\n  width: 100%;\n  float: left;\n  margin-bottom: 10px;\n}\n.sitegroup-details-editor .editor-container .status-row .left {\n  float: left;\n  width: 50%;\n}\n.sitegroup-details-editor .editor-container .status-row .left .dnn-label {\n  float: left;\n}\n.sitegroup-details-editor .editor-container .status-row .left .dnn-label label {\n  float: left;\n}\n.sitegroup-details-editor .editor-container .status-row .right {\n  float: right;\n  width: 50%;\n  text-align: right;\n}\n.sitegroup-details-editor .editor-container .status-row .right .checkbox {\n  float: right;\n}\n.sitegroup-details-editor .editor-container .editor-row {\n  float: left;\n  width: 100%;\n}\n.sitegroup-details-editor .editor-container .editor-row select.full {\n  float: left;\n  width: 100%;\n}\n.sitegroup-details-editor .editor-container .editor-row select.two-fifth {\n  float: left;\n  width: 40%;\n}\n.sitegroup-details-editor .editor-container .editor-row select.one-fourth {\n  float: left;\n  width: 25%;\n}\n.sitegroup-details-editor .editor-container .editor-row .text-section {\n  width: 5%;\n  float: left;\n  text-align: center;\n  vertical-align: middle;\n  margin-top: 9px;\n}\n.sitegroup-details-editor .editor-container .editor-row .dnn-single-line-input-with-error,\n.sitegroup-details-editor .editor-container .editor-row .dnn-multi-line-input-with-error,\n.sitegroup-details-editor .editor-container .editor-row input {\n  width: 100%;\n}\n.sitegroup-details-editor .editor-container .editor-row .dnn-multi-line-input-with-error textarea {\n  min-height: 0;\n}\n.sitegroup-details-editor .editor-container .editor-row.divider {\n  padding-bottom: 15px;\n}\n.sitegroup-details-editor .editor-container .editor-row.divider .dnn-uicommon-select,\n.sitegroup-details-editor .editor-container .editor-row.divider .dnn-ui-common-single-line-input {\n  border-radius: 0px;\n}\n.sitegroup-details-editor .editor-container .editor-row.divider .dnn-ui-common-multi-line-input {\n  padding: 8px 16px 62px;\n  border-radius: 0px;\n}\n.sitegroup-details-editor .editor-container .editor-row.divider .new-group-container {\n  position: absolute;\n  z-index: 999;\n}\n.sitegroup-details-editor .editor-container .editor-row.divider .dnn-label {\n  float: left;\n}\n.sitegroup-details-editor .editor-container .editor-row.divider .dnn-label label {\n  float: left;\n}\n.sitegroup-details-editor .buttons-box {\n  width: 100%;\n  text-align: center;\n  float: left;\n  margin: 15px 0;\n}\n.sitegroup-details-editor .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n.sitegroup-details-editor .buttons-box .edit-icon {\n  margin: 0px 10px 20px 10px;\n  float: right;\n}\n.sitegroup-details-editor .buttons-box .edit-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n}\n", ""]);



/***/ }),
/* 27 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(28);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(9)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(8)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.sitegroups-list-container .row {\n  display: table;\n  width: 100%;\n  border-bottom: 1px solid #C8C8C8;\n  cursor: auto;\n}\n.sitegroups-list-container .row > div {\n  float: left;\n  width: 100%;\n}\n.sitegroups-list-container .row > div > div {\n  float: left;\n  width: 100%;\n}\n.sitegroups-list-container .row.true {\n  border-top: 2px solid #1E88C3;\n  border-bottom: 2px solid #1E88C3;\n  margin-top: -1px;\n}\n.sitegroups-list-container .row div.rowHeader {\n  color: #6F7273;\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 18px;\n  box-sizing: border-box;\n  cursor: auto;\n}\n.sitegroups-list-container .row div.rowHeader .edit-icon {\n  margin-left: 5px;\n  float: right;\n  cursor: pointer;\n}\n.sitegroups-list-container .row div.rowHeader .edit-icon svg {\n  width: 18px;\n  float: left;\n  height: 18px;\n}\n.sitegroups-list-container .row div.rowHeader .edit-icon.false svg {\n  fill: #1E88C3;\n}\n.sitegroups-list-container .row div.rowHeader .edit-icon.false svg:hover {\n  fill: #4B4E4F;\n}\n.sitegroups-list-container .row div.rowHeader .icon-flat {\n  display: inline-block;\n  width: 18px;\n  height: 18px;\n  vertical-align: middle;\n}\n.sitegroups-list-container .row div.rowHeader .icon-flat svg {\n  fill: #4B4E4F;\n}\n.sitegroups-list-container .row div.ReactCollapse--content {\n  padding: 5px;\n}\n", ""]);



/***/ }),
/* 29 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(30);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(9)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 30 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(8)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.sitegroups-list-container {\n  margin-top: 100px;\n  width: 100%;\n  box-sizing: border-box;\n  padding: 20px;\n  display: table;\n  float: left;\n}\n.sitegroups-list-container > div {\n  padding: 0;\n}\n.sitegroups-list-container * {\n  box-sizing: border-box;\n}\n.sitegroups-list-container .container {\n  float: left;\n  width: 100%;\n  margin-bottom: 20px;\n  border-left: 1px solid #C8C8C8;\n  border-right: 1px solid #C8C8C8;\n  background-color: white;\n}\n.sitegroups-list-container .container .no-sitegroups-row {\n  width: 100%;\n  float: left;\n  text-align: center;\n  font-weight: bold;\n  padding: 13px 20px;\n  box-sizing: border-box;\n  border-bottom: 1px solid #C8C8C8;\n}\n.sitegroups-list-container .container .sitegroups-header-row {\n  display: table;\n  border-bottom: 1px solid #C8C8C8;\n  border-top: 1px solid #C8C8C8;\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 20px;\n  box-sizing: border-box;\n  text-transform: uppercase;\n}\n", ""]);



/***/ })
/******/ ]);
//# sourceMappingURL=site-groups-bundle.js.map