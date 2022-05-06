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
/******/ 	return __webpack_require__(__webpack_require__.s = 45);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.React;

/***/ }),
/* 1 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (true) {
  module.exports = __webpack_require__(46);
} else { var jsFeaturesPresent, evalAllowed; }


/***/ }),
/* 2 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();


var Localization = {
  get: function get(key) {
    var moduleName = _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].moduleName;
    return _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].utilities.getResx(moduleName, key);
  }
};
var _default = Localization;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Localization, "Localization", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\localization\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\localization\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

if (false) { var throwOnDirectAccess, isValidElement, REACT_ELEMENT_TYPE; } else {
  // By explicitly using `prop-types` you are opting into new production behavior.
  // http://fb.me/prop-types-in-prod
  module.exports = __webpack_require__(51)();
}


/***/ }),
/* 4 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.CommonComponents;

/***/ }),
/* 5 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _pagination__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(19);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "c", function() { return _pagination__WEBPACK_IMPORTED_MODULE_0__["a"]; });

/* harmony import */ var _visiblePanel__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(20);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "d", function() { return _visiblePanel__WEBPACK_IMPORTED_MODULE_1__["a"]; });

/* harmony import */ var _logSettings__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(21);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _logSettings__WEBPACK_IMPORTED_MODULE_2__["a"]; });

/* harmony import */ var _log__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(22);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _log__WEBPACK_IMPORTED_MODULE_3__["a"]; });







/***/ }),
/* 6 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(utils, "utils", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\utils\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\utils\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 7 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _pagination__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(23);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "c", function() { return _pagination__WEBPACK_IMPORTED_MODULE_0__["a"]; });

/* harmony import */ var _visiblePanel__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(24);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "d", function() { return _visiblePanel__WEBPACK_IMPORTED_MODULE_1__["a"]; });

/* harmony import */ var _logSettings__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(25);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _logSettings__WEBPACK_IMPORTED_MODULE_2__["a"]; });

