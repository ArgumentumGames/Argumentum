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
/******/ 	return __webpack_require__(__webpack_require__.s = 97);
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
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();


var Localization = {
  get: function get(key) {
    var moduleName = _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].moduleName;
    return _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].utilities.getResx(moduleName, key);
  }
};
var _default = Localization;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Localization, "Localization", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\localization\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\localization\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Localization, "Localization", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\localization\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\localization\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\localization\\index.jsx");
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
  module.exports = __webpack_require__(98);
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
  module.exports = __webpack_require__(103)();
}


/***/ }),
/* 5 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _pagination__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(55);
/* harmony import */ var _visiblePanel__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(56);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _visiblePanel__WEBPACK_IMPORTED_MODULE_1__["a"]; });

/* harmony import */ var _importExport__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(57);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _importExport__WEBPACK_IMPORTED_MODULE_2__["a"]; });






/***/ }),
/* 6 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
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
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(utils, "utils", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\utils\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\utils\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(utils, "utils", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\utils\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\utils\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\utils\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 7 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _pagination__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(52);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _pagination__WEBPACK_IMPORTED_MODULE_0__["a"]; });

/* harmony import */ var _visiblePanel__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(53);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "c", function() { return _visiblePanel__WEBPACK_IMPORTED_MODULE_1__["a"]; });

/* harmony import */ var _importExport__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(54);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _importExport__WEBPACK_IMPORTED_MODULE_2__["a"]; });






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
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
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

var ApplicationService =
/*#__PURE__*/
function () {
  function ApplicationService() {
    _classCallCheck(this, ApplicationService);
  }

  _createClass(ApplicationService, [{
    key: "getServiceFramework",
    value: function getServiceFramework(moduleRoot, controller) {
      var sf = _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].utilities.sf;
      sf.moduleRoot = moduleRoot;
      sf.controller = controller;
      return sf;
    }
  }, {
    key: "getPortals",
    value: function getPortals(callback, errorCallback) {
      var sf = this.getServiceFramework("PersonaBar", "Portals");
      sf.get("getPortals", {}, callback, errorCallback);
    }
  }, {
    key: "getInitialPortalTabs",
    value: function getInitialPortalTabs(parameters, callback, errorCallback) {
      var sf = this.getServiceFramework("PersonaBar", "Tabs");
      sf.get("GetPortalTabs?" + serializeQueryStringParameters(parameters), {}, callback, errorCallback);
    }
  }, {
    key: "getDescendantPortalTabs",
    value: function getDescendantPortalTabs(parameters, parentIdObj, callback, errorCallback) {
      var params = Object.assign({}, parameters, parentIdObj);
      var sf = this.getServiceFramework("PersonaBar", "Tabs");
      var serializedParams = serializeQueryStringParameters(params);
      sf.get("GetTabsDescendants?".concat(serializedParams), {}, callback, errorCallback);
    }
  }, {
    key: "getAllJobs",
    value: function getAllJobs(parameters, callback, errorCallback) {
      var sf = this.getServiceFramework("SiteExportImport", "ExportImport");
      sf.getsilence("AllJobs?" + serializeQueryStringParameters(parameters), {}, callback, errorCallback);
    }
  }, {
    key: "getLastJobTime",
    value: function getLastJobTime(parameters, callback, errorCallback) {
      var sf = this.getServiceFramework("SiteExportImport", "ExportImport");
      sf.get("LastJobTime?" + serializeQueryStringParameters(parameters), {}, callback, errorCallback);
    }
  }, {
    key: "getJobDetails",
    value: function getJobDetails(jobId, callback, errorCallback) {
      var sf = this.getServiceFramework("SiteExportImport", "ExportImport");
      sf.getsilence("JobDetails?jobId=" + jobId, {}, callback, errorCallback);
    }
  }, {
    key: "exportSite",
    value: function exportSite(payload, callback, errorCallback) {
      var sf = this.getServiceFramework("SiteExportImport", "ExportImport");
      sf.post("Export", payload, callback, errorCallback);
    }
  }, {
    key: "importSite",
    value: function importSite(payload, callback, errorCallback) {
      var sf = this.getServiceFramework("SiteExportImport", "ExportImport");
      sf.post("Import", payload, callback, errorCallback);
    }
  }, {
    key: "getImportPackages",
    value: function getImportPackages(parameters, callback, errorCallback) {
      var sf = this.getServiceFramework("SiteExportImport", "ExportImport");
      sf.get("GetImportPackages?" + serializeQueryStringParameters(parameters), {}, callback, errorCallback);
    }
  }, {
    key: "verifyImportPackage",
    value: function verifyImportPackage(packageId, callback, errorCallback) {
      var sf = this.getServiceFramework("SiteExportImport", "ExportImport");
      sf.get("VerifyImportPackage?packageId=" + packageId, {}, callback, errorCallback);
    }
  }, {
    key: "cancelJob",
    value: function cancelJob(jobId, callback, errorCallback) {
      var sf = this.getServiceFramework("SiteExportImport", "ExportImport");
      sf.post("CancelProcess?jobId=" + jobId, {}, callback, errorCallback);
    }
  }, {
    key: "deleteJob",
    value: function deleteJob(jobId, callback, errorCallback) {
      var sf = this.getServiceFramework("SiteExportImport", "ExportImport");
      sf.post("RemoveJob?jobId=" + jobId, {}, callback, errorCallback);
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
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(serializeQueryStringParameters, "serializeQueryStringParameters", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(ApplicationService, "ApplicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(applicationService, "applicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(serializeQueryStringParameters, "serializeQueryStringParameters", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(ApplicationService, "ApplicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(applicationService, "applicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\applicationService.js");
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

var	fixUrls = __webpack_require__(101);

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
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var itemsToExport = [];
var itemsToExportService = {
  registerItemToExport: function registerItemToExport(item) {
    itemsToExport.push(item);
  },
  getRegisteredItemsToExport: function getRegisteredItemsToExport() {
    return itemsToExport;
  }
};
var _default = itemsToExportService;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(itemsToExport, "itemsToExport", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\itemsToExportService.js");
  reactHotLoader.register(itemsToExportService, "itemsToExportService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\itemsToExportService.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\itemsToExportService.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(itemsToExport, "itemsToExport", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\itemsToExportService.js");
  reactHotLoader.register(itemsToExportService, "itemsToExportService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\itemsToExportService.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\itemsToExportService.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\services\\itemsToExportService.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 14 */
/***/ (function(module, exports) {

// https://github.com/zloirock/core-js/issues/86#issuecomment-115759028
var global = module.exports = typeof window != 'undefined' && window.Math == Math
  ? window : typeof self != 'undefined' && self.Math == Math ? self
  // eslint-disable-next-line no-new-func
  : Function('return this')();
if (typeof __g == 'number') __g = global; // eslint-disable-line no-undef


/***/ }),
/* 15 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;

var _typeof2 = __webpack_require__(65);

var _typeof3 = _interopRequireDefault(_typeof2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = function (self, call) {
  if (!self) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return call && ((typeof call === "undefined" ? "undefined" : (0, _typeof3.default)(call)) === "object" || typeof call === "function") ? call : self;
};

/***/ }),
/* 16 */
/***/ (function(module, exports) {

var core = module.exports = { version: '2.6.5' };
if (typeof __e == 'number') __e = core; // eslint-disable-line no-undef


/***/ }),
/* 17 */
/***/ (function(module, exports) {

var hasOwnProperty = {}.hasOwnProperty;
module.exports = function (it, key) {
  return hasOwnProperty.call(it, key);
};


/***/ }),
/* 18 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.Redux;

/***/ }),
/* 19 */
/***/ (function(module, exports, __webpack_require__) {

var dP = __webpack_require__(20);
var createDesc = __webpack_require__(31);
module.exports = __webpack_require__(22) ? function (object, key, value) {
  return dP.f(object, key, createDesc(1, value));
} : function (object, key, value) {
  object[key] = value;
  return object;
};


/***/ }),
/* 20 */
/***/ (function(module, exports, __webpack_require__) {

var anObject = __webpack_require__(29);
var IE8_DOM_DEFINE = __webpack_require__(59);
var toPrimitive = __webpack_require__(38);
var dP = Object.defineProperty;

exports.f = __webpack_require__(22) ? Object.defineProperty : function defineProperty(O, P, Attributes) {
  anObject(O);
  P = toPrimitive(P, true);
  anObject(Attributes);
  if (IE8_DOM_DEFINE) try {
    return dP(O, P, Attributes);
  } catch (e) { /* empty */ }
  if ('get' in Attributes || 'set' in Attributes) throw TypeError('Accessors not supported!');
  if ('value' in Attributes) O[P] = Attributes.value;
  return O;
};


/***/ }),
/* 21 */
/***/ (function(module, exports) {

module.exports = function (it) {
  return typeof it === 'object' ? it !== null : typeof it === 'function';
};


/***/ }),
/* 22 */
/***/ (function(module, exports, __webpack_require__) {

// Thank's IE8 for his funny defineProperty
module.exports = !__webpack_require__(30)(function () {
  return Object.defineProperty({}, 'a', { get: function () { return 7; } }).a != 7;
});


/***/ }),
/* 23 */
/***/ (function(module, exports, __webpack_require__) {

// to indexed object, toObject with fallback for non-array-like ES3 strings
var IObject = __webpack_require__(62);
var defined = __webpack_require__(39);
module.exports = function (it) {
  return IObject(defined(it));
};


/***/ }),
/* 24 */
/***/ (function(module, exports, __webpack_require__) {

var store = __webpack_require__(42)('wks');
var uid = __webpack_require__(34);
var Symbol = __webpack_require__(14).Symbol;
var USE_SYMBOL = typeof Symbol == 'function';

var $exports = module.exports = function (name) {
  return store[name] || (store[name] =
    USE_SYMBOL && Symbol[name] || (USE_SYMBOL ? Symbol : uid)('Symbol.' + name));
};

$exports.store = store;


/***/ }),
/* 25 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;

exports.default = function (instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
};

/***/ }),
/* 26 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;

var _setPrototypeOf = __webpack_require__(146);

var _setPrototypeOf2 = _interopRequireDefault(_setPrototypeOf);

var _create = __webpack_require__(150);

var _create2 = _interopRequireDefault(_create);

var _typeof2 = __webpack_require__(65);

var _typeof3 = _interopRequireDefault(_typeof2);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = function (subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function, not " + (typeof superClass === "undefined" ? "undefined" : (0, _typeof3.default)(superClass)));
  }

  subClass.prototype = (0, _create2.default)(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      enumerable: false,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf2.default ? (0, _setPrototypeOf2.default)(subClass, superClass) : subClass.__proto__ = superClass;
};

/***/ }),
/* 27 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;

var _assign = __webpack_require__(116);

var _assign2 = _interopRequireDefault(_assign);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = _assign2.default || function (target) {
  for (var i = 1; i < arguments.length; i++) {
    var source = arguments[i];

    for (var key in source) {
      if (Object.prototype.hasOwnProperty.call(source, key)) {
        target[key] = source[key];
      }
    }
  }

  return target;
};

/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {

var global = __webpack_require__(14);
var core = __webpack_require__(16);
var ctx = __webpack_require__(58);
var hide = __webpack_require__(19);
var has = __webpack_require__(17);
var PROTOTYPE = 'prototype';

var $export = function (type, name, source) {
  var IS_FORCED = type & $export.F;
  var IS_GLOBAL = type & $export.G;
  var IS_STATIC = type & $export.S;
  var IS_PROTO = type & $export.P;
  var IS_BIND = type & $export.B;
  var IS_WRAP = type & $export.W;
  var exports = IS_GLOBAL ? core : core[name] || (core[name] = {});
  var expProto = exports[PROTOTYPE];
  var target = IS_GLOBAL ? global : IS_STATIC ? global[name] : (global[name] || {})[PROTOTYPE];
  var key, own, out;
  if (IS_GLOBAL) source = name;
  for (key in source) {
    // contains in native
    own = !IS_FORCED && target && target[key] !== undefined;
    if (own && has(exports, key)) continue;
    // export native or passed
    out = own ? target[key] : source[key];
    // prevent global pollution for namespaces
    exports[key] = IS_GLOBAL && typeof target[key] != 'function' ? source[key]
    // bind timers to global for call from export context
    : IS_BIND && own ? ctx(out, global)
    // wrap global constructors for prevent change them in library
    : IS_WRAP && target[key] == out ? (function (C) {
      var F = function (a, b, c) {
        if (this instanceof C) {
          switch (arguments.length) {
            case 0: return new C();
            case 1: return new C(a);
            case 2: return new C(a, b);
          } return new C(a, b, c);
        } return C.apply(this, arguments);
      };
      F[PROTOTYPE] = C[PROTOTYPE];
      return F;
    // make static versions for prototype methods
    })(out) : IS_PROTO && typeof out == 'function' ? ctx(Function.call, out) : out;
    // export proto methods to core.%CONSTRUCTOR%.methods.%NAME%
    if (IS_PROTO) {
      (exports.virtual || (exports.virtual = {}))[key] = out;
      // export proto methods to core.%CONSTRUCTOR%.prototype.%NAME%
      if (type & $export.R && expProto && !expProto[key]) hide(expProto, key, out);
    }
  }
};
// type bitmap
$export.F = 1;   // forced
$export.G = 2;   // global
$export.S = 4;   // static
$export.P = 8;   // proto
$export.B = 16;  // bind
$export.W = 32;  // wrap
$export.U = 64;  // safe
$export.R = 128; // real proto method for `library`
module.exports = $export;


/***/ }),
/* 29 */
/***/ (function(module, exports, __webpack_require__) {

var isObject = __webpack_require__(21);
module.exports = function (it) {
  if (!isObject(it)) throw TypeError(it + ' is not an object!');
  return it;
};


/***/ }),
/* 30 */
/***/ (function(module, exports) {

module.exports = function (exec) {
  try {
    return !!exec();
  } catch (e) {
    return true;
  }
};


/***/ }),
/* 31 */
/***/ (function(module, exports) {

module.exports = function (bitmap, value) {
  return {
    enumerable: !(bitmap & 1),
    configurable: !(bitmap & 2),
    writable: !(bitmap & 4),
    value: value
  };
};


/***/ }),
/* 32 */
/***/ (function(module, exports, __webpack_require__) {

// 19.1.2.14 / 15.2.3.14 Object.keys(O)
var $keys = __webpack_require__(61);
var enumBugKeys = __webpack_require__(43);

module.exports = Object.keys || function keys(O) {
  return $keys(O, enumBugKeys);
};


/***/ }),
/* 33 */
/***/ (function(module, exports) {

module.exports = true;


/***/ }),
/* 34 */
/***/ (function(module, exports) {

var id = 0;
var px = Math.random();
module.exports = function (key) {
  return 'Symbol('.concat(key === undefined ? '' : key, ')_', (++id + px).toString(36));
};


/***/ }),
/* 35 */
/***/ (function(module, exports) {

exports.f = {}.propertyIsEnumerable;


/***/ }),
/* 36 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}






var PackageCard =
/*#__PURE__*/
function (_Component) {
  _inherits(PackageCard, _Component);

  function PackageCard() {
    _classCallCheck(this, PackageCard);

    return _possibleConstructorReturn(this, _getPrototypeOf(PackageCard).apply(this, arguments));
  }

  _createClass(PackageCard, [{
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: props.className || "package-card"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "card-grid"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 35,
          className: "card-column1"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-name"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["TextOverflowWrapper"], {
          text: props.selectedPackage.Name,
          maxWidth: 200
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-field"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["TextOverflowWrapper"], {
          text: props.selectedPackage.ExporTimeString,
          maxWidth: 200
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 17,
          className: "card-column2"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-field"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("FolderName")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-field"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("Website")
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 23,
          className: "card-column3"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-field"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["TextOverflowWrapper"], {
          text: props.selectedPackage.FileName,
          maxWidth: 160
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-field"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["TextOverflowWrapper"], {
          text: props.selectedPackage.PortalName,
          maxWidth: 160
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 12,
          className: "card-column4"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-field"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("Mode")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-field"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("FileSize")
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
          columnSize: 13,
          className: "card-column5"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-field"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["TextOverflowWrapper"], {
          text: props.selectedPackage.Summary.ExportMode ? localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("ExportModeDifferential") : localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("ExportModeComplete"),
          maxWidth: 70
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-field"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["TextOverflowWrapper"], {
          text: ((props.selectedPackage.Summary || {}).ExportFileInfo || {}).ExportSize,
          maxWidth: 70
        })))), props.children)
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

  return PackageCard;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

PackageCard.propTypes = {
  selectedPackage: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node,
  className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};
