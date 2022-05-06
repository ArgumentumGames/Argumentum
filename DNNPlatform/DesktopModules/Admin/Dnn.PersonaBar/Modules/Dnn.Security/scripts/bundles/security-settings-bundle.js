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
/******/ 	return __webpack_require__(__webpack_require__.s = 60);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.React;

/***/ }),
/* 1 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();


var resx = {
  get: function get(key) {
    var moduleName = _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].moduleName;
    return _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].utilities.getResx(moduleName, key);
  }
};
var _default = resx;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(resx, "resx", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\resources\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\resources\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 2 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.CommonComponents;

/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (true) {
  module.exports = __webpack_require__(61);
} else { var jsFeaturesPresent, evalAllowed; }


/***/ }),
/* 4 */
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
  module.exports = __webpack_require__(66)();
}


/***/ }),
/* 5 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _pagination__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(27);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _pagination__WEBPACK_IMPORTED_MODULE_0__["a"]; });

/* harmony import */ var _visiblePanel__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(28);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "c", function() { return _visiblePanel__WEBPACK_IMPORTED_MODULE_1__["a"]; });

/* harmony import */ var _security__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(29);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _security__WEBPACK_IMPORTED_MODULE_2__["a"]; });






/***/ }),
/* 6 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _pagination__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(30);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _pagination__WEBPACK_IMPORTED_MODULE_0__["a"]; });

/* harmony import */ var _visiblePanel__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(31);
/* harmony import */ var _security__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(32);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _security__WEBPACK_IMPORTED_MODULE_2__["a"]; });






/***/ }),
/* 7 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var utils = {
  init: function init(utilities) {
    if (!utilities) {
      throw new Error("Utilities is undefined.");
    }

    this.utilities = utilities;
  },
  utilities: null
};
var _default = utils;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(utils, "utils", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\utils\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\utils\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 8 */
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
/* 9 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactRedux;

/***/ }),
/* 10 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }



function serializeQueryStringParameters(obj) {
  var s = [];

  for (var p in obj) {
    if (Object.prototype.hasOwnProperty.call(obj, p)) {
      s.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
    }
  }

  return s.join("&");
}

var ApplicationService =
/*#__PURE__*/
function () {
  function ApplicationService() {
    _classCallCheck(this, ApplicationService);
  }

  _createClass(ApplicationService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(controller) {
      var sf = _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].utilities.sf;
      sf.moduleRoot = "PersonaBar";
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getIpFilters",
    value: function getIpFilters(callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetIpFilters", {}, callback);
    }
  }, {
    key: "getIpFilter",
    value: function getIpFilter(searchParameters, callback) {
      var sf = this.getServiceFramework("Security");
      searchParameters = Object.assign({}, searchParameters, {});
      sf.get("GetIpFilter?" + serializeQueryStringParameters(searchParameters), {}, callback);
    }
  }, {
    key: "deleteIpFilter",
    value: function deleteIpFilter(filterId, callback, failureCallback) {
      var sf = this.getServiceFramework("Security");
      sf.post("DeleteIpFilter?filterId=" + filterId, {}, callback, failureCallback);
    }
  }, {
    key: "getBasicLoginSettings",
    value: function getBasicLoginSettings(cultureCode, callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetBasicLoginSettings?cultureCode=" + cultureCode, {}, callback);
    }
  }, {
    key: "updateBasicLoginSettings",
    value: function updateBasicLoginSettings(payload, callback) {
      var sf = this.getServiceFramework("Security");
      sf.post("UpdateBasicLoginSettings", payload, callback);
    }
  }, {
    key: "updateIpFilter",
    value: function updateIpFilter(payload, callback) {
      var sf = this.getServiceFramework("Security");
      sf.post("UpdateIpFilter", payload, callback);
    }
  }, {
    key: "getMemberSettings",
    value: function getMemberSettings(callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetMemberSettings", {}, callback);
    }
  }, {
    key: "updateMemberSettings",
    value: function updateMemberSettings(payload, callback) {
      var sf = this.getServiceFramework("Security");
      sf.post("UpdateMemberSettings", payload, callback);
    }
  }, {
    key: "getRegistrationSettings",
    value: function getRegistrationSettings(callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetRegistrationSettings", {}, callback);
    }
  }, {
    key: "updateRegistrationSettings",
    value: function updateRegistrationSettings(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("Security");
      sf.post("UpdateRegistrationSettings", payload, callback, failureCallback);
    }
  }, {
    key: "getSslSettings",
    value: function getSslSettings(callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetSslSettings", {}, callback);
    }
  }, {
    key: "updateSslSettings",
    value: function updateSslSettings(payload, callback) {
      var sf = this.getServiceFramework("Security");
      sf.post("UpdateSslSettings", payload, callback);
    }
  }, {
    key: "getOtherSettings",
    value: function getOtherSettings(callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetOtherSettings", {}, callback);
    }
  }, {
    key: "updateOtherSettings",
    value: function updateOtherSettings(payload, callback) {
      var sf = this.getServiceFramework("Security");
      sf.post("UpdateOtherSettings", payload, callback);
    }
  }, {
    key: "getSecurityBulletins",
    value: function getSecurityBulletins(callback, failureCallback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetSecurityBulletins", {}, callback, failureCallback);
    }
  }, {
    key: "getSuperuserActivities",
    value: function getSuperuserActivities(callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetSuperuserActivities", {}, callback);
    }
  }, {
    key: "getAuditCheckResults",
    value: function getAuditCheckResults(callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetAuditCheckResults", {}, callback);
    }
  }, {
    key: "getAuditCheckResult",
    value: function getAuditCheckResult(id, callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetAuditCheckResult", {
        id: id
      }, callback);
    }
  }, {
    key: "searchFileSystemAndDatabase",
    value: function searchFileSystemAndDatabase(searchParameters, callback) {
      var sf = this.getServiceFramework("Security");
      searchParameters = Object.assign({}, searchParameters, {});

      if (this.searchRequest && this.searchRequest.readyState !== 4) {
        if (window.dnn) {
          window.dnn.loading = false;
        }

        this.searchRequest.abort();
      }

      this.searchRequest = sf.get("SearchFileSystemAndDatabase?" + serializeQueryStringParameters(searchParameters), {}, callback);
    }
  }, {
    key: "getLastModifiedSettings",
    value: function getLastModifiedSettings(callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetLastModifiedSettings", {}, callback);
    }
  }, {
    key: "getLastModifiedFiles",
    value: function getLastModifiedFiles(callback) {
      var sf = this.getServiceFramework("Security");
      sf.get("GetLastModifiedFiles", {}, callback);
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return ApplicationService;
}();

var applicationService = new ApplicationService();
var _default = applicationService;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(serializeQueryStringParameters, "serializeQueryStringParameters", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(ApplicationService, "ApplicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(applicationService, "applicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\services\\applicationService.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 11 */
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
/* 12 */
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

var	fixUrls = __webpack_require__(64);

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
/* 13 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.Redux;

/***/ }),
/* 14 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(84);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 15 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(68);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 16 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(69);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 17 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(70);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 18 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(79);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 19 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(81);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 20 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(85);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 21 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(86);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 22 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(87);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 23 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();


var boilerPlate = {
  init: function init() {
    // This setting is required and define the public path 
    // to allow the web application to download assets on demand 
    // eslint-disable-next-line no-undef
    // __webpack_public_path__ = options.publicPath;        
    var options = window.dnn.initSecurity();
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].init(options.utility);
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].moduleName = options.moduleName;
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].settings = options.settings; // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production

    __webpack_require__(62);
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
var _default = boilerPlate;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(boilerPlate, "boilerPlate", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\globals\\application.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\globals\\application.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 24 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 25 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(71);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(1);
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






/*eslint-disable quotes*/

var allowIcon = __webpack_require__(73).default;

var denyIcon = __webpack_require__(74).default;

var editIcon = __webpack_require__(75).default;

var deleteIcon = __webpack_require__(76).default;

var IpFilterRow =
/*#__PURE__*/
function (_Component) {
  _inherits(IpFilterRow, _Component);

  function IpFilterRow() {
    _classCallCheck(this, IpFilterRow);

    return _possibleConstructorReturn(this, _getPrototypeOf(IpFilterRow).call(this));
  }

  _createClass(IpFilterRow, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var opened = this.props.openId !== "" && this.props.id === this.props.openId;
      this.setState({
        opened: opened
      });
    }
  }, {
    key: "toggle",
    value: function toggle() {
      if (this.props.openId !== "" && this.props.id === this.props.openId) {
        this.props.Collapse();
      } else {
        this.props.OpenCollapse(this.props.id);
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "getRuleTypeDisplay",
    value: function getRuleTypeDisplay() {
      var props = this.props;

      if (props.id !== "add") {
        if (props.ruleType === 1) {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "item-row-ruleType-display"
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "allow-icon",
              dangerouslySetInnerHTML: {
                __html: allowIcon
              }
            }),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              style: {
                paddingLeft: "10px",
                paddingTop: 3,
                float: "left"
              }
            }, _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("AllowIP")))
          );
        } else {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "item-row-ruleType-display"
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "deny-icon",
              dangerouslySetInnerHTML: {
                __html: denyIcon
              }
            }),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              style: {
                paddingLeft: "10px",
                paddingTop: 3,
                float: "left"
              }
            }, _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("DenyIP")))
          );
        }
      } else {
        return "-";
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var opened = this.props.openId !== "" && this.props.id === this.props.openId;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "collapsible-component-ipfilters" + (opened ? " row-opened" : "")
        }, props.visible &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "collapsible-ipfilters " + !opened
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "row"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "ip-filter-item item-row-ruleType"
        }, this.getRuleTypeDisplay()),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "ip-filter-item item-row-ipAddress"
        }, props.ipFilter), props.id !== "add" && !props.readOnly &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "ip-filter-item item-row-editButton"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: opened ? "delete-icon-hidden" : "delete-icon",
          dangerouslySetInnerHTML: {
            __html: deleteIcon
          },
          onClick: this.props.onDelete.bind(this)
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: opened ? "edit-icon-active" : "edit-icon",
          dangerouslySetInnerHTML: {
            __html: editIcon
          },
          onClick: this.toggle.bind(this)
        })))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["Collapsible"], {
          className: "ip-filter-wrapper",
          accordion: true,
          isOpened: opened,
          style: {
            overflow: "visible",
            width: "100%"
          }
        }, opened && props.children))
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

  return IpFilterRow;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

IpFilterRow.propTypes = {
  ipFilterId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  ipFilter: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  ruleType: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  OpenCollapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  Collapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onDelete: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  id: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  openId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  visible: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  readOnly: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};
IpFilterRow.defaultProps = {
  collapsed: true,
  visible: true,
  readOnly: false
};
var _default = IpFilterRow;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(allowIcon, "allowIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipFilterRow\\index.jsx");
  reactHotLoader.register(denyIcon, "denyIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipFilterRow\\index.jsx");
  reactHotLoader.register(editIcon, "editIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipFilterRow\\index.jsx");
  reactHotLoader.register(deleteIcon, "deleteIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipFilterRow\\index.jsx");
  reactHotLoader.register(IpFilterRow, "IpFilterRow", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipFilterRow\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipFilterRow\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 26 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(77);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(6);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(1);
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








var specificityOptions = [];
var typeOptions = [];
var re = /^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
var SINGLE_IP = "SingleIP";
var IP_RANGE = "IPRange";

var IpFilterEditor =
/*#__PURE__*/
function (_Component) {
  _inherits(IpFilterEditor, _Component);

  function IpFilterEditor() {
    var _this;

    _classCallCheck(this, IpFilterEditor);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(IpFilterEditor).call(this));
    _this.state = {
      ipFilter: {
        RuleType: 1
      },
      error: {
        ip: true,
        mask: true
      },
      triedToSubmit: false,
      formModified: false,
      ruleSpecificity: SINGLE_IP
    };
    specificityOptions = [];
    specificityOptions.push({
      label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get(SINGLE_IP),
      value: SINGLE_IP
    });
    specificityOptions.push({
      label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get(IP_RANGE),
      value: IP_RANGE
    });
    typeOptions = [];
    typeOptions.push({
      label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("AllowIP"),
      value: 1
    });
    typeOptions.push({
      label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("DenyIP"),
      value: 2
    });
    return _this;
  }

  _createClass(IpFilterEditor, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var _this2 = this;

      var props = this.props;

      if (props.ipFilterId) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* security */ "b"].getIpFilter({
          filterId: props.ipFilterId
        }, function (data) {
          var ipFilter = Object.assign({}, data.Results);

          _this2.setState({
            error: {
              ip: false,
              mask: false
            },
            ruleSpecificity: ipFilter.SubnetMask === "" ? SINGLE_IP : IP_RANGE,
            ipFilter: ipFilter
          });
        }));
      }
    }
  }, {
    key: "onRuleSpecificityChange",
    value: function onRuleSpecificityChange(event) {
      this.setState({
        ruleSpecificity: event,
        formModified: true,
        triedToSubmit: false
      });
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var ipFilter = this.state.ipFilter;

      if (key === "RuleType") {
        ipFilter[key] = parseInt(event.value);
      } else {
        ipFilter[key] = _typeof(event) === "object" ? event.target.value : event;
      }

      this.setState({
        ipFilter: ipFilter,
        triedToSubmit: false,
        formModified: true
      });
    }
  }, {
    key: "onUpdateItem",
    value: function onUpdateItem(event) {
      event.preventDefault();
      var state = this.state;
      this.setState({
        triedToSubmit: true
      });

      if (this.validateIPAddressContainsError()) {
        return;
      }

      if (state.ruleSpecificity === SINGLE_IP) {
        state.ipFilter["SubnetMask"] = "";
      }

      this.props.onUpdate(this.state.ipFilter);
    }
  }, {
    key: "validateIPAddressContainsError",
    value: function validateIPAddressContainsError() {
      var state = this.state;
      state.error["ip"] = !this.isValidIpAddress(state.ipFilter["IPAddress"]);

      if (state.ruleSpecificity === IP_RANGE) {
        state.error["mask"] = !this.isValidIpAddress(state.ipFilter["SubnetMask"]);
      } else {
        state.error["mask"] = false;
      }

      this.setState({
        error: state.error
      });
      return state.error.ip || state.error.mask;
    }
  }, {
    key: "isValidIpAddress",
    value: function isValidIpAddress(ipAddress) {
      return re.test(ipAddress);
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var columnOne =
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "container"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
        labelType: "inline",
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plRuleSpecifity.Help"),
        label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plRuleSpecifity")
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["RadioButtons"], {
        onChange: this.onRuleSpecificityChange.bind(this),
        options: specificityOptions,
        buttonGroup: "specificity",
        value: this.state.ruleSpecificity
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plRuleType.Help"),
        label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plRuleType")
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
        options: typeOptions,
        value: this.state.ipFilter.RuleType,
        onSelect: this.onSettingChange.bind(this, "RuleType")
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plFirstIP.Help"),
        label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plFirstIP")
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: this.state.error.ip && this.state.triedToSubmit,
        errorMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("IPValidation.ErrorMessage"),
        value: this.state.ipFilter.IPAddress,
        onChange: this.onSettingChange.bind(this, "IPAddress")
      })), this.state.ruleSpecificity === IP_RANGE &&
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plSubnet.Help"),
        label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plSubnet")
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
        inputStyle: {
          margin: "0"
        },
        withLabel: false,
        error: this.state.error.mask && this.state.triedToSubmit,
        errorMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("IPValidation.ErrorMessage"),
        value: this.state.ipFilter.SubnetMask,
        onChange: this.onSettingChange.bind(this, "SubnetMask")
      })));
      var children = [];
      children.push(columnOne);
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "ip-filter-setting-editor"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridSystem"], {
          numberOfColumns: 1
        }, children),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "buttons-box"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
          type: "secondary",
          onClick: this.props.Collapse.bind(this)
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Cancel")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
          disabled: !this.state.formModified,
          type: "primary",
          onClick: this.onUpdateItem.bind(this)
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Save"))))
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

  return IpFilterEditor;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

IpFilterEditor.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  ipFilter: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  ipFilterId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  Collapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onUpdate: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func
};