/* harmony import */ var _log__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(26);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _log__WEBPACK_IMPORTED_MODULE_3__["a"]; });







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
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

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
    key: "getLogList",
    value: function getLogList(searchParameters, callback) {
      var sf = this.getServiceFramework("AdminLogs");
      searchParameters = _extends({}, searchParameters, {//logType: "*"
      });
      sf.get("GetLogItems?" + serializeQueryStringParameters(searchParameters), {}, callback);
    }
  }, {
    key: "getPortalList",
    value: function getPortalList(addAll, callback) {
      var sf = this.getServiceFramework("Portals");
      sf.get("GetPortals?addAll=" + addAll, {}, callback);
    }
    /*Get all the types*/

  }, {
    key: "getLogTypes",
    value: function getLogTypes(callback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.get("GetLogTypes", {}, callback);
    }
  }, {
    key: "deleteLogItems",
    value: function deleteLogItems(payload, callback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.post("DeleteLogItems", payload, callback);
    }
  }, {
    key: "emailLogItems",
    value: function emailLogItems(payload, callback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.post("EmailLogItems", payload, callback);
    }
  }, {
    key: "clearLog",
    value: function clearLog(callback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.post("ClearLog", {}, callback);
    }
    /*Get  Keep Most Recent Options*/

  }, {
    key: "getKeepMostRecentOptions",
    value: function getKeepMostRecentOptions(callback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.get("GetKeepMostRecentOptions", {}, callback);
    }
    /*Get  Occurence Options*/

  }, {
    key: "getOccurrenceOptions",
    value: function getOccurrenceOptions(callback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.get("GetOccurrenceOptions", {}, callback);
    }
    /*Get a particular log setting details*/

  }, {
    key: "getLogSettingById",
    value: function getLogSettingById(parameters, callback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.get("GetLogSetting?" + serializeQueryStringParameters(parameters), {}, callback);
    }
    /*Get latest log setting details*/

  }, {
    key: "getLatestLogSetting",
    value: function getLatestLogSetting(callback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.get("GetLatestLogSetting", {}, callback);
    }
  }, {
    key: "getLogSettings",
    value: function getLogSettings(callback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.get("GetLogSettings", {}, callback);
    }
    /*Update a log type setting*/

  }, {
    key: "updateLogSetting",
    value: function updateLogSetting(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.post("UpdateLogSetting", payload, callback, failureCallback);
    }
    /*Add a log type setting*/

  }, {
    key: "addLogSetting",
    value: function addLogSetting(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.post("AddLogSetting", payload, callback, failureCallback);
    }
  }, {
    key: "deleteLogSetting",
    value: function deleteLogSetting(payload, callback, failureCallback) {
      var sf = this.getServiceFramework("AdminLogs");
      sf.post("DeleteLogSetting", payload, callback, failureCallback);
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(serializeQueryStringParameters, "serializeQueryStringParameters", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(ApplicationService, "ApplicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(applicationService, "applicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\services\\applicationService.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 10 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactRedux;

/***/ }),
/* 11 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "f", function() { return updateLogSettingList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "e", function() { return removeLogSetting; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return createLogTypeOptions; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "d", function() { return createPortalOptions; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return createKeepMostRecentOptions; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "c", function() { return createOccurrenceOptions; });
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();

function updateLogSettingList(logSettingList, logSettingDetail) {
  if (logSettingList.some(function (logSetting) {
    return logSetting.ID === logSettingDetail.ID;
  })) {
    logSettingList = logSettingList.filter(function (logSetting) {
      return logSetting.ID !== logSettingDetail.ID;
    });
  }

  if (!logSettingList.some(function (logSetting) {
    return logSetting.ID === logSettingDetail.ID;
  })) {
    logSettingList.push(logSettingDetail);
    logSettingList = logSettingList.sort(function (a, b) {
      var logTypeFriendlyNameA = a.LogTypeFriendlyName;
      var logTypeFriendlyNameB = b.LogTypeFriendlyName;
      if (logTypeFriendlyNameA < logTypeFriendlyNameB) //sort string asce`nding
        return -1;
      if (logTypeFriendlyNameA > logTypeFriendlyNameB) return 1;
      return 0; //default return value (no sorting)
    });
    return logSettingList;
  }
}
function removeLogSetting(logSettingList, logSettingId) {
  if (logSettingList.some(function (logSetting) {
    return logSetting.ID === logSettingId;
  })) {
    logSettingList = logSettingList.filter(function (logSetting) {
      return logSetting.ID !== logSettingId;
    });
  }

  return logSettingList;
}
function createLogTypeOptions(actionLogTypeList) {
  var logTypeOptions = [];

  if (actionLogTypeList !== undefined) {
    logTypeOptions = actionLogTypeList.map(function (item) {
      return {
        label: item.LogTypeFriendlyName,
        value: item.LogTypeKey
      };
    });
  }

  return logTypeOptions;
}
function createPortalOptions(actionPortalList) {
  var portalOptions = [];

  if (actionPortalList !== undefined) {
    portalOptions = actionPortalList.map(function (item) {
      return {
        label: item.PortalName,
        value: item.PortalID
      };
    });
  }

  return portalOptions;
}
function createKeepMostRecentOptions(actionKeepMostRecentList) {
  var keepMostRecentOptions = [];

  if (actionKeepMostRecentList !== undefined) {
    keepMostRecentOptions = actionKeepMostRecentList.map(function (item) {
      return {
        label: item.Key,
        value: item.Value
      };
    });
  }

  return keepMostRecentOptions;
}
function createOccurrenceOptions(actionOccurrenceData) {
  var occurrenceOptions = {
    thresholdsOptions: [],
    notificationTimesOptions: [],
    notificationTimeTypesOptions: []
  };
  occurrenceOptions.thresholdsOptions = actionOccurrenceData.thresholds.map(function (item) {
    return {
      label: item.Key,
      value: item.Value
    };
  });
  occurrenceOptions.notificationTimesOptions = actionOccurrenceData.notificationTimes.map(function (item) {
    return {
      label: item.Key,
      value: item.Value
    };
  });
  occurrenceOptions.notificationTimeTypesOptions = actionOccurrenceData.notificationTimeTypes.map(function (item) {
    return {
      label: item.Key,
      value: item.Value
    };
  });
  return occurrenceOptions;
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(updateLogSettingList, "updateLogSettingList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducerHelpers\\index.js");
  reactHotLoader.register(removeLogSetting, "removeLogSetting", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducerHelpers\\index.js");
  reactHotLoader.register(createLogTypeOptions, "createLogTypeOptions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducerHelpers\\index.js");
  reactHotLoader.register(createPortalOptions, "createPortalOptions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducerHelpers\\index.js");
  reactHotLoader.register(createKeepMostRecentOptions, "createKeepMostRecentOptions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducerHelpers\\index.js");
  reactHotLoader.register(createOccurrenceOptions, "createOccurrenceOptions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducerHelpers\\index.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 12 */
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
/* 13 */
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

var	fixUrls = __webpack_require__(49);

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
/* 14 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.Redux;

/***/ }),
/* 15 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();


var adminLogs = {
  init: function init() {
    // This setting is required and define the public path 
    // to allow the web application to download assets on demand 
    // eslint-disable-next-line no-undef
    // __webpack_public_path__ = options.publicPath;
    var options = window.dnn.initAdminLogs();
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].init(options.utility);
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].moduleName = options.moduleName;
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].settings = options.settings; // delay the styles loading after the __webpack_public_path__ is set
    // this allows the fonts associated to be loaded properly in production

    __webpack_require__(47);
  },
  dispatch: function dispatch() {
    throw new Error("dispatch method needs to be overwritten from the Redux store");
  }
};
var _default = adminLogs;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(adminLogs, "adminLogs", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\globals\\application.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\globals\\application.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 16 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 17 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(59);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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






var LogSettingRow =
/*#__PURE__*/
function (_Component) {
  _inherits(LogSettingRow, _Component);

  function LogSettingRow() {
    var _this;

    _classCallCheck(this, LogSettingRow);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(LogSettingRow).call(this));
    _this.handleClick = _this.handleClick.bind(_assertThisInitialized(_this));
    return _this;
  }

  _createClass(LogSettingRow, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      document.addEventListener("click", this.handleClick);
      this._isMounted = true;
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      document.removeEventListener("click", this.handleClick);
      this._isMounted = false;
    }
  }, {
    key: "componentWillMount",
    value: function componentWillMount() {
      var opened = this.props.openId !== "" && this.props.id === this.props.openId;
      this.setState({
        opened: opened
      });
    }
  }, {
    key: "handleClick",
    value: function handleClick(event) {
      // Note: this workaround is needed in IE. The remove event listener in the componentWillUnmount is called
      // before the handleClick handler is called, but in spite of that, the handleClick is executed. To avoid
      // the "findDOMNode was called on an unmounted component." error we need to check if the component is mounted before execute this code
      if (!this._isMounted) {
        return;
      }

      if (!this.node.contains(event.target) && typeof event.target.className === "string" && event.target.className.indexOf("do-not-close") === -1) {
        this.timeout = 475;

        if (this.props.openId !== "" && this.props.id === this.props.openId) {
          this.props.Collapse();
        }
      } else {
        this.timeout = 0;
      }
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
    key: "render",
    value: function render() {
      var _this2 = this;

      var props = this.props;
      var opened = this.props.openId !== "" && this.props.id === this.props.openId;
      var uniqueId = "settingrow-" + Math.random() + Date.now();
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          ref: function ref(node) {
            return _this2.node = node;
          },
          className: "collapsible-component-log" + (opened ? " row-opened" : ""),
          id: uniqueId
        }, props.visible &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "collapsible-header-log " + !opened
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["GridCell"], {
          title: props.typeName,
          columnSize: 40
        }, props.typeName),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["GridCell"], {
          columnSize: 20
        }, props.website),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["GridCell"], {
          columnSize: 15
        }, props.activeStatus),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["GridCell"], {
          columnSize: 20
        }, props.fileName, "\xA0 "), !props.readOnly &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["GridCell"], {
          columnSize: 5
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "edit-icon " + !opened,
          dangerouslySetInnerHTML: {
            __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["SvgIcons"].EditIcon
          },
          onClick: this.toggle.bind(this)
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["Collapsible"], {
          isOpened: opened,
          style: {
            width: "100%",
            height: "auto"
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

  return LogSettingRow;
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

LogSettingRow.propTypes = {
  cssClass: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  typeName: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  website: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  activeStatus: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  fileName: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  className: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  OpenCollapse: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  Collapse: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  id: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  openId: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  visible: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.bool,
  readOnly: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.bool
};
LogSettingRow.defaultProps = {
  collapsed: true,
  visible: true,
  readOnly: false
};
var _default = LogSettingRow;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(LogSettingRow, "LogSettingRow", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\LogSettings\\LogSettingRow\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\LogSettings\\LogSettingRow\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 18 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(10);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(61);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(7);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(6);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(2);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }









/*eslint-disable eqeqeq*/

var LogSettingEditor =
/*#__PURE__*/
function (_Component) {
  _inherits(LogSettingEditor, _Component);

  function LogSettingEditor() {
    var _this;

    _classCallCheck(this, LogSettingEditor);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(LogSettingEditor).call(this));
    _this.state = {
      logSettingDetail: {
        KeepMostRecent: "*",
        LogTypeKey: "*",
        LogTypePortalID: "-1",
        LoggingIsActive: false,
        EmailNotificationIsActive: false,
        MailFromAddress: "",
        MailToAddress: "",
        NotificationThreshold: 1,
        NotificationThresholdTime: 1,
        NotificationThresholdTimeType: 1
      },
      triedToSubmit: false,
      formModified: false,
      error: {
        mailToAddress: false
      }
    };
    return _this;
  }

  _createClass(LogSettingEditor, [{
    key: "componentWillMount",
    value: function componentWillMount() {
      var _this2 = this;

      var props = this.props;

      if (props.logTypeSettingId !== "") {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* logSettings */ "b"].getLogSettingById({
          logTypeConfigId: props.logTypeSettingId
        }, function (data) {
          var logSettingDetail = _extends({}, data);

          _this2.setState({
            logSettingDetail: logSettingDetail
          });

          _this2.SetErrorState();
        }));
      }
    }
  }, {
    key: "getValue",
    value: function getValue(selectKey) {
      var state = this.state;

      switch (selectKey) {
        case "LogType":
          return state.logSettingDetail.LogTypeKey !== undefined ? state.logSettingDetail.LogTypeKey.toString() : "*";

        case "Website":
          return state.logSettingDetail.LogTypePortalID !== "-1" ? state.logSettingDetail.LogTypePortalID.toString() == "*" ? "-1" : state.logSettingDetail.LogTypePortalID.toString() : this.props.portalList[0].value.toString();

        case "Recent":
          return state.logSettingDetail.KeepMostRecent !== undefined && state.logSettingDetail.KeepMostRecent > 0 ? state.logSettingDetail.KeepMostRecent.toString() : "*";

        case "Threshold":
          return state.logSettingDetail.NotificationThreshold !== undefined && state.logSettingDetail.NotificationThreshold > 0 ? state.logSettingDetail.NotificationThreshold.toString() : "1";

        case "ThresholdNotificationTime":
          return state.logSettingDetail.NotificationThresholdTime !== undefined && state.logSettingDetail.NotificationThresholdTime > 0 ? state.logSettingDetail.NotificationThresholdTime.toString() : "1";

        case "ThresholdNotificationTimeType":
          return state.logSettingDetail.NotificationThresholdTimeType !== undefined && state.logSettingDetail.NotificationThresholdTimeType > 0 ? state.logSettingDetail.NotificationThresholdTimeType.toString() : "1";

        case "MailFromAddress":
          return state.logSettingDetail.MailFromAddress !== undefined ? state.logSettingDetail.MailFromAddress.toString() : "";

        case "MailToAddress":
          return state.logSettingDetail.MailToAddress !== undefined ? state.logSettingDetail.MailToAddress.toString() : "";

        default:
          break;
      }
    }
  }, {
    key: "getEnabledStatus",
    value: function getEnabledStatus(key) {
      var state = this.state;

      switch (key) {
        case "EmailNotification":
          return state.logSettingDetail.EmailNotificationIsActive !== undefined ? state.logSettingDetail.EmailNotificationIsActive : false;

        case "Logging":
          return state.logSettingDetail.LoggingIsActive !== undefined ? state.logSettingDetail.LoggingIsActive : false;

        default:
          break;
      }
    }
  }, {
    key: "onDropDownChange",
    value: function onDropDownChange(key, option) {
      this.ProcessChange(key, option.value);
    }
  }, {
    key: "onTextChange",
    value: function onTextChange(key, event) {
      this.ProcessChange(key, event.target.value);
    }
  }, {
    key: "ProcessChange",
    value: function ProcessChange(key, value) {
      var logSettingDetail = this.state.logSettingDetail;
      logSettingDetail[key] = value;
      this.setState({
        logSettingDetail: logSettingDetail
      });
      this.SetErrorState();
      var state = this.state;
      state.formModified = true;
      this.setState({
        state: state
      });
    }
  }, {
    key: "SetErrorState",
    value: function SetErrorState() {
      var logSettingDetail = this.state.logSettingDetail;
      var state = this.state;

      if (logSettingDetail.EmailNotificationIsActive) {
        if (!this.validateEmail(logSettingDetail.MailToAddress)) {
          state.error["mailToAddress"] = true;
        } else {
          state.error["mailToAddress"] = false;
        }
      } else {
        state.error["mailToAddress"] = false;
      }

      state.triedToSubmit = false;
      this.setState({
        state: state
      });
    }
  }, {
    key: "validateEmail",
    value: function validateEmail(value) {
      var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return re.test(value);
    }
  }, {
    key: "OnCheckboxChanged",
    value: function OnCheckboxChanged(key, status) {
      var logSettingDetail = this.state.logSettingDetail;
      logSettingDetail[key] = status;
      this.setState({
        logSettingDetail: logSettingDetail
      });
      this.SetErrorState();
      var state = this.state;
      state.formModified = true;
      this.setState({
        state: state
      });
    }
  }, {
    key: "addUpdateLogSetting",
    value: function addUpdateLogSetting(event) {
      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.setState({
        triedToSubmit: true
      });

      if (state.error.mailToAddress) {
        return;
      }

      if (state.formModified) {
        var logSettingDetail = this.state.logSettingDetail;

        if (props.logTypeSettingId !== "") {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* logSettings */ "b"].updateLogSetting(logSettingDetail, function () {
            _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigUpdated"));
            props.Collapse(event);
          }, function () {
            _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigUpdateError"));
          }));
        } else {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* logSettings */ "b"].addLogSetting(logSettingDetail, function () {
            _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigAdded"));
            props.Collapse(event);
          }, function () {
            _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigAddError"));
          }));
        }
      } else {
        props.Collapse(event);
      }
    }
  }, {
    key: "deleteLogSetting",
    value: function deleteLogSetting(event) {
      var props = this.props;

      if (props.logTypeSettingId !== "") {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.confirm(localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigDeletedWarning"), localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("yes"), localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("no"), function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* logSettings */ "b"].deleteLogSetting({
            LogTypeConfigId: props.logTypeSettingId
          }, function () {
            _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigDeleted"));
            props.Collapse(event);
          }, function () {
            _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("DeleteError"));
          }));
        }, function () {
          _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notify(localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigDeleteCancelled"));
        });
      } else {
        _utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigDeleteInconsistency"));
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var portalList = this.props.portalList !== undefined && this.props.portalList.map(function (log) {
        return {
          label: log.label,
          value: log.value.toString()
        };
      });
      var columnOne =
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "editor-container left-column"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "title-row divider"
      }, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("Settings")),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "status-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "left",
        title: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plIsActive.Help")
      }, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plIsActive")),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "right"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
        value: this.getEnabledStatus("Logging"),
        onChange: this.OnCheckboxChanged.bind(this, "LoggingIsActive"),
        onText: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
        offText: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff")
      }))),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "editor-row divider",
        title: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plLogTypeKey.Help")
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("label", null, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plLogTypeKey"), " "),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
        enabled: this.getEnabledStatus("Logging"),
        options: this.props.logTypeList,
        value: this.getValue("LogType"),
        onSelect: this.onDropDownChange.bind(this, "LogTypeKey"),
        style: {
          width: 100 + "%",
          float: "left"
        }
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "editor-row divider",
        title: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plLogTypePortalID.Help")
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("label", null, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plLogTypePortalID"), " "),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
        enabled: this.getEnabledStatus("Logging"),
        options: portalList,
        value: this.getValue("Website"),
        onSelect: this.onDropDownChange.bind(this, "LogTypePortalID"),
        style: {
          width: 100 + "%",
          float: "left"
        }
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "editor-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("label", null, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plKeepMostRecent"), " "),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
        enabled: this.getEnabledStatus("Logging"),
        options: this.props.keepMostRecentOptions,
        value: this.getValue("Recent"),
        onSelect: this.onDropDownChange.bind(this, "KeepMostRecent"),
        style: {
          width: 100 + "%",
          float: "left"
        }
      })));
      var columnTwo =
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "editor-container"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "title-row divider"
      }, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("EmailSettings")),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "status-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "left",
        title: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plEmailNotificationStatus.Help")
      }, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plEmailNotificationStatus")),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "right"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Switch"], {
        value: this.getEnabledStatus("EmailNotification"),
        onChange: this.OnCheckboxChanged.bind(this, "EmailNotificationIsActive"),
        onText: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOn"),
        offText: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("SwitchOff")
      }))),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "editor-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("label", null, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plThreshold")),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
        enabled: this.getEnabledStatus("EmailNotification"),
        options: this.props.thresholdsOptions,
        value: this.getValue("Threshold"),
        onSelect: this.onDropDownChange.bind(this, "NotificationThreshold"),
        style: {
          width: 40 + "%",
          float: "left"
        }
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "text-section"
      }, "in"),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
        enabled: this.getEnabledStatus("EmailNotification"),
        options: this.props.notificationTimesOptions,
        value: this.getValue("ThresholdNotificationTime"),
        onSelect: this.onDropDownChange.bind(this, "NotificationThresholdTime"),
        style: {
          width: 25 + "%",
          float: "left"
        }
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "text-section"
      }, "\xA0 "),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Dropdown"], {
        enabled: this.getEnabledStatus("EmailNotification"),
        options: this.props.notificationTimeTypesOptions,
        value: this.getValue("ThresholdNotificationTimeType"),
        onSelect: this.onDropDownChange.bind(this, "NotificationThresholdTimeType"),
        style: {
          width: 25 + "%",
          float: "left"
        }
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: "editor-row divider",
        title: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plMailToAddress.Help")
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("label", null, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("plMailToAddress"), " *"),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SingleLineInputWithError"], {
        error: this.state.error.mailToAddress && this.state.triedToSubmit,
        errorMessage: localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("MailToAddress.Message"),
        enabled: this.getEnabledStatus("EmailNotification"),
        value: this.getValue("MailToAddress"),
        onChange: this.onTextChange.bind(this, "MailToAddress")
      })));
      var children = [];
      children.push(columnOne);
      children.push(columnTwo);
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "log-setting-editor"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridSystem"], {
          numberOfColumns: 2
        }, children),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "buttons-box"
        }, this.props.logTypeSettingId !== "" &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
          type: "secondary",
          onClick: this.deleteLogSetting.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigBtnDelete")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
          type: "secondary",
          onClick: this.props.Collapse.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigBtnCancel")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Button"], {
          type: "primary",
          onClick: this.addUpdateLogSetting.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].get("ConfigBtnSave"))))
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

  return LogSettingEditor;
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