var _default = PackageCard;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(PackageCard, "PackageCard", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(PackageCard, "PackageCard", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCard.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 37 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;

exports.default = function (obj, keys) {
  var target = {};

  for (var i in obj) {
    if (keys.indexOf(i) >= 0) continue;
    if (!Object.prototype.hasOwnProperty.call(obj, i)) continue;
    target[i] = obj[i];
  }

  return target;
};

/***/ }),
/* 38 */
/***/ (function(module, exports, __webpack_require__) {

// 7.1.1 ToPrimitive(input [, PreferredType])
var isObject = __webpack_require__(21);
// instead of the ES6 spec version, we didn't implement @@toPrimitive case
// and the second argument - flag - preferred type is a string
module.exports = function (it, S) {
  if (!isObject(it)) return it;
  var fn, val;
  if (S && typeof (fn = it.toString) == 'function' && !isObject(val = fn.call(it))) return val;
  if (typeof (fn = it.valueOf) == 'function' && !isObject(val = fn.call(it))) return val;
  if (!S && typeof (fn = it.toString) == 'function' && !isObject(val = fn.call(it))) return val;
  throw TypeError("Can't convert object to primitive value");
};


/***/ }),
/* 39 */
/***/ (function(module, exports) {

// 7.2.1 RequireObjectCoercible(argument)
module.exports = function (it) {
  if (it == undefined) throw TypeError("Can't call method on  " + it);
  return it;
};


/***/ }),
/* 40 */
/***/ (function(module, exports) {

// 7.1.4 ToInteger
var ceil = Math.ceil;
var floor = Math.floor;
module.exports = function (it) {
  return isNaN(it = +it) ? 0 : (it > 0 ? floor : ceil)(it);
};


/***/ }),
/* 41 */
/***/ (function(module, exports, __webpack_require__) {

var shared = __webpack_require__(42)('keys');
var uid = __webpack_require__(34);
module.exports = function (key) {
  return shared[key] || (shared[key] = uid(key));
};


/***/ }),
/* 42 */
/***/ (function(module, exports, __webpack_require__) {

var core = __webpack_require__(16);
var global = __webpack_require__(14);
var SHARED = '__core-js_shared__';
var store = global[SHARED] || (global[SHARED] = {});

(module.exports = function (key, value) {
  return store[key] || (store[key] = value !== undefined ? value : {});
})('versions', []).push({
  version: core.version,
  mode: __webpack_require__(33) ? 'pure' : 'global',
  copyright: '© 2019 Denis Pushkarev (zloirock.ru)'
});


/***/ }),
/* 43 */
/***/ (function(module, exports) {

// IE 8- don't enum bug keys
module.exports = (
  'constructor,hasOwnProperty,isPrototypeOf,propertyIsEnumerable,toLocaleString,toString,valueOf'
).split(',');


/***/ }),
/* 44 */
/***/ (function(module, exports) {

exports.f = Object.getOwnPropertySymbols;


/***/ }),
/* 45 */
/***/ (function(module, exports) {

module.exports = {};


/***/ }),
/* 46 */
/***/ (function(module, exports, __webpack_require__) {

// 19.1.2.2 / 15.2.3.5 Object.create(O [, Properties])
var anObject = __webpack_require__(29);
var dPs = __webpack_require__(129);
var enumBugKeys = __webpack_require__(43);
var IE_PROTO = __webpack_require__(41)('IE_PROTO');
var Empty = function () { /* empty */ };
var PROTOTYPE = 'prototype';

// Create object with fake `null` prototype: use iframe Object with cleared prototype
var createDict = function () {
  // Thrash, waste and sodomy: IE GC bug
  var iframe = __webpack_require__(60)('iframe');
  var i = enumBugKeys.length;
  var lt = '<';
  var gt = '>';
  var iframeDocument;
  iframe.style.display = 'none';
  __webpack_require__(130).appendChild(iframe);
  iframe.src = 'javascript:'; // eslint-disable-line no-script-url
  // createDict = iframe.contentWindow.Object;
  // html.removeChild(iframe);
  iframeDocument = iframe.contentWindow.document;
  iframeDocument.open();
  iframeDocument.write(lt + 'script' + gt + 'document.F=Object' + lt + '/script' + gt);
  iframeDocument.close();
  createDict = iframeDocument.F;
  while (i--) delete createDict[PROTOTYPE][enumBugKeys[i]];
  return createDict();
};

module.exports = Object.create || function create(O, Properties) {
  var result;
  if (O !== null) {
    Empty[PROTOTYPE] = anObject(O);
    result = new Empty();
    Empty[PROTOTYPE] = null;
    // add "__proto__" for Object.getPrototypeOf polyfill
    result[IE_PROTO] = O;
  } else result = createDict();
  return Properties === undefined ? result : dPs(result, Properties);
};


/***/ }),
/* 47 */
/***/ (function(module, exports, __webpack_require__) {

var def = __webpack_require__(20).f;
var has = __webpack_require__(17);
var TAG = __webpack_require__(24)('toStringTag');

module.exports = function (it, tag, stat) {
  if (it && !has(it = stat ? it : it.prototype, TAG)) def(it, TAG, { configurable: true, value: tag });
};


/***/ }),
/* 48 */
/***/ (function(module, exports, __webpack_require__) {

exports.f = __webpack_require__(24);


/***/ }),
/* 49 */
/***/ (function(module, exports, __webpack_require__) {

var global = __webpack_require__(14);
var core = __webpack_require__(16);
var LIBRARY = __webpack_require__(33);
var wksExt = __webpack_require__(48);
var defineProperty = __webpack_require__(20).f;
module.exports = function (name) {
  var $Symbol = core.Symbol || (core.Symbol = LIBRARY ? {} : global.Symbol || {});
  if (name.charAt(0) != '_' && !(name in $Symbol)) defineProperty($Symbol, name, { value: wksExt.f(name) });
};


/***/ }),
/* 50 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();


var siteImportExport = {
  init: function init() {
    // This setting is required and define the public path 
    // to allow the web application to download assets on demand 
    // eslint-disable-next-line no-undef
    // __webpack_public_path__ = options.publicPath;
    var options = window.dnn.initSiteImportExport();
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].init(options.utility);
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].moduleName = options.moduleName;
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].settings = options.settings; // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production

    __webpack_require__(99);
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
var _default = siteImportExport;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(siteImportExport, "siteImportExport", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\globals\\application.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\globals\\application.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(siteImportExport, "siteImportExport", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\globals\\application.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\globals\\application.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\globals\\application.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 51 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 52 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var paginationActionTypes = {
  LOAD_MORE: "LOAD_MORE",
  LOAD_TAB_DATA: "LOAD_TAB_DATA"
};
var _default = paginationActionTypes;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\pagination.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\pagination.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\pagination.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\pagination.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\pagination.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 53 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var paginationActionTypes = {
  SELECT_PANEL: "SELECT_PANEL"
};
var _default = paginationActionTypes;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\visiblePanel.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\visiblePanel.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\visiblePanel.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\visiblePanel.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\visiblePanel.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 54 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var importExportActionTypes = {
  SELECTED_SITE: "SELECTED_SITE",
  GO_TO_WIZARD_STEP: "GO_TO_WIZARD_STEP",
  RETRIEVED_PORTALS: "RETRIEVED_PORTALS",
  RETRIEVED_JOBS: "RETRIEVED_JOBS",
  RETRIEVED_JOB_DETAILS: "RETRIEVED_JOB_DETAILS",
  SUBMITTED_EXPORT_REQUEST: "SUBMITTED_EXPORT_REQUEST",
  RETRIEVED_IMPORT_PACKAGES: "RETRIEVED_IMPORT_PACKAGES",
  GO_TO_IMPORT_WIZARD_STEP: "GO_TO_IMPORT_WIZARD_STEP",
  VERIFIED_IMPORT_PACKAGE: "VERIFIED_IMPORT_PACKAGE",
  SELECTED_PACKAGE: "SELECTED_PACKAGE",
  SUBMITTED_IMPORT_REQUEST: "SUBMITTED_IMPORT_REQUEST",
  CANCELLED_JOB: "CANCELLED_JOB",
  REMOVED_JOB: "REMOVED_JOB",
  VERIFIED_PACKAGE: "VERIFIED_PACKAGE",
  RETRIEVED_LAST_EXPORT_DATE: "RETRIEVED_LAST_EXPORT_DATE",
  SELECTED_JOB: "SELECTED_JOB",
  GET_INITIAL_PORTAL_TABS: "GET_INITIAL_PORTAL_TABS",
  GET_DESCENDANT_PORTAL_TABS: "GET_DESCENDANT_PORTAL_TABS"
};
var _default = importExportActionTypes;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(importExportActionTypes, "importExportActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\importExport.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\importExport.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(importExportActionTypes, "importExportActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\importExport.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\importExport.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\constants\\actionTypes\\importExport.js");
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
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();


var paginationActions = {
  loadTab: function loadTab(index) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* pagination */ "b"].LOAD_TAB_DATA,
        payload: {
          index: index
        }
      });
    };
  }
};
var _default = paginationActions;
var _default2 = _default;
/* unused harmony default export */ var _unused_webpack_default_export = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActions, "paginationActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\pagination.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\pagination.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActions, "paginationActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\pagination.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\pagination.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\pagination.js");
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
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

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
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(visiblePanelActions, "visiblePanelActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\visiblePanel.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\visiblePanel.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(visiblePanelActions, "visiblePanelActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\visiblePanel.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\visiblePanel.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\visiblePanel.js");
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
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
/* harmony import */ var _services_applicationService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(10);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();



var jobsActions = {
  getInitialPortalTabs: function getInitialPortalTabs(PortalTabsParameters, callback) {
    _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getInitialPortalTabs(PortalTabsParameters, function (response) {
      if (callback) {
        callback(response);
      }
    });
  },
  getDescendantPortalTabs: function getDescendantPortalTabs(PortalTabsParameters, ParentTabId, callback) {
    var ParentTabIdObj = {
      parentId: ParentTabId
    };
    _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getDescendantPortalTabs(PortalTabsParameters, ParentTabIdObj, function (response) {
      if (callback) {
        callback(response);
      }
    });
  },
  siteSelected: function siteSelected(portalId, portalName, callback) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SELECTED_SITE,
        portalId: portalId === undefined ? -1 : portalId,
        portalName: portalName,
        currentJobId: null
      });

      if (callback) {
        callback();
      }
    };
  },
  navigateWizard: function navigateWizard(wizardStep, callback) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].GO_TO_WIZARD_STEP,
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
  getPortals: function getPortals(callback, errorCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getPortals(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_PORTALS,
          portals: data.Results
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  jobSelected: function jobSelected(jobId, callback) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SELECTED_JOB,
        currentJobId: jobId === undefined ? null : jobId
      });

      if (callback) {
        callback();
      }
    };
  },
  getAllJobs: function getAllJobs(parameters, callback, errorCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getAllJobs(parameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_JOBS,
          jobs: data.Jobs,
          totalJobs: data.TotalJobs,
          lastExportTime: data.LastExportTimeString,
          lastImportTime: data.LastImportTimeString
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  getLastExportTime: function getLastExportTime(parameters, callback, errorCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getLastJobTime(parameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_LAST_EXPORT_DATE,
          lastExportTime: data.lastTime
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  getJobDetails: function getJobDetails(jobId, callback, errorCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getJobDetails(jobId, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_JOB_DETAILS,
          job: data
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  exportSite: function exportSite(payload, callback, errorCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].exportSite(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SUBMITTED_EXPORT_REQUEST,
          jobId: data.jobId
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
  importSite: function importSite(payload, callback, errorCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].importSite(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SUBMITTED_IMPORT_REQUEST,
          jobId: data.jobId,
          packageVerified: false
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
  importWizardGoToSetp: function importWizardGoToSetp(step, callback) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].GO_TO_IMPORT_WIZARD_STEP,
        importWizardStep: step
      });

      if (callback) {
        callback();
      }
    };
  },
  selectPackage: function selectPackage(pkg, callback) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SELECTED_PACKAGE,
        selectedPackage: pkg,
        importSummary: undefined
      });

      if (callback) {
        callback();
      }
    };
  },
  packageVerified: function packageVerified(verified, callback) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].VERIFIED_PACKAGE,
        packageVerified: verified
      });

      if (callback) {
        callback();
      }
    };
  },
  getImportPackages: function getImportPackages(parameters, callback, errorCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getImportPackages(parameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_IMPORT_PACKAGES,
          importPackages: data.packages,
          total: data.total,
          selectedPackage: null,
          importWizardStep: 0,
          packageVerified: false
        });

        if (callback) {
          callback();
        }
      }, errorCallback);
    };
  },
  verifyImportPackage: function verifyImportPackage(packageId, callback, errorCallback) {
    return function (dispatch) {
      setTimeout(function () {
        _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].verifyImportPackage(packageId, function (data) {
          dispatch({
            type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].VERIFIED_IMPORT_PACKAGE,
            importSummary: data
          });

          if (callback) {
            callback();
          }
        }, errorCallback);
      }, 2000);
    };
  },
  cancelJob: function cancelJob(jobId, callback, errorCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].cancelJob(jobId, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].CANCELLED_JOB
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
  deleteJob: function deleteJob(jobId, callback, errorCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].deleteJob(jobId, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].REMOVED_JOB
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
var _default = jobsActions;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(jobsActions, "jobsActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\importExport.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\importExport.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(jobsActions, "jobsActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\importExport.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\importExport.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\actions\\importExport.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 58 */
/***/ (function(module, exports, __webpack_require__) {

// optional / simple context binding
var aFunction = __webpack_require__(119);
module.exports = function (fn, that, length) {
  aFunction(fn);
  if (that === undefined) return fn;
  switch (length) {
    case 1: return function (a) {
      return fn.call(that, a);
    };
    case 2: return function (a, b) {
      return fn.call(that, a, b);
    };
    case 3: return function (a, b, c) {
      return fn.call(that, a, b, c);
    };
  }
  return function (/* ...args */) {
    return fn.apply(that, arguments);
  };
};


/***/ }),
/* 59 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = !__webpack_require__(22) && !__webpack_require__(30)(function () {
  return Object.defineProperty(__webpack_require__(60)('div'), 'a', { get: function () { return 7; } }).a != 7;
});


/***/ }),
/* 60 */
/***/ (function(module, exports, __webpack_require__) {

var isObject = __webpack_require__(21);
var document = __webpack_require__(14).document;
// typeof document.createElement is 'object' in old IE
var is = isObject(document) && isObject(document.createElement);
module.exports = function (it) {
  return is ? document.createElement(it) : {};
};


/***/ }),
/* 61 */
/***/ (function(module, exports, __webpack_require__) {

var has = __webpack_require__(17);
var toIObject = __webpack_require__(23);
var arrayIndexOf = __webpack_require__(121)(false);
var IE_PROTO = __webpack_require__(41)('IE_PROTO');

module.exports = function (object, names) {
  var O = toIObject(object);
  var i = 0;
  var result = [];
  var key;
  for (key in O) if (key != IE_PROTO) has(O, key) && result.push(key);
  // Don't enum bug & hidden keys
  while (names.length > i) if (has(O, key = names[i++])) {
    ~arrayIndexOf(result, key) || result.push(key);
  }
  return result;
};


/***/ }),
/* 62 */
/***/ (function(module, exports, __webpack_require__) {

// fallback for non-array-like ES3 and non-enumerable old V8 strings
var cof = __webpack_require__(63);
// eslint-disable-next-line no-prototype-builtins
module.exports = Object('z').propertyIsEnumerable(0) ? Object : function (it) {
  return cof(it) == 'String' ? it.split('') : Object(it);
};


/***/ }),
/* 63 */
/***/ (function(module, exports) {

var toString = {}.toString;

module.exports = function (it) {
  return toString.call(it).slice(8, -1);
};


/***/ }),
/* 64 */
/***/ (function(module, exports, __webpack_require__) {

// 7.1.13 ToObject(argument)
var defined = __webpack_require__(39);
module.exports = function (it) {
  return Object(defined(it));
};


/***/ }),
/* 65 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;

var _iterator = __webpack_require__(124);

var _iterator2 = _interopRequireDefault(_iterator);

var _symbol = __webpack_require__(136);

var _symbol2 = _interopRequireDefault(_symbol);

var _typeof = typeof _symbol2.default === "function" && typeof _iterator2.default === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof _symbol2.default === "function" && obj.constructor === _symbol2.default && obj !== _symbol2.default.prototype ? "symbol" : typeof obj; };

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = typeof _symbol2.default === "function" && _typeof(_iterator2.default) === "symbol" ? function (obj) {
  return typeof obj === "undefined" ? "undefined" : _typeof(obj);
} : function (obj) {
  return obj && typeof _symbol2.default === "function" && obj.constructor === _symbol2.default && obj !== _symbol2.default.prototype ? "symbol" : typeof obj === "undefined" ? "undefined" : _typeof(obj);
};

/***/ }),
/* 66 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var LIBRARY = __webpack_require__(33);
var $export = __webpack_require__(28);
var redefine = __webpack_require__(67);
var hide = __webpack_require__(19);
var Iterators = __webpack_require__(45);
var $iterCreate = __webpack_require__(128);
var setToStringTag = __webpack_require__(47);
var getPrototypeOf = __webpack_require__(131);
var ITERATOR = __webpack_require__(24)('iterator');
var BUGGY = !([].keys && 'next' in [].keys()); // Safari has buggy iterators w/o `next`
var FF_ITERATOR = '@@iterator';
var KEYS = 'keys';
var VALUES = 'values';

var returnThis = function () { return this; };

module.exports = function (Base, NAME, Constructor, next, DEFAULT, IS_SET, FORCED) {
  $iterCreate(Constructor, NAME, next);
  var getMethod = function (kind) {
    if (!BUGGY && kind in proto) return proto[kind];
    switch (kind) {
      case KEYS: return function keys() { return new Constructor(this, kind); };
      case VALUES: return function values() { return new Constructor(this, kind); };
    } return function entries() { return new Constructor(this, kind); };
  };
  var TAG = NAME + ' Iterator';
  var DEF_VALUES = DEFAULT == VALUES;
  var VALUES_BUG = false;
  var proto = Base.prototype;
  var $native = proto[ITERATOR] || proto[FF_ITERATOR] || DEFAULT && proto[DEFAULT];
  var $default = $native || getMethod(DEFAULT);
  var $entries = DEFAULT ? !DEF_VALUES ? $default : getMethod('entries') : undefined;
  var $anyNative = NAME == 'Array' ? proto.entries || $native : $native;
  var methods, key, IteratorPrototype;
  // Fix native
  if ($anyNative) {
    IteratorPrototype = getPrototypeOf($anyNative.call(new Base()));
    if (IteratorPrototype !== Object.prototype && IteratorPrototype.next) {
      // Set @@toStringTag to native iterators
      setToStringTag(IteratorPrototype, TAG, true);
      // fix for some old engines
      if (!LIBRARY && typeof IteratorPrototype[ITERATOR] != 'function') hide(IteratorPrototype, ITERATOR, returnThis);
    }
  }
  // fix Array#{values, @@iterator}.name in V8 / FF
  if (DEF_VALUES && $native && $native.name !== VALUES) {
    VALUES_BUG = true;
    $default = function values() { return $native.call(this); };
  }
  // Define iterator
  if ((!LIBRARY || FORCED) && (BUGGY || VALUES_BUG || !proto[ITERATOR])) {
    hide(proto, ITERATOR, $default);
  }
  // Plug for library
  Iterators[NAME] = $default;
  Iterators[TAG] = returnThis;
  if (DEFAULT) {
    methods = {
      values: DEF_VALUES ? $default : getMethod(VALUES),
      keys: IS_SET ? $default : getMethod(KEYS),
      entries: $entries
    };
    if (FORCED) for (key in methods) {
      if (!(key in proto)) redefine(proto, key, methods[key]);
    } else $export($export.P + $export.F * (BUGGY || VALUES_BUG), NAME, methods);
  }
  return methods;
};


/***/ }),
/* 67 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(19);


/***/ }),
/* 68 */
/***/ (function(module, exports, __webpack_require__) {

// 19.1.2.7 / 15.2.3.4 Object.getOwnPropertyNames(O)
var $keys = __webpack_require__(61);
var hiddenKeys = __webpack_require__(43).concat('length', 'prototype');

exports.f = Object.getOwnPropertyNames || function getOwnPropertyNames(O) {
  return $keys(O, hiddenKeys);
};


/***/ }),
/* 69 */
/***/ (function(module, exports, __webpack_require__) {

var pIE = __webpack_require__(35);
var createDesc = __webpack_require__(31);
var toIObject = __webpack_require__(23);
var toPrimitive = __webpack_require__(38);
var has = __webpack_require__(17);
var IE8_DOM_DEFINE = __webpack_require__(59);
var gOPD = Object.getOwnPropertyDescriptor;

exports.f = __webpack_require__(22) ? gOPD : function getOwnPropertyDescriptor(O, P) {
  O = toIObject(O);
  P = toPrimitive(P, true);
  if (IE8_DOM_DEFINE) try {
    return gOPD(O, P);
  } catch (e) { /* empty */ }
  if (has(O, P)) return createDesc(!pIE.f.call(O, P), O[P]);
};


/***/ }),
/* 70 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 71 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return configureStore; });
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(18);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(51);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_thunk__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(72);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _reducers_rootReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(73);
/* harmony import */ var _containers_DevTools__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(77);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

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

  reactHotLoader.register(IS_PRODUCTION, "IS_PRODUCTION", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\store\\configureStore.js");
  reactHotLoader.register(configureStore, "configureStore", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\store\\configureStore.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(IS_PRODUCTION, "IS_PRODUCTION", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\store\\configureStore.js");
  reactHotLoader.register(configureStore, "configureStore", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\store\\configureStore.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 72 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 73 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(18);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _paginationReducer__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(74);
/* harmony import */ var _visiblePanelReducer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(75);
/* harmony import */ var _importExportReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(76);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();





var rootReducer = Object(redux__WEBPACK_IMPORTED_MODULE_0__["combineReducers"])({
  pagination: _paginationReducer__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"],
  visiblePanel: _visiblePanelReducer__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"],
  importExport: _importExportReducer__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"]
});
var _default = rootReducer;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(rootReducer, "rootReducer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\rootReducer.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\rootReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(rootReducer, "rootReducer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\rootReducer.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\rootReducer.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\rootReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 74 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return pagination; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) {
  for (var i = 1; i < arguments.length; i++) {
    var source = arguments[i] != null ? arguments[i] : {};
    var ownKeys = Object.keys(source);

    if (typeof Object.getOwnPropertySymbols === 'function') {
      ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) {
        return Object.getOwnPropertyDescriptor(source, sym).enumerable;
      }));
    }

    ownKeys.forEach(function (key) {
      _defineProperty(target, key, source[key]);
    });
  }

  return target;
}

function _defineProperty(obj, key, value) {
  if (key in obj) {
    Object.defineProperty(obj, key, {
      value: value,
      enumerable: true,
      configurable: true,
      writable: true
    });
  } else {
    obj[key] = value;
  }

  return obj;
}


function pagination() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    tabIndex: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  if (!action) {
    return _objectSpread2({}, state);
  }

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* pagination */ "b"].LOAD_TAB_DATA:
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

  reactHotLoader.register(pagination, "pagination", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\paginationReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_objectSpread2, "_objectSpread2", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\paginationReducer.js");
  reactHotLoader.register(_defineProperty, "_defineProperty", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\paginationReducer.js");
  reactHotLoader.register(pagination, "pagination", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\paginationReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 75 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return visiblePanel; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) {
  for (var i = 1; i < arguments.length; i++) {
    var source = arguments[i] != null ? arguments[i] : {};
    var ownKeys = Object.keys(source);

    if (typeof Object.getOwnPropertySymbols === 'function') {
      ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) {
        return Object.getOwnPropertyDescriptor(source, sym).enumerable;
      }));
    }

    ownKeys.forEach(function (key) {
      _defineProperty(target, key, source[key]);
    });
  }

  return target;
}

function _defineProperty(obj, key, value) {
  if (key in obj) {
    Object.defineProperty(obj, key, {
      value: value,
      enumerable: true,
      configurable: true,
      writable: true
    });
  } else {
    obj[key] = value;
  }

  return obj;
}


function visiblePanel() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    selectedPage: 0,
    selectedPageVisibleIndex: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  if (!action) {
    return _objectSpread2({}, state);
  }

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

  reactHotLoader.register(visiblePanel, "visiblePanel", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\visiblePanelReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_objectSpread2, "_objectSpread2", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\visiblePanelReducer.js");
  reactHotLoader.register(_defineProperty, "_defineProperty", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\visiblePanelReducer.js");
  reactHotLoader.register(visiblePanel, "visiblePanel", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\visiblePanelReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 76 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return importExport; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) {
  for (var i = 1; i < arguments.length; i++) {
    var source = arguments[i] != null ? arguments[i] : {};
    var ownKeys = Object.keys(source);

    if (typeof Object.getOwnPropertySymbols === 'function') {
      ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) {
        return Object.getOwnPropertyDescriptor(source, sym).enumerable;
      }));
    }

    ownKeys.forEach(function (key) {
      _defineProperty(target, key, source[key]);
    });
  }

  return target;
}

function _defineProperty(obj, key, value) {
  if (key in obj) {
    Object.defineProperty(obj, key, {
      value: value,
      enumerable: true,
      configurable: true,
      writable: true
    });
  } else {
    obj[key] = value;
  }

  return obj;
}


function importExport() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    jobs: [],
    portalId: -1,
    portals: [],
    totalJobs: 0,
    portalName: null,
    logoUrl: null,
    exportWizardStep: 0,
    importWizardStep: 0,
    exportJobId: -1,
    importPackages: [],
    selectedPackage: undefined,
    verificationSummary: undefined
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  if (!action) {
    return _objectSpread2({}, state);
  }

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SELECTED_SITE:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        portalId: action.portalId,
        portalName: action.portalName,
        currentJobId: action.currentJobId
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SELECTED_JOB:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        currentJobId: action.currentJobId
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].GO_TO_WIZARD_STEP:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        exportWizardStep: action.payload.wizardStep
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_PORTALS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        portals: action.portals
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_JOBS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        jobs: action.jobs,
        totalJobs: action.totalJobs,
        lastExportTime: action.lastExportTime,
        lastImportTime: action.lastImportTime
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_JOB_DETAILS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        job: action.job
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SUBMITTED_EXPORT_REQUEST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        exportJobId: action.jobId
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SUBMITTED_IMPORT_REQUEST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        importJobId: action.jobId,
        packageVerified: action.packageVerified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_IMPORT_PACKAGES:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        importPackages: action.importPackages,
        totalPackages: action.total,
        selectedPackage: action.selectedPackage,
        importWizardStep: 0,
        packageVerified: false
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].GO_TO_IMPORT_WIZARD_STEP:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        importWizardStep: action.importWizardStep
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].VERIFIED_IMPORT_PACKAGE:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        importSummary: action.importSummary
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].SELECTED_PACKAGE:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        selectedPackage: action.selectedPackage,
        importSummary: action.importSummary
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].VERIFIED_PACKAGE:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        packageVerified: action.packageVerified
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* importExport */ "a"].RETRIEVED_LAST_EXPORT_DATE:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        lastExportTime: action.lastExportTime
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

  reactHotLoader.register(importExport, "importExport", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\importExportReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_objectSpread2, "_objectSpread2", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\importExportReducer.js");
  reactHotLoader.register(_defineProperty, "_defineProperty", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\importExportReducer.js");
  reactHotLoader.register(importExport, "importExport", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\reducers\\importExportReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 77 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(78);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_devtools__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(79);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(80);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

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

var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\DevTools.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\DevTools.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\DevTools.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 78 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 79 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 80 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 81 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(102);
} else {}

/***/ }),
/* 82 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(5);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(1);
/* harmony import */ var _Dashboard__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(83);
/* harmony import */ var _ExportModal__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(87);
/* harmony import */ var _ImportModal__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(89);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(6);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}