function mapStateToProps(state) {
  return {
    ipFilter: state.security.ipFilter
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(IpFilterEditor);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(specificityOptions, "specificityOptions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipfilterEditor\\index.jsx");
  reactHotLoader.register(typeOptions, "typeOptions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipfilterEditor\\index.jsx");
  reactHotLoader.register(re, "re", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipfilterEditor\\index.jsx");
  reactHotLoader.register(SINGLE_IP, "SINGLE_IP", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipfilterEditor\\index.jsx");
  reactHotLoader.register(IP_RANGE, "IP_RANGE", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipfilterEditor\\index.jsx");
  reactHotLoader.register(IpFilterEditor, "IpFilterEditor", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipfilterEditor\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipfilterEditor\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\ipfilterEditor\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 27 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var paginationActionTypes = {
  LOAD_MORE: "LOAD_MORE",
  LOAD_TAB_DATA: "LOAD_TAB_DATA"
};
var _default = paginationActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\constants\\actionTypes\\pagination.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\constants\\actionTypes\\pagination.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 28 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var paginationActionTypes = {
  SELECT_PANEL: "SELECT_PANEL"
};
var _default = paginationActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\constants\\actionTypes\\visiblePanel.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\constants\\actionTypes\\visiblePanel.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 29 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var securityActionTypes = {
  RETRIEVED_SECURITY_IP_FILTERS: "RETRIEVED_SECURITY_IP_FILTERS",
  RETRIEVED_SECURITY_IP_FILTER: "RETRIEVED_SECURITY_IP_FILTER",
  RETRIEVED_SECURITY_BASIC_LOGIN_SETTINS: "RETRIEVED_SECURITY_BASIC_LOGIN_SETTINS",
  UPDATED_SECURITY_BASIC_LOGIN_SETTINS: "UPDATED_SECURITY_BASIC_LOGIN_SETTINS",
  UPDATED_SECURITY_IP_FILTER: "UPDATED_SECURITY_IP_FILTER",
  DELETED_SECURITY_IP_FILTER: "DELETED_SECURITY_IP_FILTER",
  RETRIEVED_SECURITY_MEMBER_SETTINS: "RETRIEVED_SECURITY_MEMBER_SETTINS",
  UPDATED_SECURITY_MEMBER_SETTINS: "UPDATED_SECURITY_MEMBER_SETTINS",
  RETRIEVED_SECURITY_REGISTRATION_SETTINS: "RETRIEVED_SECURITY_REGISTRATION_SETTINS",
  UPDATED_SECURITY_REGISTRATION_SETTINS: "UPDATED_SECURITY_REGISTRATION_SETTINS",
  RETRIEVED_SECURITY_SSL_SETTINS: "RETRIEVED_SECURITY_SSL_SETTINS",
  UPDATED_SECURITY_SSL_SETTINS: "UPDATED_SECURITY_SSL_SETTINS",
  RETRIEVED_SECURITY_OTHER_SETTINS: "RETRIEVED_SECURITY_OTHER_SETTINS",
  UPDATED_SECURITY_OTHER_SETTINS: "UPDATED_SECURITY_OTHER_SETTINS",
  RETRIEVED_SECURITY_BULLETINS: "RETRIEVED_SECURITY_BULLETINS",
  RETRIEVED_SECURITY_SUPERUSER_ACTIVITIES: "RETRIEVED_SECURITY_SUPERUSER_ACTIVITIES",
  RETRIEVED_SECURITY_AUDITCHECK_RESULTS: "RETRIEVED_SECURITY_AUDITCHECK_RESULTS",
  RETRIEVED_SECURITY_AUDITCHECK_RESULT: "RETRIEVED_SECURITY_AUDITCHECK_RESULT",
  RETRIEVED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE: "RETRIEVED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE",
  RETRIEVED_SECURITY_LAST_MODIFIED_SETTINGS: "RETRIEVED_SECURITY_LAST_MODIFIED_SETTINGS",
  RETRIEVED_SECURITY_LAST_MODIFIED_FILES: "RETRIEVED_SECURITY_LAST_MODIFIED_FILES",
  SECURITY_BASIC_LOGIN_SETTINS_CLIENT_MODIFIED: "SECURITY_BASIC_LOGIN_SETTINS_CLIENT_MODIFIED",
  SECURITY_MEMBER_SETTINS_CLIENT_MODIFIED: "SECURITY_MEMBER_SETTINS_CLIENT_MODIFIED",
  SECURITY_REGISTRATION_SETTINS_CLIENT_MODIFIED: "SECURITY_REGISTRATION_SETTINS_CLIENT_MODIFIED",
  SECURITY_SSL_SETTINS_CLIENT_MODIFIED: "SECURITY_SSL_SETTINS_CLIENT_MODIFIED",
  SECURITY_OTHER_SETTINS_CLIENT_MODIFIED: "SECURITY_OTHER_SETTINS_CLIENT_MODIFIED",
  UPDATED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE_KEYWORD: "UPDATED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE_KEYWORD",
  UPDATED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE_ACTIVE_TAB: "UPDATED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE_ACTIVE_TAB"
};
var _default = securityActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(securityActionTypes, "securityActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\constants\\actionTypes\\security.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\constants\\actionTypes\\security.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 30 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();


var paginationActions = {
  loadTab: function loadTab(index) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* pagination */ "a"].LOAD_TAB_DATA,
        payload: {
          index: index
        }
      });
    };
  }
};
var _default = paginationActions;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActions, "paginationActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\actions\\pagination.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\actions\\pagination.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 31 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();


var visiblePanelActions = {
  selectPanel: function selectPanel(panel, selectedPageVisibleIndex) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* visiblePanel */ "c"].SELECT_PANEL,
        payload: {
          selectedPage: panel,
          selectedPageVisibleIndex: selectedPageVisibleIndex
        }
      });
    };
  }
};
var _default = visiblePanelActions;
/* unused harmony default export */ var _unused_webpack_default_export = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(visiblePanelActions, "visiblePanelActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\actions\\visiblePanel.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\actions\\visiblePanel.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 32 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
/* harmony import */ var _services_applicationService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(10);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();




var securityActions = {
  getIpFilters: function getIpFilters(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getIpFilters(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_IP_FILTERS,
          data: {
            ipFilters: data.Results.Filters,
            enableIPChecking: data.Results.EnableIPChecking
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getIpFilter: function getIpFilter(searchParameters, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getIpFilter(searchParameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_IP_FILTER,
          data: {
            ipFilter: data.Results
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  deleteIpFilter: function deleteIpFilter(filterId, ipFilters, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].deleteIpFilter(filterId, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].DELETED_SECURITY_IP_FILTER,
          data: {
            ipFilters: ipFilters
          }
        });

        if (callback) {
          callback(data);
        }
      }, function (failureMessage) {
        var errorMessage = JSON.parse(failureMessage.responseText);
        _utils__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].utilities.notifyError(errorMessage.Message);
      });
    };
  },
  getBasicLoginSettings: function getBasicLoginSettings(cultureCode, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getBasicLoginSettings(cultureCode, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_BASIC_LOGIN_SETTINS,
          data: {
            basicLoginSettings: data.Results.Settings,
            adminUsers: data.Results.Administrators,
            authProviders: data.Results.AuthProviders,
            basicLoginSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateBasicLoginSettings: function updateBasicLoginSettings(payload, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].updateBasicLoginSettings(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_BASIC_LOGIN_SETTINS,
          data: {
            basicLoginSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  basicLoginSettingsClientModified: function basicLoginSettingsClientModified(parameter) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_BASIC_LOGIN_SETTINS_CLIENT_MODIFIED,
        data: {
          basicLoginSettings: parameter,
          basicLoginSettingsClientModified: true
        }
      });
    };
  },
  memberSettingsClientModified: function memberSettingsClientModified(parameter) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_MEMBER_SETTINS_CLIENT_MODIFIED,
        data: {
          memberSettings: parameter,
          memberSettingsClientModified: true
        }
      });
    };
  },
  registrationSettingsClientModified: function registrationSettingsClientModified(parameter) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_REGISTRATION_SETTINS_CLIENT_MODIFIED,
        data: {
          registrationSettings: parameter,
          registrationSettingsClientModified: true
        }
      });
    };
  },
  sslSettingsClientModified: function sslSettingsClientModified(parameter) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_SSL_SETTINS_CLIENT_MODIFIED,
        data: {
          sslSettings: parameter,
          sslSettingsClientModified: true
        }
      });
    };
  },
  otherSettingsClientModified: function otherSettingsClientModified(parameter) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_OTHER_SETTINS_CLIENT_MODIFIED,
        data: {
          otherSettings: parameter,
          otherSettingsClientModified: true
        }
      });
    };
  },
  updateIpFilter: function updateIpFilter(payload, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].updateIpFilter(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_IP_FILTER,
          data: {}
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getMemberSettings: function getMemberSettings(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getMemberSettings(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_MEMBER_SETTINS,
          data: {
            memberSettings: data.Results.Settings,
            memberSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateMemberSettings: function updateMemberSettings(payload, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].updateMemberSettings(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_MEMBER_SETTINS,
          data: {
            memberSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getRegistrationSettings: function getRegistrationSettings(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getRegistrationSettings(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_REGISTRATION_SETTINS,
          data: {
            registrationSettings: data.Results.Settings,
            userRegistrationOptions: data.Results.UserRegistrationOptions,
            registrationFormTypeOptions: data.Results.RegistrationFormTypeOptions,
            registrationSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateRegistrationSettings: function updateRegistrationSettings(payload, callback, failureCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].updateRegistrationSettings(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_REGISTRATION_SETTINS,
          data: {
            registrationSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  },
  getSslSettings: function getSslSettings(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getSslSettings(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_SSL_SETTINS,
          data: {
            sslSettings: data.Results.Settings,
            sslSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateSslSettings: function updateSslSettings(payload, callback, failureCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].updateSslSettings(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_SSL_SETTINS,
          data: {
            sslSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  },
  getOtherSettings: function getOtherSettings(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getOtherSettings(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_OTHER_SETTINS,
          data: {
            otherSettings: data.Results.Settings,
            otherSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateOtherSettings: function updateOtherSettings(payload, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].updateOtherSettings(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_OTHER_SETTINS,
          data: {
            otherSettingsClientModified: false
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getSecurityBulletins: function getSecurityBulletins(callback, failureCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getSecurityBulletins(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_BULLETINS,
          data: {
            platformVersion: data.Results.PlatformVersion,
            securityBulletins: data.Results.SecurityBulletins
          }
        });

        if (callback) {
          callback(data);
        }
      }, function (data) {
        if (failureCallback) {
          failureCallback(data);
        }
      });
    };
  },
  getSuperuserActivities: function getSuperuserActivities(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getSuperuserActivities(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_SUPERUSER_ACTIVITIES,
          data: {
            activities: data.Results.Activities
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getAuditCheckResults: function getAuditCheckResults(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getAuditCheckResults(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_AUDITCHECK_RESULTS,
          data: {
            auditCheckResults: data.Results
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getAuditCheckResult: function getAuditCheckResult(id, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getAuditCheckResult(id, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_AUDITCHECK_RESULT,
          data: {
            auditCheckResult: data.Result
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  searchFileSystemAndDatabase: function searchFileSystemAndDatabase(searchParameters, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].searchFileSystemAndDatabase(searchParameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE,
          data: {
            searchResults: data.Results,
            scannerCheckKeyword: searchParameters.term,
            scannerCheckActiveTab: "search"
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  clearFileSystemAndDatabaseSearch: function clearFileSystemAndDatabaseSearch() {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE,
        data: {
          searchResults: undefined,
          scannerCheckKeyword: "",
          scannerCheckActiveTab: "search"
        }
      });
    };
  },
  getLastModifiedSettings: function getLastModifiedSettings(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getLastModifiedSettings(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_LAST_MODIFIED_SETTINGS,
          data: {
            modifiedSettings: data.Results,
            scannerCheckActiveTab: "settings"
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getLastModifiedFiles: function getLastModifiedFiles(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getLastModifiedFiles(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_LAST_MODIFIED_FILES,
          data: {
            modifiedFiles: data.Results,
            scannerCheckActiveTab: "files"
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updatefileSystemAndDatabaseSearchKeyword: function updatefileSystemAndDatabaseSearchKeyword(keyword) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE_KEYWORD,
        data: {
          scannerCheckKeyword: keyword,
          scannerCheckActiveTab: "search"
        }
      });
    };
  },
  updatefileSystemAndDatabaseActiveTab: function updatefileSystemAndDatabaseActiveTab(tab) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE_ACTIVE_TAB,
        data: {
          scannerCheckActiveTab: tab
        }
      });
    };
  }
};
var _default = securityActions;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(securityActions, "securityActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\actions\\security.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\actions\\security.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 33 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<style type=\"text/css\">\r\n\t.st0{fill:#E72635;}\r\n</style>\r\n<path class=\"st0\" d=\"M1564.2,483.7c-298.4-298.2-782.1-298.2-1080.5,0c-298.4,298.2-298.4,782.1,0,1080.5\r\n\tc298.4,298.4,782.3,298.2,1080.5,0C1862.6,1266,1862.6,781.9,1564.2,483.7z M1160.2,507.3l-33.6,703.7H929.4l-33.6-703.7H1160.2z\r\n\t M1028,1638.6c-82.9,0-150.1-67.2-150.1-150.1c0-82.9,67.2-150.1,150.1-150.1s150.1,67.2,150.1,150.1\r\n\tC1178,1571.4,1110.9,1638.6,1028,1638.6z\"/>\r\n</svg>\r\n");

/***/ }),
/* 34 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 35 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return configureStore; });
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(13);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(24);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_thunk__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(36);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _reducers_rootReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(37);
/* harmony import */ var _containers_DevTools__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(41);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();






/* eslint-disable no-undef */

var IS_PRODUCTION = "production" === "production";
function configureStore(initialState) {
  var store = Object(redux__WEBPACK_IMPORTED_MODULE_0__["createStore"])(_reducers_rootReducer__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"], initialState, Object(redux__WEBPACK_IMPORTED_MODULE_0__["compose"])(IS_PRODUCTION ? Object(redux__WEBPACK_IMPORTED_MODULE_0__["applyMiddleware"])(redux_thunk__WEBPACK_IMPORTED_MODULE_1___default.a) : Object(redux__WEBPACK_IMPORTED_MODULE_0__["applyMiddleware"])(redux_thunk__WEBPACK_IMPORTED_MODULE_1___default.a, redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2___default()()), _containers_DevTools__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].instrument()));
  return store;
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(IS_PRODUCTION, "IS_PRODUCTION", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\store\\configureStore.js");
  reactHotLoader.register(configureStore, "configureStore", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\store\\configureStore.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 36 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 37 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(13);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _paginationReducer__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(38);
/* harmony import */ var _visiblePanelReducer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(39);
/* harmony import */ var _securityReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(40);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();





var rootReducer = Object(redux__WEBPACK_IMPORTED_MODULE_0__["combineReducers"])({
  pagination: _paginationReducer__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"],
  visiblePanel: _visiblePanelReducer__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"],
  security: _securityReducer__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"]
});
var _default = rootReducer;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(rootReducer, "rootReducer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\reducers\\rootReducer.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\reducers\\rootReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 38 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return pagination; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function pagination() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    tabIndex: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* pagination */ "a"].LOAD_TAB_DATA:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        tabIndex: action.payload.index
      });

    default:
      return _objectSpread2({}, state);
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(pagination, "pagination", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\reducers\\paginationReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 39 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return visiblePanel; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function visiblePanel() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    selectedPage: 0,
    selectedPageVisibleIndex: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* visiblePanel */ "c"].SELECT_PANEL:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        selectedPage: action.payload.selectedPage,
        selectedPageVisibleIndex: action.payload.selectedPageVisibleIndex
      });

    default:
      return _objectSpread2({}, state);
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(visiblePanel, "visiblePanel", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\reducers\\visiblePanelReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 40 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return securitySettings; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function securitySettings() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_BASIC_LOGIN_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        basicLoginSettings: action.data.basicLoginSettings,
        adminUsers: action.data.adminUsers,
        authProviders: action.data.authProviders,
        basicLoginSettingsClientModified: false
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_MEMBER_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        memberSettings: action.data.memberSettings,
        memberSettingsClientModified: false
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_OTHER_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        otherSettings: action.data.otherSettings,
        otherSettingsClientModified: false
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_SSL_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        sslSettings: action.data.sslSettings,
        sslSettingsClientModified: false
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_REGISTRATION_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        registrationSettings: action.data.registrationSettings,
        userRegistrationOptions: action.data.userRegistrationOptions,
        registrationFormTypeOptions: action.data.registrationFormTypeOptions,
        registrationSettingsClientModified: false
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_IP_FILTERS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        ipFilters: action.data.ipFilters,
        enableIPChecking: action.data.enableIPChecking
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].DELETED_SECURITY_IP_FILTER:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        ipFilters: action.data.ipFilters
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_IP_FILTER:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        ipFilter: action.data.ipFilter
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_BULLETINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        platformVersion: action.data.platformVersion,
        securityBulletins: action.data.securityBulletins
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_SUPERUSER_ACTIVITIES:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        activities: action.data.activities
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_AUDITCHECK_RESULTS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        auditCheckResults: action.data.auditCheckResults
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_AUDITCHECK_RESULT:
      {
        var result = action.data.auditCheckResult;
        var curentResults = Object.assign([], JSON.parse(JSON.stringify(state.auditCheckResults)));

        for (var i = 0; i < curentResults.length; i++) {
          if (curentResults[i].CheckName === result.CheckName) {
            curentResults[i] = result;
            break;
          }
        }

        return _objectSpread2(_objectSpread2({}, state), {}, {
          auditCheckResults: curentResults
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        searchResults: action.data.searchResults,
        scannerCheckKeyword: action.data.scannerCheckKeyword,
        scannerCheckActiveTab: action.data.scannerCheckActiveTab
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_LAST_MODIFIED_SETTINGS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        modifiedSettings: action.data.modifiedSettings,
        scannerCheckActiveTab: action.data.scannerCheckActiveTab
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].RETRIEVED_SECURITY_LAST_MODIFIED_FILES:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        modifiedFiles: action.data.modifiedFiles,
        scannerCheckActiveTab: action.data.scannerCheckActiveTab
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_BASIC_LOGIN_SETTINS_CLIENT_MODIFIED:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        basicLoginSettings: action.data.basicLoginSettings,
        basicLoginSettingsClientModified: action.data.basicLoginSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_BASIC_LOGIN_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        basicLoginSettingsClientModified: action.data.basicLoginSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_MEMBER_SETTINS_CLIENT_MODIFIED:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        memberSettings: action.data.memberSettings,
        memberSettingsClientModified: action.data.memberSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_REGISTRATION_SETTINS_CLIENT_MODIFIED:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        registrationSettings: action.data.registrationSettings,
        registrationSettingsClientModified: action.data.registrationSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_SSL_SETTINS_CLIENT_MODIFIED:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        sslSettings: action.data.sslSettings,
        sslSettingsClientModified: action.data.sslSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].SECURITY_OTHER_SETTINS_CLIENT_MODIFIED:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        otherSettings: action.data.otherSettings,
        otherSettingsClientModified: action.data.otherSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_MEMBER_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        memberSettingsClientModified: action.data.memberSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_REGISTRATION_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        registrationSettingsClientModified: action.data.registrationSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_SSL_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        sslSettingsClientModified: action.data.sslSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_OTHER_SETTINS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        otherSettingsClientModified: action.data.otherSettingsClientModified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE_KEYWORD:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        scannerCheckActiveTab: action.data.scannerCheckActiveTab,
        scannerCheckKeyword: action.data.scannerCheckKeyword
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* security */ "b"].UPDATED_SECURITY_SEARCH_FILE_SYSTEM_DATABASE_ACTIVE_TAB:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        scannerCheckActiveTab: action.data.scannerCheckActiveTab
      });

    default:
      return _objectSpread2({}, state);
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(securitySettings, "securitySettings", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\reducers\\securityReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 41 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(42);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_devtools__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(43);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(44);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();






var _default = Object(redux_devtools__WEBPACK_IMPORTED_MODULE_1__["createDevTools"])(
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3___default.a, {
  toggleVisibilityKey: "ctrl-h",
  changePositionKey: "ctrl-q"
},
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2___default.a, null)));

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\containers\\DevTools.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 42 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 43 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 44 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 45 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(65);
} else {}

/***/ }),
/* 46 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _body__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(47);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(1);
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








var App =
/*#__PURE__*/
function (_Component) {
  _inherits(App, _Component);

  function App() {
    _classCallCheck(this, App);

    return _possibleConstructorReturn(this, _getPrototypeOf(App).call(this));
  }

  _createClass(App, [{
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "securitySettings-app"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPage"], {
          isOpen: props.selectedPage === 0
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPageHeader"], {
          title: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("nav_Security")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_body__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
          cultureCode: props.cultureCode
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

  return App;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

App.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  selectedPage: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  cultureCode: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};

function mapStateToProps(state) {
  return {
    selectedPage: state.visiblePanel.selectedPage,
    selectedPageVisibleIndex: state.visiblePanel.selectedPageVisibleIndex
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(App);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(App, "App", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\App.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\App.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 47 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* unused harmony export Body */
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(6);
/* harmony import */ var _basicSettings__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(48);
/* harmony import */ var _sslSettings__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(49);
/* harmony import */ var _otherSettings__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(50);
/* harmony import */ var _ipFilters__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(51);
/* harmony import */ var _memberManagement__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(52);
/* harmony import */ var _registrationSettings__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(54);
/* harmony import */ var _securityBulletins__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(55);
/* harmony import */ var _superuserActivity__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(57);
/* harmony import */ var _auditCheck__WEBPACK_IMPORTED_MODULE_13__ = __webpack_require__(58);
/* harmony import */ var _scannerCheck__WEBPACK_IMPORTED_MODULE_14__ = __webpack_require__(59);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_15__ = __webpack_require__(88);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_15___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_15__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_16__ = __webpack_require__(7);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_17__ = __webpack_require__(1);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }



















var isHost = false;
var isAdmin = false;
var canViewBasicLoginSettings = false;
var canViewRegistrationSettings = false;
var Body =
/*#__PURE__*/
function (_Component) {
  _inherits(Body, _Component);

  function Body() {
    var _this;

    _classCallCheck(this, Body);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(Body).call(this));
    _this.handleSelect = _this.handleSelect.bind(_assertThisInitialized(_this));
    isHost = _utils__WEBPACK_IMPORTED_MODULE_16__[/* default */ "a"].settings.isHost;
    isAdmin = isHost || _utils__WEBPACK_IMPORTED_MODULE_16__[/* default */ "a"].settings.isAdmin;
    canViewBasicLoginSettings = isHost || isAdmin || _utils__WEBPACK_IMPORTED_MODULE_16__[/* default */ "a"].settings.permissions.BASIC_LOGIN_SETTINGS_VIEW;
    canViewRegistrationSettings = isHost || isAdmin || _utils__WEBPACK_IMPORTED_MODULE_16__[/* default */ "a"].settings.permissions.REGISTRATION_SETTINGS_VIEW;
    return _this;
  }

  _createClass(Body, [{
    key: "handleSelect",
    value: function handleSelect(index) {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_4__[/* pagination */ "a"].loadTab(index)); //index acts as scopeTypeId
    }
  }, {
    key: "renderTabs",
    value: function renderTabs() {
      var securityTabs = [];
      var tabHeaders = [];
      var loginSettingTabHeaders = [];
      var memberAccountsTabHeaders = [];
      var moreTabHeaders = [];
      var moreTabs = [];
      var memberAccountsTabs = [];

      if (canViewBasicLoginSettings || isHost) {
        tabHeaders.push([_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabLoginSettings")]);
        if (canViewBasicLoginSettings) loginSettingTabHeaders.push(_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabBasicLoginSettings"));

        if (isHost) {
          loginSettingTabHeaders.push(
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            style: {
              fontSize: "9pt"
            }
          }, _resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabIpFilters"),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["Tooltip"], {
            messages: [_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("GlobalSettingsTab")],
            type: "global",
            style: {
              position: "absolute",
              right: -27,
              top: 15,
              textTransform: "none"
            }
          })));
        }
      }

      if (isHost || canViewRegistrationSettings) {
        tabHeaders.push([_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabMemberAccounts")]);

        if (isHost) {
          memberAccountsTabHeaders.push(
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            style: {
              fontSize: "9pt"
            }
          }, _resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabMemberSettings"),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["Tooltip"], {
            messages: [_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("GlobalSettingsTab")],
            type: "global",
            style: {
              position: "absolute",
              right: -27,
              top: 15,
              textTransform: "none"
            }
          })));
          memberAccountsTabs.push(
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_memberManagement__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"], {
            key: "memberManagement"
          }));
        }

        if (canViewRegistrationSettings) {
          memberAccountsTabHeaders.push(
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            style: {
              marginLeft: isHost ? 35 : 0
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            style: {
              width: isHost ? 35 : "auto",
              height: 3,
              position: "absolute",
              left: 0,
              bottom: -3,
              backgroundColor: "white"
            }
          }), _resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabRegistrationSettings")));
          memberAccountsTabs.push(
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_registrationSettings__WEBPACK_IMPORTED_MODULE_10__[/* default */ "a"], {
            key: "registrationSettings"
          }));
        }
      }

      if (isAdmin) {
        moreTabHeaders.push(_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabSslSettings"));
        moreTabs.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_sslSettings__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"], {
          key: "sslSettings"
        }));
      }

      if (isHost) {
        tabHeaders.push(_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabSecurityAnalyzer"));
        tabHeaders.push(_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabSecurityBulletins"));
        moreTabHeaders.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          style: {
            fontSize: "9pt"
          }
        }, _resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabMoreSecuritySettings"),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["Tooltip"], {
          messages: [_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("GlobalSettingsTab")],
          type: "global",
          style: {
            position: "absolute",
            right: -27,
            top: 15,
            textTransform: "none"
          }
        })));
        moreTabs.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_otherSettings__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], {
          key: "otherSettings"
        }));
      }

      if (isAdmin) {
        tabHeaders.push(_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabMore"));
      }

      if (canViewBasicLoginSettings || isHost) {
        securityTabs.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["DnnTabs"], {
          key: "loginSettingsTab",
          onSelect: this.handleSelect.bind(this),
          tabHeaders: loginSettingTabHeaders,
          type: "secondary"
        }, canViewBasicLoginSettings &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_basicSettings__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], {
          cultureCode: this.props.cultureCode
        }), isHost &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_ipFilters__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"], null)));
      }

      if (isHost || canViewRegistrationSettings) {
        securityTabs.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["DnnTabs"], {
          key: "memberAccountsTab",
          onSelect: this.handleSelect.bind(this),
          tabHeaders: memberAccountsTabHeaders,
          type: "secondary"
        }, memberAccountsTabs));
      }

      if (isHost) {
        securityTabs.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["DnnTabs"], {
          key: "auditChecksTab",
          onSelect: this.handleSelect.bind(this),
          tabHeaders: [_resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabAuditChecks"), _resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabScannerCheck"), _resources__WEBPACK_IMPORTED_MODULE_17__[/* default */ "a"].get("TabSuperuserActivity")],
          type: "secondary"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_auditCheck__WEBPACK_IMPORTED_MODULE_13__[/* default */ "a"], null),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_scannerCheck__WEBPACK_IMPORTED_MODULE_14__[/* default */ "a"], null),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_superuserActivity__WEBPACK_IMPORTED_MODULE_12__[/* default */ "a"], null)));
        securityTabs.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_securityBulletins__WEBPACK_IMPORTED_MODULE_11__[/* default */ "a"], {
          key: "securityBulletinsTab"
        }));
      }

      if (isAdmin) {
        securityTabs.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["DnnTabs"], {
          key: "moreTab",
          onSelect: this.handleSelect.bind(this),
          tabHeaders: moreTabHeaders,
          type: "secondary"
        }, moreTabs));
      }

      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["DnnTabs"], {
          onSelect: this.handleSelect.bind(this),
          tabHeaders: tabHeaders,
          type: "primary"
        }, securityTabs)
      );
    }
    /*eslint no-mixed-spaces-and-tabs: "error"*/

  }, {
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["PersonaBarPageBody"], null, this.renderTabs())
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

  return Body;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);