LogSettingEditor.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func.isRequired,
  logTypeList: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  logSettingDetail: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.object,
  portalList: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  logTypeSettingId: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  Collapse: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func,
  keepMostRecentOptions: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  thresholdsOptions: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  notificationTimesOptions: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  notificationTimeTypesOptions: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired
};

function mapStateToProps(state) {
  return {
    logSettingDetail: state.logSettings.logSettingDetail
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(LogSettingEditor);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(LogSettingEditor, "LogSettingEditor", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\LogSettings\\LogSettingEditor\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\LogSettings\\LogSettingEditor\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\LogSettings\\LogSettingEditor\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 19 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\constants\\actionTypes\\pagination.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\constants\\actionTypes\\pagination.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 20 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();

var paginationActionTypes = {
  SELECT_PANEL: "SELECT_PANEL"
};
var _default = paginationActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActionTypes, "paginationActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\constants\\actionTypes\\visiblePanel.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\constants\\actionTypes\\visiblePanel.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 21 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();

var logSettingsActionTypes = {
  RETRIEVED_LOGTYPE_OPTIONS: "RETRIEVED_LOGTYPE_OPTIONS",
  RETRIEVED_PORTAL_OPTIONS: "RETRIEVED_PORTAL_OPTIONS",
  RETRIEVED_KEEPMOSTRECENT_OPTIONS: "RETRIEVED_KEEPMOSTRECENT_OPTIONS",
  RETRIEVED_OCCURRENCE_OPTIONS: "RETRIEVED_OCCURRENCE_OPTIONS",
  RETRIEVED_LOGSETTING_LIST: "RETRIEVED_LOGSETTING_LIST",
  RETRIEVED_LOGSETTING_BY_ID: "RETRIEVED_LOGSETTING_BY_ID",
  RETRIEVED_LATEST_LOGSETTING: "RETRIEVED_LATEST_LOGSETTING",
  UPDATED_LOGSETTING: "UPDATED_LOGSETTING",
  ADDED_LOGSETTING: "ADDED_LOGSETTING",
  DELETED_LOGSETTING: "DELETED_LOGSETTING"
};
var _default = logSettingsActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(logSettingsActionTypes, "logSettingsActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\constants\\actionTypes\\logSettings.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\constants\\actionTypes\\logSettings.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 22 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();

var logActionTypes = {
  RETRIEVED_LOG_LIST: "RETRIEVED_LOG_LIST",
  RETRIEVED_PORTAL_LIST: "RETRIEVED_PORTAL_LIST",
  RETRIEVED_LOGTYPE_LIST: "RETRIEVED_LOGTYPE_LIST",
  DELETED_LOG_ITEMS: "DELETED_LOG_ITEMS",
  EMAILED_LOG_ITEMS: "EMAILED_LOG_ITEMS",
  SELECTED_ROW: "SELECTED_ROW",
  DESELECTED_ROW: "DESELECTED_ROW",
  SELECTED_ALL: "SELECTED_ALL",
  DESELECTED_ALL: "DESELECTED_ALL",
  CLEARED_LOG: "CLEARED_LOG"
};
var _default = logActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(logActionTypes, "logActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\constants\\actionTypes\\log.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\constants\\actionTypes\\log.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 23 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();


var paginationActions = {
  loadTab: function loadTab(index) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* pagination */ "c"].LOAD_TAB_DATA,
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(paginationActions, "paginationActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\actions\\pagination.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\actions\\pagination.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 24 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();


var visiblePanelActions = {
  selectPanel: function selectPanel(panel, selectedPageVisibleIndex) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* visiblePanel */ "d"].SELECT_PANEL,
        payload: {
          selectedPage: panel,
          selectedPageVisibleIndex: selectedPageVisibleIndex
        }
      });
    };
  }
};
var _default = visiblePanelActions;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(visiblePanelActions, "visiblePanelActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\actions\\visiblePanel.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\actions\\visiblePanel.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 25 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
/* harmony import */ var _services_applicationService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(9);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();



var logSettingsActions = {
  getKeepMostRecentOptions: function getKeepMostRecentOptions(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getKeepMostRecentOptions(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].RETRIEVED_KEEPMOSTRECENT_OPTIONS,
          data: {
            keepMostRecent: data.Results
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getOccurrenceOptions: function getOccurrenceOptions(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getOccurrenceOptions(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].RETRIEVED_OCCURRENCE_OPTIONS,
          data: {
            thresholds: data.Results.Thresholds,
            notificationTimes: data.Results.NotificationTimes,
            notificationTimeTypes: data.Results.NotificationTimeTypes
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getLogSettings: function getLogSettings(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getLogSettings(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].RETRIEVED_LOGSETTING_LIST,
          data: {
            logSettingList: data.Results
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getLogSettingById: function getLogSettingById(parameters, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getLogSettingById(parameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].RETRIEVED_LOGSETTING_BY_ID,
          data: {
            logSettingDetail: data
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  updateLogSetting: function updateLogSetting(payload, callback, failureCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].updateLogSetting(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].UPDATED_LOGSETTING,
          data: {
            logSettingDetail: data
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
  addLogSetting: function addLogSetting(payload, callback, failureCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].addLogSetting(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].ADDED_LOGSETTING,
          data: {
            logSettingDetail: data
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
  deleteLogSetting: function deleteLogSetting(payload, callback, failureCallback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].deleteLogSetting(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].DELETED_LOGSETTING,
          data: {
            Success: data.Success,
            LogSettingId: data.LogSettingId
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
  }
};
var _default = logSettingsActions;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(logSettingsActions, "logSettingsActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\actions\\logSettings.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\actions\\logSettings.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 26 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
/* harmony import */ var _services_applicationService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(9);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();



var logActions = {
  getLogList: function getLogList(searchParameters, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getLogList(searchParameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].RETRIEVED_LOG_LIST,
          data: {
            logList: data.Results,
            selectedRowIds: [],
            excludedRowIds: data.Results.map(function (row) {
              return row.LogGUID;
            }),
            totalCount: data.TotalResults
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getPortalList: function getPortalList(addAll, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getPortalList(addAll, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].RETRIEVED_PORTAL_LIST,
          data: {
            portalList: data.Results,
            totalCount: data.TotalResults
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  getLogTypeList: function getLogTypeList(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getLogTypes(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].RETRIEVED_LOGTYPE_LIST,
          data: {
            logTypeList: data.Results,
            totalCount: data.TotalResults
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  deleteLogItems: function deleteLogItems(payload, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].deleteLogItems(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].DELETED_LOG_ITEMS,
          data: {
            selectedRowIds: [],
            excludedRowIds: []
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  clearLog: function clearLog(callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].clearLog(function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].CLEARED_LOG,
          data: {
            selectedRowIds: [],
            excludedRowIds: []
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  emailLogItems: function emailLogItems(payload, callback) {
    return function (dispatch) {
      payload.Email = payload.Email.split(/[ ,;]+/).filter(function (v) {
        return v.trim().length > 0;
      }).join();
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].emailLogItems(payload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].EMAILED_LOG_ITEMS,
          data: {
            logList: data.Results,
            totalCount: data.TotalResults
          }
        });

        if (callback) {
          callback(data);
        }
      });
    };
  },
  selectRow: function selectRow(rowId) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].SELECTED_ROW,
        data: {
          rowId: rowId
        }
      });
    };
  },
  deselectRow: function deselectRow(rowId) {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].DESELECTED_ROW,
        data: {
          rowId: rowId
        }
      });
    };
  },
  selectAll: function selectAll() {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].SELECTED_ALL,
        data: {}
      });
    };
  },
  deselectAll: function deselectAll() {
    return function (dispatch) {
      dispatch({
        type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].DESELECTED_ALL,
        data: {}
      });
    };
  }
};
var _default = logActions;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(logActions, "logActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\actions\\log.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\actions\\log.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 27 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 28 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return configureStore; });
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(14);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(16);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_thunk__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(29);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _reducers_rootReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(30);
/* harmony import */ var _containers_DevTools__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(35);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(IS_PRODUCTION, "IS_PRODUCTION", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\store\\configureStore.js");
  reactHotLoader.register(configureStore, "configureStore", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\store\\configureStore.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 29 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 30 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(14);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _paginationReducer__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(31);
/* harmony import */ var _visiblePanelReducer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(32);
/* harmony import */ var _logReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(33);
/* harmony import */ var _logSettingsReducer__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(34);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();






var rootReducer = Object(redux__WEBPACK_IMPORTED_MODULE_0__["combineReducers"])({
  pagination: _paginationReducer__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"],
  visiblePanel: _visiblePanelReducer__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"],
  log: _logReducer__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"],
  logSettings: _logSettingsReducer__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"]
});
var _default = rootReducer;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(rootReducer, "rootReducer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducers\\rootReducer.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducers\\rootReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 31 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return pagination; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* pagination */ "c"].LOAD_TAB_DATA:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        tabIndex: action.payload.index
      });

    default:
      return _objectSpread2({}, state);
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(pagination, "pagination", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducers\\paginationReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 32 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return visiblePanel; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* visiblePanel */ "d"].SELECT_PANEL:
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(visiblePanel, "visiblePanel", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducers\\visiblePanelReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 33 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return logList; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }


function logList() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    logList: [],
    portalList: [],
    logTypeList: [],
    selectedRowIds: [],
    excludedRowIds: [],
    totalCount: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].RETRIEVED_LOG_LIST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        logList: action.data.logList,
        selectedRowIds: action.data.selectedRowIds,
        excludedRowIds: action.data.excludedRowIds,
        totalCount: action.data.totalCount
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].RETRIEVED_PORTAL_LIST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        portalList: action.data.portalList
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].RETRIEVED_LOGTYPE_LIST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        logTypeList: action.data.logTypeList
      });

    /*eslint-disable eqeqeq*/

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].SELECTED_ROW:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        selectedRowIds: state.selectedRowIds.concat(action.data.rowId),
        excludedRowIds: state.excludedRowIds.filter(function (id) {
          return id != action.data.rowId;
        })
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].DESELECTED_ROW:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        excludedRowIds: state.excludedRowIds.concat(action.data.rowId),
        selectedRowIds: state.selectedRowIds.filter(function (id) {
          return id != action.data.rowId;
        })
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].SELECTED_ALL:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        selectedRowIds: state.selectedRowIds.concat(state.excludedRowIds),
        excludedRowIds: []
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].DESELECTED_ALL:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        selectedRowIds: [],
        excludedRowIds: state.excludedRowIds.concat(state.selectedRowIds)
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].DELETED_LOG_ITEMS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        selectedRowIds: action.data.selectedRowIds,
        excludedRowIds: action.data.excludedRowIds
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* log */ "a"].CLEARED_LOG:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        selectedRowIds: action.data.selectedRowIds,
        excludedRowIds: action.data.excludedRowIds
      });

    default:
      return _objectSpread2({}, state);
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(logList, "logList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducers\\logReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 34 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return logSettings; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
/* harmony import */ var _reducerHelpers__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(11);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



function logSettings() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    logSettingList: [],
    keepMostRecentOptions: [],
    thresholdsOptions: [],
    notificationTimesOptions: [],
    notificationTimeTypesOptions: [],
    totalCount: 0,
    logSettingDetail: {}
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].RETRIEVED_KEEPMOSTRECENT_OPTIONS:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        keepMostRecentOptions: Object(_reducerHelpers__WEBPACK_IMPORTED_MODULE_1__[/* createKeepMostRecentOptions */ "a"])(action.data.keepMostRecent)
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].RETRIEVED_OCCURRENCE_OPTIONS:
      {
        var occurrenceOptions = Object(_reducerHelpers__WEBPACK_IMPORTED_MODULE_1__[/* createOccurrenceOptions */ "c"])(action.data);
        return _objectSpread2(_objectSpread2({}, state), {}, {
          thresholdsOptions: occurrenceOptions.thresholdsOptions,
          notificationTimesOptions: occurrenceOptions.notificationTimesOptions,
          notificationTimeTypesOptions: occurrenceOptions.notificationTimeTypesOptions
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].RETRIEVED_LOGSETTING_LIST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        logSettingList: action.data.logSettingList
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].RETRIEVED_LOGSETTING_BY_ID:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        logSettingDetail: action.data.logSettingDetail
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].UPDATED_LOGSETTING:
      {
        var logSettingList = _extends([], state.logSettingList);

        return _objectSpread2(_objectSpread2({}, state), {}, {
          logSettingList: Object(_reducerHelpers__WEBPACK_IMPORTED_MODULE_1__[/* updateLogSettingList */ "f"])(logSettingList, action.data.logSettingDetail)
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].DELETED_LOGSETTING:
      {
        var _logSettingList = _extends([], state.logSettingList);

        return _objectSpread2(_objectSpread2({}, state), {}, {
          logSettingList: Object(_reducerHelpers__WEBPACK_IMPORTED_MODULE_1__[/* removeLogSetting */ "e"])(_logSettingList, action.data.LogSettingId)
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* logSettings */ "b"].ADDED_LOGSETTING:
      {
        var _logSettingList2 = _extends([], state.logSettingList);

        return _objectSpread2(_objectSpread2({}, state), {}, {
          logSettingList: Object(_reducerHelpers__WEBPACK_IMPORTED_MODULE_1__[/* updateLogSettingList */ "f"])(_logSettingList2, action.data.logSettingDetail)
        });
      }

    default:
      return _objectSpread2({}, state);
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(logSettings, "logSettings", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\reducers\\logSettingsReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 35 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(36);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_devtools__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(37);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(38);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\containers\\DevTools.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 36 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 37 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 38 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 39 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(50);
} else {}

/***/ }),
/* 40 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(10);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(7);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(2);
/* harmony import */ var _AdminLog__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(41);
/* harmony import */ var _LogSettings__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(44);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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











var isHost = false;
var isAdmin = false;
var canViewAdminLogs = false;
var canViewLogSettings = false;

var App =
/*#__PURE__*/
function (_Component) {
  _inherits(App, _Component);

  function App() {
    var _this;

    _classCallCheck(this, App);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(App).call(this));
    isHost = _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.isHost;
    isAdmin = isHost || _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.isAdmin;
    canViewAdminLogs = isAdmin || _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.permissions.ADMIN_LOGS_VIEW;
    canViewLogSettings = isHost || _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.permissions.LOG_SETTINGS_VIEW;
    return _this;
  }

  _createClass(App, [{
    key: "handleSelect",
    value: function handleSelect(index) {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_4__[/* pagination */ "c"].loadTab(index)); //index acts as scopeTypeId
    }
  }, {
    key: "navigateMap",
    value: function navigateMap(page, index, event) {
      event.preventDefault();
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_4__[/* visiblePanel */ "d"].selectPanel(page, index));
    }
  }, {
    key: "render",
    value: function render() {
      var renderTabs = [];
      var tabHeaders = [];

      if (canViewAdminLogs) {
        tabHeaders.push(localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("AdminLogs.Header"));
        renderTabs.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_AdminLog__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"], null));
      }

      if (canViewLogSettings) {
        tabHeaders.push(localization__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("LogSettings.Header"));
        renderTabs.push(
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_LogSettings__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], null));
      }

      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["GridCell"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["PersonaBarPageHeader"], {
          title: "Admin Logs"
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["PersonaBarPageBody"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_2__["DnnTabs"], {
          onSelect: this.handleSelect.bind(this),
          tabHeaders: tabHeaders,
          type: "primary"
        }, renderTabs)))
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
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

App.PropTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func.isRequired,
  selectedPage: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number,
  selectedPageVisibleIndex: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number
};

function mapStateToProps(state) {
  return {
    selectedPage: state.visiblePanel.selectedPage,
    selectedPageVisibleIndex: state.visiblePanel.selectedPageVisibleIndex
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_3__["connect"])(mapStateToProps)(App);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(isHost, "isHost", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\App.jsx");
  reactHotLoader.register(isAdmin, "isAdmin", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\App.jsx");
  reactHotLoader.register(canViewAdminLogs, "canViewAdminLogs", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\App.jsx");
  reactHotLoader.register(canViewLogSettings, "canViewLogSettings", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\App.jsx");
  reactHotLoader.register(App, "App", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\App.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\App.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 41 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(10);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(7);
/* harmony import */ var _LogItemRow__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(42);
/* harmony import */ var _EmailPanel__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(43);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(57);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_7__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(6);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(2);
/* harmony import */ var _reducerHelpers__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(11);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();

function _typeof(obj) { if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") { _typeof = function _typeof(obj) { return typeof obj; }; } else { _typeof = function _typeof(obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; }; } return _typeof(obj); }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

function _possibleConstructorReturn(self, call) { if (call && (_typeof(call) === "object" || typeof call === "function")) { return call; } return _assertThisInitialized(self); }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _getPrototypeOf(o) { _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf : function _getPrototypeOf(o) { return o.__proto__ || Object.getPrototypeOf(o); }; return _getPrototypeOf(o); }

function _inherits(subClass, superClass) { if (typeof superClass !== "function" && superClass !== null) { throw new TypeError("Super expression must either be null or a function"); } subClass.prototype = Object.create(superClass && superClass.prototype, { constructor: { value: subClass, writable: true, configurable: true } }); if (superClass) _setPrototypeOf(subClass, superClass); }

function _setPrototypeOf(o, p) { _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) { o.__proto__ = p; return o; }; return _setPrototypeOf(o, p); }












var pageSizeOptions = [];
var canEdit = false;
var isHost = false;
/*eslint-disable eqeqeq*/

var AdminLogPanelBody =
/*#__PURE__*/
function (_Component) {
  _inherits(AdminLogPanelBody, _Component);

  function AdminLogPanelBody() {
    var _this;

    _classCallCheck(this, AdminLogPanelBody);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(AdminLogPanelBody).call(this));
    _this.state = {
      logList: [],
      portalList: [],
      logTypeList: [],
      allRowIds: [],
      emailPanelOpen: false,
      currentPortal: "",
      currentPortalId: "-2",
      currentLogType: "",
      currentLogTypeKey: "",
      pageIndex: 0,
      pageSize: 10,
      selectedRowIds: [],
      excludedRowIds: [],
      totalCount: 0
    };
    canEdit = _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.isHost || _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.permissions.ADMIN_LOGS_EDIT;
    isHost = _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.isHost;
    return _this;
  }

  _createClass(AdminLogPanelBody, [{
    key: "componentWillMount",
    value: function componentWillMount() {
      var _this2 = this;

      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getPortalList(_utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.isHost, function (dataPortal) {
        var portalList = _extends([], dataPortal.Results);

        var currentPortalId = portalList[0].PortalID;
        var currentPortal = portalList[0].PortalName;

        _this2.setState({
          portalList: portalList,
          currentPortalId: currentPortalId,
          currentPortal: currentPortal
        }, function () {
          _this2.getLogTypes();
        });
      }));
      pageSizeOptions = [];
      pageSizeOptions.push({
        "value": 10,
        "label": "10 entries per page"
      });
      pageSizeOptions.push({
        "value": 25,
        "label": "25 entries per page"
      });
      pageSizeOptions.push({
        "value": 50,
        "label": "50 entries per page"
      });
      pageSizeOptions.push({
        "value": 100,
        "label": "100 entries per page"
      });
      pageSizeOptions.push({
        "value": 250,
        "label": "250 entries per page"
      });
    }
  }, {
    key: "getLogTypes",
    value: function getLogTypes() {
      var _this3 = this;

      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getLogTypeList(function (dataLog) {
        var logTypeList = _extends([], dataLog.Results);

        var currentLogType = logTypeList[0].LogTypeFriendlyName;
        var currentLogTypeKey = logTypeList[0].LogTypeKey;

        _this3.setState({
          logTypeList: logTypeList,
          currentLogType: currentLogType,
          currentLogTypeKey: currentLogTypeKey
        }, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getLogList(_this3.getNextPage()));
        });
      }));
    }
  }, {
    key: "getNextPage",
    value: function getNextPage() {
      var state = this.state;

      if (state.currentPortalId === -1 || state.currentPortalId === "-1") {
        return {
          pageIndex: state.pageIndex || 0,
          pageSize: state.pageSize,
          logType: state.currentLogTypeKey
        };
      } else {
        return {
          portalId: state.currentPortalId,
          pageIndex: state.pageIndex || 0,
          pageSize: state.pageSize,
          logType: state.currentLogTypeKey
        };
      }
    }
  }, {
    key: "onSelectAll",
    value: function onSelectAll() {
      var props = this.props;

      if (props.excludedRowIds.length == 0) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].deselectAll());
      } else {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].selectAll());
      }
    }
  }, {
    key: "onClearLog",
    value: function onClearLog() {
      var _this4 = this;

      var props = this.props;
      _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].utilities.confirm(localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("ClearLog"), localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("yes"), localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("no"), function () {
        var getNextPageParam = _this4.getNextPage();

        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].clearLog(function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getLogList(getNextPageParam));
        }));
      });
    }
  }, {
    key: "onDeleteLogItems",
    value: function onDeleteLogItems() {
      var _this5 = this;

      var props = this.props,
          state = this.state;

      if (props.selectedRowIds.length > 0) {
        _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].utilities.confirm(localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("LogDeleteWarning"), localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("yes"), localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("no"), function () {
          if (props.excludedRowIds.length == 0 && _this5.isLastPage()) {
            _this5.setState({
              pageIndex: state.pageIndex > 0 ? state.pageIndex - 1 : 0
            }, function () {
              var getNextPageParam = _this5.getNextPage();

              props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].deleteLogItems(props.selectedRowIds, function () {
                props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getLogList(getNextPageParam));
              }));
            });
          } else {
            var getNextPageParam = _this5.getNextPage();

            props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].deleteLogItems(props.selectedRowIds, function () {
              props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getLogList(getNextPageParam));
            }));
          }
        });
      } else {
        _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("SelectException"));
      }
    }
  }, {
    key: "toggleEmailPanel",
    value: function toggleEmailPanel() {
      this.setState({
        emailPanelOpen: !this.state.emailPanelOpen
      });
    }
  }, {
    key: "onSelectPortal",
    value: function onSelectPortal(option) {
      var _this6 = this;

      var props = this.props,
          state = this.state;

      if (option.value !== state.currentPortalId) {
        this.setState({
          currentPortal: option.label,
          currentPortalId: option.value,
          pageIndex: 0
        }, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getLogList(_this6.getNextPage()));
        });
      }
    }
  }, {
    key: "onSelectLogType",
    value: function onSelectLogType(option) {
      var _this7 = this;

      var props = this.props,
          state = this.state;

      if (option.value !== state.currentLogTypeKey) {
        this.setState({
          currentLogType: option.label,
          currentLogTypeKey: option.value,
          pageIndex: 0
        }, function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getLogList(_this7.getNextPage()));
        });
      }
    }
  }, {
    key: "renderLogListHeader",
    value: function renderLogListHeader() {
      var props = this.props;

      if (!props.excludedRowIds) {
        return;
      }

      var tableFields = [{
        "name": "",
        "id": "LogTypeCSSClass"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("Date"),
        "id": "LogCreateDate"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("Type"),
        "id": "LogTypeFriendlyName"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("Username"),
        "id": "LogUserName"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("Portal"),
        "id": "LogPortalName"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("Summary"),
        "id": "Summary"
      }];
      var tableHeaders = tableFields.map(function (field) {
        var className = "logHeader logHeader-" + field.id;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
            key: field.id,
            className: className
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, field.name, "\xA0 "))
        );
      });
      var isDeselectState = props.excludedRowIds.length == 0 && props.excludedRowIds.length || !props.excludedRowIds.length == 0 && props.selectedRowIds.length;
      var checkboxClassName = "checkbox" + (isDeselectState ? " deselect-state" : "");
      tableHeaders.unshift(
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        key: "selector" + "999999",
        className: "logHeader logHeader-Checkbox",
        "data-index": "0"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
        className: checkboxClassName
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Checkbox"], {
        value: props.excludedRowIds.length === 0 && props.selectedRowIds.length > 0 || isDeselectState,
        onChange: this.onSelectAll.bind(this)
      }))));
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "logHeader-wrapper"
        }, tableHeaders)
      );
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "renderedLogList",
    value: function renderedLogList() {
      var _this8 = this;

      var props = this.props;
      return props.logList.map(function (term, index) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_LogItemRow__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
            cssClass: term.LogTypeCSSClass,
            logId: term.LogGUID,
            allRowIds: _this8.props.logList.map(function (row) {
              return row.LogGUID;
            }),
            typeName: term.LogTypeFriendlyName,
            createDate: term.LogCreateDate,
            userName: term.LogUserName,
            portalName: term.LogPortalName,
            summary: term.Summary,
            index: index,
            key: "logTerm-" + index,
            closeOnClick: true
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
            className: "log-detail",
            dangerouslySetInnerHTML: {
              __html: term.LogProperties
            }
          }))
        );
      });
    }
  }, {
    key: "isLastPage",
    value: function isLastPage() {
      var props = this.props,
          state = this.state;
      var total = Math.ceil(props.totalCount / state.pageSize);
      var current = state.pageIndex * state.pageSize / state.pageSize + 1;
      return total === current ? true : false;
    }
  }, {
    key: "onPageChange",
    value: function onPageChange(currentPage, pageSize) {
      var _this9 = this;

      var state = this.state,
          props = this.props;

      if (pageSize !== undefined && state.pageSize !== pageSize) {
        state.pageSize = pageSize;
      }

      state.pageIndex = currentPage;
      this.setState({
        state: state
      }, function () {
        var getNextPageParam = _this9.getNextPage();

        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getLogList(getNextPageParam));
      });
    }
  }, {
    key: "renderPager",
    value: function renderPager() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "logPager"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Pager"], {
          showStartEndButtons: false,
          showPageSizeOptions: true,
          showPageInfo: false,
          numericCounters: 4,
          pageSize: state.pageSize,
          totalRecords: props.totalCount,
          onPageChanged: this.onPageChange.bind(this),
          pageSizeDropDownWithoutBorder: true,
          pageSizeOptionText: "{0} results per page",
          summaryText: "Showing {0}-{1} of {2} results",
          culture: _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].utilities.getCulture()
        }))
      );
    }
  }, {
    key: "renderedLogLegend",
    value: function renderedLogLegend() {
      var legendItems = [{
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("ExceptionCode"),
        "id": "Exception"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("ItemCreatedCode"),
        "id": "ItemCreated"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("ItemUpdatedCode"),
        "id": "ItemUpdated"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("ItemDeletedCode"),
        "id": "ItemDeleted"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("SuccessCode"),
        "id": "OperationSuccess"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("FailureCode"),
        "id": "OperationFailure"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("AdminOpCode"),
        "id": "GeneralAdminOperation"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("AdminAlertCode"),
        "id": "AdminAlert"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("HostAlertCode"),
        "id": "HostAlert"
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("SecurityException"),
        "id": "SecurityException"
      }];
      var legend = legendItems.map(function (item) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
            key: item.id,
            className: "logLegend-item"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
            className: item.id
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null)),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", null,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, item.name)))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "logLegend-wrapper"
        }, legend)
      );
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var portalOptions = Object(_reducerHelpers__WEBPACK_IMPORTED_MODULE_10__[/* createPortalOptions */ "d"])(state.portalList);
      var logTypeOptions = Object(_reducerHelpers__WEBPACK_IMPORTED_MODULE_10__[/* createLogTypeOptions */ "b"])(state.logTypeList);
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          style: {
            margin: "0 20px",
            float: "left"
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "toolbar"
        }, state.portalList.length > 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "adminlogs-filter-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Dropdown"], {
          value: state.currentPortalId,
          style: {
            width: "100%"
          },
          options: portalOptions,
          withBorder: false,
          onSelect: this.onSelectPortal.bind(this)
        })), state.logTypeList.length > 0 &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "adminlogs-filter-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Dropdown"], {
          value: state.currentLogTypeKey,
          style: {
            width: "100%"
          },
          options: logTypeOptions,
          withBorder: false,
          onSelect: this.onSelectLogType.bind(this)
        })), (canEdit || _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.isAdmin) &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "toolbar-button toolbar-button-actions",
          style: {
            width: "15%",
            paddingRight: 0,
            textAlign: "right"
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          onClick: this.toggleEmailPanel.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["TextOverflowWrapper"], {
          text: localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("btnEmail"),
          maxWidth: 100,
          enabled: canEdit || _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.isAdmin
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "collapsible-content"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_EmailPanel__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], {
          fixedHeight: 450,
          isOpened: state.emailPanelOpen,
          logIds: props.selectedRowIds,
          onCloseEmailPanel: this.toggleEmailPanel.bind(this)
        }))), isHost &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "toolbar-button toolbar-button-actions",
          onClick: this.onDeleteLogItems.bind(this),
          style: {
            width: "18%"
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["TextOverflowWrapper"], {
          text: localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("btnDelete"),
          maxWidth: 115
        })), isHost &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "toolbar-button toolbar-button-actions",
          onClick: this.onClearLog.bind(this),
          style: {
            borderLeft: "none",
            width: "15%"
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["TextOverflowWrapper"], {
          text: localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("btnClear"),
          maxWidth: 90
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "logContainer"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "logContainerBox"
        }, this.renderLogListHeader(), this.renderedLogList())), this.renderPager(), this.renderedLogLegend())
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

  return AdminLogPanelBody;
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