var App =
/*#__PURE__*/
function (_Component) {
  _inherits(App, _Component);

  function App() {
    var _this;

    _classCallCheck(this, App);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(App).call(this));
    _this.state = {
      referrer: _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.referrer,
      referrerText: _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.referrerText,
      backToReferrerFunc: null
    };
    return _this;
  }

  _createClass(App, [{
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
    key: "componentDidMount",
    value: function componentDidMount() {
      var _this3 = this;

      var props = this.props;
      document.addEventListener("siteImportExport", function (e) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].siteSelected(e.importExportJob.portalId, e.importExportJob.portalName, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getLastExportTime({
            "portal": e.importExportJob.portalId,
            "jobType": "Export"
          }));

          _this3.selectPanel(e.importExportJob.jobType === "Export" ? 1 : 2);
        }));

        _this3.updateReferrerInfo(e);
      }, false);

      if (_utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.importExportJob) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].siteSelected(_utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.importExportJob.portalId, _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.importExportJob.portalName, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getLastExportTime({
            "portal": _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.importExportJob.portalId,
            "jobType": "Export"
          }));

          _this3.selectPanel(_utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.importExportJob.jobType === "Export" ? 1 : 2);
        }));
        this.updateReferrerInfo(_utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings);
      }
    }
  }, {
    key: "selectPanel",
    value: function selectPanel(panel, event) {
      if (event) {
        event.preventDefault();
      }

      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* visiblePanel */ "b"].selectPanel(panel));
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].jobSelected());
    }
  }, {
    key: "selectPanelInternal",
    value: function selectPanelInternal(panel, event) {
      if (event) {
        event.preventDefault();
      }

      var props = this.props;

      if (props.importWizardStep > 0) {
        _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].utilities.confirm(localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("CancelImportMessage"), localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ConfirmCancel"), localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("KeepImport"), function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].packageVerified(false));
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].importWizardGoToSetp(0));
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* visiblePanel */ "b"].selectPanel(panel));
        });
      } else {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* visiblePanel */ "b"].selectPanel(panel));
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].jobSelected());
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var PageHeaderSubtitleStyles = {
        clear: "both",
        display: "block",
        position: "relative",
        top: "-10px"
      };
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["PersonaBarPage"], {
          isOpen: props.selectedPage === 0,
          className: props.selectedPage !== 0 ? "hidden" : ""
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["PersonaBarPageHeader"], {
          title: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SiteImportExport.Header")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["PersonaBarPageBody"], {
          backToLinkProps: {
            text: state.referrer && state.referrerText,
            onClick: state.backToReferrerFunc
          }
        }, props.selectedPage === 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_Dashboard__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], {
          selectPanel: this.selectPanel.bind(this)
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["PersonaBarPage"], {
          isOpen: props.selectedPage === 1
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["PersonaBarPageHeader"], {
          title: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Export")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["PersonaBarPageBody"], {
          backToLinkProps: {
            text: state.referrer && state.referrerText || localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("BackToImportExport"),
            onClick: state.backToReferrerFunc || this.selectPanel.bind(this, 0)
          }
        }, props.selectedPage === 1 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_ExportModal__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"], {
          onCancel: this.selectPanelInternal.bind(this, 0),
          getInitialPortalTabs: _actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getInitialPortalTabs
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["PersonaBarPage"], {
          isOpen: props.selectedPage === 2
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["PersonaBarPageHeader"], {
          title: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Import")
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          style: PageHeaderSubtitleStyles
        }, props.portalName)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_9__["PersonaBarPageBody"], {
          backToLinkProps: {
            text: state.referrer && state.referrerText || localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("BackToImportExport"),
            onClick: state.backToReferrerFunc || this.selectPanelInternal.bind(this, 0)
          }
        }, props.selectedPage === 2 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_ImportModal__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], {
          onCancel: this.selectPanel.bind(this, 0)
        }))))
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
  selectedPageVisibleIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  portalId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  portalName: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  importWizardStep: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number
};

function mapStateToProps(state) {
  return {
    selectedPage: state.visiblePanel.selectedPage,
    selectedPageVisibleIndex: state.visiblePanel.selectedPageVisibleIndex,
    portalId: state.importExport.portalId,
    portalName: state.importExport.portalName,
    importWizardStep: state.importExport.importWizardStep
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(App);

var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(App, "App", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(App, "App", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\App.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 83 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(5);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(105);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(6);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(1);
/* harmony import */ var _JobRow__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(84);
/* harmony import */ var _FiltersBar__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(85);
/* harmony import */ var _JobDetails__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(86);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}













var DashboardPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(DashboardPanelBody, _Component);

  function DashboardPanelBody() {
    var _this;

    _classCallCheck(this, DashboardPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(DashboardPanelBody).call(this));
    _this.state = {
      jobs: [],
      pageIndex: 0,
      pageSize: 10,
      filter: null,
      keyword: "",
      currentJobId: null
    };
    return _this;
  }

  _createClass(DashboardPanelBody, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var _this2 = this;

      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getPortals(function (data) {
        if (data.TotalResults === 1) {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].siteSelected(data.Results[0].PortalID, data.Results[0].PortalName, function () {
            props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this2.getNextPage(data.Results[0].PortalID), function (data) {
              if (data.Jobs && data.Jobs.find(function (j) {
                return j.Status < 2 && !j.Cancelled;
              })) {
                _this2.addInterval(props);
              } else {
                clearInterval(_this2.jobListTimeout);
              }
            }));
          }));
        } else {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this2.getNextPage(props.portalId), function (data) {
            if (data.Jobs && data.Jobs.find(function (j) {
              return j.Status < 2 && !j.Cancelled;
            })) {
              _this2.addInterval(props);
            } else {
              clearInterval(_this2.jobListTimeout);
            }
          }));
        }
      }));
    }
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate() {
      var props = this.props;

      if (this.state.currentJobId !== props.currentJobId) {
        this.setState({
          currentJobId: props.currentJobId
        });
      }
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      clearInterval(this.jobListTimeout);
    }
  }, {
    key: "addInterval",
    value: function addInterval(props) {
      var _this3 = this;

      clearInterval(this.jobListTimeout);
      var persistedSettings = _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.persistent.load();
      this.jobListTimeout = setInterval(function () {
        if (persistedSettings.expandPersonaBar && persistedSettings.activeIdentifier === "Dnn.SiteImportExport") {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this3.getNextPage(), function (data) {
            if (!data.Jobs || !data.Jobs.find(function (j) {
              return j.Status < 2 && !j.Cancelled;
            })) {
              clearInterval(_this3.jobListTimeout);

              if (_this3.state.currentJobId) {
                props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getJobDetails(_this3.state.currentJobId));
              }
            } else if (_this3.state.currentJobId && data.Jobs.find(function (j) {
              return j.JobId === _this3.state.currentJobId && j.Status < 2 && !j.Cancelled;
            })) {
              props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getJobDetails(_this3.state.currentJobId));
            }
          }));
        }
      }, 5000);
    }
  }, {
    key: "uncollapse",
    value: function uncollapse(id) {
      this.props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].jobSelected(id));
    }
  }, {
    key: "collapse",
    value: function collapse() {
      if (this.props.currentJobId !== "") {
        this.props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].jobSelected());
      }
    }
  }, {
    key: "toggle",
    value: function toggle(id) {
      if (id !== "") {
        this.uncollapse(id);
      } else {
        this.collapse();
      }
    }
  }, {
    key: "isLastPageOnlyJob",
    value: function isLastPageOnlyJob(jobId) {
      var props = this.props,
          state = this.state;
      var total = Math.ceil(props.totalJobs / state.pageSize);
      var current = state.pageIndex * state.pageSize / state.pageSize + 1;
      var isLastPage = total === current ? true : false;

      if (isLastPage && props.jobs.length === 1 && props.jobs[0].JobId === jobId) {
        return true;
      } else return false;
    }
  }, {
    key: "getNextPage",
    value: function getNextPage(portalId) {
      var state = this.state,
          props = this.props;
      return {
        portal: portalId === undefined ? props.portalId : portalId,
        pageIndex: state.pageIndex || 0,
        pageSize: state.pageSize,
        jobType: state.filter === -1 ? null : state.filter,
        keywords: state.keyword
      };
    }
  }, {
    key: "getPortalOptions",
    value: function getPortalOptions() {
      var props = this.props;
      var options = [];

      if (props.portals) {
        options = props.portals.map(function (item) {
          return {
            label: item.PortalName,
            value: item.PortalID
          };
        });

        if (options.length > 1) {
          options.unshift({
            "label": localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("AllSites"),
            "value": -1
          });
        }
      }

      return options;
    }
  }, {
    key: "onPortalChange",
    value: function onPortalChange(option) {
      var _this4 = this;

      var props = this.props;

      if (option.value !== props.portalId) {
        this.setState({
          pageIndex: 0
        }, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].siteSelected(option.value, option.label));
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this4.getNextPage(option.value), function (data) {
            if (data.Jobs && data.Jobs.find(function (j) {
              return j.Status < 2 && !j.Cancelled;
            })) {
              _this4.addInterval(props);
            } else {
              clearInterval(_this4.jobListTimeout);
            }
          }));
        });
      }
    }
  }, {
    key: "onImportData",
    value: function onImportData() {
      var props = this.props;
      props.selectPanel(2);
    }
  }, {
    key: "onExportData",
    value: function onExportData() {
      var props = this.props;
      props.selectPanel(1);
    }
  }, {
    key: "onPageChange",
    value: function onPageChange(currentPage, pageSize) {
      var _this5 = this;

      var state = this.state,
          props = this.props;

      if (pageSize !== undefined && state.pageSize !== pageSize) {
        state.pageSize = pageSize;
      }

      state.pageIndex = currentPage;
      this.setState({
        state: state
      }, function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].jobSelected(null, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this5.getNextPage(props.portalId), function (data) {
            if (data.Jobs && data.Jobs.find(function (j) {
              return j.Status < 2 && !j.Cancelled;
            })) {
              _this5.addInterval(props);
            } else {
              clearInterval(_this5.jobListTimeout);
            }
          }));
        }));
      });
    }
  }, {
    key: "onFilterChanged",
    value: function onFilterChanged(filter) {
      var _this6 = this;

      var props = this.props;
      this.setState({
        pageIndex: 0,
        filter: filter.value
      }, function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].jobSelected(null, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this6.getNextPage(props.portalId), function (data) {
            if (data.Jobs && data.Jobs.find(function (j) {
              return j.Status < 2 && !j.Cancelled;
            })) {
              _this6.addInterval(props);
            } else {
              clearInterval(_this6.jobListTimeout);
            }
          }));
        }));
      });
    }
  }, {
    key: "onKeywordChanged",
    value: function onKeywordChanged(keyword) {
      var _this7 = this;

      var props = this.props;
      this.setState({
        pageIndex: 0,
        keyword: keyword
      }, function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].jobSelected(null, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this7.getNextPage(props.portalId), function (data) {
            if (data.Jobs && data.Jobs.find(function (j) {
              return j.Status < 2 && !j.Cancelled;
            })) {
              _this7.addInterval(props);
            } else {
              clearInterval(_this7.jobListTimeout);
            }
          }));
        }));
      });
    }
  }, {
    key: "cancelJob",
    value: function cancelJob(jobId) {
      var _this8 = this;

      var props = this.props;
      _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.confirm(localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("CancelJobMessage"), localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ConfirmCancel"), localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("No"), function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].cancelJob(jobId, function () {
          _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobCancelled"));
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this8.getNextPage(props.portalId), function (data) {
            if (data.Jobs && data.Jobs.find(function (j) {
              return j.Status < 2 && !j.Cancelled;
            })) {
              _this8.addInterval(props);
            } else {
              clearInterval(_this8.jobListTimeout);
            }
          }));
        }, function () {
          _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobCancel.ErrorMessage"));
        }));
      });
    }
  }, {
    key: "deleteJob",
    value: function deleteJob(jobId) {
      var _this9 = this;

      var props = this.props,
          state = this.state;
      _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.confirm(localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("DeleteJobMessage"), localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ConfirmDelete"), localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("No"), function () {
        if (_this9.isLastPageOnlyJob(jobId)) {
          _this9.setState({
            pageIndex: state.pageIndex > 0 ? state.pageIndex - 1 : 0
          }, function () {
            props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].deleteJob(jobId, function () {
              _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobDeleted"));

              _this9.collapse();

              props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this9.getNextPage(props.portalId), function (data) {
                if (data.Jobs && data.Jobs.find(function (j) {
                  return j.Status < 2 && !j.Cancelled;
                })) {
                  _this9.addInterval(props);
                } else {
                  clearInterval(_this9.jobListTimeout);
                }
              }));
            }, function () {
              _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobDelete.ErrorMessage"));
            }));
          });
        } else {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].deleteJob(jobId, function () {
            _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobDeleted"));

            _this9.collapse();

            props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs(_this9.getNextPage(props.portalId), function (data) {
              if (data.Jobs && data.Jobs.find(function (j) {
                return j.Status < 2 && !j.Cancelled;
              })) {
                _this9.addInterval(props);
              } else {
                clearInterval(_this9.jobListTimeout);
              }
            }));
          }, function () {
            _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobDelete.ErrorMessage"));
          }));
        }
      });
    }
  }, {
    key: "renderTopPane",
    value: function renderTopPane() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "top-panel"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__["GridCell"], {
          columnSize: 100
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "site-selection"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__["Dropdown"], {
          enabled: props.portals.length > 1 ? true : false,
          options: this.getPortalOptions(),
          autoHide: false,
          value: props.portalId,
          onSelect: this.onPortalChange.bind(this),
          prependWith: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ShowSiteLabel")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "last-actions"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "action-labels"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__["Label"], {
          labelType: "block",
          label: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LastImport")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__["Label"], {
          labelType: "block",
          label: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LastExport")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "action-dates"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, props.lastImportTime || localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("EmptyDateTime")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, props.lastExportTime || localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("EmptyDateTime")))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "action-buttons"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__["Button"], {
          disabled: props.portalId < 0,
          className: "action-button",
          type: "secondary",
          onClick: this.onExportData.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ExportButton")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__["Button"], {
          disabled: props.portalId < 0,
          className: "action-button",
          type: "secondary",
          onClick: this.onImportData.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("ImportButton")))))
      );
    }
  }, {
    key: "renderJobListHeader",
    value: function renderJobListHeader() {
      var tableFields = [{
        "name": "",
        "id": "Indicator"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobDate.Header"),
        "id": "JobDate"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobType.Header"),
        "id": "JobType"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobUser.Header"),
        "id": "JobUser"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobPortal.Header"),
        "id": "JobPortal"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("JobStatus.Header"),
        "id": "JobStatus"
      }, {
        "name": "",
        "id": "Arrow"
      }];
      var tableHeaders = tableFields.map(function (field, i) {
        var className = "jobHeader jobHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: className,
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, field.name, "\xA0 "))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "jobHeader-wrapper"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedJobList",
    value: function renderedJobList() {
      var _this10 = this;

      var props = this.props;

      if (props.portals.length > 0 && props.jobs && props.jobs.length > 0) {
        return props.jobs.map(function (job, index) {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_JobRow__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], {
              jobId: job.JobId,
              jobType: job.JobType,
              jobDate: job.CreatedOnString,
              jobUser: job.User,
              jobPortal: props.portals.find(function (p) {
                return p.PortalID === job.PortalId;
              }) ? props.portals.find(function (p) {
                return p.PortalID === job.PortalId;
              }).PortalName : localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("DeletedPortal"),
              jobStatus: job.Status,
              jobCancelled: job.Cancelled,
              index: index,
              key: "jobTerm-" + index,
              closeOnClick: true,
              openId: props.currentJobId,
              OpenCollapse: _this10.toggle.bind(_this10, job.JobId),
              Collapse: _this10.collapse.bind(_this10)
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_JobDetails__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"], {
              jobId: job.JobId,
              cancelled: job.Cancelled,
              Collapse: _this10.collapse.bind(_this10),
              openId: props.currentJobId,
              cancelJob: _this10.cancelJob.bind(_this10),
              deleteJob: _this10.deleteJob.bind(_this10)
            }))
          );
        });
      } else return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__["GridCell"], {
          className: "no-jobs"
        }, localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("NoJobs"))
      );
    }
  }, {
    key: "renderPager",
    value: function renderPager() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "logPager"
        }, props.jobs &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_10__["Pager"], {
          showStartEndButtons: true,
          showPageSizeOptions: true,
          showPageInfo: false,
          numericCounters: 4,
          pageSize: state.pageSize,
          totalRecords: props.totalJobs,
          onPageChanged: this.onPageChange.bind(this),
          pageSizeDropDownWithoutBorder: true,
          pageSizeOptionText: "{0} results per page",
          summaryText: "Showing {0}-{1} of {2} results",
          culture: _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.getCulture()
        }))
      );
    }
  }, {
    key: "renderedLegend",
    value: function renderedLegend() {
      var legendItems = [{
        "name": localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LegendExport"),
        "id": "legend-export"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LegendImport"),
        "id": "legend-import"
      }];
      var legend = legendItems.map(function (item, i) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "logLegend-item",
            key: i
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: item.id
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null)),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, item.name)))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "logLegend-wrapper"
        }, legend)
      );
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, props.portals.length > 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, this.renderTopPane(),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "section-title"
        }, localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LogSection")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_FiltersBar__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"], {
          onFilterChanged: this.onFilterChanged.bind(this),
          onKeywordChanged: this.onKeywordChanged.bind(this)
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "logContainer"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "logContainerBox"
        }, this.renderJobListHeader(), this.renderedJobList())), this.renderPager(), this.renderedLegend()))
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

  return DashboardPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

DashboardPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  jobs: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  portals: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  totalJobs: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  portalName: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  selectPanel: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  portalId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  lastExportTime: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  lastImportTime: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  currentJobId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number
};
DashboardPanelBody.defaultProps = {
  totalJobs: 0
};

function mapStateToProps(state) {
  return {
    jobs: state.importExport.jobs,
    portalId: state.importExport.portalId,
    portals: state.importExport.portals,
    totalJobs: state.importExport.totalJobs,
    portalName: state.importExport.portalName,
    lastExportTime: state.importExport.lastExportTime,
    lastImportTime: state.importExport.lastImportTime,
    currentJobId: state.importExport.currentJobId
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(DashboardPanelBody);

var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(DashboardPanelBody, "DashboardPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(DashboardPanelBody, "DashboardPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 84 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(1);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(107);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}






/*eslint-disable eqeqeq*/

var JobRow =
/*#__PURE__*/
function (_Component) {
  _inherits(JobRow, _Component);

  function JobRow() {
    var _this;

    _classCallCheck(this, JobRow);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(JobRow).call(this));
    _this.state = {
      collapsed: true,
      collapsedClass: true,
      repainting: false
    };
    return _this;
  }

  _createClass(JobRow, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var opened = this.props.openId !== "" && this.props.jobId === this.props.openId;
      this.setState({
        opened: opened
      });
    }
  }, {
    key: "toggle",
    value: function toggle() {
      if (this.props.openId !== "" && this.props.jobId === this.props.openId) {
        this.props.Collapse();
      } else {
        this.props.OpenCollapse(this.props.jobId);
      }
    }
  }, {
    key: "getTypeIndicator",
    value: function getTypeIndicator() {
      var props = this.props;
      return props.jobType.indexOf("Import") >= 0 ?
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "jobIndicator-import"
      }) :
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "jobIndicator-export"
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var opened = props.openId !== "" && this.props.jobId === props.openId;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "collapsible-jobdetail " + !opened + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "collapsible-jobdetail-header " + state.collapsed
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-header"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-cssclass",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "jobIndicator"
        }, this.getTypeIndicator())),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-createdate",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, props.jobDate))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-jobtype",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, props.jobType))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-username",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, props.jobUser, "\xA0 "))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-portalname",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, props.jobPortal, "\xA0 "))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-jobstatus",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
          className: "job-status" + (props.jobCancelled ? 4 : props.jobStatus)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["TextOverflowWrapper"], {
          text: localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("JobStatus" + (props.jobCancelled ? 4 : props.jobStatus)),
          maxWidth: 80
        }), props.jobStatus === 1 && !props.jobCancelled &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "cycle-icon",
          dangerouslySetInnerHTML: {
            __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SvgIcons"].CycleIcon
          }
        })))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-arrow",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "arrow-icon",
          dangerouslySetInnerHTML: {
            __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SvgIcons"].ArrowDownIcon
          }
        }))))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Collapsible"], {
          autoScroll: true,
          isOpened: opened,
          fixedHeight: 480,
          style: {
            float: "left",
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

  return JobRow;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

JobRow.propTypes = {
  jobId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  jobType: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  jobDate: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  jobUser: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  jobPortal: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  jobStatus: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  jobCancelled: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.node,
  className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  OpenCollapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  Collapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  openId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};
var _default = JobRow;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(JobRow, "JobRow", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(JobRow, "JobRow", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobRow\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 85 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(109);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}







var FiltersBar =
/*#__PURE__*/
function (_Component) {
  _inherits(FiltersBar, _Component);

  function FiltersBar(props) {
    var _this;

    _classCallCheck(this, FiltersBar);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(FiltersBar).call(this, props));
    _this.state = {
      selectedJobFilter: {
        label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("JobTypeAll"),
        value: -1
      },
      searchText: ""
    };
    return _this;
  }

  _createClass(FiltersBar, [{
    key: "onSelect",
    value: function onSelect(option) {
      var _this2 = this;

      var selectedJobFilter = this.state.selectedJobFilter;

      if (option.value !== selectedJobFilter.value) {
        selectedJobFilter.label = option.label;
        selectedJobFilter.value = option.value;
        this.setState({
          selectedJobFilter: selectedJobFilter
        }, function () {
          _this2.props.onFilterChanged(option);
        });
      }
    }
  }, {
    key: "BuildFiltersOptions",
    value: function BuildFiltersOptions() {
      var jobFilters = [{
        "Key": localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("JobTypeAll"),
        "Value": -1
      }, {
        "Key": localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("JobTypeImport"),
        "Value": 1
      }, {
        "Key": localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("JobTypeExport"),
        "Value": 0
      }];
      var jobFiltersOptions = [];
      jobFiltersOptions = jobFilters.map(function (jobFilters) {
        return {
          label: jobFilters.Key,
          value: jobFilters.Value
        };
      });
      return jobFiltersOptions;
    }
  }, {
    key: "render",
    value: function render() {
      var jobFiltersOptions = this.BuildFiltersOptions();
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "jobs-filter-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 35
        }, jobFiltersOptions.length > 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "job-filters-filter"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
          style: {
            width: "100%"
          },
          withBorder: false,
          options: jobFiltersOptions,
          onSelect: this.onSelect.bind(this),
          value: this.state.selectedJobFilter.value,
          prependWith: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("ShowFilterLabel")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "clear"
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 30
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, "\xA0 ")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 35
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "search-filter"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SearchBox"], {
          placeholder: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SearchPlaceHolder"),
          onSearch: this.props.onKeywordChanged,
          maxLength: 50,
          iconStyle: {
            right: 0
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "clear"
        }))))
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

  return FiltersBar;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