Body.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  cultureCode: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.index
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_3__["connect"])(mapStateToProps)(Body);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(isHost, "isHost", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\body\\index.jsx");
  reactHotLoader.register(isAdmin, "isAdmin", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\body\\index.jsx");
  reactHotLoader.register(canViewBasicLoginSettings, "canViewBasicLoginSettings", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\body\\index.jsx");
  reactHotLoader.register(canViewRegistrationSettings, "canViewRegistrationSettings", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\body\\index.jsx");
  reactHotLoader.register(Body, "Body", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\body\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\body\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\body\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 48 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(15);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(7);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(1);
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










var canEdit = false;

var BasicSettingsPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(BasicSettingsPanelBody, _Component);

  function BasicSettingsPanelBody() {
    var _this;

    _classCallCheck(this, BasicSettingsPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(BasicSettingsPanelBody).call(this));
    _this.state = {
      basicLoginSettings: undefined
    };
    canEdit = _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].settings.isHost || _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].settings.isAdmin || _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].settings.permissions.BASIC_LOGIN_SETTINGS_EDIT;
    return _this;
  }

  _createClass(BasicSettingsPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (props.basicLoginSettings) {
        this.setState({
          basicLoginSettings: props.basicLoginSettings
        });
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getBasicLoginSettings(props.cultureCode, function (data) {
        var basicLoginSettings = Object.assign({}, data.Results.Settings);

        _this2.setState({
          basicLoginSettings: basicLoginSettings
        });
      }));
    }
  }, {
    key: "getAuthProviderOptions",
    value: function getAuthProviderOptions() {
      var options = [];

      if (this.props.authProviders !== undefined) {
        options = this.props.authProviders.map(function (item) {
          return {
            label: item.Name,
            value: item.Value
          };
        });
      }

      return options;
    }
  }, {
    key: "getAdminUserOptions",
    value: function getAdminUserOptions() {
      var options = [];

      if (this.props.adminUsers !== undefined) {
        options = this.props.adminUsers.map(function (item) {
          return {
            label: item.FullName,
            value: item.UserID
          };
        });
      }

      return options;
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var props = this.props,
          state = this.state;
      var basicLoginSettings = Object.assign({}, state.basicLoginSettings);

      if (key === "RedirectAfterLoginTabId" || key === "RedirectAfterLogoutTabId") {
        if (basicLoginSettings[key] !== parseInt(event)) {
          basicLoginSettings[key] = event;
        } else {
          return;
        }
      } else if (key === "DefaultAuthProvider" || key === "PrimaryAdministratorId") {
        basicLoginSettings[key] = event.value;
      } else {
        basicLoginSettings[key] = _typeof(event) === "object" ? event.target.value : event;
      }

      this.setState({
        basicLoginSettings: basicLoginSettings
      });
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].basicLoginSettingsClientModified(basicLoginSettings));
    }
  }, {
    key: "onUpdate",
    value: function onUpdate(event) {
      event.preventDefault();
      var props = this.props,
          state = this.state;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updateBasicLoginSettings(state.basicLoginSettings, function () {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("BasicLoginSettingsUpdateSuccess"));
      }, function () {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notifyError(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("BasicLoginSettingsError"));
      }));
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var _this3 = this;

      var props = this.props;
      _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.confirm(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("LoginSettingsRestoreWarning"), _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Yes"), _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("No"), function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getBasicLoginSettings(props.cultureCode, function (data) {
          var basicLoginSettings = Object.assign({}, data.Results.Settings);

          _this3.setState({
            basicLoginSettings: basicLoginSettings
          });
        }));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var state = this.state;
      var noneSpecifiedText = "<" + _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("NoneSpecified") + ">";
      var RedirectAfterLogoutParameters = {
        portalId: -2,
        cultureCode: "",
        isMultiLanguage: false,
        excludeAdminTabs: false,
        disabledNotSelectable: false,
        roles: "-1",
        sortOrder: 0
      };
      var RedirectAfterLoginParameters = {
        portalId: -2,
        cultureCode: "",
        isMultiLanguage: false,
        excludeAdminTabs: false,
        disabledNotSelectable: false,
        roles: "1;-1",
        sortOrder: 0
      };

      if (state.basicLoginSettings) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: _style_less__WEBPACK_IMPORTED_MODULE_5___default.a.loginSettings
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("DefaultAuthProvider.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("DefaultAuthProvider")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
            options: this.getAuthProviderOptions(),
            value: state.basicLoginSettings.DefaultAuthProvider,
            onSelect: this.onSettingChange.bind(this, "DefaultAuthProvider"),
            enabled: canEdit
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plAdministrator.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plAdministrator")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
            options: this.getAdminUserOptions(),
            value: state.basicLoginSettings.PrimaryAdministratorId,
            onSelect: this.onSettingChange.bind(this, "PrimaryAdministratorId"),
            enabled: canEdit
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            style: {
              paddingBottom: "15px",
              fontStyle: "italic"
            }
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("RedirectionMovedToSiteSettings")),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "loginSettings-row_switch",
            style: {
              margin: "0"
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_RequireValidProfileAtLogin.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_RequireValidProfileAtLogin")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.basicLoginSettings.RequireValidProfileAtLogin,
            onChange: this.onSettingChange.bind(this, "RequireValidProfileAtLogin"),
            readOnly: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "loginSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_CaptchaLogin.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_CaptchaLogin")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.basicLoginSettings.CaptchaLogin,
            onChange: this.onSettingChange.bind(this, "CaptchaLogin"),
            readOnly: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "loginSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_CaptchaRetrivePassword.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_CaptchaRetrivePassword")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.basicLoginSettings.CaptchaRetrivePassword,
            onChange: this.onSettingChange.bind(this, "CaptchaRetrivePassword"),
            readOnly: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "loginSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_CaptchaChangePassword.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_CaptchaChangePassword")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.basicLoginSettings.CaptchaChangePassword,
            onChange: this.onSettingChange.bind(this, "CaptchaChangePassword"),
            readOnly: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "loginSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plHideLoginControl.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plHideLoginControl")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.basicLoginSettings.HideLoginControl,
            onChange: this.onSettingChange.bind(this, "HideLoginControl"),
            readOnly: !canEdit
          }))), canEdit &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "buttons-box"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.basicLoginSettingsClientModified,
            type: "secondary",
            onClick: this.onCancel.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Cancel")),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.basicLoginSettingsClientModified,
            type: "primary",
            onClick: this.onUpdate.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Save"))))
        );
      } else return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null)
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

  return BasicSettingsPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

BasicSettingsPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  authProviders: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  adminUsers: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  basicLoginSettings: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  basicLoginSettingsClientModified: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  cultureCode: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    authProviders: state.security.authProviders,
    adminUsers: state.security.adminUsers,
    basicLoginSettings: state.security.basicLoginSettings,
    basicLoginSettingsClientModified: state.security.basicLoginSettingsClientModified
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(BasicSettingsPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(canEdit, "canEdit", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\basicSettings\\index.jsx");
  reactHotLoader.register(BasicSettingsPanelBody, "BasicSettingsPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\basicSettings\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\basicSettings\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\basicSettings\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 49 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(16);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(7);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(1);
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











var SslSettingsPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(SslSettingsPanelBody, _Component);

  function SslSettingsPanelBody() {
    var _this;

    _classCallCheck(this, SslSettingsPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(SslSettingsPanelBody).call(this));
    _this.state = {
      sslSettings: undefined
    };
    return _this;
  }

  _createClass(SslSettingsPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;

      if (props.sslSettings) {
        this.setState({
          sslSettings: props.sslSettings
        });
        return;
      }

      this.getSslSettings();
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var state = this.state,
          props = this.props;
      var sslSettings = Object.assign({}, state.sslSettings);
      sslSettings[key] = _typeof(event) === "object" ? event.target.value : event;
      this.setState({
        sslSettings: sslSettings
      });
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].sslSettingsClientModified(sslSettings));
    }
  }, {
    key: "onUpdate",
    value: function onUpdate(event) {
      var _this2 = this;

      event.preventDefault();
      var props = this.props,
          state = this.state;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updateSslSettings(state.sslSettings, function () {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SslSettingsUpdateSuccess"));

        _this2.getSslSettings();
      }, function () {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notifyError(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SslSettingsError"));
      }));
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var _this3 = this;

      _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.confirm(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SslSettingsRestoreWarning"), _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Yes"), _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("No"), function () {
        _this3.getSslSettings();
      });
    }
  }, {
    key: "getSslSettings",
    value: function getSslSettings() {
      var _this4 = this;

      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getSslSettings(function (data) {
        var sslSettings = Object.assign({}, data.Results.Settings);

        _this4.setState({
          sslSettings: sslSettings
        });
      }));
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var state = this.state;

      if (state.sslSettings) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: _style_less__WEBPACK_IMPORTED_MODULE_5___default.a.sslSettings
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "sslSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSSLEnabled.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSSLEnabled")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.sslSettings.SSLEnabled,
            onChange: this.onSettingChange.bind(this, "SSLEnabled")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "sslSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSSLEnforced.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSSLEnforced")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.sslSettings.SSLEnforced,
            onChange: this.onSettingChange.bind(this, "SSLEnforced")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSSLURL.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSSLURL")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: false,
            value: state.sslSettings.SSLURL,
            onChange: this.onSettingChange.bind(this, "SSLURL")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSTDURL.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSTDURL")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: false,
            value: state.sslSettings.STDURL,
            onChange: this.onSettingChange.bind(this, "STDURL")
          })),
          /*eslint-disable eqeqeq*/
          state.sslSettings.SSLOffloadHeader != undefined &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSSLOffload.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plSSLOffload"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: false,
            value: state.sslSettings.SSLOffloadHeader,
            onChange: this.onSettingChange.bind(this, "SSLOffloadHeader")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "buttons-box"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.sslSettingsClientModified,
            type: "secondary",
            onClick: this.onCancel.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Cancel")),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.sslSettingsClientModified,
            type: "primary",
            onClick: this.onUpdate.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Save"))))
        );
      } else return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null)
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

  return SslSettingsPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

SslSettingsPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  sslSettings: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  sslSettingsClientModified: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    sslSettings: state.security.sslSettings,
    sslSettingsClientModified: state.security.sslSettingsClientModified
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(SslSettingsPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(SslSettingsPanelBody, "SslSettingsPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\sslSettings\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\sslSettings\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\sslSettings\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 50 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(17);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(7);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(1);
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










var re = /^[1-9][0-9]?[0-9]?$|^0$/;
var re2 = /^([9]\d|\d{3,4})$/;
var re3 = /^([1][2-9]|[2-9][0-9]|\d{3,})$/;

var OtherSettingsPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(OtherSettingsPanelBody, _Component);

  function OtherSettingsPanelBody() {
    var _this;

    _classCallCheck(this, OtherSettingsPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(OtherSettingsPanelBody).call(this));
    _this.state = {
      otherSettings: undefined,
      error: {
        autoAccountUnlockDuration: false,
        asyncTimeout: false,
        maxUploadSize: false
      },
      triedToSubmit: false
    };
    return _this;
  }

  _createClass(OtherSettingsPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (props.otherSettings) {
        this.setState({
          otherSettings: props.otherSettings
        });
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getOtherSettings(function (data) {
        var otherSettings = Object.assign({}, data.Results.Settings);

        _this2.setState({
          otherSettings: otherSettings
        });
      }));
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      var state = this.state;
      var autoAccountUnlockDuration = props.otherSettings["AutoAccountUnlockDuration"];

      if (autoAccountUnlockDuration === "" || !re.test(autoAccountUnlockDuration)) {
        state.error["autoAccountUnlockDuration"] = true;
      } else if (autoAccountUnlockDuration !== "" && re.test(autoAccountUnlockDuration)) {
        state.error["autoAccountUnlockDuration"] = false;
      }

      var asyncTimeout = props.otherSettings["AsyncTimeout"];

      if (asyncTimeout === "" || !re2.test(asyncTimeout)) {
        state.error["asyncTimeout"] = true;
      } else if (asyncTimeout !== "" && re2.test(asyncTimeout)) {
        state.error["asyncTimeout"] = false;
      }

      var maxUploadSize = props.otherSettings["MaxUploadSize"];

      if (!this.isMaxUploadSizeValid(maxUploadSize)) {
        state.error["maxUploadSize"] = true;
      } else if (this.isMaxUploadSizeValid(maxUploadSize)) {
        state.error["maxUploadSize"] = false;
      }

      this.setState({
        otherSettings: Object.assign({}, props.otherSettings),
        triedToSubmit: false,
        error: state.error
      });
    }
  }, {
    key: "isMaxUploadSizeValid",
    value: function isMaxUploadSizeValid(size) {
      var props = this.props;

      if (props.otherSettings === undefined) {
        return true;
      }

      var rangeUploadSize = parseInt(props.otherSettings["RangeUploadSize"]);

      if (size === "") {
        return false;
      } else {
        var maxUploadSize = parseInt(size);

        if (!re3.test(maxUploadSize) || maxUploadSize > rangeUploadSize) {
          return false;
        } else {
          return true;
        }
      }
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var state = this.state,
          props = this.props;
      var otherSettings = Object.assign({}, state.otherSettings);
      otherSettings[key] = _typeof(event) === "object" ? event.target.value : event;

      if (!re.test(otherSettings[key]) && key === "AutoAccountUnlockDuration") {
        state.error["autoAccountUnlockDuration"] = true;
      } else if (re.test(otherSettings[key]) && key === "AutoAccountUnlockDuration") {
        state.error["autoAccountUnlockDuration"] = false;
      }

      if (!re2.test(otherSettings[key]) && key === "AsyncTimeout") {
        state.error["asyncTimeout"] = true;
      } else if (re2.test(otherSettings[key]) && key === "AsyncTimeout") {
        state.error["asyncTimeout"] = false;
      }

      if (key === "MaxUploadSize" && !this.isMaxUploadSizeValid(otherSettings[key])) {
        state.error["maxUploadSize"] = true;
      } else if (key === "MaxUploadSize" && this.isMaxUploadSizeValid(otherSettings[key])) {
        state.error["maxUploadSize"] = false;
      }

      this.setState({
        otherSettings: otherSettings,
        error: state.error,
        triedToSubmit: false
      });
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].otherSettingsClientModified(otherSettings));
    }
  }, {
    key: "onUpdate",
    value: function onUpdate(event) {
      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.setState({
        triedToSubmit: true
      });

      if (state.error.autoAccountUnlockDuration || state.error.asyncTimeout || state.error.maxUploadSize) {
        return;
      }

      var parameters = Object.assign({}, state.otherSettings);
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updateOtherSettings(parameters, function () {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("OtherSettingsUpdateSuccess"));
      }, function () {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notifyError(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("OtherSettingsError"));
      }));
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var _this3 = this;

      var props = this.props;
      _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.confirm(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("OtherSettingsRestoreWarning"), _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Yes"), _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("No"), function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getOtherSettings(function (data) {
          var otherSettings = Object.assign({}, data.Results.Settings);

          _this3.setState({
            otherSettings: otherSettings
          });
        }));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var state = this.state;

      if (state.otherSettings) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: _style_less__WEBPACK_IMPORTED_MODULE_5___default.a.otherSettings
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "otherSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plShowCriticalErrors.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plShowCriticalErrors"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.otherSettings.ShowCriticalErrors,
            onChange: this.onSettingChange.bind(this, "ShowCriticalErrors")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "otherSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plDebugMode.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plDebugMode"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.otherSettings.DebugMode,
            onChange: this.onSettingChange.bind(this, "DebugMode")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "otherSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plRememberMe.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plRememberMe"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.otherSettings.RememberCheckbox,
            onChange: this.onSettingChange.bind(this, "RememberCheckbox")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plAutoAccountUnlock.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plAutoAccountUnlock"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.autoAccountUnlockDuration && this.state.triedToSubmit,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("AutoAccountUnlockDuration.ErrorMessage"),
            value: state.otherSettings.AutoAccountUnlockDuration,
            onChange: this.onSettingChange.bind(this, "AutoAccountUnlockDuration")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plAsyncTimeout.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plAsyncTimeout"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.asyncTimeout && this.state.triedToSubmit,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("AsyncTimeout.ErrorMessage"),
            value: state.otherSettings.AsyncTimeout,
            onChange: this.onSettingChange.bind(this, "AsyncTimeout")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plMaxUploadSize.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plMaxUploadSize"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.maxUploadSize && this.state.triedToSubmit,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("maxUploadSize.Error").replace("{0}", state.otherSettings.RangeUploadSize),
            value: state.otherSettings.MaxUploadSize,
            onChange: this.onSettingChange.bind(this, "MaxUploadSize")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plFileExtensions.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plFileExtensions"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["MultiLineInputWithError"], {
            value: state.otherSettings.AllowedExtensionWhitelist,
            onChange: this.onSettingChange.bind(this, "AllowedExtensionWhitelist")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plDefaultEndUserExtensionWhitelist.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plDefaultEndUserExtensionWhitelist"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["MultiLineInputWithError"], {
            value: state.otherSettings.DefaultEndUserExtensionWhitelist,
            onChange: this.onSettingChange.bind(this, "DefaultEndUserExtensionWhitelist")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "buttons-box"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.otherSettingsClientModified,
            type: "secondary",
            onClick: this.onCancel.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Cancel")),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.otherSettingsClientModified,
            type: "primary",
            onClick: this.onUpdate.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Save"))))
        );
      } else return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null)
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

  return OtherSettingsPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

OtherSettingsPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  otherSettings: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  otherSettingsClientModified: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    otherSettings: state.security.otherSettings,
    otherSettingsClientModified: state.security.otherSettingsClientModified
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(OtherSettingsPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(re, "re", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\otherSettings\\index.jsx");
  reactHotLoader.register(re2, "re2", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\otherSettings\\index.jsx");
  reactHotLoader.register(re3, "re3", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\otherSettings\\index.jsx");
  reactHotLoader.register(OtherSettingsPanelBody, "OtherSettingsPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\otherSettings\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\otherSettings\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\otherSettings\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 51 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _ipFilterRow__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(25);
/* harmony import */ var _ipfilterEditor__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(26);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(18);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_6__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(7);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__);
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












/*eslint-disable quotes*/

var warningIcon = __webpack_require__(33).default;

var tableFields = [];

var IpFiltersPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(IpFiltersPanelBody, _Component);

  function IpFiltersPanelBody() {
    var _this;

    _classCallCheck(this, IpFiltersPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(IpFiltersPanelBody).call(this));
    _this.state = {
      openId: ""
    };
    return _this;
  }

  _createClass(IpFiltersPanelBody, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getIpFilters());
      tableFields = [];
      tableFields.push({
        "name": _resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("FilterType.Header"),
        "id": "RuleType"
      });
      tableFields.push({
        "name": _resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("IpAddress.Header"),
        "id": "IPAddress"
      });
    }
  }, {
    key: "renderHeader",
    value: function renderHeader() {
      var tableHeaders = tableFields.map(function (field) {
        var className = "ip-filter-items header-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: "header-" + field.id
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "header-row"
        }, tableHeaders)
      );
    }
  }, {
    key: "uncollapse",
    value: function uncollapse(id) {
      this.setState({
        openId: id
      });
    }
  }, {
    key: "collapse",
    value: function collapse() {
      if (this.state.openId !== "") {
        this.setState({
          openId: ""
        });
      }
    }
  }, {
    key: "toggle",
    value: function toggle(openId) {
      if (openId !== "") {
        this.uncollapse(openId);
      }
    }
  }, {
    key: "onUpdateIpFilter",
    value: function onUpdateIpFilter(ipFilter) {
      var _this2 = this;

      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updateIpFilter(ipFilter, function () {
        _utils__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("IpFilterUpdateSuccess"));

        _this2.collapse();

        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getIpFilters());
      }, function () {
        _utils__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].utilities.notifyError(_resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("IpFilterUpdateError"));
      }));
    }
  }, {
    key: "onDeleteIpFilter",
    value: function onDeleteIpFilter(ipFilterId) {
      var _this3 = this;

      var props = this.props;
      _utils__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].utilities.confirm(_resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("IpFilterDeletedWarning"), _resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("Yes"), _resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("No"), function () {
        var itemList = props.ipFilters.filter(function (item) {
          return item.IPFilterID !== ipFilterId;
        });
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].deleteIpFilter(ipFilterId, itemList, function () {
          _utils__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("DeleteSuccess"));

          _this3.collapse();
        }, function () {
          _utils__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].utilities.notifyError(_resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("DeleteError"));
        }));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "renderedIpFilters",
    value: function renderedIpFilters() {
      var _this4 = this;

      var i = 0;
      return this.props.ipFilters.map(function (item, index) {
        var id = "row-" + i++;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_ipFilterRow__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
            ipFilterId: item.IPFilterID,
            ruleType: item.RuleType,
            ipFilter: item.IPFilter,
            index: index,
            key: "ipFilter-" + index,
            closeOnClick: true,
            openId: _this4.state.openId,
            OpenCollapse: _this4.toggle.bind(_this4),
            Collapse: _this4.collapse.bind(_this4),
            onDelete: _this4.onDeleteIpFilter.bind(_this4, item.IPFilterID),
            id: id
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_ipfilterEditor__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], {
            ipFilterId: item.IPFilterID,
            Collapse: _this4.collapse.bind(_this4),
            onUpdate: _this4.onUpdateIpFilter.bind(_this4),
            id: id,
            openId: _this4.state.openId
          }))
        );
      });
    }
  }, {
    key: "render",
    value: function render() {
      var opened = this.state.openId === "add";

      if (this.props.ipFilters) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: _style_less__WEBPACK_IMPORTED_MODULE_6___default.a.ipFilterItems
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "ip-filter-topbar"
          }, !this.props.enableIPChecking &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "warning-container"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "warning-icon",
            dangerouslySetInnerHTML: {
              __html: warningIcon
            }
          }), _resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("IPFiltersDisabled")),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "AddItemRow"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: opened ? "AddItemBox-active" : "AddItemBox",
            onClick: this.toggle.bind(this, opened ? "" : "add")
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "add-icon",
            dangerouslySetInnerHTML: {
              __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["SvgIcons"].AddIcon
            }
          }), " ", _resources__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].get("cmdAdd")))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "ip-filter-items-grid"
          }, this.renderHeader(),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_ipFilterRow__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
            ipFilterId: "-",
            ruleType: "-",
            ipFilter: "-",
            index: "add",
            key: "ipFilter-add",
            closeOnClick: true,
            openId: this.state.openId,
            OpenCollapse: this.toggle.bind(this),
            Collapse: this.collapse.bind(this),
            onDelete: this.onDeleteIpFilter.bind(this),
            id: "add",
            visible: opened
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_ipfilterEditor__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], {
            Collapse: this.collapse.bind(this),
            onUpdate: this.onUpdateIpFilter.bind(this),
            id: "add",
            openId: this.state.openId
          })), this.renderedIpFilters()))
        );
      } else return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null)
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

  return IpFiltersPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

IpFiltersPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  ipFilters: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  enableIPChecking: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    ipFilters: state.security.ipFilters,
    enableIPChecking: state.security.enableIPChecking
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(IpFiltersPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(warningIcon, "warningIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\index.jsx");
  reactHotLoader.register(tableFields, "tableFields", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\index.jsx");
  reactHotLoader.register(IpFiltersPanelBody, "IpFiltersPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\ipFilters\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 52 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(7);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(1);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(53);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_7__);
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









var re = /^[1-9][0-9]?[0-9]?[0-9]?$/;
var re2 = /^[1-9][0-9]?[0-9]?[0-9]?$|^0$/;

var MemberManagementPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(MemberManagementPanelBody, _Component);

  function MemberManagementPanelBody() {
    var _this;

    _classCallCheck(this, MemberManagementPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(MemberManagementPanelBody).call(this));
    _this.state = {
      memberSettings: undefined,
      error: {
        membershipResetLinkValidity: true,
        adminMembershipResetLinkValidity: true,
        membershipNumberPasswords: true,
        membershipDaysBeforePasswordReuse: true,
        passwordExpiry: true,
        passwordExpiryReminder: true
      },
      triedToSubmit: false
    };
    return _this;
  }

  _createClass(MemberManagementPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (props.memberSettings) {
        this.updateMemberSettings(props.memberSettings);
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getMemberSettings(function (data) {
        _this2.updateMemberSettings(data.Results.Settings);
      }));
    }
  }, {
    key: "updateMemberSettings",
    value: function updateMemberSettings(memberSettings) {
      if (!memberSettings) {
        return;
      }

      var state = this.state;
      var membershipResetLinkValidity = memberSettings["MembershipResetLinkValidity"];

      if (membershipResetLinkValidity === "" || !re.test(membershipResetLinkValidity)) {
        state.error["membershipResetLinkValidity"] = true;
      } else if (membershipResetLinkValidity !== "" && re.test(membershipResetLinkValidity)) {
        state.error["membershipResetLinkValidity"] = false;
      }

      var adminMembershipResetLinkValidity = memberSettings["AdminMembershipResetLinkValidity"];

      if (adminMembershipResetLinkValidity === "" || !re.test(adminMembershipResetLinkValidity)) {
        state.error["adminMembershipResetLinkValidity"] = true;
      } else if (adminMembershipResetLinkValidity !== "" && re.test(adminMembershipResetLinkValidity)) {
        state.error["adminMembershipResetLinkValidity"] = false;
      }

      var membershipNumberPasswords = memberSettings["MembershipNumberPasswords"];

      if (membershipNumberPasswords === "" || !re2.test(membershipNumberPasswords)) {
        state.error["membershipNumberPasswords"] = true;
      } else if (membershipNumberPasswords !== "" && re2.test(membershipNumberPasswords)) {
        state.error["membershipNumberPasswords"] = false;
      }

      var membershipDaysBeforePasswordReuse = memberSettings["MembershipDaysBeforePasswordReuse"];

      if (membershipDaysBeforePasswordReuse === "" || !re2.test(membershipDaysBeforePasswordReuse)) {
        state.error["membershipDaysBeforePasswordReuse"] = true;
      } else if (membershipDaysBeforePasswordReuse !== "" && re2.test(membershipDaysBeforePasswordReuse)) {
        state.error["membershipDaysBeforePasswordReuse"] = false;
      }

      var passwordExpiry = memberSettings["PasswordExpiry"];

      if (passwordExpiry === "" || !re2.test(passwordExpiry)) {
        state.error["passwordExpiry"] = true;
      } else if (passwordExpiry !== "" && re2.test(passwordExpiry)) {
        state.error["passwordExpiry"] = false;
      }

      var passwordExpiryReminder = memberSettings["PasswordExpiryReminder"];

      if (passwordExpiryReminder === "" || !re2.test(passwordExpiryReminder)) {
        state.error["passwordExpiryReminder"] = true;
      } else if (passwordExpiryReminder !== "" && re2.test(passwordExpiryReminder)) {
        state.error["passwordExpiryReminder"] = false;
      }

      this.setState({
        memberSettings: Object.assign({}, memberSettings),
        triedToSubmit: false,
        error: state.error
      });
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      this.updateMemberSettings(props.memberSettings);
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var state = this.state,
          props = this.props;
      var memberSettings = Object.assign({}, state.memberSettings);
      memberSettings[key] = _typeof(event) === "object" ? event.target.value : event;
      this.updateMemberSettings(memberSettings);
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].memberSettingsClientModified(memberSettings));
    }
  }, {
    key: "onUpdate",
    value: function onUpdate(event) {
      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.setState({
        triedToSubmit: true
      });

      if (state.error.membershipResetLinkValidity || state.error.adminMembershipResetLinkValidity || state.error.membershipNumberPasswords || state.error.membershipDaysBeforePasswordReuse || state.error.passwordExpiry || state.error.passwordExpiryReminder) {
        return;
      }

      var parameters = Object.assign({}, state.memberSettings);
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updateMemberSettings(parameters, function () {
        _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("MemberSettingsUpdateSuccess"));
      }, function () {
        _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.notifyError(_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("MemberSettingsError"));
      }));
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var _this3 = this;

      var props = this.props;
      _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.confirm(_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("MemberSettingsRestoreWarning"), _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Yes"), _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("No"), function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getMemberSettings(function (data) {
          _this3.updateMemberSettings(data.Results.Settings);
        }));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var state = this.state;

      if (state.memberSettings) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: _style_less__WEBPACK_IMPORTED_MODULE_7___default.a.memberSettings
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plResetLinkValidity.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plResetLinkValidity"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.membershipResetLinkValidity && this.state.triedToSubmit,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("MembershipResetLinkValidity.ErrorMessage"),
            value: state.memberSettings.MembershipResetLinkValidity,
            onChange: this.onSettingChange.bind(this, "MembershipResetLinkValidity")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plAdminResetLinkValidity.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plAdminResetLinkValidity"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.adminMembershipResetLinkValidity && this.state.triedToSubmit,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("AdminMembershipResetLinkValidity.ErrorMessage"),
            value: state.memberSettings.AdminMembershipResetLinkValidity,
            onChange: this.onSettingChange.bind(this, "AdminMembershipResetLinkValidity")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "memberSettings-row_switch",
            style: {
              margin: "0 0 20px 0"
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEnablePasswordHistory.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEnablePasswordHistory"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOff"),
            value: state.memberSettings.EnablePasswordHistory,
            onChange: this.onSettingChange.bind(this, "EnablePasswordHistory")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plNumberPasswords.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plNumberPasswords"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.membershipNumberPasswords && this.state.triedToSubmit,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("MembershipNumberPasswords.ErrorMessage"),
            value: state.memberSettings.MembershipNumberPasswords,
            onChange: this.onSettingChange.bind(this, "MembershipNumberPasswords")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plPasswordDays.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plPasswordDays"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.membershipDaysBeforePasswordReuse && this.state.triedToSubmit,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("MembershipDaysBeforePasswordReuse.ErrorMessage"),
            value: state.memberSettings.MembershipDaysBeforePasswordReuse,
            onChange: this.onSettingChange.bind(this, "MembershipDaysBeforePasswordReuse")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "memberSettings-row_switch",
            style: {
              margin: "0 0 20px 0"
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEnableBannedList.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEnableBannedList"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOff"),
            value: state.memberSettings.EnableBannedList,
            onChange: this.onSettingChange.bind(this, "EnableBannedList")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "memberSettings-row_switch",
            style: {
              margin: "0 0 20px 0"
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEnableStrengthMeter.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEnableStrengthMeter"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOff"),
            value: state.memberSettings.EnableStrengthMeter,
            onChange: this.onSettingChange.bind(this, "EnableStrengthMeter")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "memberSettings-row_switch",
            style: {
              margin: "0 0 20px 0"
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEnableIPChecking.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEnableIPChecking"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOff"),
            value: state.memberSettings.EnableIPChecking,
            onChange: this.onSettingChange.bind(this, "EnableIPChecking")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "memberSettings-row_switch",
            style: {
              margin: "0 0 20px 0"
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plForceLogOutAfterPasswordChanged.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plForceLogOutAfterPasswordChanged"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SwitchOff"),
            value: state.memberSettings.ForceLogoutAfterPasswordChanged,
            onChange: this.onSettingChange.bind(this, "ForceLogoutAfterPasswordChanged")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PasswordConfig_PasswordExpiry.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PasswordConfig_PasswordExpiry"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.passwordExpiry && this.state.triedToSubmit,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PasswordExpiry.ErrorMessage"),
            value: state.memberSettings.PasswordExpiry,
            onChange: this.onSettingChange.bind(this, "PasswordExpiry")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PasswordConfig_PasswordExpiryReminder.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PasswordConfig_PasswordExpiryReminder"),
            extra:
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
              messages: [_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("GlobalSetting")],
              type: "global",
              style: {
                float: "left",
                position: "static"
              }
            })
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.passwordExpiryReminder && this.state.triedToSubmit,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PasswordExpiryReminder.ErrorMessage"),
            value: state.memberSettings.PasswordExpiryReminder,
            onChange: this.onSettingChange.bind(this, "PasswordExpiryReminder")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "buttons-box"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.memberSettingsClientModified,
            type: "secondary",
            onClick: this.onCancel.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Cancel")),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.memberSettingsClientModified,
            type: "primary",
            onClick: this.onUpdate.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Save"))))
        );
      } else return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null)
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

  return MemberManagementPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

MemberManagementPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  memberSettings: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  memberSettingsClientModified: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    memberSettings: state.security.memberSettings,
    memberSettingsClientModified: state.security.memberSettingsClientModified
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(MemberManagementPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(re, "re", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\memberManagement\\index.jsx");
  reactHotLoader.register(re2, "re2", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\memberManagement\\index.jsx");
  reactHotLoader.register(MemberManagementPanelBody, "MemberManagementPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\memberManagement\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\memberManagement\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\memberManagement\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 53 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(80);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 54 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(19);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(7);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(1);
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










var canEdit = false;
var reUserNameMinLength = /^[1-9][0-9]?[0-9]?$|^0$/;
/*eslint-disable eqeqeq*/

var RegistrationSettingsPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(RegistrationSettingsPanelBody, _Component);

  function RegistrationSettingsPanelBody() {
    var _this;

    _classCallCheck(this, RegistrationSettingsPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(RegistrationSettingsPanelBody).call(this));
    _this.state = {
      registrationSettings: undefined,
      triedToSubmit: false,
      error: {
        registrationFields: "",
        userNameMinLength: false,
        validationFields: ""
      }
    };
    canEdit = _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].settings.isHost || _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].settings.isAdmin || _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].settings.permissions.REGISTRATION_SETTINGS_EDIT;
    return _this;
  }

  _createClass(RegistrationSettingsPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (props.registrationSettings) {
        this.setState({
          registrationSettings: props.registrationSettings
        });
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getRegistrationSettings(function (data) {
        var registrationSettings = Object.assign({}, data.Results.Settings);

        _this2.setState({
          registrationSettings: registrationSettings
        });
      }));
    }
  }, {
    key: "UNSAFE_componentWillReceiveProps",
    value: function UNSAFE_componentWillReceiveProps(props) {
      this.setState({
        registrationSettings: Object.assign({}, props.registrationSettings),
        triedToSubmit: false
      });
    }
  }, {
    key: "onSettingChange",
    value: function onSettingChange(key, event) {
      var state = this.state,
          props = this.props;
      var registrationSettings = Object.assign({}, state.registrationSettings);

      if (key === "UserRegistration" || key === "RegistrationFormType") {
        registrationSettings[key] = parseInt(event);
      } else if (key === "RedirectAfterRegistrationTabId") {
        if (registrationSettings[key] !== parseInt(event)) {
          registrationSettings[key] = event;
        } else {
          return;
        }
      } else {
        registrationSettings[key] = _typeof(event) === "object" ? event.target.value : event;
      }

      if (key === "UserNameMinLength") {
        state.error.userNameMinLength = !reUserNameMinLength.test(registrationSettings[key]);
      }

      this.setState({
        registrationSettings: registrationSettings,
        triedToSubmit: false,
        error: state.error
      });
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].registrationSettingsClientModified(registrationSettings));
    }
  }, {
    key: "onUpdate",
    value: function onUpdate(event) {
      var _this3 = this;

      event.preventDefault();
      var props = this.props,
          state = this.state;
      state.error["validationFields"] = "";

      if (state.error.userNameMinLength) {
        // there is an error, so no saving
        return;
      }

      this.setState({
        triedToSubmit: true
      });
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updateRegistrationSettings(state.registrationSettings, function () {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("RegistrationSettingsUpdateSuccess"));
      }, function (error) {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notifyError(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("RegistrationSettingsError"));
        var errorMessage = JSON.parse(error.responseText);
        state.error["registrationFields"] = errorMessage.Message;
        state.error["validationFields"] = errorMessage.ModelState;

        _this3.setState({
          error: state.error
        });
      }));
    }
  }, {
    key: "onCancel",
    value: function onCancel() {
      var _this4 = this;

      var props = this.props;
      _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.confirm(_resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("RegistrationSettingsRestoreWarning"), _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Yes"), _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("No"), function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getRegistrationSettings(function (data) {
          var registrationSettings = Object.assign({}, data.Results.Settings);

          _this4.setState({
            registrationSettings: registrationSettings
          });
        }));
      });
    }
  }, {
    key: "keyValuePairsToOptions",
    value: function keyValuePairsToOptions(keyValuePairs) {
      var options = [];

      if (keyValuePairs !== undefined) {
        options = keyValuePairs.map(function (item) {
          return {
            label: item.Key,
            value: item.Value
          };
        });
      }

      return options;
    }
  }, {
    key: "isCustomFormType",
    value: function isCustomFormType() {
      var state = this.state;

      if (state.registrationSettings != undefined && state.registrationSettings.RegistrationFormType === 1) {
        return true;
      }

      return false;
    }
  }, {
    key: "getRegistrationFields",
    value: function getRegistrationFields(fields) {
      var fieldList = [];

      if (fields) {
        fieldList = fields.split(",").map(function (item) {
          return {
            id: item,
            name: item
          };
        });
      }

      return fieldList;
    }
  }, {
    key: "onUpdateTags",
    value: function onUpdateTags(event) {
      var state = this.state,
          props = this.props;
      var registrationSettings = Object.assign({}, state.registrationSettings);
      var fields = event.map(function (field) {
        return field.name;
      }).join(",");
      registrationSettings["RegistrationFields"] = fields;
      this.setState({
        registrationSettings: registrationSettings,
        triedToSubmit: false
      });
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].registrationSettingsClientModified(registrationSettings));
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var noneSpecifiedText = "<" + _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("NoneSpecified") + ">";
      var RedirectAfterRegistrationParameters = {
        portalId: -2,
        cultureCode: "",
        isMultiLanguage: false,
        excludeAdminTabs: false,
        disabledNotSelectable: false,
        roles: "1;-1",
        sortOrder: 0
      };
      var ToolTipStyle = {
        padding: "5px 0",
        float: "left"
      };

      if (state.registrationSettings) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: _style_less__WEBPACK_IMPORTED_MODULE_5___default.a.registrationSettings
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-options"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plUserRegistration.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plUserRegistration")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["RadioButtons"], {
            onChange: this.onSettingChange.bind(this, "UserRegistration"),
            options: this.keyValuePairsToOptions(props.userRegistrationOptions),
            buttonGroup: "registrationType",
            value: state.registrationSettings.UserRegistration,
            disabled: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], {
            style: {
              marginBottom: "5px"
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-options"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("registrationFormTypeLabel.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("registrationFormTypeLabel")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["RadioButtons"], {
            onChange: this.onSettingChange.bind(this, "RegistrationFormType"),
            options: this.keyValuePairsToOptions(props.registrationFormTypeOptions),
            buttonGroup: "formType",
            value: state.registrationSettings.RegistrationFormType,
            disabled: !canEdit
          }))), this.isCustomFormType() &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], {
            style: {
              marginBottom: "30px"
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-input"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("registrationFieldsLabel.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("registrationFieldsLabel")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SearchableTags"], {
            utils: _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"],
            tags: this.getRegistrationFields(state.registrationSettings.RegistrationFields),
            onUpdateTags: this.onUpdateTags.bind(this),
            error: this.state.error.registrationFields !== "",
            errorMessage: this.state.error.registrationFields,
            enabled: canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-input"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_DisplayNameFormat.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_DisplayNameFormat")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            error: false,
            value: state.registrationSettings.DisplayNameFormat,
            onChange: this.onSettingChange.bind(this, "DisplayNameFormat"),
            enabled: canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-input"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_UserNameValidation.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_UserNameValidation")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            error: false,
            value: state.registrationSettings.UserNameValidation,
            onChange: this.onSettingChange.bind(this, "UserNameValidation"),
            enabled: canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_UserNameMinLength.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_UserNameMinLength")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            withLabel: false,
            error: this.state.error.userNameMinLength,
            errorMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_UserNameMinLength.ErrorMessage"),
            value: state.registrationSettings.UserNameMinLength,
            onChange: this.onSettingChange.bind(this, "UserNameMinLength")
          })),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-input"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_EmailValidation.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_EmailValidation")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            error: false,
            withLabel: false,
            value: state.registrationSettings.EmailAddressValidation,
            onChange: this.onSettingChange.bind(this, "EmailAddressValidation"),
            enabled: canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-input"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_ExcludeTerms.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_ExcludeTerms")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
            error: false,
            withLabel: false,
            value: state.registrationSettings.ExcludedTerms,
            onChange: this.onSettingChange.bind(this, "ExcludedTerms"),
            enabled: canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            style: {
              paddingBottom: "15px",
              fontStyle: "italic"
            }
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("RedirectionMovedToSiteSettings")),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plEnableRegisterNotification.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plEnableRegisterNotification")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.registrationSettings.EnableRegisterNotification,
            onChange: this.onSettingChange.bind(this, "EnableRegisterNotification"),
            readOnly: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_UseAuthProviders.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_UseAuthProviders")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.registrationSettings.UseAuthenticationProviders,
            onChange: this.onSettingChange.bind(this, "UseAuthenticationProviders"),
            readOnly: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_UseProfanityFilter.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_UseProfanityFilter")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.registrationSettings.UseProfanityFilter,
            onChange: this.onSettingChange.bind(this, "UseProfanityFilter"),
            readOnly: !canEdit
          }))), !this.isCustomFormType() &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_UseEmailAsUserName.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_UseEmailAsUserName")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.registrationSettings.UseEmailAsUsername,
            onChange: this.onSettingChange.bind(this, "UseEmailAsUsername"),
            readOnly: !canEdit
          }), this.state.error.validationFields["request.UseEmailAsUsername"] !== "" &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Tooltip"], {
            messages: [this.state.error.validationFields["request.UseEmailAsUsername"]],
            type: "error",
            tooltipPlace: "top",
            style: ToolTipStyle,
            rendered: this.state.error.validationFields["request.UseEmailAsUsername"] !== ""
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_RequireUniqueDisplayName.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_RequireUniqueDisplayName")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.registrationSettings.RequireUniqueDisplayName,
            onChange: this.onSettingChange.bind(this, "RequireUniqueDisplayName"),
            readOnly: !canEdit
          }))), !this.isCustomFormType() &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_RandomPassword.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_RandomPassword")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.registrationSettings.UseRandomPassword,
            onChange: this.onSettingChange.bind(this, "UseRandomPassword"),
            readOnly: !canEdit
          }))), !this.isCustomFormType() &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_RequireConfirmPassword.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Registration_RequireConfirmPassword")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.registrationSettings.RequirePasswordConfirmation,
            onChange: this.onSettingChange.bind(this, "RequirePasswordConfirmation"),
            readOnly: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_RequireValidProfile.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_RequireValidProfile")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.registrationSettings.RequireValidProfile,
            onChange: this.onSettingChange.bind(this, "RequireValidProfile"),
            readOnly: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row_switch"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_CaptchaRegister.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Security_CaptchaRegister")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
            onText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
            offText: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff"),
            value: state.registrationSettings.UseCaptchaRegister,
            onChange: this.onSettingChange.bind(this, "UseCaptchaRegister"),
            readOnly: !canEdit
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("RequiresUniqueEmail.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("RequiresUniqueEmail")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.RequiresUniqueEmail))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordFormat.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordFormat")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.PasswordFormat))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordRetrievalEnabled.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordRetrievalEnabled")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.PasswordRetrievalEnabled))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordResetEnabledTitle.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordResetEnabledTitle")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.PasswordResetEnabled))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("MinPasswordLengthTitle.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("MinPasswordLengthTitle")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.MinPasswordLength))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("MinNonAlphanumericCharactersTitle.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("MinNonAlphanumericCharactersTitle")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.MinNonAlphanumericCharacters))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("RequiresQuestionAndAnswerTitle.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("RequiresQuestionAndAnswerTitle")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.RequiresQuestionAndAnswer))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordStrengthRegularExpressionTitle.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordStrengthRegularExpressionTitle")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.PasswordStrengthRegularExpression))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("MaxInvalidPasswordAttemptsTitle.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("MaxInvalidPasswordAttemptsTitle")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.MaxInvalidPasswordAttempts))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["InputGroup"], null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Label"], {
            labelType: "inline",
            tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordAttemptWindowTitle.Help"),
            label: _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("PasswordAttemptWindowTitle")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "registrationSettings-row-static-text"
          }, state.registrationSettings.PasswordAttemptWindow))), canEdit &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "buttons-box"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.registrationSettingsClientModified,
            type: "secondary",
            onClick: this.onCancel.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Cancel")),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
            disabled: !this.props.registrationSettingsClientModified,
            type: "primary",
            onClick: this.onUpdate.bind(this)
          }, _resources__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Save"))))
        );
      } else return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null)
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

  return RegistrationSettingsPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

RegistrationSettingsPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  registrationSettings: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  userRegistrationOptions: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  registrationFormTypeOptions: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  registrationSettingsClientModified: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    registrationSettings: state.security.registrationSettings,
    userRegistrationOptions: state.security.userRegistrationOptions,
    registrationFormTypeOptions: state.security.registrationFormTypeOptions,
    registrationSettingsClientModified: state.security.registrationSettingsClientModified
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(RegistrationSettingsPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(canEdit, "canEdit", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\registrationSettings\\index.jsx");
  reactHotLoader.register(reUserNameMinLength, "reUserNameMinLength", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\registrationSettings\\index.jsx");
  reactHotLoader.register(RegistrationSettingsPanelBody, "RegistrationSettingsPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\registrationSettings\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\registrationSettings\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\registrationSettings\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 55 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _bulletinItemRow__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(56);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(14);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(1);
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









/*eslint-disable quotes*/

var warningIcon = __webpack_require__(33).default;

var SecurityBulletinsPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(SecurityBulletinsPanelBody, _Component);

  function SecurityBulletinsPanelBody() {
    var _this;

    _classCallCheck(this, SecurityBulletinsPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(SecurityBulletinsPanelBody).call(this));
    _this.state = {
      error: undefined
    };
    return _this;
  }

  _createClass(SecurityBulletinsPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (props.securityBulletins) {
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getSecurityBulletins(function () {}, function (error) {
        var errorMessage = JSON.parse(error.responseText);

        _this2.setState({
          error: errorMessage.Message
        });
      }));
    }
  }, {
    key: "renderHeader",
    value: function renderHeader() {
      var tableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Bulletins"),
        "id": "Bulletins"
      }];
      var tableHeaders = tableFields.map(function (field, i) {
        var className = "bulletinsHeader bulletinsHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "bulletinsHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedList",
    value: function renderedList() {
      var props = this.props;

      if (props.securityBulletins) {
        return props.securityBulletins.map(function (term, index) {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_bulletinItemRow__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
              className: "bulletin-detail-wrapper",
              pubDate: term.PubDate,
              title: term.Title,
              link: term.Link,
              description: term.Description,
              author: term.Author,
              index: index,
              key: "bulletin-" + index,
              closeOnClick: true
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "bulletin-detail"
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              style: {
                fontWeight: "bolder",
                margin: "0 0 15px 0"
              }
            }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("BulletinDescription")),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "bulletin-detail-desc"
            }, term.Description),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              style: {
                fontWeight: "bolder",
                margin: "15px 0 15px 0"
              }
            }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("BulletinLink")),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "bulletin-detail-link"
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("a", {
              target: "_blank",
              href: term.Link,
              rel: "noopener noreferrer"
            }, term.Link)),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              style: {
                height: "20px"
              }
            }, "\xA0")))
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

      if (state.error) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: _style_less__WEBPACK_IMPORTED_MODULE_5___default.a.bulletins
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "warning-container"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "warning-icon",
            dangerouslySetInnerHTML: {
              __html: warningIcon
            }
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "warning-msg"
          }, state.error)))
        );
      } else {
        if (props.securityBulletins && props.securityBulletins.length > 0) {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: _style_less__WEBPACK_IMPORTED_MODULE_5___default.a.bulletins
            }, this.renderHeader(), this.renderedList())
          );
        } else {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              style: {
                margin: "25px"
              }
            }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("BulletinsDoNotExist").replace("{0}", props.platformVersion))
          );
        }
      }
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return SecurityBulletinsPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

SecurityBulletinsPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  platformVersion: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  securityBulletins: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    platformVersion: state.security.platformVersion,
    securityBulletins: state.security.securityBulletins
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(SecurityBulletinsPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(warningIcon, "warningIcon", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\securityBulletins\\index.jsx");
  reactHotLoader.register(SecurityBulletinsPanelBody, "SecurityBulletinsPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\securityBulletins\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\securityBulletins\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\securityBulletins\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 56 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(82);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
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






var BulletinItemRow =
/*#__PURE__*/
function (_Component) {
  _inherits(BulletinItemRow, _Component);

  function BulletinItemRow() {
    var _this;

    _classCallCheck(this, BulletinItemRow);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(BulletinItemRow).call(this));
    _this.state = {
      collapsed: true,
      collapsedClass: true,
      repainting: false
    };
    return _this;
  }

  _createClass(BulletinItemRow, [{
    key: "uncollapse",
    value: function uncollapse() {
      this.setState({
        collapsed: false
      });
    }
  }, {
    key: "collapse",
    value: function collapse() {
      this.setState({
        collapsed: true
      });
    }
  }, {
    key: "toggle",
    value: function toggle() {
      if (this.state.collapsed) {
        this.uncollapse();
      } else {
        this.collapse();
      }
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "bulletinItem"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "collapsible-bulletinitemdetail-header " + state.collapsed
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-header"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-title",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, this.props.title, "\xA0 "))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-date",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, this.props.pubDate, "\xA0 "))))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["Collapsible"], {
          className: this.props.className,
          isOpened: !this.state.collapsed
        }, !state.collapsed && props.children))
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

  return BulletinItemRow;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

BulletinItemRow.propTypes = {
  pubDate: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  title: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  link: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  description: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  author: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  label: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node,
  children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node,
  disabled: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};
var _default = BulletinItemRow;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(BulletinItemRow, "BulletinItemRow", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\securityBulletins\\bulletinItemRow\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\securityBulletins\\bulletinItemRow\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 57 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(20);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(1);
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









var SuperuserActivityPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(SuperuserActivityPanelBody, _Component);

  function SuperuserActivityPanelBody() {
    _classCallCheck(this, SuperuserActivityPanelBody);

    return _possibleConstructorReturn(this, _getPrototypeOf(SuperuserActivityPanelBody).call(this));
  }

  _createClass(SuperuserActivityPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;

      if (props.activities) {
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getSuperuserActivities());
    }
  }, {
    key: "renderHeader",
    value: function renderHeader() {
      var tableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Username"),
        "id": "Username"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("CreatedDate"),
        "id": "CreatedDate"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("LastLogin"),
        "id": "LastLogin"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("LastActivityDate"),
        "id": "LastActivityDate"
      }];
      var tableHeaders = tableFields.map(function (field, i) {
        var className = "activityHeader activityHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "activityHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedList",
    value: function renderedList() {
      var props = this.props;
      return props.activities.map(function (term, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "activityItem",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-username"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.Username, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-createddate"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.CreatedDate, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-lastlogindate"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastLoginDate, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-lastactivitydate"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastActivityDate, "\xA0 "))))
        );
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;

      if (props.activities) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: _style_less__WEBPACK_IMPORTED_MODULE_4___default.a.activities
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "activities-topbar"
          }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SuperUserActivityExplaination")),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "activityItems"
          }, this.renderHeader(), this.renderedList()))
        );
      } else return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null)
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

  return SuperuserActivityPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

SuperuserActivityPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  activities: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    activities: state.security.activities
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(SuperuserActivityPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(SuperuserActivityPanelBody, "SuperuserActivityPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\superuserActivity\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\superuserActivity\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\superuserActivity\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 58 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(21);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(1);
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

/* eslint-disable react/no-danger */









var AuditCheckPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(AuditCheckPanelBody, _Component);

  function AuditCheckPanelBody() {
    var _this;

    _classCallCheck(this, AuditCheckPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(AuditCheckPanelBody).call(this));
    _this.state = {
      checking: false
    };
    return _this;
  }

  _createClass(AuditCheckPanelBody, [{
    key: "UNSAFE_componentWillMount",
    value: function UNSAFE_componentWillMount() {
      var props = this.props;

      if (props.auditCheckResults) {
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getAuditCheckResults());
    }
  }, {
    key: "renderHeader",
    value: function renderHeader() {
      var tableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SecurityCheck"),
        "id": "SecurityCheck"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Result"),
        "id": "Result"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Notes"),
        "id": "Notes"
      }];
      var tableHeaders = tableFields.map(function (field, i) {
        var className = "auditCheckHeader auditCheckHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "auditCheckHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "getResultDisplay",
    value: function getResultDisplay(result) {
      var severity = result.Severity;
      var successText = result.SuccessText;
      var failureText = result.FailureText;
      var reason = result.Reason;

      switch (severity) {
        case 0:
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "label-result-severity"
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "label-result-severity-pass"
            }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Pass")),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, successText))
          );

        case 1:
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "label-result-severity"
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "label-result-severity-alert"
            }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Alert")),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, failureText))
          );

        case 2:
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "label-result-severity"
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "label-result-severity-fail"
            }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Fail")),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              dangerouslySetInnerHTML: {
                __html: failureText
              }
            }))
          );

        default:
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "label-result-severity"
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "label-result-severity-unverified",
              onClick: this.onAuditCheck.bind(this, result)
            }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Check")),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, reason))
          );
      }
    }
  }, {
    key: "onAuditCheck",
    value: function onAuditCheck(result) {
      var _this2 = this;

      var state = this.state,
          props = this.props;

      if (state.checking) {
        return;
      }

      this.setState({
        checking: true
      }, function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getAuditCheckResult(result.CheckName, function () {
          _this2.setState({
            checking: false
          });
        }));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "getNotesDisplay",
    value: function getNotesDisplay(notes) {
      if (notes && notes.length > 0) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "log-detail",
            dangerouslySetInnerHTML: {
              __html: notes
            }
          })
        );
      } else {
        return "N/A";
      }
    }
  }, {
    key: "renderedList",
    value: function renderedList() {
      var _this3 = this;

      var props = this.props;
      return props.auditCheckResults.map(function (term, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "auditCheckItem",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-name"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.CheckNameText, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-result"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, _this3.getResultDisplay(term), "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-notes"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, _this3.getNotesDisplay(term.Notes), "\xA0 "))))
        );
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      var contentShouldShow = props.auditCheckResults && props.auditCheckResults.length > 0 ? true : false;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["ContentLoadWrapper"], {
          loadComplete: contentShouldShow,
          svgSkeleton:
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            dangerouslySetInnerHTML: {
              __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["SvgIcons"].TableEmptyState
            }
          })
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: _style_less__WEBPACK_IMPORTED_MODULE_4___default.a.auditCheckResults
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "auditcheck-topbar"
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("AuditExplanation")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "auditCheckItems"
        }, contentShouldShow && this.renderHeader(), contentShouldShow && this.renderedList())))
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

  return AuditCheckPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

AuditCheckPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  auditCheckResults: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    auditCheckResults: state.security.auditCheckResults
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(AuditCheckPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(AuditCheckPanelBody, "AuditCheckPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\auditCheck\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\auditCheck\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\auditCheck\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 59 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(6);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(22);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(1);
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










var ScannerCheckPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(ScannerCheckPanelBody, _Component);

  function ScannerCheckPanelBody() {
    _classCallCheck(this, ScannerCheckPanelBody);

    return _possibleConstructorReturn(this, _getPrototypeOf(ScannerCheckPanelBody).call(this));
  }

  _createClass(ScannerCheckPanelBody, [{
    key: "renderFileHeader",
    value: function renderFileHeader() {
      var fileTableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("FileName"),
        "id": "FileName"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LastWriteTime"),
        "id": "LastWriteTime"
      }];
      var tableHeaders = fileTableFields.map(function (field, i) {
        var className = "scannerCheckHeader scannerCheckHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedFileList",
    value: function renderedFileList() {
      var props = this.props;
      return props.searchResults.FoundInFiles.map(function (term, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "scannerCheckItem",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-name"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.FileName, "\xA0"))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-date"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastWriteTime, "\xA0"))))
        );
      });
    }
  }, {
    key: "renderDatabaseHeader",
    value: function renderDatabaseHeader() {
      var fileTableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("DatabaseInstance"),
        "id": "DatabaseInstance"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("DatabaseValue"),
        "id": "DatabaseValue"
      }];
      var tableHeaders = fileTableFields.map(function (field, i) {
        var className = "scannerCheckHeader scannerCheckHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedDatabaseList",
    value: function renderedDatabaseList() {
      var props = this.props;
      return props.searchResults.FoundInDatabase.map(function (term, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "scannerCheckItem",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-columnname"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.ColumnName, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-columnvalue"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.ColumnValue, "\xA0 "))))
        );
      });
    }
  }, {
    key: "renderRiskFilesHeader",
    value: function renderRiskFilesHeader() {
      var fileTableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("FileName"),
        "id": "FileName"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LastWriteTime"),
        "id": "LastWriteTime"
      }];
      var tableHeaders = fileTableFields.map(function (field, i) {
        var className = "scannerCheckHeader scannerCheckHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderRiskFilesList",
    value: function renderRiskFilesList(risk) {
      var props = this.props;
      var list = props.modifiedFiles.HighRiskFiles;

      if (risk === "low") {
        list = props.modifiedFiles.LowRiskFiles;
      }

      return list.map(function (term, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "scannerCheckItem",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-name"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.FilePath, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-date"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastWriteTime, "\xA0 "))))
        );
      });
    }
  }, {
    key: "renderPortalSettingsHeader",
    value: function renderPortalSettingsHeader() {
      var fileTableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PortalId"),
        "id": "PortalId"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SettingName"),
        "id": "SettingName"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SettingValue"),
        "id": "SettingValue"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("UserId"),
        "id": "UserId"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LastWriteTime"),
        "id": "LastWriteTime"
      }];
      var tableHeaders = fileTableFields.map(function (field, i) {
        var className = "scannerCheckHeader scannerCheckHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedPortalSettingsList",
    value: function renderedPortalSettingsList() {
      var props = this.props;
      return props.modifiedSettings.PortalSettings.map(function (term, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "scannerCheckItem",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-id"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.PortalId, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-name"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.SettingName, "\xA0"))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-value"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.SettingValue, "\xA0"))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-user"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastModifiedByUserId, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-date"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastModifiedOnDate, "\xA0 "))))
        );
      });
    }
  }, {
    key: "renderHostSettingsHeader",
    value: function renderHostSettingsHeader() {
      var fileTableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SettingName"),
        "id": "SettingName"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SettingValue"),
        "id": "SettingValue"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("UserId"),
        "id": "UserId"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LastWriteTime"),
        "id": "LastWriteTime"
      }];
      var tableHeaders = fileTableFields.map(function (field, i) {
        var className = "scannerCheckHeader scannerCheckHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedHostSettingsList",
    value: function renderedHostSettingsList() {
      var props = this.props;
      return props.modifiedSettings.PortalSettings.map(function (term, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "scannerCheckItem",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-name"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.SettingName, "\xA0"))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-value"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.SettingValue, "\xA0"))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-user"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastModifiedByUserId, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-date"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastModifiedOnDate, "\xA0 "))))
        );
      });
    }
  }, {
    key: "renderTabSettingsHeader",
    value: function renderTabSettingsHeader() {
      var fileTableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("TabId"),
        "id": "TabId"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PortalId"),
        "id": "PortalId"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SettingName"),
        "id": "SettingName"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SettingValue"),
        "id": "SettingValue"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("UserId"),
        "id": "UserId"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LastWriteTime"),
        "id": "LastWriteTime"
      }];
      var tableHeaders = fileTableFields.map(function (field, i) {
        var className = "scannerCheckHeader scannerCheckHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedTabSettingsList",
    value: function renderedTabSettingsList() {
      var props = this.props;
      return props.modifiedSettings.TabSettings.map(function (term, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "scannerCheckItem",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-tab"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.TabId, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-id"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.PortalId, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-name"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.SettingName, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-value"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.SettingValue, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-user"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastModifiedByUserId, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-date"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastModifiedOnDate, "\xA0 "))))
        );
      });
    }
  }, {
    key: "renderModuleSettingsHeader",
    value: function renderModuleSettingsHeader() {
      var fileTableFields = [{
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Type"),
        "id": "Type"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ModuleId"),
        "id": "ModuleId"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PortalId"),
        "id": "PortalId"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SettingName"),
        "id": "SettingName"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SettingValue"),
        "id": "SettingValue"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("UserId"),
        "id": "UserId"
      }, {
        "name": _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LastWriteTime"),
        "id": "LastWriteTime"
      }];
      var tableHeaders = fileTableFields.map(function (field, i) {
        var className = "scannerCheckHeader scannerCheckHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedModuleSettingsList",
    value: function renderedModuleSettingsList() {
      var props = this.props;
      return props.modifiedSettings.ModuleSettings.map(function (term, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "scannerCheckItem",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-type"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.Type, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-module"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.ModuleId, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-id"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.PortalId, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-name"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.SettingName, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-value"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.SettingValue, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-user"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastModifiedByUserId, "\xA0 "))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-date"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "label-wrapper"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, term.LastModifiedOnDate, "\xA0 "))))
        );
      });
    }
  }, {
    key: "onKeywordChanged",
    value: function onKeywordChanged(keyword) {
      var props = this.props;

      if (keyword === "" && props.scannerCheckKeyword !== "") {
        keyword = props.scannerCheckKeyword;
      }

      if (keyword === props.scannerCheckKeyword) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updatefileSystemAndDatabaseActiveTab("search"));
        return;
      }

      if (keyword && keyword !== "") {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].searchFileSystemAndDatabase({
          term: keyword
        }, function () {}));
      } else {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].clearFileSystemAndDatabaseSearch());
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updatefileSystemAndDatabaseSearchKeyword(keyword));
    }
  }, {
    key: "getModifiedSettings",
    value: function getModifiedSettings() {
      var props = this.props;

      if (props.scannerCheckActiveTab === "settings") {
        return;
      }

      if (props.modifiedSettings) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updatefileSystemAndDatabaseActiveTab("settings"));
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getLastModifiedSettings(function () {}));
    }
  }, {
    key: "getModifiedFiles",
    value: function getModifiedFiles() {
      var props = this.props;

      if (props.scannerCheckActiveTab === "files") {
        return;
      }

      if (props.modifiedFiles) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].updatefileSystemAndDatabaseActiveTab("files"));
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* security */ "b"].getLastModifiedFiles(function () {}));
    }
  }, {
    key: "showSearchResults",
    value: function showSearchResults() {
      var props = this.props;

      if (props.searchResults && props.scannerCheckActiveTab === "search") {
        return true;
      }

      return false;
    }
  }, {
    key: "showModifiedSettings",
    value: function showModifiedSettings() {
      var props = this.props;

      if (props.modifiedSettings && props.scannerCheckActiveTab === "settings") {
        return true;
      }

      return false;
    }
  }, {
    key: "showModifiedFiles",
    value: function showModifiedFiles() {
      var props = this.props;

      if (props.modifiedFiles && props.scannerCheckActiveTab === "files") {
        return true;
      }

      return false;
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: _style_less__WEBPACK_IMPORTED_MODULE_5___default.a.scannerCheckResults
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannercheck-topbar"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: props.scannerCheckActiveTab === "search" ? "search-filter-active" : "search-filter"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SearchBox"], {
          placeholder: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ScannerChecks"),
          initialValue: props.scannerCheckKeyword && props.scannerCheckKeyword.length > 0 ? props.scannerCheckKeyword : null,
          onSearch: this.onKeywordChanged.bind(this),
          maxLength: 50
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "clear"
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: this.showModifiedSettings() ? "settings-filter-active" : "settings-filter",
          onClick: this.getModifiedSettings.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["TextOverflowWrapper"], {
          text: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ModifiedSettings"),
          maxWidth: 150
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: this.showModifiedFiles() ? "files-filter-active" : "files-filter",
          onClick: this.getModifiedFiles.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["TextOverflowWrapper"], {
          text: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ModifiedFiles"),
          maxWidth: 150
        }))), this.showSearchResults() && props.searchResults.FoundInFiles &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-title"
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SearchFileSystemResult").replace("{0}", props.searchResults.FoundInFiles.length)), props.searchResults.FoundInFiles.length > 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems"
        }, this.renderFileHeader(), this.renderedFileList())), this.showSearchResults() && props.searchResults.FoundInDatabase &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-title"
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SearchDatabaseResult").replace("{0}", props.searchResults.FoundInDatabase.length)), props.searchResults.FoundInDatabase.length > 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems"
        }, this.renderDatabaseHeader(), this.renderedDatabaseList())), this.showModifiedFiles() &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-title"
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("HighRiskFiles")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-riskFiles"
        }, this.renderRiskFilesHeader(), this.renderRiskFilesList("high"))), this.showModifiedFiles() &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-title"
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LowRiskFiles")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-riskFiles"
        }, this.renderRiskFilesHeader(), this.renderRiskFilesList("low"))), this.showModifiedSettings() &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-title"
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("PortalSettings")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-portalSettings"
        }, this.renderPortalSettingsHeader(), this.renderedPortalSettingsList())), this.showModifiedSettings() &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-title"
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("HostSettings")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-hostSettings"
        }, this.renderHostSettingsHeader(), this.renderedHostSettingsList())), this.showModifiedSettings() &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-title"
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("TabSettings")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-tabSettings"
        }, this.renderTabSettingsHeader(), this.renderedTabSettingsList())), this.showModifiedSettings() &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-title"
        }, _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ModuleSettings")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "scannerCheckItems-moduleSettings"
        }, this.renderModuleSettingsHeader(), this.renderedModuleSettingsList())))
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

  return ScannerCheckPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

ScannerCheckPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  searchResults: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  modifiedSettings: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  modifiedFiles: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  scannerCheckKeyword: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  scannerCheckActiveTab: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};

function mapStateToProps(state) {
  return {
    tabIndex: state.pagination.tabIndex,
    searchResults: state.security.searchResults,
    modifiedSettings: state.security.modifiedSettings,
    modifiedFiles: state.security.modifiedFiles,
    scannerCheckKeyword: state.security.scannerCheckKeyword,
    scannerCheckActiveTab: state.security.scannerCheckActiveTab
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(ScannerCheckPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(ScannerCheckPanelBody, "ScannerCheckPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\scannerCheck\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\scannerCheck\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\components\\scannerCheck\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 60 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(34);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _globals_application__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(23);
/* harmony import */ var _store_configureStore__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(35);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(45);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_containers_Root__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();







var store = Object(_store_configureStore__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"])();
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].dispatch = store.dispatch;
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].init();
var appContainer = document.getElementById("security-container");
Object(react_dom__WEBPACK_IMPORTED_MODULE_1__["render"])(
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_redux__WEBPACK_IMPORTED_MODULE_2__["Provider"], {
  store: store
},
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_containers_Root__WEBPACK_IMPORTED_MODULE_5___default.a, null)), appContainer);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(store, "store", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\main.jsx");
  reactHotLoader.register(appContainer, "appContainer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\main.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 61 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
function _interopDefault(e){return e&&"object"==typeof e&&"default"in e?e.default:e}Object.defineProperty(exports,"__esModule",{value:!0});var React=_interopDefault(__webpack_require__(0));function AppContainer(e){return React.Children.only(e.children)}var hot_prod=function(){return function(e){return e}},areComponentsEqual=function(e,n){return e===n},setConfig=function(){},cold=function(e){return e},configureComponent=function(){};exports.AppContainer=AppContainer,exports.hot=hot_prod,exports.areComponentsEqual=areComponentsEqual,exports.setConfig=setConfig,exports.cold=cold,exports.configureComponent=configureComponent;


/***/ }),
/* 62 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(63);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 63 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, ".securitySettings-app .dnn-persona-bar-page-body .persona-bar-page-body {\n  min-height: 600px;\n}\n", ""]);



/***/ }),
/* 64 */
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
/* 65 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _components_App__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(46);
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




var Root =
/*#__PURE__*/
function (_Component) {
  _inherits(Root, _Component);

  function Root() {
    _classCallCheck(this, Root);

    return _possibleConstructorReturn(this, _getPrototypeOf(Root).call(this));
  }

  _createClass(Root, [{
    key: "render",
    value: function render() {
      var culture = window.parent["personaBarSettings"]["culture"];
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "taskScheduler-Root"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_components_App__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"], {
          cultureCode: culture
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

  return Root;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

var _default = Root;
/* harmony default export */ __webpack_exports__["default"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Root, "Root", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Security.Web\\src\\containers\\Root.prod.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 66 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(67);

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
/* 67 */
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
/* 68 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.kIkCR5w5DhUjzOfvYBmGC {\n  margin: 30px 30px;\n}\n.kIkCR5w5DhUjzOfvYBmGC * {\n  box-sizing: border-box;\n}\n.kIkCR5w5DhUjzOfvYBmGC .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n.kIkCR5w5DhUjzOfvYBmGC .dnn-ui-common-input-group .dnn-dropdown {\n  width: 100%;\n  margin-bottom: 32px;\n}\n.kIkCR5w5DhUjzOfvYBmGC .dnn-ui-common-input-group .dnn-switch-container {\n  float: right;\n}\n.kIkCR5w5DhUjzOfvYBmGC .dnn-ui-common-input-group .dnn-page-picker {\n  margin-bottom: 32px;\n}\n.kIkCR5w5DhUjzOfvYBmGC .dnn-ui-common-input-group .loginSettings-row_switch {\n  border-bottom: 1px solid #C8C8C8;\n  margin: 20px 0 0 0;\n  display: table;\n  width: 100%;\n  padding: 0 0 20px 0;\n}\n.kIkCR5w5DhUjzOfvYBmGC .buttons-box {\n  width: 100%;\n  text-align: center;\n  margin: 20px 0;\n}\n.kIkCR5w5DhUjzOfvYBmGC .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n.kIkCR5w5DhUjzOfvYBmGC .globalIcon {\n  width: 18px;\n  height: 18px;\n  float: left;\n  margin: 5px 0 0 -10px;\n}\n.kIkCR5w5DhUjzOfvYBmGC .globalIcon > svg {\n  fill: #4B4E4F;\n}\n", ""]);

// Exports
exports.locals = {
	"loginSettings": "kIkCR5w5DhUjzOfvYBmGC"
};

/***/ }),
/* 69 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3XoNXQBqTTctzszPNP2Ntp {\n  margin: 30px 30px;\n}\n._3XoNXQBqTTctzszPNP2Ntp * {\n  box-sizing: border-box;\n}\n._3XoNXQBqTTctzszPNP2Ntp .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n._3XoNXQBqTTctzszPNP2Ntp .dnn-ui-common-input-group .dnn-switch-container {\n  float: right;\n}\n._3XoNXQBqTTctzszPNP2Ntp .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 100%;\n}\n._3XoNXQBqTTctzszPNP2Ntp .dnn-ui-common-input-group .sslSettings-row_switch {\n  border-bottom: 1px solid #C8C8C8;\n  margin: 0 0 20px 0;\n  display: table;\n  width: 100%;\n  padding: 0 0 10px 0;\n}\n._3XoNXQBqTTctzszPNP2Ntp .buttons-box {\n  width: 100%;\n  text-align: center;\n  margin: 0 0 10px 0;\n}\n._3XoNXQBqTTctzszPNP2Ntp .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n", ""]);

// Exports
exports.locals = {
	"sslSettings": "_3XoNXQBqTTctzszPNP2Ntp"
};

/***/ }),
/* 70 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3NPQ9fcK0FTTh2u8C7klQH {\n  margin: 30px 30px;\n}\n._3NPQ9fcK0FTTh2u8C7klQH * {\n  box-sizing: border-box;\n}\n._3NPQ9fcK0FTTh2u8C7klQH .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n._3NPQ9fcK0FTTh2u8C7klQH .dnn-ui-common-input-group .dnn-switch-container {\n  float: right;\n}\n._3NPQ9fcK0FTTh2u8C7klQH .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 100%;\n}\n._3NPQ9fcK0FTTh2u8C7klQH .dnn-ui-common-input-group .dnn-multi-line-input-with-error {\n  width: 100%;\n}\n._3NPQ9fcK0FTTh2u8C7klQH .dnn-ui-common-input-group .dnn-multi-line-input-with-error .dnn-ui-common-multi-line-input {\n  margin-bottom: 0 !important;\n}\n._3NPQ9fcK0FTTh2u8C7klQH .dnn-ui-common-input-group .otherSettings-row_switch {\n  margin: 0 0 20px 0;\n  border-bottom: 1px solid #C8C8C8;\n  display: table;\n  width: 100%;\n  padding: 0 0 10px 0;\n}\n._3NPQ9fcK0FTTh2u8C7klQH .buttons-box {\n  width: 100%;\n  text-align: center;\n  margin: 10px 0;\n}\n._3NPQ9fcK0FTTh2u8C7klQH .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n", ""]);

// Exports
exports.locals = {
	"otherSettings": "_3NPQ9fcK0FTTh2u8C7klQH"
};

/***/ }),
/* 71 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(72);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 72 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.collapsible-component-ipfilters {\n  display: block;\n  float: left;\n  width: 100%;\n  cursor: auto;\n}\n.collapsible-component-ipfilters:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n.collapsible-component-ipfilters .ip-filter-wrapper > div {\n  float: left;\n  width: 100%;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters {\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 0 10px 0;\n  box-sizing: border-box;\n  cursor: auto;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-ruleType {\n  width: 20%;\n  float: left;\n  padding-left: 15px;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-ruleType .item-row-ruleType-display {\n  color: #959695;\n  margin: 0 0 0 -15px;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-ruleType .item-row-ruleType-display .allow-icon,\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-ruleType .item-row-ruleType-display .deny-icon {\n  padding-left: 15px;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-ruleType .item-row-ruleType-display .allow-icon > svg,\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-ruleType .item-row-ruleType-display .deny-icon > svg {\n  width: 20px;\n  float: left;\n  height: 20px;\n  fill: #959695;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-ipAddress {\n  width: 65%;\n  float: left;\n  padding-top: 3px;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton {\n  width: 13%;\n  float: left;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton .edit-icon {\n  cursor: pointer;\n  float: right;\n  margin-right: 5px;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton .edit-icon > svg {\n  width: 20px;\n  float: right;\n  height: 20px;\n  fill: #C8C8C8;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton .edit-icon > svg:hover {\n  fill: #4B4E4F;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton .edit-icon-active {\n  float: right;\n  cursor: pointer;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton .edit-icon-active > svg {\n  width: 20px;\n  float: right;\n  height: 20px;\n  fill: #1E88C3;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton .delete-icon-hidden {\n  display: none;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton .delete-icon {\n  cursor: pointer;\n  float: right;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton .delete-icon > svg {\n  width: 20px;\n  float: right;\n  height: 20px;\n  fill: #C8C8C8;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-editButton .delete-icon > svg:hover {\n  fill: #4B4E4F;\n}\n.collapsible-component-ipfilters div.collapsible-ipfilters .row .item-row-wrapper {\n  padding: 0 5px 0 5px;\n}\n", ""]);



/***/ }),
/* 73 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<path d=\"M256.9,260.9V1793h1532.1V260.9H256.9z M1612.3,440.2v75.6l-2-2l-770.4,770.4l-295-295l-116.7,116.7V440.2H1612.3z\r\n\t M428.2,1624.3V1174l411.8,411.8l150.8-150.8l0,0l621.6-621.6v811H428.2z\"/>\r\n</svg>\r\n");

/***/ }),
/* 74 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<path d=\"M1024,258.7c-423.2,0-766.4,343.1-766.4,766.4s343.1,766.4,766.4,766.4s766.4-343.1,766.4-766.4S1447.2,258.7,1024,258.7z\r\n\t M446.1,1025.1c0-320.6,259.9-580.5,580.5-580.5c129.8,0,249.2,43.1,345.9,115.1L561.2,1371C489.2,1274.3,446.1,1154.9,446.1,1025.1\r\n\tz M1026.5,1605.6c-122.4,0-235.9-38.1-329.5-102.9l807.2-807.2c64.7,93.6,102.8,207.1,102.8,329.5\r\n\tC1607,1345.7,1347.1,1605.6,1026.5,1605.6z\"/>\r\n</svg>\r\n");

/***/ }),
/* 75 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<g>\r\n\t<polygon points=\"306.5,1438.4 259,1789 609.6,1741.5 610.8,1740.3 307.7,1437.2 \t\"/>\r\n\t<rect x=\"294.4\" y=\"752.6\" transform=\"matrix(0.7071 -0.7071 0.7071 0.7071 -367.0447 1047.6558)\" width=\"1573.5\" height=\"428.7\"/>\r\n</g>\r\n</svg>\r\n");

/***/ }),
/* 76 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<g>\r\n\t<polygon points=\"503.6,1786 1535.6,1786 1672.6,626.4 390.5,626.4 \t\"/>\r\n\t<polygon points=\"1287.6,386.2 1287.6,262 767.5,262 767.5,386.2 317.4,386.2 317.4,514 1730.6,514 1730.6,386.2 \t\"/>\r\n</g>\r\n</svg>\r\n");

/***/ }),
/* 77 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(78);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 78 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.ip-filter-setting-editor .container {\n  padding: 10px 30px 10px 30px;\n}\n.ip-filter-setting-editor .container .dnn-ui-common-input-group {\n  width: 100%;\n  padding: 0 0 10px 0;\n  margin: 0 0 10px 0;\n}\n.ip-filter-setting-editor .container .dnn-ui-common-input-group:first-child {\n  border-bottom: solid 1px #C8C8C8;\n  margin: 0 0 20px 0;\n}\n.ip-filter-setting-editor .container .dnn-ui-common-input-group .dnn-dropdown {\n  width: 100%;\n}\n.ip-filter-setting-editor .container .dnn-ui-common-input-group .dnn-label {\n  margin: 5px 0 0 0;\n}\n.ip-filter-setting-editor .container .dnn-ui-common-input-group .dnn-label label {\n  float: left;\n  font-weight: bolder;\n}\n.ip-filter-setting-editor .container .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 100%;\n}\n.ip-filter-setting-editor .container .dnn-ui-common-input-group .dnn-radio-buttons {\n  float: left;\n  width: 300px;\n  margin: 9px 0 0 30px;\n}\n.ip-filter-setting-editor .container .dnn-ui-common-input-group .dnn-radio-buttons label {\n  float: left;\n}\n.ip-filter-setting-editor .container .dnn-ui-common-input-group .dnn-radio-buttons li {\n  margin: 0 20px 0 0;\n}\n.ip-filter-setting-editor .container .mask {\n  display: block;\n}\n.ip-filter-setting-editor .container .mask-hidden {\n  display: none;\n}\n.ip-filter-setting-editor .buttons-box {\n  width: 100%;\n  text-align: center;\n  float: left;\n  margin: 0 0 20px 0;\n}\n.ip-filter-setting-editor .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n.ip-filter-setting-editor .buttons-box .edit-icon {\n  margin: 0px 10px 20px 10px;\n  float: right;\n}\n.ip-filter-setting-editor .buttons-box .edit-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n}\n.ip-filter-setting-editor .buttons-box .delete-icon {\n  margin: 0px 10px 20px 10px;\n  float: right;\n}\n.ip-filter-setting-editor .buttons-box .delete-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n}\n", ""]);



/***/ }),
/* 79 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._12gGN-MUMm3hAMmgrd8E80 {\n  margin: 20px 30px;\n  display: block;\n}\n._12gGN-MUMm3hAMmgrd8E80 * {\n  box-sizing: border-box;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-items-grid {\n  border: solid 1px #C8C8C8;\n  margin: 20px 0 55px 0;\n  float: left;\n  width: 100%;\n}\n._12gGN-MUMm3hAMmgrd8E80 .header-row {\n  padding: 10px 0 10px 0;\n  width: 100%;\n  float: left;\n  overflow: hidden;\n}\n._12gGN-MUMm3hAMmgrd8E80 .header-RuleType {\n  width: 20%;\n  float: left;\n  font-weight: bolder;\n  padding-left: 15px;\n}\n._12gGN-MUMm3hAMmgrd8E80 .header-IPAddress {\n  width: 65%;\n  float: left;\n  font-weight: bolder;\n}\n._12gGN-MUMm3hAMmgrd8E80 .collapsible-component .collapsible-header {\n  text-align: right;\n  text-transform: none;\n  padding-right: 40px;\n}\n._12gGN-MUMm3hAMmgrd8E80 .collapsible-component .collapsible-header .collapse-icon {\n  display: none;\n}\n._12gGN-MUMm3hAMmgrd8E80 .collapsible-component .collapsible-header .collapse-icon.collapsed {\n  display: none;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar {\n  border-bottom: 1px solid #C8C8C8;\n  float: left;\n  width: 100%;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .warning-container {\n  width: 75%;\n  float: left;\n  margin: 10px 0 15px 0;\n  font-weight: bolder;\n  color: #EA2134;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .warning-container .collapsible-content {\n  margin-top: 5px;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .warning-container .collapsible-content > div {\n  border: solid 1px;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .warning-container .warning-icon > svg {\n  width: 17px;\n  float: left;\n  height: 17px;\n  margin: 0 10px 0 0;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .AddItemRow {\n  text-align: right;\n  width: 25%;\n  float: right;\n  margin: 10px 0 15px 0;\n  font-weight: bolder;\n  overflow: hidden;\n  cursor: pointer;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .AddItemRow .AddItemBox {\n  width: auto;\n  float: right;\n  color: #4B4E4F;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .AddItemRow .AddItemBox .add-icon {\n  margin-left: 20px;\n  margin-right: 5px;\n  float: left;\n  cursor: pointer;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .AddItemRow .AddItemBox .add-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n  fill: #4B4E4F;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .AddItemRow .AddItemBox-active {\n  width: auto;\n  float: right;\n  color: #1E88C3;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .AddItemRow .AddItemBox-active .add-icon {\n  margin-left: 20px;\n  margin-right: 5px;\n  float: left;\n  cursor: pointer;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-topbar .AddItemRow .AddItemBox-active .add-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n  fill: #1E88C3;\n}\n._12gGN-MUMm3hAMmgrd8E80 .ip-filter-item {\n  color: #4B4E4F;\n}\n", ""]);

// Exports
exports.locals = {
	"ipFilterItems": "_12gGN-MUMm3hAMmgrd8E80"
};

/***/ }),
/* 80 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1c24_V4B0QpqBXurcXuM8s {\n  margin: 30px 30px;\n}\n._1c24_V4B0QpqBXurcXuM8s * {\n  box-sizing: border-box;\n}\n._1c24_V4B0QpqBXurcXuM8s .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n._1c24_V4B0QpqBXurcXuM8s .dnn-ui-common-input-group .dnn-switch-container {\n  float: right;\n}\n._1c24_V4B0QpqBXurcXuM8s .dnn-ui-common-input-group .memberSettings-row_switch {\n  border-bottom: 1px solid #C8C8C8;\n  margin: 20px 0 0 0;\n  display: table;\n  width: 100%;\n  padding: 0 0 20px 0;\n}\n._1c24_V4B0QpqBXurcXuM8s .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 100%;\n}\n._1c24_V4B0QpqBXurcXuM8s .buttons-box {\n  width: 100%;\n  text-align: center;\n}\n._1c24_V4B0QpqBXurcXuM8s .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n._1c24_V4B0QpqBXurcXuM8s .globalIcon {\n  width: 18px;\n  height: 18px;\n  float: left;\n  margin: 5px 0 0 -10px;\n}\n._1c24_V4B0QpqBXurcXuM8s .globalIcon > svg {\n  fill: #4B4E4F;\n}\n", ""]);

// Exports
exports.locals = {
	"memberSettings": "_1c24_V4B0QpqBXurcXuM8s"
};

/***/ }),
/* 81 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._2p6jX-baSb_GVJbY7Culx4 {\n  margin: 30px 30px;\n}\n._2p6jX-baSb_GVJbY7Culx4 * {\n  box-sizing: border-box;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .registrationSettings-row-options {\n  float: left;\n  width: 100%;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .registrationSettings-row-options .dnn-label {\n  width: 30%;\n  margin-right: 0;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .registrationSettings-row-options .dnn-radio-buttons {\n  float: left;\n  width: 70%;\n  margin: 5px 0 0 0;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .registrationSettings-row-input {\n  float: left;\n  width: 100%;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .registrationSettings-row-static {\n  float: left;\n  width: 100%;\n  margin: 20px 0 0 0;\n  padding: 0 0 20px 0;\n  border-bottom: solid 1px #C8C8C8;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .registrationSettings-row-static .registrationSettings-row-static-text {\n  color: #4B4E4F;\n  margin: 6px 0 0 0;\n  float: right;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .tags-field {\n  width: 100%;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .tags-field .tag-search-results {\n  z-index: 99;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .registrationSettings-row_switch {\n  border-bottom: 1px solid #C8C8C8;\n  margin: 20px 0 0 0;\n  display: table;\n  width: 100%;\n  padding: 0 0 20px 0;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .registrationSettings-row_switch .dnn-switch-container {\n  float: right;\n}\n._2p6jX-baSb_GVJbY7Culx4 .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 100%;\n}\n._2p6jX-baSb_GVJbY7Culx4 .buttons-box {\n  width: 100%;\n  text-align: center;\n  margin: 30px 0 10px 0;\n}\n._2p6jX-baSb_GVJbY7Culx4 .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n._2p6jX-baSb_GVJbY7Culx4 .globalIcon {\n  width: 18px;\n  height: 18px;\n  float: left;\n  margin: 5px 0 0 -10px;\n}\n._2p6jX-baSb_GVJbY7Culx4 .globalIcon > svg {\n  fill: #4B4E4F;\n}\n", ""]);

// Exports
exports.locals = {
	"registrationSettings": "_2p6jX-baSb_GVJbY7Culx4"
};

/***/ }),
/* 82 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(83);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 83 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "", ""]);



/***/ }),
/* 84 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3ViSU018VS9mVohAQXPO17 {\n  display: inline-block;\n  margin: 30px 20px;\n  border: solid 1px #C8C8C8;\n  width: 94%;\n}\n._3ViSU018VS9mVohAQXPO17 .warning-container {\n  float: left;\n  margin: 25px;\n  color: #EA2134;\n  width: 90%;\n}\n._3ViSU018VS9mVohAQXPO17 .warning-container .warning-icon {\n  float: left;\n  width: 5%;\n}\n._3ViSU018VS9mVohAQXPO17 .warning-container .warning-icon > svg {\n  width: 17px;\n  float: left;\n  height: 17px;\n  margin: 0 10px 0 0;\n}\n._3ViSU018VS9mVohAQXPO17 .warning-container .warning-msg {\n  float: left;\n  width: 95%;\n  font-weight: bold;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinsHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n  font-weight: bolder;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinsHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinsHeader-wrapper .bulletinsHeader-Bulletins {\n  margin: 0 20px 0 25px;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinItem {\n  display: table;\n  width: 100%;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinItem:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinItem .bulletin-detail-wrapper {\n  float: left;\n  width: 700px;\n  margin: 0 25px 0px 25px;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinItem .collapsible-bulletinitemdetail-header {\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 20px;\n  box-sizing: border-box;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinItem .collapsible-bulletinitemdetail-header .term-header {\n  cursor: pointer;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinItem .collapsible-bulletinitemdetail-header .term-header .term-label-title {\n  width: 80%;\n  float: left;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinItem .collapsible-bulletinitemdetail-header .term-header .term-label-date {\n  width: 20%;\n  float: left;\n  text-align: right;\n}\n._3ViSU018VS9mVohAQXPO17 .bulletinItem .collapsible-bulletinitemdetail-header .term-header .term-label-wrapper {\n  padding: 0 5px 0 5px;\n  overflow-wrap: break-word;\n}\n", ""]);

// Exports
exports.locals = {
	"bulletins": "_3ViSU018VS9mVohAQXPO17"
};

/***/ }),
/* 85 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._3oV8nWdPoTSQlDGmGxXjX7 .activities-topbar {\n  border-bottom: solid 1px #C8C8C8;\n  margin: 20px 25px 0 25px;\n  padding: 0 0 15px 0;\n  font-weight: bolder;\n  color: #4B4E4F;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems {\n  display: inline-block;\n  margin: 15px 25px 30px 25px;\n  border: solid 1px #C8C8C8;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n  font-weight: bolder;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityHeader-wrapper .activityHeader-Username {\n  padding: 0 0 0 25px;\n  width: 34%;\n  float: left;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityHeader-wrapper .activityHeader-CreatedDate {\n  width: 20%;\n  float: left;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityHeader-wrapper .activityHeader-LastLogin {\n  width: 20%;\n  float: left;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityHeader-wrapper .activityHeader-LastActivityDate {\n  width: 20%;\n  float: left;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityItem {\n  display: table;\n  width: 100%;\n  padding: 15px 0;\n  color: #4B4E4F;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityItem:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityItem .label-username {\n  padding: 0 0 0 20px;\n  width: 34%;\n  float: left;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityItem .label-createddate {\n  width: 20%;\n  float: left;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityItem .label-lastlogindate {\n  width: 20%;\n  float: left;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityItem .label-lastactivitydate {\n  width: 20%;\n  float: left;\n}\n._3oV8nWdPoTSQlDGmGxXjX7 .activityItems .activityItem .label-wrapper {\n  padding: 0 5px 0 5px;\n  overflow-wrap: break-word;\n}\n", ""]);

// Exports
exports.locals = {
	"activities": "_3oV8nWdPoTSQlDGmGxXjX7"
};

/***/ }),
/* 86 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1YD5s8gju6zxFXOP0Dy_PH .auditcheck-topbar {\n  border-bottom: solid 1px #C8C8C8;\n  margin: 20px 25px 0 25px;\n  padding: 0 0 15px 0;\n  font-weight: bolder;\n  color: #4B4E4F;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems {\n  display: inline-block;\n  margin: 15px 25px 50px 25px;\n  border: solid 1px #C8C8C8;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n  font-weight: bolder;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckHeader-wrapper .auditCheckHeader-SecurityCheck {\n  padding: 0 0 0 25px;\n  width: 34%;\n  float: left;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckHeader-wrapper .auditCheckHeader-Result {\n  width: 31%;\n  float: left;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckHeader-wrapper .auditCheckHeader-Notes {\n  width: 31%;\n  float: left;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem {\n  display: table;\n  width: 100%;\n  padding: 15px 0;\n  color: #4B4E4F;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem .label-name {\n  padding: 0 0 0 20px;\n  width: 34%;\n  float: left;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem .label-result {\n  width: 31%;\n  float: left;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem .label-notes {\n  width: 31%;\n  float: left;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem .label-wrapper {\n  word-break: break-word;\n  padding: 0 5px 0 5px;\n  overflow-wrap: break-word;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem .label-wrapper .label-result-severity-fail {\n  color: #EA2134;\n  font-weight: bolder;\n  margin: 0 0 10px 0;\n  text-transform: uppercase;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem .label-wrapper .label-result-severity-alert {\n  color: #1E88C3;\n  font-weight: bolder;\n  margin: 0 0 10px 0;\n  text-transform: uppercase;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem .label-wrapper .label-result-severity-pass {\n  color: #368254;\n  font-weight: bolder;\n  margin: 0 0 10px 0;\n  text-transform: uppercase;\n}\n._1YD5s8gju6zxFXOP0Dy_PH .auditCheckItems .auditCheckItem .label-wrapper .label-result-severity-unverified {\n  color: #1E88C3;\n  font-weight: bolder;\n  margin: 0 0 10px 0;\n  text-transform: uppercase;\n  text-decoration: underline;\n  cursor: pointer;\n}\n", ""]);

// Exports
exports.locals = {
	"auditCheckResults": "_1YD5s8gju6zxFXOP0Dy_PH"
};

/***/ }),
/* 87 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1cNC-Cro3OloCzqAHm2ANo {\n  min-height: 171px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-title {\n  margin: 0 0 0 25px;\n  color: #4B4E4F;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar {\n  border-bottom: solid 1px #C8C8C8;\n  height: 34px;\n  margin: 20px 25px 20px 25px;\n  padding: 0 0 10px 0;\n  font-weight: bolder;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter {\n  float: left;\n  width: 400px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter:hover svg {\n  fill: #1E88C3;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter > div {\n  display: block !important;\n  border-right: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter > div input {\n  display: block;\n  width: 100%;\n  position: inherit !important;\n  border: none;\n  border-radius: none;\n  background-color: transparent;\n  outline: none;\n  padding-right: 45px;\n  padding: 9px 16px 9px 0 !important;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter > div input::-webkit-search-cancel-button {\n  position: relative;\n  right: 30px;\n  cursor: pointer;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter-active {\n  float: left;\n  width: 400px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter-active > div {\n  display: block !important;\n  border-right: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter-active > div input {\n  display: block;\n  width: 100%;\n  position: inherit !important;\n  border: none;\n  border-radius: none;\n  background-color: transparent;\n  outline: none;\n  padding-right: 45px;\n  color: #1E88C3 !important;\n  padding: 9px 16px 9px 0 !important;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter-active > div input::-webkit-search-cancel-button {\n  position: relative;\n  right: 30px;\n  cursor: pointer;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .search-filter-active svg {\n  fill: #1E88C3;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .files-filter {\n  float: right;\n  border-right: 1px solid #C8C8C8;\n  margin: 0px 20px 0 0;\n  padding: 9px 20px 4px 0px;\n  height: 21px;\n  color: #4B4E4F;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .files-filter:hover {\n  cursor: pointer;\n  color: #1E88C3;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .files-filter-active {\n  float: right;\n  border-right: 1px solid #C8C8C8;\n  margin: 0px 20px 0 0;\n  padding: 9px 20px 4px 0px;\n  height: 21px;\n  color: #1E88C3;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .settings-filter {\n  float: right;\n  color: #4B4E4F;\n  margin: 9px 0 0 0;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .settings-filter:hover {\n  cursor: pointer;\n  color: #1E88C3;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannercheck-topbar .settings-filter-active {\n  float: right;\n  color: #1E88C3;\n  margin: 9px 0 0 0;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems {\n  display: inline-block;\n  margin: 15px 25px 30px 25px;\n  border: solid 1px #C8C8C8;\n  width: 746px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n  font-weight: bolder;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckHeader-wrapper .scannerCheckHeader-FileName {\n  padding: 0 0 0 25px;\n  width: 73%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckHeader-wrapper .scannerCheckHeader-LastWriteTime {\n  width: 23%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckHeader-wrapper .scannerCheckHeader-DatabaseInstance {\n  padding: 0 0 0 25px;\n  width: 40%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckHeader-wrapper .scannerCheckHeader-DatabaseValue {\n  width: 54%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckItem {\n  display: table;\n  width: 100%;\n  padding: 15px 0;\n  color: #4B4E4F;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckItem:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckItem .label-name {\n  padding: 0 0 0 20px;\n  width: 73%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckItem .label-date {\n  width: 23%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckItem .label-columnname {\n  padding: 0 0 0 20px;\n  width: 40%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckItem .label-columnvalue {\n  width: 54%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems .scannerCheckItem .label-wrapper {\n  padding: 0 5px 0 5px;\n  overflow-wrap: break-word;\n  word-break: break-all;\n  max-width: 500px;\n  width: 100%;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles {\n  display: inline-block;\n  margin: 15px 25px 30px 25px;\n  border: solid 1px #C8C8C8;\n  width: 746px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles .scannerCheckHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n  font-weight: bolder;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles .scannerCheckHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles .scannerCheckHeader-wrapper .scannerCheckHeader-FileName {\n  padding: 0 0 0 25px;\n  width: 75%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles .scannerCheckHeader-wrapper .scannerCheckHeader-LastWriteTime {\n  width: 20%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles .scannerCheckItem {\n  display: table;\n  width: 100%;\n  padding: 15px 0;\n  color: #4B4E4F;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles .scannerCheckItem:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles .scannerCheckItem .label-name {\n  padding: 0 0 0 20px;\n  width: 75%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles .scannerCheckItem .label-date {\n  width: 20%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-riskFiles .scannerCheckItem .label-wrapper {\n  padding: 0 5px 0 5px;\n  overflow-wrap: break-word;\n  word-break: break-all;\n  max-width: 500px;\n  width: 100%;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings {\n  display: inline-block;\n  margin: 15px 25px 30px 25px;\n  border: solid 1px #C8C8C8;\n  width: 746px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n  font-weight: bolder;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckHeader-wrapper .scannerCheckHeader-PortalId {\n  padding: 0 0 0 25px;\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckHeader-wrapper .scannerCheckHeader-SettingName {\n  width: 35%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckHeader-wrapper .scannerCheckHeader-SettingValue {\n  width: 20%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckHeader-wrapper .scannerCheckHeader-UserId {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckHeader-wrapper .scannerCheckHeader-LastWriteTime {\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckItem {\n  display: table;\n  width: 100%;\n  padding: 15px 0;\n  color: #4B4E4F;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckItem:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckItem .label-id {\n  padding: 0 0 0 20px;\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckItem .label-name {\n  width: 35%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckItem .label-value {\n  width: 20%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckItem .label-user {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckItem .label-date {\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-portalSettings .scannerCheckItem .label-wrapper {\n  padding: 0 5px 0 5px;\n  overflow-wrap: break-word;\n  word-break: break-all;\n  width: 100%;\n  max-width: 150px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings {\n  display: inline-block;\n  margin: 15px 25px 30px 25px;\n  border: solid 1px #C8C8C8;\n  width: 746px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n  font-weight: bolder;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckHeader-wrapper .scannerCheckHeader-SettingName {\n  padding: 0 0 0 25px;\n  width: 35%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckHeader-wrapper .scannerCheckHeader-SettingValue {\n  width: 30%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckHeader-wrapper .scannerCheckHeader-UserId {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckHeader-wrapper .scannerCheckHeader-LastWriteTime {\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckItem {\n  display: table;\n  width: 100%;\n  padding: 15px 0;\n  color: #4B4E4F;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckItem:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckItem .label-name {\n  padding: 0 0 0 20px;\n  width: 35%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckItem .label-value {\n  width: 30%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckItem .label-user {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckItem .label-date {\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-hostSettings .scannerCheckItem .label-wrapper {\n  padding: 0 5px 0 5px;\n  overflow-wrap: break-word;\n  word-break: break-all;\n  width: 100%;\n  max-width: 150px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings {\n  display: inline-block;\n  margin: 15px 25px 30px 25px;\n  border: solid 1px #C8C8C8;\n  width: 746px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n  font-weight: bolder;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckHeader-wrapper .scannerCheckHeader-TabId {\n  padding: 0 0 0 25px;\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckHeader-wrapper .scannerCheckHeader-PortalId {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckHeader-wrapper .scannerCheckHeader-SettingName {\n  width: 25%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckHeader-wrapper .scannerCheckHeader-SettingValue {\n  width: 20%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckHeader-wrapper .scannerCheckHeader-UserId {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckHeader-wrapper .scannerCheckHeader-LastWriteTime {\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckItem {\n  display: table;\n  width: 100%;\n  padding: 15px 0;\n  color: #4B4E4F;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckItem:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckItem .label-tab {\n  padding: 0 0 0 20px;\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckItem .label-id {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckItem .label-name {\n  width: 25%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckItem .label-value {\n  width: 20%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckItem .label-user {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckItem .label-date {\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-tabSettings .scannerCheckItem .label-wrapper {\n  padding: 0 5px 0 5px;\n  overflow-wrap: break-word;\n  word-break: break-all;\n  width: 100%;\n  max-width: 150px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings {\n  display: inline-block;\n  margin: 15px 25px 30px 25px;\n  border: solid 1px #C8C8C8;\n  width: 746px;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n  font-weight: bolder;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckHeader-wrapper .scannerCheckHeader-Type {\n  padding: 0 0 0 25px;\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckHeader-wrapper .scannerCheckHeader-ModuleId {\n  width: 11%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckHeader-wrapper .scannerCheckHeader-PortalId {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckHeader-wrapper .scannerCheckHeader-SettingName {\n  width: 15%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckHeader-wrapper .scannerCheckHeader-SettingValue {\n  width: 15%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckHeader-wrapper .scannerCheckHeader-UserId {\n  width: 8%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckHeader-wrapper .scannerCheckHeader-LastWriteTime {\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem {\n  display: table;\n  width: 100%;\n  padding: 15px 0;\n  color: #4B4E4F;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem .label-type {\n  padding: 0 0 0 20px;\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem .label-module {\n  width: 11%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem .label-id {\n  width: 10%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem .label-name {\n  width: 15%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem .label-value {\n  width: 15%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem .label-user {\n  width: 8%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem .label-date {\n  width: 18%;\n  float: left;\n}\n._1cNC-Cro3OloCzqAHm2ANo .scannerCheckItems-moduleSettings .scannerCheckItem .label-wrapper {\n  padding: 0 5px 0 5px;\n  overflow-wrap: break-word;\n  word-break: break-all;\n  width: 100%;\n  max-width: 150px;\n}\n", ""]);

// Exports
exports.locals = {
	"scannerCheckResults": "_1cNC-Cro3OloCzqAHm2ANo"
};

/***/ }),
/* 88 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(89);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(12)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 89 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#security-container .dnn-persona-bar-page {\n  border-bottom: 0;\n}\n#security-container .dnn-persona-bar-page .primary > .react-tabs__tab-list > .react-tabs__tab {\n  padding-left: 30px;\n  padding-right: 30px;\n}\n#security-container .dnn-persona-bar-page .row-opened {\n  margin-top: -1px;\n  margin-bottom: -1px;\n  border-top: 2px solid #1E88C3;\n  border-bottom: 2px solid #1E88C3 !important;\n}\n", ""]);



/***/ })
/******/ ]);
//# sourceMappingURL=security-settings-bundle.js.map