AdminLogPanelBody.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number,
  logList: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  logTypeList: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  portalList: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  selectedRowIds: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  allRowIds: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  excludedRowIds: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  totalCount: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number
};

function mapStateToProps(state) {
  return {
    logList: state.log.logList,
    logTypeList: state.log.logTypeList,
    portalList: state.log.portalList,
    tabIndex: state.pagination.tabIndex,
    selectedRowIds: state.log.selectedRowIds,
    excludedRowIds: state.log.excludedRowIds,
    totalCount: state.log.totalCount
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(AdminLogPanelBody);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(pageSizeOptions, "pageSizeOptions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\index.jsx");
  reactHotLoader.register(canEdit, "canEdit", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\index.jsx");
  reactHotLoader.register(isHost, "isHost", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\index.jsx");
  reactHotLoader.register(AdminLogPanelBody, "AdminLogPanelBody", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 42 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(10);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(53);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(7);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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







/*eslint-disable eqeqeq*/

var LogItemRow =
/*#__PURE__*/
function (_Component) {
  _inherits(LogItemRow, _Component);

  function LogItemRow() {
    var _this;

    _classCallCheck(this, LogItemRow);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(LogItemRow).call(this));
    _this.state = {
      collapsed: true,
      collapsedClass: true,
      repainting: false
    };
    _this.timeout = 0;
    _this.handleClick = _this.handleClick.bind(_assertThisInitialized(_this));
    return _this;
  }

  _createClass(LogItemRow, [{
    key: "componentWillReceiveProps",
    value: function componentWillReceiveProps() {
      this.setState({});
    }
  }, {
    key: "componentDidMount",
    value: function componentDidMount() {
      document.addEventListener("click", this.handleClick);
      this._isMounted = true;
    }
  }, {
    key: "componentWillUnmount",
    value: function componentWillUnmount() {
      document.removeEventListener("click", this.handleClick);
      this._isMounted = false;
    }
  }, {
    key: "handleClick",
    value: function handleClick(event) {
      // Note: this workaround is needed in IE. The remove event listener in the componentWillUnmount is called
      // before the handleClick handler is called, but in spite of that, the handleClick is executed. To avoid
      // the "findDOMNode was called on an unmounted component." error we need to check if the component is mounted before execute this code
      if (!this._isMounted) {
        return;
      }

      if (!this.node.contains(event.target) && typeof event.target.className == "string" && event.target.className.indexOf("do-not-close") == -1) {
        this.timeout = 475;
        this.collapse();
      } else {
        this.timeout = 0;
      }
    }
  }, {
    key: "uncollapse",
    value: function uncollapse() {
      var _this2 = this;

      setTimeout(function () {
        _this2.setState({
          collapsed: false
        });
      }, this.timeout);
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
    key: "onSelectRow",
    value: function onSelectRow(rowId) {
      var props = this.props;

      if (this.isRowSelected(rowId)) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* log */ "a"].deselectRow(rowId));
      } else {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* log */ "a"].selectRow(rowId));
      }
    }
  }, {
    key: "isRowSelected",
    value: function isRowSelected() {
      var props = this.props;
      return props.selectedRowIds.some(function (id) {
        return id === props.logId;
      });
    }
  }, {
    key: "render",
    value: function render() {
      var _this3 = this;

      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          ref: function ref(node) {
            return _this3.node = node;
          },
          className: "collapsible-logitemdetail " + state.collapsed + (props.className ? " " + props.className : "")
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "collapsible-logitemdetail-header " + state.collapsed
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-header"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          "data-index": "0",
          className: "term-label-checkbox"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Checkbox"], {
          value: this.isRowSelected(),
          onChange: this.onSelectRow.bind(this, this.props.logId)
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-cssclass",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "logItemIndicator"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", {
          className: this.props.cssClass
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-createdate",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, this.props.createDate))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-typename",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, this.props.typeName))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-username",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, this.props.userName, "\xA0 "))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-portalname",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, this.props.portalName, "\xA0 "))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-summary",
          onClick: this.toggle.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "term-label-wrapper"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, this.props.summary, "\xA0 "))))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Collapsible"], {
          className: "logitem-collapsible",
          isOpened: !this.state.collapsed,
          style: {
            float: "left",
            width: "100%"
          }
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

  return LogItemRow;
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