FiltersBar.propTypes = {
  onFilterChanged: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  onKeywordChanged: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired
};
var _default = FiltersBar;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(FiltersBar, "FiltersBar", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(FiltersBar, "FiltersBar", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\FiltersBar\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 86 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(111);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(5);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(1);
/* harmony import */ var _services_itemsToExportService__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(13);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}










var JobDetails =
/*#__PURE__*/
function (_Component) {
  _inherits(JobDetails, _Component);

  function JobDetails(props) {
    var _this;

    _classCallCheck(this, JobDetails);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(JobDetails).call(this));
    _this.state = {
      cancelled: props.cancelled
    };
    return _this;
  }

  _createClass(JobDetails, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;

      if (props.jobId) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_4__[/* importExport */ "a"].getJobDetails(props.jobId));
      }
    }
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate(prevProps) {
      var props = this.props;

      if (props.cancelled !== prevProps.cancelled) {
        this.setState({
          cancelled: props.cancelled
        });
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "getSummaryItem",
    value: function getSummaryItem(category) {
      var props = this.props,
          state = this.state;

      if (props.jobDetail.Summary) {
        var detail = props.jobDetail.Summary.SummaryItems.find(function (c) {
          return c.Category.toUpperCase() === category.toUpperCase();
        });

        if (detail) {
          if (detail.Completed || state.cancelled || props.jobDetail.Status !== 1 || detail.TotalItems === 0) {
            return detail.ProcessedItems > detail.TotalItems ? detail.TotalItemsString : detail.ProcessedItemsString + " / " + detail.TotalItemsString;
          } else {
            return (
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "cycle-icon",
                dangerouslySetInnerHTML: {
                  __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["SvgIcons"].CycleIcon
                }
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                style: {
                  float: "right"
                }
              }, detail.ProcessedItemsString + " / " + detail.TotalItemsString + " (" + (detail.ProcessedItems / detail.TotalItems * 100).toFixed(1) + "%)"))
            );
          }
        } else return "-";
      } else {
        return "-";
      }
    }
  }, {
    key: "getUsersSummary",
    value: function getUsersSummary() {
      var props = this.props,
          state = this.state;

      if (props.jobDetail.Summary) {
        var users = props.jobDetail.Summary.SummaryItems.find(function (c) {
          return c.Category === "USERS";
        });

        if (users && props.jobDetail.JobType === "Site Import") {
          var usersData = props.jobDetail.Summary.SummaryItems.find(function (c) {
            return c.Category === "USERS_DATA";
          });

          if (users.TotalItems > 0 && !users.Completed && !state.cancelled) {
            return (
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
                labelType: "inline",
                label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("UsersStep1")
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "import-summary-item users"
              },
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "cycle-icon",
                dangerouslySetInnerHTML: {
                  __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["SvgIcons"].CycleIcon
                }
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                style: {
                  float: "right"
                }
              }, users.ProcessedItemsString + " / " + users.TotalItemsString + " (" + (users.ProcessedItems / users.TotalItems * 100).toFixed(1) + "%)"))))
            );
          } else if (users.TotalItems > 0 && users.Completed && usersData.TotalItems === 0) {
            return (
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
                labelType: "inline",
                label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("UsersStep1")
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "import-summary-item users"
              }, users.ProcessedItems > users.TotalItems ? users.TotalItemsString : users.ProcessedItemsString + " / " + users.TotalItemsString))
            );
          } else if (usersData.TotalItems > 0 && !usersData.Completed && !state.cancelled) {
            return (
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
                labelType: "inline",
                label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("UsersStep2")
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "import-summary-item users"
              },
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "cycle-icon",
                dangerouslySetInnerHTML: {
                  __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["SvgIcons"].CycleIcon
                }
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                style: {
                  float: "right"
                }
              }, usersData.ProcessedItemsString + " / " + usersData.TotalItemsString + " (" + (usersData.ProcessedItems / usersData.TotalItems * 100).toFixed(1) + "%)"))))
            );
          } else {
            return (
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
                labelType: "inline",
                label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Users")
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "import-summary-item users"
              }, users.ProcessedItems > users.TotalItems ? users.TotalItemsString : users.ProcessedItemsString + " / " + users.TotalItemsString))
            );
          }
        } else if (users && props.jobDetail.JobType === "Site Export") {
          if (users.TotalItems > 0 && !users.Completed && !state.cancelled) {
            return (
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
                labelType: "inline",
                label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Users")
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "import-summary-item"
              },
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "cycle-icon",
                dangerouslySetInnerHTML: {
                  __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["SvgIcons"].CycleIcon
                }
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                style: {
                  float: "right"
                }
              }, users.ProcessedItemsString + " / " + users.TotalItemsString + " (" + (users.ProcessedItems / users.TotalItems * 100).toFixed(1) + "%)"))))
            );
          } else {
            return (
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
                labelType: "inline",
                label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Users")
              }),
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "import-summary-item"
              }, users.ProcessedItems > users.TotalItems ? users.TotalItemsString : users.ProcessedItemsString + " / " + users.TotalItemsString))
            );
          }
        } else {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
              labelType: "inline",
              label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Users")
            }),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "import-summary-item"
            }, "-"))
          );
        }
      }
    }
  }, {
    key: "cancel",
    value: function cancel(jobId) {
      this.props.cancelJob(jobId);
    }
  }, {
    key: "delete",
    value: function _delete(jobId) {
      this.props.deleteJob(jobId);
    }
  }, {
    key: "renderExportSummary",
    value: function renderExportSummary() {
      var _this2 = this;

      var props = this.props,
          state = this.state;
      var registeredItemsToExport = _services_itemsToExportService__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].getRegisteredItemsToExport();
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          style: {
            float: "left",
            width: "100%"
          }
        }, props.jobDetail &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "export-summary"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], {
          className: "export-site-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "left-column"
        }, this.getUsersSummary(),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Pages")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item pagers"
        }, this.getSummaryItem("Pages"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Roles")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Roles"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Vocabularies")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Vocabularies"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("PageTemplates")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Templates"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("IncludeContent")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.jobDetail.Summary.IncludeContent ? localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("No"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("IncludeProfileProperties")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.jobDetail.Summary.IncludeProfileProperties ? localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("No"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("IncludePermissions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.jobDetail.Summary.IncludePermissions ? localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("No"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("IncludeExtensions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.jobDetail.Summary.IncludeExtensions ? localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("No"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("IncludeDeletions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.jobDetail.Summary.IncludeDeletions ? localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("No")))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "right-column"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Name")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.jobDetail.Name || "-")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("FolderName")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.jobDetail.ExportFile)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Extensions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Packages"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Assets")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Assets"))), registeredItemsToExport.map(function (item, i) {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], {
              key: i
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
              labelType: "inline",
              label: item.name
            }),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "import-summary-item"
            }, _this2.getSummaryItem(item.category)))
          );
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("TotalExportSize")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.jobDetail.Summary.ExportFileInfo ? props.jobDetail.Summary.ExportFileInfo.ExportSize : "-")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("ExportMode")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.jobDetail.Summary.ExportMode === 1 ? localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("ExportModeDifferential") : localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("ExportModeComplete"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "summary-note"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "note-title"
        }, localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SummaryNoteTitle")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "note-description"
        }, localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SummaryNoteDescription"))))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], {
          className: "action-buttons"
        }, props.jobDetail.Status < 2 && !state.cancelled &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Button"], {
          type: "secondary",
          onClick: this.cancel.bind(this, props.jobId)
        }, props.jobDetail.JobType.indexOf("Export") >= 0 ? localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("CancelExport") : localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("CancelImport")), (props.jobDetail.Status > 1 || state.cancelled) &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["Button"], {
          type: "secondary",
          onClick: this.delete.bind(this, props.jobId)
        }, localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Delete"))))))
      );
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props;

      if (props.jobDetail !== undefined) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "job-details"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "summary-title"
          }, props.jobDetail.JobType.indexOf("Export") >= 0 ? localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("ExportSummary") : localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("ImportSummary")), this.renderExportSummary())
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

  return JobDetails;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

JobDetails.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  jobDetail: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  jobId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  cancelled: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  Collapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  cancelJob: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  deleteJob: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func
};

function mapStateToProps(state) {
  return {
    jobDetail: state.importExport.job
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(JobDetails);

var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(JobDetails, "JobDetails", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(JobDetails, "JobDetails", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\Dashboard\\JobDetails\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 87 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(5);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(1);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(88);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var utils__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(6);
/* harmony import */ var _services_itemsToExportService__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(13);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}










var keysToValidate = ["ExportName"];

var ExportModal =
/*#__PURE__*/
function (_Component) {
  _inherits(ExportModal, _Component);

  function ExportModal(props) {
    var _this;

    _classCallCheck(this, ExportModal);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ExportModal).call(this));
    _this.state = {
      wizardStep: 0,
      exportRequest: {
        PortalId: -1,
        ExportName: "",
        ExportDescription: "",
        IncludeUsers: true,
        IncludeVocabularies: true,
        IncludeTemplates: true,
        IncludeProperfileProperties: true,
        IncludeRoles: true,
        IncludePermissions: true,
        IncludeDeletions: false,
        IncludeContent: true,
        IncludeExtensions: true,
        IncludeFiles: true,
        ExportMode: props.lastExportTime ? "Differential" : "Full",
        ItemsToExport: _this.getRegisteredItemsToExport(),
        RunNow: true
      },
      errors: {
        ExportName: false
      },
      IncludeContentEnabled: true,
      reloadPages: false,
      requestSubmitting: false
    };
    _this.getInitialPortalTabs = props.getInitialPortalTabs;
    return _this;
  }

  _createClass(ExportModal, [{
    key: "getRegisteredItemsToExport",
    value: function getRegisteredItemsToExport() {
      return _services_itemsToExportService__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].getRegisteredItemsToExport().filter(function (x) {
        return x.defaultSelected;
      }).map(function (item) {
        return item.category;
      });
    }
  }, {
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props,
          state = this.state;
      var exportRequest = state.exportRequest;
      exportRequest.PortalId = props.portalId;
      this.setState({
        exportRequest: exportRequest
      });
    }
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate() {
      var _this2 = this;

      var props = this.props,
          state = this.state;
      var exportRequest = state.exportRequest;
      var reloadPages = state.reloadPages;

      if (exportRequest.PortalId !== props.portalId) {
        exportRequest.PortalId = props.portalId;
        reloadPages = true;
        this.setState({
          exportRequest: exportRequest,
          reloadPages: reloadPages
        }, function () {
          _this2.setState({
            reloadPages: false
          });
        });
      }
    }
  }, {
    key: "goToStep",
    value: function goToStep(wizardStep) {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].navigateWizard(wizardStep));
    }
  }, {
    key: "cancelExport",
    value: function cancelExport() {
      var props = this.props;
      this.goToStep(0);
      props.onCancel();
    }
  }, {
    key: "onExport",
    value: function onExport() {
      var _this3 = this;

      var props = this.props,
          state = this.state;

      if (this.Validate()) {
        this.setState({
          requestSubmitting: true
        });
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].exportSite(state.exportRequest, function (data) {
          _this3.setState({
            requestSubmitting: false
          });

          utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ExportRequestSubmitted"));
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs({
            portal: props.portalId,
            pageIndex: 0,
            pageSize: 10
          }, function () {
            props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].jobSelected(data.jobId));
          }));
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* visiblePanel */ "b"].selectPanel(0));
        }, function () {
          _this3.setState({
            requestSubmitting: false
          });

          utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ExportRequestSubmit.ErrorMessage"));
        }));
      }
    }
  }, {
    key: "Validate",
    value: function Validate() {
      var success = true;
      success = this.ValidateTexts();

      if (success && !this.ValidateHasExportItem()) {
        utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("NoExportItem.ErrorMessage"));
        success = false;
      }

      return success;
    }
  }, {
    key: "getDescendantPortalTabs",
    value: function getDescendantPortalTabs(PortalTabsParameters, ParentTabId, callback) {
      this.props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getDescendantPortalTabs(PortalTabsParameters, ParentTabId, callback));
    }
  }, {
    key: "ValidateTexts",
    value: function ValidateTexts(key) {
      var _this4 = this;

      var success = true;
      var exportRequest = this.state.exportRequest;
      var errors = this.state.errors;
      keysToValidate.map(function (vkey) {
        if (key === undefined || key === vkey) {
          if (exportRequest[vkey] === "") {
            success = false;
            errors[vkey] = true;
          } else {
            errors[vkey] = false;
          }
        }

        _this4.setState({});
      });
      return success;
    }
  }, {
    key: "ValidateHasExportItem",
    value: function ValidateHasExportItem() {
      var success = true;
      var exportRequest = this.state.exportRequest;

      if (exportRequest.IncludeContent || exportRequest.IncludeFiles || exportRequest.IncludeUsers || exportRequest.IncludeRoles || exportRequest.IncludeVocabularies || exportRequest.IncludeTemplates || exportRequest.IncludeProperfileProperties || exportRequest.IncludePermissions || exportRequest.IncludeExtensions || exportRequest.pages && exportRequest.pages.length > 0 || exportRequest.ItemsToExport && exportRequest.ItemsToExport.length > 0) {
        success = true;
      } else {
        success = false;
      }

      return success;
    }
  }, {
    key: "getPortalOptions",
    value: function getPortalOptions() {
      var props = this.props;
      var options = [{
        label: props.portalName,
        value: props.portalId
      }];
      return options;
    }
  }, {
    key: "onChange",
    value: function onChange(key, event) {
      var value = _typeof(event) === "object" ? event.target.value : event;
      var exportRequest = this.state.exportRequest;
      exportRequest[key] = value;
      this.setState({
        exportRequest: exportRequest
      });
      if (keysToValidate.some(function (vkey) {
        return vkey === key;
      })) this.ValidateTexts(key);
    }
  }, {
    key: "onChangeItemsToExport",
    value: function onChangeItemsToExport(name) {
      var exportRequest = this.state.exportRequest;
      var index = exportRequest.ItemsToExport.indexOf(name);

      if (index !== -1) {
        exportRequest.ItemsToExport.splice(index, 1);
      } else {
        exportRequest.ItemsToExport.push(name);
      }

      this.setState({
        exportRequest: exportRequest
      });
    }
  }, {
    key: "updatePagesToExport",
    value: function updatePagesToExport(selectedPages) {
      var _this5 = this;

      var exportRequest = this.state.exportRequest;
      var prevCount = exportRequest.pages !== null && exportRequest.pages !== undefined ? exportRequest.pages.length : 0;
      exportRequest.pages = selectedPages;

      if (prevCount === 0) {
        exportRequest["IncludeContent"] = !(selectedPages === undefined || selectedPages.length <= 0);
      }

      this.setState({
        exportRequest: exportRequest
      }, function () {
        if (keysToValidate.some(function (vkey) {
          return vkey === "IncludeContent";
        })) _this5.ValidateTexts("IncludeContent");
        var state = _this5.state;
        state.IncludeContentEnabled = !(selectedPages === undefined || selectedPages.length <= 0);

        _this5.setState({
          state: state
        });
      });
    }
  }, {
    key: "getExportModeOptions",
    value: function getExportModeOptions() {
      var options = [{
        label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ExportModeDifferential"),
        value: "Differential"
      }, {
        label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ExportModeComplete"),
        value: "Full"
      }];
      return options;
    }
  }, {
    key: "render",
    value: function render() {
      var _this6 = this;

      var state = this.state,
          props = this.props;
      var PortalTabsParameters = {
        portalId: props.portalId,
        cultureCode: "",
        isMultiLanguage: false,
        excludeAdminTabs: true,
        disabledNotSelectable: false,
        roles: "",
        sortOrder: 0,
        includeDisabled: true
      };
      var registeredItemsToExport = _services_itemsToExportService__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].getRegisteredItemsToExport();
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: _style_less__WEBPACK_IMPORTED_MODULE_5___default.a.exportModal
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "pageTitle"
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ExportSettings")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["GridCell"], {
          className: "export-site-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "left-column"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "block",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Site")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Dropdown"], {
          enabled: false,
          options: this.getPortalOptions(),
          value: props.portalId,
          onSelect: this.onChange.bind(this, "PortalId")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["SingleLineInputWithError"], {
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Name") + "*",
          inputStyle: {
            margin: "0"
          },
          withLabel: false,
          maxLength: 100,
          error: state.errors.ExportName,
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ExportName.ErrorMessage"),
          value: state.exportRequest.ExportName,
          onChange: this.onChange.bind(this, "ExportName"),
          style: {
            width: "100%"
          }
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "right-column"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["MultiLineInputWithError"], {
          inputStyle: {
            "minHeight": 110
          },
          maxLength: 250,
          style: {
            "width": "100%"
          },
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Description"),
          value: state.exportRequest.ExportDescription,
          onChange: this.onChange.bind(this, "ExportDescription")
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "seperator"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("hr", null)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["GridSystem"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "export-switches"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "sectionTitle"
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("IncludeInExport")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Content")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludeContent,
          onChange: this.onChange.bind(this, "IncludeContent"),
          readOnly: !state.IncludeContentEnabled
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Assets")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludeFiles,
          onChange: this.onChange.bind(this, "IncludeFiles")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Users")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludeUsers,
          onChange: this.onChange.bind(this, "IncludeUsers")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Roles")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludeRoles,
          onChange: this.onChange.bind(this, "IncludeRoles")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Vocabularies")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludeVocabularies,
          onChange: this.onChange.bind(this, "IncludeVocabularies")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("PageTemplates")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludeTemplates,
          onChange: this.onChange.bind(this, "IncludeTemplates")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ProfileProperties")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludeProperfileProperties,
          onChange: this.onChange.bind(this, "IncludeProperfileProperties")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Permissions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludePermissions,
          onChange: this.onChange.bind(this, "IncludePermissions")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Extensions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludeExtensions,
          onChange: this.onChange.bind(this, "IncludeExtensions")
        })), registeredItemsToExport.map(function (item, i) {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], {
              key: i
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
              labelType: "inline",
              label: item.name
            }),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
              onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
              offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
              value: state.exportRequest.ItemsToExport.indexOf(item.category) !== -1,
              onChange: _this6.onChangeItemsToExport.bind(_this6, item.category)
            }))
          );
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          style: {
            "paddingBottom": 20,
            "paddingTop": 10
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("hr", null)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("DeletionsInExport")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.IncludeDeletions,
          onChange: this.onChange.bind(this, "IncludeDeletions")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("RunNow")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SwitchOff"),
          value: state.exportRequest.RunNow,
          onChange: this.onChange.bind(this, "RunNow")
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "export-pages"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "sectionTitle"
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("PagesInExport")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["TreeControlInteractor"], {
          PortalTabsParameters: PortalTabsParameters,
          OnSelect: this.updatePagesToExport.bind(this),
          moduleRoot: "PersonaBar",
          controller: "Tabs",
          serviceFramework: utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.sf,
          getInitialPortalTabs: this.getInitialPortalTabs,
          getDescendantPortalTabs: this.getDescendantPortalTabs.bind(this)
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "seperator2"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("hr", null)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ExportMode")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["RadioButtons"], {
          disabled: !props.lastExportTime,
          onChange: this.onChange.bind(this, "ExportMode"),
          options: this.getExportModeOptions(),
          buttonGroup: "exportMode",
          value: props.lastExportTime ? state.exportRequest.ExportMode : "Full"
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("LastExport")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "lastExport"
        }, props.lastExportTime || localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("EmptyDateTime"))))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["GridCell"], {
          className: "action-buttons"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Button"], {
          type: "secondary",
          onClick: this.cancelExport.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Cancel")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["Button"], {
          type: "primary",
          disabled: state.requestSubmitting,
          onClick: this.onExport.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("BeginExport")))))
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

  return ExportModal;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

ExportModal.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  onCancel: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  wizardStep: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  exportLogs: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  viewingLog: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  portalId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number.isRequired,
  portalName: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  exportJobId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  lastExportTime: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  getInitialPortalTabs: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired
};

function mapStateToProps(state) {
  return {
    wizardStep: state.importExport.exportWizardStep,
    exportLogs: state.importExport.exportLogs,
    viewingLog: state.importExport.viewingLog,
    exportJobId: state.importExport.exportJobId,
    lastExportTime: state.importExport.lastExportTime,
    portalId: state.importExport.portalId,
    portalName: state.importExport.portalName
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(ExportModal);

var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(keysToValidate, "keysToValidate", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(ExportModal, "ExportModal", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(keysToValidate, "keysToValidate", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(ExportModal, "ExportModal", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ExportModal\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 88 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(113);

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
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(5);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(1);
/* harmony import */ var _ImportSummary__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(90);
/* harmony import */ var _PackagesList__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(91);
/* harmony import */ var _PackageCard__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(36);
/* harmony import */ var _FiltersBar__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(93);
/* harmony import */ var _ProgressBar__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(94);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(95);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_10___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_10__);
/* harmony import */ var utils__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(6);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_12___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_12__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}















var noDataImg = __webpack_require__(156).default;

var ImportModal =
/*#__PURE__*/
function (_Component) {
  _inherits(ImportModal, _Component);

  function ImportModal() {
    var _this;

    _classCallCheck(this, ImportModal);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ImportModal).call(this));
    _this.state = {
      importRequest: {
        PortalId: -1,
        PackageId: -1,
        CollisionResolution: 1,
        RunNow: true
      },
      pageIndex: 0,
      pageSize: 5,
      filter: "newest",
      keyword: "",
      requestSubmitting: false
    };
    return _this;
  }

  _createClass(ImportModal, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props,
          state = this.state;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getImportPackages(this.getNextPage()));
      var importRequest = state.importRequest;
      importRequest.PortalId = props.portalId;
      importRequest.PackageId = props.selectedPackage ? props.selectedPackage.PackageId : -1;
      this.setState({
        importRequest: importRequest
      });
    }
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate() {
      var props = this.props,
          state = this.state;
      var importRequest = state.importRequest;

      if (importRequest.PortalId === -1 || importRequest.PortalId !== props.portalId) {
        importRequest.PortalId = props.portalId;
        this.setState({
          importRequest: importRequest
        });
      }
    }
  }, {
    key: "goToStep",
    value: function goToStep(wizardStep) {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].navigateWizard(wizardStep));
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].importWizardGoToSetp(0, function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].selectPackage());
      }));
    }
  }, {
    key: "selectPackage",
    value: function selectPackage(pkg) {
      var _this2 = this;

      var props = this.props;

      if (props.selectedPackage && props.selectedPackage.PackageId === pkg.PackageId) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].selectPackage());
      } else {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].selectPackage(pkg, function () {
          var importRequest = _this2.state.importRequest;
          importRequest.PackageId = pkg.PackageId;

          _this2.setState({
            importRequest: importRequest
          });
        }));
      }
    }
  }, {
    key: "cancelImport",
    value: function cancelImport() {
      var props = this.props;

      if (props.wizardStep === 0) {
        this.goToStep(0);
        props.onCancel();
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].packageVerified(false));
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].importWizardGoToSetp(0));
      } else {
        utils__WEBPACK_IMPORTED_MODULE_11__[/* default */ "a"].utilities.confirm(localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("CancelImportMessage"), localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ConfirmCancel"), localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("KeepImport"), function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].packageVerified(false));
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].selectPackage());
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].importWizardGoToSetp(0));
        });
      }
    }
  }, {
    key: "onAnalyze",
    value: function onAnalyze() {
      var props = this.props;

      if (props.selectedPackage) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].importWizardGoToSetp(1, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].verifyImportPackage(props.selectedPackage.PackageId, function () {}, function () {
            props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].packageVerified(true));
          }));
        }));
      } else {
        utils__WEBPACK_IMPORTED_MODULE_11__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SelectException"));
      }
    }
  }, {
    key: "onImport",
    value: function onImport() {
      var _this3 = this;

      var props = this.props,
          state = this.state;
      this.setState({
        requestSubmitting: true
      });
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].importSite(state.importRequest, function (data) {
        _this3.setState({
          requestSubmitting: false
        });

        utils__WEBPACK_IMPORTED_MODULE_11__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ImportRequestSubmitted"));
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].packageVerified(false));

        _this3.goToStep(0);

        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getAllJobs({
          portal: state.importRequest.PortalId,
          pageIndex: 0,
          pageSize: 10
        }, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].jobSelected(data.jobId));
        }));
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* visiblePanel */ "b"].selectPanel(0));
      }, function () {
        _this3.setState({
          requestSubmitting: false
        });

        utils__WEBPACK_IMPORTED_MODULE_11__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("ImportRequestSubmit.ErrorMessage"));
      }));
    }
  }, {
    key: "getSummaryItem",
    value: function getSummaryItem(category) {
      var props = this.props;
      var detail = props.importSummary.SummaryItems.find(function (c) {
        return c.Category === category.toUpperCase();
      });
      return detail ? detail.TotalItemsString : "-";
    }
  }, {
    key: "onSwitchChange",
    value: function onSwitchChange(key, event) {
      var value = _typeof(event) === "object" ? event.target.value : event;
      var importRequest = this.state.importRequest;

      if (key === "CollisionResolution") {
        importRequest.CollisionResolution = value ? 1 : 0;
      } else if (key === "RunNow") {
        importRequest.RunNow = value;
      }

      this.setState({
        importRequest: importRequest
      });
    }
  }, {
    key: "getNextPage",
    value: function getNextPage() {
      var state = this.state;
      return {
        pageIndex: state.pageIndex || 0,
        pageSize: state.pageSize,
        order: state.filter,
        keyword: state.keyword
      };
    }
  }, {
    key: "onFilterChanged",
    value: function onFilterChanged(filter) {
      var _this4 = this;

      var props = this.props;
      this.setState({
        pageIndex: 0,
        filter: filter.value
      }, function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getImportPackages(_this4.getNextPage()));
      });
    }
  }, {
    key: "onKeywordChanged",
    value: function onKeywordChanged(keyword) {
      var _this5 = this;

      var props = this.props;
      this.setState({
        pageIndex: 0,
        keyword: keyword
      }, function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getImportPackages(_this5.getNextPage()));
      });
    }
  }, {
    key: "onPageChange",
    value: function onPageChange(currentPage, pageSize) {
      var _this6 = this;

      var state = this.state,
          props = this.props;

      if (pageSize !== undefined && state.pageSize !== pageSize) {
        state.pageSize = pageSize;
      }

      state.pageIndex = currentPage;
      this.setState({
        state: state
      }, function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].getImportPackages(_this6.getNextPage()));
      });
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "renderPackageVerification",
    value: function renderPackageVerification() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, props.selectedPackage && !props.packageVerified &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-analyzing"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "noDataText"
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("VerifyPackage")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "noDataImage",
          dangerouslySetInnerHTML: {
            __html: noDataImg
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_ProgressBar__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"], {
          className: "progressCards",
          visible: true,
          loaded: props.importSummary
        })))
      );
    }
  }, {
    key: "renderPager",
    value: function renderPager() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "packagePager"
        }, props.importPackages &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_12__["Pager"], {
          showStartEndButtons: true,
          showPageSizeOptions: true,
          showPageInfo: false,
          numericCounters: 4,
          pageSize: state.pageSize,
          totalRecords: props.totalPackages,
          onPageChanged: this.onPageChange.bind(this),
          pageSizeDropDownWithoutBorder: true,
          pageSizeOptionText: "{0} results per page",
          summaryText: "Showing {0}-{1} of {2} results",
          culture: utils__WEBPACK_IMPORTED_MODULE_11__[/* default */ "a"].utilities.getCulture()
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
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: _style_less__WEBPACK_IMPORTED_MODULE_10___default.a.importModal
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "pageTitle"
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SelectImportPackage")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "packages-wrapper"
        }, props.wizardStep === 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_FiltersBar__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"], {
          onFilterChanged: this.onFilterChanged.bind(this),
          onKeywordChanged: this.onKeywordChanged.bind(this)
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "packages"
        }, props.wizardStep === 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_PackagesList__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"], {
          selectPackage: this.selectPackage.bind(this)
        }), props.wizardStep === 1 && props.selectedPackage &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-card-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_PackageCard__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], {
          selectedPackage: props.selectedPackage
        })), props.wizardStep === 1 && !props.packageVerified && this.renderPackageVerification()), props.wizardStep === 0 && this.renderPager(), props.wizardStep === 1 && props.packageVerified &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_ImportSummary__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], {
          collisionResolution: state.importRequest.CollisionResolution,
          runNow: state.importRequest.RunNow,
          onSwitchChange: this.onSwitchChange.bind(this)
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_12__["GridCell"], {
          className: "action-buttons"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_12__["Button"], {
          type: "secondary",
          onClick: this.cancelImport.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Cancel")), props.wizardStep === 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_12__["Button"], {
          type: "primary",
          disabled: props.selectedPackage ? false : true,
          onClick: this.onAnalyze.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Continue")), props.wizardStep === 1 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_12__["Button"], {
          type: "primary",
          disabled: !props.importSummary || state.requestSubmitting,
          onClick: this.onImport.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Continue"))))
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

  return ImportModal;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

ImportModal.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  portalId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number.isRequired,
  portalName: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  onCancel: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  wizardStep: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  importPackages: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  selectedPackage: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  importSummary: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  totalPackages: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  packageVerified: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};
ImportModal.defaultProps = {
  totalPackages: 0
};

function mapStateToProps(state) {
  return {
    wizardStep: state.importExport.importWizardStep,
    importPackages: state.importExport.importPackages,
    selectedPackage: state.importExport.selectedPackage,
    importSummary: state.importExport.importSummary,
    totalPackages: state.importExport.totalPackages,
    portalId: state.importExport.portalId,
    packageVerified: state.importExport.packageVerified
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(ImportModal);

var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(noDataImg, "noDataImg", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(ImportModal, "ImportModal", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(noDataImg, "noDataImg", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(ImportModal, "ImportModal", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 90 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(1);
/* harmony import */ var _services_itemsToExportService__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(13);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}








var ImportSummary =
/*#__PURE__*/
function (_Component) {
  _inherits(ImportSummary, _Component);

  function ImportSummary() {
    _classCallCheck(this, ImportSummary);

    return _possibleConstructorReturn(this, _getPrototypeOf(ImportSummary).apply(this, arguments));
  }

  _createClass(ImportSummary, [{
    key: "getSummaryItem",
    value: function getSummaryItem(category) {
      var props = this.props;
      var detail = props.importSummary.SummaryItems.find(function (c) {
        return c.Category.toUpperCase() === category.toUpperCase();
      });
      return detail ? detail.TotalItemsString : "-";
    }
  }, {
    key: "onChange",
    value: function onChange(key, e) {
      this.props.onSwitchChange(key, e);
    }
  }, {
    key: "render",
    value: function render() {
      var _this = this;

      var props = this.props;
      var registeredItemsToExport = _services_itemsToExportService__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].getRegisteredItemsToExport();
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          style: {
            float: "left",
            width: "100%"
          }
        }, props.importSummary && props.selectedPackage &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "sectionTitle"
        }, localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("ImportSummary")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], {
          className: "import-site-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "left-column"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Users")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Users"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Pages")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Pages"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Roles")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Roles"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Vocabularies")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Vocabularies"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("PageTemplates")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Templates"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("IncludeContent")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.importSummary.IncludeContent ? localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("No"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("IncludeProfileProperties")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.importSummary.IncludeProfileProperties ? localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("No"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("IncludePermissions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.importSummary.IncludePermissions ? localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("No"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("IncludeExtensions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.importSummary.IncludeExtensions ? localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("No"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("IncludeDeletions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.importSummary.IncludeDeletions ? localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Yes") : localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("No")))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "right-column"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("FolderName")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.selectedPackage.FileName)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Timestamp")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.selectedPackage.ExporTimeString)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Extensions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Packages"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Assets")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, this.getSummaryItem("Assets"))), registeredItemsToExport.map(function (item, i) {
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], {
              key: i
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
              labelType: "inline",
              label: item.name
            }),
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
              className: "import-summary-item"
            }, _this.getSummaryItem(item.category)))
          );
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("TotalExportSize")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.importSummary.ExportFileInfo.ExportSize)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("ExportMode")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.importSummary.ExportMode === 1 ? localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("ExportModeDifferential") : localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("ExportModeComplete"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("LastExport")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "import-summary-item"
        }, props.lastExportTime || localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("EmptyDateTime"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "seperator"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("hr", null)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], {
          style: {
            paddingBottom: "6"
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("OverwriteCollisions")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SwitchOff"),
          value: props.collisionResolution === 0 ? false : true,
          onChange: this.onChange.bind(this, "CollisionResolution")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Label"], {
          labelType: "inline",
          label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("RunNow")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_5__["Switch"], {
          onText: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SwitchOn"),
          offText: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SwitchOff"),
          value: props.runNow,
          onChange: this.onChange.bind(this, "RunNow")
        })))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "finish-importing"
        }, localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("FinishImporting"))))
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

  return ImportSummary;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

ImportSummary.propTypes = {
  importSummary: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  selectedPackage: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  collisionResolution: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  runNow: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  onSwitchChange: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  lastExportTime: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};

function mapStateToProps(state) {
  return {
    selectedPackage: state.importExport.selectedPackage,
    importSummary: state.importExport.importSummary,
    lastExportTime: state.importExport.lastExportTime
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(ImportSummary);

var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(ImportSummary, "ImportSummary", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(ImportSummary, "ImportSummary", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ImportSummary.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 91 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(1);
/* harmony import */ var _PackageCard__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(36);
/* harmony import */ var _PackageCardOverlay__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(92);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}









var PackagesList =
/*#__PURE__*/
function (_Component) {
  _inherits(PackagesList, _Component);

  function PackagesList() {
    _classCallCheck(this, PackagesList);

    return _possibleConstructorReturn(this, _getPrototypeOf(PackagesList).apply(this, arguments));
  }

  _createClass(PackagesList, [{
    key: "onSelect",
    value: function onSelect(pkg) {
      this.props.selectPackage(pkg);
    }
  }, {
    key: "renderTooltipMessage",
    value: function renderTooltipMessage(description) {
      if (description) {
        return "<div><div style='text-transform: uppercase;font-weight: 700;padding: 20px 20px 15px 20px;color: #000'>" + localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("PackageDescription") + "</div><div style='color: #4b4e4f;padding: 0 20px 20px 20px;'>" + description + "</div></div>";
      } else return;
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var _this = this;

      var props = this.props;

      if (props.importPackages && props.importPackages.length > 0) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "package-cards"
          }, props.importPackages.map(function (pkg, i) {
            return (
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "package-card-wrapper",
                key: i
              },
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_PackageCard__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
                selectedPackage: pkg,
                className: props.selectedPackage && props.selectedPackage.PackageId === pkg.PackageId ? "package-card selected" : "package-card"
              },
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_PackageCardOverlay__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], {
                selectPackage: _this.onSelect.bind(_this, pkg),
                packageName: pkg.Name,
                packageDescription: pkg.Description,
                isSelected: props.selectedPackage && props.selectedPackage.PackageId === pkg.PackageId
              }), props.selectedPackage && props.selectedPackage.PackageId === pkg.PackageId &&
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "checkmark",
                dangerouslySetInnerHTML: {
                  __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["SvgIcons"].CheckMarkIcon
                }
              })), pkg.Description &&
              /*#__PURE__*/
              react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
                className: "package-card-tooltip",
                dangerouslySetInnerHTML: {
                  __html: _this.renderTooltipMessage(pkg.Description)
                },
                onClick: _this.onSelect.bind(_this, pkg)
              }))
            );
          }))
        );
      } else return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "noPackages"
        }, localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("NoPackages"))
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

  return PackagesList;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

PackagesList.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  importPackages: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  selectedPackage: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  selectPackage: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func
};