LogItemRow.propTypes = {
  allRowIds: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  selectedRowIds: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  excludedRowIds: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  cssClass: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  logId: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  typeName: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  createDate: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  userName: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  portalName: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  summary: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  label: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.node,
  children: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.node,
  disabled: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.bool,
  className: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string
};

function mapStateToProps(state) {
  return {
    selectedRowIds: state.log.selectedRowIds,
    excludedRowIds: state.log.excludedRowIds
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(LogItemRow);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(LogItemRow, "LogItemRow", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\LogItemRow\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\LogItemRow\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\LogItemRow\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 43 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(10);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(55);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(7);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(2);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(6);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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










var EmailPanel =
/*#__PURE__*/
function (_Component) {
  _inherits(EmailPanel, _Component);

  function EmailPanel() {
    var _this;

    _classCallCheck(this, EmailPanel);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(EmailPanel).call(this));
    _this.state = {
      emailRequest: {
        Email: "",
        Subject: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LogEntryDefaultSubject"),
        Message: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("LogEntryDefaultMsg"),
        LogIds: []
      },
      error: {
        email: false
      }
    };
    return _this;
  }

  _createClass(EmailPanel, [{
    key: "componentWillMount",
    value: function componentWillMount() {
      this.SetErrorState();
    }
  }, {
    key: "SetErrorState",
    value: function SetErrorState() {
      var state = this.state;
      var emailRequest = this.state.emailRequest;

      if (!this.validateEmail(emailRequest.Email)) {
        state.error["email"] = true;
      } else {
        state.error["email"] = false;
      }

      state.triedToSubmit = false;
      this.setState({
        state: state
      });
    }
  }, {
    key: "onSendEmail",
    value: function onSendEmail(event) {
      var _this2 = this;

      event.preventDefault();
      var props = this.props,
          state = this.state;
      var emailRequest = this.state.emailRequest;
      emailRequest["LogIds"] = props.logIds;
      this.setState({
        emailRequest: emailRequest,
        triedToSubmit: true
      });

      if (state.error.email) {
        return;
      }

      if (props.logIds.length <= 0) {
        _utils__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].utilities.notifyError(localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SelectException"));
        return;
      }

      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* log */ "a"].emailLogItems(state.emailRequest, function (data) {
        _utils__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"].utilities.notify(data.ErrorMessage + data.ReturnMessage);

        if (data.Success) {
          _this2.onCloseEmailPanel();
        }
      }));
    }
  }, {
    key: "validateEmail",
    value: function validateEmail(value) {
      var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return value.split(/[ ,;]+/).every(function (v) {
        return re.test(v) || v.trim().length === 0;
      });
    }
  }, {
    key: "onEmailValueChange",
    value: function onEmailValueChange(key, event) {
      var value = event.target.value;
      var emailRequest = this.state.emailRequest;
      emailRequest[key] = value;
      this.setState({
        emailRequest: emailRequest
      });
      this.SetErrorState();
    }
  }, {
    key: "onCloseEmailPanel",
    value: function onCloseEmailPanel() {
      var props = this.props;
      props.onCloseEmailPanel();
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "collapsible-content-email"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Collapsible"], {
          fixedHeight: props.fixedHeight,
          keepCollapsedContent: props.keepCollapsedContent,
          isOpened: props.isOpened
        }, props.fixedHeight &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "emailpanel-content-wrapper",
          style: {
            height: "100%"
          }
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: ""
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Label"], {
          labelType: "inline",
          tooltipMessage: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEmailAddress.Help"),
          label: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plEmailAddress")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          error: state.error.email && state.triedToSubmit,
          inputStyle: {
            marginBottom: "15px"
          },
          errorMessage: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Email.Message"),
          value: state.emailRequest.Email,
          onChange: this.onEmailValueChange.bind(this, "Email")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Label"], {
          labelType: "inline",
          tooltipMessage: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plSubject.Help"),
          label: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("plSubject")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["SingleLineInputWithError"], {
          error: false,
          inputStyle: {
            marginBottom: "15px"
          },
          value: state.emailRequest.Subject,
          onChange: this.onEmailValueChange.bind(this, "Subject")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["InputGroup"], null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Label"], {
          labelType: "inline",
          tooltipMessage: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SendMessage.Help"),
          label: localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("SendMessage")
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["MultiLineInput"], {
          value: state.emailRequest.Message,
          onChange: this.onEmailValueChange.bind(this, "Message")
        })),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "action-buttons"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "secondary",
          onClick: this.onCloseEmailPanel.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("btnCancel")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "primary",
          onClick: this.onSendEmail.bind(this)
        }, localization__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("btnSend")))))), !props.fixedHeight && props.children))
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

  return EmailPanel;
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