function mapStateToProps(state) {
  return {
    importPackages: state.importExport.importPackages,
    selectedPackage: state.importExport.selectedPackage
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(PackagesList);

var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(PackagesList, "PackagesList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(PackagesList, "PackagesList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackagesList.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 92 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(1);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}





var PackageCardOverlay =
/*#__PURE__*/
function (_Component) {
  _inherits(PackageCardOverlay, _Component);

  function PackageCardOverlay() {
    _classCallCheck(this, PackageCardOverlay);

    return _possibleConstructorReturn(this, _getPrototypeOf(PackageCardOverlay).apply(this, arguments));
  }

  _createClass(PackageCardOverlay, [{
    key: "render",

    /* eslint-disable react/no-danger */
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "package-card-overlay",
          onClick: props.selectPackage
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "icon-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, props.isSelected ? localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("ClicktoDeselect") : localization__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].get("ClicktoSelect"))))
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

  return PackageCardOverlay;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

PackageCardOverlay.propTypes = {
  packageName: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  packageDescription: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  selectPackage: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  isSelected: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};
var _default = PackageCardOverlay;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(PackageCardOverlay, "PackageCardOverlay", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(PackageCardOverlay, "PackageCardOverlay", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\PackageCardOverlay.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 93 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(114);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(2);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}







var FiltersBar =
/*#__PURE__*/
function (_Component) {
  _inherits(FiltersBar, _Component);

  function FiltersBar(props) {
    var _this;

    _classCallCheck(this, FiltersBar);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(FiltersBar).call(this, props));
    _this.state = {
      selectedJobFilter: {
        label: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SortByDateNewest"),
        value: "newest"
      },
      searchText: ""
    };
    return _this;
  }

  _createClass(FiltersBar, [{
    key: "onSelect",
    value: function onSelect(option) {
      var _this2 = this;

      var selectedJobFilter = this.state.selectedJobFilter;

      if (option.value !== selectedJobFilter.value) {
        selectedJobFilter.label = option.label;
        selectedJobFilter.value = option.value;
        this.setState({
          selectedJobFilter: selectedJobFilter
        }, function () {
          _this2.props.onFilterChanged(option);
        });
      }
    }
  }, {
    key: "BuildFiltersOptions",
    value: function BuildFiltersOptions() {
      var jobFilters = [{
        "Key": localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SortByDateNewest"),
        "Value": "newest"
      }, {
        "Key": localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SortByDateOldest"),
        "Value": "oldest"
      }, {
        "Key": localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SortByName"),
        "Value": "name"
      }];
      var jobFiltersOptions = [];
      jobFiltersOptions = jobFilters.map(function (jobFilters) {
        return {
          label: jobFilters.Key,
          value: jobFilters.Value
        };
      });
      return jobFiltersOptions;
    }
  }, {
    key: "render",
    value: function render() {
      var jobFiltersOptions = this.BuildFiltersOptions();
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "jobs-filter-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 35
        }, jobFiltersOptions.length > 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "job-filters-filter"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
          style: {
            width: "100%"
          },
          withBorder: false,
          options: jobFiltersOptions,
          onSelect: this.onSelect.bind(this),
          value: this.state.selectedJobFilter.value,
          prependWith: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("ShowSortLabel")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "clear"
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 30
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, "\xA0 ")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 35
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "search-filter"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SearchBox"], {
          placeholder: localization__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SearchPlaceHolder"),
          onSearch: this.props.onKeywordChanged,
          maxLength: 50,
          iconStyle: {
            right: 0
          }
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "clear"
        }))))
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

  return FiltersBar;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

FiltersBar.propTypes = {
  onFilterChanged: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  onKeywordChanged: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired
};
var _default = FiltersBar;
var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(FiltersBar, "FiltersBar", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(FiltersBar, "FiltersBar", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\FiltersBar\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 94 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(4);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(5);
/* harmony import */ var rc_progress__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(96);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(153);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}








var ProgressBar =
/*#__PURE__*/
function (_Component) {
  _inherits(ProgressBar, _Component);

  function ProgressBar() {
    var _this;

    _classCallCheck(this, ProgressBar);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(ProgressBar).call(this));
    _this.timeout = "";
    _this.started = false;
    _this.state = {
      percent: 0
    };
    return _this;
  }

  _createClass(ProgressBar, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      this.started = true;
      this.increase();
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      clearTimeout(this.timeout);
      this.setState({
        percent: 0
      });
    }
  }, {
    key: "componentDidUpdate",
    value: function componentDidUpdate() {
      var props = this.props;

      if (props.visible !== undefined) {
        if (!props.visible && this.started) {
          this.started = false;
          this.setState({
            percent: 0
          });
          clearTimeout(this.timeout);
        }

        if (props.visible && !this.started) {
          this.started = true;
          this.setState({
            percent: 0
          });
          this.increase();
        }
      }
    }
  }, {
    key: "increase",
    value: function increase() {
      var _this2 = this;

      if (this.state.percent > 95) {
        //clearTimeout(this.timeout);
        this.setState({
          percent: 0
        });
      } else {
        if (this.props.loaded) {
          this.setState({
            percent: 100
          }, function () {
            _this2.timeout = setTimeout(function () {
              _this2.props.dispatch(actions__WEBPACK_IMPORTED_MODULE_3__[/* importExport */ "a"].packageVerified(true));
            }, 100);
          });
        } else {
          this.setState({
            percent: this.state.percent + 5
          });
        }
      }

      this.timeout = setTimeout(function () {
        _this2.increase();
      }, 200);
    }
  }, {
    key: "render",
    value: function render() {
      var visibility = this.props.visible ? "block" : "none";
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          id: "progressContainer",
          className: this.props.className,
          style: {
            display: visibility
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(rc_progress__WEBPACK_IMPORTED_MODULE_4__[/* Line */ "a"], {
          percent: this.state.percent,
          strokeWidth: 5,
          trailWidth: 5,
          strokeLinecap: "square",
          trailColor: "#eff0f0",
          strokeColor: "#9fdbf0"
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

  return ProgressBar;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

ProgressBar.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  visible: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  className: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  loaded: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};

function mapStateToProps() {
  return {};
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(ProgressBar);

var _default2 = _default;
/* harmony default export */ __webpack_exports__["a"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(ProgressBar, "ProgressBar", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(ProgressBar, "ProgressBar", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\components\\ImportModal\\ProgressBar\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 95 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(155);

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
/* 96 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";

// EXTERNAL MODULE: D:/a/1/s/node_modules/babel-runtime/helpers/extends.js
var helpers_extends = __webpack_require__(27);
var extends_default = /*#__PURE__*/__webpack_require__.n(helpers_extends);

// EXTERNAL MODULE: D:/a/1/s/node_modules/babel-runtime/helpers/objectWithoutProperties.js
var objectWithoutProperties = __webpack_require__(37);
var objectWithoutProperties_default = /*#__PURE__*/__webpack_require__.n(objectWithoutProperties);

// EXTERNAL MODULE: D:/a/1/s/node_modules/babel-runtime/helpers/classCallCheck.js
var classCallCheck = __webpack_require__(25);
var classCallCheck_default = /*#__PURE__*/__webpack_require__.n(classCallCheck);

// EXTERNAL MODULE: D:/a/1/s/node_modules/babel-runtime/helpers/possibleConstructorReturn.js
var possibleConstructorReturn = __webpack_require__(15);
var possibleConstructorReturn_default = /*#__PURE__*/__webpack_require__.n(possibleConstructorReturn);

// EXTERNAL MODULE: D:/a/1/s/node_modules/babel-runtime/helpers/inherits.js
var inherits = __webpack_require__(26);
var inherits_default = /*#__PURE__*/__webpack_require__.n(inherits);

// EXTERNAL MODULE: external "window.dnn.nodeModules.React"
var external_window_dnn_nodeModules_React_ = __webpack_require__(0);
var external_window_dnn_nodeModules_React_default = /*#__PURE__*/__webpack_require__.n(external_window_dnn_nodeModules_React_);

// CONCATENATED MODULE: D:/a/1/s/node_modules/rc-progress/es/enhancer.js



var enhancer_enhancer = function enhancer(WrappedComponent) {
  return function (_WrappedComponent) {
    inherits_default()(Progress, _WrappedComponent);

    function Progress() {
      classCallCheck_default()(this, Progress);

      return possibleConstructorReturn_default()(this, _WrappedComponent.apply(this, arguments));
    }

    Progress.prototype.componentDidUpdate = function componentDidUpdate() {
      var _this2 = this;

      var now = Date.now();
      var updated = false;

      Object.keys(this.paths).forEach(function (key) {
        var path = _this2.paths[key];

        if (!path) {
          return;
        }

        updated = true;
        var pathStyle = path.style;
        pathStyle.transitionDuration = '.3s, .3s, .3s, .06s';

        if (_this2.prevTimeStamp && now - _this2.prevTimeStamp < 100) {
          pathStyle.transitionDuration = '0s, 0s';
        }
      });

      if (updated) {
        this.prevTimeStamp = Date.now();
      }
    };

    Progress.prototype.render = function render() {
      return _WrappedComponent.prototype.render.call(this);
    };

    return Progress;
  }(WrappedComponent);
};

/* harmony default export */ var es_enhancer = (enhancer_enhancer);
// EXTERNAL MODULE: D:/a/1/s/node_modules/prop-types/index.js
var prop_types = __webpack_require__(4);
var prop_types_default = /*#__PURE__*/__webpack_require__.n(prop_types);

// CONCATENATED MODULE: D:/a/1/s/node_modules/rc-progress/es/types.js


var defaultProps = {
  className: '',
  percent: 0,
  prefixCls: 'rc-progress',
  strokeColor: '#2db7f5',
  strokeLinecap: 'round',
  strokeWidth: 1,
  style: {},
  trailColor: '#D9D9D9',
  trailWidth: 1
};

var mixedType = prop_types_default.a.oneOfType([prop_types_default.a.number, prop_types_default.a.string]);

var propTypes = {
  className: prop_types_default.a.string,
  percent: prop_types_default.a.oneOfType([mixedType, prop_types_default.a.arrayOf(mixedType)]),
  prefixCls: prop_types_default.a.string,
  strokeColor: prop_types_default.a.oneOfType([prop_types_default.a.string, prop_types_default.a.arrayOf(prop_types_default.a.string)]),
  strokeLinecap: prop_types_default.a.oneOf(['butt', 'round', 'square']),
  strokeWidth: mixedType,
  style: prop_types_default.a.object,
  trailColor: prop_types_default.a.string,
  trailWidth: mixedType
};
// CONCATENATED MODULE: D:/a/1/s/node_modules/rc-progress/es/Line.js









var Line_Line = function (_Component) {
  inherits_default()(Line, _Component);

  function Line() {
    var _temp, _this, _ret;

    classCallCheck_default()(this, Line);

    for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    return _ret = (_temp = (_this = possibleConstructorReturn_default()(this, _Component.call.apply(_Component, [this].concat(args))), _this), _this.paths = {}, _temp), possibleConstructorReturn_default()(_this, _ret);
  }

  Line.prototype.render = function render() {
    var _this2 = this;

    var _props = this.props,
        className = _props.className,
        percent = _props.percent,
        prefixCls = _props.prefixCls,
        strokeColor = _props.strokeColor,
        strokeLinecap = _props.strokeLinecap,
        strokeWidth = _props.strokeWidth,
        style = _props.style,
        trailColor = _props.trailColor,
        trailWidth = _props.trailWidth,
        restProps = objectWithoutProperties_default()(_props, ['className', 'percent', 'prefixCls', 'strokeColor', 'strokeLinecap', 'strokeWidth', 'style', 'trailColor', 'trailWidth']);

    delete restProps.gapPosition;

    var percentList = Array.isArray(percent) ? percent : [percent];
    var strokeColorList = Array.isArray(strokeColor) ? strokeColor : [strokeColor];

    var center = strokeWidth / 2;
    var right = 100 - strokeWidth / 2;
    var pathString = 'M ' + (strokeLinecap === 'round' ? center : 0) + ',' + center + '\n           L ' + (strokeLinecap === 'round' ? right : 100) + ',' + center;
    var viewBoxString = '0 0 100 ' + strokeWidth;

    var stackPtg = 0;

    return external_window_dnn_nodeModules_React_default.a.createElement(
      'svg',
      extends_default()({
        className: prefixCls + '-line ' + className,
        viewBox: viewBoxString,
        preserveAspectRatio: 'none',
        style: style
      }, restProps),
      external_window_dnn_nodeModules_React_default.a.createElement('path', {
        className: prefixCls + '-line-trail',
        d: pathString,
        strokeLinecap: strokeLinecap,
        stroke: trailColor,
        strokeWidth: trailWidth || strokeWidth,
        fillOpacity: '0'
      }),
      percentList.map(function (ptg, index) {
        var pathStyle = {
          strokeDasharray: ptg + 'px, 100px',
          strokeDashoffset: '-' + stackPtg + 'px',
          transition: 'stroke-dashoffset 0.3s ease 0s, stroke-dasharray .3s ease 0s, stroke 0.3s linear'
        };
        var color = strokeColorList[index] || strokeColorList[strokeColorList.length - 1];

        stackPtg += ptg;

        return external_window_dnn_nodeModules_React_default.a.createElement('path', {
          key: index,
          className: prefixCls + '-line-path',
          d: pathString,
          strokeLinecap: strokeLinecap,
          stroke: color,
          strokeWidth: strokeWidth,
          fillOpacity: '0',
          ref: function ref(path) {
            _this2.paths[index] = path;
          },
          style: pathStyle
        });
      })
    );
  };

  return Line;
}(external_window_dnn_nodeModules_React_["Component"]);

Line_Line.propTypes = propTypes;

Line_Line.defaultProps = defaultProps;

/* harmony default export */ var es_Line = (es_enhancer(Line_Line));
// CONCATENATED MODULE: D:/a/1/s/node_modules/rc-progress/es/Circle.js





/* eslint react/prop-types: 0 */





var Circle_Circle = function (_Component) {
  inherits_default()(Circle, _Component);

  function Circle() {
    var _temp, _this, _ret;

    classCallCheck_default()(this, Circle);

    for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    return _ret = (_temp = (_this = possibleConstructorReturn_default()(this, _Component.call.apply(_Component, [this].concat(args))), _this), _this.paths = {}, _temp), possibleConstructorReturn_default()(_this, _ret);
  }

  Circle.prototype.getPathStyles = function getPathStyles(offset, percent, strokeColor, strokeWidth) {
    var gapDegree = arguments.length > 4 && arguments[4] !== undefined ? arguments[4] : 0;
    var gapPosition = arguments[5];

    var radius = 50 - strokeWidth / 2;
    var beginPositionX = 0;
    var beginPositionY = -radius;
    var endPositionX = 0;
    var endPositionY = -2 * radius;
    switch (gapPosition) {
      case 'left':
        beginPositionX = -radius;
        beginPositionY = 0;
        endPositionX = 2 * radius;
        endPositionY = 0;
        break;
      case 'right':
        beginPositionX = radius;
        beginPositionY = 0;
        endPositionX = -2 * radius;
        endPositionY = 0;
        break;
      case 'bottom':
        beginPositionY = radius;
        endPositionY = 2 * radius;
        break;
      default:
    }
    var pathString = 'M 50,50 m ' + beginPositionX + ',' + beginPositionY + '\n     a ' + radius + ',' + radius + ' 0 1 1 ' + endPositionX + ',' + -endPositionY + '\n     a ' + radius + ',' + radius + ' 0 1 1 ' + -endPositionX + ',' + endPositionY;
    var len = Math.PI * 2 * radius;

    var pathStyle = {
      stroke: strokeColor,
      strokeDasharray: percent / 100 * (len - gapDegree) + 'px ' + len + 'px',
      strokeDashoffset: '-' + (gapDegree / 2 + offset / 100 * (len - gapDegree)) + 'px',
      transition: 'stroke-dashoffset .3s ease 0s, stroke-dasharray .3s ease 0s, stroke .3s, stroke-width .06s ease .3s' // eslint-disable-line
    };

    return {
      pathString: pathString,
      pathStyle: pathStyle
    };
  };

  Circle.prototype.getStokeList = function getStokeList() {
    var _this2 = this;

    var _props = this.props,
        prefixCls = _props.prefixCls,
        percent = _props.percent,
        strokeColor = _props.strokeColor,
        strokeWidth = _props.strokeWidth,
        strokeLinecap = _props.strokeLinecap,
        gapDegree = _props.gapDegree,
        gapPosition = _props.gapPosition;

    var percentList = Array.isArray(percent) ? percent : [percent];
    var strokeColorList = Array.isArray(strokeColor) ? strokeColor : [strokeColor];

    var stackPtg = 0;
    return percentList.map(function (ptg, index) {
      var color = strokeColorList[index] || strokeColorList[strokeColorList.length - 1];

      var _getPathStyles = _this2.getPathStyles(stackPtg, ptg, color, strokeWidth, gapDegree, gapPosition),
          pathString = _getPathStyles.pathString,
          pathStyle = _getPathStyles.pathStyle;

      stackPtg += ptg;

      return external_window_dnn_nodeModules_React_default.a.createElement('path', {
        key: index,
        className: prefixCls + '-circle-path',
        d: pathString,
        strokeLinecap: strokeLinecap,
        strokeWidth: ptg === 0 ? 0 : strokeWidth,
        fillOpacity: '0',
        style: pathStyle,
        ref: function ref(path) {
          _this2.paths[index] = path;
        }
      });
    });
  };

  Circle.prototype.render = function render() {
    var _props2 = this.props,
        prefixCls = _props2.prefixCls,
        strokeWidth = _props2.strokeWidth,
        trailWidth = _props2.trailWidth,
        gapDegree = _props2.gapDegree,
        gapPosition = _props2.gapPosition,
        trailColor = _props2.trailColor,
        strokeLinecap = _props2.strokeLinecap,
        style = _props2.style,
        className = _props2.className,
        restProps = objectWithoutProperties_default()(_props2, ['prefixCls', 'strokeWidth', 'trailWidth', 'gapDegree', 'gapPosition', 'trailColor', 'strokeLinecap', 'style', 'className']);

    var _getPathStyles2 = this.getPathStyles(0, 100, trailColor, strokeWidth, gapDegree, gapPosition),
        pathString = _getPathStyles2.pathString,
        pathStyle = _getPathStyles2.pathStyle;

    delete restProps.percent;
    delete restProps.strokeColor;
    return external_window_dnn_nodeModules_React_default.a.createElement(
      'svg',
      extends_default()({
        className: prefixCls + '-circle ' + className,
        viewBox: '0 0 100 100',
        style: style
      }, restProps),
      external_window_dnn_nodeModules_React_default.a.createElement('path', {
        className: prefixCls + '-circle-trail',
        d: pathString,
        stroke: trailColor,
        strokeLinecap: strokeLinecap,
        strokeWidth: trailWidth || strokeWidth,
        fillOpacity: '0',
        style: pathStyle
      }),
      this.getStokeList()
    );
  };

  return Circle;
}(external_window_dnn_nodeModules_React_["Component"]);

Circle_Circle.propTypes = extends_default()({}, propTypes, {
  gapPosition: prop_types_default.a.oneOf(['top', 'bottom', 'left', 'right'])
});

Circle_Circle.defaultProps = extends_default()({}, defaultProps, {
  gapPosition: 'top'
});

/* harmony default export */ var es_Circle = (es_enhancer(Circle_Circle));
// CONCATENATED MODULE: D:/a/1/s/node_modules/rc-progress/es/index.js
/* concated harmony reexport Line */__webpack_require__.d(__webpack_exports__, "a", function() { return es_Line; });
/* unused concated harmony import Circle */





/* harmony default export */ var es = ({
  Line: es_Line,
  Circle: es_Circle
});

/***/ }),
/* 97 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(70);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _globals_application__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(50);
/* harmony import */ var _store_configureStore__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(71);
/* harmony import */ var _services_itemsToExportService__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(13);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(81);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_containers_Root__WEBPACK_IMPORTED_MODULE_6__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();








var store = Object(_store_configureStore__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"])();
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].dispatch = store.dispatch;
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].init();

if (!window.dnn.siteImportExport) {
  window.dnn.siteImportExport = {};
}

window.dnn.siteImportExport.registerItemToExport = _services_itemsToExportService__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].registerItemToExport;
var appContainer = document.getElementById("siteimportexport-container");
Object(react_dom__WEBPACK_IMPORTED_MODULE_1__["render"])(
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_redux__WEBPACK_IMPORTED_MODULE_2__["Provider"], {
  store: store
},
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_containers_Root__WEBPACK_IMPORTED_MODULE_6___default.a, null)), appContainer);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(store, "store", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\main.jsx");
  reactHotLoader.register(appContainer, "appContainer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\main.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(store, "store", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\main.jsx");
  reactHotLoader.register(appContainer, "appContainer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\main.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 98 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
function _interopDefault(e){return e&&"object"==typeof e&&"default"in e?e.default:e}Object.defineProperty(exports,"__esModule",{value:!0});var React=_interopDefault(__webpack_require__(0));function AppContainer(e){return React.Children.only(e.children)}var hot_prod=function(){return function(e){return e}},areComponentsEqual=function(e,n){return e===n},setConfig=function(){},cold=function(e){return e},configureComponent=function(){};exports.AppContainer=AppContainer,exports.hot=hot_prod,exports.areComponentsEqual=areComponentsEqual,exports.setConfig=setConfig,exports.cold=cold,exports.configureComponent=configureComponent;


/***/ }),
/* 99 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(100);

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
/* 100 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.siteImportExport-app .dnn-persona-bar-page.hidden {\n  display: none;\n}\n", ""]);



/***/ }),
/* 101 */
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
/* 102 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _components_App__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(82);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof2(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof2 = function _typeof2(obj) { return typeof obj; }; } else { _typeof2 = function _typeof2(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof2(obj); }

(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) {
  if (typeof Symbol === "function" && _typeof2(Symbol.iterator) === "symbol") {
    _typeof = function _typeof(obj) {
      return _typeof2(obj);
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : _typeof2(obj);
    };
  }

  return _typeof(obj);
}

function _classCallCheck(instance, Constructor) {
  if (!(instance instanceof Constructor)) {
    throw new TypeError("Cannot call a class as a function");
  }
}

function _defineProperties(target, props) {
  for (var i = 0; i < props.length; i++) {
    var descriptor = props[i];
    descriptor.enumerable = descriptor.enumerable || false;
    descriptor.configurable = true;
    if ("value" in descriptor) descriptor.writable = true;
    Object.defineProperty(target, descriptor.key, descriptor);
  }
}

function _createClass(Constructor, protoProps, staticProps) {
  if (protoProps) _defineProperties(Constructor.prototype, protoProps);
  if (staticProps) _defineProperties(Constructor, staticProps);
  return Constructor;
}

function _possibleConstructorReturn(self, call) {
  if (call && (_typeof(call) === "object" || typeof call === "function")) {
    return call;
  }

  return _assertThisInitialized(self);
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}

function _getPrototypeOf(o) {
  _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) {
    return o.__proto__ || Object.getPrototypeOf(o);
  };
  return _getPrototypeOf(o);
}

function _inherits(subClass, superClass) {
  if (typeof superClass !== "function" && superClass !== null) {
    throw new TypeError("Super expression must either be null or a function");
  }

  subClass.prototype = Object.create(superClass && superClass.prototype, {
    constructor: {
      value: subClass,
      writable: true,
      configurable: true
    }
  });
  if (superClass) _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}




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
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "siteImportExport-app personaBar-mainContainer"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_components_App__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"], null))
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
var _default2 = _default;
/* harmony default export */ __webpack_exports__["default"] = (_default2);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Root, "Root", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();

;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_typeof, "_typeof", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_classCallCheck, "_classCallCheck", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_defineProperties, "_defineProperties", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_createClass, "_createClass", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_possibleConstructorReturn, "_possibleConstructorReturn", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_assertThisInitialized, "_assertThisInitialized", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_getPrototypeOf, "_getPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_inherits, "_inherits", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_setPrototypeOf, "_setPrototypeOf", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(Root, "Root", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_default, "_default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_default2, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\SiteImportExport.Web\\src\\containers\\Root.prod.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 103 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(104);

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
/* 104 */
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
/* 105 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(106);

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
/* 106 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#siteimportexport-container .dnn-persona-bar-page-body {\n  border-bottom: 0;\n}\n#siteimportexport-container .dnn-persona-bar-page-body * {\n  box-sizing: border-box;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body {\n  border: 0;\n  background: none;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel {\n  width: 100%;\n  float: left;\n  border: 1px solid #C8C8C8;\n  background: #fff;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .site-selection {\n  padding: 20px;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .site-selection .dnn-dropdown {\n  width: 100%;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .site-selection .dnn-dropdown .collapsible-label {\n  text-transform: uppercase;\n  font-weight: bold;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .last-actions {\n  width: 50%;\n  float: left;\n  padding: 0 20px 20px 20px;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .last-actions .action-labels {\n  float: left;\n  width: 50%;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .last-actions .action-labels .dnn-label {\n  padding: 0 0 10px 0;\n  font-weight: 700;\n  color: #4B4E4F;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .last-actions .action-dates {\n  float: right;\n  width: 50%;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .last-actions .action-dates div {\n  color: #4B4E4F;\n  padding: 0 0 10px 0;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .action-buttons {\n  width: 50%;\n  float: right;\n  padding: 0 30px 0 20px;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .top-panel .action-buttons .action-button {\n  margin-left: 20px;\n  float: right;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .section-title {\n  text-transform: uppercase;\n  font-weight: bold;\n  float: left;\n  width: 100%;\n  padding: 20px 0 10px 0;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer {\n  float: left;\n  width: 100%;\n  border: 1px solid #C8C8C8;\n  background: #fff;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .no-jobs {\n  float: left;\n  width: 100%;\n  text-align: center;\n  padding: 10px;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .jobHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .jobHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .jobHeader-wrapper .jobHeader-Indicator {\n  width: 5%;\n  float: left;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .jobHeader-wrapper .jobHeader-JobDate {\n  width: 20%;\n  float: left;\n  font-weight: bolder;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .jobHeader-wrapper .jobHeader-JobType {\n  width: 15%;\n  float: left;\n  font-weight: bolder;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .jobHeader-wrapper .jobHeader-JobUser {\n  width: 20%;\n  float: left;\n  font-weight: bolder;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .jobHeader-wrapper .jobHeader-JobPortal {\n  width: 20%;\n  float: left;\n  font-weight: bolder;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .jobHeader-wrapper .jobHeader-JobStatus {\n  width: 15%;\n  float: left;\n  font-weight: bolder;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logContainer .jobHeader-wrapper .jobHeader-Arrow {\n  width: 5%;\n  float: left;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logPager {\n  padding: 20px 0;\n  float: left;\n  width: 100%;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logLegend-wrapper {\n  float: left;\n  border-top: solid 1px #C8C8C8;\n  width: 100%;\n  padding: 20px 0;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logLegend-wrapper .logLegend-item {\n  float: left;\n  width: 200px;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logLegend-wrapper .logLegend-item .legend-export {\n  color: #fff;\n  background-color: #ff8a14;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0 6px 0 0;\n}\n#siteimportexport-container .dnn-persona-bar-page-body .persona-bar-page-body .logLegend-wrapper .logLegend-item .legend-import {\n  color: #fff;\n  background-color: #099;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0 6px 0 0;\n}\n", ""]);



/***/ }),
/* 107 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(108);

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
/* 108 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.collapsible-jobdetail {\n  display: table;\n  width: 100%;\n  float: left;\n}\n.collapsible-jobdetail.false {\n  border-top: 2px solid #1E88C3 !important;\n  border-bottom: 2px solid #1E88C3 !important;\n  position: relative;\n  margin-top: -1px;\n  margin-bottom: -1px;\n}\n.collapsible-jobdetail:not(:last-child) {\n  border-bottom: 1px solid #ddd;\n}\n.collapsible-jobdetail .job-collapsible > div {\n  float: left;\n  padding-bottom: 25px;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header {\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 0 15px 0;\n  box-sizing: border-box;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header {\n  cursor: pointer;\n  float: left;\n  width: 100%;\n  height: 18px;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-cssclass {\n  width: 5%;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-cssclass .jobIndicator-export {\n  color: #fff;\n  background-color: #ff8a14;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0 0 0 12px;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-cssclass .jobIndicator-import {\n  color: #fff;\n  background-color: #099;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0 0 0 12px;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-createdate {\n  width: 20%;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-jobtype {\n  width: 15%;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-username {\n  width: 20%;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-portalname {\n  width: 20%;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-jobstatus {\n  width: 15%;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-jobstatus .job-status0 {\n  color: #ff8a14;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-jobstatus .job-status1 {\n  color: #78C16B;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-jobstatus .job-status2 {\n  color: #1E88C3;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-jobstatus .job-status3 {\n  color: #EA2134;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-jobstatus .job-status4 {\n  color: #C8C8C8;\n  float: left;\n}\n@-ms-keyframes spin {\n  from {\n    -ms-transform: rotate(360deg);\n  }\n  to {\n    -ms-transform: rotate(0deg);\n  }\n}\n@-moz-keyframes spin {\n  from {\n    -moz-transform: rotate(360deg);\n  }\n  to {\n    -moz-transform: rotate(0deg);\n  }\n}\n@-webkit-keyframes spin {\n  from {\n    -webkit-transform: rotate(360deg);\n  }\n  to {\n    -webkit-transform: rotate(0deg);\n  }\n}\n@keyframes spin {\n  from {\n    transform: rotate(360deg);\n  }\n  to {\n    transform: rotate(0deg);\n  }\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-jobstatus .cycle-icon {\n  width: 24px;\n  height: 24px;\n  float: right;\n  margin-top: -5px;\n  -webkit-animation-name: spin;\n  -webkit-animation-duration: 4000ms;\n  -webkit-animation-iteration-count: infinite;\n  -webkit-animation-timing-function: linear;\n  -moz-animation-name: spin;\n  -moz-animation-duration: 4000ms;\n  -moz-animation-iteration-count: infinite;\n  -moz-animation-timing-function: linear;\n  -ms-animation-name: spin;\n  -ms-animation-duration: 4000ms;\n  -ms-animation-iteration-count: infinite;\n  -ms-animation-timing-function: linear;\n  animation-name: spin;\n  animation-duration: 4000ms;\n  animation-iteration-count: infinite;\n  animation-timing-function: linear;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-jobstatus .cycle-icon > svg {\n  fill: #78C16B;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-arrow {\n  width: 5%;\n  float: left;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-arrow .arrow-icon svg {\n  width: 16px;\n  height: 16px;\n  float: left;\n  fill: #4B4E4F;\n  margin-right: 5px;\n}\n.collapsible-jobdetail div.collapsible-jobdetail-header .term-header .term-label-wrapper {\n  word-wrap: break-word;\n}\n", ""]);



/***/ }),
/* 109 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(110);

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
/* 110 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.jobs-filter-container {\n  float: left;\n  box-sizing: border-box;\n  margin-bottom: 15px;\n  width: 100%;\n  border: none;\n  border-bottom: 1px solid #C8C8C8;\n  padding-bottom: 5px;\n}\n.jobs-filter-container .dnn-grid-cell {\n  margin-bottom: 5px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter {\n  width: 100%;\n  border-right: 1px solid #C8C8C8;\n  font-size: 13px;\n  font-family: inherit;\n  float: left;\n  height: 32px;\n  padding-top: 4px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-dropdown .collapsible-label {\n  padding-top: 4px;\n  padding-left: 0;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-dropdown .dropdown-icon {\n  top: 5px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-search-box {\n  width: 100%;\n  height: 24px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-search-box input {\n  padding: 0px 16px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-dropdown .collapsible-toggle {\n  width: auto;\n}\n.jobs-filter-container .dnn-grid-cell .search-filter {\n  float: left;\n  width: 100%;\n}\n.jobs-filter-container .dnn-grid-cell .search-filter > div {\n  display: block !important;\n  border-left: 1px solid #C8C8C8;\n}\n.jobs-filter-container .dnn-grid-cell .search-filter > div input {\n  display: block;\n  width: 100%;\n  position: inherit !important;\n  border: none;\n  border-radius: none;\n  background-color: transparent;\n  outline: none;\n  padding-right: 45px;\n}\n", ""]);



/***/ }),
/* 111 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(112);

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
/* 112 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.job-details {\n  margin: 10px 0;\n  float: left;\n  width: 100%;\n}\n.job-details .summary-title {\n  width: 100%;\n  float: left;\n  font-weight: bold;\n  text-transform: uppercase;\n  padding-left: 4%;\n}\n.job-details .export-summary {\n  float: left;\n  width: 100%;\n  padding: 0 30px 20px 30px;\n}\n.job-details .export-summary .export-site-container {\n  font-weight: 700;\n}\n.job-details .export-summary .export-site-container .left-column {\n  width: 50%;\n  float: left;\n  border-right: 1px solid #C8C8C8;\n  padding: 10px 20px 10px 0;\n}\n.job-details .export-summary .export-site-container .right-column {\n  width: 50%;\n  float: left;\n  padding: 10px 0 10px 30px;\n}\n.job-details .export-summary .export-site-container .right-column .dnn-switch-container {\n  float: right;\n}\n.job-details .export-summary .export-site-container .right-column .summary-note {\n  float: left;\n  width: 100%;\n  color: #4B4E4F;\n  font-weight: 500;\n  padding-top: 30px;\n  padding-right: 29px;\n}\n.job-details .export-summary .export-site-container .right-column .summary-note .note-title {\n  float: left;\n  width: 15%;\n}\n.job-details .export-summary .export-site-container .right-column .summary-note .note-description {\n  float: left;\n  width: 85%;\n}\n.job-details .export-summary .export-site-container .import-summary-item {\n  padding-top: 7px;\n  float: right;\n  color: #4B4E4F;\n  width: 130px;\n  text-align: right;\n  margin-right: 29px;\n  font-weight: 100;\n}\n.job-details .export-summary .export-site-container .import-summary-item.users,\n.job-details .export-summary .export-site-container .import-summary-item.pagers {\n  width: 180px;\n}\n.job-details .export-summary .export-site-container .import-summary-item > div {\n  margin-right: -29px;\n}\n@-ms-keyframes spin {\n  from {\n    -ms-transform: rotate(360deg);\n  }\n  to {\n    -ms-transform: rotate(0deg);\n  }\n}\n@-moz-keyframes spin {\n  from {\n    -moz-transform: rotate(360deg);\n  }\n  to {\n    -moz-transform: rotate(0deg);\n  }\n}\n@-webkit-keyframes spin {\n  from {\n    -webkit-transform: rotate(360deg);\n  }\n  to {\n    -webkit-transform: rotate(0deg);\n  }\n}\n@keyframes spin {\n  from {\n    transform: rotate(360deg);\n  }\n  to {\n    transform: rotate(0deg);\n  }\n}\n.job-details .export-summary .export-site-container .import-summary-item .cycle-icon {\n  width: 24px;\n  height: 24px;\n  float: right;\n  margin-top: -5px;\n  margin-left: 5px;\n  -webkit-animation-name: spin;\n  -webkit-animation-duration: 4000ms;\n  -webkit-animation-iteration-count: infinite;\n  -webkit-animation-timing-function: linear;\n  -moz-animation-name: spin;\n  -moz-animation-duration: 4000ms;\n  -moz-animation-iteration-count: infinite;\n  -moz-animation-timing-function: linear;\n  -ms-animation-name: spin;\n  -ms-animation-duration: 4000ms;\n  -ms-animation-iteration-count: infinite;\n  -ms-animation-timing-function: linear;\n  animation-name: spin;\n  animation-duration: 4000ms;\n  animation-iteration-count: infinite;\n  animation-timing-function: linear;\n}\n.job-details .export-summary .export-site-container .import-summary-item .cycle-icon > svg {\n  fill: #78C16B;\n}\n.job-details .export-summary .export-site-container .dnn-grid-cell {\n  padding-bottom: 10px;\n}\n.job-details .export-summary .export-site-container .action-buttons {\n  text-align: center;\n  padding: 30px 0 0 0;\n}\n.job-details .export-summary .export-site-container .action-buttons button {\n  margin: 5px;\n}\n", ""]);



/***/ }),
/* 113 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.gQtuCxogEVtjJbm4r0t_I {\n  border: 1px solid #C8C8C8;\n  background: #fff;\n  float: left;\n}\n.gQtuCxogEVtjJbm4r0t_I * {\n  box-sizing: border-box;\n}\n.gQtuCxogEVtjJbm4r0t_I .sectionTitle {\n  font-weight: bold;\n  margin: 20px 0;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .left-column {\n  width: 50%;\n  float: left;\n  padding: 10px 30px 10px 30px;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .left-column .dnn-dropdown-with-error {\n  width: 100%;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .left-column .dnn-single-line-input-with-error {\n  padding-top: 15px;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .left-column .dnn-label {\n  margin-bottom: 5px;\n  font-weight: 700;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .right-column {\n  width: 50%;\n  float: left;\n  padding: 10px 30px 10px 30px;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-switches {\n  padding: 0 30px 0 30px;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-switches .dnn-ui-common-input-group {\n  padding: 5px 0;\n  float: left;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-switches .dnn-ui-common-input-group .dnn-label {\n  font-weight: bolder;\n  float: left;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-switches .dnn-ui-common-input-group .dnn-switch-container {\n  float: right;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages {\n  padding: 0 30px 20px 30px;\n  border-left: 1px solid #C8C8C8;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .export-page-picker {\n  width: 100% !important;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .export-page-picker .page-picker-content > div {\n  height: 395px !important;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .language-box {\n  float: left;\n  width: 100%;\n  height: 70px;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .language-box .dnn-dropdown-with-error {\n  width: 100%;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .dnn-checkbox-container {\n  float: left;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .dnn-checkbox-container .checkbox {\n  margin-bottom: 2px;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .dnn-label {\n  float: left;\n  font-weight: 700;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .dnn-label label {\n  float: left;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .dnn-dropdown .collapsible-content ul li {\n  display: -webkit-box;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .dnn-dropdown .collapsible-content ul li .dnn-ui-common-tooltip {\n  float: left;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages div.tooltip-text {\n  max-width: 250px;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .dnn-radio-buttons {\n  float: right;\n  width: auto;\n  margin-top: 6px;\n}\n.gQtuCxogEVtjJbm4r0t_I .export-site-container .export-pages .lastExport {\n  margin-top: 6px;\n  font-weight: 500;\n  color: #4B4E4F;\n  float: right;\n}\n.gQtuCxogEVtjJbm4r0t_I .seperator {\n  padding: 0 20px;\n}\n.gQtuCxogEVtjJbm4r0t_I .seperator2 {\n  padding: 30px 0 20px 0;\n}\n.gQtuCxogEVtjJbm4r0t_I .pageTitle {\n  text-transform: uppercase;\n  font-weight: bold;\n  padding: 20px 0 10px 30px;\n}\n.gQtuCxogEVtjJbm4r0t_I .sectionTitle {\n  text-transform: uppercase;\n  font-weight: bold;\n}\n.gQtuCxogEVtjJbm4r0t_I .action-buttons {\n  text-align: center;\n  padding: 30px 0;\n}\n.gQtuCxogEVtjJbm4r0t_I .action-buttons button {\n  margin: 5px;\n}\n", ""]);

// Exports
exports.locals = {
	"exportModal": "gQtuCxogEVtjJbm4r0t_I"
};

/***/ }),
/* 114 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(115);

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
/* 115 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.jobs-filter-container {\n  float: left;\n  box-sizing: border-box;\n  margin-bottom: 15px;\n  width: 100%;\n  border: none;\n  border-bottom: 1px solid #C8C8C8;\n  padding-bottom: 5px;\n}\n.jobs-filter-container .dnn-grid-cell {\n  margin-bottom: 5px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter {\n  width: 100%;\n  border-right: 1px solid #C8C8C8;\n  font-size: 13px;\n  font-family: inherit;\n  float: left;\n  height: 32px;\n  padding-top: 4px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-dropdown .collapsible-label {\n  padding-top: 4px;\n  padding-left: 0;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-dropdown .dropdown-icon {\n  top: 5px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-search-box {\n  width: 100%;\n  height: 24px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-search-box input {\n  padding: 0px 16px;\n}\n.jobs-filter-container .dnn-grid-cell .job-filters-filter .dnn-dropdown .collapsible-toggle {\n  width: auto;\n}\n.jobs-filter-container .dnn-grid-cell .search-filter {\n  float: left;\n  width: 100%;\n}\n.jobs-filter-container .dnn-grid-cell .search-filter > div {\n  display: block !important;\n  border-left: 1px solid #C8C8C8;\n}\n.jobs-filter-container .dnn-grid-cell .search-filter > div input {\n  display: block;\n  width: 100%;\n  position: inherit !important;\n  border: none;\n  border-radius: none;\n  background-color: transparent;\n  outline: none;\n  padding-right: 45px;\n}\n", ""]);



/***/ }),
/* 116 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = { "default": __webpack_require__(117), __esModule: true };

/***/ }),
/* 117 */
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(118);
module.exports = __webpack_require__(16).Object.assign;


/***/ }),
/* 118 */
/***/ (function(module, exports, __webpack_require__) {

// 19.1.3.1 Object.assign(target, source)
var $export = __webpack_require__(28);

$export($export.S + $export.F, 'Object', { assign: __webpack_require__(120) });


/***/ }),
/* 119 */
/***/ (function(module, exports) {

module.exports = function (it) {
  if (typeof it != 'function') throw TypeError(it + ' is not a function!');
  return it;
};


/***/ }),
/* 120 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// 19.1.2.1 Object.assign(target, source, ...)
var getKeys = __webpack_require__(32);
var gOPS = __webpack_require__(44);
var pIE = __webpack_require__(35);
var toObject = __webpack_require__(64);
var IObject = __webpack_require__(62);
var $assign = Object.assign;

// should work with symbols and should have deterministic property order (V8 bug)
module.exports = !$assign || __webpack_require__(30)(function () {
  var A = {};
  var B = {};
  // eslint-disable-next-line no-undef
  var S = Symbol();
  var K = 'abcdefghijklmnopqrst';
  A[S] = 7;
  K.split('').forEach(function (k) { B[k] = k; });
  return $assign({}, A)[S] != 7 || Object.keys($assign({}, B)).join('') != K;
}) ? function assign(target, source) { // eslint-disable-line no-unused-vars
  var T = toObject(target);
  var aLen = arguments.length;
  var index = 1;
  var getSymbols = gOPS.f;
  var isEnum = pIE.f;
  while (aLen > index) {
    var S = IObject(arguments[index++]);
    var keys = getSymbols ? getKeys(S).concat(getSymbols(S)) : getKeys(S);
    var length = keys.length;
    var j = 0;
    var key;
    while (length > j) if (isEnum.call(S, key = keys[j++])) T[key] = S[key];
  } return T;
} : $assign;


/***/ }),
/* 121 */
/***/ (function(module, exports, __webpack_require__) {

// false -> Array#indexOf
// true  -> Array#includes
var toIObject = __webpack_require__(23);
var toLength = __webpack_require__(122);
var toAbsoluteIndex = __webpack_require__(123);
module.exports = function (IS_INCLUDES) {
  return function ($this, el, fromIndex) {
    var O = toIObject($this);
    var length = toLength(O.length);
    var index = toAbsoluteIndex(fromIndex, length);
    var value;
    // Array#includes uses SameValueZero equality algorithm
    // eslint-disable-next-line no-self-compare
    if (IS_INCLUDES && el != el) while (length > index) {
      value = O[index++];
      // eslint-disable-next-line no-self-compare
      if (value != value) return true;
    // Array#indexOf ignores holes, Array#includes - not
    } else for (;length > index; index++) if (IS_INCLUDES || index in O) {
      if (O[index] === el) return IS_INCLUDES || index || 0;
    } return !IS_INCLUDES && -1;
  };
};


/***/ }),
/* 122 */
/***/ (function(module, exports, __webpack_require__) {

// 7.1.15 ToLength
var toInteger = __webpack_require__(40);
var min = Math.min;
module.exports = function (it) {
  return it > 0 ? min(toInteger(it), 0x1fffffffffffff) : 0; // pow(2, 53) - 1 == 9007199254740991
};


/***/ }),
/* 123 */
/***/ (function(module, exports, __webpack_require__) {

var toInteger = __webpack_require__(40);
var max = Math.max;
var min = Math.min;
module.exports = function (index, length) {
  index = toInteger(index);
  return index < 0 ? max(index + length, 0) : min(index, length);
};


/***/ }),
/* 124 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = { "default": __webpack_require__(125), __esModule: true };

/***/ }),
/* 125 */
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(126);
__webpack_require__(132);
module.exports = __webpack_require__(48).f('iterator');


/***/ }),
/* 126 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var $at = __webpack_require__(127)(true);

// 21.1.3.27 String.prototype[@@iterator]()
__webpack_require__(66)(String, 'String', function (iterated) {
  this._t = String(iterated); // target
  this._i = 0;                // next index
// 21.1.5.2.1 %StringIteratorPrototype%.next()
}, function () {
  var O = this._t;
  var index = this._i;
  var point;
  if (index >= O.length) return { value: undefined, done: true };
  point = $at(O, index);
  this._i += point.length;
  return { value: point, done: false };
});


/***/ }),
/* 127 */
/***/ (function(module, exports, __webpack_require__) {

var toInteger = __webpack_require__(40);
var defined = __webpack_require__(39);
// true  -> String#at
// false -> String#codePointAt
module.exports = function (TO_STRING) {
  return function (that, pos) {
    var s = String(defined(that));
    var i = toInteger(pos);
    var l = s.length;
    var a, b;
    if (i < 0 || i >= l) return TO_STRING ? '' : undefined;
    a = s.charCodeAt(i);
    return a < 0xd800 || a > 0xdbff || i + 1 === l || (b = s.charCodeAt(i + 1)) < 0xdc00 || b > 0xdfff
      ? TO_STRING ? s.charAt(i) : a
      : TO_STRING ? s.slice(i, i + 2) : (a - 0xd800 << 10) + (b - 0xdc00) + 0x10000;
  };
};


/***/ }),
/* 128 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var create = __webpack_require__(46);
var descriptor = __webpack_require__(31);
var setToStringTag = __webpack_require__(47);
var IteratorPrototype = {};

// 25.1.2.1.1 %IteratorPrototype%[@@iterator]()
__webpack_require__(19)(IteratorPrototype, __webpack_require__(24)('iterator'), function () { return this; });

module.exports = function (Constructor, NAME, next) {
  Constructor.prototype = create(IteratorPrototype, { next: descriptor(1, next) });
  setToStringTag(Constructor, NAME + ' Iterator');
};


/***/ }),
/* 129 */
/***/ (function(module, exports, __webpack_require__) {

var dP = __webpack_require__(20);
var anObject = __webpack_require__(29);
var getKeys = __webpack_require__(32);

module.exports = __webpack_require__(22) ? Object.defineProperties : function defineProperties(O, Properties) {
  anObject(O);
  var keys = getKeys(Properties);
  var length = keys.length;
  var i = 0;
  var P;
  while (length > i) dP.f(O, P = keys[i++], Properties[P]);
  return O;
};


/***/ }),
/* 130 */
/***/ (function(module, exports, __webpack_require__) {

var document = __webpack_require__(14).document;
module.exports = document && document.documentElement;


/***/ }),
/* 131 */
/***/ (function(module, exports, __webpack_require__) {

// 19.1.2.9 / 15.2.3.2 Object.getPrototypeOf(O)
var has = __webpack_require__(17);
var toObject = __webpack_require__(64);
var IE_PROTO = __webpack_require__(41)('IE_PROTO');
var ObjectProto = Object.prototype;

module.exports = Object.getPrototypeOf || function (O) {
  O = toObject(O);
  if (has(O, IE_PROTO)) return O[IE_PROTO];
  if (typeof O.constructor == 'function' && O instanceof O.constructor) {
    return O.constructor.prototype;
  } return O instanceof Object ? ObjectProto : null;
};


/***/ }),
/* 132 */
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(133);
var global = __webpack_require__(14);
var hide = __webpack_require__(19);
var Iterators = __webpack_require__(45);
var TO_STRING_TAG = __webpack_require__(24)('toStringTag');

var DOMIterables = ('CSSRuleList,CSSStyleDeclaration,CSSValueList,ClientRectList,DOMRectList,DOMStringList,' +
  'DOMTokenList,DataTransferItemList,FileList,HTMLAllCollection,HTMLCollection,HTMLFormElement,HTMLSelectElement,' +
  'MediaList,MimeTypeArray,NamedNodeMap,NodeList,PaintRequestList,Plugin,PluginArray,SVGLengthList,SVGNumberList,' +
  'SVGPathSegList,SVGPointList,SVGStringList,SVGTransformList,SourceBufferList,StyleSheetList,TextTrackCueList,' +
  'TextTrackList,TouchList').split(',');

for (var i = 0; i < DOMIterables.length; i++) {
  var NAME = DOMIterables[i];
  var Collection = global[NAME];
  var proto = Collection && Collection.prototype;
  if (proto && !proto[TO_STRING_TAG]) hide(proto, TO_STRING_TAG, NAME);
  Iterators[NAME] = Iterators.Array;
}


/***/ }),
/* 133 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var addToUnscopables = __webpack_require__(134);
var step = __webpack_require__(135);
var Iterators = __webpack_require__(45);
var toIObject = __webpack_require__(23);

// 22.1.3.4 Array.prototype.entries()
// 22.1.3.13 Array.prototype.keys()
// 22.1.3.29 Array.prototype.values()
// 22.1.3.30 Array.prototype[@@iterator]()
module.exports = __webpack_require__(66)(Array, 'Array', function (iterated, kind) {
  this._t = toIObject(iterated); // target
  this._i = 0;                   // next index
  this._k = kind;                // kind
// 22.1.5.2.1 %ArrayIteratorPrototype%.next()
}, function () {
  var O = this._t;
  var kind = this._k;
  var index = this._i++;
  if (!O || index >= O.length) {
    this._t = undefined;
    return step(1);
  }
  if (kind == 'keys') return step(0, index);
  if (kind == 'values') return step(0, O[index]);
  return step(0, [index, O[index]]);
}, 'values');

// argumentsList[@@iterator] is %ArrayProto_values% (9.4.4.6, 9.4.4.7)
Iterators.Arguments = Iterators.Array;

addToUnscopables('keys');
addToUnscopables('values');
addToUnscopables('entries');


/***/ }),
/* 134 */
/***/ (function(module, exports) {

module.exports = function () { /* empty */ };


/***/ }),
/* 135 */
/***/ (function(module, exports) {

module.exports = function (done, value) {
  return { value: value, done: !!done };
};


/***/ }),
/* 136 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = { "default": __webpack_require__(137), __esModule: true };

/***/ }),
/* 137 */
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(138);
__webpack_require__(143);
__webpack_require__(144);
__webpack_require__(145);
module.exports = __webpack_require__(16).Symbol;


/***/ }),
/* 138 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// ECMAScript 6 symbols shim
var global = __webpack_require__(14);
var has = __webpack_require__(17);
var DESCRIPTORS = __webpack_require__(22);
var $export = __webpack_require__(28);
var redefine = __webpack_require__(67);
var META = __webpack_require__(139).KEY;
var $fails = __webpack_require__(30);
var shared = __webpack_require__(42);
var setToStringTag = __webpack_require__(47);
var uid = __webpack_require__(34);
var wks = __webpack_require__(24);
var wksExt = __webpack_require__(48);
var wksDefine = __webpack_require__(49);
var enumKeys = __webpack_require__(140);
var isArray = __webpack_require__(141);
var anObject = __webpack_require__(29);
var isObject = __webpack_require__(21);
var toIObject = __webpack_require__(23);
var toPrimitive = __webpack_require__(38);
var createDesc = __webpack_require__(31);
var _create = __webpack_require__(46);
var gOPNExt = __webpack_require__(142);
var $GOPD = __webpack_require__(69);
var $DP = __webpack_require__(20);
var $keys = __webpack_require__(32);
var gOPD = $GOPD.f;
var dP = $DP.f;
var gOPN = gOPNExt.f;
var $Symbol = global.Symbol;
var $JSON = global.JSON;
var _stringify = $JSON && $JSON.stringify;
var PROTOTYPE = 'prototype';
var HIDDEN = wks('_hidden');
var TO_PRIMITIVE = wks('toPrimitive');
var isEnum = {}.propertyIsEnumerable;
var SymbolRegistry = shared('symbol-registry');
var AllSymbols = shared('symbols');
var OPSymbols = shared('op-symbols');
var ObjectProto = Object[PROTOTYPE];
var USE_NATIVE = typeof $Symbol == 'function';
var QObject = global.QObject;
// Don't use setters in Qt Script, https://github.com/zloirock/core-js/issues/173
var setter = !QObject || !QObject[PROTOTYPE] || !QObject[PROTOTYPE].findChild;

// fallback for old Android, https://code.google.com/p/v8/issues/detail?id=687
var setSymbolDesc = DESCRIPTORS && $fails(function () {
  return _create(dP({}, 'a', {
    get: function () { return dP(this, 'a', { value: 7 }).a; }
  })).a != 7;
}) ? function (it, key, D) {
  var protoDesc = gOPD(ObjectProto, key);
  if (protoDesc) delete ObjectProto[key];
  dP(it, key, D);
  if (protoDesc && it !== ObjectProto) dP(ObjectProto, key, protoDesc);
} : dP;

var wrap = function (tag) {
  var sym = AllSymbols[tag] = _create($Symbol[PROTOTYPE]);
  sym._k = tag;
  return sym;
};

var isSymbol = USE_NATIVE && typeof $Symbol.iterator == 'symbol' ? function (it) {
  return typeof it == 'symbol';
} : function (it) {
  return it instanceof $Symbol;
};

var $defineProperty = function defineProperty(it, key, D) {
  if (it === ObjectProto) $defineProperty(OPSymbols, key, D);
  anObject(it);
  key = toPrimitive(key, true);
  anObject(D);
  if (has(AllSymbols, key)) {
    if (!D.enumerable) {
      if (!has(it, HIDDEN)) dP(it, HIDDEN, createDesc(1, {}));
      it[HIDDEN][key] = true;
    } else {
      if (has(it, HIDDEN) && it[HIDDEN][key]) it[HIDDEN][key] = false;
      D = _create(D, { enumerable: createDesc(0, false) });
    } return setSymbolDesc(it, key, D);
  } return dP(it, key, D);
};
var $defineProperties = function defineProperties(it, P) {
  anObject(it);
  var keys = enumKeys(P = toIObject(P));
  var i = 0;
  var l = keys.length;
  var key;
  while (l > i) $defineProperty(it, key = keys[i++], P[key]);
  return it;
};
var $create = function create(it, P) {
  return P === undefined ? _create(it) : $defineProperties(_create(it), P);
};
var $propertyIsEnumerable = function propertyIsEnumerable(key) {
  var E = isEnum.call(this, key = toPrimitive(key, true));
  if (this === ObjectProto && has(AllSymbols, key) && !has(OPSymbols, key)) return false;
  return E || !has(this, key) || !has(AllSymbols, key) || has(this, HIDDEN) && this[HIDDEN][key] ? E : true;
};
var $getOwnPropertyDescriptor = function getOwnPropertyDescriptor(it, key) {
  it = toIObject(it);
  key = toPrimitive(key, true);
  if (it === ObjectProto && has(AllSymbols, key) && !has(OPSymbols, key)) return;
  var D = gOPD(it, key);
  if (D && has(AllSymbols, key) && !(has(it, HIDDEN) && it[HIDDEN][key])) D.enumerable = true;
  return D;
};
var $getOwnPropertyNames = function getOwnPropertyNames(it) {
  var names = gOPN(toIObject(it));
  var result = [];
  var i = 0;
  var key;
  while (names.length > i) {
    if (!has(AllSymbols, key = names[i++]) && key != HIDDEN && key != META) result.push(key);
  } return result;
};
var $getOwnPropertySymbols = function getOwnPropertySymbols(it) {
  var IS_OP = it === ObjectProto;
  var names = gOPN(IS_OP ? OPSymbols : toIObject(it));
  var result = [];
  var i = 0;
  var key;
  while (names.length > i) {
    if (has(AllSymbols, key = names[i++]) && (IS_OP ? has(ObjectProto, key) : true)) result.push(AllSymbols[key]);
  } return result;
};

// 19.4.1.1 Symbol([description])
if (!USE_NATIVE) {
  $Symbol = function Symbol() {
    if (this instanceof $Symbol) throw TypeError('Symbol is not a constructor!');
    var tag = uid(arguments.length > 0 ? arguments[0] : undefined);
    var $set = function (value) {
      if (this === ObjectProto) $set.call(OPSymbols, value);
      if (has(this, HIDDEN) && has(this[HIDDEN], tag)) this[HIDDEN][tag] = false;
      setSymbolDesc(this, tag, createDesc(1, value));
    };
    if (DESCRIPTORS && setter) setSymbolDesc(ObjectProto, tag, { configurable: true, set: $set });
    return wrap(tag);
  };
  redefine($Symbol[PROTOTYPE], 'toString', function toString() {
    return this._k;
  });

  $GOPD.f = $getOwnPropertyDescriptor;
  $DP.f = $defineProperty;
  __webpack_require__(68).f = gOPNExt.f = $getOwnPropertyNames;
  __webpack_require__(35).f = $propertyIsEnumerable;
  __webpack_require__(44).f = $getOwnPropertySymbols;

  if (DESCRIPTORS && !__webpack_require__(33)) {
    redefine(ObjectProto, 'propertyIsEnumerable', $propertyIsEnumerable, true);
  }

  wksExt.f = function (name) {
    return wrap(wks(name));
  };
}

$export($export.G + $export.W + $export.F * !USE_NATIVE, { Symbol: $Symbol });

for (var es6Symbols = (
  // 19.4.2.2, 19.4.2.3, 19.4.2.4, 19.4.2.6, 19.4.2.8, 19.4.2.9, 19.4.2.10, 19.4.2.11, 19.4.2.12, 19.4.2.13, 19.4.2.14
  'hasInstance,isConcatSpreadable,iterator,match,replace,search,species,split,toPrimitive,toStringTag,unscopables'
).split(','), j = 0; es6Symbols.length > j;)wks(es6Symbols[j++]);

for (var wellKnownSymbols = $keys(wks.store), k = 0; wellKnownSymbols.length > k;) wksDefine(wellKnownSymbols[k++]);

$export($export.S + $export.F * !USE_NATIVE, 'Symbol', {
  // 19.4.2.1 Symbol.for(key)
  'for': function (key) {
    return has(SymbolRegistry, key += '')
      ? SymbolRegistry[key]
      : SymbolRegistry[key] = $Symbol(key);
  },
  // 19.4.2.5 Symbol.keyFor(sym)
  keyFor: function keyFor(sym) {
    if (!isSymbol(sym)) throw TypeError(sym + ' is not a symbol!');
    for (var key in SymbolRegistry) if (SymbolRegistry[key] === sym) return key;
  },
  useSetter: function () { setter = true; },
  useSimple: function () { setter = false; }
});

$export($export.S + $export.F * !USE_NATIVE, 'Object', {
  // 19.1.2.2 Object.create(O [, Properties])
  create: $create,
  // 19.1.2.4 Object.defineProperty(O, P, Attributes)
  defineProperty: $defineProperty,
  // 19.1.2.3 Object.defineProperties(O, Properties)
  defineProperties: $defineProperties,
  // 19.1.2.6 Object.getOwnPropertyDescriptor(O, P)
  getOwnPropertyDescriptor: $getOwnPropertyDescriptor,
  // 19.1.2.7 Object.getOwnPropertyNames(O)
  getOwnPropertyNames: $getOwnPropertyNames,
  // 19.1.2.8 Object.getOwnPropertySymbols(O)
  getOwnPropertySymbols: $getOwnPropertySymbols
});

// 24.3.2 JSON.stringify(value [, replacer [, space]])
$JSON && $export($export.S + $export.F * (!USE_NATIVE || $fails(function () {
  var S = $Symbol();
  // MS Edge converts symbol values to JSON as {}
  // WebKit converts symbol values to JSON as null
  // V8 throws on boxed symbols
  return _stringify([S]) != '[null]' || _stringify({ a: S }) != '{}' || _stringify(Object(S)) != '{}';
})), 'JSON', {
  stringify: function stringify(it) {
    var args = [it];
    var i = 1;
    var replacer, $replacer;
    while (arguments.length > i) args.push(arguments[i++]);
    $replacer = replacer = args[1];
    if (!isObject(replacer) && it === undefined || isSymbol(it)) return; // IE8 returns string on undefined
    if (!isArray(replacer)) replacer = function (key, value) {
      if (typeof $replacer == 'function') value = $replacer.call(this, key, value);
      if (!isSymbol(value)) return value;
    };
    args[1] = replacer;
    return _stringify.apply($JSON, args);
  }
});

// 19.4.3.4 Symbol.prototype[@@toPrimitive](hint)
$Symbol[PROTOTYPE][TO_PRIMITIVE] || __webpack_require__(19)($Symbol[PROTOTYPE], TO_PRIMITIVE, $Symbol[PROTOTYPE].valueOf);
// 19.4.3.5 Symbol.prototype[@@toStringTag]
setToStringTag($Symbol, 'Symbol');
// 20.2.1.9 Math[@@toStringTag]
setToStringTag(Math, 'Math', true);
// 24.3.3 JSON[@@toStringTag]
setToStringTag(global.JSON, 'JSON', true);


/***/ }),
/* 139 */
/***/ (function(module, exports, __webpack_require__) {

var META = __webpack_require__(34)('meta');
var isObject = __webpack_require__(21);
var has = __webpack_require__(17);
var setDesc = __webpack_require__(20).f;
var id = 0;
var isExtensible = Object.isExtensible || function () {
  return true;
};
var FREEZE = !__webpack_require__(30)(function () {
  return isExtensible(Object.preventExtensions({}));
});
var setMeta = function (it) {
  setDesc(it, META, { value: {
    i: 'O' + ++id, // object ID
    w: {}          // weak collections IDs
  } });
};
var fastKey = function (it, create) {
  // return primitive with prefix
  if (!isObject(it)) return typeof it == 'symbol' ? it : (typeof it == 'string' ? 'S' : 'P') + it;
  if (!has(it, META)) {
    // can't set metadata to uncaught frozen object
    if (!isExtensible(it)) return 'F';
    // not necessary to add metadata
    if (!create) return 'E';
    // add missing metadata
    setMeta(it);
  // return object ID
  } return it[META].i;
};
var getWeak = function (it, create) {
  if (!has(it, META)) {
    // can't set metadata to uncaught frozen object
    if (!isExtensible(it)) return true;
    // not necessary to add metadata
    if (!create) return false;
    // add missing metadata
    setMeta(it);
  // return hash weak collections IDs
  } return it[META].w;
};
// add metadata on freeze-family methods calling
var onFreeze = function (it) {
  if (FREEZE && meta.NEED && isExtensible(it) && !has(it, META)) setMeta(it);
  return it;
};
var meta = module.exports = {
  KEY: META,
  NEED: false,
  fastKey: fastKey,
  getWeak: getWeak,
  onFreeze: onFreeze
};


/***/ }),
/* 140 */
/***/ (function(module, exports, __webpack_require__) {

// all enumerable object keys, includes symbols
var getKeys = __webpack_require__(32);
var gOPS = __webpack_require__(44);
var pIE = __webpack_require__(35);
module.exports = function (it) {
  var result = getKeys(it);
  var getSymbols = gOPS.f;
  if (getSymbols) {
    var symbols = getSymbols(it);
    var isEnum = pIE.f;
    var i = 0;
    var key;
    while (symbols.length > i) if (isEnum.call(it, key = symbols[i++])) result.push(key);
  } return result;
};


/***/ }),
/* 141 */
/***/ (function(module, exports, __webpack_require__) {

// 7.2.2 IsArray(argument)
var cof = __webpack_require__(63);
module.exports = Array.isArray || function isArray(arg) {
  return cof(arg) == 'Array';
};


/***/ }),
/* 142 */
/***/ (function(module, exports, __webpack_require__) {

// fallback for IE11 buggy Object.getOwnPropertyNames with iframe and window
var toIObject = __webpack_require__(23);
var gOPN = __webpack_require__(68).f;
var toString = {}.toString;

var windowNames = typeof window == 'object' && window && Object.getOwnPropertyNames
  ? Object.getOwnPropertyNames(window) : [];

var getWindowNames = function (it) {
  try {
    return gOPN(it);
  } catch (e) {
    return windowNames.slice();
  }
};

module.exports.f = function getOwnPropertyNames(it) {
  return windowNames && toString.call(it) == '[object Window]' ? getWindowNames(it) : gOPN(toIObject(it));
};


/***/ }),
/* 143 */
/***/ (function(module, exports) {



/***/ }),
/* 144 */
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(49)('asyncIterator');