EmailPanel.PropTypes = {
  label: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.string,
  fixedHeight: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number,
  collapsibleWidth: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number,
  collapsibleHeight: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number,
  keepCollapsedContent: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.bool,
  scrollAreaStyle: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.object,
  children: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.node,
  isOpened: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.bool,
  logIds: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  onCloseEmailPanel: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func.isRequired
};

function mapStateToProps(state) {
  return {
    logList: state.log.logList
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(EmailPanel);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(EmailPanel, "EmailPanel", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\EmailPanel\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\EmailPanel\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\AdminLog\\EmailPanel\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 44 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(3);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(10);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(7);
/* harmony import */ var _LogSettingRow__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(17);
/* harmony import */ var _LogSettingEditor__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(18);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(63);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_6__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(6);
/* harmony import */ var localization__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(2);
/* harmony import */ var _reducerHelpers__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(11);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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

var LogSettingsPanel =
/*#__PURE__*/
function (_Component) {
  _inherits(LogSettingsPanel, _Component);

  function LogSettingsPanel() {
    var _this;

    _classCallCheck(this, LogSettingsPanel);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(LogSettingsPanel).call(this));
    _this.state = {
      openId: ""
    };
    canEdit = _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.isHost || _utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.permissions.LOG_SETTINGS_EDIT;
    return _this;
  }

  _createClass(LogSettingsPanel, [{
    key: "componentWillMount",
    value: function componentWillMount() {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* logSettings */ "b"].getLogSettings());

      if (canEdit) {
        if (this.props.logTypeList === null || this.props.logTypeList === undefined || this.props.logTypeList.length === 0) {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getLogTypeList());
        }

        if (this.props.portalList === null || this.props.portalList === undefined || this.props.portalList.length === 0) props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* log */ "a"].getPortalList(_utils__WEBPACK_IMPORTED_MODULE_8__[/* default */ "a"].settings.isHost));
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* logSettings */ "b"].getKeepMostRecentOptions());
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_3__[/* logSettings */ "b"].getOccurrenceOptions());
      }
    }
  }, {
    key: "renderHeader",
    value: function renderHeader() {
      var tableFields = [{
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("LogType.Header"),
        "width": 40
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("Portal.Header"),
        "width": 20
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("Active.Header"),
        "width": 15
      }, {
        "name": localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("FileName.Header"),
        "width": 20
      }, {
        "name": "",
        "width": 5
      }];
      var tableHeaders = tableFields.map(function (field) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["GridCell"], {
            key: field.name,
            columnSize: field.width,
            style: {
              fontWeight: "bolder"
            }
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("span", null, field.name, "\xA0 "))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          id: "header-row",
          className: "header-row"
        }, tableHeaders)
      );
    }
  }, {
    key: "uncollapse",
    value: function uncollapse(id) {
      var _this2 = this;

      setTimeout(function () {
        _this2.setState({
          openId: id
        });
      }, 0);
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
      } else {
        this.collapse();
      }
    }
    /* eslint-disable react/no-danger */

  }, {
    key: "renderedLogSettingList",
    value: function renderedLogSettingList(logTypeOptions, portalOptions) {
      var _this3 = this;

      var validLogSettingList = this.props.logSettingList.filter(function (logSetting) {
        return !!logSetting;
      });
      var i = 0;
      return validLogSettingList.map(function (logSetting, index) {
        var id = "row-" + i++;
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_LogSettingRow__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
            typeName: logSetting.LogTypeFriendlyName,
            website: logSetting.LogTypePortalName,
            activeStatus: logSetting.LoggingIsActive ? localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("True") : localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("False"),
            fileName: logSetting.LogFileName,
            logTypeKey: logSetting.LogTypeKey,
            index: index,
            key: "logSetting-" + index,
            closeOnClick: true,
            openId: _this3.state.openId,
            OpenCollapse: _this3.toggle.bind(_this3),
            Collapse: _this3.collapse.bind(_this3),
            id: id,
            readOnly: !canEdit
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_LogSettingEditor__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], {
            logTypeList: logTypeOptions,
            portalList: portalOptions,
            keepMostRecentOptions: _this3.props.keepMostRecentOptions,
            thresholdsOptions: _this3.props.thresholdsOptions,
            notificationTimesOptions: _this3.props.notificationTimesOptions,
            notificationTimeTypesOptions: _this3.props.notificationTimeTypesOptions,
            logTypeSettingId: logSetting.ID,
            Collapse: _this3.collapse.bind(_this3),
            id: id,
            openId: _this3.state.openId
          }))
        );
      });
    }
  }, {
    key: "render",
    value: function render() {
      var opened = this.state.openId === "add";
      var logTypeOptions = Object(_reducerHelpers__WEBPACK_IMPORTED_MODULE_10__[/* createLogTypeOptions */ "b"])(this.props.logTypeList);
      var portalOptions = Object(_reducerHelpers__WEBPACK_IMPORTED_MODULE_10__[/* createPortalOptions */ "d"])(this.props.portalList);
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "log-settings"
        }, canEdit &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "add-setting-row",
          onClick: this.toggle.bind(this, opened ? "" : "add")
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "add-setting-box " + !opened
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "add-icon",
          dangerouslySetInnerHTML: {
            __html: _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_7__["SvgIcons"].AddIcon
          }
        }), " ", localization__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].get("AddContent.Action"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "container"
        }, this.renderHeader(), canEdit &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement("div", {
          className: "add-setting-editor"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_LogSettingRow__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
          typeName: "-",
          website: "-",
          activeStatus: "-",
          fileName: "",
          logTypeKey: "-",
          index: "add",
          key: "logSetting-add",
          closeOnClick: true,
          openId: this.state.openId,
          OpenCollapse: this.toggle.bind(this),
          Collapse: this.collapse.bind(this),
          id: "add",
          visible: opened
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_1___default.a.createElement(_LogSettingEditor__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"], {
          logTypeList: logTypeOptions,
          portalList: portalOptions,
          keepMostRecentOptions: this.props.keepMostRecentOptions,
          thresholdsOptions: this.props.thresholdsOptions,
          notificationTimesOptions: this.props.notificationTimesOptions,
          notificationTimeTypesOptions: this.props.notificationTimeTypesOptions,
          logTypeSettingId: "",
          Collapse: this.collapse.bind(this),
          id: "add",
          openId: this.state.openId
        }))), this.renderedLogSettingList(logTypeOptions, portalOptions))))
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

  return LogSettingsPanel;
}(react__WEBPACK_IMPORTED_MODULE_1__["Component"]);