/***/ }),
/* 145 */
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(49)('observable');


/***/ }),
/* 146 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = { "default": __webpack_require__(147), __esModule: true };

/***/ }),
/* 147 */
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(148);
module.exports = __webpack_require__(16).Object.setPrototypeOf;


/***/ }),
/* 148 */
/***/ (function(module, exports, __webpack_require__) {

// 19.1.3.19 Object.setPrototypeOf(O, proto)
var $export = __webpack_require__(28);
$export($export.S, 'Object', { setPrototypeOf: __webpack_require__(149).set });


/***/ }),
/* 149 */
/***/ (function(module, exports, __webpack_require__) {

// Works with __proto__ only. Old v8 can't work with null proto objects.
/* eslint-disable no-proto */
var isObject = __webpack_require__(21);
var anObject = __webpack_require__(29);
var check = function (O, proto) {
  anObject(O);
  if (!isObject(proto) && proto !== null) throw TypeError(proto + ": can't set as prototype!");
};
module.exports = {
  set: Object.setPrototypeOf || ('__proto__' in {} ? // eslint-disable-line
    function (test, buggy, set) {
      try {
        set = __webpack_require__(58)(Function.call, __webpack_require__(69).f(Object.prototype, '__proto__').set, 2);
        set(test, []);
        buggy = !(test instanceof Array);
      } catch (e) { buggy = true; }
      return function setPrototypeOf(O, proto) {
        check(O, proto);
        if (buggy) O.__proto__ = proto;
        else set(O, proto);
        return O;
      };
    }({}, false) : undefined),
  check: check
};


/***/ }),
/* 150 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = { "default": __webpack_require__(151), __esModule: true };

/***/ }),
/* 151 */
/***/ (function(module, exports, __webpack_require__) {

__webpack_require__(152);
var $Object = __webpack_require__(16).Object;
module.exports = function create(P, D) {
  return $Object.create(P, D);
};


/***/ }),
/* 152 */
/***/ (function(module, exports, __webpack_require__) {

var $export = __webpack_require__(28);
// 19.1.2.2 / 15.2.3.5 Object.create(O [, Properties])
$export($export.S, 'Object', { create: __webpack_require__(46) });


/***/ }),
/* 153 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(154);

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
/* 154 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.package-analyzing {\n  text-align: center;\n}\n.package-analyzing #progressContainer {\n  width: 105px;\n  height: 5px;\n  margin-left: auto;\n  margin-right: auto;\n  margin-top: -60px;\n}\n.package-analyzing .progressCards,\n.package-analyzing .progressGraph,\n.package-analyzing .progressFlow {\n  padding-top: 9px;\n}\n.package-analyzing .progressTable {\n  padding-top: 7px;\n}\n.package-analyzing .progressTopFive {\n  padding-top: 8px;\n}\n", ""]);



/***/ }),
/* 155 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(11)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n._1KRSzonIHT12Bdse4iT_GU {\n  border: 1px solid #C8C8C8;\n  background: #fff;\n  float: left;\n  width: 100%;\n}\n._1KRSzonIHT12Bdse4iT_GU * {\n  box-sizing: border-box;\n}\n._1KRSzonIHT12Bdse4iT_GU .pageTitle {\n  text-transform: uppercase;\n  font-weight: bold;\n  padding: 40px 0 30px 40px;\n}\n._1KRSzonIHT12Bdse4iT_GU .pageDescription {\n  font-weight: 700;\n  color: #4B4E4F;\n  padding: 0 40px 15px 40px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper {\n  width: 100%;\n  padding: 0 40px 70px 40px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-analyzing {\n  float: left;\n  width: 100%;\n  padding-bottom: 40px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-analyzing .noDataText {\n  font-size: x-large;\n  color: #4B4E4F;\n  padding: 40px 0 0 0;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-analyzing .noDataImage {\n  width: 100%;\n  text-align: center;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-analyzing .noDataImage > svg {\n  width: 648px;\n  height: 400px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper {\n  float: left;\n  position: relative;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper:not(:last-child) {\n  margin-bottom: 15px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card-tooltip {\n  background-color: #fff;\n  position: absolute;\n  padding: 10px 20px;\n  background: #ffffff;\n  box-shadow: #c8c8c8 0px 0px 10px 1px;\n  border-radius: 3px;\n  transition: opacity 0.2s ease-in-out 0s, visibility 0.2s ease-in-out 0s;\n  color: white;\n  max-width: 500px;\n  left: 50%;\n  margin-left: -25%;\n  opacity: 0;\n  visibility: hidden;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card-tooltip ::after {\n  content: \" \";\n  position: absolute;\n  bottom: 100%;\n  left: 50%;\n  margin-left: -10px;\n  border-width: 10px;\n  border-style: solid;\n  border-color: transparent transparent white transparent;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper:hover .package-card-tooltip {\n  opacity: 1;\n  visibility: visible;\n  z-index: 9999;\n  cursor: pointer;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card {\n  text-align: left;\n  border: 1px solid #C8C8C8;\n  width: 100%;\n  display: inline-block;\n  position: relative;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card:hover .package-card-overlay {\n  opacity: 0.75;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card.selected {\n  border: 2px solid #1E88C3;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card.selected .checkmark {\n  position: absolute;\n  top: 5px;\n  right: 5px;\n  background-color: #1E88C3;\n  display: block;\n  width: 15px;\n  height: 15px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card.selected .checkmark svg {\n  width: 100%;\n  fill: #FFFFFF;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid {\n  padding: 20px 0 10px 0;\n  width: 100%;\n  float: left;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column1 {\n  border-right: 1px solid #C8C8C8;\n  padding: 0 20px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column1 .package-name {\n  font-weight: bold;\n  padding-bottom: 10px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column1 .package-field {\n  padding-bottom: 10px;\n  color: #4B4E4F;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column2 .package-field,\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column4 .package-field {\n  float: left;\n  width: 100%;\n  padding: 0 0 13px 20px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column2 .package-field .dnn-label,\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column4 .package-field .dnn-label {\n  padding-top: 0;\n  font-weight: 700;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column2 .package-field span,\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column4 .package-field span {\n  float: right;\n  color: #4B4E4F;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column3 {\n  color: #4B4E4F;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column3 .package-field {\n  float: right;\n  padding: 0 10px 10px 0;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column5 {\n  color: #4B4E4F;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .card-grid .card-column5 .package-field {\n  padding: 0 20px 10px 0;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .template-icon {\n  background-color: #EFF0F0;\n  height: 120px;\n  text-align: center;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .template-icon svg {\n  width: 60px;\n  height: 60px;\n  margin-top: 30px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .package-card-overlay {\n  position: absolute;\n  top: 0;\n  width: 100%;\n  height: 100%;\n  background: #1E88C3;\n  transition: 0.3s;\n  opacity: 0;\n  cursor: pointer;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .package-card-overlay .icon-container {\n  color: #fff;\n  text-align: center;\n  width: 100%;\n  position: absolute;\n  top: 50%;\n  -webkit-transform: translateY(-50%);\n  -ms-transform: translateY(-50%);\n  transform: translateY(-50%);\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .package-card-overlay .icon-container > div {\n  display: inline;\n  margin-right: 10px;\n  font-size: 15px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .package-card-overlay .icon-container > div svg {\n  width: 40px;\n  fill: white;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .packages .package-card-wrapper .package-card .package-card-overlay .dnn-text-overflow-wrapper {\n  position: absolute;\n  bottom: 92px;\n  font-size: 15px;\n  margin-left: 16px;\n  opacity: 0;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary {\n  float: left;\n  width: 100%;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary .sectionTitle {\n  text-transform: uppercase;\n  font-weight: bold;\n  padding: 40px 0 0;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary .import-site-container {\n  font-weight: 700;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary .import-site-container .left-column {\n  width: 50%;\n  float: left;\n  border-right: 1px solid #C8C8C8;\n  padding: 10px 30px 10px 0;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary .import-site-container .right-column {\n  width: 50%;\n  float: left;\n  padding: 10px 0 10px 30px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary .import-site-container .right-column .seperator {\n  float: left;\n  width: 100%;\n  padding: 10px 0;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary .import-site-container .right-column .dnn-switch-container {\n  float: right;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary .import-site-container .import-summary-item {\n  padding-top: 7px;\n  float: right;\n  color: #4B4E4F;\n  font-weight: 100;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary .import-site-container .dnn-grid-cell {\n  padding-bottom: 10px;\n}\n._1KRSzonIHT12Bdse4iT_GU .packages-wrapper .import-summary .finish-importing {\n  float: left;\n  font-weight: 700;\n  color: #1E88C3;\n  padding: 30px 0 20px 0;\n  width: 100%;\n  text-align: center;\n}\n._1KRSzonIHT12Bdse4iT_GU .packagePager {\n  padding: 15px 0 20px 0;\n  float: left;\n  width: 100%;\n  border-bottom: 1px solid #C8C8C8;\n}\n._1KRSzonIHT12Bdse4iT_GU .action-buttons {\n  text-align: center;\n  padding: 20px 0 50px 0;\n}\n._1KRSzonIHT12Bdse4iT_GU .action-buttons button {\n  margin: 5px;\n}\n", ""]);

// Exports
exports.locals = {
	"importModal": "_1KRSzonIHT12Bdse4iT_GU"
};

/***/ }),
/* 156 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 526.08 398.98\"><defs><style>.a{fill:#ccc;}.b{fill:#e6e6e6;}.c{fill:#b3b3b3;}.d{fill:#9f9fa0;}</style></defs><title>revised no data states</title><ellipse class=\"a\" cx=\"330.81\" cy=\"309.79\" rx=\"94.54\" ry=\"5.32\"/><polygon class=\"b\" points=\"326.94 249.77 334.37 250.44 351.94 59.79 344.51 59.12 326.94 249.77\"/><path class=\"c\" d=\"M351.92,60,334.37,250.44c6.74-11.63,14.34-49.29,18.49-94.33S356.42,72.67,351.92,60Z\"/><path class=\"c\" d=\"M402.31,290.89,365.52,162.31c-2-6.9-9.55-10.87-16.85-8.84l-1.5.42.65,2.27a12.57,12.57,0,0,1,14.5,8.87l35.23,123.14a12.37,12.37,0,0,1-7.65,15.06h-63v4.33h64.2l1.5-.42C399.93,305.1,404.28,297.79,402.31,290.89Z\"/><polygon class=\"c\" points=\"324.16 298.83 239.48 306.63 239.11 302.73 323.8 294.93 324.16 298.83\"/><polygon class=\"c\" points=\"315.77 309.67 305.55 309.67 305.55 299.28 315.77 296.1 315.77 309.67\"/><path class=\"a\" d=\"M239.56,307.51a31.91,31.91,0,0,1-6.12,3.58s-12.41-1.18-14,0S203.69,308,203.69,308l-8.11-.79-6.65,1.69h-10.3s-11.57-3-13.9-3.16a4.66,4.66,0,0,1-.64-.11c-27,.79-46.44,3-46.44,5.61,0,3.27,30.65,5.92,68.46,5.92s68.46-2.65,68.46-5.92C254.57,309.81,248.95,308.52,239.56,307.51Z\"/><path class=\"c\" d=\"M150.71,289.22c-.43,4.75,9.8,16.42,14.45,19.15,7.51,4.41,25.07,3.9,30.48-1.17-5,1.45-26.25.06-31.34-4.83s-11.09-20-11.21-19.83c-.45-3.41-3-6.9-6.82-9.92-2.18,6.08-12.3,9-24.4,6.62a42.38,42.38,0,0,1-17.49-7.64,7.41,7.41,0,0,0-.45,2.39c-.12,7.67,10.8,16.08,24.4,18.78C138.17,294.73,146.69,293.15,150.71,289.22Z\"/><path class=\"c\" d=\"M223.2,288.73a8.59,8.59,0,0,1,1.38-3l1.7,4.16a5,5,0,0,0,.9,2.21l0,.08h0c1.35,1.82,3.94,3.08,7.18,3.19,5.32.19,10.34-2.76,11.2-6.59s-2.75-7.1-8.07-7.29a13.87,13.87,0,0,0-4,.45l-.67-1.64a19.27,19.27,0,0,1,5.9-.72c7.4.27,12.42,4.8,11.22,10.13s-8.17,9.43-15.57,9.16S222,294.05,223.2,288.73Z\"/><path class=\"c\" d=\"M103.09,272l-.17,0,.09.17Z\"/><path class=\"c\" d=\"M202.75,167.56c-2.07-2.64-4.6-4.06-5.65-3.19s-.23,3.72,1.84,6.36,4.6,4.06,5.65,3.19S204.82,170.2,202.75,167.56Z\"/><path class=\"c\" d=\"M214.52,174.79c.5,3.37,6.47,5.22,13.33,4.14s12-4.68,11.53-8-6.47-5.22-13.33-4.14S214,171.42,214.52,174.79Z\"/><path class=\"c\" d=\"M183.92,161.85c5.23-1.84,10.28-1.9,13.37-.45-1.95-4.06-8.75-5.39-15.71-2.94-7.31,2.58-12,8.36-10.49,12.91l1.63-.57C173.91,167.44,178.2,163.86,183.92,161.85Z\"/><path class=\"c\" d=\"M239.71,170.11a5.75,5.75,0,0,1,.86,2.88c.39,7.25-7.75,20.05-18.19,28.59-3,2.42-9.65,9.08-12.19,10.28,3,.71,11.33-5.46,16.35-8.95,9.45-6.56,17.35-18.29,17.64-26.2C244.33,172.83,242.61,170.61,239.71,170.11Z\"/><path class=\"c\" d=\"M150.43,269.88c2.92-2.53,3.46-5,5.56-8.14a25,25,0,0,1,2.87-3.57,19.63,19.63,0,0,0,2.54-1.68,8.11,8.11,0,0,0,4.93-1.19c1.43-1,4.65-2.54,7.5-5.51a9.47,9.47,0,0,0,12-1.91,10.18,10.18,0,0,0-.63-14.08,9.48,9.48,0,0,0-7-2.59L183,215.46l-3.43-3.22s-6.9,15.18-7.69,18.22c-.49,1.88-6.17,6.25-10.43,9.3a20.6,20.6,0,0,0-3.08,2.16L156.77,243l-.48.89c-.22.23-.43.45-.64.69-3,3.39-4.83,7.14-5.16,10l-.24.44-2.57,14.11c-.38.89-.76,2-.39,2.36C150.33,274.21,150.13,270.14,150.43,269.88Z\"/><path class=\"c\" d=\"M161.81,197.27l6.05,4.78s5.33,2.11,6.77,1.89,6.42-6,6.42-6l-13.88-23.36c-2.81,1.67-4.85-1.62-10.7,1.24s-8.55,9.19-8.55,9.19l6.94,6.15Z\"/><path class=\"c\" d=\"M233.71,295.35a23.63,23.63,0,0,0-13.08-.24,18.9,18.9,0,0,0-.88-3.63,18.52,18.52,0,0,0-4.08-6.68,16.86,16.86,0,0,0,.89-13,17.77,17.77,0,0,0-8.36-9.53,32.5,32.5,0,0,0-21.75-1,34.62,34.62,0,0,0-7.35,3.09,26.7,26.7,0,0,0-3.27,2.66,37.65,37.65,0,0,1,5.28-2.14,37.23,37.23,0,0,1,10.34-1.77l5.72.37c7.56,1.16,13.6,5,15.69,11,2,5.74-.12,12.1-5.08,17.28a14.41,14.41,0,0,1,6.43,7,25.41,25.41,0,0,1,11.68.36c7.21,1.76,12,5.55,10.6,8.47s-14,3.51-21.2,1.75c-1.15-.28-18.63-1.72-19.64-2.09,1.81,1.45,19.1,3.84,21.92,4.67,8.11,2.39,23.88.86,26.35-3.79S241.82,297.74,233.71,295.35Z\"/><path class=\"c\" d=\"M216.74,255.26c-6.82-30.18-5-40.71-6.7-22.91-4.65,3.13-4.43,15.27.68,28,5.37,13.41,17.41,23.84,22.82,21.54l-.67-1.64C226.91,277.59,219.38,267,216.74,255.26Z\"/><path class=\"d\" d=\"M209.81,215c.93,6.35-3.47,10.88-10.53,10.35-6.28-.47-12.7-4.79-16-10.31l-1.83-2.89-3.87-.34c3.11,9.09,13.11,16.87,22.83,17.6,9.42.71,15.24-5.45,13.73-14Z\"/></svg>");

/***/ })
/******/ ]);
//# sourceMappingURL=siteimportexport-bundle.js.map