LogSettingsPanel.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.func.isRequired,
  tabIndex: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.number,
  logTypeList: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  logSettingList: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  portalList: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array,
  logTypeSetting: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.object,
  keepMostRecentOptions: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  thresholdsOptions: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  notificationTimesOptions: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired,
  notificationTimeTypesOptions: prop_types__WEBPACK_IMPORTED_MODULE_0___default.a.array.isRequired
};

function mapStateToProps(state) {
  return {
    logSettingList: state.logSettings.logSettingList,
    logTypeList: state.log.logTypeList,
    portalList: state.log.portalList,
    keepMostRecentOptions: state.logSettings.keepMostRecentOptions,
    thresholdsOptions: state.logSettings.thresholdsOptions,
    notificationTimesOptions: state.logSettings.notificationTimesOptions,
    notificationTimeTypesOptions: state.logSettings.notificationTimeTypesOptions,
    tabIndex: state.pagination.tabIndex
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(LogSettingsPanel);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(canEdit, "canEdit", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\LogSettings\\index.jsx");
  reactHotLoader.register(LogSettingsPanel, "LogSettingsPanel", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\LogSettings\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\LogSettings\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\components\\LogSettings\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 45 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(27);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(10);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _globals_application__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(15);
/* harmony import */ var _store_configureStore__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(28);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(39);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_containers_Root__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
  enterModule && enterModule(module);
})();







var store = Object(_store_configureStore__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"])();
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].dispatch = store.dispatch;
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].init();
var appContainer = document.getElementById("adminlogs-container");
Object(react_dom__WEBPACK_IMPORTED_MODULE_1__["render"])(
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_redux__WEBPACK_IMPORTED_MODULE_2__["Provider"], {
  store: store
},
/*#__PURE__*/
react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_containers_Root__WEBPACK_IMPORTED_MODULE_5___default.a, null)), appContainer);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(store, "store", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\main.jsx");
  reactHotLoader.register(appContainer, "appContainer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\main.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 46 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
function _interopDefault(e){return e&&"object"==typeof e&&"default"in e?e.default:e}Object.defineProperty(exports,"__esModule",{value:!0});var React=_interopDefault(__webpack_require__(0));function AppContainer(e){return React.Children.only(e.children)}var hot_prod=function(){return function(e){return e}},areComponentsEqual=function(e,n){return e===n},setConfig=function(){},cold=function(e){return e},configureComponent=function(){};exports.AppContainer=AppContainer,exports.hot=hot_prod,exports.areComponentsEqual=areComponentsEqual,exports.setConfig=setConfig,exports.cold=cold,exports.configureComponent=configureComponent;


/***/ }),
/* 47 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(48);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 48 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, "", ""]);



/***/ }),
/* 49 */
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
/* 50 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _components_App__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(40);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).enterModule;
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
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "adminlogs-app personaBar-mainContainer"
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
/* harmony default export */ __webpack_exports__["default"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Root, "Root", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\AdminLogs.Web\\src\\containers\\Root.prod.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(1)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(8)(module)))

/***/ }),
/* 51 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(52);

function emptyFunction() {}

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
    instanceOf: getShim,
    node: shim,
    objectOf: getShim,
    oneOf: getShim,
    oneOfType: getShim,
    shape: getShim,
    exact: getShim
  };

  ReactPropTypes.checkPropTypes = emptyFunction;
  ReactPropTypes.PropTypes = ReactPropTypes;

  return ReactPropTypes;
};


/***/ }),
/* 52 */
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
/* 53 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(54);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 54 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.collapsible-logitemdetail {\n  display: table;\n  width: 100%;\n}\n.collapsible-logitemdetail.false {\n  border-top: 2px solid #1E88C3 !important;\n  border-bottom: 2px solid #1E88C3 !important;\n  position: relative;\n  top: -1px;\n}\n.collapsible-logitemdetail:not(:last-child) {\n  border-bottom: 1px solid #ddd;\n}\n.collapsible-logitemdetail .logitem-collapsible > div {\n  float: left;\n  padding-bottom: 25px;\n}\n.collapsible-logitemdetail div.collapsible-logitemdetail-header {\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 0 15px 0;\n  box-sizing: border-box;\n}\n.collapsible-logitemdetail div.collapsible-logitemdetail-header .term-header {\n  cursor: pointer;\n}\n.collapsible-logitemdetail div.collapsible-logitemdetail-header .term-header .term-label-checkbox {\n  float: left;\n  margin: 1px 0 0 10px;\n  width: 30px;\n}\n.collapsible-logitemdetail div.collapsible-logitemdetail-header .term-header .term-label-createdate {\n  width: 130px;\n  float: left;\n}\n.collapsible-logitemdetail div.collapsible-logitemdetail-header .term-header .term-label-typename {\n  width: 140px;\n  float: left;\n}\n.collapsible-logitemdetail div.collapsible-logitemdetail-header .term-header .term-label-username {\n  width: 90px;\n  float: left;\n}\n.collapsible-logitemdetail div.collapsible-logitemdetail-header .term-header .term-label-portalname {\n  width: 110px;\n  float: left;\n}\n.collapsible-logitemdetail div.collapsible-logitemdetail-header .term-header .term-label-summary {\n  width: 220px;\n  float: left;\n}\n.collapsible-logitemdetail div.collapsible-logitemdetail-header .term-header .term-label-wrapper {\n  padding: 0 5px 0 5px;\n  word-wrap: break-word;\n}\n", ""]);



/***/ }),
/* 55 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(56);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 56 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, ".collapsible-content-email {\n  box-shadow: 0 0 20px 0 rgba(0, 0, 0, 0.2);\n}\n.collapsible-content-email .emailpanel-content-wrapper {\n  padding: 20px 30px 20px 30px;\n}\n.collapsible-content-email .emailpanel-content-wrapper .dnn-ui-common-input-group label {\n  font-weight: bolder;\n  float: left;\n}\n.collapsible-content-email .emailpanel-content-wrapper .dnn-ui-common-input-group .dnn-single-line-input-with-error {\n  width: 100%;\n}\n.collapsible-content-email .emailpanel-content-wrapper .action-buttons {\n  text-align: center;\n  margin: 20px 0;\n}\n.collapsible-content-email .emailpanel-content-wrapper .action-buttons button:first-child {\n  margin-right: 15px;\n}\n", ""]);



/***/ }),
/* 57 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(58);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 58 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#adminlogs-container .dnn-persona-bar-page-body {\n  border-bottom: 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs * {\n  box-sizing: border-box;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-detail {\n  margin: 0px 10px 10px 58px;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-detail * {\n  white-space: normal;\n  word-break: break-all;\n  word-wrap: break-word;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-detail pre {\n  white-space: pre-wrap;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logHeader-wrapper {\n  width: 100%;\n  float: left;\n  padding: 15px 0 15px 0;\n  text-transform: uppercase;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logHeader-wrapper:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logHeader-Checkbox {\n  float: left;\n  margin-left: 15px;\n  width: 30px;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logHeader-LogTypeCSSClass {\n  width: 21px;\n  float: left;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logHeader-LogCreateDate {\n  width: 130px;\n  float: left;\n  font-weight: bolder;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logHeader-LogTypeFriendlyName {\n  width: 140px;\n  float: left;\n  font-weight: bolder;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logHeader-LogUserName {\n  width: 90px;\n  float: left;\n  font-weight: bolder;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logHeader-LogPortalName {\n  width: 110px;\n  float: left;\n  font-weight: bolder;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logHeader-Summary {\n  width: 220px;\n  float: left;\n  font-weight: bolder;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .Exception {\n  color: #fff;\n  background-color: #ff1414;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .ItemCreated {\n  color: #fff;\n  background-color: #009900;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .ItemUpdated {\n  color: #fff;\n  background-color: #009999;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .ItemDeleted {\n  color: #000;\n  background-color: #14ffff;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .OperationSuccess {\n  color: #fff;\n  background-color: #999900;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .OperationFailure {\n  color: #fff;\n  background-color: #990000;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .GeneralAdminOperation {\n  color: #fff;\n  background-color: #4d0099;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .AdminAlert {\n  color: #fff;\n  background-color: #148aff;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .HostAlert {\n  color: #fff;\n  background-color: #ff8a14;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .SecurityException {\n  color: #fff;\n  background-color: #000;\n  width: 15px;\n  height: 15px;\n  display: block;\n  float: left;\n  margin: 0px 6px 0 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logContainer {\n  padding: 20px 0;\n  float: left;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logContainerBox {\n  border: solid 1px #C8C8C8;\n  float: left;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .toolbar {\n  border-bottom: solid 1px #C8C8C8;\n  display: block;\n  float: left;\n  width: 100%;\n  padding: 7px 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .toolbar .adminlogs-filter-container:first-child > .dnn-dropdown .collapsible-label {\n  padding-left: 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .toolbar .adminlogs-filter-container {\n  width: 26%;\n  height: 32px;\n  margin-top: 10px;\n  border-right: 1px solid #C8C8C8;\n  float: left;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .toolbar .adminlogs-filter-container .dnn-dropdown .collapsible-toggle {\n  width: auto;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .toolbar .toolbar-button {\n  float: right;\n  border-left: solid 1px #C8C8C8;\n  text-align: center;\n  cursor: pointer;\n  height: 32px;\n  width: 100%;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .toolbar .toolbar-button:hover {\n  color: #0087C6;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .toolbar .toolbar-button:hover .collapsible-content {\n  color: #000;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .toolbar .toolbar-button .collapsible-content {\n  -webkit-box-shadow: 0px 0px 1px 0px rgba(0, 0, 0, 0.2);\n  -moz-box-shadow: 0px 0px 1px 0px rgba(0, 0, 0, 0.2);\n  box-shadow: 0px 0px 1px 0px rgba(0, 0, 0, 0.2);\n  margin-top: 13px;\n  z-index: 1000;\n  background-color: #fff;\n  position: absolute;\n  width: 400px;\n  margin-left: -288px;\n  text-align: left;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .toolbar .toolbar-button-actions {\n  margin-top: 10px;\n  padding-top: 8px;\n  font-weight: bold;\n  color: #4B4E4F;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logPager {\n  margin: 0 0 20px 0;\n  float: left;\n  width: 100%;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logLegend-wrapper {\n  float: left;\n  border-top: solid 1px #C8C8C8;\n  width: 100%;\n  padding: 20px 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .logLegend-wrapper .logLegend-item {\n  float: left;\n  width: 250px;\n  padding: 5px 5px 5px 0px;\n}\n", ""]);



/***/ }),
/* 59 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(60);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 60 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.collapsible-component-log {\n  display: block;\n  float: left;\n  width: 100%;\n  border-bottom: 1px solid #C8C8C8;\n  cursor: auto;\n}\n.collapsible-component-log > div > div {\n  float: left;\n}\n.collapsible-component-log div.collapsible-header-log {\n  color: #6F7273;\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 20px;\n  box-sizing: border-box;\n  cursor: auto;\n}\n.collapsible-component-log div.collapsible-header-log.false {\n  background-color: #FBFCFC;\n}\n.collapsible-component-log div.collapsible-header-log .edit-icon {\n  margin-left: 20px;\n  float: right;\n  cursor: pointer;\n}\n.collapsible-component-log div.collapsible-header-log .edit-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n}\n.collapsible-component-log div.collapsible-header-log .edit-icon.false svg {\n  fill: #1E88C3;\n}\n.collapsible-component-log div.collapsible-header-log .edit-icon.false svg:hover {\n  fill: #4B4E4F;\n}\n", ""]);



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

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 62 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.log-setting-editor {\n  padding: 10px 0;\n  background-color: #FBFCFC;\n  display: block;\n  float: left;\n}\n.log-setting-editor .editor-container {\n  float: left;\n  padding: 10px 20px;\n  width: auto;\n}\n.log-setting-editor .editor-container.left-column {\n  border-right: 1px solid #C8C8C8;\n}\n.log-setting-editor .editor-container .title-row {\n  width: 100%;\n  float: left;\n  font-weight: bold;\n  text-transform: uppercase;\n  margin-bottom: 10px;\n}\n.log-setting-editor .editor-container .status-row {\n  width: 100%;\n  float: left;\n  margin-bottom: 10px;\n}\n.log-setting-editor .editor-container .status-row .left {\n  float: left;\n  width: 50%;\n}\n.log-setting-editor .editor-container .status-row .right {\n  float: right;\n  width: 50%;\n  text-align: right;\n}\n.log-setting-editor .editor-container .status-row .right .checkbox {\n  float: right;\n}\n.log-setting-editor .editor-container .divider {\n  padding-bottom: 15px;\n}\n.log-setting-editor .editor-container .divider .dnn-uicommon-select,\n.log-setting-editor .editor-container .divider .dnn-ui-common-single-line-input {\n  border-radius: 0px;\n}\n.log-setting-editor .editor-container .editor-row {\n  float: left;\n  width: 100%;\n}\n.log-setting-editor .editor-container .editor-row label {\n  clear: both;\n  float: left;\n  text-align: left;\n  width: 100%;\n  margin-bottom: 5px;\n}\n.log-setting-editor .editor-container .editor-row select.full {\n  float: left;\n  width: 100%;\n}\n.log-setting-editor .editor-container .editor-row select.two-fifth {\n  float: left;\n  width: 40%;\n}\n.log-setting-editor .editor-container .editor-row select.one-fourth {\n  float: left;\n  width: 25%;\n}\n.log-setting-editor .editor-container .editor-row .text-section {\n  width: 5%;\n  float: left;\n  text-align: center;\n  vertical-align: middle;\n  margin-top: 9px;\n}\n.log-setting-editor .editor-container .editor-row .dnn-single-line-input-with-error,\n.log-setting-editor .editor-container .editor-row input {\n  width: 100%;\n}\n.log-setting-editor .buttons-box {\n  width: 100%;\n  text-align: center;\n  float: left;\n  margin: 10px 0;\n}\n.log-setting-editor .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n.log-setting-editor .buttons-box .edit-icon {\n  margin: 0px 10px 20px 10px;\n  float: right;\n}\n.log-setting-editor .buttons-box .edit-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n}\n", ""]);



/***/ }),
/* 63 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(64);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(13)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 64 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(12)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n#adminlogs-container > div {\n  padding: 0;\n}\n#adminlogs-container .dnn-persona-bar-page-body * {\n  box-sizing: border-box;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings {\n  width: 100%;\n  box-sizing: border-box;\n  padding: 20px 20px 55px;\n  display: table;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .container {\n  float: left;\n  border-left: 1px solid #C8C8C8;\n  border-right: 1px solid #C8C8C8;\n  width: 100%;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .container .header-row {\n  display: table;\n  border-bottom: 1px solid #C8C8C8;\n  border-top: 1px solid #C8C8C8;\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 20px;\n  box-sizing: border-box;\n  text-transform: uppercase;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .container .add-setting-editor {\n  float: left;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .container .row-opened {\n  margin-top: -3px;\n  margin-bottom: -1px;\n  border-top: 2px solid #1E88C3;\n  border-bottom: 2px solid #1E88C3 !important;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .add-setting-row {\n  text-align: right;\n  width: 100%;\n  font-weight: bolder;\n  overflow: hidden;\n  cursor: pointer;\n  box-sizing: border-box;\n  border-bottom: 1px solid #C8C8C8;\n  padding: 0 0 15px 20px;\n  margin-bottom: 15px;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .add-setting-row .add-setting-box {\n  width: auto;\n  float: right;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .add-setting-row .add-setting-box .add-icon {\n  margin-left: 20px;\n  margin-right: 5px;\n  float: left;\n  cursor: pointer;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .add-setting-row .add-setting-box .add-icon svg {\n  fill: #6F7273;\n  width: 16px;\n  float: left;\n  height: 16px;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .add-setting-row .add-setting-box.false {\n  color: #1E88C3;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .add-setting-row .add-setting-box.false svg {\n  fill: #1E88C3;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .add-setting-row .add-setting-box.false:hover {\n  color: #4B4E4F;\n}\n#adminlogs-container .dnn-persona-bar-page-body .react-tabs .log-settings .add-setting-row .add-setting-box.false:hover svg {\n  fill: #4B4E4F;\n}\n", ""]);



/***/ })
/******/ ]);
//# sourceMappingURL=adminLogs-bundle.js.map