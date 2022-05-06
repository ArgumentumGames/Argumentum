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
/******/ 	return __webpack_require__(__webpack_require__.s = 73);
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
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
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

  reactHotLoader.register(resx, "resx", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\resources\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\resources\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

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
  module.exports = __webpack_require__(76)();
}


/***/ }),
/* 3 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (true) {
  module.exports = __webpack_require__(74);
} else { var jsFeaturesPresent, evalAllowed; }


/***/ }),
/* 4 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.CommonComponents;

/***/ }),
/* 5 */
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

  reactHotLoader.register(utils, "utils", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\utils\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\utils\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 6 */
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
/* 7 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _roles__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(37);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _roles__WEBPACK_IMPORTED_MODULE_0__["a"]; });

/* harmony import */ var _roleUsers__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(38);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _roleUsers__WEBPACK_IMPORTED_MODULE_1__["a"]; });





/***/ }),
/* 8 */
/***/ (function(module, exports) {

function _interopRequireDefault(obj) {
  return obj && obj.__esModule ? obj : {
    "default": obj
  };
}

module.exports = _interopRequireDefault;
module.exports["default"] = module.exports, module.exports.__esModule = true;

/***/ }),
/* 9 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactRedux;

/***/ }),
/* 10 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* harmony import */ var _roles__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(39);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "b", function() { return _roles__WEBPACK_IMPORTED_MODULE_0__["a"]; });

/* harmony import */ var _roleUsers__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(40);
/* harmony reexport (safe) */ __webpack_require__.d(__webpack_exports__, "a", function() { return _roleUsers__WEBPACK_IMPORTED_MODULE_1__["a"]; });





/***/ }),
/* 11 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }



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
    key: "getRoleGroups",
    value: function getRoleGroups(reload, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.get("GetRoleGroups", {
        reload: reload
      }, callback, errorCallback);
    }
  }, {
    key: "getRoles",
    value: function getRoles(parameters, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.get("GetRoles", parameters, callback, errorCallback);
    }
  }, {
    key: "saveRoleGroup",
    value: function saveRoleGroup(group, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.post("SaveRoleGroup", group, callback, errorCallback);
    }
  }, {
    key: "deleteRoleGroup",
    value: function deleteRoleGroup(group, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.post("DeleteRoleGroup", group, callback, errorCallback);
    }
  }, {
    key: "saveRole",
    value: function saveRole(assignExistUsers, role, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.post("SaveRole?assignExistUsers=" + assignExistUsers, role, callback, errorCallback);
    }
  }, {
    key: "deleteRole",
    value: function deleteRole(role, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.post("DeleteRole", role, callback, errorCallback);
    }
  }, {
    key: "getSuggestUsers",
    value: function getSuggestUsers(parameters, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.get("GetSuggestUsers", parameters, callback, errorCallback);
    }
  }, {
    key: "getRoleUsers",
    value: function getRoleUsers(parameters, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.get("GetRoleUsers", parameters, callback, errorCallback);
    }
  }, {
    key: "addUserToRole",
    value: function addUserToRole(parameters, notifyUser, isOwner, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.post("AddUserToRole?notifyUser=" + notifyUser + "&isOwner=" + isOwner, parameters, callback, errorCallback);
    }
  }, {
    key: "removeUserFromRole",
    value: function removeUserFromRole(parameters, callback, errorCallback) {
      var sf = this.getServiceFramework("Roles");
      sf.post("RemoveUserFromRole", parameters, callback, errorCallback);
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

  reactHotLoader.register(ApplicationService, "ApplicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(applicationService, "applicationService", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\services\\applicationService.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\services\\applicationService.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 12 */
/***/ (function(module, exports, __webpack_require__) {

var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/*!
  Copyright (c) 2017 Jed Watson.
  Licensed under the MIT License (MIT), see
  http://jedwatson.github.io/classnames
*/
/* global define */

(function () {
	'use strict';

	var hasOwn = {}.hasOwnProperty;

	function classNames () {
		var classes = [];

		for (var i = 0; i < arguments.length; i++) {
			var arg = arguments[i];
			if (!arg) continue;

			var argType = typeof arg;

			if (argType === 'string' || argType === 'number') {
				classes.push(arg);
			} else if (Array.isArray(arg) && arg.length) {
				var inner = classNames.apply(null, arg);
				if (inner) {
					classes.push(inner);
				}
			} else if (argType === 'object') {
				for (var key in arg) {
					if (hasOwn.call(arg, key) && arg[key]) {
						classes.push(key);
					}
				}
			}
		}

		return classes.join(' ');
	}

	if ( true && module.exports) {
		classNames.default = classNames;
		module.exports = classNames;
	} else if (true) {
		// register as 'classnames', consistent with npm package name
		!(__WEBPACK_AMD_DEFINE_ARRAY__ = [], __WEBPACK_AMD_DEFINE_RESULT__ = (function () {
			return classNames;
		}).apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__),
				__WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
	} else {}
}());


/***/ }),
/* 13 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return createRoleGroupOptions; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "e", function() { return removeFromRolesList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "d", function() { return removeFromRoleGroupList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "g", function() { return updateRolesList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "f", function() { return updateRoleGroupList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return decrementUsersCountFromRoleList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "c", function() { return incrementUsersCountFromRoleList; });
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(1);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();


function createRoleGroupOptions(roleGroupList) {
  var roleGroupOptions = [];

  if (roleGroupList !== undefined) {
    roleGroupOptions = roleGroupList.map(function (item) {
      return {
        label: item.name,
        value: item.id
      };
    });
  }

  return roleGroupOptions;
}
function removeFromRolesList(rolesList, roleId) {
  if (rolesList.some(function (role) {
    return role.id === roleId;
  })) {
    rolesList = rolesList.filter(function (role) {
      return role.id !== roleId;
    });
  }

  return rolesList;
}
function removeFromRoleGroupList(roleGroups, roleGroupId) {
  if (roleGroups.some(function (roleGroup) {
    return roleGroup.id === roleGroupId;
  })) {
    roleGroups = roleGroups.filter(function (roleGroup) {
      return roleGroup.id !== roleGroupId;
    });
  }

  return roleGroups;
}
function updateRolesList(rolesList, roleDetails) {
  if (rolesList.some(function (role) {
    return role.id === roleDetails.id;
  })) {
    rolesList = rolesList.filter(function (role) {
      return role.id !== roleDetails.id;
    });
  }

  if (!rolesList.some(function (role) {
    return role.id === roleDetails.id;
  })) {
    rolesList = [roleDetails].concat(rolesList);
    rolesList = rolesList.sort(function (a, b) {
      var nameA = a.name;
      var nameB = b.name;
      if (nameA < nameB) //sort string ascending
        return -1;
      if (nameA > nameB) return 1;
      return 0; //default return value (no sorting)
    });
    return rolesList;
  }
}
function updateRoleGroupList(roleGroups, roleGroup) {
  if (roleGroup.id > -1) {
    if (roleGroups.some(function (group) {
      return group.id === roleGroup.id;
    })) {
      roleGroups = roleGroups.filter(function (group) {
        return group.id !== roleGroup.id;
      });
    }

    roleGroups = roleGroups.filter(function (group) {
      return group.id !== -1 && group.id !== -2;
    });

    if (!roleGroups.some(function (group) {
      return group.id === roleGroup.id;
    })) {
      roleGroups = [roleGroup].concat(roleGroups);
      roleGroups = roleGroups.sort(function (a, b) {
        var nameA = a.name;
        var nameB = b.name;
        if (nameA < nameB) //sort string ascending
          return -1;
        if (nameA > nameB) return 1;
        return 0; //default return value (no sorting)
      });
      roleGroups = [{
        id: -2,
        name: _resources__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].get("AllGroups"),
        description: _resources__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].get("AllGroups")
      }, {
        id: -1,
        name: _resources__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].get("GlobalRolesGroup"),
        description: _resources__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].get("AllGroups")
      }].concat(roleGroups);
      return roleGroups;
    }
  }
}
function decrementUsersCountFromRoleList(rolesList, roleId) {
  if (rolesList.some(function (roleUser) {
    return roleUser.id === roleId;
  })) {
    rolesList = rolesList.map(function (roleUser) {
      if (roleUser.id === roleId && roleUser.usersCount > 0) {
        roleUser.usersCount--;
      }

      return roleUser;
    });
  }

  return rolesList;
}
function incrementUsersCountFromRoleList(rolesList, roleId) {
  if (rolesList.some(function (roleUser) {
    return roleUser.id === roleId;
  })) {
    rolesList = rolesList.map(function (roleUser) {
      if (roleUser.id === roleId) {
        roleUser.usersCount++;
      }

      return roleUser;
    });
  }

  return rolesList;
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(createRoleGroupOptions, "createRoleGroupOptions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\helpers\\roles.js");
  reactHotLoader.register(removeFromRolesList, "removeFromRolesList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\helpers\\roles.js");
  reactHotLoader.register(removeFromRoleGroupList, "removeFromRoleGroupList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\helpers\\roles.js");
  reactHotLoader.register(updateRolesList, "updateRolesList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\helpers\\roles.js");
  reactHotLoader.register(updateRoleGroupList, "updateRoleGroupList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\helpers\\roles.js");
  reactHotLoader.register(decrementUsersCountFromRoleList, "decrementUsersCountFromRoleList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\helpers\\roles.js");
  reactHotLoader.register(incrementUsersCountFromRoleList, "incrementUsersCountFromRoleList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\helpers\\roles.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 14 */
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
/* 15 */
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

var	fixUrls = __webpack_require__(80);

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
/* 16 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReactDOM;

/***/ }),
/* 17 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _default = !!(typeof window !== 'undefined' && window.document && window.document.createElement);

exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 18 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.Redux;

/***/ }),
/* 19 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.message = exports.accessor = exports.disabled = exports.dateFormat = exports.numberFormat = void 0;

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _elementType = _interopRequireDefault(__webpack_require__(102));

exports.elementType = _elementType.default;

var _createChainableTypeChecker = _interopRequireDefault(__webpack_require__(43));

var _localizers = __webpack_require__(105);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var numberFormat = (0, _createChainableTypeChecker.default)(function () {
  return _localizers.number.propType.apply(_localizers.number, arguments);
});
exports.numberFormat = numberFormat;
var dateFormat = (0, _createChainableTypeChecker.default)(function () {
  return _localizers.date.propType.apply(_localizers.date, arguments);
});
exports.dateFormat = dateFormat;
var disabled = (0, _createChainableTypeChecker.default)(function () {
  return _propTypes.default.bool.apply(_propTypes.default, arguments);
});
exports.disabled = disabled;
disabled.acceptsArray = _propTypes.default.oneOfType([disabled, _propTypes.default.array]);

var accessor = _propTypes.default.oneOfType([_propTypes.default.string, _propTypes.default.func]);

exports.accessor = accessor;

var message = _propTypes.default.oneOfType([_propTypes.default.node, _propTypes.default.string, _propTypes.default.func]);

exports.message = message;

/***/ }),
/* 20 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.isShallowEqual = isShallowEqual;
exports.chunk = chunk;
exports.groupBySortedKeys = groupBySortedKeys;
exports.has = exports.makeArray = void 0;

var _warning = _interopRequireDefault(__webpack_require__(106));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var makeArray = function makeArray(obj) {
  return obj == null ? [] : [].concat(obj);
};

exports.makeArray = makeArray;

var has = function has(o, k) {
  return o ? Object.prototype.hasOwnProperty.call(o, k) : false;
};

exports.has = has;

function isShallowEqual(a, b) {
  if (a === b) return true;
  if (a instanceof Date && b instanceof Date) return +a === +b;
  if (typeof a !== 'object' && typeof b !== 'object') return a === b;
  if (typeof a !== typeof b) return false;
  if (a == null || b == null) return false; // if they were both null we wouldn't be here

  var keysA = Object.keys(a);
  var keysB = Object.keys(b);
  if (keysA.length !== keysB.length) return false;

  for (var i = 0; i < keysA.length; i++) {
    if (!has(b, keysA[i]) || a[keysA[i]] !== b[keysA[i]]) return false;
  }

  return true;
}

function chunk(array, chunkSize) {
  var index = 0,
      length = array ? array.length : 0;
  var result = [];
  chunkSize = Math.max(+chunkSize || 1, 1);

  while (index < length) {
    result.push(array.slice(index, index += chunkSize));
  }

  return result;
}

function groupBySortedKeys(groupBy, data, keys) {
  var iter = typeof groupBy === 'function' ? groupBy : function (item) {
    return item[groupBy];
  }; // the keys array ensures that groups are rendered in the order they came in
  // which means that if you sort the data array it will render sorted,
  // so long as you also sorted by group

  keys = keys || [];
   false ? undefined : void 0;
  return data.reduce(function (grps, item) {
    var group = iter(item);

    if (has(grps, group)) {
      grps[group].push(item);
    } else {
      keys.push(group);
      grps[group] = [item];
    }

    return grps;
  }, {});
}

/***/ }),
/* 21 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = getWindow;

function getWindow(node) {
  return node === node.window ? node : node.nodeType === 9 ? node.defaultView || node.parentWindow : false;
}

module.exports = exports["default"];

/***/ }),
/* 22 */
/***/ (function(module, exports) {

var LIFECYCLE_HOOKS = {
  componentWillMount: true,
  componentDidMount: true,
  componentWillReceiveProps: true,
  getSnapshotBeforeUpdate: true,
  shouldComponentUpdate: true,
  componentWillUpdate: true,
  componentDidUpdate: true,
  componentWillUnmount: true,
}

var STATIC_HOOKS = {
  getDerivedStateFromProps: true,
}

function wrap(base, method, isStatic) {
  var before = true

  if (Array.isArray(method)) {
    before = method[0] !== 'after'
    method = method[1]
  }

  if (!base) return method

  return function wrappedLifecyclehook() {
    var ctx = isStatic ? null : this
    before && method.apply(ctx, arguments)
    base.apply(ctx, arguments)
    !before && method.apply(ctx, arguments)
  }
}

module.exports = function spyOnComponent(component, hooks) {
  var originals = Object.create(null)

  for (var key in hooks)
    if (STATIC_HOOKS[key])
      component.constructor[key] = wrap(
        (originals[key] = component.constructor[key]),
        hooks[key],
        true
      )

  for (var key in hooks)
    if (LIFECYCLE_HOOKS[key])
      component[key] = wrap((originals[key] = component[key]), hooks[key])

  return function reset(key) {
    var subject = STATIC_HOOKS[key] ? component.constructor : component

    if (key && key in originals) subject[key] = originals[key]
    else for (var key in originals) subject[key] = originals[key]
  }
}

module.exports.mixin = function mixinIntoComponent(componentClass, hooks) {
  spyOnComponent(componentClass.prototype, hooks)
  return componentClass
}


/***/ }),
/* 23 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(2);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(92);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(5);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(10);
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










var RoleGroupEditor =
/*#__PURE__*/
function (_Component) {
  _inherits(RoleGroupEditor, _Component);

  function RoleGroupEditor(props) {
    var _this;

    _classCallCheck(this, RoleGroupEditor);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(RoleGroupEditor).call(this, props));
    var group = Object.assign({}, props.group);
    _this.state = {
      group: group,
      errors: {
        groupName: false
      }
    };
    _this.submitted = false;
    _this.handleClick = _this.handleClick.bind(_assertThisInitialized(_this));
    _this.node = react__WEBPACK_IMPORTED_MODULE_0___default.a.createRef();
    return _this;
  }

  _createClass(RoleGroupEditor, [{
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

      var node = this.rootElement !== undefined ? this.rootElement.current : null;

      if (node && !node.contains(event.target) && event.target.firstChild !== null && typeof event.target.firstChild.className === "string" && event.target.firstChild.className.indexOf("do-not-close") === -1) {
        if (typeof this.props.onCancel === "function") {
          this.props.onCancel();
        }

        this.props.onCancel();
      }
    }
  }, {
    key: "onEditFieldChanged",
    value: function onEditFieldChanged(name, event) {
      var _this2 = this;

      var group = this.state.group;
      group[name] = event.target.value || "";
      this.setState({
        group: group
      }, function () {
        _this2.validateForm();
      });
    }
  }, {
    key: "onCancel",
    value: function onCancel(e) {
      var _this3 = this;

      e.stopPropagation();
      this.setState({
        group: {}
      }, function () {
        if (typeof _this3.props.onCancel === "function") {
          _this3.props.onCancel();
        }
      });
    }
  }, {
    key: "onSave",
    value: function onSave(e) {
      var _this4 = this;

      e.stopPropagation();
      var props = this.props,
          state = this.state;
      this.submitted = true;

      if (this.validateForm()) {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_7__[/* roles */ "b"].saveRoleGroup(state.group, function (group) {
          _utils__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("RoleGroupUpdated.Message"));

          if (typeof _this4.props.onSave === "function") {
            _this4.props.onSave(group);
          }
        }));
      }
    }
  }, {
    key: "validateForm",
    value: function validateForm() {
      var valid = true;

      if (this.submitted) {
        var group = this.state.group;
        var errors = this.state.errors;
        errors.groupName = false;

        if (group.name === "") {
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
    key: "render",
    value: function render() {
      var _this5 = this;

      var props = this.props,
          state = this.state;
      var group = Object.assign({}, state.group);
      return props.visible &&
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        ref: function ref(node) {
          return _this5.rootElement = node;
        },
        className: "role-group-editor",
        onClick: this.props.onClick.bind(this)
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("h2", null, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get(props.title)),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "edit-form"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "form-items"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "form-item"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["SingleLineInputWithError"], {
        value: group.name,
        onChange: this.onEditFieldChanged.bind(this, "name"),
        maxLength: 50,
        error: state.errors.groupName,
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GroupName"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GroupName.Help"),
        errorMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GroupName.Help"),
        autoComplete: "off",
        inputStyle: {
          marginBottom: 15
        },
        tabIndex: 1
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "form-item"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Label"], {
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GroupDescription"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GroupDescription.Help"),
        tooltipPlace: "top"
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["MultiLineInput"], {
        value: group.description,
        onChange: this.onEditFieldChanged.bind(this, "description"),
        maxLength: 500
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "clear"
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "actions"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Button"], {
        onClick: this.onCancel.bind(this),
        className: "do-not-close"
      }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Cancel")),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Button"], {
        type: "primary",
        onClick: this.onSave.bind(this)
      }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Save")))));
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return RoleGroupEditor;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

RoleGroupEditor.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  title: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  visible: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  group: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  deleteAllowed: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  onDeleteClick: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onDeleted: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onSave: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onClick: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onCancel: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func
};
RoleGroupEditor.defaultProps = {
  deleteAllowed: false,
  visible: true,
  group: {
    id: -2,
    name: "",
    description: ""
  }
};

function mapStateToProps() {
  return {};
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(RoleGroupEditor);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(RoleGroupEditor, "RoleGroupEditor", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleEditor\\RoleGroupEditor\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleEditor\\RoleGroupEditor\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleEditor\\RoleGroupEditor\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 24 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



/**
 * Use invariant() to assert state which your program assumes to be true.
 *
 * Provide sprintf-style format (only %s is supported) and arguments
 * to provide information about what broke and what you were
 * expecting.
 *
 * The invariant message will be stripped in production, but the invariant
 * will remain to ensure logic does not differ in production.
 */

var invariant = function(condition, format, a, b, c, d, e, f) {
  if (false) {}

  if (!condition) {
    var error;
    if (format === undefined) {
      error = new Error(
        'Minified exception occurred; use the non-minified dev environment ' +
        'for the full error message and additional helpful warnings.'
      );
    } else {
      var args = [a, b, c, d, e, f];
      var argIndex = 0;
      error = new Error(
        format.replace(/%s/g, function() { return args[argIndex++]; })
      );
      error.name = 'Invariant Violation';
    }

    error.framesToPop = 1; // we don't care about invariant's own frame
    throw error;
  }
};

module.exports = invariant;


/***/ }),
/* 25 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.pick = pick;
exports.pickElementProps = pickElementProps;
exports.omitOwn = omitOwn;
var whitelist = ['style', 'className', 'role', 'id', 'autocomplete', 'size', 'tabIndex', 'maxLength', 'name'];
var whitelistRegex = [/^aria-/, /^data-/, /^on[A-Z]\w+/];

function pick(props, componentClass) {
  var keys = Object.keys(componentClass.propTypes);
  var result = {};
  Object.keys(props).forEach(function (key) {
    if (keys.indexOf(key) === -1) return;
    result[key] = props[key];
  });
  return result;
}

function pickElementProps(component) {
  for (var _len = arguments.length, others = new Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
    others[_key - 1] = arguments[_key];
  }

  var props = omitOwn.apply(void 0, [component].concat(others));
  var result = {};
  Object.keys(props).forEach(function (key) {
    if (whitelist.indexOf(key) !== -1 || whitelistRegex.some(function (r) {
      return !!key.match(r);
    })) result[key] = props[key];
  });
  return result;
}

function omitOwn(component) {
  var initial = Object.keys(component.constructor.propTypes);

  for (var _len2 = arguments.length, others = new Array(_len2 > 1 ? _len2 - 1 : 0), _key2 = 1; _key2 < _len2; _key2++) {
    others[_key2 - 1] = arguments[_key2];
  }

  var keys = others.reduce(function (arr, compClass) {
    return arr.concat(Object.keys(compClass.propTypes));
  }, initial);
  var result = {};
  Object.keys(component.props).forEach(function (key) {
    if (keys.indexOf(key) !== -1) return;
    result[key] = component.props[key];
  });
  return result;
}

/***/ }),
/* 26 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.notify = notify;
exports.instanceId = instanceId;
exports.isFirstFocusedRender = isFirstFocusedRender;
var idCount = 0;

function uniqueId(prefix) {
  return '' + ((prefix == null ? '' : prefix) + ++idCount);
}

function notify(handler, args) {
  handler && handler.apply(null, [].concat(args));
}

function instanceId(component, suffix) {
  if (suffix === void 0) {
    suffix = '';
  }

  component.__id || (component.__id = uniqueId('rw_'));
  return (component.props.id || component.__id) + suffix;
}
/**
 * Allows for defering popup rendering untill the widget is focused,
 * or has been opened (in order to not remove it suddenly on close)
 */


function isFirstFocusedRender(component) {
  return component._firstFocus || (component.state.focused || !!component.props.open) && (component._firstFocus = true);
}

/***/ }),
/* 27 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.dataIndexOf = dataIndexOf;
exports.valueMatcher = valueMatcher;
exports.dataItem = dataItem;
exports.dataText = exports.dataValue = void 0;

var _ = __webpack_require__(20);

var dataValue = function dataValue(data, field) {
  var value = data;
  if (typeof field === 'function') value = field(data);else if (data == null) value = data;else if (typeof field === 'string' && typeof data === 'object' && field in data) value = data[field];
  return value;
};

exports.dataValue = dataValue;

var dataText = function dataText(item, textField) {
  var value = dataValue(item, textField);
  return value == null ? '' : value + '';
};

exports.dataText = dataText;

function dataIndexOf(data, item, valueField) {
  var idx = -1;

  var isValueEqual = function isValueEqual(datum) {
    return valueMatcher(item, datum, valueField);
  };

  while (++idx < data.length) {
    var datum = data[idx];
    if (datum === item || isValueEqual(datum)) return idx;
  }

  return -1;
}
/**
 * I don't know that the shallow equal makes sense here but am too afraid to
 * remove it.
 */


function valueMatcher(a, b, valueField) {
  return (0, _.isShallowEqual)(dataValue(a, valueField), dataValue(b, valueField));
}

function dataItem(data, item, valueField) {
  var idx = dataIndexOf(data, item, valueField);
  return idx !== -1 ? data[idx] : item;
}

/***/ }),
/* 28 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = height;

var _offset = _interopRequireDefault(__webpack_require__(54));

var _isWindow = _interopRequireDefault(__webpack_require__(21));

function height(node, client) {
  var win = (0, _isWindow.default)(node);
  return win ? win.innerHeight : client ? node.clientHeight : (0, _offset.default)(node).height;
}

module.exports = exports["default"];

/***/ }),
/* 29 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = spyOnMount;

var _spyOnComponent = _interopRequireDefault(__webpack_require__(22));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function spyOnMount(componentInstance) {
  var mounted = true;
  (0, _spyOnComponent.default)(componentInstance, {
    componentWillUnmount: function componentWillUnmount() {
      mounted = false;
    }
  });
  return function () {
    return mounted;
  };
}

module.exports = exports["default"];

/***/ }),
/* 30 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(5);
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
    var options = window.dnn.initRoles();
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].init(options.utility);
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].moduleName = options.moduleName;
    _utils__WEBPACK_IMPORTED_MODULE_0__[/* default */ "a"].settings = options.settings;
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

  reactHotLoader.register(boilerPlate, "boilerPlate", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\globals\\application.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\globals\\application.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 31 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxThunk;

/***/ }),
/* 32 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return removeRoleUserFromList; });
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "b", function() { return updateRoleUserList; });
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function removeRoleUserFromList(roleUsers, userId) {
  if (roleUsers.some(function (roleGroup) {
    return roleGroup.userId === userId;
  })) {
    roleUsers = roleUsers.filter(function (roleGroup) {
      return roleGroup.userId !== userId;
    });
  }

  return roleUsers;
}
function updateRoleUserList(roleUsers, roleUserDetails) {
  if (roleUsers.some(function (role) {
    return role.userId === roleUserDetails.userId;
  })) {
    roleUsers = roleUsers.filter(function (role) {
      return role.userId !== roleUserDetails.userId;
    });
  }

  if (!roleUsers.some(function (role) {
    return role.userId === roleUserDetails.userId;
  })) {
    roleUsers.push(roleUserDetails);
    roleUsers = roleUsers.sort(function (a, b) {
      var userIdA = a.userId;
      var userIdB = b.userId;
      if (userIdA < userIdB) //sort string ascending
        return -1;
      if (userIdA > userIdB) return 1;
      return 0; //default return value (no sorting)
    });
    return roleUsers;
  }
}
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(removeRoleUserFromList, "removeRoleUserFromList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\helpers\\roleUsers.js");
  reactHotLoader.register(updateRoleUserList, "updateRoleUserList", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\helpers\\roleUsers.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 33 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(2);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(94);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var resources__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(1);
/* harmony import */ var utils__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(5);
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







var canEdit = false;
var roleLabelStyle = {
  wordWrap: "break-word"
};

var RoleRow =
/*#__PURE__*/
function (_Component) {
  _inherits(RoleRow, _Component);

  function RoleRow() {
    var _this;

    _classCallCheck(this, RoleRow);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(RoleRow).call(this));
    _this.handleClick = _this.handleClick.bind(_assertThisInitialized(_this));
    canEdit = utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].settings.isHost || utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].settings.isAdmin || utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].settings.permissions.EDIT;
    return _this;
  }

  _createClass(RoleRow, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var opened = this.props.openId !== "" && this.props.id === this.props.openId;
      this.setState({
        opened: opened
      });
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

      if (!this.node.contains(event.target) && typeof event.target.className === "string" && event.target.className.indexOf("do-not-close") === -1 && !(event.target.id === "confirmbtn" || event.target.id === "cancelbtn") && this.props.openId !== "add") {
        this.timeout = 475;

        if (this.props.openId !== "" && this.props.id === this.props.openId) {
          this.props.Collapse();
        }
      } else {
        this.timeout = 0;
      }
    }
  }, {
    key: "toggleEditRole",
    value: function toggleEditRole() {
      if (this.props.openId !== "" && this.props.id === this.props.openId && this.props.currentIndex === 1) {
        this.props.Collapse();
      } else {
        this.props.OpenCollapseEditRoles(this.props.id);
      }
    }
  }, {
    key: "toggleUsers",
    value: function toggleUsers() {
      if (this.props.openId !== "" && this.props.id === this.props.openId && this.props.currentIndex === 0) {
        this.props.Collapse();
      } else {
        this.props.OpenCollapseUsers(this.props.id);
      }
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      var props = this.props;
      var opened = this.props.openId !== "" && this.props.id === this.props.openId;
      var uniqueId = "roleRow-" + Math.random() + Date.now();

      if (props.visible) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            ref: function ref(node) {
              return _this2.node = node;
            },
            className: "collapsible-component1 " + opened,
            id: uniqueId
          }, !props.addIsClosed &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "collapsible-header1 " + !opened
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
            style: roleLabelStyle,
            title: props.roleName,
            columnSize: 40,
            className: "ellipsis"
          }, props.roleName),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
            columnSize: 20
          }, props.groupName),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
            columnSize: 15
          }, props.userCount),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
            columnSize: 15
          }, props.auto ?
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["IconButton"], {
            className: "icon-flat",
            customIcon: __webpack_require__(96).default
          }) :
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, "\xA0 "), " "), canEdit &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["GridCell"], {
            columnSize: 10
          }, props.id !== "add" && props.roleIsApproved &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["IconButton"], {
            type: "user",
            className: "edit-icon " + !(opened && props.currentIndex === 0),
            onClick: this.toggleUsers.bind(this),
            title: resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("UsersInRole")
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["IconButton"], {
            type: "edit",
            className: "edit-icon " + !(opened && props.currentIndex === 1),
            onClick: this.toggleEditRole.bind(this),
            title: resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("EditRole")
          }))),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Collapsible"], {
            accordion: true,
            isOpened: opened,
            className: "role-row-collapsible"
          }, opened && props.children))
        );
      } else {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null)
        );
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

  return RoleRow;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

RoleRow.propTypes = {
  roleName: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  groupName: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  userCount: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  auto: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  OpenCollapseUsers: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  OpenCollapseEditRoles: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  Collapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  id: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  openId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  currentIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  visible: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool,
  roleIsApproved: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};
RoleRow.defaultProps = {
  collapsed: true,
  visible: true,
  roleIsApproved: false
};
var _default = RoleRow;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(canEdit, "canEdit", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleRow\\index.jsx");
  reactHotLoader.register(roleLabelStyle, "roleLabelStyle", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleRow\\index.jsx");
  reactHotLoader.register(RoleRow, "RoleRow", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleRow\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleRow\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 34 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(2);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(140);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(5);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(1);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(10);
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










var UserRow =
/*#__PURE__*/
function (_Component) {
  _inherits(UserRow, _Component);

  function UserRow() {
    var _this;

    _classCallCheck(this, UserRow);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(UserRow).call(this));
    _this.state = {
      userSelectState: {
        userId: -1,
        keyword: ""
      },
      currentPage: 0,
      pageSize: 10,
      usersKeyword: "",
      editIndex: -1,
      editCommand: "",
      isCalendarVisible: false
    };
    return _this;
  }

  _createClass(UserRow, [{
    key: "formateDate",
    value: function formateDate(dateValue) {
      var date = new Date(dateValue);
      var dayValue = date.getDate(),
          monthValue = date.getMonth() + 1,
          yearValue = date.getFullYear();

      if (yearValue < 1900) {
        return "-";
      }

      return monthValue + "/" + dayValue + "/" + yearValue;
    }
  }, {
    key: "onStartTimeClick",
    value: function onStartTimeClick(userRole, index) {
      this.setState({
        editIndex: index,
        editCommand: "startTime",
        isCalendarVisible: true
      });
    }
  }, {
    key: "onExpiresTimeClick",
    value: function onExpiresTimeClick(userRole, index) {
      this.setState({
        editIndex: index,
        editCommand: "expiresTime",
        isCalendarVisible: true
      });
    }
  }, {
    key: "onDeleteClick",
    value: function onDeleteClick(userRole) {
      var props = this.props;
      _utils__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].utilities.confirm(_resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("DeleteUser.Confirm"), _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Delete"), _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Cancel"), function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_7__[/* roleUsers */ "a"].removeUserFromRole(userRole));
      });
    }
  }, {
    key: "isEmptyDate",
    value: function isEmptyDate(date) {
      return !date || new Date(date).getFullYear() < 1970;
    }
  }, {
    key: "onChange",
    value: function onChange(userRole, command, FirstDate) {
      var state = this.state;
      state.editIndex = -1;
      state.editCommand = "";
      var startTime = command === "startTime" ? FirstDate : userRole.startTime;
      var expiresTime = command === "expiresTime" ? FirstDate : userRole.expiresTime;
      this.props.saveUser(userRole.userId, startTime, expiresTime);
      this.setState({
        isCalendarVisible: false
      });
    }
  }, {
    key: "getBoundDate",
    value: function getBoundDate(userRole, command) {
      if (command === "startTime") {
        var maxValue = new Date(2049, 11, 31);

        if (!this.isEmptyDate(userRole.expiresTime)) {
          maxValue = new Date(new Date().setTime(new Date(userRole.expiresTime).getTime() - 1 * 86400000));
        }

        return maxValue;
      } else if (command === "expiresTime") {
        var minValue = new Date(1970, 0, 1);

        if (!this.isEmptyDate(userRole.startTime)) {
          minValue = new Date(new Date().setTime(new Date(userRole.startTime).getTime() + 1 * 86400000));
        }

        return minValue;
      }
    }
  }, {
    key: "getDate",
    value: function getDate(userDetails, command) {
      var dateValue = new Date();

      if (command === "startTime") {
        if (!this.isEmptyDate(userDetails.startTime)) {
          dateValue = new Date(userDetails.startTime);
        }
      } else if (command === "expiresTime") {
        if (!this.isEmptyDate(userDetails.expiresTime)) {
          dateValue = new Date(userDetails.expiresTime);
        }
      }

      return dateValue;
    }
  }, {
    key: "createUserActions",
    value: function createUserActions() {
      var props = this.props,
          state = this.state;
      var startTimeAction = props.userDetails.allowExpired ?
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null,
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["DatePicker"], {
        date: this.getDate(props.userDetails, "startTime"),
        maxDate: this.getBoundDate(props.userDetails, "startTime"),
        updateDate: this.onChange.bind(this, props.userDetails, "startTime"),
        mode: "start",
        applyButtonText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Apply"),
        showIcon: true,
        showInput: false,
        onIconClick: this.onStartTimeClick.bind(this, props.userDetails, props.index)
      })) : null;
      var expiresTimeAction = props.userDetails.allowExpired ?
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null,
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["DatePicker"], {
        date: this.getDate(props.userDetails, "expiresTime"),
        minDate: this.getBoundDate(props.userDetails, "expiresTime"),
        updateDate: this.onChange.bind(this, props.userDetails, "expiresTime"),
        mode: "end",
        applyButtonText: _resources__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].get("Apply"),
        showIcon: true,
        showInput: false,
        onIconClick: this.onExpiresTimeClick.bind(this, props.userDetails, props.index)
      })) : null;
      var deleteAction = props.userDetails.allowDelete ?
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["IconButton"], {
        customIcon: __webpack_require__(142).default,
        width: 17,
        onClick: this.onDeleteClick.bind(this, props.userDetails, props.index)
      }) : null;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: state.editIndex === props.index ? "edit-row" : null
        }, deleteAction, expiresTimeAction, startTimeAction)
      );
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props;
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "role-user-row"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          title: props.roleName,
          columnSize: 25
        }, props.userDetails.displayName),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 20
        }, this.formateDate(props.userDetails.startTime)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 20
        }, this.formateDate(props.userDetails.expiresTime)),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 35
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "actions"
        }, this.createUserActions())))
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

  return UserRow;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

UserRow.propTypes = {
  userDetails: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object.isRequired,
  index: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  saveUser: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  deleteUser: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired
};
UserRow.defaultProps = {
  roleUsersList: []
};

function mapStateToProps() {
  return {};
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(UserRow);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(UserRow, "UserRow", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\UsersInRole\\UserRow\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\UsersInRole\\UserRow\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\UsersInRole\\UserRow\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 35 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(2);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(145);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(5);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__);
/* harmony import */ var _RoleGroupEditor__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(23);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(10);
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











var RolesEditor =
/*#__PURE__*/
function (_Component) {
  _inherits(RolesEditor, _Component);

  function RolesEditor(props) {
    var _this;

    _classCallCheck(this, RolesEditor);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(RolesEditor).call(this, props));
    var roleDetails = Object.assign({}, props.roleDetails);
    _this.state = {
      roleDetails: props.roleId !== -1 ? roleDetails : {
        id: -1,
        name: "",
        groupId: -1,
        description: "",
        securityMode: 0,
        status: 1,
        isPublic: false,
        autoAssign: false,
        isSystem: false
      },
      errors: {
        roleName: false
      },
      groupId: props.roleId !== -1 ? roleDetails.groupId : -1,
      assignToUsers: false,
      formModified: false,
      createGroupVisible: false
    };
    _this.submitted = false;
    return _this;
  }

  _createClass(RolesEditor, [{
    key: "getValue",
    value: function getValue(selectKey) {
      var state = this.state;

      switch (selectKey) {
        case "RoleGroup":
          return state.roleDetails.groupId !== undefined ? state.roleDetails.groupId : -1;

        case "SecurityMode":
          return state.roleDetails.securityMode !== undefined ? state.roleDetails.securityMode : 0;

        case "Status":
          return state.roleDetails.status !== undefined ? state.roleDetails.status : 1;

        case "AutoAssignment":
          return state.roleDetails.autoAssign !== undefined ? state.roleDetails.autoAssign : false;

        case "Public":
          return state.roleDetails.isPublic !== undefined ? state.roleDetails.isPublic : false;

        default:
          break;
      }
    }
  }, {
    key: "onDropDownChange",
    value: function onDropDownChange(key, option) {
      if (key === "groupId" && option.value === -3) {
        var createGroupVisible = this.state.createGroupVisible;
        createGroupVisible = true;
        this.setState({
          createGroupVisible: createGroupVisible
        });
      }

      this.performChange(key, option.value);
    }
  }, {
    key: "onTextChange",
    value: function onTextChange(key, event) {
      this.performChange(key, event.target.value);
    }
  }, {
    key: "performChange",
    value: function performChange(key, value) {
      var _this2 = this;

      if (key !== "assignToUsers") {
        var roleDetails = this.state.roleDetails;
        roleDetails[key] = value;
        this.setState({
          roleDetails: roleDetails
        });
      } else {
        var assignToUsers = this.state.assignToUsers;
        assignToUsers = value;
        this.setState({
          assignToUsers: assignToUsers
        });
      }

      var state = this.state;
      state.formModified = true;
      this.setState({
        state: state
      }, function () {
        _this2.validateForm();
      });
    }
  }, {
    key: "onSwitchToggle",
    value: function onSwitchToggle(key, status) {
      this.performChange(key, status);
    }
  }, {
    key: "addUpdateRoleDetails",
    value: function addUpdateRoleDetails(event) {
      event.preventDefault();
      var props = this.props,
          state = this.state;
      this.submitted = true;

      if (!this.validateForm()) {
        return;
      }

      if (state.formModified) {
        var roleDetails = this.state.roleDetails;

        if (roleDetails.groupId === -3) {
          return; //Avoid saving while in create group window
        }

        var successMsg = _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("RoleAdded.Message");
        var errorMsg = _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("RoleAdded.Error");

        if (props.roleId > 0) {
          successMsg = _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("RoleUpdated.Message");
          errorMsg = _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("RoleUpdated.Error");
        }

        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_8__[/* roles */ "b"].saveRole(this.props.currentGroupId, state.assignToUsers, roleDetails, function () {
          _utils__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].utilities.notify(successMsg);
          props.Collapse(event);
        }, function () {
          _utils__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].utilities.notify(errorMsg);
        }));
      } else {
        props.Collapse(event);
      }
    }
  }, {
    key: "validateForm",
    value: function validateForm() {
      var valid = true;

      if (this.submitted) {
        var roleDetails = this.state.roleDetails;
        var errors = this.state.errors;
        errors.roleName = false;

        if (roleDetails.name === "") {
          errors.roleName = true;
          valid = false;
        }

        this.setState({
          errors: errors
        });
      }

      return valid;
    }
  }, {
    key: "refreshRolesListIfRequired",
    value: function refreshRolesListIfRequired() {
      var props = this.props,
          state = this.state;
      var group = props.roleGroups.find(function (group) {
        return group.id === state.groupId;
      });

      if (group !== undefined && group.rolesCount <= 1) {
        props.refreshRolesList();
      }
    }
  }, {
    key: "deleteRole",
    value: function deleteRole(event) {
      var roleDetails = this.state.roleDetails;
      var props = this.props;

      if (props.roleId > 0) {
        _utils__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].utilities.confirm(_resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("DeleteRole.Confirm"), _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Delete"), _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Cancel"), function () {
          props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_8__[/* roles */ "b"].deleteRole(roleDetails, function () {
            _utils__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("DeleteRole.Message"));
            props.Collapse(event);
          }));
        });
      } else {
        _utils__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("DeleteInconsistency.Error"));
      }
    }
  }, {
    key: "getRoleGroupOptions",
    value: function getRoleGroupOptions() {
      var groupOptions = this.props.roleGroupOptions;
      groupOptions = groupOptions.filter(function (group) {
        return group.value !== -2;
      });
      groupOptions = [{
        label:
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", {
          className: "do-not-close"
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("lblNewGroup")),
        value: -3
      }].concat(groupOptions);
      return groupOptions;
    }
  }, {
    key: "CloseCreateGroup",
    value: function CloseCreateGroup() {
      var createGroupVisible = this.state.createGroupVisible;
      createGroupVisible = false;
      this.setState({
        createGroupVisible: createGroupVisible
      });
    }
  }, {
    key: "onCancelCreateGroup",
    value: function onCancelCreateGroup() {
      this.CloseCreateGroup();
      this.onDropDownChange("groupId", {
        value: -1
      });
    }
  }, {
    key: "onCreateGroup",
    value: function onCreateGroup(group) {
      this.CloseCreateGroup();
      this.onDropDownChange("groupId", {
        value: group.id
      });
    }
  }, {
    key: "doNothing",
    value: function doNothing() {}
    /* eslint-disable react/no-danger */

  }, {
    key: "render",
    value: function render() {
      var state = this.state,
          props = this.props;
      var columnOne =
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        key: "editor-container-columnOne",
        className: "editor-container"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "editor-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["SingleLineInputWithError"], {
        value: state.roleDetails.name,
        enabled: !state.roleDetails.isSystem,
        onChange: this.onTextChange.bind(this, "name"),
        maxLength: 50,
        error: state.errors.roleName,
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("RoleName"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("RoleName.Help"),
        errorMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("RoleName.Required"),
        autoComplete: "off",
        inputStyle: {
          marginBottom: 0
        },
        tabIndex: 1
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "editor-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Label"], {
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Description"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Description.Help"),
        tooltipPlace: "top"
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["MultiLineInput"], {
        value: state.roleDetails.description,
        onChange: this.onTextChange.bind(this, "description"),
        maxLength: 500
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "editor-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Label"], {
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("statusListLabel"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("statusListLabel.Help"),
        tooltipPlace: "top"
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Dropdown"], {
        value: this.getValue("Status"),
        enabled: !state.roleDetails.isSystem,
        options: this.props.statusOptions,
        style: {
          width: 100 + "%",
          float: "left"
        },
        onSelect: this.onDropDownChange.bind(this, "status")
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "status-row"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "left"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Label"], {
        labelType: "inline",
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Public"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("PublicRole.Help"),
        tooltipPlace: "top"
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "right"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Switch"], {
        onText: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SwitchOn"),
        offText: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SwitchOff"),
        readOnly: state.roleDetails.isSystem,
        value: this.getValue("Public"),
        onChange: this.onSwitchToggle.bind(this, "isPublic")
      }))));
      var columnTwo =
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        key: "editor-container-columnTwo",
        className: "editor-container right-column"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "editor-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Label"], {
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("plRoleGroups"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("plRoleGroups.Help"),
        tooltipPlace: "top"
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Dropdown"], {
        value: this.getValue("RoleGroup"),
        enabled: !state.roleDetails.isSystem,
        options: this.getRoleGroupOptions(),
        style: {
          width: 100 + "%",
          float: "left"
        },
        onSelect: this.onDropDownChange.bind(this, "groupId")
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "new-group-container"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_RoleGroupEditor__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], {
        visible: this.state.createGroupVisible,
        onSave: this.onCreateGroup.bind(this),
        onCancel: this.onCancelCreateGroup.bind(this),
        onClick: this.doNothing.bind(this),
        showPopup: this.state.createGroupVisible,
        group: {
          id: -2,
          name: "",
          description: ""
        },
        title: "New Group"
      }))),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "editor-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Label"], {
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("securityModeListLabel"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("securityModeListLabel.Help"),
        tooltipPlace: "top"
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Dropdown"], {
        value: this.getValue("SecurityMode"),
        enabled: !state.roleDetails.isSystem,
        options: this.props.securityModeOptions,
        style: {
          width: 100 + "%",
          float: "left"
        },
        onSelect: this.onDropDownChange.bind(this, "securityMode")
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "editor-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["SingleLineInputWithError"], {
        value: state.roleDetails.rsvpCode,
        enabled: !state.roleDetails.isSystem,
        maxLength: 50,
        onChange: this.onTextChange.bind(this, "rsvpCode"),
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("plRSVPCode"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("plRSVPCode.Help"),
        inputStyle: {
          marginBottom: 5
        },
        tabIndex: 1
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "editor-row divider"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["SingleLineInputWithError"], {
        value: state.roleDetails.rsvpCode && state.roleDetails.rsvpCode !== "" ? props.rsvpLink + "&rsvp=" + state.roleDetails.rsvpCode : "",
        enabled: false,
        onChange: this.onTextChange.bind(this, "rsvpLink"),
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("plRSVPLink"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("plRSVPLink.Help"),
        inputStyle: {
          marginBottom: 0
        },
        tabIndex: 1
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "status-row"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "left"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Label"], {
        labelType: "inline",
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("AutoAssignment"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("AutoAssignment.Help"),
        tooltipPlace: "top"
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "right"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Switch"], {
        onText: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SwitchOn"),
        offText: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SwitchOff"),
        readOnly: state.roleDetails.isSystem,
        value: state.roleDetails.autoAssign,
        onChange: this.onSwitchToggle.bind(this, "autoAssign")
      }))), state.roleDetails.autoAssign &&
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "status-row"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "left"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Label"], {
        labelType: "inline",
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("AssignToExistUsers"),
        tooltipMessage: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("AssignToExistUsers.Help"),
        tooltipPlace: "top"
      })),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "right"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Switch"], {
        onText: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SwitchOn"),
        offText: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SwitchOff"),
        readOnly: state.roleDetails.isSystem,
        value: state.assignToUsers,
        onChange: this.onSwitchToggle.bind(this, "assignToUsers")
      }))));
      var children = [];
      children.push(columnOne);
      children.push(columnTwo);
      /* eslint-disable react/no-danger */

      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "role-details-editor"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["GridSystem"], {
          numberOfColumns: 2
        }, children),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "buttons-box"
        }, this.props.roleId > 0 && !state.roleDetails.isSystem && state.roleDetails.id > -1 ?
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Button"], {
          type: "secondary",
          onClick: this.deleteRole.bind(this)
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Delete")) : null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Button"], {
          type: "secondary",
          onClick: this.props.Collapse.bind(this)
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Cancel")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Button"], {
          type: "primary",
          onClick: this.addUpdateRoleDetails.bind(this)
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Save"))))
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

  return RolesEditor;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

RolesEditor.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  roleId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  roleDetails: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  roleGroupOptions: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  securityModeOptions: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  statusOptions: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  Collapse: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  refreshRolesList: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  roleGroups: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  currentGroupId: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  rsvpLink: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string
};

function mapStateToProps(state) {
  return {
    roleGroups: state.roles.roleGroups,
    rsvpLink: state.roles.rsvpLink
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(RolesEditor);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(RolesEditor, "RolesEditor", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleEditor\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleEditor\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\RoleEditor\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 36 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(2);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(147);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_2__);
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





var CollapsibleSwitcher =
/*#__PURE__*/
function (_Component) {
  _inherits(CollapsibleSwitcher, _Component);

  function CollapsibleSwitcher() {
    _classCallCheck(this, CollapsibleSwitcher);

    return _possibleConstructorReturn(this, _getPrototypeOf(CollapsibleSwitcher).call(this));
  }

  _createClass(CollapsibleSwitcher, [{
    key: "render",
    value: function render() {
      var props = this.props;
      return props.renderIndex >= 0 && props.children[props.renderIndex];
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }]);

  return CollapsibleSwitcher;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

CollapsibleSwitcher.propTypes = {
  renderIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  children: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array
};
CollapsibleSwitcher.defaultProps = {
  renderIndex: 0
};
var _default = CollapsibleSwitcher;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(CollapsibleSwitcher, "CollapsibleSwitcher", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\common\\CollapsibleSwitcher\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\common\\CollapsibleSwitcher\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 37 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var rolesActionTypes = {
  RETRIEVED_GROUPS_LIST: "RETRIEVED_GROUPS_LIST",
  RETRIEVED_ROLES_LIST: "RETRIEVED_ROLES_LIST",
  UPDATE_ROLEGROUP: "UPDATE_ROLEGROUP",
  DELETE_ROLEGROUP: "DELETE_ROLEGROUP",
  UPDATE_ROLE: "UPDATE_ROLE",
  DELETE_ROLE: "DELETE_ROLE",
  DELETE_USER_FROM_ROLE: "DELETE_USER_FROM_ROLE"
};
var _default = rolesActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(rolesActionTypes, "rolesActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\constants\\actionTypes\\roles.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\constants\\actionTypes\\roles.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 38 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

var roleUsersActionTypes = {
  RETRIEVED_SUGGEST_LIST: "RETRIEVED_SUGGEST_LIST",
  RETRIEVED_USERS_LIST: "RETRIEVED_USERS_LIST",
  ADD_USER_INTO_ROLE: "ADD_USER_INTO_ROLE",
  REMOVE_USER: "REMOVE_USER"
};
var _default = roleUsersActionTypes;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(roleUsersActionTypes, "roleUsersActionTypes", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\constants\\actionTypes\\roleUsers.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\constants\\actionTypes\\roleUsers.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 39 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
/* harmony import */ var _services_applicationService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(11);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();





function errorCallback(message) {
  _utils__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].utilities.notifyError(JSON.parse(message.responseText).Message, 5000);
}

var rolesActions = {
  getRoleGroupsList: function getRoleGroupsList(reload, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getRoleGroups(reload, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].RETRIEVED_GROUPS_LIST,
          data: {
            roleGroups: data
          }
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  saveRoleGroup: function saveRoleGroup(group, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].saveRoleGroup(group, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].UPDATE_ROLEGROUP,
          data: {
            roleGroup: data
          }
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  deleteRoleGroup: function deleteRoleGroup(group, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].deleteRoleGroup(group, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].DELETE_ROLEGROUP,
          data: {
            groupId: data.groupId
          }
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  getRolesList: function getRolesList(parameters, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getRoles(parameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].RETRIEVED_ROLES_LIST,
          data: {
            rolesList: data.roles
          },
          loadMore: data.loadMore,
          reload: parameters.reload,
          rsvpLink: data.rsvpLink
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  saveRole: function saveRole(currentGroupId, assignExistUsers, role, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].saveRole(assignExistUsers, role, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].UPDATE_ROLE,
          data: {
            roleDetails: data,
            currentGroupId: currentGroupId
          }
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  deleteRole: function deleteRole(role, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].deleteRole(role, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].DELETE_ROLE,
          data: {
            roleId: data.roleId
          }
        });

        if (callback) {
          callback();
        }
      }, errorCallback);
    };
  }
};
var _default = rolesActions;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(errorCallback, "errorCallback", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\actions\\roles.js");
  reactHotLoader.register(rolesActions, "rolesActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\actions\\roles.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\actions\\roles.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 40 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
/* harmony import */ var _services_applicationService__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(11);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(5);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();





function errorCallback(message) {
  _utils__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"].utilities.notifyError(JSON.parse(message.responseText).Message, 5000);
}

var roleUsersActions = {
  getSuggestUsers: function getSuggestUsers(parameters, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getSuggestUsers(parameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].RETRIEVED_SUGGEST_LIST,
          data: {
            matchedUsers: data
          }
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  getRoleUsers: function getRoleUsers(parameters, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].getRoleUsers(parameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].RETRIEVED_USERS_LIST,
          data: {
            roleUsers: data.users,
            totalRecords: data.totalRecords
          }
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  addUserToRole: function addUserToRole(parameters, notifyUser, isOwner, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].addUserToRole(parameters, notifyUser, isOwner, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].ADD_USER_INTO_ROLE,
          data: {
            roleUserDetails: data,
            isAdd: parameters.isAdd
          }
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  },
  removeUserFromRole: function removeUserFromRole(parameters, callback) {
    return function (dispatch) {
      _services_applicationService__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].removeUserFromRole(parameters, function (data) {
        dispatch({
          type: _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].REMOVE_USER,
          data: {
            UserId: data.UserId,
            RoleId: data.RoleId
          }
        });

        if (callback) {
          callback(data);
        }
      }, errorCallback);
    };
  }
};
var _default = roleUsersActions;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(errorCallback, "errorCallback", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\actions\\roleUsers.js");
  reactHotLoader.register(roleUsersActions, "roleUsersActions", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\actions\\roleUsers.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\actions\\roleUsers.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 41 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(81);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(15)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 42 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "polyfill", function() { return polyfill; });
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

function componentWillMount() {
  // Call this.constructor.gDSFP to support sub-classes.
  var state = this.constructor.getDerivedStateFromProps(this.props, this.state);
  if (state !== null && state !== undefined) {
    this.setState(state);
  }
}

function componentWillReceiveProps(nextProps) {
  // Call this.constructor.gDSFP to support sub-classes.
  // Use the setState() updater to ensure state isn't stale in certain edge cases.
  function updater(prevState) {
    var state = this.constructor.getDerivedStateFromProps(nextProps, prevState);
    return state !== null && state !== undefined ? state : null;
  }
  // Binding "this" is important for shallow renderer support.
  this.setState(updater.bind(this));
}

function componentWillUpdate(nextProps, nextState) {
  try {
    var prevProps = this.props;
    var prevState = this.state;
    this.props = nextProps;
    this.state = nextState;
    this.__reactInternalSnapshotFlag = true;
    this.__reactInternalSnapshot = this.getSnapshotBeforeUpdate(
      prevProps,
      prevState
    );
  } finally {
    this.props = prevProps;
    this.state = prevState;
  }
}

// React may warn about cWM/cWRP/cWU methods being deprecated.
// Add a flag to suppress these warnings for this special case.
componentWillMount.__suppressDeprecationWarning = true;
componentWillReceiveProps.__suppressDeprecationWarning = true;
componentWillUpdate.__suppressDeprecationWarning = true;

function polyfill(Component) {
  var prototype = Component.prototype;

  if (!prototype || !prototype.isReactComponent) {
    throw new Error('Can only polyfill class components');
  }

  if (
    typeof Component.getDerivedStateFromProps !== 'function' &&
    typeof prototype.getSnapshotBeforeUpdate !== 'function'
  ) {
    return Component;
  }

  // If new component APIs are defined, "unsafe" lifecycles won't be called.
  // Error if any of these lifecycles are present,
  // Because they would work differently between older and newer (16.3+) versions of React.
  var foundWillMountName = null;
  var foundWillReceivePropsName = null;
  var foundWillUpdateName = null;
  if (typeof prototype.componentWillMount === 'function') {
    foundWillMountName = 'componentWillMount';
  } else if (typeof prototype.UNSAFE_componentWillMount === 'function') {
    foundWillMountName = 'UNSAFE_componentWillMount';
  }
  if (typeof prototype.componentWillReceiveProps === 'function') {
    foundWillReceivePropsName = 'componentWillReceiveProps';
  } else if (typeof prototype.UNSAFE_componentWillReceiveProps === 'function') {
    foundWillReceivePropsName = 'UNSAFE_componentWillReceiveProps';
  }
  if (typeof prototype.componentWillUpdate === 'function') {
    foundWillUpdateName = 'componentWillUpdate';
  } else if (typeof prototype.UNSAFE_componentWillUpdate === 'function') {
    foundWillUpdateName = 'UNSAFE_componentWillUpdate';
  }
  if (
    foundWillMountName !== null ||
    foundWillReceivePropsName !== null ||
    foundWillUpdateName !== null
  ) {
    var componentName = Component.displayName || Component.name;
    var newApiName =
      typeof Component.getDerivedStateFromProps === 'function'
        ? 'getDerivedStateFromProps()'
        : 'getSnapshotBeforeUpdate()';

    throw Error(
      'Unsafe legacy lifecycles will not be called for components using new component APIs.\n\n' +
        componentName +
        ' uses ' +
        newApiName +
        ' but also contains the following legacy lifecycles:' +
        (foundWillMountName !== null ? '\n  ' + foundWillMountName : '') +
        (foundWillReceivePropsName !== null
          ? '\n  ' + foundWillReceivePropsName
          : '') +
        (foundWillUpdateName !== null ? '\n  ' + foundWillUpdateName : '') +
        '\n\nThe above lifecycles should be removed. Learn more about this warning here:\n' +
        'https://fb.me/react-async-component-lifecycle-hooks'
    );
  }

  // React <= 16.2 does not support static getDerivedStateFromProps.
  // As a workaround, use cWM and cWRP to invoke the new static lifecycle.
  // Newer versions of React will ignore these lifecycles if gDSFP exists.
  if (typeof Component.getDerivedStateFromProps === 'function') {
    prototype.componentWillMount = componentWillMount;
    prototype.componentWillReceiveProps = componentWillReceiveProps;
  }

  // React <= 16.2 does not support getSnapshotBeforeUpdate.
  // As a workaround, use cWU to invoke the new lifecycle.
  // Newer versions of React will ignore that lifecycle if gSBU exists.
  if (typeof prototype.getSnapshotBeforeUpdate === 'function') {
    if (typeof prototype.componentDidUpdate !== 'function') {
      throw new Error(
        'Cannot polyfill getSnapshotBeforeUpdate() for components that do not define componentDidUpdate() on the prototype'
      );
    }

    prototype.componentWillUpdate = componentWillUpdate;

    var componentDidUpdate = prototype.componentDidUpdate;

    prototype.componentDidUpdate = function componentDidUpdatePolyfill(
      prevProps,
      prevState,
      maybeSnapshot
    ) {
      // 16.3+ will not execute our will-update method;
      // It will pass a snapshot value to did-update though.
      // Older versions will require our polyfilled will-update value.
      // We need to handle both cases, but can't just check for the presence of "maybeSnapshot",
      // Because for <= 15.x versions this might be a "prevContext" object.
      // We also can't just check "__reactInternalSnapshot",
      // Because get-snapshot might return a falsy value.
      // So check for the explicit __reactInternalSnapshotFlag flag to determine behavior.
      var snapshot = this.__reactInternalSnapshotFlag
        ? this.__reactInternalSnapshot
        : maybeSnapshot;

      componentDidUpdate.call(this, prevProps, prevState, snapshot);
    };
  }

  return Component;
}




/***/ }),
/* 43 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = createChainableTypeChecker;
/**
 * Copyright 2013-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 */

// Mostly taken from ReactPropTypes.

function createChainableTypeChecker(validate) {
  function checkType(isRequired, props, propName, componentName, location, propFullName) {
    var componentNameSafe = componentName || '<<anonymous>>';
    var propFullNameSafe = propFullName || propName;

    if (props[propName] == null) {
      if (isRequired) {
        return new Error('Required ' + location + ' `' + propFullNameSafe + '` was not specified ' + ('in `' + componentNameSafe + '`.'));
      }

      return null;
    }

    for (var _len = arguments.length, args = Array(_len > 6 ? _len - 6 : 0), _key = 6; _key < _len; _key++) {
      args[_key - 6] = arguments[_key];
    }

    return validate.apply(undefined, [props, propName, componentNameSafe, location, propFullNameSafe].concat(args));
  }

  var chainedCheckType = checkType.bind(null, false);
  chainedCheckType.isRequired = checkType.bind(null, true);

  return chainedCheckType;
}
module.exports = exports['default'];

/***/ }),
/* 44 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.defaultGetDataState = defaultGetDataState;
exports.getCommonListProps = getCommonListProps;
exports.default = reduceToListState;

var _Filter = __webpack_require__(45);

var _ = __webpack_require__(20);

var _dataHelpers = __webpack_require__(27);

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

var EMPTY_VALUE = {};

var returnFalse = function returnFalse() {
  return false;
};

function defaultGetDataState(data, _ref, lastState) {
  var groupBy = _ref.groupBy;

  if (lastState === void 0) {
    lastState = {};
  }

  if (lastState.data !== data || lastState.groupBy !== groupBy) {
    if (!groupBy) return {};
    var keys = [];
    var groups = (0, _.groupBySortedKeys)(groupBy, data, keys);
    return {
      data: data,
      groupBy: groupBy,
      groups: groups,
      sortedKeys: keys,
      sequentialData: Object.keys(groups).reduce(function (flat, grp) {
        return flat.concat(groups[grp]);
      }, [])
    };
  }

  return lastState;
}

var getStateGetterFromList = function getStateGetterFromList(_ref2) {
  var l = _ref2.listComponent;
  return l && l.getDataState;
};

var getIsDisabled = function getIsDisabled(disabledProp, valueField) {
  return !Array.isArray(disabledProp) ? returnFalse : function (item) {
    return disabledProp.some(function (i) {
      return (0, _dataHelpers.dataValue)(item, valueField) === (0, _dataHelpers.dataValue)(i, valueField);
    });
  };
};

function getCommonListProps(list, accessors, _ref3) {
  var groupBy = _ref3.groupBy,
      optionComponent = _ref3.optionComponent,
      itemComponent = _ref3.itemComponent,
      groupComponent = _ref3.groupComponent,
      searchTerm = _ref3.searchTerm,
      listProps = _ref3.listProps;
  return _extends({
    searchTerm: searchTerm,
    groupBy: groupBy,
    groupComponent: groupComponent,
    itemComponent: itemComponent,
    optionComponent: optionComponent
  }, listProps, {
    data: list.data,
    dataState: list.state,
    textAccessor: accessors.text,
    valueAccessor: accessors.value
  });
}

function reduceToListState(nextListData, prevList, _temp) {
  var _ref4 = _temp === void 0 ? {} : _temp,
      nextProps = _ref4.nextProps,
      getDataState = _ref4.getDataState;

  var disabled = nextProps.disabled,
      valueField = nextProps.valueField,
      textField = nextProps.textField;
  getDataState = getDataState || getStateGetterFromList(nextProps) || defaultGetDataState;
  var dataState = getDataState(nextListData, nextProps, prevList && prevList.dataState);
  var data = dataState && dataState.sequentialData || nextListData;
  var isDisabled = getIsDisabled(disabled, valueField);

  var moveNext = function moveNext(item, word) {
    return isDisabled(item) || word && !_Filter.presets.startsWith((0, _dataHelpers.dataText)(item, textField).toLowerCase(), word.toLowerCase());
  };

  var list = {
    dataState: dataState,
    isDisabled: isDisabled,
    first: function first() {
      return list.next(EMPTY_VALUE);
    },
    last: function last() {
      return list.prevEnabled(data[data.length - 1]);
    },
    prev: function prev(item, word) {
      var nextIdx = Math.max(0, data.indexOf(item)) - 1;

      while (nextIdx > -1 && moveNext(data[nextIdx], word)) {
        nextIdx--;
      }

      if (nextIdx >= 0) return data[nextIdx];
      return isDisabled(item) ? null : item;
    },
    next: function next(item, word) {
      var nextIdx = data.indexOf(item) + 1;

      while (nextIdx < data.length && moveNext(data[nextIdx], word)) {
        nextIdx++;
      }

      if (nextIdx < data.length) return data[nextIdx];
      return isDisabled(item) ? null : item;
    },
    prevEnabled: function prevEnabled(item) {
      return isDisabled(item) ? list.prev(item) : item;
    },
    nextEnabled: function nextEnabled(item) {
      return isDisabled(item) ? list.next(item) : item;
    }
  };
  return list;
}

/***/ }),
/* 45 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.indexOf = indexOf;
exports.filter = filter;
exports.suggest = suggest;
exports.propTypes = exports.presets = void 0;

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var CustomPropTypes = _interopRequireWildcard(__webpack_require__(19));

var _dataHelpers = __webpack_require__(27);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

var presets = {
  eq: function eq(a, b) {
    return a === b;
  },
  neq: function neq(a, b) {
    return a !== b;
  },
  gt: function gt(a, b) {
    return a > b;
  },
  gte: function gte(a, b) {
    return a >= b;
  },
  lt: function lt(a, b) {
    return a < b;
  },
  lte: function lte(a, b) {
    return a <= b;
  },
  contains: function contains(a, b) {
    return a.indexOf(b) !== -1;
  },
  startsWith: function startsWith(a, b) {
    return a.lastIndexOf(b, 0) === 0;
  },
  endsWith: function endsWith(a, b) {
    var pos = a.length - b.length;
    var lastIndex = a.indexOf(b, pos);
    return lastIndex !== -1 && lastIndex === pos;
  }
};
exports.presets = presets;

function normalizeFilterType(type) {
  if (type === false) return null;
  if (type === true) return 'startsWith';
  return type || 'eq';
}

function normalizeFilter(_ref) {
  var filter = _ref.filter,
      _ref$caseSensitive = _ref.caseSensitive,
      caseSensitive = _ref$caseSensitive === void 0 ? false : _ref$caseSensitive,
      textField = _ref.textField;
  filter = normalizeFilterType(filter);

  if (typeof filter === 'function' || !filter) {
    return filter;
  }

  filter = presets[filter];
  return function (item, searchTerm) {
    var textValue = (0, _dataHelpers.dataText)(item, textField);

    if (!caseSensitive) {
      textValue = textValue.toLowerCase();
      searchTerm = searchTerm.toLowerCase();
    }

    return filter(textValue, searchTerm);
  };
}

function normalizeOptions(nextOptions) {
  var options = _extends({}, nextOptions);

  options.minLengh = options.minLengh || 0;
  options.filter = normalizeFilter(options);
  return options;
}

var propTypes = {
  textField: CustomPropTypes.accessor,
  caseSensitive: _propTypes.default.bool,
  minLength: _propTypes.default.number,
  filter: _propTypes.default.oneOfType([_propTypes.default.func, _propTypes.default.bool, _propTypes.default.oneOf(Object.keys(presets))])
};
exports.propTypes = propTypes;

function indexOf(data, _ref2) {
  var _ref2$searchTerm = _ref2.searchTerm,
      searchTerm = _ref2$searchTerm === void 0 ? '' : _ref2$searchTerm,
      options = _objectWithoutProperties(_ref2, ["searchTerm"]);

  var _normalizeOptions = normalizeOptions(options),
      filter = _normalizeOptions.filter,
      minLength = _normalizeOptions.minLength;

  if (!filter || !searchTerm || !searchTerm.trim() || searchTerm.length < minLength) return -1;

  for (var idx = 0; idx < data.length; idx++) {
    if (filter(data[idx], searchTerm, idx)) return idx;
  }

  return -1;
}

function filter(data, _ref3) {
  var _ref3$searchTerm = _ref3.searchTerm,
      searchTerm = _ref3$searchTerm === void 0 ? '' : _ref3$searchTerm,
      options = _objectWithoutProperties(_ref3, ["searchTerm"]);

  var _normalizeOptions2 = normalizeOptions(options),
      filter = _normalizeOptions2.filter,
      minLength = _normalizeOptions2.minLength;

  if (!filter || !searchTerm || !searchTerm.trim() || searchTerm.length < minLength) return data;
  return data.filter(function (item, idx) {
    return filter(item, searchTerm, idx);
  });
}

function suggest(data, _ref4) {
  var _ref4$searchTerm = _ref4.searchTerm,
      searchTerm = _ref4$searchTerm === void 0 ? '' : _ref4$searchTerm,
      options = _objectWithoutProperties(_ref4, ["searchTerm"]);

  var _normalizeOptions3 = normalizeOptions(options),
      filter = _normalizeOptions3.filter,
      minLength = _normalizeOptions3.minLength;

  if (!filter || !searchTerm || !searchTerm.trim() || searchTerm.length < minLength) return searchTerm;

  for (var idx = 0; idx < data.length; idx++) {
    if (filter(data[idx], searchTerm, idx)) return data[idx];
  }

  return searchTerm;
}

/***/ }),
/* 46 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.getMessages = getMessages;

var _react = _interopRequireDefault(__webpack_require__(0));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var messages = {
  moveBack: 'Navigate back',
  moveForward: 'Navigate forward',
  dateButton: 'Select date',
  timeButton: 'Select time',
  openCombobox: 'open combobox',
  openDropdown: 'open dropdown',
  placeholder: '',
  filterPlaceholder: '',
  emptyList: 'There are no items in this list',
  emptyFilter: 'The filter returned no results',
  createOption: function createOption(_ref) {
    var searchTerm = _ref.searchTerm;
    return [' Create option', searchTerm && ' ', searchTerm && _react.default.createElement("strong", {
      key: "_"
    }, "\"" + searchTerm + "\"")];
  },
  tagsLabel: 'Selected items',
  removeLabel: 'Remove selected item',
  noneSelected: 'no selected items',
  selectedItems: function selectedItems(labels) {
    return "Selected items: " + labels.join(', ');
  },
  // number
  increment: 'Increment value',
  decrement: 'Decrement value'
};

function getMessages(defaults) {
  if (defaults === void 0) {
    defaults = {};
  }

  var processed = {};
  Object.keys(messages).forEach(function (message) {
    var value = defaults[message];
    if (value == null) value = messages[message];
    processed[message] = typeof value === 'function' ? value : function () {
      return value;
    };
  });
  return processed;
}

/***/ }),
/* 47 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = void 0;

var _inDOM = _interopRequireDefault(__webpack_require__(17));

var on = function on() {};

if (_inDOM.default) {
  on = function () {
    if (document.addEventListener) return function (node, eventName, handler, capture) {
      return node.addEventListener(eventName, handler, capture || false);
    };else if (document.attachEvent) return function (node, eventName, handler) {
      return node.attachEvent('on' + eventName, function (e) {
        e = e || window.event;
        e.target = e.target || e.srcElement;
        e.currentTarget = node;
        handler.call(node, e);
      });
    };
  }();
}

var _default = on;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 48 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = void 0;

var _inDOM = _interopRequireDefault(__webpack_require__(17));

var off = function off() {};

if (_inDOM.default) {
  off = function () {
    if (document.addEventListener) return function (node, eventName, handler, capture) {
      return node.removeEventListener(eventName, handler, capture || false);
    };else if (document.attachEvent) return function (node, eventName, handler) {
      return node.detachEvent('on' + eventName, handler);
    };
  }();
}

var _default = off;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 49 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = void 0;

var _inDOM = _interopRequireDefault(__webpack_require__(17));

var _default = function () {
  // HTML DOM and SVG DOM may have different support levels,
  // so we need to check on context instead of a document root element.
  return _inDOM.default ? function (context, node) {
    if (context.contains) {
      return context.contains(node);
    } else if (context.compareDocumentPosition) {
      return context === node || !!(context.compareDocumentPosition(node) & 16);
    } else {
      return fallback(context, node);
    }
  } : fallback;
}();

exports.default = _default;

function fallback(context, node) {
  if (node) do {
    if (node === context) return true;
  } while (node = node.parentNode);
  return false;
}

module.exports = exports["default"];

/***/ }),
/* 50 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = qsa;
// Zepto.js
// (c) 2010-2015 Thomas Fuchs
// Zepto.js may be freely distributed under the MIT license.
var simpleSelectorRE = /^[\w-]*$/;
var toArray = Function.prototype.bind.call(Function.prototype.call, [].slice);

function qsa(element, selector) {
  var maybeID = selector[0] === '#',
      maybeClass = selector[0] === '.',
      nameOnly = maybeID || maybeClass ? selector.slice(1) : selector,
      isSimple = simpleSelectorRE.test(nameOnly),
      found;

  if (isSimple) {
    if (maybeID) {
      element = element.getElementById ? element : document;
      return (found = element.getElementById(nameOnly)) ? [found] : [];
    }

    if (element.getElementsByClassName && maybeClass) return toArray(element.getElementsByClassName(nameOnly));
    return toArray(element.getElementsByTagName(selector));
  }

  return toArray(element.querySelectorAll(selector));
}

module.exports = exports["default"];

/***/ }),
/* 51 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = style;

var _camelizeStyle = _interopRequireDefault(__webpack_require__(52));

var _hyphenateStyle = _interopRequireDefault(__webpack_require__(116));

var _getComputedStyle2 = _interopRequireDefault(__webpack_require__(118));

var _removeStyle = _interopRequireDefault(__webpack_require__(119));

var _properties = __webpack_require__(53);

var _isTransform = _interopRequireDefault(__webpack_require__(120));

function style(node, property, value) {
  var css = '';
  var transforms = '';
  var props = property;

  if (typeof property === 'string') {
    if (value === undefined) {
      return node.style[(0, _camelizeStyle.default)(property)] || (0, _getComputedStyle2.default)(node).getPropertyValue((0, _hyphenateStyle.default)(property));
    } else {
      (props = {})[property] = value;
    }
  }

  Object.keys(props).forEach(function (key) {
    var value = props[key];

    if (!value && value !== 0) {
      (0, _removeStyle.default)(node, (0, _hyphenateStyle.default)(key));
    } else if ((0, _isTransform.default)(key)) {
      transforms += key + "(" + value + ") ";
    } else {
      css += (0, _hyphenateStyle.default)(key) + ": " + value + ";";
    }
  });

  if (transforms) {
    css += _properties.transform + ": " + transforms + ";";
  }

  node.style.cssText += ';' + css;
}

module.exports = exports["default"];

/***/ }),
/* 52 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = camelizeStyleName;

var _camelize = _interopRequireDefault(__webpack_require__(115));

/**
 * Copyright 2014-2015, Facebook, Inc.
 * All rights reserved.
 * https://github.com/facebook/react/blob/2aeb8a2a6beb00617a4217f7f8284924fa2ad819/src/vendor/core/camelizeStyleName.js
 */
var msPattern = /^-ms-/;

function camelizeStyleName(string) {
  return (0, _camelize.default)(string.replace(msPattern, 'ms-'));
}

module.exports = exports["default"];

/***/ }),
/* 53 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = exports.animationEnd = exports.animationDelay = exports.animationTiming = exports.animationDuration = exports.animationName = exports.transitionEnd = exports.transitionDuration = exports.transitionDelay = exports.transitionTiming = exports.transitionProperty = exports.transform = void 0;

var _inDOM = _interopRequireDefault(__webpack_require__(17));

var transform = 'transform';
exports.transform = transform;
var prefix, transitionEnd, animationEnd;
exports.animationEnd = animationEnd;
exports.transitionEnd = transitionEnd;
var transitionProperty, transitionDuration, transitionTiming, transitionDelay;
exports.transitionDelay = transitionDelay;
exports.transitionTiming = transitionTiming;
exports.transitionDuration = transitionDuration;
exports.transitionProperty = transitionProperty;
var animationName, animationDuration, animationTiming, animationDelay;
exports.animationDelay = animationDelay;
exports.animationTiming = animationTiming;
exports.animationDuration = animationDuration;
exports.animationName = animationName;

if (_inDOM.default) {
  var _getTransitionPropert = getTransitionProperties();

  prefix = _getTransitionPropert.prefix;
  exports.transitionEnd = transitionEnd = _getTransitionPropert.transitionEnd;
  exports.animationEnd = animationEnd = _getTransitionPropert.animationEnd;
  exports.transform = transform = prefix + "-" + transform;
  exports.transitionProperty = transitionProperty = prefix + "-transition-property";
  exports.transitionDuration = transitionDuration = prefix + "-transition-duration";
  exports.transitionDelay = transitionDelay = prefix + "-transition-delay";
  exports.transitionTiming = transitionTiming = prefix + "-transition-timing-function";
  exports.animationName = animationName = prefix + "-animation-name";
  exports.animationDuration = animationDuration = prefix + "-animation-duration";
  exports.animationTiming = animationTiming = prefix + "-animation-delay";
  exports.animationDelay = animationDelay = prefix + "-animation-timing-function";
}

var _default = {
  transform: transform,
  end: transitionEnd,
  property: transitionProperty,
  timing: transitionTiming,
  delay: transitionDelay,
  duration: transitionDuration
};
exports.default = _default;

function getTransitionProperties() {
  var style = document.createElement('div').style;
  var vendorMap = {
    O: function O(e) {
      return "o" + e.toLowerCase();
    },
    Moz: function Moz(e) {
      return e.toLowerCase();
    },
    Webkit: function Webkit(e) {
      return "webkit" + e;
    },
    ms: function ms(e) {
      return "MS" + e;
    }
  };
  var vendors = Object.keys(vendorMap);
  var transitionEnd, animationEnd;
  var prefix = '';

  for (var i = 0; i < vendors.length; i++) {
    var vendor = vendors[i];

    if (vendor + "TransitionProperty" in style) {
      prefix = "-" + vendor.toLowerCase();
      transitionEnd = vendorMap[vendor]('TransitionEnd');
      animationEnd = vendorMap[vendor]('AnimationEnd');
      break;
    }
  }

  if (!transitionEnd && 'transitionProperty' in style) transitionEnd = 'transitionend';
  if (!animationEnd && 'animationName' in style) animationEnd = 'animationend';
  style = null;
  return {
    animationEnd: animationEnd,
    transitionEnd: transitionEnd,
    prefix: prefix
  };
}

/***/ }),
/* 54 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = offset;

var _contains = _interopRequireDefault(__webpack_require__(49));

var _isWindow = _interopRequireDefault(__webpack_require__(21));

var _ownerDocument = _interopRequireDefault(__webpack_require__(121));

function offset(node) {
  var doc = (0, _ownerDocument.default)(node),
      win = (0, _isWindow.default)(doc),
      docElem = doc && doc.documentElement,
      box = {
    top: 0,
    left: 0,
    height: 0,
    width: 0
  };
  if (!doc) return; // Make sure it's not a disconnected DOM node

  if (!(0, _contains.default)(docElem, node)) return box;
  if (node.getBoundingClientRect !== undefined) box = node.getBoundingClientRect(); // IE8 getBoundingClientRect doesn't support width & height

  box = {
    top: box.top + (win.pageYOffset || docElem.scrollTop) - (docElem.clientTop || 0),
    left: box.left + (win.pageXOffset || docElem.scrollLeft) - (docElem.clientLeft || 0),
    width: (box.width == null ? node.offsetWidth : box.width) || 0,
    height: (box.height == null ? node.offsetHeight : box.height) || 0
  };
  return box;
}

module.exports = exports["default"];

/***/ }),
/* 55 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;

var _spyOnComponent = _interopRequireDefault(__webpack_require__(22));

exports.spyOnComponent = _spyOnComponent.default;

var _autoFocus = _interopRequireDefault(__webpack_require__(129));

exports.autoFocus = _autoFocus.default;

var _focusManager = _interopRequireDefault(__webpack_require__(130));

exports.focusManager = _focusManager.default;

var _mountManager = _interopRequireDefault(__webpack_require__(29));

exports.mountManager = _mountManager.default;

var _timeoutManager = _interopRequireDefault(__webpack_require__(56));

exports.timeoutManager = _timeoutManager.default;

var _mixin = _interopRequireDefault(__webpack_require__(131));

exports.mixin = _mixin.default;

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

/***/ }),
/* 56 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = createTimeoutManager;

var _spyOnComponent = _interopRequireDefault(__webpack_require__(22));

var _mountManager = _interopRequireDefault(__webpack_require__(29));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function createTimeoutManager(componentInstance) {
  var isMounted = (0, _mountManager.default)(componentInstance);
  var timers = Object.create(null);
  var manager;
  (0, _spyOnComponent.default)(componentInstance, {
    componentWillUnmount: function componentWillUnmount() {
      for (var k in timers) {
        clearTimeout(timers[k]);
      }

      timers = null;
    }
  });
  return manager = {
    clear: function clear(key) {
      clearTimeout(timers[key]);
    },
    set: function set(key, fn, ms) {
      if (!isMounted()) return;
      manager.clear(key);
      timers[key] = setTimeout(fn, ms);
    }
  };
}

module.exports = exports["default"];

/***/ }),
/* 57 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.widgetEditable = exports.widgetEnabled = exports.isInDisabledFieldset = void 0;

var _reactDom = __webpack_require__(16);

var _matches = _interopRequireDefault(__webpack_require__(132));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var isInDisabledFieldset = function isInDisabledFieldset(inst) {
  var node;

  try {
    node = (0, _reactDom.findDOMNode)(inst);
  } catch (err) {
    /* ignore */
  }

  return !!node && (0, _matches.default)(node, 'fieldset[disabled] *');
};

exports.isInDisabledFieldset = isInDisabledFieldset;
var widgetEnabled = interactionDecorator(true);
exports.widgetEnabled = widgetEnabled;
var widgetEditable = interactionDecorator(false);
exports.widgetEditable = widgetEditable;

function interactionDecorator(disabledOnly) {
  function wrap(method) {
    return function decoratedMethod() {
      var _this$props = this.props,
          disabled = _this$props.disabled,
          readOnly = _this$props.readOnly;
      disabled = isInDisabledFieldset(this) || disabled == true || !disabledOnly && readOnly === true;

      for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
        args[_key] = arguments[_key];
      }

      if (!disabled) return method.apply(this, args);
    };
  }

  return function decorate(target, key, desc) {
    if (desc.initializer) {
      var init = desc.initializer;

      desc.initializer = function () {
        return wrap(init.call(this)).bind(this);
      };
    } else desc.value = wrap(desc.value);

    return desc;
  };
}

/***/ }),
/* 58 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return configureStore; });
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(18);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(31);
/* harmony import */ var redux_thunk__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_thunk__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(59);
/* harmony import */ var redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_immutable_state_invariant__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _reducers_rootReducer__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(60);
/* harmony import */ var _containers_DevTools__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(63);
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

  reactHotLoader.register(IS_PRODUCTION, "IS_PRODUCTION", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\store\\configureStore.js");
  reactHotLoader.register(configureStore, "configureStore", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\store\\configureStore.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 59 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxImmutableStateInvariant;

/***/ }),
/* 60 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(18);
/* harmony import */ var redux__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(redux__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _rolesReducer__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(61);
/* harmony import */ var _roleUsersReducer__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(62);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();




var rootReducer = Object(redux__WEBPACK_IMPORTED_MODULE_0__["combineReducers"])({
  roles: _rolesReducer__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"],
  roleUsers: _roleUsersReducer__WEBPACK_IMPORTED_MODULE_2__[/* default */ "a"]
});
var _default = rootReducer;
/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(rootReducer, "rootReducer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\reducers\\rootReducer.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\reducers\\rootReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 61 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return rolesReducer; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(1);
/* harmony import */ var _components_roles_helpers_roles__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(13);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }




function rolesReducer() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    roleGroups: [],
    rolesList: [],
    loadMore: true,
    roleDetails: {},
    roleGroup: {}
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].RETRIEVED_GROUPS_LIST:
      {
        var roleGroups = [{
          id: -2,
          name: _resources__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].get("AllGroups"),
          description: _resources__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].get("AllGroups")
        }, {
          id: -1,
          name: _resources__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].get("GlobalRolesGroup"),
          description: _resources__WEBPACK_IMPORTED_MODULE_1__[/* default */ "a"].get("GlobalRolesGroup")
        }].concat(action.data.roleGroups);
        return _objectSpread2(_objectSpread2({}, state), {}, {
          roleGroups: roleGroups
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].RETRIEVED_ROLES_LIST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        rolesList: action.reload ? action.data.rolesList : state.rolesList.concat(action.data.rolesList),
        loadMore: action.loadMore,
        rsvpLink: action.rsvpLink
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].UPDATE_ROLEGROUP:
      {
        var _roleGroups = Object.assign([], JSON.parse(JSON.stringify(state.roleGroups)));

        return _objectSpread2(_objectSpread2({}, state), {}, {
          roleGroups: Object(_components_roles_helpers_roles__WEBPACK_IMPORTED_MODULE_2__[/* updateRoleGroupList */ "f"])(_roleGroups, action.data.roleGroup)
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].UPDATE_ROLE:
      {
        var rolesList = Object.assign([], JSON.parse(JSON.stringify(state.rolesList)));

        if (action.data.currentGroupId === action.data.roleDetails.groupId || action.data.currentGroupId === -2) {
          return _objectSpread2(_objectSpread2({}, state), {}, {
            rolesList: Object(_components_roles_helpers_roles__WEBPACK_IMPORTED_MODULE_2__[/* updateRolesList */ "g"])(rolesList, action.data.roleDetails)
          });
        }

        return _objectSpread2(_objectSpread2({}, state), {}, {
          rolesList: Object(_components_roles_helpers_roles__WEBPACK_IMPORTED_MODULE_2__[/* removeFromRolesList */ "e"])(rolesList, action.data.roleDetails.id)
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].DELETE_ROLE:
      {
        var _rolesList = Object.assign([], JSON.parse(JSON.stringify(state.rolesList)));

        return _objectSpread2(_objectSpread2({}, state), {}, {
          rolesList: Object(_components_roles_helpers_roles__WEBPACK_IMPORTED_MODULE_2__[/* removeFromRolesList */ "e"])(_rolesList, action.data.roleId)
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roles */ "b"].DELETE_ROLEGROUP:
      {
        var _roleGroups2 = Object.assign([], JSON.parse(JSON.stringify(state.roleGroups)));

        return _objectSpread2(_objectSpread2({}, state), {}, {
          roleGroups: Object(_components_roles_helpers_roles__WEBPACK_IMPORTED_MODULE_2__[/* removeFromRoleGroupList */ "d"])(_roleGroups2, action.data.groupId)
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].ADD_USER_INTO_ROLE:
      {
        if (action.data.isAdd) {
          var _rolesList2 = Object.assign([], JSON.parse(JSON.stringify(state.rolesList)));

          return _objectSpread2(_objectSpread2({}, state), {}, {
            rolesList: Object(_components_roles_helpers_roles__WEBPACK_IMPORTED_MODULE_2__[/* incrementUsersCountFromRoleList */ "c"])(_rolesList2, action.data.roleUserDetails.roleId)
          });
        }

        return _objectSpread2({}, state);
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].REMOVE_USER:
      {
        var _rolesList3 = Object.assign([], JSON.parse(JSON.stringify(state.rolesList)));

        return _objectSpread2(_objectSpread2({}, state), {}, {
          rolesList: Object(_components_roles_helpers_roles__WEBPACK_IMPORTED_MODULE_2__[/* decrementUsersCountFromRoleList */ "b"])(_rolesList3, action.data.RoleId)
        });
      }

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

  reactHotLoader.register(rolesReducer, "rolesReducer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\reducers\\rolesReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 62 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "a", function() { return roleUsersReducer; });
/* harmony import */ var _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(7);
/* harmony import */ var _components_roles_helpers_roleUsers__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(32);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();

function _objectSpread2(target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i] != null ? arguments[i] : {}; var ownKeys = Object.keys(source); if (typeof Object.getOwnPropertySymbols === 'function') { ownKeys = ownKeys.concat(Object.getOwnPropertySymbols(source).filter(function (sym) { return Object.getOwnPropertyDescriptor(source, sym).enumerable; })); } ownKeys.forEach(function (key) { _defineProperty(target, key, source[key]); }); } return target; }

function _defineProperty(obj, key, value) { if (key in obj) { Object.defineProperty(obj, key, { value: value, enumerable: true, configurable: true, writable: true }); } else { obj[key] = value; } return obj; }



function roleUsersReducer() {
  var state = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {
    matchedUsers: [],
    roleUsers: [],
    totalRecords: 0
  };
  var action = arguments.length > 1 ? arguments[1] : undefined;

  switch (action.type) {
    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].RETRIEVED_SUGGEST_LIST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        matchedUsers: action.data.matchedUsers
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].RETRIEVED_USERS_LIST:
      return _objectSpread2(_objectSpread2({}, state), {}, {
        roleUsers: action.data.roleUsers,
        totalRecords: action.data.totalRecords
      });

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].ADD_USER_INTO_ROLE:
      {
        var roleUsers = Object.assign([], JSON.parse(JSON.stringify(state.roleUsers)));
        return _objectSpread2(_objectSpread2({}, state), {}, {
          roleUsers: Object(_components_roles_helpers_roleUsers__WEBPACK_IMPORTED_MODULE_1__[/* updateRoleUserList */ "b"])(roleUsers, action.data.roleUserDetails)
        });
      }

    case _constants_actionTypes__WEBPACK_IMPORTED_MODULE_0__[/* roleUsers */ "a"].REMOVE_USER:
      {
        var _roleUsers = Object.assign([], JSON.parse(JSON.stringify(state.roleUsers)));

        return _objectSpread2(_objectSpread2({}, state), {}, {
          roleUsers: Object(_components_roles_helpers_roleUsers__WEBPACK_IMPORTED_MODULE_1__[/* removeRoleUserFromList */ "a"])(_roleUsers, action.data.UserId)
        });
      }

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

  reactHotLoader.register(roleUsersReducer, "roleUsersReducer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\reducers\\roleUsersReducer.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 63 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(64);
/* harmony import */ var redux_devtools__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(redux_devtools__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(65);
/* harmony import */ var redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(redux_devtools_log_monitor__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var redux_devtools_dock_monitor__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(66);
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

  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\containers\\DevTools.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 64 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevTools;

/***/ }),
/* 65 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsLogMonitor;

/***/ }),
/* 66 */
/***/ (function(module, exports) {

module.exports = window.dnn.nodeModules.ReduxDevToolsDockMonitor;

/***/ }),
/* 67 */
/***/ (function(module, exports, __webpack_require__) {

/* eslint-disable no-undef */
if (true) {
  module.exports = __webpack_require__(75);
} else {}

/***/ }),
/* 68 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(2);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _roles__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(69);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(1);
/* harmony import */ var utils__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(5);
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

var Root =
/*#__PURE__*/
function (_Component) {
  _inherits(Root, _Component);

  function Root() {
    var _this;

    _classCallCheck(this, Root);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(Root).call(this));
    canEdit = utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].settings.isHost || utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].settings.isAdmin || utils__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].settings.permissions.EDIT;
    return _this;
  }

  _createClass(Root, [{
    key: "onCreate",
    value: function onCreate() {
      this.rolesPanelref.getWrappedInstance().onAddRole();
    }
  }, {
    key: "render",
    value: function render() {
      var _this2 = this;

      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "roles-app"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPage"], {
          isOpen: true
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["PersonaBarPageHeader"], {
          title: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Roles")
        }, canEdit &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_3__["Button"], {
          type: "primary",
          size: "large",
          onClick: this.onCreate.bind(this)
        }, _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Create"))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_roles__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"], {
          ref: function ref(node) {
            return _this2.rolesPanelref = node;
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

  return Root;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

Root.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  selectedPage: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  selectedPageVisibleIndex: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number
};

function mapStateToProps() {
  return {};
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(Root);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(canEdit, "canEdit", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\App.jsx");
  reactHotLoader.register(Root, "Root", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\App.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\App.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\App.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 69 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(2);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(78);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(1);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(10);
/* harmony import */ var _FiltersBar__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(70);
/* harmony import */ var _RoleRow__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(33);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__);
/* harmony import */ var _UsersInRole__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(71);
/* harmony import */ var _RoleEditor__WEBPACK_IMPORTED_MODULE_10__ = __webpack_require__(35);
/* harmony import */ var _common_CollapsibleSwitcher__WEBPACK_IMPORTED_MODULE_11__ = __webpack_require__(36);
/* harmony import */ var _helpers_roles__WEBPACK_IMPORTED_MODULE_12__ = __webpack_require__(13);
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
  name: "RoleName",
  width: 40
}, {
  name: "GroupName",
  width: 20
}, {
  name: "Users",
  width: 15
}, {
  name: "Auto",
  width: 15
}, {
  name: "",
  width: 10
}];

var RolesPanel =
/*#__PURE__*/
function (_Component) {
  _inherits(RolesPanel, _Component);

  function RolesPanel() {
    var _this;

    _classCallCheck(this, RolesPanel);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(RolesPanel).call(this));
    _this.state = {
      defaultRoleGroup: _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("AllGroups"),
      openId: "",
      renderIndex: -1,
      deleteAllowed: false,
      parameter: {
        groupId: -1,
        keyword: "",
        startIndex: 0,
        pageSize: 10,
        reload: true
      }
    };
    return _this;
  }

  _createClass(RolesPanel, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      var props = this.props;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* roles */ "b"].getRoleGroupsList(false));
      this.refreshRolesList();
      var deleteAllowed = this.state.deleteAllowed;
      deleteAllowed = props.rolesList !== undefined && props.rolesList.length === 0;
      this.setState({
        deleteAllowed: deleteAllowed
      });
    }
  }, {
    key: "refreshRolesList",
    value: function refreshRolesList() {
      var _this2 = this;

      var props = this.props,
          state = this.state;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_5__[/* roles */ "b"].getRolesList(state.parameter, function (data) {
        var rolesList = Object.assign([], data.roles);

        _this2.setState({
          rolesList: rolesList
        });
      }));
    }
  }, {
    key: "onPageChanged",
    value: function onPageChanged() {
      var props = this.props;
      var startIndex = props.rolesList.length;
      var parameter = this.state.parameter;
      parameter.startIndex = startIndex;
      parameter.reload = false;
      this.setState({
        parameter: parameter
      });
      this.refreshRolesList(startIndex);
    }
  }, {
    key: "onAddRole",
    value: function onAddRole() {
      this.toggle(this.state.openId === "add" ? "" : "add", 1);
    }
  }, {
    key: "uncollapse",
    value: function uncollapse(id, index) {
      var _this3 = this;

      setTimeout(function () {
        _this3.setState({
          openId: id,
          renderIndex: index
        });
      }, this.timeout);
    }
  }, {
    key: "collapse",
    value: function collapse() {
      if (this.state.openId !== "") {
        this.setState({
          openId: "",
          renderIndex: -1
        });
      }
    }
  }, {
    key: "toggle",
    value: function toggle(openId, index) {
      if (openId !== "") {
        this.uncollapse(openId, index);
      } else {
        this.collapse();
      }
    }
  }, {
    key: "onRoleGroupChanged",
    value: function onRoleGroupChanged(group) {
      var parameter = this.state.parameter;
      parameter.groupId = group.value;
      parameter.startIndex = 0;
      parameter.reload = true;
      this.setState({
        parameter: parameter
      });
      this.refreshRolesList();
    }
  }, {
    key: "onKeywordChanged",
    value: function onKeywordChanged(keyword) {
      var parameter = this.state.parameter;
      parameter.keyword = keyword;
      parameter.startIndex = 0;
      parameter.reload = true;
      this.setState({
        parameter: parameter
      });
      this.refreshRolesList();
    }
  }, {
    key: "GetGroupName",
    value: function GetGroupName(groupId) {
      var props = this.props;

      if (props.roleGroups.some(function (group) {
        return group.id === groupId;
      })) {
        return props.roleGroups.filter(function (group) {
          return group.id === groupId;
        })[0].name;
      }

      return groupId;
    }
  }, {
    key: "renderHeader",
    value: function renderHeader() {
      var tableHeaders = tableFields.map(function (field) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_8__["GridCell"], {
            key: field.name,
            columnSize: field.width,
            style: {
              fontWeight: "bolder"
            }
          }, field.name !== "" ?
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get(field.name + ".Header")) :
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, "\xA0 "))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          id: "users-header-row",
          className: "users-header-row"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderedRolesList",
    value: function renderedRolesList(roleGroupOptions, securityModeOptions, statusOptions) {
      var _this4 = this;

      if (this.props.rolesList.length > 0) {
        var validRolesList = this.props.rolesList.filter(function (logSetting) {
          return !!logSetting;
        });
        var i = 0;
        return validRolesList.map(function (role, index) {
          var id = "row-" + i++;
          var children = [
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_UsersInRole__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"], {
            key: "userInRole-" + id,
            Collapse: _this4.collapse.bind(_this4),
            roleDetails: role
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_RoleEditor__WEBPACK_IMPORTED_MODULE_10__[/* default */ "a"], {
            key: "roleeditor-" + id,
            roleDetails: role,
            roleGroupOptions: roleGroupOptions,
            securityModeOptions: securityModeOptions,
            statusOptions: statusOptions,
            refreshRolesList: _this4.refreshRolesList.bind(_this4),
            roleId: role.id,
            Collapse: _this4.collapse.bind(_this4),
            currentGroupId: _this4.state.parameter.groupId
          })];
          return (
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_RoleRow__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], {
              key: "role-" + index,
              roleName: role.name,
              groupName: _this4.GetGroupName(role.groupId),
              userCount: role.usersCount,
              auto: role.autoAssign,
              index: index,
              closeOnClick: true,
              openId: _this4.state.openId,
              currentIndex: _this4.state.renderIndex,
              OpenCollapseUsers: _this4.toggle.bind(_this4, id, 0),
              OpenCollapseEditRoles: _this4.toggle.bind(_this4, id, 1),
              Collapse: _this4.collapse.bind(_this4, id),
              roleIsApproved: role.status === 1,
              id: id
            },
            /*#__PURE__*/
            react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_common_CollapsibleSwitcher__WEBPACK_IMPORTED_MODULE_11__[/* default */ "a"], {
              renderIndex: _this4.state.renderIndex
            }, children))
          );
        });
      } else {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "no-users-row"
          }, _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("NoData"))
        );
      }
    }
  }, {
    key: "render",
    value: function render() {
      var props = this.props,
          state = this.state;
      var opened = this.state.openId === "add";
      var roleGroupOptions = Object(_helpers_roles__WEBPACK_IMPORTED_MODULE_12__[/* createRoleGroupOptions */ "a"])(this.props.roleGroups);
      var securityModeOptions = [{
        label: _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SecurityRole"),
        value: 0
      }, {
        label: _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("SocialGroup"),
        value: 1
      }, {
        label: _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Both"),
        value: 2
      }];
      var statusOptions = [{
        label: _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Pending"),
        value: -1
      }, {
        label: _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Disabled"),
        value: 0
      }, {
        label: _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("Approved"),
        value: 1
      }];
      var children = [
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        key: ""
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_RoleEditor__WEBPACK_IMPORTED_MODULE_10__[/* default */ "a"], {
        key: "",
        roleGroupOptions: roleGroupOptions,
        securityModeOptions: securityModeOptions,
        statusOptions: statusOptions,
        roleId: -1,
        Collapse: this.collapse.bind(this),
        currentGroupId: this.state.parameter.groupId
      })];
      return props.roleGroups &&
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "roles-list-container"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_FiltersBar__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"], {
        onRoleGroupChanged: this.onRoleGroupChanged.bind(this),
        roleGroups: this.props.roleGroups,
        onKeywordChanged: this.onKeywordChanged.bind(this),
        DeleteAllowed: state.deleteAllowed
      }),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "container"
      }, this.renderHeader(),
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "add-setting-editor"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_RoleRow__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], {
        roleName: "-",
        groupName: "-",
        userCount: 0,
        auto: false,
        index: "add",
        key: "role-add",
        closeOnClick: true,
        openId: this.state.openId,
        currentIndex: this.state.renderIndex,
        OpenCollapseUsers: this.toggle.bind(this, "add", 1),
        OpenCollapseEditRoles: this.toggle.bind(this, "add", 1),
        Collapse: this.collapse.bind(this, "add"),
        id: "add",
        addIsClosed: !opened
      }, opened &&
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_common_CollapsibleSwitcher__WEBPACK_IMPORTED_MODULE_11__[/* default */ "a"], {
        renderIndex: this.state.renderIndex
      }, children))), this.renderedRolesList(roleGroupOptions, securityModeOptions, statusOptions)), props.loadMore &&
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
        className: "loadMore"
      },
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("a", {
        href: "#",
        onClick: this.onPageChanged.bind(this)
      }, _resources__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"].get("LoadMore"))));
    }
  }, {
    key: "__reactstandin__regenerateByEval",
    // @ts-ignore
    value: function __reactstandin__regenerateByEval(key, code) {
      // @ts-ignore
      this[key] = eval(code);
    }
  }], [{
    key: "getDerivedStateFromProps",
    value: function getDerivedStateFromProps(props, state) {
      var deleteAllowed = state.deleteAllowed;
      deleteAllowed = props.rolesList !== undefined && props.rolesList.length === 0;
      return {
        deleteAllowed: deleteAllowed
      };
    }
  }]);

  return RolesPanel;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

RolesPanel.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  rolesList: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  roleGroups: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  loadMore: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};

function mapStateToProps(state) {
  return {
    roleGroups: state.roles.roleGroups,
    rolesList: state.roles.rolesList,
    loadMore: state.roles.loadMore
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps, null, null, {
  withRef: true
})(RolesPanel);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(tableFields, "tableFields", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\index.jsx");
  reactHotLoader.register(RolesPanel, "RolesPanel", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 70 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(2);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(41);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_3__);
/* harmony import */ var react_widgets_lib_less_react_widgets_less__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(82);
/* harmony import */ var react_widgets_lib_less_react_widgets_less__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(react_widgets_lib_less_react_widgets_less__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__);
/* harmony import */ var _RoleEditor_RoleGroupEditor__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(23);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(10);
/* harmony import */ var utils__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(5);
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

var FiltersBar =
/*#__PURE__*/
function (_Component) {
  _inherits(FiltersBar, _Component);

  function FiltersBar(props) {
    var _this;

    _classCallCheck(this, FiltersBar);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(FiltersBar).call(this, props));
    _this.state = {
      selectedGroup: {
        label: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GlobalRolesGroup"),
        value: -1
      },
      showPopup: false
    };
    canEdit = utils__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].settings.isHost || utils__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].settings.isAdmin || utils__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].settings.permissions.EDIT;
    return _this;
  }

  _createClass(FiltersBar, [{
    key: "onDeleteGroup",
    value: function onDeleteGroup(e) {
      var _this2 = this;

      e.stopPropagation();
      var props = this.props;
      this.closeDropDown();
      utils__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].utilities.confirm(_resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("DeleteRoleGroup.Confirm"), _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Delete"), _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("Cancel"), function () {
        props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_8__[/* roles */ "b"].deleteRoleGroup(_this2.getCurrentGroup(), function () {
          utils__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].utilities.notify(_resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("DeleteRoleGroup.Message"));
          var selectedGroup = _this2.state.selectedGroup;
          selectedGroup.value = -1;
          selectedGroup.label = _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("GlobalRolesGroup");

          _this2.setState({
            selectedGroup: selectedGroup
          });

          _this2.props.onRoleGroupChanged(selectedGroup);
        }));
      }, function () {});
    }
  }, {
    key: "onSave",
    value: function onSave(group) {
      this.updateSelectedGroup({
        label: group.name,
        value: group.id
      });
      var showPopup = this.state.showPopup;
      showPopup = !showPopup;
      this.setState({
        showPopup: showPopup
      });
    }
  }, {
    key: "closeDropDown",
    value: function closeDropDown() {
      /*This is done in order to keep the dropdown closed on click on edit/delete*/
      var state = this.groupsDropdownRef.state;

      if (state) {
        state.dropDownOpen = false;
        this.groupsDropdownRef.setState({
          state: state
        });
      }
    }
  }, {
    key: "onSelect",
    value: function onSelect(option) {
      this.updateSelectedGroup(option);
      this.props.onRoleGroupChanged(option);
    }
  }, {
    key: "toggleEditGroup",
    value: function toggleEditGroup() {
      var _this3 = this;

      var showPopup = this.state.showPopup;
      showPopup = !showPopup;
      this.setState({
        showPopup: showPopup
      }, function () {
        _this3.closeDropDown();
      });
    }
  }, {
    key: "updateSelectedGroup",
    value: function updateSelectedGroup(option) {
      var label = option.label;
      var value = option.value;
      var selectedGroup = this.state.selectedGroup;
      selectedGroup.label = label;
      selectedGroup.value = value;
      this.setState({
        selectedGroup: selectedGroup
      });
    }
  }, {
    key: "getCurrentLabel",
    value: function getCurrentLabel() {
      var _this4 = this;

      var props = this.props;
      var selectedGroup = Object.assign({}, this.state.selectedGroup);
      var value = selectedGroup.value;
      var label = selectedGroup.label;
      /*eslint-disable no-unused-vars*/

      var group = {
        id: value,
        name: label
      };

      if (props.roleGroups.some(function (group) {
        return group.id === _this4.state.selectedGroup.value;
      })) {
        group = Object.assign({}, props.roleGroups.filter(function (group) {
          return group.id === value;
        })[0]);
      }

      if (value > -1) {
        if (canEdit) {
          label =
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "group-actions"
          }, label,
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "role-group-actions"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["IconButton"], {
            type: "edit",
            onClick: this.toggleEditGroup.bind(this)
          }), this.props.DeleteAllowed &&
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["IconButton"], {
            type: "trash",
            onClick: this.onDeleteGroup.bind(this)
          })));
        }
      }

      return label;
    }
  }, {
    key: "getCurrentGroup",
    value: function getCurrentGroup() {
      var _this5 = this;

      var props = this.props;
      var selectedGroup = Object.assign({}, this.state.selectedGroup);
      var value = selectedGroup.value;
      var label = selectedGroup.label;
      var group = {
        id: value,
        name: label
      };

      if (props.roleGroups.some(function (group) {
        return group.id === _this5.state.selectedGroup.value;
      })) {
        group = Object.assign({}, props.roleGroups.filter(function (group) {
          return group.id === value;
        })[0]);
      }

      return group;
    }
  }, {
    key: "getRoleGroupsDropDown",
    value: function getRoleGroupsDropDown() {
      var _this6 = this;

      var label = this.getCurrentLabel();
      var roleGroupsOptions = this.BuildRoleGroupsOptions();
      var GroupsDropDown =
      /*#__PURE__*/
      react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["Dropdown"], {
        style: {
          width: "100%"
        },
        withBorder: false,
        options: roleGroupsOptions,
        label: label,
        onSelect: this.onSelect.bind(this),
        ref: function ref(node) {
          return _this6.groupsDropdownRef = node;
        }
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "groups-filter"
        }, GroupsDropDown,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "clear"
        }))
      );
    }
  }, {
    key: "BuildRoleGroupsOptions",
    value: function BuildRoleGroupsOptions() {
      var roleGroups = this.props.roleGroups;
      var roleGroupsOptions = [];
      roleGroupsOptions = roleGroups.map(function (group) {
        return {
          label: group.name,
          value: group.id
        };
      });
      return roleGroupsOptions;
    }
  }, {
    key: "render",
    value: function render() {
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "filter-container"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["GridCell"], {
          columnSize: 35
        }, this.props.roleGroups.length > 0 && this.getRoleGroupsDropDown(), this.state.showPopup &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "edit-group-popup"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_RoleEditor_RoleGroupEditor__WEBPACK_IMPORTED_MODULE_7__[/* default */ "a"], {
          onSave: this.onSave.bind(this),
          onClick: this.closeDropDown.bind(this),
          onCancel: this.toggleEditGroup.bind(this),
          title: "Edit Group",
          group: this.getCurrentGroup()
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["GridCell"], {
          columnSize: 30
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", null, "\xA0 ")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["GridCell"], {
          columnSize: 35
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "search-filter"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_6__["SearchBox"], {
          placeholder: _resources__WEBPACK_IMPORTED_MODULE_5__[/* default */ "a"].get("SearchPlaceHolder"),
          onSearch: this.props.onKeywordChanged.bind(this),
          maxLength: 50
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
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  onRoleGroupChanged: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  onKeywordChanged: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  roleGroups: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array.isRequired,
  DeleteAllowed: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.bool
};

function mapStateToProps(state) {
  return {
    roleGroups: state.roles.roleGroups
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(FiltersBar);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(canEdit, "canEdit", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\FiltersBar\\index.jsx");
  reactHotLoader.register(FiltersBar, "FiltersBar", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\FiltersBar\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\FiltersBar\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\FiltersBar\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 71 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(2);
/* harmony import */ var prop_types__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(prop_types__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _resources__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(1);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(4);
/* harmony import */ var _dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__);
/* harmony import */ var react_widgets_lib_Combobox__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(72);
/* harmony import */ var react_widgets_lib_Combobox__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(react_widgets_lib_Combobox__WEBPACK_IMPORTED_MODULE_5__);
/* harmony import */ var _UserRow__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(34);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(143);
/* harmony import */ var _style_less__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(_style_less__WEBPACK_IMPORTED_MODULE_7__);
/* harmony import */ var _actions__WEBPACK_IMPORTED_MODULE_8__ = __webpack_require__(10);
/* harmony import */ var _utils__WEBPACK_IMPORTED_MODULE_9__ = __webpack_require__(5);
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












var UsersInRole =
/*#__PURE__*/
function (_Component) {
  _inherits(UsersInRole, _Component);

  function UsersInRole(props) {
    var _this;

    _classCallCheck(this, UsersInRole);

    _this = _possibleConstructorReturn(this, _getPrototypeOf(UsersInRole).call(this, props));
    _this.state = {
      userSelectState: {
        userId: -1,
        keyword: ""
      },
      currentPage: 0,
      totalPages: 0,
      pageSize: 10,
      usersKeyword: "",
      editIndex: -1,
      editCommand: "",
      sendEmail: true,
      isOwner: false,
      loading: false
    };
    return _this;
  }

  _createClass(UsersInRole, [{
    key: "componentDidMount",
    value: function componentDidMount() {
      this.getUsers();
      if (this.comboBoxRef.childNodes && this.comboBoxRef.childNodes[1]) this.comboBoxRef.childNodes[1].setAttribute("aria-label", "Suggestion");
    }
  }, {
    key: "getUsers",
    value: function getUsers() {
      var _this2 = this;

      var props = this.props,
          state = this.state;
      this.setState({
        loading: true
      });
      var parameter = {
        roleId: props.roleDetails.id,
        keyword: state.usersKeyword,
        pageIndex: state.currentPage,
        pageSize: state.pageSize
      };
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_8__[/* roleUsers */ "a"].getRoleUsers(parameter, function () {
        _this2.setState({
          loading: false
        });
      }));
    }
  }, {
    key: "getSuggestUsers",
    value: function getSuggestUsers() {
      var props = this.props,
          state = this.state;
      var keyword = state.userSelectState.userId > 0 ? "" : state.userSelectState.keyword;
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_8__[/* roleUsers */ "a"].getSuggestUsers({
        keyword: keyword,
        roleId: -1,
        count: 10
      }));
    }
  }, {
    key: "debounceGetSuggestUsers",
    value: function debounceGetSuggestUsers() {
      this.getSuggestUsers();
    }
  }, {
    key: "onUserSelectorChanged",
    value: function onUserSelectorChanged(item) {
      if (this.state.loading) {
        return;
      }

      if (item.userId || item.displayName || _typeof(item) === "object") {
        return;
      }

      this.setState({
        userSelectState: {
          userId: -1,
          keyword: item
        }
      });
      this.debounceGetSuggestUsers();
    }
  }, {
    key: "onUserSelectorSelected",
    value: function onUserSelectorSelected(item) {
      if (!item.userId || !item.displayName) {
        return;
      }

      this.setState({
        userSelectState: {
          userId: item.userId,
          keyword: item.displayName
        }
      }, function () {
        this.getSuggestUsers();
      });
    }
  }, {
    key: "onUserSelectorToggle",
    value: function onUserSelectorToggle() {}
  }, {
    key: "onAddUser",
    value: function onAddUser() {
      var state = this.state;
      var userId = state.userSelectState.userId;

      if (userId === -1) {
        return;
      }

      this.saveUser(true, userId);
      this.setState({
        userSelectState: {
          userId: -1,
          keyword: ""
        }
      });
    }
  }, {
    key: "saveUser",
    value: function saveUser(isAdd, userId, startTime, expiresTime) {
      var props = this.props;
      var parameter = {
        userId: userId,
        roleId: props.roleDetails.id,
        startTime: startTime,
        expiresTime: expiresTime,
        isAdd: isAdd
      };
      props.dispatch(_actions__WEBPACK_IMPORTED_MODULE_8__[/* roleUsers */ "a"].addUserToRole(parameter, this.state.sendEmail, this.state.isOwner));
      this.setState({
        sendEmail: true,
        isOwner: false
      });
    }
  }, {
    key: "onUsersKeywordChanged",
    value: function onUsersKeywordChanged(keyword) {
      var _this3 = this;

      var newState = {
        usersKeyword: keyword,
        currentPage: 0
      };
      this.setState(newState, function () {
        _this3.getUsers();
      });
    }
  }, {
    key: "onPageChanged",
    value: function onPageChanged(currentPage, pageSize) {
      var state = this.state;

      if (pageSize !== undefined && state.pageSize !== pageSize) {
        state.pageSize = pageSize;
      }

      state.currentPage = currentPage;
      this.setState({
        state: state
      });
      this.getUsers();
    }
  }, {
    key: "getUserRows",
    value: function getUserRows() {
      var _this4 = this;

      var roleUsers = this.props.roleUsers;
      var userRows = roleUsers.map(function (user, index) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_UserRow__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"], {
            key: index,
            userDetails: user,
            index: index,
            saveUser: _this4.saveUser.bind(_this4, false)
          })
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "role-user-body"
        }, roleUsers.length > 0 ? userRows :
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "no-roles-row"
        }, _resources__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("NoUsers")))
      );
    }
  }, {
    key: "onSendEmailClick",
    value: function onSendEmailClick(sendEmail) {
      if (this.state.loading) {
        return;
      }

      this.setState({
        sendEmail: sendEmail
      });
    }
  }, {
    key: "onIsOwnerClick",
    value: function onIsOwnerClick(isOwner) {
      if (this.state.loading) {
        return;
      }

      this.setState({
        isOwner: isOwner
      });
    }
  }, {
    key: "renderHeader",
    value: function renderHeader() {
      var _this5 = this;

      var tableFields = [{
        name: "Members",
        width: 25
      }, {
        name: "Start",
        width: 20
      }, {
        name: "Expires",
        width: 20
      }, {
        name: "",
        width: 35
      }];
      var tableHeaders = tableFields.map(function (field) {
        return (
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
            key: field.name,
            columnSize: field.width,
            style: {
              fontWeight: "bolder"
            }
          }, field.name !== "" ?
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null, _resources__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get(field.name + ".Header")) :
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "search-container"
          },
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["SearchBox"], {
            placeholder: _resources__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Search"),
            onSearch: _this5.onUsersKeywordChanged.bind(_this5),
            maxLength: 50
          }),
          /*#__PURE__*/
          react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
            className: "clear"
          })))
        );
      });
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "role-user-header-row"
        }, tableHeaders)
      );
    }
  }, {
    key: "renderPaging",
    value: function renderPaging() {
      if (this.props.totalRecords > 0) return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Pager"], {
          showStartEndButtons: false,
          showPageSizeOptions: false,
          numericCounters: 0,
          pageSize: this.state.pageSize,
          totalRecords: this.props.totalRecords,
          onPageChanged: this.onPageChanged.bind(this),
          culture: _utils__WEBPACK_IMPORTED_MODULE_9__[/* default */ "a"].utilities.getCulture()
        })
      );
    }
  }, {
    key: "render",
    value: function render() {
      var _this6 = this;

      var state = this.state;
      var className = "roleusers-form" + (state.loading ? " isloading" : "");
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: className
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "header"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "header-title"
        }, _resources__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("PermissionsByRole")),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "add-box"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 50
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "send-email-box"
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Checkbox"], {
          value: this.state.sendEmail,
          enabled: !state.loading,
          onChange: this.onSendEmailClick.bind(this),
          label: _resources__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("SendEmail"),
          labelPlace: "right"
        }), this.props.roleDetails.allowOwner &&
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["Checkbox"], {
          value: this.state.isOwner,
          onChange: this.onIsOwnerClick.bind(this),
          label: _resources__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("isOwner"),
          labelPlace: "right"
        }))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["GridCell"], {
          columnSize: 50
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("span", null,
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(react_widgets_lib_Combobox__WEBPACK_IMPORTED_MODULE_5___default.a, {
          suggest: false,
          ref: function ref(node) {
            _this6.comboBoxRef = node;
          },
          placeholder: _resources__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("AddUserPlaceHolder"),
          open: this.props.matchedUsers.length > 0,
          onToggle: this.onUserSelectorToggle.bind(this),
          onChange: this.onUserSelectorChanged.bind(this),
          onSelect: this.onUserSelectorSelected.bind(this),
          data: this.props.matchedUsers,
          value: state.userSelectState.keyword,
          valueField: "userId",
          textField: "displayName"
        }),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "add-user-button",
          onClick: this.onAddUser.bind(this)
        },
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement(_dnnsoftware_dnn_react_common__WEBPACK_IMPORTED_MODULE_4__["IconButton"], {
          type: "add",
          width: 17,
          height: 15
        }), " ", _resources__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].get("Add")))))),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "role-users-list"
        }, this.renderHeader(), this.getUserRows()),
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "role-users-list-paging"
        }, this.renderPaging()))
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

  return UsersInRole;
}(react__WEBPACK_IMPORTED_MODULE_0__["Component"]);

UsersInRole.propTypes = {
  dispatch: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func.isRequired,
  roleDetails: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object.isRequired,
  title: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.string,
  group: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.object,
  beforeSave: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onSaved: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  onCancel: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.func,
  roleUsers: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array,
  totalRecords: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.number,
  matchedUsers: prop_types__WEBPACK_IMPORTED_MODULE_1___default.a.array
};
_UserRow__WEBPACK_IMPORTED_MODULE_6__[/* default */ "a"].defaultProps = {
  matchedUsers: []
};

function mapStateToProps(state) {
  return {
    matchedUsers: state.roleUsers.matchedUsers,
    roleUsers: state.roleUsers.roleUsers,
    totalRecords: state.roleUsers.totalRecords,
    roleList: state.roles.rolesList
  };
}

var _default = Object(react_redux__WEBPACK_IMPORTED_MODULE_2__["connect"])(mapStateToProps)(UsersInRole);

/* harmony default export */ __webpack_exports__["a"] = (_default);
;

(function () {
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(UsersInRole, "UsersInRole", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\UsersInRole\\index.jsx");
  reactHotLoader.register(mapStateToProps, "mapStateToProps", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\UsersInRole\\index.jsx");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\components\\roles\\UsersInRole\\index.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 72 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _classnames = _interopRequireDefault(__webpack_require__(12));

var _reactLifecyclesCompat = __webpack_require__(42);

var _uncontrollable = _interopRequireDefault(__webpack_require__(97));

var _Widget = _interopRequireDefault(__webpack_require__(99));

var _WidgetPicker = _interopRequireDefault(__webpack_require__(100));

var _List = _interopRequireDefault(__webpack_require__(101));

var _Popup = _interopRequireDefault(__webpack_require__(110));

var _Select = _interopRequireDefault(__webpack_require__(124));

var _ComboboxInput = _interopRequireDefault(__webpack_require__(126));

var _messages = __webpack_require__(46);

var _focusManager = _interopRequireDefault(__webpack_require__(128));

var _reduceToListState = _interopRequireDefault(__webpack_require__(44));

var _getAccessors = _interopRequireDefault(__webpack_require__(133));

var CustomPropTypes = _interopRequireWildcard(__webpack_require__(19));

var _scrollManager = _interopRequireDefault(__webpack_require__(134));

var _ = __webpack_require__(20);

var Props = _interopRequireWildcard(__webpack_require__(25));

var Filter = _interopRequireWildcard(__webpack_require__(45));

var _interaction = __webpack_require__(57);

var _widgetHelpers = __webpack_require__(26);

var _Icon = __webpack_require__(139);

var _class, _class2, _descriptor, _descriptor2, _descriptor3, _class3, _temp;

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _initializerDefineProperty(target, property, descriptor, context) { if (!descriptor) return; Object.defineProperty(target, property, { enumerable: descriptor.enumerable, configurable: descriptor.configurable, writable: descriptor.writable, value: descriptor.initializer ? descriptor.initializer.call(context) : void 0 }); }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

function _assertThisInitialized(self) { if (self === void 0) { throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); } return self; }

function _applyDecoratedDescriptor(target, property, decorators, descriptor, context) { var desc = {}; Object['ke' + 'ys'](descriptor).forEach(function (key) { desc[key] = descriptor[key]; }); desc.enumerable = !!desc.enumerable; desc.configurable = !!desc.configurable; if ('value' in desc || desc.initializer) { desc.writable = true; } desc = decorators.slice().reverse().reduce(function (desc, decorator) { return decorator(target, property, desc) || desc; }, desc); if (context && desc.initializer !== void 0) { desc.value = desc.initializer ? desc.initializer.call(context) : void 0; desc.initializer = undefined; } if (desc.initializer === void 0) { Object['define' + 'Property'](target, property, desc); desc = null; } return desc; }

function _initializerWarningHelper(descriptor, context) { throw new Error('Decorating class property failed. Please ensure that ' + 'proposal-class-properties is enabled and set to use loose mode. ' + 'To use proposal-class-properties in spec mode with decorators, wait for ' + 'the next major version of decorators in stage 2.'); }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

var propTypes = _extends({}, Filter.propTypes, {
  value: _propTypes.default.any,
  onChange: _propTypes.default.func,
  open: _propTypes.default.bool,
  onToggle: _propTypes.default.func,
  itemComponent: CustomPropTypes.elementType,
  listComponent: CustomPropTypes.elementType,
  groupComponent: CustomPropTypes.elementType,
  groupBy: CustomPropTypes.accessor,
  data: _propTypes.default.array,
  valueField: CustomPropTypes.accessor,
  textField: CustomPropTypes.accessor,
  name: _propTypes.default.string,

  /**
   *
   * @type {(dataItem: ?any, metadata: { originalEvent: SyntheticEvent }) => void}
   */
  onSelect: _propTypes.default.func,
  autoFocus: _propTypes.default.bool,
  disabled: CustomPropTypes.disabled.acceptsArray,
  readOnly: CustomPropTypes.disabled,

  /**
   * When `true` the Combobox will suggest, or fill in, values as you type. The suggestions
   * are always "startsWith", meaning it will search from the start of the `textField` property
   */
  suggest: Filter.propTypes.filter,
  busy: _propTypes.default.bool,

  /** Specify the element used to render the select (down arrow) icon. */
  selectIcon: _propTypes.default.node,

  /** Specify the element used to render the busy indicator */
  busySpinner: _propTypes.default.node,
  delay: _propTypes.default.number,
  dropUp: _propTypes.default.bool,
  popupTransition: CustomPropTypes.elementType,
  placeholder: _propTypes.default.string,

  /** Adds a css class to the input container element. */
  containerClassName: _propTypes.default.string,
  inputProps: _propTypes.default.object,
  listProps: _propTypes.default.object,
  isRtl: _propTypes.default.bool,
  messages: _propTypes.default.shape({
    openCombobox: CustomPropTypes.message,
    emptyList: CustomPropTypes.message,
    emptyFilter: CustomPropTypes.message
  })
  /**
   * ---
   * shortcuts:
   *   - { key: alt + down arrow, label: open combobox }
   *   - { key: alt + up arrow, label: close combobox }
   *   - { key: down arrow, label: move focus to next item }
   *   - { key: up arrow, label: move focus to previous item }
   *   - { key: home, label: move focus to first item }
   *   - { key: end, label: move focus to last item }
   *   - { key: enter, label: select focused item }
   *   - { key: any key, label: search list for item starting with key }
   * ---
   *
   * Select an item from the list, or input a custom value. The Combobox can also make suggestions as you type.
  
   * @public
   */

});

var Combobox = (0, _reactLifecyclesCompat.polyfill)(_class = (_class2 = (_temp = _class3 =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(Combobox, _React$Component);

  function Combobox(props, context) {
    var _this;

    _this = _React$Component.call(this, props, context) || this;

    _this.handleFocusWillChange = function (focused) {
      if (!focused && _this.inputRef) _this.inputRef.accept();
      if (focused) _this.focus();
    };

    _this.handleFocusChanged = function (focused) {
      if (!focused) _this.close();
    };

    _initializerDefineProperty(_this, "handleSelect", _descriptor, _assertThisInitialized(_assertThisInitialized(_this)));

    _this.handleInputKeyDown = function (_ref) {
      var key = _ref.key;
      _this._deleting = key === 'Backspace' || key === 'Delete';
      _this._isTyping = true;
    };

    _this.handleInputChange = function (event) {
      var suggestion = _this.suggest(event.target.value);

      _this.change(suggestion, true, event);

      _this.open();
    };

    _initializerDefineProperty(_this, "handleKeyDown", _descriptor2, _assertThisInitialized(_assertThisInitialized(_this)));

    _this.attachListRef = function (ref) {
      _this.listRef = ref;
    };

    _this.attachInputRef = function (ref) {
      _this.inputRef = ref;
    };

    _initializerDefineProperty(_this, "toggle", _descriptor3, _assertThisInitialized(_assertThisInitialized(_this)));

    _this.inputId = (0, _widgetHelpers.instanceId)(_assertThisInitialized(_assertThisInitialized(_this)), '_input');
    _this.listId = (0, _widgetHelpers.instanceId)(_assertThisInitialized(_assertThisInitialized(_this)), '_listbox');
    _this.activeId = (0, _widgetHelpers.instanceId)(_assertThisInitialized(_assertThisInitialized(_this)), '_listbox_active_option');
    _this.handleScroll = (0, _scrollManager.default)(_assertThisInitialized(_assertThisInitialized(_this)));
    _this.focusManager = (0, _focusManager.default)(_assertThisInitialized(_assertThisInitialized(_this)), {
      willHandle: _this.handleFocusWillChange,
      didHandle: _this.handleFocusChanged
    });
    _this.state = {
      isSuggesting: function isSuggesting() {
        return _this.inputRef && _this.inputRef.isSuggesting();
      }
    };
    return _this;
  }

  var _proto = Combobox.prototype;

  _proto.shouldComponentUpdate = function shouldComponentUpdate(nextProps, nextState) {
    var isSuggesting = nextState.isSuggesting(),
        stateChanged = !(0, _.isShallowEqual)(nextState, this.state),
        valueChanged = !(0, _.isShallowEqual)(nextProps, this.props);
    return isSuggesting || stateChanged || valueChanged;
  };

  Combobox.getDerivedStateFromProps = function getDerivedStateFromProps(nextProps, prevState) {
    var value = nextProps.value,
        data = nextProps.data,
        messages = nextProps.messages,
        filter = nextProps.filter,
        minLength = nextProps.minLength,
        caseSensitive = nextProps.caseSensitive;
    var focusedItem = prevState.focusedItem;
    var accessors = (0, _getAccessors.default)(nextProps);
    var valueChanged = value !== prevState.lastValue;
    var selectedIndex = accessors.indexOf(data, value);
    var dataItem = selectedIndex === -1 ? value : data[selectedIndex];
    var searchTerm; // filter only when the value is not an item in the data list

    if (selectedIndex === -1 || prevState.isSuggesting()) {
      searchTerm = accessors.text(dataItem);
    }

    data = Filter.filter(data, {
      filter: filter,
      searchTerm: searchTerm,
      minLength: minLength,
      caseSensitive: caseSensitive,
      textField: accessors.text
    });
    var list = (0, _reduceToListState.default)(data, prevState.list, {
      nextProps: nextProps
    }); // index may have changed after filtering

    if (selectedIndex !== -1) {
      selectedIndex = accessors.indexOf(data, value);
    }

    var focusedIndex = accessors.indexOf(data, focusedItem);

    if (focusedIndex === -1) {
      // value isn't a dataItem so find the close match
      focusedIndex = Filter.indexOf(data, {
        searchTerm: searchTerm,
        textField: accessors.text,
        filter: filter || true
      });
    }

    var selectedItem = data[selectedIndex];
    var nextFocusedItem = null; // If no item is focused, or is no longer in the dataset, default to either the selected item, or to the first item in the list

    if (focusedIndex === -1) {
      if (selectedItem !== undefined) {
        nextFocusedItem = selectedItem;
      } else {
        nextFocusedItem = data[0];
      }
    } else {
      nextFocusedItem = data[focusedIndex];
    }

    return {
      data: data,
      list: list,
      accessors: accessors,
      lastValue: value,
      messages: (0, _messages.getMessages)(messages),
      selectedItem: valueChanged ? list.nextEnabled(selectedItem) : prevState.selectedItem,
      focusedItem: valueChanged || focusedItem === undefined ? list.nextEnabled(selectedItem !== undefined ? selectedItem : nextFocusedItem) : nextFocusedItem
    };
  }; // has to be done early since `accept()` re-focuses the input


  _proto.renderInput = function renderInput() {
    var _this$props = this.props,
        suggest = _this$props.suggest,
        filter = _this$props.filter,
        busy = _this$props.busy,
        name = _this$props.name,
        data = _this$props.data,
        value = _this$props.value,
        autoFocus = _this$props.autoFocus,
        tabIndex = _this$props.tabIndex,
        placeholder = _this$props.placeholder,
        inputProps = _this$props.inputProps,
        disabled = _this$props.disabled,
        readOnly = _this$props.readOnly,
        open = _this$props.open;
    var accessors = this.state.accessors;
    var valueItem = accessors.findOrSelf(data, value);
    var completeType = suggest ? filter ? 'both' : 'inline' : filter ? 'list' : '';
    return _react.default.createElement(_ComboboxInput.default, _extends({}, inputProps, {
      role: "combobox",
      name: name,
      id: this.inputId,
      autoFocus: autoFocus,
      tabIndex: tabIndex,
      suggest: suggest,
      disabled: disabled === true,
      readOnly: readOnly === true,
      "aria-busy": !!busy,
      "aria-owns": this.listId,
      "aria-autocomplete": completeType,
      "aria-activedescendant": open ? this.activeId : null,
      "aria-expanded": open,
      "aria-haspopup": true,
      placeholder: placeholder,
      value: accessors.text(valueItem),
      onChange: this.handleInputChange,
      onKeyDown: this.handleInputKeyDown,
      ref: this.attachInputRef
    }));
  };

  _proto.renderList = function renderList(messages) {
    var activeId = this.activeId,
        inputId = this.inputId,
        listId = this.listId;
    var _this$props2 = this.props,
        open = _this$props2.open,
        data = _this$props2.data,
        value = _this$props2.value,
        listProps = _this$props2.listProps,
        optionComponent = _this$props2.optionComponent,
        itemComponent = _this$props2.itemComponent,
        groupComponent = _this$props2.groupComponent;
    var _this$state = this.state,
        list = _this$state.list,
        accessors = _this$state.accessors,
        focusedItem = _this$state.focusedItem,
        selectedItem = _this$state.selectedItem,
        filteredData = _this$state.data;
    var List = this.props.listComponent;
    return _react.default.createElement(List, _extends({}, listProps, {
      id: listId,
      activeId: activeId,
      data: filteredData,
      dataState: list.dataState,
      isDisabled: list.isDisabled,
      textAccessor: accessors.text,
      valueAccessor: accessors.value,
      itemComponent: itemComponent,
      groupComponent: groupComponent,
      optionComponent: optionComponent,
      selectedItem: selectedItem,
      focusedItem: open ? focusedItem : null,
      searchTerm: accessors.text(value) || '',
      "aria-hidden": !open,
      "aria-labelledby": inputId,
      "aria-live": open && 'polite',
      onSelect: this.handleSelect,
      onMove: this.handleScroll,
      ref: this.attachListRef,
      messages: {
        emptyList: data.length ? messages.emptyFilter : messages.emptyList
      }
    }));
  };

  _proto.render = function render() {
    var _this2 = this;

    var _this$props3 = this.props,
        isRtl = _this$props3.isRtl,
        className = _this$props3.className,
        popupTransition = _this$props3.popupTransition,
        busy = _this$props3.busy,
        dropUp = _this$props3.dropUp,
        open = _this$props3.open,
        selectIcon = _this$props3.selectIcon,
        busySpinner = _this$props3.busySpinner,
        containerClassName = _this$props3.containerClassName;
    var _this$state2 = this.state,
        focused = _this$state2.focused,
        messages = _this$state2.messages;
    var disabled = this.props.disabled === true,
        readOnly = this.props.readOnly === true;
    var elementProps = Props.pickElementProps(this);
    var shouldRenderPopup = (0, _widgetHelpers.isFirstFocusedRender)(this);
    return _react.default.createElement(_Widget.default, _extends({}, elementProps, {
      open: open,
      isRtl: isRtl,
      dropUp: dropUp,
      focused: focused,
      disabled: disabled,
      readOnly: readOnly,
      onBlur: this.focusManager.handleBlur,
      onFocus: this.focusManager.handleFocus,
      onKeyDown: this.handleKeyDown,
      className: (0, _classnames.default)(className, 'rw-combobox')
    }), _react.default.createElement(_WidgetPicker.default, {
      className: containerClassName
    }, this.renderInput(), _react.default.createElement(_Select.default, {
      bordered: true,
      busy: busy,
      icon: selectIcon,
      spinner: busySpinner,
      onClick: this.toggle,
      disabled: disabled || readOnly,
      label: messages.openCombobox(this.props)
    })), shouldRenderPopup && _react.default.createElement(_Popup.default, {
      open: open,
      dropUp: dropUp,
      transition: popupTransition,
      onEntering: function onEntering() {
        return _this2.listRef.forceUpdate();
      }
    }, _react.default.createElement("div", null, this.renderList(messages))));
  };

  _proto.focus = function focus() {
    if (this.inputRef) this.inputRef.focus();
  };

  _proto.change = function change(nextValue, typing, originalEvent) {
    var _this$props4 = this.props,
        onChange = _this$props4.onChange,
        lastValue = _this$props4.value;
    this._typedChange = !!typing;
    (0, _widgetHelpers.notify)(onChange, [nextValue, {
      lastValue: lastValue,
      originalEvent: originalEvent
    }]);
  };

  _proto.open = function open() {
    if (!this.props.open) (0, _widgetHelpers.notify)(this.props.onToggle, true);
  };

  _proto.close = function close() {
    if (this.props.open) (0, _widgetHelpers.notify)(this.props.onToggle, false);
  };

  _proto.suggest = function suggest(searchTerm) {
    var _this$props5 = this.props,
        textField = _this$props5.textField,
        suggest = _this$props5.suggest,
        minLength = _this$props5.minLength;
    var data = this.state.data;
    if (!this._deleting) return Filter.suggest(data, {
      minLength: minLength,
      textField: textField,
      searchTerm: searchTerm,
      filter: suggest,
      caseSensitive: false
    });
    return searchTerm;
  };

  return Combobox;
}(_react.default.Component), _class3.propTypes = propTypes, _class3.defaultProps = {
  data: [],
  value: '',
  open: false,
  suggest: false,
  filter: false,
  delay: 500,
  selectIcon: _Icon.caretDown,
  listComponent: _List.default
}, _temp), (_descriptor = _applyDecoratedDescriptor(_class2.prototype, "handleSelect", [_interaction.widgetEditable], {
  enumerable: true,
  initializer: function initializer() {
    var _this3 = this;

    return function (data, originalEvent) {
      _this3.close();

      (0, _widgetHelpers.notify)(_this3.props.onSelect, [data, {
        originalEvent: originalEvent
      }]);

      _this3.change(data, false, originalEvent);

      _this3.inputRef && _this3.inputRef.accept(true);

      _this3.focus();
    };
  }
}), _descriptor2 = _applyDecoratedDescriptor(_class2.prototype, "handleKeyDown", [_interaction.widgetEditable], {
  enumerable: true,
  initializer: function initializer() {
    var _this4 = this;

    return function (e) {
      var key = e.key,
          altKey = e.altKey;
      var _this4$props = _this4.props,
          open = _this4$props.open,
          onKeyDown = _this4$props.onKeyDown;
      var _this4$state = _this4.state,
          focusedItem = _this4$state.focusedItem,
          selectedItem = _this4$state.selectedItem,
          list = _this4$state.list;
      (0, _widgetHelpers.notify)(onKeyDown, [e]);
      if (e.defaultPrevented) return;

      var select = function select(item) {
        return item != null && _this4.handleSelect(item, e);
      };

      var focusItem = function focusItem(item) {
        return _this4.setState({
          focusedItem: item
        });
      };

      if (key === 'End' && open) {
        e.preventDefault();
        focusItem(list.last());
      } else if (key === 'Home' && open) {
        e.preventDefault();
        focusItem(list.first());
      } else if (key === 'Escape' && open) {
        e.preventDefault();

        _this4.close();
      } else if (key === 'Enter' && open) {
        e.preventDefault();
        select(_this4.state.focusedItem);
      } else if (key === 'Tab') {
        _this4.inputRef.accept();
      } else if (key === 'ArrowDown') {
        e.preventDefault();
        if (altKey) return _this4.open();
        if (open) focusItem(list.next(focusedItem));else select(list.next(selectedItem));
      } else if (key === 'ArrowUp') {
        e.preventDefault();
        if (altKey) return _this4.close();
        if (open) focusItem(list.prev(focusedItem));else select(list.prev(selectedItem));
      }
    };
  }
}), _descriptor3 = _applyDecoratedDescriptor(_class2.prototype, "toggle", [_interaction.widgetEditable], {
  enumerable: true,
  initializer: function initializer() {
    var _this5 = this;

    return function () {
      _this5.focus();

      _this5.props.open ? _this5.close() : _this5.open();
    };
  }
})), _class2)) || _class;

var _default = (0, _uncontrollable.default)(Combobox, {
  open: 'onToggle',
  value: 'onChange'
}, ['focus']);

exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 73 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(16);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(9);
/* harmony import */ var react_redux__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(react_redux__WEBPACK_IMPORTED_MODULE_2__);
/* harmony import */ var _globals_application__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(30);
/* harmony import */ var _store_configureStore__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(58);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(67);
/* harmony import */ var _containers_Root__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_containers_Root__WEBPACK_IMPORTED_MODULE_5__);
(function () {
  var enterModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).enterModule;
  enterModule && enterModule(module);
})();







var store = Object(_store_configureStore__WEBPACK_IMPORTED_MODULE_4__[/* default */ "a"])();
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].dispatch = store.dispatch;
_globals_application__WEBPACK_IMPORTED_MODULE_3__[/* default */ "a"].init();
var appContainer = document.getElementById("roles-container");
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

  reactHotLoader.register(store, "store", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\main.jsx");
  reactHotLoader.register(appContainer, "appContainer", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\main.jsx");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 74 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
function _interopDefault(e){return e&&"object"==typeof e&&"default"in e?e.default:e}Object.defineProperty(exports,"__esModule",{value:!0});var React=_interopDefault(__webpack_require__(0));function AppContainer(e){return React.Children.only(e.children)}var hot_prod=function(){return function(e){return e}},areComponentsEqual=function(e,n){return e===n},setConfig=function(){},cold=function(e){return e},configureComponent=function(){};exports.AppContainer=AppContainer,exports.hot=hot_prod,exports.areComponentsEqual=areComponentsEqual,exports.setConfig=setConfig,exports.cold=cold,exports.configureComponent=configureComponent;


/***/ }),
/* 75 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* WEBPACK VAR INJECTION */(function(module) {/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(0);
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var _components_App__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(68);
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
      return (
        /*#__PURE__*/
        react__WEBPACK_IMPORTED_MODULE_0___default.a.createElement("div", {
          className: "roles-Root"
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
  var reactHotLoader = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).default;

  if (!reactHotLoader) {
    return;
  }

  reactHotLoader.register(Root, "Root", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\containers\\Root.prod.js");
  reactHotLoader.register(_default, "default", "D:\\a\\1\\s\\Dnn.AdminExperience\\ClientSide\\Roles.Web\\src\\containers\\Root.prod.js");
})();

;

(function () {
  var leaveModule = (typeof reactHotLoaderGlobal !== 'undefined' ? reactHotLoaderGlobal : __webpack_require__(3)).leaveModule;
  leaveModule && leaveModule(module);
})();
/* WEBPACK VAR INJECTION */}.call(this, __webpack_require__(6)(module)))

/***/ }),
/* 76 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = __webpack_require__(77);

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
/* 77 */
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
/* 78 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(79);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(15)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 79 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(14)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.roles-list-container {\n  margin-top: 100px;\n  width: 100%;\n  box-sizing: border-box;\n  padding: 20px;\n  display: table;\n  float: left;\n}\n.roles-list-container > div {\n  padding: 0;\n}\n.roles-list-container * {\n  box-sizing: border-box;\n}\n.roles-list-container .container {\n  float: left;\n  width: 100%;\n  margin-bottom: 20px;\n  border-left: 1px solid #C8C8C8;\n  border-right: 1px solid #C8C8C8;\n  background-color: white;\n}\n.roles-list-container .container .no-users-row {\n  width: 100%;\n  float: left;\n  text-align: center;\n  font-weight: bold;\n  padding: 13px 20px;\n  box-sizing: border-box;\n  border-bottom: 1px solid #C8C8C8;\n}\n.roles-list-container .container .users-header-row {\n  display: table;\n  border-bottom: 1px solid #C8C8C8;\n  border-top: 1px solid #C8C8C8;\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 20px;\n  box-sizing: border-box;\n  text-transform: uppercase;\n}\n.roles-list-container .container .add-setting-editor {\n  float: left;\n}\n.roles-list-container .container .add-setting-editor .collapsible-component1.false {\n  border-bottom: 0;\n}\n.roles-list-container .add-setting-row {\n  text-align: right;\n  width: 100%;\n  font-weight: bolder;\n  overflow: hidden;\n  cursor: pointer;\n  box-sizing: border-box;\n  border-bottom: 1px solid #C8C8C8;\n  padding: 0 0 15px 20px;\n  margin-bottom: 15px;\n}\n.roles-list-container .add-setting-row .add-setting-box {\n  width: auto;\n  float: right;\n}\n.roles-list-container .add-setting-row .add-setting-box .add-icon {\n  margin-left: 20px;\n  margin-right: 5px;\n  float: left;\n  cursor: pointer;\n}\n.roles-list-container .add-setting-row .add-setting-box .add-icon svg {\n  fill: #6F7273;\n  width: 16px;\n  float: left;\n  height: 16px;\n}\n.roles-list-container .add-setting-row .add-setting-box.false {\n  color: #1E88C3;\n}\n.roles-list-container .add-setting-row .add-setting-box.false svg {\n  fill: #1E88C3;\n}\n.roles-list-container .add-setting-row .add-setting-box.false:hover {\n  color: #4B4E4F;\n}\n.roles-list-container .add-setting-row .add-setting-box.false:hover svg {\n  fill: #4B4E4F;\n}\n.roles-list-container .loadMore {\n  text-align: center;\n}\n.roles-list-container .loadMore a {\n  display: inline-block;\n  height: 36px;\n  line-height: 36px;\n  min-width: 130px;\n  border: 1px solid #C8C8C8;\n  color: #1E88C3;\n  text-decoration: none;\n  font-weight: bold;\n}\n", ""]);



/***/ }),
/* 80 */
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
/* 81 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(14)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.filter-container {\n  border-bottom: 1px solid #C8C8C8;\n  float: left;\n  box-sizing: border-box;\n  margin-bottom: 15px;\n  width: 100%;\n}\n.filter-container .dnn-grid-cell {\n  margin-bottom: 5px;\n}\n.filter-container .dnn-grid-cell .groups-filter {\n  width: 100%;\n  border-right: 1px solid #C8C8C8;\n  font-size: 13px;\n  font-family: inherit;\n  float: left;\n}\n.filter-container .dnn-grid-cell .groups-filter .dnn-dropdown .collapsible-toggle {\n  width: auto;\n}\n.filter-container .dnn-grid-cell .groups-filter .group-actions {\n  color: #46292B;\n  top: 2px;\n  right: 30px;\n}\n.filter-container .dnn-grid-cell .groups-filter .group-actions a {\n  display: inline-block;\n  margin-right: 8px;\n  width: 16px;\n  height: 16px;\n  vertical-align: middle;\n  margin-top: -2px;\n  margin-left: 2px;\n}\n.filter-container .dnn-grid-cell .groups-filter .group-actions .role-group-actions {\n  float: right;\n  width: auto;\n}\n.filter-container .dnn-grid-cell .edit-group-popup {\n  position: absolute;\n  left: 71px;\n  z-index: 999;\n}\n.filter-container .dnn-grid-cell .search-filter {\n  float: left;\n  width: 100%;\n}\n.filter-container .dnn-grid-cell .search-filter > div {\n  display: block !important;\n  border-left: 1px solid #C8C8C8;\n}\n.filter-container .dnn-grid-cell .search-filter > div input {\n  display: block;\n  width: 100%;\n  position: inherit !important;\n  border: none;\n  border-radius: none;\n  background-color: transparent;\n  outline: none;\n  padding-right: 45px;\n}\n", ""]);



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

var update = __webpack_require__(15)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 83 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(14)(false);
// Imports
var urlEscape = __webpack_require__(84);
var ___CSS_LOADER_URL___0___ = urlEscape(__webpack_require__(85));
var ___CSS_LOADER_URL___1___ = urlEscape(__webpack_require__(86) + "?#iefix&v=4.1.0");
var ___CSS_LOADER_URL___2___ = urlEscape(__webpack_require__(87));
var ___CSS_LOADER_URL___3___ = urlEscape(__webpack_require__(88));
var ___CSS_LOADER_URL___4___ = urlEscape(__webpack_require__(89) + "#fontawesomeregular");
var ___CSS_LOADER_URL___5___ = urlEscape(__webpack_require__(90));
var ___CSS_LOADER_URL___6___ = urlEscape(__webpack_require__(91));

// Module
exports.push([module.i, ".rw-btn,\n.rw-input-reset,\n.rw-input,\n.rw-dropdown-list-autofill,\n.rw-filter-input {\n  color: inherit;\n  padding: 0;\n  margin: 0;\n  border: none;\n  box-shadow: none;\n  background: none;\n  background-image: none;\n  font-family: inherit;\n  font-size: inherit;\n  line-height: inherit;\n  -ms-touch-action: manipulation;\n  touch-action: manipulation;\n}\n.rw-btn::-moz-focus-inner {\n  padding: 0;\n  border: 0;\n}\nselect.rw-input {\n  text-transform: none;\n}\nhtml input[type=\"button\"].rw-input {\n  -webkit-appearance: button;\n  cursor: pointer;\n}\ntextarea.rw-input {\n  overflow: auto;\n  resize: vertical;\n}\nbutton[disabled].rw-input,\nfieldset[disabled] .rw-input,\nhtml input[disabled].rw-input {\n  cursor: not-allowed;\n}\nbutton.rw-input::-moz-focus-inner,\ninput.rw-input::-moz-focus-inner {\n  border: 0;\n  padding: 0;\n}\ninput[type=\"checkbox\"],\ninput[type=\"radio\"] {\n  box-sizing: border-box;\n  padding: 0;\n}\n/* -------------- */\n@font-face {\n  font-family: 'RwWidgets';\n  font-weight: normal;\n  font-style: normal;\n  src: url(" + ___CSS_LOADER_URL___0___ + ");\n  src: url(" + ___CSS_LOADER_URL___1___ + ") format('embedded-opentype'), url(" + ___CSS_LOADER_URL___2___ + ") format('woff'), url(" + ___CSS_LOADER_URL___3___ + ") format('truetype'), url(" + ___CSS_LOADER_URL___4___ + ") format('svg');\n}\n.rw-i {\n  display: inline-block;\n  color: inherit;\n  font-family: RwWidgets;\n  font-style: normal;\n  font-weight: normal;\n  font-variant: normal;\n  text-transform: none;\n  -moz-osx-font-smoothing: grayscale;\n  -webkit-font-smoothing: antialiased;\n}\n.rw-i-caret-down:before {\n  content: '\\e803';\n}\n.rw-i-caret-up:before {\n  content: '\\e800';\n}\n.rw-i-chevron-left:before {\n  content: '\\f104';\n}\n.rw-i-chevron-right:before {\n  content: '\\f105';\n}\n.rw-i-clock-o:before {\n  content: '\\e805';\n}\n.rw-i-calendar:before {\n  content: '\\e804';\n}\n.rw-i-search:before {\n  content: '\\e801';\n}\n.rw-btn {\n  position: relative;\n  color: #333333;\n  display: inline-block;\n  text-align: center;\n  vertical-align: middle;\n  border: 1px solid transparent;\n  cursor: pointer;\n  outline: none;\n}\n.rw-state-readonly .rw-btn,\n.rw-state-disabled .rw-btn {\n  cursor: not-allowed;\n}\n.rw-btn-select {\n  opacity: 0.75;\n  transition: opacity 150ms ease-in;\n}\n.rw-btn-select:hover,\n.rw-state-focus .rw-btn-select,\n:hover > .rw-btn-select {\n  opacity: 1;\n}\n.rw-btn-primary {\n  width: 100%;\n  white-space: normal;\n  line-height: 2em;\n}\n.rw-btn-primary:hover {\n  background-color: #e6e6e6;\n}\n.rw-btn-select[disabled],\n.rw-btn-primary[disabled],\nfieldset[disabled] .rw-btn-select,\nfieldset[disabled] .rw-btn-primary {\n  box-shadow: none;\n  cursor: not-allowed;\n  opacity: 0.65;\n  pointer-events: none;\n}\n/* for debugging */\n@keyframes react-widgets-autofill-start {\n  from {\n    /**/\n  }\n  to {\n    /**/\n  }\n}\n@keyframes react-widgets-autofill-cancel {\n  from {\n    /**/\n  }\n  to {\n    /**/\n  }\n}\n.rw-sr {\n  position: absolute;\n  width: 1px;\n  height: 1px;\n  margin: -1px;\n  padding: 0;\n  overflow: hidden;\n  clip: rect(0, 0, 0, 0);\n  border: 0;\n}\n.rw-widget {\n  background-clip: border-box;\n  border: none;\n  color: #333333;\n  font-size: 1em;\n  font-family: inherit;\n  outline: none;\n  position: relative;\n}\n.rw-widget,\n.rw-widget * {\n  box-sizing: border-box;\n}\n.rw-widget:before,\n.rw-widget *:before,\n.rw-widget:after,\n.rw-widget *:after {\n  box-sizing: border-box;\n}\n.rw-widget > .rw-widget-container {\n  width: 100%;\n  margin: 0;\n}\n.rw-widget-container {\n  background-color: #fff;\n  border: #ccc 1px solid;\n  border-radius: 4px;\n}\n.rw-widget-container.rw-state-focus,\n.rw-state-focus > .rw-widget-container,\n.rw-widget-container.rw-state-focus:hover,\n.rw-state-focus > .rw-widget-container:hover {\n  background-color: #fff;\n  border-color: #66afe9;\n  box-shadow: 0 0 8px rgba(102, 175, 233, 0.6);\n}\n.rw-widget-container.rw-state-readonly,\n.rw-state-readonly > .rw-widget-container {\n  cursor: not-allowed;\n}\n.rw-widget-container.rw-state-disabled,\n.rw-state-disabled > .rw-widget-container,\nfieldset[disabled] .rw-widget-container,\n.rw-widget-container.rw-state-disabled:hover,\n.rw-state-disabled > .rw-widget-container:hover,\nfieldset[disabled] .rw-widget-container:hover,\n.rw-widget-container.rw-state-disabled:active,\n.rw-state-disabled > .rw-widget-container:active,\nfieldset[disabled] .rw-widget-container:active {\n  box-shadow: none;\n  cursor: not-allowed;\n}\n.rw-widget-picker {\n  position: relative;\n  overflow: hidden;\n  border-collapse: separate;\n  display: inline-table;\n  height: 2.429em;\n}\n.rw-widget-picker > * {\n  position: relative;\n  border: none;\n  outline: none;\n  width: 100%;\n  height: 100%;\n  display: table-cell;\n}\n.rw-widget-picker > .rw-select {\n  width: 1%;\n  white-space: nowrap;\n}\n.rw-open > .rw-widget-picker {\n  border-bottom-right-radius: 0;\n  border-bottom-left-radius: 0;\n}\n.rw-open-up > .rw-widget-picker {\n  border-top-right-radius: 0;\n  border-top-left-radius: 0;\n}\nfieldset[disabled] .rw-widget-picker,\n.rw-state-disabled > .rw-widget-picker {\n  background-color: #eeeeee;\n}\n.rw-multiselect > .rw-widget-picker {\n  height: auto;\n}\n.rw-select {\n  cursor: pointer;\n}\n.rw-select > * {\n  width: 1.9em;\n  height: 100%;\n}\n.rw-state-readonly .rw-select,\n.rw-state-disabled .rw-select {\n  cursor: not-allowed;\n}\n.rw-select-bordered {\n  cursor: pointer;\n  border: none;\n  border-left: #ccc 1px solid;\n}\n.rw-select-bordered:hover,\n.rw-select-bordered:active {\n  background-color: #e6e6e6;\n}\n.rw-select-bordered:active {\n  box-shadow: inset 0 3px 5px rgba(0, 0, 0, 0.125);\n}\n.rw-state-disabled .rw-select-bordered,\n.rw-state-readonly .rw-select-bordered,\nfieldset[disabled] .rw-select-bordered,\n.rw-state-disabled .rw-select-bordered:hover,\n.rw-state-readonly .rw-select-bordered:hover,\nfieldset[disabled] .rw-select-bordered:hover,\n.rw-state-disabled .rw-select-bordered:active,\n.rw-state-readonly .rw-select-bordered:active,\nfieldset[disabled] .rw-select-bordered:active {\n  cursor: not-allowed;\n  background-color: inherit;\n  background-image: none;\n  box-shadow: none;\n}\n.rw-rtl .rw-select-bordered {\n  border-right: #ccc 1px solid;\n  border-left: none;\n}\n.rw-rtl {\n  direction: rtl;\n}\n.rw-input-reset,\n.rw-input,\n.rw-dropdown-list-autofill,\n.rw-filter-input {\n  outline: 0;\n}\n.rw-input-reset::-moz-placeholder {\n  color: #999;\n  opacity: 1;\n}\n.rw-input-reset:-ms-input-placeholder {\n  color: #999;\n}\n.rw-input-reset::-webkit-input-placeholder {\n  color: #999;\n}\n.rw-input,\n.rw-dropdown-list-autofill,\n.rw-filter-input {\n  color: #555555;\n  padding: 0 0.857em;\n  background-color: #fff;\n}\n.rw-input[type=\"text\"]::-ms-clear {\n  display: none;\n}\n.rw-input[disabled],\nfieldset[disabled] .rw-input {\n  box-shadow: none;\n  cursor: not-allowed;\n  opacity: 1;\n  background-color: #eeeeee;\n  border-color: #ccc;\n}\n.rw-input[readonly] {\n  cursor: not-allowed;\n}\n.rw-i.rw-loading {\n  display: block;\n  background: url(" + ___CSS_LOADER_URL___5___ + ") no-repeat center;\n  min-width: 16px;\n  width: 1.9em;\n  height: 16px;\n}\n.rw-i.rw-loading:before {\n  content: \"\";\n}\n.rw-placeholder {\n  color: #999;\n}\n.rw-detect-autofill:-webkit-autofill {\n  animation-name: react-widgets-autofill-start;\n  transition: background-color 50000s ease-in-out 0s;\n}\n.rw-detect-autofill:not(:-webkit-autofill) {\n  animation-name: react-widgets-autofill-cancel;\n}\n.rw-webkit-autofill .rw-widget-container,\n.rw-input:-webkit-autofill {\n  background-color: #faffbd !important;\n  background-image: none !important;\n  color: #000000 !important;\n}\n.rw-widget-input,\n.rw-filter-input {\n  box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);\n}\n.rw-widget-input.rw-state-focus {\n  box-shadow: 0 0 8px rgba(102, 175, 233, 0.6), inset 0 1px 1px rgba(0, 0, 0, 0.075);\n}\n.rw-list {\n  margin: 0;\n  padding: 0;\n  list-style: none;\n  font-size: 1em;\n  outline: 0;\n  overflow: auto;\n  max-height: 200px;\n}\n.rw-list-option {\n  user-select: none;\n  color: #333333;\n  cursor: pointer;\n  border: 1px solid transparent;\n}\n.rw-list-option.rw-state-focus,\n.rw-list-option.rw-state-focus:hover {\n  background-color: transparent;\n  border-color: #66afe9;\n  color: #333333;\n}\n.rw-list-option:hover,\n.rw-list-option:hover.rw-state-focus {\n  background-color: #e6e6e6;\n  border-color: #e6e6e6;\n  color: #333333;\n}\n.rw-list-option.rw-state-selected,\n.rw-list-option.rw-state-selected:hover {\n  background-color: #337ab7;\n  border-color: #337ab7;\n  color: white;\n}\nfieldset[disabled] .rw-list-option,\n.rw-list-option.rw-state-disabled,\n.rw-list-option.rw-state-readonly {\n  box-shadow: none;\n  cursor: not-allowed;\n  color: #999999;\n  opacity: 0.7;\n}\nfieldset[disabled] .rw-list-option:hover,\n.rw-list-option.rw-state-disabled:hover,\n.rw-list-option.rw-state-readonly:hover {\n  background: none;\n  border-color: transparent;\n}\n.rw-list-empty,\n.rw-list-option,\n.rw-list-optgroup {\n  padding: 0.143em 0.75em;\n  outline: 0;\n}\n.rw-list-optgroup {\n  font-weight: bold;\n  padding-top: 7px;\n}\n.rw-list-option-create {\n  border-top: 1px #ccc solid;\n}\n.rw-dropdown-list-autofill {\n  padding: 0;\n}\n.rw-dropdown-list-input {\n  background-color: transparent;\n  vertical-align: middle;\n  padding-right: 0;\n  /* ellipsis */\n  max-width: 1px;\n  text-overflow: ellipsis;\n  white-space: nowrap;\n  overflow: hidden;\n}\n.rw-rtl .rw-dropdown-list-input {\n  padding-right: 0.857em;\n  padding-left: 0;\n}\n.rw-filter-input {\n  position: relative;\n  margin: 4px;\n  padding-right: 0;\n}\n.rw-filter-input .rw-rtl {\n  padding-right: 0.857em;\n  padding-left: 0;\n}\n.rw-filter-input .rw-select,\n.rw-filter-input .rw-btn {\n  opacity: 0.75;\n  cursor: text;\n}\n.rw-filter-input > .rw-select,\n.rw-filter-input > .rw-select:active,\n.rw-filter-input > .rw-select:hover {\n  background: none;\n  cursor: initial;\n  box-shadow: none;\n}\n.rw-number-picker .rw-btn {\n  cursor: pointer;\n  height: calc(1.2145em - 1px);\n  /** ie8 fallback **/\n  margin-top: -1px\\9;\n  height: 1.2145em\\9;\n  /** --- **/\n  line-height: 1.2145em;\n  line-height: calc(1.2145em - 1px);\n  display: block;\n  border: none;\n}\n.rw-number-picker .rw-btn:hover,\n.rw-number-picker .rw-btn:active {\n  background-color: #e6e6e6;\n}\n.rw-number-picker .rw-btn:active {\n  box-shadow: inset 0 3px 5px rgba(0, 0, 0, 0.125);\n}\n.rw-state-disabled .rw-number-picker .rw-btn,\n.rw-state-readonly .rw-number-picker .rw-btn,\nfieldset[disabled] .rw-number-picker .rw-btn,\n.rw-state-disabled .rw-number-picker .rw-btn:hover,\n.rw-state-readonly .rw-number-picker .rw-btn:hover,\nfieldset[disabled] .rw-number-picker .rw-btn:hover,\n.rw-state-disabled .rw-number-picker .rw-btn:active,\n.rw-state-readonly .rw-number-picker .rw-btn:active,\nfieldset[disabled] .rw-number-picker .rw-btn:active {\n  cursor: not-allowed;\n  background-color: inherit;\n  background-image: none;\n  box-shadow: none;\n}\n.rw-number-picker .rw-select {\n  vertical-align: middle;\n}\n.rw-number-picker .rw-select,\n.rw-number-picker .rw-select:hover,\n.rw-number-picker .rw-select:active {\n  box-shadow: none;\n}\n.rw-calendar-popup {\n  right: auto;\n  min-width: 0;\n  width: 18em;\n}\n.rw-calendar {\n  border-radius: 4px;\n  background-color: #fff;\n  border: #ccc 1px solid;\n  overflow: hidden;\n}\n.rw-calendar.rw-popup {\n  border-color: #ccc;\n}\n.rw-calendar-now {\n  font-weight: bold;\n}\n.rw-calendar-btn-left,\n.rw-calendar-btn-right {\n  width: 12.5%;\n}\n.rw-calendar-btn-view {\n  width: 75%;\n}\n.rw-calendar-footer {\n  border-top: 1px solid #ccc;\n}\n.rw-calendar-grid {\n  outline: none;\n  height: 14.28571429em;\n  table-layout: fixed;\n  border-collapse: separate;\n  border-spacing: 0;\n  width: 100%;\n  background-color: #fff;\n}\n.rw-head-cell {\n  text-align: center;\n  border-bottom: 1px solid #ccc;\n  padding: 0.25em;\n}\n.rw-cell {\n  color: #333333;\n  border-radius: 4px;\n  cursor: pointer;\n  line-height: normal;\n  text-align: center;\n  border: 1px solid transparent;\n  padding: 0.25em;\n}\n.rw-cell:hover {\n  background-color: #e6e6e6;\n  border-color: #e6e6e6;\n  color: #333333;\n}\n.rw-cell.rw-state-focus,\n.rw-cell.rw-state-focus:hover {\n  background-color: transparent;\n  border-color: #66afe9;\n  color: #333333;\n}\n.rw-cell.rw-state-selected,\n.rw-cell.rw-state-selected:hover {\n  background-color: #337ab7;\n  border-color: #337ab7;\n  color: white;\n}\n.rw-cell.rw-state-disabled {\n  color: #999999;\n  opacity: 0.7;\n}\n.rw-cell.rw-state-disabled:hover {\n  background: none;\n  border-color: transparent;\n}\n.rw-calendar-month .rw-cell {\n  text-align: center;\n}\n.rw-cell-off-range {\n  color: #999999;\n}\n.rw-calendar-transition-group {\n  position: relative;\n}\n.rw-calendar-transition {\n  transition: transform 300ms;\n  overflow: hidden;\n}\n.rw-calendar-transition-top {\n  transform: translateY(-100%);\n}\n.rw-calendar-transition-bottom {\n  transform: translateY(100%);\n}\n.rw-calendar-transition-right {\n  transform: translateX(-100%);\n}\n.rw-calendar-transition-left {\n  transform: translateX(100%);\n}\n.rw-calendar-transition-entering.rw-calendar-transition-top,\n.rw-calendar-transition-entered.rw-calendar-transition-top,\n.rw-calendar-transition-entering.rw-calendar-transition-bottom,\n.rw-calendar-transition-entered.rw-calendar-transition-bottom {\n  transform: translateY(0);\n}\n.rw-calendar-transition-entering.rw-calendar-transition-right,\n.rw-calendar-transition-entered.rw-calendar-transition-right,\n.rw-calendar-transition-entering.rw-calendar-transition-left,\n.rw-calendar-transition-entered.rw-calendar-transition-left {\n  transform: translateX(0);\n}\n.rw-calendar-transition-exiting.rw-calendar-transition-top {\n  transform: translateY(100%);\n}\n.rw-calendar-transition-exiting.rw-calendar-transition-bottom {\n  transform: translateY(-100%);\n}\n.rw-calendar-transition-exiting.rw-calendar-transition-right {\n  transform: translateX(100%);\n}\n.rw-calendar-transition-exiting.rw-calendar-transition-left {\n  transform: translateX(-100%);\n}\n.rw-select-list {\n  overflow: auto;\n  position: relative;\n}\n.rw-select-list .rw-list {\n  max-height: none;\n  font-size: 1em;\n}\n.rw-select-list-label {\n  display: block;\n  position: relative;\n  font-weight: normal;\n  cursor: inherit;\n  padding-left: 20px;\n  margin: 0;\n}\n.rw-rtl .rw-select-list-label {\n  padding-left: 0;\n  padding-right: 20px;\n}\ninput.rw-select-list-input {\n  position: absolute;\n  left: 0;\n  top: 50%;\n  transform: translateY(-50%);\n  top: 0.1em /9;\n  margin: 0;\n  line-height: normal;\n  cursor: inherit;\n}\n.rw-rtl input.rw-select-list-input {\n  left: auto;\n  right: 0;\n}\n.rw-loading-mask {\n  content: '';\n  background: url(" + ___CSS_LOADER_URL___6___ + ") no-repeat center;\n  position: absolute;\n  background-color: #fff;\n  border-radius: 4px;\n  opacity: 0.7;\n  top: 0;\n  left: 0;\n  height: 100%;\n  width: 100%;\n}\n.rw-multiselect {\n  cursor: text;\n}\n.rw-multiselect .rw-input-reset {\n  height: calc(2.429em - 2px);\n  /** ie8 fallback **/\n  margin-top: -2px\\9;\n  height: 2.429em\\9;\n  /** --- **/\n  border-width: 0;\n  width: auto;\n  max-width: 100%;\n  padding: 0 0.857em;\n}\n.rw-multiselect .rw-select {\n  vertical-align: middle;\n}\n.rw-multiselect .rw-select,\n.rw-multiselect .rw-select:hover,\n.rw-multiselect .rw-select:active {\n  box-shadow: none;\n  background: none;\n}\n.rw-multiselect-taglist {\n  margin: 0;\n  padding: 0;\n  list-style: none;\n  display: inline-block;\n  vertical-align: 0;\n  outline: none;\n}\n.rw-multiselect-tag {\n  display: inline-table;\n  color: inherit;\n  padding: 0 0.35em 0 0.35em;\n  margin-left: calc(0.279335em - 1px);\n  margin-top: 0.279335em;\n  margin-top: calc(0.279335em - 1px);\n  height: 1.87033em;\n  border-radius: 3px;\n  background-color: #eeeeee;\n  border: 1px solid #ccc;\n  cursor: default;\n  vertical-align: top;\n  text-align: center;\n  overflow: hidden;\n  max-width: 100%;\n}\n.rw-multiselect-tag > * {\n  display: table-cell;\n  vertical-align: middle;\n  height: 100%;\n}\n.rw-rtl .rw-multiselect-tag {\n  margin-left: 0;\n  margin-right: calc(0.279335em - 1px);\n  padding: 0 0.35em 0 0.35em;\n}\n.rw-multiselect-tag.rw-state-focus,\n.rw-multiselect-tag.rw-state-focus:hover {\n  background-color: transparent;\n  border-color: #66afe9;\n  color: #333333;\n}\n.rw-multiselect-tag.rw-state-readonly,\n.rw-multiselect-tag.rw-state-disabled,\n.rw-state-readonly .rw-multiselect-tag,\n.rw-state-disabled .rw-multiselect-tag,\nfieldset[disabled] .rw-multiselect-tag {\n  cursor: not-allowed;\n}\n.rw-multiselect-tag.rw-state-disabled,\n.rw-state-disabled .rw-multiselect-tag,\nfieldset[disabled] .rw-multiselect-tag {\n  opacity: 0.65;\n}\nfieldset[disabled] .rw-multiselect-tag {\n  box-shadow: none;\n  cursor: not-allowed;\n}\n.rw-multiselect-tag-btn {\n  color: inherit;\n  margin-left: 0.25em;\n}\n.rw-rtl .rw-multiselect-tag-btn {\n  margin-left: 0;\n  margin-right: 0.25em;\n}\n.rw-autocomplete .rw-select {\n  position: absolute;\n  display: block;\n  width: auto;\n  top: 0;\n  bottom: 0;\n  right: 0;\n}\n.rw-popup-container {\n  position: absolute;\n  z-index: 1005;\n  top: 100%;\n  left: -6px;\n  right: -6px;\n}\n.rw-popup-container.rw-dropup {\n  top: auto;\n  bottom: 100%;\n}\n.rw-state-focus .rw-popup-container {\n  z-index: 1006;\n}\n.rw-popup-transition {\n  width: 100%;\n  margin-bottom: 6px;\n  padding: 0 6px;\n}\n.rw-dropup > .rw-popup-transition {\n  margin-bottom: 0;\n  margin-top: 6px;\n}\n.rw-popup {\n  border-top-right-radius: 0;\n  border-top-left-radius: 0;\n  border-bottom-right-radius: 3px;\n  border-bottom-left-radius: 3px;\n  box-shadow: 0 5px 6px rgba(0, 0, 0, 0.2);\n  border: #ccc 1px solid;\n  background: #fff;\n}\n.rw-dropup .rw-popup {\n  border-bottom-right-radius: 0;\n  border-bottom-left-radius: 0;\n  border-top-right-radius: 3px;\n  border-top-left-radius: 3px;\n  box-shadow: 0 -2px 6px rgba(0, 0, 0, 0.2);\n}\n.rw-popup-transition {\n  transition: transform 200ms;\n}\n.rw-popup-transition-entering {\n  overflow: hidden;\n}\n.rw-popup-transition-entering .rw-popup-transition {\n  transform: translateY(0);\n  transition-timing-function: ease-out;\n}\n.rw-popup-transition-exiting .rw-popup-transition {\n  transition-timing-function: ease-in;\n}\n.rw-popup-transition-exiting,\n.rw-popup-transition-exited {\n  overflow: hidden;\n}\n.rw-popup-transition-exiting .rw-popup-transition,\n.rw-popup-transition-exited .rw-popup-transition {\n  transform: translateY(-100%);\n}\n.rw-popup-transition-exiting.rw-dropup .rw-popup-transition,\n.rw-popup-transition-exited.rw-dropup .rw-popup-transition {\n  transform: translateY(100%);\n}\n.rw-popup-transition-exited {\n  display: none;\n}\n.rw-state-disabled {\n  box-shadow: none;\n  cursor: not-allowed;\n}\n", ""]);



/***/ }),
/* 84 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


module.exports = function escape(url, needQuotes) {
  if (typeof url !== 'string') {
    return url;
  } // If url is already wrapped in quotes, remove them


  if (/^['"].*['"]$/.test(url)) {
    url = url.slice(1, -1);
  } // Should url be wrapped?
  // See https://drafts.csswg.org/css-values-3/#urls


  if (/["'() \t\n]/.test(url) || needQuotes) {
    return '"' + url.replace(/"/g, '\\"').replace(/\n/g, '\\n') + '"';
  }

  return url;
};

/***/ }),
/* 85 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__.p + "rw-widgets.eot";

/***/ }),
/* 86 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__.p + "rw-widgets.eot";

/***/ }),
/* 87 */
/***/ (function(module, exports) {

module.exports = "data:font/woff;base64,bW9kdWxlLmV4cG9ydHMgPSAiZGF0YTphcHBsaWNhdGlvbi9mb250LXdvZmY7YmFzZTY0LGQwOUdSZ0FCQUFBQUFBNmdBQThBQUFBQUdGZ0FBUUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFCSFUxVkNBQUFCV0FBQUFEc0FBQUJVSUl3bGVVOVRMeklBQUFHVUFBQUFRd0FBQUZZK0lGRk1ZMjFoY0FBQUFkZ0FBQUI1QUFBQjFNSmptZE5qZG5RZ0FBQUNWQUFBQUJNQUFBQWdCdFgvQkdad1oyMEFBQUpvQUFBRmtBQUFDM0NLa1pCWloyRnpjQUFBQi9nQUFBQUlBQUFBQ0FBQUFCQm5iSGxtQUFBSUFBQUFBNWdBQUFTd1ZyTWZIV2hsWVdRQUFBdVlBQUFBTXdBQUFEWU1oRGxGYUdobFlRQUFDOHdBQUFBZkFBQUFKQWM2QTFWb2JYUjRBQUFMN0FBQUFDQUFBQUFnRmNQLysyeHZZMkVBQUF3TUFBQUFFZ0FBQUJJRkNnUWliV0Y0Y0FBQURDQUFBQUFnQUFBQUlBRjRERHB1WVcxbEFBQU1RQUFBQVlzQUFBTGx2WTRxVUhCdmMzUUFBQTNNQUFBQVZnQUFBSExhbkx5OGNISmxjQUFBRGlRQUFBQjZBQUFBaHVWQks3eDRuR05nWkdCZzRHSXdZTEJqWU1wSkxNbGo0SE54OHdsaGtHSmdZWUFBa0R3eW16RW5NejJSZ1FQR0E4cXhnR2tPSUdhRGlBSUFLVmtGU0FCNG5HTmdaTnJCT0lHQmxZR0JxWXBwRHdNRFF3K0VabnpBWU1qSUJCUmxZR1Ztd0FvQzBseFRHQnhlTUh4a1pRNzZuOFVReFJ6RU1BMG96QWlTQXdEN2pRdnlBSGljN1pIQkRRSXhEQVFuRitlQ1RvaEtxSUZxZVBHaWJIOVR3Ykcycnd3Y1RlUmRLNHEwQmdiUXhWTVl0QytOcUkvY2xuN25TTjk0U2Q5ME52RG1ZNDN6VklmM1pkRmxOYzBQSHRsdGVtdjZZV2RLN3Z6cm52ZjdValBTS3lKcnY4aDhleEc3Y1N0aVB6NksyTnV5UXZteVJzSDhBVlJIR1g0QUFBQjRuR05nUUFNU0VNZ2M5RDhMaEFFU2JBUGRBSGljclZacGQ5TkdGQjE1U1p5RUxDVUxMV3BoeE1ScHNFWW1iTUdBQ1VHeVl5QmRuSzJWb0lzVU8rbSs4WW5mNEYvelpOcHo2RGQrV3U4Ykx5U1F0T2R3bXBPamQrZk4xY3piWlJKYWt0Z0w2NUdVbXkvRjFOWW1qZXc4Q2VtR1RjdFJmQ2c3ZXlGbGlzbmZCVkVRclpiYXR4MkhSRVFpVUxXdXNFUVEreDVabW1SODZGRkd5N2FrVjAzS0xUM3BMbHZqUWIxVjMzNGFPc3F4TzZHa1pqTjBhRDJ5SlZVWVZhSklwajFTMHFabHFQb3JTU3U4djhMTVY4MVF3b2hPSW1tOEdjYlFTTjRiWjdUS2FEVzI0eWlLYkxMY0tGSWttdUZCRkhtVTFSTG41SW9KRE1vSHpaRHl5cWNSNWNQOGlLellvNXhXc0V1MjAveStMM21uZHprL3NWOXZVYmJrUUIvSWp1emc3SFFsWDRSYlcySGN0SlB0S0ZRUmR0ZDNRbXpaN0ZUL1pvL3lta1lEdHlzeXZkQ01ZS2w4aFJBclA2SE0vaUZaTFp4UCtaSkhvMXF5a1JOQjYyVk83RXMrZ2RiamlDbHh6UmhaME4zUkNSSFUvWkl6RFBhWVBoNzg4ZDRwbGdzVEFuZ2N5M3BISlp3SUV5bGhjelJKMmpCeVlDVmxpeXFwOWE2WU9PVjFXc1Jid243dDJ0R1h6bWpqVUhkaVBGc1BIVnM1VWNueGFGS25tVXlkMmtuTm95a05vcFIwSm5qTXJ3TW9QNkpKWG0xak5ZbVZSOU00WnNhRVJDSUNMZHhMVTBFc083R2tLUVROb3htOXVSdW11WFl0V3FUSkEvWGNvL2YwNWxhNHVkTlQyZzcwczBaL1ZxZGlPdGdMMCtscDVDL3hhZHJsSWtYcCt1a1pma3ppUWRZQ01wRXROc09VZ3dkdi9RN1N5OWVXSElYWEJ0anU3Zk1ycUgzV1JQQ2tBZnNiMEI1UDFTa0pUSVdZVlloV1FHS3RhMW1XeWRXc0ZxbkkxSGREbWxhK3JOTUVpbkljRjhlK2pISDlYek16bHBnU3Z0K0owN01qTGoxejdVc0kweHg4bTNVOW10ZXB4WElCY1daNVRxZFpsdS9yTk1meUE1M21XWjdYNlFoTFc2ZWpMRC9VYVlIbFJ6b2RZM2xCQzVwMDM4R1FpekRrQWc2UU1JU2xBME5ZWG9JaExCVU1ZYmtJUTFnV1lRakxKUmpDOG1NWXduSVpockM4ckdYVjFGTko0OXFaV0Fac1FtQmlqaDY1ekVYbGFpcTVWRUs3YUZScVE1NFNicFZVRk0rcWYyV2dYanp5aGptd0ZraVh5SnBmTWM2VmowYmwrTllWTFc4YU8xZkFzZXB2SDQ3Mk9mRlMxb3VGUHdYLzFkWlVKYjFpemNPVHEvQWJocDVzSjZvMnFYaDBUWmZQVlQyNi9sOVVWRmdMOUJ0SWhWZ295ckpzY0djaWhJODZuWVpxb0pWRHpHek1QTFRyZGN1YW44UDlOekZDRmxEOStEY1VHZ3ZjZzA1WlNWbnQ0S3pWMTl1eTNEdURjamdUTEVreE4vUDZWdmdpSTdQU2ZwRlp5cDZQZkI1d0JZeEtaZGhxQTYwVnZOa25NUStaM2lUUEJIRmJVVFpJMnRqT0JJa05IUE9BZWZPZEJDWmg2cW9ONUU3aGhnMzRCV0Z1d1hrblhLSjZveXlIN2tYczh5aWsvRnVuNGtUMnFHaU13TFBaRzJHdjcwTEtiM0VNSkRUNXBYNE1WQldocVJnMUZkQTBVbTZvQmwvRzJicHRRc1lPOUNNcWRzT3lyT0xEeHhiM2xaSnRHWVI4cElqVm82T2YxbDZpVHFyY2ZtWVVsKytkdmdYQklEVXhmM3ZmZEhHUXlydGF5VEpIYlFOVHR4cVZVOWVhUStOVmgrcm1VZlc5NCt3VE9XdWFicm9uSG5wZjA2cmJ3Y1ZjTExEMmJRN1NVaVlYMVBWaGhRMml5OFdsVU9wbE5FbnZ1QWNZRmhqUTcxQ0tqZityK3RoOG5pdFZoZEZ4Sk45TzFMZlI1MkFNL0EvWWYwZjFBOUQzWStoeURTN1A5NW9UbjI3MDRXeVpycUlYNjZmb056QnJyYmxadWdiYzBIUUQ0aUZIclk2NHlnMThwd1p4ZXFTNUhPa2g0R1BkRmVJQndDYUF4ZUFUM2JXTTVsTUFvL21NT1Q3QTU4eGgwR1FPZ3kzbU1OaG16aHJBRG5NWTdES0h3UjV6R0h6Qm5IV0FMNW5ESUdRT2c0ZzVESjR3SndCNHlod0dYekdId2RmTVlmQU5jKzREZk1zY0JqRnpHQ1RNWWJDdjZkWXd6QzFlMEYyZ3RrRlZvQU5UVDFqY3crSlFVMlhJL280WGh2MjlRY3ord1NDbS9xanA5cEQ2RXk4TTlXZURtUHFMUVV6OVZkT2RJZlUzWGhqcTd3WXg5UStEbVBwTXZ4akxaUWEvakh5WENnZVVYV3crNSsrSjl3L2J4VUM1QUFFQUFmLy9BQTk0bkYxVFRXOGJSUmgrMzVuZDJjM2FXY2YyZmpUTjdycitCanVrNEk4MXhQa3dxSkViS1ZWREUxV1JFR2trcWhTRnRxQlNMa1J3QUFtaHFMSlFKS3FLbnJaQ0hJQUx6ZzBoNWVKV0ZEaHdDb2YwQjFSSWNNckpKRGF6dG9OU1ZyUFB6TTc3enZ2TU0vc01ZTGNMUUtyNEdGUUkxMVJBd0hjQllGMHZFTkhJcHpXV2pTY21jQmJkUWd3TlV0VkNuV0JJSThHMjRpaHQ1VHZWUU0wSnRCV2xIZUNMU0xmYjNhQWVIUVlaNHBDdlBSZERCQlVwa2pvSUFDakFHcTlQQ2RJMUlHU1l6SnRKTFJvUnhkRThsaWFRYWNZTW9zWVNaNUVsRXhOa1JuQ0lxVW1tUVQ3ZC9uMmJOM1RHSjdYZHE1dUwyMi9YeU5UMXh0ZU42MU00dDZ2ako5ZTJ5ZDFmN3JFN25hL3NuTDQ3TjdQeHhZUEd6VW5odGZXN0Z6YXY3dXA4YTV6WDMrQVUvc1YxbWpYTjE0bjEvOVNXTlY4dGFpSDA2VE1sdHhndmNMVmNsaFZvQnd3VkQ3alN4eW9mODJZWkJrQUlvS2YxRWxDUUlNQy9vMkRDR01RZ0NVV29RQlZtNFNLOEQvZHFYNzZhSm5aay9vVVVqZHFrSG84Uk96cGtMNDloZE5RY3BrTnlkT2pLS1NOSVpUMHNVVkdReFZWdGhGRWhGS0FVUWNEVjB4aUpPQ3RuMEhHQzUxV0ZVQUJyQlN4cjJKcS85ZDZOald2cmI2MjkrY2JscFFzTGMrZG1aNmFucXBPdnZGeHh5NldYemo2ZlRTYmlaMktPYlkyZEhqMWxHam8vNmZEZ0dVbUlkdDRzbHBQK216M1I0Lzk2V2k1bGVCMG02WnBSS1JaY1BKRmZHY1RNUWF3UzFsZ3luc2lVd3lXM2ltVWZDb2JOVi9SSGtsNHNHSFhQZTloc1BqeEd2TCt6czlkczR2ZWV0N2V6MHdxeWxLUmdIKy8zcHZZOEw2TElTU21JSEdYbGozSHI4Rzg3bDdQUGw5S3BkR25QVFNYVEx0YnQzRVhQODFMTlpqUGxIYlc4dGcrcEpyN285YXA1L3VwT2hzYzg3NTBUVStOSFpiOFUrZFhPdWVsU0tlMzJNY2Z0UUx0SDNSL29GUm9BamZ0NEFtcTFhUldSRENFQjM4cUVJdmZ3RFdEY3lvejZsdVkvaWx0YkVFVmhHUVJCWEFGUkVCZDBRemVTbWFRa2p1VlIxMWdlRTVscFBIRTJKdmU4U2gweWd4Vk5SZC93V0hBUUR4UzVLU3VLL0pFaXBXVGw2YVd0Yis0c2t1WFB2LzNzOHVhdFIwOS92c2srL09uZ3g0OUpvaTBwdll3V1QvNXRhV3VaTERZZU5Iam0xdElIcmR1M1czLzZjT3g3WENXdmcrNzduajdyZTNQMDJQZW0wVE4vbHB1LzByL3N1TXIyOXhtelJKVjFEaGxEUVJ3UkxFWVNrdmhrbjZsOFhrU1J4MUR3Y3dZOEM4YzhnV2Q1ck1IOWtnYjNheGF6UHJnRmsvTXNpSjEvZUNHTERmaEdCRjRZSnlWL1dyUTQxZjZUZnJoektQNExYakhxajNpY1kyQmtZR0FBWW4rL211ZngvRFpmR2JpWlh3QkZHSzdzLzNVSlJ2Ly8reitMK1FWekVKREx3Y0FFRWdVQXFOUVFCUUI0bkdOZ1pHQmdEdnFmQlNSZi9QLzcveGZ6Q3dhZ0NBcmdBQUMyQ3dlVUFBUG9BQUFDTy8vL0E2RC8vd0k3QUFBRG9BQUFBMW4vL1FGbEFBQUJaUUFBQUFBQUFBQW1BSFFBbkFHS0FmQUNKQUpZQUFBQUFRQUFBQWdBZEFBUEFBQUFBQUFDQUVRQVZBQnpBQUFBcVF0d0FBQUFBSGljZFpMTmFzSkFGSVhQV0xWVW9ZdTJ0TnRaRlVXTVAxQ2tiaW9JdWlvVUZ5NjZpem9ta1ppUnlhajRESDJEdmtOZnFkQTM2VEVPdFFXYk1MbmZQWE55NzgwUUFGZjRoTURoZXVBNnNFQ0oyWUZ6T01lVDR6UHFBOGQ1OG92akFzcDRkVnlrcmh5WFVJTjJYTVkxM2xsQjVDK1lMZkRoV09CRzNEbk80VkxVSEo5UmYzU2NKejg3THVCVytJNkwxTGVPU3hpTE44ZGwzSXV2dmw3dFRCU0VWbGI2VmRsdXRqcHlzcE9hVXBUNHNmVFhOdFFtbFQwNTE0bFZjYXk5cVY2YWJYMGJ6UUpsMDVFSzFyRnZqc0tSeHNxa2tVNWt5MnNleGFGS2xQR3RtdTI3cEp1Z2JlMWN6bzFleW9HckwxZEdMOVRVZXFHMXEyNmo4YnN2K2p5b0ZYWXdpQkFnaElWRWhXcVZzWTBtV3VpUUpuUklPZyt1Q0FsOHhGUjhyUGxHbU8ya3pIdGNjMllKVlVWSFRQWXc1WE5KeHhaMXJnZ3oxbEIwcEJneEJxd1JzNUk1NlRpbGpSbjMvYUtzaytTTUhpYzk1Und5SnBuYnp5YWEvWHhMaWcxZGJhcVdFKytuTnRtVWtqL2EzL2tsejJlL3Q2QXlwZTVscDJTcGR0SGcvYy8zZmdPRlBJcHRBSGljYmNkQkRvQWdEQURCRmdVcnZvVkhFVUFnTm1BUTQvZU40ZXFjZGtIQW9PRWZvY0FKWjVTb2NFSENGZFI5R3ArYnVvSnRMcEd2VC9tZW5PVlF2RzNTY1hXSHRpVnlNQnoydm8xc09hWU84QUxPWmhYaEFBQjRuR1B3M3NGd0lpaGlJeU5qWCtRR3hwMGNEQndNeVFVYkdWaWROakV3TW1pQkdKdTVtQmc1SUN3K0JqQ0x6V2tYMHdHZ05DZVF6ZTYwaThFQndtWm1jTm1vd3RnUkdMSEJvU05pSTNPS3kwWTFFRzhYUndNREk0dERSM0pJQkVoSkpCQnM1bUZpNU5IYXdmaS9kUU5MNzBZbUJoY0FESFlqOUFBQSI="

/***/ }),
/* 88 */
/***/ (function(module, exports) {

module.exports = "data:font/ttf;base64,bW9kdWxlLmV4cG9ydHMgPSBfX3dlYnBhY2tfcHVibGljX3BhdGhfXyArICJydy13aWRnZXRzLnR0ZiI7"

/***/ }),
/* 89 */
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__.p + "rw-widgets.svg";

/***/ }),
/* 90 */
/***/ (function(module, exports) {

module.exports = "data:image/png;base64,R0lGODlhEAAQAPIAAP///zMzM87OzmdnZzMzM4GBgZqamqenpyH/C05FVFNDQVBFMi4wAwEAAAAh/hpDcmVhdGVkIHdpdGggYWpheGxvYWQuaW5mbwAh+QQJCgAAACwAAAAAEAAQAAADMwi63P4wyklrE2MIOggZnAdOmGYJRbExwroUmcG2LmDEwnHQLVsYOd2mBzkYDAdKa+dIAAAh+QQJCgAAACwAAAAAEAAQAAADNAi63P5OjCEgG4QMu7DmikRxQlFUYDEZIGBMRVsaqHwctXXf7WEYB4Ag1xjihkMZsiUkKhIAIfkECQoAAAAsAAAAABAAEAAAAzYIujIjK8pByJDMlFYvBoVjHA70GU7xSUJhmKtwHPAKzLO9HMaoKwJZ7Rf8AYPDDzKpZBqfvwQAIfkECQoAAAAsAAAAABAAEAAAAzMIumIlK8oyhpHsnFZfhYumCYUhDAQxRIdhHBGqRoKw0R8DYlJd8z0fMDgsGo/IpHI5TAAAIfkECQoAAAAsAAAAABAAEAAAAzIIunInK0rnZBTwGPNMgQwmdsNgXGJUlIWEuR5oWUIpz8pAEAMe6TwfwyYsGo/IpFKSAAAh+QQJCgAAACwAAAAAEAAQAAADMwi6IMKQORfjdOe82p4wGccc4CEuQradylesojEMBgsUc2G7sDX3lQGBMLAJibufbSlKAAAh+QQJCgAAACwAAAAAEAAQAAADMgi63P7wCRHZnFVdmgHu2nFwlWCI3WGc3TSWhUFGxTAUkGCbtgENBMJAEJsxgMLWzpEAACH5BAkKAAAALAAAAAAQABAAAAMyCLrc/jDKSatlQtScKdceCAjDII7HcQ4EMTCpyrCuUBjCYRgHVtqlAiB1YhiCnlsRkAAAOwAAAAAAAAAAAA=="

/***/ }),
/* 91 */
/***/ (function(module, exports) {

module.exports = "data:image/png;base64,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"

/***/ }),
/* 92 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(93);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(15)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 93 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(14)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.role-group-editor {\n  float: left;\n  width: 235px;\n  background-color: #FFFFFF;\n  border-radius: 3px;\n  border: 1px sol #C8C8C8;\n  padding: 20px;\n  -webkit-box-shadow: 0px 0px 1px 0px #C8C8C8;\n  -moz-box-shadow: 0px 0px 10px 0px #C8C8C8;\n  box-shadow: 0px 0px 10px 0px #C8C8C8;\n  z-index: 9999;\n  cursor: default;\n}\n.role-group-editor h2 {\n  color: #6F7273;\n  margin-bottom: 20px;\n}\n.role-group-editor textarea {\n  height: 75px;\n}\n.role-group-editor .dnn-ui-common-multi-line-input {\n  padding: 10px 10px;\n  border-radius: 0px;\n  height: 88px;\n}\n.role-group-editor .edit-form {\n  color: #4B4E4F;\n  clear: both;\n  text-align: left;\n}\n.role-group-editor .edit-form .form-items:after {\n  display: none;\n}\n.role-group-editor .edit-form.short-left {\n  width: 45%;\n  float: left;\n  clear: left;\n}\n.role-group-editor .edit-form.short-right {\n  width: 45%;\n  float: right;\n  clear: right;\n  padding-left: 5%;\n}\n.role-group-editor .edit-form.short-right::before {\n  content: \".\";\n  position: absolute;\n  height: 100%;\n  top: 0;\n  left: -5%;\n}\n.role-group-editor .edit-form .form-item {\n  line-height: 24px;\n  width: 100%;\n  clear: both;\n}\n.role-group-editor .edit-form .form-item.last {\n  margin-bottom: 0;\n}\n.role-group-editor .edit-form .form-item.short-left {\n  width: 45%;\n  float: left;\n  clear: left;\n}\n.role-group-editor .edit-form .form-item.short-right {\n  width: 45%;\n  float: right;\n  clear: right;\n}\n.role-group-editor .edit-form .form-item label {\n  display: block;\n  font-weight: bold;\n  color: #46292B;\n}\n.role-group-editor .edit-form .form-item label.inline {\n  display: inline-block;\n  vertical-align: middle;\n}\n.role-group-editor .edit-form .form-item > label.middle {\n  margin-bottom: 10px;\n}\n.role-group-editor .edit-form .form-item label.required:after {\n  content: \"*\";\n  color: #EA2134;\n  margin-left: 4px;\n}\n.role-group-editor .edit-form .form-item label + input[type=\"text\"],\n.role-group-editor .edit-form .form-item label + textarea,\n.role-group-editor .edit-form .form-item label + select {\n  width: 100%;\n  background-color: #FFFFFF;\n  border: 1px solid #959695;\n  -ms-border-radius: 0;\n  border-radius: 0;\n  outline: none;\n  -webkit-box-sizing: border-box;\n  -moz-box-sizing: border-box;\n  box-sizing: border-box;\n  margin-bottom: 0;\n  -webkit-box-shadow: none;\n  -moz-box-shadow: none;\n  box-shadow: none;\n  padding: 8px;\n  vertical-align: top;\n}\n.role-group-editor .edit-form .form-item label + input[type=\"text\"]:focus,\n.role-group-editor .edit-form .form-item label + textarea:focus {\n  color: #1E88C3;\n  text-shadow: 0px 0px 0px #46292B;\n  -webkit-text-fill-color: transparent;\n  border: 1px solid #1E88C3;\n}\n.role-group-editor .edit-form label.inline + * {\n  width: 50%;\n  float: right;\n}\n.role-group-editor .edit-form select {\n  height: 34px;\n  vertical-align: bottom;\n}\n.role-group-editor .edit-form .form-item.half-input {\n  position: relative;\n}\n.role-group-editor .edit-form .form-item.half-input > * {\n  width: 48%;\n}\n.role-group-editor .edit-form .form-item.half-input > input + * {\n  position: absolute;\n  right: 0;\n}\n.role-group-editor .edit-form .form-item .dnn-switch-container {\n  text-align: right;\n  width: 80px;\n}\n.role-group-editor .edit-form .form-item .dnn-switch-container label {\n  margin-top: 0px;\n}\n.role-group-editor .edit-form .form-item.calendar input {\n  width: 90%;\n}\n.role-group-editor .edit-form .form-item.calendar:after {\n  display: block;\n  content: \"\";\n  float: right;\n  background: none;\n  width: 22px;\n  height: 24px;\n  margin-top: 5px;\n}\n.role-group-editor .edit-form .actions {\n  text-align: center;\n  margin-top: 15px;\n}\n.role-group-editor .edit-form .actions button[role=\"secondary\"] {\n  margin-right: 11px;\n}\n.role-group-editor .edit-form .form-items {\n  position: relative;\n}\n.role-group-editor .edit-form .form-items:after {\n  display: block;\n  content: \"\";\n  height: 100%;\n  position: absolute;\n  left: 50%;\n  top: 0;\n}\n.role-group-editor .edit-form .form-item[readonly] label {\n  color: #959695;\n}\n.role-group-editor .edit-form .dnn-label .dnn-ui-common-tooltip .icon {\n  margin-top: 1px;\n}\n.role-group-editor .edit-form .dnn-label label {\n  float: left;\n}\n.role-group-editor .edit-form .dnn-ui-common-single-line-input {\n  border-radius: 0px;\n  width: 100%;\n}\n", ""]);



/***/ }),
/* 94 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(95);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(15)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 95 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(14)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.roles-list-container .collapsible-component1 {\n  display: table;\n  width: 100%;\n  border-bottom: 1px solid #C8C8C8;\n  cursor: auto;\n}\n.roles-list-container .collapsible-component1 > div {\n  float: left;\n  width: 100%;\n}\n.roles-list-container .collapsible-component1 > div > div {\n  float: left;\n  width: 100%;\n}\n.roles-list-container .collapsible-component1.true {\n  border-top: 2px solid #1E88C3;\n  border-bottom: 2px solid #1E88C3;\n  margin-top: -1px;\n}\n.roles-list-container .collapsible-component1 .role-row-collapsible {\n  background-color: #FFFFFF;\n}\n.roles-list-container .collapsible-component1 .role-row-collapsible > div {\n  padding: 25px;\n}\n.roles-list-container .collapsible-component1 div.collapsible-header1 {\n  color: #6F7273;\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 18px;\n  box-sizing: border-box;\n  cursor: auto;\n}\n.roles-list-container .collapsible-component1 div.collapsible-header1 .ellipsis {\n  -ms-text-overflow: ellipsis;\n  text-overflow: ellipsis;\n  overflow: hidden;\n  white-space: nowrap;\n}\n.roles-list-container .collapsible-component1 div.collapsible-header1 .edit-icon {\n  margin-left: 5px;\n  float: right;\n  cursor: pointer;\n}\n.roles-list-container .collapsible-component1 div.collapsible-header1 .edit-icon svg {\n  width: 18px;\n  float: left;\n  height: 18px;\n}\n.roles-list-container .collapsible-component1 div.collapsible-header1 .edit-icon.false svg {\n  fill: #1E88C3;\n}\n.roles-list-container .collapsible-component1 div.collapsible-header1 .edit-icon.false svg:hover {\n  fill: #4B4E4F;\n}\n.roles-list-container .collapsible-component1 div.collapsible-header1 .icon-flat {\n  display: inline-block;\n  width: 18px;\n  height: 18px;\n  vertical-align: middle;\n}\n.roles-list-container .collapsible-component1 div.collapsible-header1 .icon-flat svg {\n  fill: #4B4E4F;\n}\n", ""]);



/***/ }),
/* 96 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<polygon points=\"1793.7,585.8 1623.9,416.1 756.6,1283.4 424.4,951.3 254.6,1121 756.6,1623 926.4,1453.2 926.4,1453.2 \"/>\r\n</svg>\r\n");

/***/ }),
/* 97 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = uncontrollable;

var _react = _interopRequireDefault(__webpack_require__(0));

var _invariant = _interopRequireDefault(__webpack_require__(24));

var Utils = _interopRequireWildcard(__webpack_require__(98));

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

function uncontrollable(Component, controlledValues, methods) {
  if (methods === void 0) {
    methods = [];
  }

  var displayName = Component.displayName || Component.name || 'Component';
  var isCompositeComponent = Utils.isReactComponent(Component);
  var controlledProps = Object.keys(controlledValues);
  var PROPS_TO_OMIT = controlledProps.map(Utils.defaultKey);
  !(isCompositeComponent || !methods.length) ?  false ? undefined : invariant(false) : void 0;

  var UncontrolledComponent =
  /*#__PURE__*/
  function (_React$Component) {
    _inheritsLoose(UncontrolledComponent, _React$Component);

    function UncontrolledComponent() {
      var _this;

      for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
        args[_key] = arguments[_key];
      }

      _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;
      _this.handlers = Object.create(null);
      controlledProps.forEach(function (propName) {
        var handlerName = controlledValues[propName];

        var handleChange = function handleChange(value) {
          if (_this.props[handlerName]) {
            var _this$props;

            _this._notifying = true;

            for (var _len2 = arguments.length, args = new Array(_len2 > 1 ? _len2 - 1 : 0), _key2 = 1; _key2 < _len2; _key2++) {
              args[_key2 - 1] = arguments[_key2];
            }

            (_this$props = _this.props)[handlerName].apply(_this$props, [value].concat(args));

            _this._notifying = false;
          }

          _this._values[propName] = value;
          if (!_this.unmounted) _this.forceUpdate();
        };

        _this.handlers[handlerName] = handleChange;
      });
      if (isCompositeComponent) _this.attachRef = function (ref) {
        _this.inner = ref;
      };
      return _this;
    }

    var _proto = UncontrolledComponent.prototype;

    _proto.shouldComponentUpdate = function shouldComponentUpdate() {
      //let the forceUpdate trigger the update
      return !this._notifying;
    };

    _proto.componentWillMount = function componentWillMount() {
      var _this2 = this;

      var props = this.props;
      this._values = Object.create(null);
      controlledProps.forEach(function (key) {
        _this2._values[key] = props[Utils.defaultKey(key)];
      });
    };

    _proto.componentWillReceiveProps = function componentWillReceiveProps(nextProps) {
      var _this3 = this;

      var props = this.props;
      controlledProps.forEach(function (key) {
        /**
         * If a prop switches from controlled to Uncontrolled
         * reset its value to the defaultValue
         */
        if (!Utils.isProp(nextProps, key) && Utils.isProp(props, key)) {
          _this3._values[key] = nextProps[Utils.defaultKey(key)];
        }
      });
    };

    _proto.componentWillUnmount = function componentWillUnmount() {
      this.unmounted = true;
    };

    _proto.getControlledInstance = function getControlledInstance() {
      return this.inner;
    };

    _proto.render = function render() {
      var _this4 = this;

      var props = _extends({}, this.props);

      PROPS_TO_OMIT.forEach(function (prop) {
        delete props[prop];
      });
      var newProps = {};
      controlledProps.forEach(function (propName) {
        var propValue = _this4.props[propName];
        newProps[propName] = propValue !== undefined ? propValue : _this4._values[propName];
      });
      return _react.default.createElement(Component, _extends({}, props, newProps, this.handlers, {
        ref: this.attachRef
      }));
    };

    return UncontrolledComponent;
  }(_react.default.Component);

  UncontrolledComponent.displayName = "Uncontrolled(" + displayName + ")";
  UncontrolledComponent.propTypes = Utils.uncontrolledPropTypes(controlledValues, displayName);
  methods.forEach(function (method) {
    UncontrolledComponent.prototype[method] = function $proxiedMethod() {
      var _inner;

      return (_inner = this.inner)[method].apply(_inner, arguments);
    };
  });
  UncontrolledComponent.ControlledComponent = Component;
  /**
   * useful when wrapping a Component and you want to control
   * everything
   */

  UncontrolledComponent.deferControlTo = function (newComponent, additions, nextMethods) {
    if (additions === void 0) {
      additions = {};
    }

    return uncontrollable(newComponent, _extends({}, controlledValues, additions), nextMethods);
  };

  return UncontrolledComponent;
}

module.exports = exports["default"];

/***/ }),
/* 98 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.uncontrolledPropTypes = uncontrolledPropTypes;
exports.isProp = isProp;
exports.defaultKey = defaultKey;
exports.isReactComponent = isReactComponent;

var _invariant = _interopRequireDefault(__webpack_require__(24));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var noop = function noop() {};

function readOnlyPropType(handler, name) {
  return function (props, propName) {
    if (props[propName] !== undefined) {
      if (!props[handler]) {
        return new Error("You have provided a `" + propName + "` prop to `" + name + "` " + ("without an `" + handler + "` handler prop. This will render a read-only field. ") + ("If the field should be mutable use `" + defaultKey(propName) + "`. ") + ("Otherwise, set `" + handler + "`."));
      }
    }
  };
}

function uncontrolledPropTypes(controlledValues, displayName) {
  var propTypes = {};
  Object.keys(controlledValues).forEach(function (prop) {
    // add default propTypes for folks that use runtime checks
    propTypes[defaultKey(prop)] = noop;

    if (false) { var handler; }
  });
  return propTypes;
}

function isProp(props, prop) {
  return props[prop] !== undefined;
}

function defaultKey(key) {
  return 'default' + key.charAt(0).toUpperCase() + key.substr(1);
}
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 */


function isReactComponent(component) {
  return !!(component && component.prototype && component.prototype.isReactComponent);
}

/***/ }),
/* 99 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _classnames = _interopRequireDefault(__webpack_require__(12));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var Widget =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(Widget, _React$Component);

  function Widget() {
    return _React$Component.apply(this, arguments) || this;
  }

  var _proto = Widget.prototype;

  _proto.render = function render() {
    var _this$props = this.props,
        className = _this$props.className,
        tabIndex = _this$props.tabIndex,
        focused = _this$props.focused,
        open = _this$props.open,
        dropUp = _this$props.dropUp,
        disabled = _this$props.disabled,
        readOnly = _this$props.readOnly,
        autofilling = _this$props.autofilling,
        _this$props$isRtl = _this$props.isRtl,
        isRtl = _this$props$isRtl === void 0 ? this.context.isRtl : _this$props$isRtl,
        props = _objectWithoutProperties(_this$props, ["className", "tabIndex", "focused", "open", "dropUp", "disabled", "readOnly", "autofilling", "isRtl"]);

    tabIndex = tabIndex != null ? tabIndex : '-1';
    return _react.default.createElement("div", _extends({}, props, {
      tabIndex: tabIndex,
      className: (0, _classnames.default)(className, 'rw-widget', isRtl && 'rw-rtl', disabled && 'rw-state-disabled', readOnly && 'rw-state-readonly', focused && 'rw-state-focus', autofilling && 'rw-webkit-autofill', open && "rw-open" + (dropUp ? '-up' : ''))
    }));
  };

  return Widget;
}(_react.default.Component);

Widget.contextTypes = {
  isRtl: _propTypes.default.bool
};
Widget.propTypes = {
  tabIndex: _propTypes.default.node,
  focused: _propTypes.default.bool,
  disabled: _propTypes.default.bool,
  readOnly: _propTypes.default.bool,
  autofilling: _propTypes.default.bool,
  open: _propTypes.default.bool,
  dropUp: _propTypes.default.bool,
  isRtl: _propTypes.default.bool
};
var _default = Widget;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 100 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _classnames = _interopRequireDefault(__webpack_require__(12));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var WidgetPicker =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(WidgetPicker, _React$Component);

  function WidgetPicker() {
    return _React$Component.apply(this, arguments) || this;
  }

  var _proto = WidgetPicker.prototype;

  _proto.render = function render() {
    var _this$props = this.props,
        open = _this$props.open,
        dropUp = _this$props.dropUp,
        className = _this$props.className,
        disabled = _this$props.disabled,
        readOnly = _this$props.readOnly,
        focused = _this$props.focused,
        props = _objectWithoutProperties(_this$props, ["open", "dropUp", "className", "disabled", "readOnly", "focused"]);

    var openClass = "rw-open" + (dropUp ? '-up' : '');
    return _react.default.createElement("div", _extends({}, props, {
      className: (0, _classnames.default)(className, 'rw-widget-picker', 'rw-widget-container', open && openClass, disabled && 'rw-state-disabled', readOnly && 'rw-state-readonly', focused && 'rw-state-focus')
    }));
  };

  return WidgetPicker;
}(_react.default.Component);

WidgetPicker.propTypes = {
  tabIndex: _propTypes.default.node,
  focused: _propTypes.default.bool,
  disabled: _propTypes.default.bool,
  readOnly: _propTypes.default.bool,
  open: _propTypes.default.bool,
  dropUp: _propTypes.default.bool,
  picker: _propTypes.default.bool
};
var _default = WidgetPicker;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 101 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _react = _interopRequireDefault(__webpack_require__(0));

var _reactDom = __webpack_require__(16);

var CustomPropTypes = _interopRequireWildcard(__webpack_require__(19));

var Props = _interopRequireWildcard(__webpack_require__(25));

var _widgetHelpers = __webpack_require__(26);

var _reduceToListState = __webpack_require__(44);

var _Listbox = _interopRequireDefault(__webpack_require__(107));

var _ListOption = _interopRequireDefault(__webpack_require__(108));

var _ListOptionGroup = _interopRequireDefault(__webpack_require__(109));

var _messages = __webpack_require__(46);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var EMPTY_DATA_STATE = {};
var propTypes = {
  data: _propTypes.default.array,
  dataState: _propTypes.default.shape({
    sortedKeys: _propTypes.default.array,
    groups: _propTypes.default.object,
    data: _propTypes.default.array,
    sequentialData: _propTypes.default.array
  }),
  valueAccessor: CustomPropTypes.accessor,
  textAccessor: CustomPropTypes.accessor,
  onSelect: _propTypes.default.func,
  onMove: _propTypes.default.func,
  activeId: _propTypes.default.string,
  itemComponent: CustomPropTypes.elementType,
  groupComponent: CustomPropTypes.elementType,
  optionComponent: CustomPropTypes.elementType,
  renderItem: _propTypes.default.func,
  renderGroup: _propTypes.default.func,
  focusedItem: _propTypes.default.any,
  selectedItem: _propTypes.default.any,
  searchTerm: _propTypes.default.string,
  isDisabled: _propTypes.default.func.isRequired,
  messages: _propTypes.default.shape({
    emptyList: _propTypes.default.func.isRequired
  })
};
var defaultProps = {
  onSelect: function onSelect() {},
  data: [],
  dataState: EMPTY_DATA_STATE,
  optionComponent: _ListOption.default
};

var List =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(List, _React$Component);

  function List() {
    var _this;

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;

    _this.renderItem = function (_ref) {
      var item = _ref.item,
          rest = _objectWithoutProperties(_ref, ["item"]);

      var _this$props = _this.props,
          isDisabled = _this$props.isDisabled,
          renderItem = _this$props.renderItem,
          textAccessor = _this$props.textAccessor,
          valueAccessor = _this$props.valueAccessor;
      var Component = _this.props.itemComponent;

      if (renderItem) {
        return renderItem(_extends({
          item: item
        }, rest));
      } else if (Component) {
        return _react.default.createElement(Component, _extends({
          item: item,
          value: valueAccessor(item),
          text: textAccessor(item),
          disabled: isDisabled(item)
        }, rest));
      }

      return textAccessor(item);
    };

    _this.renderGroup = function (group) {
      var _this$props2 = _this.props,
          renderGroup = _this$props2.renderGroup,
          Component = _this$props2.groupComponent;

      if (renderGroup) {
        return renderGroup({
          group: group
        });
      } else if (Component) {
        return _react.default.createElement(Component, {
          item: group
        });
      }

      return group;
    };

    return _this;
  }

  var _proto = List.prototype;

  _proto.componentDidMount = function componentDidMount() {
    this.move();
  };

  _proto.componentDidUpdate = function componentDidUpdate() {
    this.move();
  };

  _proto.mapItems = function mapItems(fn) {
    var _this$props3 = this.props,
        data = _this$props3.data,
        dataState = _this$props3.dataState;
    var sortedKeys = dataState.sortedKeys,
        groups = dataState.groups;
    if (!groups) return data.map(function (item, idx) {
      return fn(item, idx, false);
    });
    var idx = -1;
    return sortedKeys.reduce(function (items, key) {
      var group = groups[key];
      return items.concat(fn(key, idx, true), group.map(function (item) {
        return fn(item, ++idx, false);
      }));
    }, []);
  };

  _proto.move = function move() {
    var _this$props4 = this.props,
        focusedItem = _this$props4.focusedItem,
        onMove = _this$props4.onMove,
        data = _this$props4.data,
        dataState = _this$props4.dataState;
    var list = (0, _reactDom.findDOMNode)(this);
    var idx = renderedIndexOf(focusedItem, list, data, dataState);
    var selectedItem = list.children[idx];
    if (selectedItem) (0, _widgetHelpers.notify)(onMove, [selectedItem, list, focusedItem]);
  };

  _proto.renderOption = function renderOption(item, index) {
    var _this$props5 = this.props,
        activeId = _this$props5.activeId,
        focusedItem = _this$props5.focusedItem,
        selectedItem = _this$props5.selectedItem,
        onSelect = _this$props5.onSelect,
        isDisabled = _this$props5.isDisabled,
        searchTerm = _this$props5.searchTerm,
        Option = _this$props5.optionComponent;
    var isFocused = focusedItem === item;
    return _react.default.createElement(Option, {
      dataItem: item,
      key: 'item_' + index,
      index: index,
      activeId: activeId,
      focused: isFocused,
      onSelect: onSelect,
      disabled: isDisabled(item),
      selected: selectedItem === item
    }, this.renderItem({
      item: item,
      index: index,
      searchTerm: searchTerm
    }));
  };

  _proto.render = function render() {
    var _this2 = this;

    var _this$props6 = this.props,
        className = _this$props6.className,
        messages = _this$props6.messages;
    var elementProps = Props.pickElementProps(this);

    var _getMessages = (0, _messages.getMessages)(messages),
        emptyList = _getMessages.emptyList;

    return _react.default.createElement(_Listbox.default, _extends({}, elementProps, {
      className: className,
      emptyListMessage: emptyList(this.props)
    }), this.mapItems(function (item, idx, isHeader) {
      return isHeader ? _react.default.createElement(_ListOptionGroup.default, {
        key: 'group_' + item,
        group: item
      }, _this2.renderGroup(item)) : _this2.renderOption(item, idx);
    }));
  };

  return List;
}(_react.default.Component);

List.getDataState = _reduceToListState.defaultGetDataState;

function renderedIndexOf(item, list, data, dataState) {
  var groups = dataState.groups,
      sortedKeys = dataState.sortedKeys;
  if (!groups) return data.indexOf(item);
  var runningIdx = -1;
  var idx = -1;
  sortedKeys.some(function (group) {
    var itemIdx = groups[group].indexOf(item);
    runningIdx++;

    if (itemIdx !== -1) {
      idx = runningIdx + itemIdx + 1;
      return true;
    }

    runningIdx += groups[group].length;
  });
  return idx;
}

List.propTypes = propTypes;
List.defaultProps = defaultProps;
var _default = List;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 102 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});

var _react = __webpack_require__(0);

var _react2 = _interopRequireDefault(_react);

var _reactIs = __webpack_require__(103);

var _createChainableTypeChecker = __webpack_require__(43);

var _createChainableTypeChecker2 = _interopRequireDefault(_createChainableTypeChecker);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function elementType(props, propName, componentName, location, propFullName) {
  var propValue = props[propName];

  if (_react2.default.isValidElement(propValue)) {
    return new Error('Invalid ' + location + ' `' + propFullName + '` of type ReactElement ' + ('supplied to `' + componentName + '`,expected an element type (a string ') + ', component class, or function component).');
  }

  if (!(0, _reactIs.isValidElementType)(propValue)) {
    return new Error('Invalid ' + location + ' `' + propFullName + '` of value `' + propValue + '` ' + ('supplied to `' + componentName + '`, expected an element type (a string ') + ', component class, or function component).');
  }

  return null;
}

exports.default = (0, _createChainableTypeChecker2.default)(elementType);
module.exports = exports['default'];

/***/ }),
/* 103 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


if (true) {
  module.exports = __webpack_require__(104);
} else {}


/***/ }),
/* 104 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/** @license React v16.13.1
 * react-is.production.min.js
 *
 * Copyright (c) Facebook, Inc. and its affiliates.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

var b="function"===typeof Symbol&&Symbol.for,c=b?Symbol.for("react.element"):60103,d=b?Symbol.for("react.portal"):60106,e=b?Symbol.for("react.fragment"):60107,f=b?Symbol.for("react.strict_mode"):60108,g=b?Symbol.for("react.profiler"):60114,h=b?Symbol.for("react.provider"):60109,k=b?Symbol.for("react.context"):60110,l=b?Symbol.for("react.async_mode"):60111,m=b?Symbol.for("react.concurrent_mode"):60111,n=b?Symbol.for("react.forward_ref"):60112,p=b?Symbol.for("react.suspense"):60113,q=b?
Symbol.for("react.suspense_list"):60120,r=b?Symbol.for("react.memo"):60115,t=b?Symbol.for("react.lazy"):60116,v=b?Symbol.for("react.block"):60121,w=b?Symbol.for("react.fundamental"):60117,x=b?Symbol.for("react.responder"):60118,y=b?Symbol.for("react.scope"):60119;
function z(a){if("object"===typeof a&&null!==a){var u=a.$$typeof;switch(u){case c:switch(a=a.type,a){case l:case m:case e:case g:case f:case p:return a;default:switch(a=a&&a.$$typeof,a){case k:case n:case t:case r:case h:return a;default:return u}}case d:return u}}}function A(a){return z(a)===m}exports.AsyncMode=l;exports.ConcurrentMode=m;exports.ContextConsumer=k;exports.ContextProvider=h;exports.Element=c;exports.ForwardRef=n;exports.Fragment=e;exports.Lazy=t;exports.Memo=r;exports.Portal=d;
exports.Profiler=g;exports.StrictMode=f;exports.Suspense=p;exports.isAsyncMode=function(a){return A(a)||z(a)===l};exports.isConcurrentMode=A;exports.isContextConsumer=function(a){return z(a)===k};exports.isContextProvider=function(a){return z(a)===h};exports.isElement=function(a){return"object"===typeof a&&null!==a&&a.$$typeof===c};exports.isForwardRef=function(a){return z(a)===n};exports.isFragment=function(a){return z(a)===e};exports.isLazy=function(a){return z(a)===t};
exports.isMemo=function(a){return z(a)===r};exports.isPortal=function(a){return z(a)===d};exports.isProfiler=function(a){return z(a)===g};exports.isStrictMode=function(a){return z(a)===f};exports.isSuspense=function(a){return z(a)===p};
exports.isValidElementType=function(a){return"string"===typeof a||"function"===typeof a||a===e||a===m||a===g||a===f||a===p||a===q||"object"===typeof a&&null!==a&&(a.$$typeof===t||a.$$typeof===r||a.$$typeof===h||a.$$typeof===k||a.$$typeof===n||a.$$typeof===w||a.$$typeof===x||a.$$typeof===y||a.$$typeof===v)};exports.typeOf=z;


/***/ }),
/* 105 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.setNumber = setNumber;
exports.setDate = setDate;
exports.date = exports.number = void 0;

var _invariant = _interopRequireDefault(__webpack_require__(24));

var _ = __webpack_require__(20);

var _propTypes = _interopRequireDefault(__webpack_require__(2));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var localePropType = _propTypes.default.oneOfType([_propTypes.default.string, _propTypes.default.func]);

var REQUIRED_NUMBER_FORMATS = ['default'];
var REQUIRED_DATE_FORMATS = ['default', 'date', 'time', 'header', 'footer', 'weekday', 'dayOfMonth', 'month', 'year', 'decade', 'century'];

var _numberLocalizer = createWrapper('NumberPicker');

var number = {
  propType: function propType() {
    var _numberLocalizer2;

    return (_numberLocalizer2 = _numberLocalizer).propType.apply(_numberLocalizer2, arguments);
  },
  getFormat: function getFormat(key, format) {
    return format || _numberLocalizer.formats[key];
  },
  parse: function parse() {
    var _numberLocalizer3;

    return (_numberLocalizer3 = _numberLocalizer).parse.apply(_numberLocalizer3, arguments);
  },
  format: function format() {
    var _numberLocalizer4;

    return (_numberLocalizer4 = _numberLocalizer).format.apply(_numberLocalizer4, arguments);
  },
  decimalChar: function decimalChar() {
    var _numberLocalizer5;

    return (_numberLocalizer5 = _numberLocalizer).decimalChar.apply(_numberLocalizer5, arguments);
  },
  precision: function precision() {
    var _numberLocalizer6;

    return (_numberLocalizer6 = _numberLocalizer).precision.apply(_numberLocalizer6, arguments);
  }
};
exports.number = number;

function setNumber(_ref) {
  var format = _ref.format,
      _parse = _ref.parse,
      formats = _ref.formats,
      _ref$propType = _ref.propType,
      propType = _ref$propType === void 0 ? localePropType : _ref$propType,
      _ref$decimalChar = _ref.decimalChar,
      decimalChar = _ref$decimalChar === void 0 ? function () {
    return '.';
  } : _ref$decimalChar,
      _ref$precision = _ref.precision,
      precision = _ref$precision === void 0 ? function () {
    return null;
  } : _ref$precision;
  checkFormats(REQUIRED_NUMBER_FORMATS, formats);
  _numberLocalizer = {
    formats: formats,
    precision: precision,
    decimalChar: decimalChar,
    propType: propType,
    format: wrapFormat(format),
    parse: function parse(value, culture, format) {
      var result = _parse.call(this, value, culture, format);

      !(result == null || typeof result === 'number') ?  false ? undefined : invariant(false) : void 0;
      return result;
    }
  };
}

var _dateLocalizer = createWrapper('DateTimePicker');

var date = {
  propType: function propType() {
    var _dateLocalizer2;

    return (_dateLocalizer2 = _dateLocalizer).propType.apply(_dateLocalizer2, arguments);
  },
  getFormat: function getFormat(key, format) {
    return format || _dateLocalizer.formats[key];
  },
  parse: function parse() {
    var _dateLocalizer3;

    return (_dateLocalizer3 = _dateLocalizer).parse.apply(_dateLocalizer3, arguments);
  },
  format: function format() {
    var _dateLocalizer4;

    return (_dateLocalizer4 = _dateLocalizer).format.apply(_dateLocalizer4, arguments);
  },
  firstOfWeek: function firstOfWeek() {
    var _dateLocalizer5;

    return (_dateLocalizer5 = _dateLocalizer).firstOfWeek.apply(_dateLocalizer5, arguments);
  }
};
exports.date = date;

function setDate(_ref2) {
  var formats = _ref2.formats,
      format = _ref2.format,
      _parse2 = _ref2.parse,
      firstOfWeek = _ref2.firstOfWeek,
      _ref2$propType = _ref2.propType,
      propType = _ref2$propType === void 0 ? localePropType : _ref2$propType;
  checkFormats(REQUIRED_DATE_FORMATS, formats);
  _dateLocalizer = {
    formats: formats,
    propType: propType,
    firstOfWeek: firstOfWeek,
    format: wrapFormat(format),
    parse: function parse(value, format, culture) {
      var result = _parse2.call(this, value, format, culture);

      !(result == null || result instanceof Date && !isNaN(result.getTime())) ?  false ? undefined : invariant(false) : void 0;
      return result;
    }
  };
}

var wrapFormat = function wrapFormat(formatter) {
  return function (value, format, culture) {
    var result = typeof format === 'function' ? format(value, culture, this) : formatter.call(this, value, format, culture);
    !(result == null || typeof result === 'string') ?  false ? undefined : invariant(false) : void 0;
    return result;
  };
};

function checkFormats(required, formats) {
  if (false) {}
}

function createWrapper() {
  var dummy = {};

  if (false) {}

  return dummy;
}

/***/ }),
/* 106 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright 2014-2015, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 */



/**
 * Similar to invariant but only logs a warning if the condition is not met.
 * This can be used to log issues in development environments in critical
 * paths. Removing the logging code for production environments will keep the
 * same logic and follow the same code paths.
 */

var warning = function() {};

if (false) {}

module.exports = warning;


/***/ }),
/* 107 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _classnames = _interopRequireDefault(__webpack_require__(12));

var _widgetHelpers = __webpack_require__(26);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var propTypes = {
  className: _propTypes.default.string,
  role: _propTypes.default.string,
  nodeRef: _propTypes.default.func,
  emptyListMessage: _propTypes.default.node
};

var Listbox =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(Listbox, _React$Component);

  function Listbox() {
    return _React$Component.apply(this, arguments) || this;
  }

  var _proto = Listbox.prototype;

  _proto.render = function render() {
    var _this$props = this.props,
        className = _this$props.className,
        role = _this$props.role,
        children = _this$props.children,
        emptyListMessage = _this$props.emptyListMessage,
        nodeRef = _this$props.nodeRef,
        props = _objectWithoutProperties(_this$props, ["className", "role", "children", "emptyListMessage", "nodeRef"]);

    var id = (0, _widgetHelpers.instanceId)(this);
    return _react.default.createElement("ul", _extends({
      id: id,
      tabIndex: "-1",
      ref: nodeRef,
      className: (0, _classnames.default)(className, 'rw-list'),
      role: role === undefined ? 'listbox' : role
    }, props), _react.default.Children.count(children) ? children : _react.default.createElement("li", {
      className: "rw-list-empty"
    }, emptyListMessage));
  };

  return Listbox;
}(_react.default.Component);

Listbox.propTypes = propTypes;
var _default = Listbox;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 108 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _classnames = _interopRequireDefault(__webpack_require__(12));

var Props = _interopRequireWildcard(__webpack_require__(25));

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var ListOption =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(ListOption, _React$Component);

  function ListOption() {
    var _this;

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;

    _this.handleSelect = function (event) {
      var _this$props = _this.props,
          onSelect = _this$props.onSelect,
          disabled = _this$props.disabled,
          dataItem = _this$props.dataItem;
      if (onSelect && !disabled) onSelect(dataItem, event);
    };

    return _this;
  }

  var _proto = ListOption.prototype;

  _proto.render = function render() {
    var _this$props2 = this.props,
        className = _this$props2.className,
        children = _this$props2.children,
        focused = _this$props2.focused,
        selected = _this$props2.selected,
        disabled = _this$props2.disabled,
        activeId = _this$props2.activeId;
    var Tag = this.props.component || 'li';
    var props = Props.omitOwn(this);
    var classes = {
      'rw-state-focus': focused,
      'rw-state-selected': selected,
      'rw-state-disabled': disabled
    };
    var id = focused ? activeId : undefined;
    return _react.default.createElement(Tag, _extends({
      id: id,
      role: "option",
      tabIndex: !disabled ? '-1' : undefined,
      "aria-selected": !!selected,
      className: (0, _classnames.default)('rw-list-option', className, classes),
      onClick: this.handleSelect
    }, props), children);
  };

  return ListOption;
}(_react.default.Component);

ListOption.propTypes = {
  activeId: _propTypes.default.string,
  dataItem: _propTypes.default.any,
  index: _propTypes.default.number,
  focused: _propTypes.default.bool,
  selected: _propTypes.default.bool,
  disabled: _propTypes.default.bool,
  onSelect: _propTypes.default.func,
  component: _propTypes.default.string
};
var _default = ListOption;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 109 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _classnames = _interopRequireDefault(__webpack_require__(12));

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var propTypes = {
  className: _propTypes.default.string,
  component: _propTypes.default.string
};

function ListOptionGroup(_ref) {
  var children = _ref.children,
      className = _ref.className,
      _ref$component = _ref.component,
      component = _ref$component === void 0 ? 'li' : _ref$component;
  var Tag = component;
  return _react.default.createElement(Tag, {
    tabIndex: "-1",
    role: "separator",
    className: (0, _classnames.default)(className, 'rw-list-optgroup')
  }, children);
}

ListOptionGroup.propTypes = propTypes;
var _default = ListOptionGroup;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 110 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _classnames = _interopRequireDefault(__webpack_require__(12));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _react = _interopRequireWildcard(__webpack_require__(0));

var _SlideDownTransition = _interopRequireDefault(__webpack_require__(111));

var _PropTypes = __webpack_require__(19);

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var StaticContainer =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(StaticContainer, _React$Component);

  function StaticContainer() {
    return _React$Component.apply(this, arguments) || this;
  }

  var _proto = StaticContainer.prototype;

  _proto.shouldComponentUpdate = function shouldComponentUpdate(_ref) {
    var shouldUpdate = _ref.shouldUpdate;
    return !!shouldUpdate;
  };

  _proto.render = function render() {
    var _this$props = this.props,
        className = _this$props.className,
        children = _this$props.children,
        props = _objectWithoutProperties(_this$props, ["className", "children"]);

    delete props.shouldUpdate;
    return (0, _react.cloneElement)(children, _extends({}, props, {
      className: (0, _classnames.default)(className, children.props.className, 'rw-popup')
    }));
  };

  return StaticContainer;
}(_react.default.Component);

StaticContainer.propTypes = {
  shouldUpdate: function shouldUpdate() {}
};

var Popup =
/*#__PURE__*/
function (_React$Component2) {
  _inheritsLoose(Popup, _React$Component2);

  function Popup() {
    return _React$Component2.apply(this, arguments) || this;
  }

  var _proto2 = Popup.prototype;

  _proto2.render = function render() {
    var _this$props2 = this.props,
        className = _this$props2.className,
        dropUp = _this$props2.dropUp,
        open = _this$props2.open,
        Transition = _this$props2.transition,
        props = _objectWithoutProperties(_this$props2, ["className", "dropUp", "open", "transition"]);

    return _react.default.createElement(Transition, _extends({}, props, {
      in: open,
      dropUp: dropUp,
      className: (0, _classnames.default)(className, 'rw-popup-container')
    }), _react.default.createElement(StaticContainer, {
      shouldUpdate: open
    }, _react.default.Children.only(this.props.children)));
  };

  return Popup;
}(_react.default.Component);

Popup.defaultProps = {
  open: false,
  transition: _SlideDownTransition.default
};
Popup.propTypes = {
  open: _propTypes.default.bool,
  dropUp: _propTypes.default.bool,
  onEntering: _propTypes.default.func,
  onEntered: _propTypes.default.func,
  transition: _PropTypes.elementType
};
var _default = Popup;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 111 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _classnames = _interopRequireDefault(__webpack_require__(12));

var _events = _interopRequireDefault(__webpack_require__(112));

var _style = _interopRequireDefault(__webpack_require__(51));

var _height = _interopRequireDefault(__webpack_require__(28));

var _properties = __webpack_require__(53);

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _react = _interopRequireDefault(__webpack_require__(0));

var _Transition = _interopRequireWildcard(__webpack_require__(122));

var _transitionClasses;

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var transitionClasses = (_transitionClasses = {}, _transitionClasses[_Transition.ENTERING] = 'rw-popup-transition-entering', _transitionClasses[_Transition.EXITING] = 'rw-popup-transition-exiting', _transitionClasses[_Transition.EXITED] = 'rw-popup-transition-exited', _transitionClasses);
var propTypes = {
  in: _propTypes.default.bool.isRequired,
  dropUp: _propTypes.default.bool,
  onEntering: _propTypes.default.func,
  onEntered: _propTypes.default.func
};

function parseDuration(node) {
  var str = (0, _style.default)(node, _properties.transitionDuration);
  var mult = str.indexOf('ms') === -1 ? 1000 : 1;
  return parseFloat(str) * mult;
}

var SlideDownTransition =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(SlideDownTransition, _React$Component);

  function SlideDownTransition() {
    var _this;

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;

    _this.setContainerHeight = function (elem) {
      elem.style.height = _this.getHeight() + 'px';
    };

    _this.clearContainerHeight = function (elem) {
      elem.style.height = '';
    };

    _this.handleEntered = function (elem) {
      _this.clearContainerHeight(elem);

      if (_this.props.onEntered) _this.props.onEntered();
    };

    _this.handleEntering = function () {
      if (_this.props.onEntering) _this.props.onEntering();
    };

    _this.handleTransitionEnd = function (node, done) {
      var duration = parseDuration(node.lastChild) || 0;

      var handler = function handler() {
        _events.default.off(node, _properties.transitionEnd, handler, false);

        done();
      };

      setTimeout(handler, duration * 1.5);

      _events.default.on(node, _properties.transitionEnd, handler, false);
    };

    _this.attachRef = function (ref) {
      return _this.element = ref;
    };

    return _this;
  }

  var _proto = SlideDownTransition.prototype;

  _proto.getHeight = function getHeight() {
    var container = this.element;
    var content = container.firstChild;
    var margin = parseInt((0, _style.default)(content, 'margin-top'), 10) + parseInt((0, _style.default)(content, 'margin-bottom'), 10);
    var old = container.style.display;
    var height;
    container.style.display = 'block';
    height = ((0, _height.default)(content) || 0) + (isNaN(margin) ? 0 : margin);
    container.style.display = old;
    return height;
  };

  _proto.render = function render() {
    var _this2 = this;

    var _this$props = this.props,
        children = _this$props.children,
        className = _this$props.className,
        dropUp = _this$props.dropUp;
    return _react.default.createElement(_Transition.default, {
      appear: true,
      in: this.props.in,
      timeout: 5000,
      onEnter: this.setContainerHeight,
      onEntering: this.handleEntering,
      onEntered: this.handleEntered,
      onExit: this.setContainerHeight,
      onExited: this.clearContainerHeight,
      addEndListener: this.handleTransitionEnd
    }, function (status, innerProps) {
      return _react.default.createElement("div", _extends({}, innerProps, {
        ref: _this2.attachRef,
        className: (0, _classnames.default)(className, dropUp && 'rw-dropup', transitionClasses[status])
      }), _react.default.createElement("div", {
        className: "rw-popup-transition"
      }, children));
    });
  };

  return SlideDownTransition;
}(_react.default.Component);

SlideDownTransition.propTypes = propTypes;
var _default = SlideDownTransition;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 112 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = void 0;

var _on = _interopRequireDefault(__webpack_require__(47));

exports.on = _on.default;

var _off = _interopRequireDefault(__webpack_require__(48));

exports.off = _off.default;

var _filter = _interopRequireDefault(__webpack_require__(113));

exports.filter = _filter.default;

var _listen = _interopRequireDefault(__webpack_require__(114));

exports.listen = _listen.default;
var _default = {
  on: _on.default,
  off: _off.default,
  filter: _filter.default,
  listen: _listen.default
};
exports.default = _default;

/***/ }),
/* 113 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = filterEvents;

var _contains = _interopRequireDefault(__webpack_require__(49));

var _querySelectorAll = _interopRequireDefault(__webpack_require__(50));

function filterEvents(selector, handler) {
  return function filterHandler(e) {
    var top = e.currentTarget,
        target = e.target,
        matches = (0, _querySelectorAll.default)(top, selector);
    if (matches.some(function (match) {
      return (0, _contains.default)(match, target);
    })) handler.call(this, e);
  };
}

module.exports = exports["default"];

/***/ }),
/* 114 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = void 0;

var _inDOM = _interopRequireDefault(__webpack_require__(17));

var _on = _interopRequireDefault(__webpack_require__(47));

var _off = _interopRequireDefault(__webpack_require__(48));

var listen = function listen() {};

if (_inDOM.default) {
  listen = function listen(node, eventName, handler, capture) {
    (0, _on.default)(node, eventName, handler, capture);
    return function () {
      (0, _off.default)(node, eventName, handler, capture);
    };
  };
}

var _default = listen;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 115 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = camelize;
var rHyphen = /-(.)/g;

function camelize(string) {
  return string.replace(rHyphen, function (_, chr) {
    return chr.toUpperCase();
  });
}

module.exports = exports["default"];

/***/ }),
/* 116 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = hyphenateStyleName;

var _hyphenate = _interopRequireDefault(__webpack_require__(117));

/**
 * Copyright 2013-2014, Facebook, Inc.
 * All rights reserved.
 * https://github.com/facebook/react/blob/2aeb8a2a6beb00617a4217f7f8284924fa2ad819/src/vendor/core/hyphenateStyleName.js
 */
var msPattern = /^ms-/;

function hyphenateStyleName(string) {
  return (0, _hyphenate.default)(string).replace(msPattern, '-ms-');
}

module.exports = exports["default"];

/***/ }),
/* 117 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = hyphenate;
var rUpper = /([A-Z])/g;

function hyphenate(string) {
  return string.replace(rUpper, '-$1').toLowerCase();
}

module.exports = exports["default"];

/***/ }),
/* 118 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = _getComputedStyle;

var _camelizeStyle = _interopRequireDefault(__webpack_require__(52));

var rposition = /^(top|right|bottom|left)$/;
var rnumnonpx = /^([+-]?(?:\d*\.|)\d+(?:[eE][+-]?\d+|))(?!px)[a-z%]+$/i;

function _getComputedStyle(node) {
  if (!node) throw new TypeError('No Element passed to `getComputedStyle()`');
  var doc = node.ownerDocument;
  return 'defaultView' in doc ? doc.defaultView.opener ? node.ownerDocument.defaultView.getComputedStyle(node, null) : window.getComputedStyle(node, null) : {
    //ie 8 "magic" from: https://github.com/jquery/jquery/blob/1.11-stable/src/css/curCSS.js#L72
    getPropertyValue: function getPropertyValue(prop) {
      var style = node.style;
      prop = (0, _camelizeStyle.default)(prop);
      if (prop == 'float') prop = 'styleFloat';
      var current = node.currentStyle[prop] || null;
      if (current == null && style && style[prop]) current = style[prop];

      if (rnumnonpx.test(current) && !rposition.test(prop)) {
        // Remember the original values
        var left = style.left;
        var runStyle = node.runtimeStyle;
        var rsLeft = runStyle && runStyle.left; // Put in the new values to get a computed value out

        if (rsLeft) runStyle.left = node.currentStyle.left;
        style.left = prop === 'fontSize' ? '1em' : current;
        current = style.pixelLeft + 'px'; // Revert the changed values

        style.left = left;
        if (rsLeft) runStyle.left = rsLeft;
      }

      return current;
    }
  };
}

module.exports = exports["default"];

/***/ }),
/* 119 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = removeStyle;

function removeStyle(node, key) {
  return 'removeProperty' in node.style ? node.style.removeProperty(key) : node.style.removeAttribute(key);
}

module.exports = exports["default"];

/***/ }),
/* 120 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = isTransform;
var supportedTransforms = /^((translate|rotate|scale)(X|Y|Z|3d)?|matrix(3d)?|perspective|skew(X|Y)?)$/i;

function isTransform(property) {
  return !!(property && supportedTransforms.test(property));
}

module.exports = exports["default"];

/***/ }),
/* 121 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = ownerDocument;

function ownerDocument(node) {
  return node && node.ownerDocument || document;
}

module.exports = exports["default"];

/***/ }),
/* 122 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = exports.EXITING = exports.ENTERED = exports.ENTERING = exports.EXITED = exports.UNMOUNTED = void 0;

var PropTypes = _interopRequireWildcard(__webpack_require__(2));

var _react = _interopRequireDefault(__webpack_require__(0));

var _reactDom = _interopRequireDefault(__webpack_require__(16));

var _reactLifecyclesCompat = __webpack_require__(42);

var _PropTypes = __webpack_require__(123);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function _objectWithoutPropertiesLoose(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var UNMOUNTED = 'unmounted';
exports.UNMOUNTED = UNMOUNTED;
var EXITED = 'exited';
exports.EXITED = EXITED;
var ENTERING = 'entering';
exports.ENTERING = ENTERING;
var ENTERED = 'entered';
exports.ENTERED = ENTERED;
var EXITING = 'exiting';
/**
 * The Transition component lets you describe a transition from one component
 * state to another _over time_ with a simple declarative API. Most commonly
 * it's used to animate the mounting and unmounting of a component, but can also
 * be used to describe in-place transition states as well.
 *
 * ---
 *
 * **Note**: `Transition` is a platform-agnostic base component. If you're using
 * transitions in CSS, you'll probably want to use
 * [`CSSTransition`](https://reactcommunity.org/react-transition-group/css-transition)
 * instead. It inherits all the features of `Transition`, but contains
 * additional features necessary to play nice with CSS transitions (hence the
 * name of the component).
 *
 * ---
 *
 * By default the `Transition` component does not alter the behavior of the
 * component it renders, it only tracks "enter" and "exit" states for the
 * components. It's up to you to give meaning and effect to those states. For
 * example we can add styles to a component when it enters or exits:
 *
 * ```jsx
 * import { Transition } from 'react-transition-group';
 *
 * const duration = 300;
 *
 * const defaultStyle = {
 *   transition: `opacity ${duration}ms ease-in-out`,
 *   opacity: 0,
 * }
 *
 * const transitionStyles = {
 *   entering: { opacity: 0 },
 *   entered:  { opacity: 1 },
 * };
 *
 * const Fade = ({ in: inProp }) => (
 *   <Transition in={inProp} timeout={duration}>
 *     {state => (
 *       <div style={{
 *         ...defaultStyle,
 *         ...transitionStyles[state]
 *       }}>
 *         I'm a fade Transition!
 *       </div>
 *     )}
 *   </Transition>
 * );
 * ```
 *
 * There are 4 main states a Transition can be in:
 *  - `'entering'`
 *  - `'entered'`
 *  - `'exiting'`
 *  - `'exited'`
 *
 * Transition state is toggled via the `in` prop. When `true` the component
 * begins the "Enter" stage. During this stage, the component will shift from
 * its current transition state, to `'entering'` for the duration of the
 * transition and then to the `'entered'` stage once it's complete. Let's take
 * the following example (we'll use the
 * [useState](https://reactjs.org/docs/hooks-reference.html#usestate) hook):
 *
 * ```jsx
 * function App() {
 *   const [inProp, setInProp] = useState(false);
 *   return (
 *     <div>
 *       <Transition in={inProp} timeout={500}>
 *         {state => (
 *           // ...
 *         )}
 *       </Transition>
 *       <button onClick={() => setInProp(true)}>
 *         Click to Enter
 *       </button>
 *     </div>
 *   );
 * }
 * ```
 *
 * When the button is clicked the component will shift to the `'entering'` state
 * and stay there for 500ms (the value of `timeout`) before it finally switches
 * to `'entered'`.
 *
 * When `in` is `false` the same thing happens except the state moves from
 * `'exiting'` to `'exited'`.
 */

exports.EXITING = EXITING;

var Transition =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(Transition, _React$Component);

  function Transition(props, context) {
    var _this;

    _this = _React$Component.call(this, props, context) || this;
    var parentGroup = context.transitionGroup; // In the context of a TransitionGroup all enters are really appears

    var appear = parentGroup && !parentGroup.isMounting ? props.enter : props.appear;
    var initialStatus;
    _this.appearStatus = null;

    if (props.in) {
      if (appear) {
        initialStatus = EXITED;
        _this.appearStatus = ENTERING;
      } else {
        initialStatus = ENTERED;
      }
    } else {
      if (props.unmountOnExit || props.mountOnEnter) {
        initialStatus = UNMOUNTED;
      } else {
        initialStatus = EXITED;
      }
    }

    _this.state = {
      status: initialStatus
    };
    _this.nextCallback = null;
    return _this;
  }

  var _proto = Transition.prototype;

  _proto.getChildContext = function getChildContext() {
    return {
      transitionGroup: null // allows for nested Transitions

    };
  };

  Transition.getDerivedStateFromProps = function getDerivedStateFromProps(_ref, prevState) {
    var nextIn = _ref.in;

    if (nextIn && prevState.status === UNMOUNTED) {
      return {
        status: EXITED
      };
    }

    return null;
  }; // getSnapshotBeforeUpdate(prevProps) {
  //   let nextStatus = null
  //   if (prevProps !== this.props) {
  //     const { status } = this.state
  //     if (this.props.in) {
  //       if (status !== ENTERING && status !== ENTERED) {
  //         nextStatus = ENTERING
  //       }
  //     } else {
  //       if (status === ENTERING || status === ENTERED) {
  //         nextStatus = EXITING
  //       }
  //     }
  //   }
  //   return { nextStatus }
  // }


  _proto.componentDidMount = function componentDidMount() {
    this.updateStatus(true, this.appearStatus);
  };

  _proto.componentDidUpdate = function componentDidUpdate(prevProps) {
    var nextStatus = null;

    if (prevProps !== this.props) {
      var status = this.state.status;

      if (this.props.in) {
        if (status !== ENTERING && status !== ENTERED) {
          nextStatus = ENTERING;
        }
      } else {
        if (status === ENTERING || status === ENTERED) {
          nextStatus = EXITING;
        }
      }
    }

    this.updateStatus(false, nextStatus);
  };

  _proto.componentWillUnmount = function componentWillUnmount() {
    this.cancelNextCallback();
  };

  _proto.getTimeouts = function getTimeouts() {
    var timeout = this.props.timeout;
    var exit, enter, appear;
    exit = enter = appear = timeout;

    if (timeout != null && typeof timeout !== 'number') {
      exit = timeout.exit;
      enter = timeout.enter; // TODO: remove fallback for next major

      appear = timeout.appear !== undefined ? timeout.appear : enter;
    }

    return {
      exit: exit,
      enter: enter,
      appear: appear
    };
  };

  _proto.updateStatus = function updateStatus(mounting, nextStatus) {
    if (mounting === void 0) {
      mounting = false;
    }

    if (nextStatus !== null) {
      // nextStatus will always be ENTERING or EXITING.
      this.cancelNextCallback();

      var node = _reactDom.default.findDOMNode(this);

      if (nextStatus === ENTERING) {
        this.performEnter(node, mounting);
      } else {
        this.performExit(node);
      }
    } else if (this.props.unmountOnExit && this.state.status === EXITED) {
      this.setState({
        status: UNMOUNTED
      });
    }
  };

  _proto.performEnter = function performEnter(node, mounting) {
    var _this2 = this;

    var enter = this.props.enter;
    var appearing = this.context.transitionGroup ? this.context.transitionGroup.isMounting : mounting;
    var timeouts = this.getTimeouts();
    var enterTimeout = appearing ? timeouts.appear : timeouts.enter; // no enter animation skip right to ENTERED
    // if we are mounting and running this it means appear _must_ be set

    if (!mounting && !enter) {
      this.safeSetState({
        status: ENTERED
      }, function () {
        _this2.props.onEntered(node);
      });
      return;
    }

    this.props.onEnter(node, appearing);
    this.safeSetState({
      status: ENTERING
    }, function () {
      _this2.props.onEntering(node, appearing);

      _this2.onTransitionEnd(node, enterTimeout, function () {
        _this2.safeSetState({
          status: ENTERED
        }, function () {
          _this2.props.onEntered(node, appearing);
        });
      });
    });
  };

  _proto.performExit = function performExit(node) {
    var _this3 = this;

    var exit = this.props.exit;
    var timeouts = this.getTimeouts(); // no exit animation skip right to EXITED

    if (!exit) {
      this.safeSetState({
        status: EXITED
      }, function () {
        _this3.props.onExited(node);
      });
      return;
    }

    this.props.onExit(node);
    this.safeSetState({
      status: EXITING
    }, function () {
      _this3.props.onExiting(node);

      _this3.onTransitionEnd(node, timeouts.exit, function () {
        _this3.safeSetState({
          status: EXITED
        }, function () {
          _this3.props.onExited(node);
        });
      });
    });
  };

  _proto.cancelNextCallback = function cancelNextCallback() {
    if (this.nextCallback !== null) {
      this.nextCallback.cancel();
      this.nextCallback = null;
    }
  };

  _proto.safeSetState = function safeSetState(nextState, callback) {
    // This shouldn't be necessary, but there are weird race conditions with
    // setState callbacks and unmounting in testing, so always make sure that
    // we can cancel any pending setState callbacks after we unmount.
    callback = this.setNextCallback(callback);
    this.setState(nextState, callback);
  };

  _proto.setNextCallback = function setNextCallback(callback) {
    var _this4 = this;

    var active = true;

    this.nextCallback = function (event) {
      if (active) {
        active = false;
        _this4.nextCallback = null;
        callback(event);
      }
    };

    this.nextCallback.cancel = function () {
      active = false;
    };

    return this.nextCallback;
  };

  _proto.onTransitionEnd = function onTransitionEnd(node, timeout, handler) {
    this.setNextCallback(handler);
    var doesNotHaveTimeoutOrListener = timeout == null && !this.props.addEndListener;

    if (!node || doesNotHaveTimeoutOrListener) {
      setTimeout(this.nextCallback, 0);
      return;
    }

    if (this.props.addEndListener) {
      this.props.addEndListener(node, this.nextCallback);
    }

    if (timeout != null) {
      setTimeout(this.nextCallback, timeout);
    }
  };

  _proto.render = function render() {
    var status = this.state.status;

    if (status === UNMOUNTED) {
      return null;
    }

    var _this$props = this.props,
        children = _this$props.children,
        childProps = _objectWithoutPropertiesLoose(_this$props, ["children"]); // filter props for Transtition


    delete childProps.in;
    delete childProps.mountOnEnter;
    delete childProps.unmountOnExit;
    delete childProps.appear;
    delete childProps.enter;
    delete childProps.exit;
    delete childProps.timeout;
    delete childProps.addEndListener;
    delete childProps.onEnter;
    delete childProps.onEntering;
    delete childProps.onEntered;
    delete childProps.onExit;
    delete childProps.onExiting;
    delete childProps.onExited;

    if (typeof children === 'function') {
      return children(status, childProps);
    }

    var child = _react.default.Children.only(children);

    return _react.default.cloneElement(child, childProps);
  };

  return Transition;
}(_react.default.Component);

Transition.contextTypes = {
  transitionGroup: PropTypes.object
};
Transition.childContextTypes = {
  transitionGroup: function transitionGroup() {}
};
Transition.propTypes =  false ? undefined : {};

function noop() {}

Transition.defaultProps = {
  in: false,
  mountOnEnter: false,
  unmountOnExit: false,
  appear: false,
  enter: true,
  exit: true,
  onEnter: noop,
  onEntering: noop,
  onEntered: noop,
  onExit: noop,
  onExiting: noop,
  onExited: noop
};
Transition.UNMOUNTED = 0;
Transition.EXITED = 1;
Transition.ENTERING = 2;
Transition.ENTERED = 3;
Transition.EXITING = 4;

var _default = (0, _reactLifecyclesCompat.polyfill)(Transition);

exports.default = _default;

/***/ }),
/* 123 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.classNamesShape = exports.timeoutsShape = void 0;

var _propTypes = _interopRequireDefault(__webpack_require__(2));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var timeoutsShape =  false ? undefined : null;
exports.timeoutsShape = timeoutsShape;
var classNamesShape =  false ? undefined : null;
exports.classNamesShape = classNamesShape;

/***/ }),
/* 124 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _classnames = _interopRequireDefault(__webpack_require__(12));

var _Button = _interopRequireDefault(__webpack_require__(125));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var Select =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(Select, _React$Component);

  function Select() {
    return _React$Component.apply(this, arguments) || this;
  }

  var _proto = Select.prototype;

  _proto.render = function render() {
    var _this$props = this.props,
        className = _this$props.className,
        bordered = _this$props.bordered,
        children = _this$props.children,
        props = _objectWithoutProperties(_this$props, ["className", "bordered", "children"]);

    return _react.default.createElement("span", {
      className: (0, _classnames.default)(className, 'rw-select', bordered && 'rw-select-bordered')
    }, children ? _react.default.Children.map(children, function (child) {
      return child && _react.default.cloneElement(child, {
        variant: 'select'
      });
    }) : _react.default.createElement(_Button.default, _extends({}, props, {
      variant: "select"
    })));
  };

  return Select;
}(_react.default.Component);

Select.propTypes = {
  bordered: _propTypes.default.bool
};
var _default = Select;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 125 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _classnames = _interopRequireDefault(__webpack_require__(12));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var Loading = function Loading() {
  return _react.default.createElement("span", {
    "aria-hidden": "true",
    className: "rw-i rw-loading"
  });
};

var Button =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(Button, _React$Component);

  function Button() {
    return _React$Component.apply(this, arguments) || this;
  }

  var _proto = Button.prototype;

  _proto.render = function render() {
    var _this$props = this.props,
        className = _this$props.className,
        disabled = _this$props.disabled,
        label = _this$props.label,
        icon = _this$props.icon,
        busy = _this$props.busy,
        active = _this$props.active,
        children = _this$props.children,
        _this$props$variant = _this$props.variant,
        variant = _this$props$variant === void 0 ? 'primary' : _this$props$variant,
        _this$props$spinner = _this$props.spinner,
        spinner = _this$props$spinner === void 0 ? _react.default.createElement(Loading, null) : _this$props$spinner,
        _this$props$component = _this$props.component,
        Tag = _this$props$component === void 0 ? 'button' : _this$props$component,
        props = _objectWithoutProperties(_this$props, ["className", "disabled", "label", "icon", "busy", "active", "children", "variant", "spinner", "component"]);

    var type = props.type;
    if (Tag === 'button') type = type || 'button';
    return _react.default.createElement(Tag, _extends({}, props, {
      tabIndex: "-1",
      title: label,
      type: type,
      disabled: disabled,
      "aria-disabled": disabled,
      "aria-label": label,
      className: (0, _classnames.default)(className, 'rw-btn', active && !disabled && 'rw-state-active', variant && 'rw-btn-' + variant)
    }), busy ? spinner : icon, children);
  };

  return Button;
}(_react.default.Component);

Button.propTypes = {
  disabled: _propTypes.default.bool,
  label: _propTypes.default.string,
  icon: _propTypes.default.node,
  busy: _propTypes.default.bool,
  active: _propTypes.default.bool,
  variant: _propTypes.default.oneOf(['primary', 'select']),
  component: _propTypes.default.any,
  spinner: _propTypes.default.node
};
var _default = Button;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 126 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = exports.caretSet = void 0;

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _reactDom = __webpack_require__(16);

var _Input = _interopRequireDefault(__webpack_require__(127));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function _inheritsLoose(subClass, superClass) { subClass.prototype = Object.create(superClass.prototype); subClass.prototype.constructor = subClass; subClass.__proto__ = superClass; }

var caretSet = function caretSet(node, start, end) {
  if (end === void 0) {
    end = start;
  }

  try {
    node.setSelectionRange(start, end);
  } catch (e) {
    /* not focused or not visible */
  }
};

exports.caretSet = caretSet;

var ComboboxInput =
/*#__PURE__*/
function (_React$Component) {
  _inheritsLoose(ComboboxInput, _React$Component);

  function ComboboxInput() {
    var _this;

    for (var _len = arguments.length, args = new Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    _this = _React$Component.call.apply(_React$Component, [this].concat(args)) || this;

    _this.handleChange = function (e) {
      var _this$props = _this.props,
          placeholder = _this$props.placeholder,
          value = _this$props.value,
          onChange = _this$props.onChange;
      var stringValue = e.target.value;
      var hasPlaceholder = !!placeholder; // IE fires input events when setting/unsetting placeholders.
      // issue #112

      if (hasPlaceholder && !stringValue && stringValue === (value || '')) return;
      _this._last = stringValue;
      onChange(e, stringValue);
    };

    return _this;
  }

  var _proto = ComboboxInput.prototype;

  _proto.componentDidUpdate = function componentDidUpdate() {
    var input = (0, _reactDom.findDOMNode)(this);
    var val = this.props.value;

    if (this.isSuggesting()) {
      var start = val.toLowerCase().indexOf(this._last.toLowerCase()) + this._last.length;

      var end = val.length - start;

      if (start >= 0 && end !== 0) {
        caretSet(input, start, start + end);
      }
    }
  };

  _proto.accept = function accept(clearSelection) {
    if (clearSelection === void 0) {
      clearSelection = false;
    }

    this._last = null;

    if (clearSelection) {
      var node = (0, _reactDom.findDOMNode)(this);
      caretSet(node, node.value.length);
    }
  };

  _proto.focus = function focus() {
    (0, _reactDom.findDOMNode)(this).focus();
  };

  _proto.isSuggesting = function isSuggesting() {
    var _this$props2 = this.props,
        value = _this$props2.value,
        suggest = _this$props2.suggest;
    if (!suggest) return false;
    return this._last != null && value.toLowerCase().indexOf(this._last.toLowerCase()) !== -1;
  };

  _proto.render = function render() {
    var _this$props3 = this.props,
        onKeyDown = _this$props3.onKeyDown,
        props = _objectWithoutProperties(_this$props3, ["onKeyDown"]);

    delete props.suggest;
    return _react.default.createElement(_Input.default, _extends({}, props, {
      className: "rw-widget-input",
      onKeyDown: onKeyDown,
      onChange: this.handleChange
    }));
  };

  return ComboboxInput;
}(_react.default.Component);

ComboboxInput.defaultProps = {
  value: ''
};
ComboboxInput.propTypes = {
  value: _propTypes.default.string,
  placeholder: _propTypes.default.string,
  suggest: _propTypes.default.bool,
  onChange: _propTypes.default.func.isRequired,
  onKeyDown: _propTypes.default.func
};
var _default = ComboboxInput;
exports.default = _default;

/***/ }),
/* 127 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = void 0;

var _react = _interopRequireDefault(__webpack_require__(0));

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _classnames = _interopRequireDefault(__webpack_require__(12));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

Input.propTypes = {
  disabled: _propTypes.default.bool,
  readOnly: _propTypes.default.bool,
  value: _propTypes.default.string,
  type: _propTypes.default.string,
  tabIndex: _propTypes.default.string,
  component: _propTypes.default.any,
  nodeRef: _propTypes.default.func
};

function Input(_ref) {
  var className = _ref.className,
      disabled = _ref.disabled,
      readOnly = _ref.readOnly,
      value = _ref.value,
      tabIndex = _ref.tabIndex,
      nodeRef = _ref.nodeRef,
      _ref$type = _ref.type,
      type = _ref$type === void 0 ? 'text' : _ref$type,
      _ref$component = _ref.component,
      Component = _ref$component === void 0 ? 'input' : _ref$component,
      props = _objectWithoutProperties(_ref, ["className", "disabled", "readOnly", "value", "tabIndex", "nodeRef", "type", "component"]);

  return _react.default.createElement(Component, _extends({}, props, {
    type: type,
    ref: nodeRef,
    tabIndex: tabIndex || 0,
    autoComplete: "off",
    disabled: disabled,
    readOnly: readOnly,
    "aria-disabled": disabled,
    "aria-readonly": readOnly,
    value: value == null ? '' : value,
    className: (0, _classnames.default)(className, 'rw-input')
  }));
}

var _default = Input;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 128 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = createFocusManager;

var _reactComponentManagers = __webpack_require__(55);

var _interaction = __webpack_require__(57);

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function createFocusManager(inst, options) {
  var _didHandle = options.didHandle;
  return (0, _reactComponentManagers.focusManager)(inst, _extends({}, options, {
    onChange: function onChange(focused) {
      inst.setState({
        focused: focused
      });
    },
    isDisabled: function isDisabled() {
      return inst.props.disabled === true || (0, _interaction.isInDisabledFieldset)(inst);
    },
    didHandle: function didHandle(focused, event) {
      var handler = this.props[focused ? 'onFocus' : 'onBlur'];
      handler && handler(event);
      if (_didHandle && !event.isWidgetDefaultPrevented) _didHandle(focused, event);
    }
  }));
}

module.exports = exports["default"];

/***/ }),
/* 129 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = makeAutoFocusable;
exports.PropTypes = void 0;

var _propTypes = __webpack_require__(2);

var _reactDom = __webpack_require__(16);

var _spyOnComponent = _interopRequireDefault(__webpack_require__(22));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var PropTypes = {
  autoFocus: _propTypes.bool
};
exports.PropTypes = PropTypes;

function makeAutoFocusable(instance) {
  (0, _spyOnComponent.default)(instance, {
    componentDidMount: function componentDidMount() {
      var autoFocus = this.props.autoFocus;
      if (autoFocus) this.focus ? this.focus() : (0, _reactDom.findDOMNode)(this).focus();
    }
  });
}

/***/ }),
/* 130 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.callFocusEventHandler = callFocusEventHandler;
exports.default = createFocusManager;

var _reactDom = __webpack_require__(16);

var _timeoutManager = _interopRequireDefault(__webpack_require__(56));

var _mountManager = _interopRequireDefault(__webpack_require__(29));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function callFocusEventHandler(inst, focused, e) {
  var handler = inst.props[focused ? 'onFocus' : 'onBlur'];
  handler && handler(e);
}

function createFocusManager(instance, _temp) {
  var _ref = _temp === void 0 ? {} : _temp,
      willHandle = _ref.willHandle,
      didHandle = _ref.didHandle,
      onChange = _ref.onChange,
      _ref$isDisabled = _ref.isDisabled,
      isDisabled = _ref$isDisabled === void 0 ? function () {
    return !!instance.props.disabled;
  } : _ref$isDisabled;

  var lastFocused;
  var timeouts = (0, _timeoutManager.default)(instance);
  var isMounted = (0, _mountManager.default)(instance);

  function _handleFocus(focused, event) {
    if (event && event.persist) event.persist();
    if (willHandle && willHandle(focused, event) === false) return;
    timeouts.set('focus', function () {
      (0, _reactDom.unstable_batchedUpdates)(function () {
        if (focused !== lastFocused) {
          if (didHandle) didHandle.call(instance, focused, event); // only fire a change when unmounted if its a blur

          if (isMounted() || !focused) {
            lastFocused = focused;
            onChange && onChange(focused, event);
          }
        }
      });
    });
  }

  return {
    handleBlur: function handleBlur(event) {
      if (!isDisabled()) _handleFocus(false, event);
    },
    handleFocus: function handleFocus(event) {
      if (!isDisabled()) _handleFocus(true, event);
    }
  };
}

/***/ }),
/* 131 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.mixin = mixin;
exports.default = mixIntoClass;

function _extends() { _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; }; return _extends.apply(this, arguments); }

function _objectWithoutProperties(source, excluded) { if (source == null) return {}; var target = {}; var sourceKeys = Object.keys(source); var key, i; for (i = 0; i < sourceKeys.length; i++) { key = sourceKeys[i]; if (excluded.indexOf(key) >= 0) continue; target[key] = source[key]; } if (Object.getOwnPropertySymbols) { var sourceSymbolKeys = Object.getOwnPropertySymbols(source); for (i = 0; i < sourceSymbolKeys.length; i++) { key = sourceSymbolKeys[i]; if (excluded.indexOf(key) >= 0) continue; if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue; target[key] = source[key]; } } return target; }

function mixin(componentClass, _ref) {
  var propTypes = _ref.propTypes,
      contextTypes = _ref.contextTypes,
      childContextTypes = _ref.childContextTypes,
      getChildContext = _ref.getChildContext,
      protoSpec = _objectWithoutProperties(_ref, ["propTypes", "contextTypes", "childContextTypes", "getChildContext"]);

  if (propTypes) componentClass.propTypes = _extends({}, componentClass.propTypes, propTypes);
  if (contextTypes) componentClass.contextTypes = _extends({}, componentClass.contextTypes, contextTypes);
  if (childContextTypes) componentClass.childContextTypes = _extends({}, componentClass.childContextTypes, childContextTypes);

  if (getChildContext) {
    var baseGCContext = componentClass.prototype.getChildContext;

    componentClass.prototype.getChildContext = function $getChildContext() {
      return _extends({}, baseGCContext && baseGCContext.call(this), getChildContext.call(this));
    };
  }

  _extends(componentClass.prototype, protoSpec);

  return componentClass;
}

function mixIntoClass(spec) {
  return function (componentClass) {
    return mixin(componentClass, spec);
  };
}

/***/ }),
/* 132 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = matches;

var _inDOM = _interopRequireDefault(__webpack_require__(17));

var _querySelectorAll = _interopRequireDefault(__webpack_require__(50));

var matchesCache;

function matches(node, selector) {
  if (!matchesCache && _inDOM.default) {
    var body = document.body;
    var nativeMatch = body.matches || body.matchesSelector || body.webkitMatchesSelector || body.mozMatchesSelector || body.msMatchesSelector;
    matchesCache = nativeMatch ? function (node, selector) {
      return nativeMatch.call(node, selector);
    } : ie8MatchesSelector;
  }

  return matchesCache ? matchesCache(node, selector) : null;
}

function ie8MatchesSelector(node, selector) {
  var matches = (0, _querySelectorAll.default)(node.document || node.ownerDocument, selector),
      i = 0;

  while (matches[i] && matches[i] !== node) {
    i++;
  }

  return !!matches[i];
}

module.exports = exports["default"];

/***/ }),
/* 133 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = createAccessors;

var helpers = _interopRequireWildcard(__webpack_require__(27));

function _interopRequireWildcard(obj) { if (obj && obj.__esModule) { return obj; } else { var newObj = {}; if (obj != null) { for (var key in obj) { if (Object.prototype.hasOwnProperty.call(obj, key)) { var desc = Object.defineProperty && Object.getOwnPropertyDescriptor ? Object.getOwnPropertyDescriptor(obj, key) : {}; if (desc.get || desc.set) { Object.defineProperty(newObj, key, desc); } else { newObj[key] = obj[key]; } } } } newObj.default = obj; return newObj; } }

function createAccessors(_ref) {
  var textField = _ref.textField,
      valueField = _ref.valueField;
  return {
    text: function text(item) {
      return helpers.dataText(item, textField);
    },
    value: function value(item) {
      return helpers.dataValue(item, valueField);
    },
    indexOf: function indexOf(data, item) {
      return helpers.dataIndexOf(data, item, valueField);
    },
    matches: function matches(a, b) {
      return helpers.valueMatcher(a, b, valueField);
    },
    findOrSelf: function findOrSelf(data, item) {
      return helpers.dataItem(data, item, valueField);
    },
    includes: function includes(data, item) {
      return helpers.dataIndexOf(data, item, valueField) !== -1;
    }
  };
}

module.exports = exports["default"];

/***/ }),
/* 134 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = createScrollManager;

var _scrollTo = _interopRequireDefault(__webpack_require__(135));

var _reactComponentManagers = __webpack_require__(55);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function createScrollManager(inst, getScrollParent) {
  if (getScrollParent === void 0) {
    getScrollParent = function getScrollParent(list) {
      return list.parentNode;
    };
  }

  var isMounted = (0, _reactComponentManagers.mountManager)(inst);
  var currentFocused, currentVisible, cancelScroll;

  function handleScroll(selected, list, nextFocused) {
    if (!isMounted()) return;
    var lastVisible = currentVisible;
    var lastItem = currentFocused;
    var shown, changed;
    currentVisible = !(!list.offsetWidth || !list.offsetHeight);
    currentFocused = nextFocused;
    changed = lastItem !== nextFocused;
    shown = currentVisible && !lastVisible;

    if (shown || currentVisible && changed) {
      if (this.props.onMove) this.props.onMove(selected, list, nextFocused);else {
        cancelScroll && cancelScroll();
        cancelScroll = (0, _scrollTo.default)(selected,  false && false);
      }
    }
  }

  return handleScroll.bind(inst);
}

module.exports = exports["default"];

/***/ }),
/* 135 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = scrollTo;

var _offset = _interopRequireDefault(__webpack_require__(54));

var _height = _interopRequireDefault(__webpack_require__(28));

var _scrollParent = _interopRequireDefault(__webpack_require__(136));

var _scrollTop = _interopRequireDefault(__webpack_require__(137));

var _requestAnimationFrame = _interopRequireDefault(__webpack_require__(138));

var _isWindow = _interopRequireDefault(__webpack_require__(21));

function scrollTo(selected, scrollParent) {
  var offset = (0, _offset.default)(selected);
  var poff = {
    top: 0,
    left: 0
  };
  var list, listScrollTop, selectedTop, isWin;
  var selectedHeight, listHeight, bottom;
  if (!selected) return;
  list = scrollParent || (0, _scrollParent.default)(selected);
  isWin = (0, _isWindow.default)(list);
  listScrollTop = (0, _scrollTop.default)(list);
  listHeight = (0, _height.default)(list, true);
  isWin = (0, _isWindow.default)(list);
  if (!isWin) poff = (0, _offset.default)(list);
  offset = {
    top: offset.top - poff.top,
    left: offset.left - poff.left,
    height: offset.height,
    width: offset.width
  };
  selectedHeight = offset.height;
  selectedTop = offset.top + (isWin ? 0 : listScrollTop);
  bottom = selectedTop + selectedHeight;
  listScrollTop = listScrollTop > selectedTop ? selectedTop : bottom > listScrollTop + listHeight ? bottom - listHeight : listScrollTop;
  var id = (0, _requestAnimationFrame.default)(function () {
    return (0, _scrollTop.default)(list, listScrollTop);
  });
  return function () {
    return _requestAnimationFrame.default.cancel(id);
  };
}

module.exports = exports["default"];

/***/ }),
/* 136 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = scrollPrarent;

var _style = _interopRequireDefault(__webpack_require__(51));

var _height = _interopRequireDefault(__webpack_require__(28));

function scrollPrarent(node) {
  var position = (0, _style.default)(node, 'position'),
      excludeStatic = position === 'absolute',
      ownerDoc = node.ownerDocument;
  if (position === 'fixed') return ownerDoc || document;

  while ((node = node.parentNode) && node.nodeType !== 9) {
    var isStatic = excludeStatic && (0, _style.default)(node, 'position') === 'static',
        style = (0, _style.default)(node, 'overflow') + (0, _style.default)(node, 'overflow-y') + (0, _style.default)(node, 'overflow-x');
    if (isStatic) continue;
    if (/(auto|scroll)/.test(style) && (0, _height.default)(node) < node.scrollHeight) return node;
  }

  return document;
}

module.exports = exports["default"];

/***/ }),
/* 137 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = scrollTop;

var _isWindow = _interopRequireDefault(__webpack_require__(21));

function scrollTop(node, val) {
  var win = (0, _isWindow.default)(node);
  if (val === undefined) return win ? 'pageYOffset' in win ? win.pageYOffset : win.document.documentElement.scrollTop : node.scrollTop;
  if (win) win.scrollTo('pageXOffset' in win ? win.pageXOffset : win.document.documentElement.scrollLeft, val);else node.scrollTop = val;
}

module.exports = exports["default"];

/***/ }),
/* 138 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var _interopRequireDefault = __webpack_require__(8);

exports.__esModule = true;
exports.default = void 0;

var _inDOM = _interopRequireDefault(__webpack_require__(17));

var vendors = ['', 'webkit', 'moz', 'o', 'ms'];
var cancel = 'clearTimeout';
var raf = fallback;
var compatRaf;

var getKey = function getKey(vendor, k) {
  return vendor + (!vendor ? k : k[0].toUpperCase() + k.substr(1)) + 'AnimationFrame';
};

if (_inDOM.default) {
  vendors.some(function (vendor) {
    var rafKey = getKey(vendor, 'request');

    if (rafKey in window) {
      cancel = getKey(vendor, 'cancel');
      return raf = function raf(cb) {
        return window[rafKey](cb);
      };
    }
  });
}
/* https://github.com/component/raf */


var prev = new Date().getTime();

function fallback(fn) {
  var curr = new Date().getTime(),
      ms = Math.max(0, 16 - (curr - prev)),
      req = setTimeout(fn, ms);
  prev = curr;
  return req;
}

compatRaf = function compatRaf(cb) {
  return raf(cb);
};

compatRaf.cancel = function (id) {
  window[cancel] && typeof window[cancel] === 'function' && window[cancel](id);
};

var _default = compatRaf;
exports.default = _default;
module.exports = exports["default"];

/***/ }),
/* 139 */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.default = exports.search = exports.clock = exports.calendar = exports.chevronLeft = exports.chevronRight = exports.caretDown = exports.caretUp = void 0;

var _propTypes = _interopRequireDefault(__webpack_require__(2));

var _react = _interopRequireDefault(__webpack_require__(0));

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var propTypes = {
  icon: _propTypes.default.string.isRequired
};

var Icon = function Icon(_ref) {
  var icon = _ref.icon;
  return _react.default.createElement("span", {
    "aria-hidden": "true",
    className: "rw-i rw-i-" + icon
  });
};

Icon.propTypes = propTypes;

var caretUp = _react.default.createElement(Icon, {
  icon: "caret-up"
});

exports.caretUp = caretUp;

var caretDown = _react.default.createElement(Icon, {
  icon: "caret-down"
});

exports.caretDown = caretDown;

var chevronRight = _react.default.createElement(Icon, {
  icon: "chevron-right"
});

exports.chevronRight = chevronRight;

var chevronLeft = _react.default.createElement(Icon, {
  icon: "chevron-left"
});

exports.chevronLeft = chevronLeft;

var calendar = _react.default.createElement(Icon, {
  icon: "calendar"
});

exports.calendar = calendar;

var clock = _react.default.createElement(Icon, {
  icon: "clock-o"
});

exports.clock = clock;

var search = _react.default.createElement(Icon, {
  icon: "search"
});

exports.search = search;
var _default = Icon;
exports.default = _default;

/***/ }),
/* 140 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(141);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(15)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 141 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(14)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.role-user-row {\n  width: 100%;\n  float: left;\n  padding: 13px 20px;\n  box-sizing: border-box;\n}\n.role-user-row:not(:last-child) {\n  border-bottom: 1px solid #C8C8C8;\n}\n.role-user-row div.edit-row a {\n  display: inline-block;\n}\n.role-user-row .dnn-grid-cell {\n  box-sizing: border-box;\n  text-overflow: ellipsis;\n  white-space: nowrap;\n  overflow: hidden;\n}\n.role-user-row .dnn-grid-cell .actions {\n  float: right;\n  box-sizing: border-box;\n  height: 20px;\n  text-align: right;\n}\n.role-user-row .dnn-grid-cell .actions > div {\n  text-align: right;\n}\n.role-user-row .dnn-grid-cell .actions span {\n  float: right;\n}\n.role-user-row .dnn-grid-cell .actions span .dnn-day-picker {\n  height: 19px;\n}\n.role-user-row .dnn-grid-cell .actions span .dnn-day-picker .calendar-icon {\n  opacity: 0;\n  width: 19px;\n  height: 19px;\n}\n.role-user-row .dnn-grid-cell .actions span .dnn-day-picker .calendar-icon.active {\n  opacity: 1;\n}\n.role-user-row .dnn-grid-cell .actions span a svg {\n  width: 23px;\n  height: 23px;\n}\n.role-user-row .dnn-grid-cell .actions a {\n  float: right;\n  opacity: 0;\n}\n.role-user-row .dnn-grid-cell .actions:hover a {\n  display: inline-block;\n}\n.role-user-row .dnn-grid-cell .actions div.edit-row a {\n  display: inline-block;\n}\n.role-user-row .dnn-grid-cell:last-child {\n  overflow: visible;\n}\n.role-user-row:hover .dnn-grid-cell .actions span {\n  float: right;\n}\n.role-user-row:hover .dnn-grid-cell .actions span .dnn-day-picker .calendar-icon {\n  opacity: 1;\n}\n.role-user-row:hover .dnn-grid-cell .actions a {\n  opacity: 1;\n}\n", ""]);



/***/ }),
/* 142 */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony default export */ __webpack_exports__["default"] = ("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<!-- Generator: Adobe Illustrator 20.0.0, SVG Export Plug-In . SVG Version: 6.00 Build 0)  -->\r\n<svg version=\"1.1\" id=\"Layer_1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" x=\"0px\" y=\"0px\"\r\n\t viewBox=\"0 0 2048 2048\" style=\"enable-background:new 0 0 2048 2048;\" xml:space=\"preserve\">\r\n<g>\r\n\t<path d=\"M662.6,1565.4L483,1385.8l360.6-360.6L480.1,661.8l182.6-182.6l363.5,363.4l359-359l179.6,179.6l-359,359l362,362\r\n\t\tl-182.6,182.6l-362-362L662.6,1565.4z\"/>\r\n</g>\r\n</svg>\r\n");

/***/ }),
/* 143 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(144);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(15)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 144 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(14)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.roles-list-container .roleusers-form {\n  width: 80%;\n  margin: 20px 10%;\n  float: left;\n  box-sizing: border-box;\n}\n.roles-list-container .roleusers-form.isloading {\n  pointer-events: none !important;\n}\n.roles-list-container .roleusers-form.isloading * {\n  pointer-events: none !important;\n}\n.roles-list-container .roleusers-form.isloading .role-user-row {\n  opacity: 0.4;\n}\n.roles-list-container .roleusers-form .role-users-list {\n  float: left;\n  width: 100%;\n  margin: 15px 0;\n  display: table;\n  cursor: auto;\n  box-sizing: border-box;\n  border: 1px solid #C8C8C8;\n}\n.roles-list-container .roleusers-form .role-users-list .role-user-body {\n  color: #6F7273;\n}\n.roles-list-container .roleusers-form .role-users-list .role-user-body .no-roles-row {\n  width: 100%;\n  float: left;\n  text-align: center;\n  font-weight: bold;\n  padding: 13px 20px;\n  box-sizing: border-box;\n}\n.roles-list-container .roleusers-form .role-users-list .role-user-header-row {\n  display: table;\n  border-bottom: 1px solid #C8C8C8;\n  width: 100%;\n  float: left;\n  position: relative;\n  padding: 15px 20px;\n  box-sizing: border-box;\n  text-transform: uppercase;\n}\n.roles-list-container .roleusers-form .role-users-list .role-user-header-row .search-container {\n  float: left;\n  width: 100%;\n  box-sizing: border-box;\n}\n.roles-list-container .roleusers-form .role-users-list .role-user-header-row .search-container > div {\n  float: left;\n  width: 100%;\n  box-sizing: border-box;\n  display: block !important;\n}\n.roles-list-container .roleusers-form .role-users-list .role-user-header-row .search-container > div input {\n  display: block;\n  width: 100%;\n  border: none;\n  border-left: 1px solid #C8C8C8;\n  background-color: transparent;\n  border-radius: 0;\n  outline: none;\n  padding-right: 24px;\n}\n.roles-list-container .roleusers-form .role-users-list .role-user-header-row .search-container > div svg {\n  width: 16px !important;\n  height: 16px !important;\n  margin-top: 7px;\n}\n.roles-list-container .roleusers-form .header {\n  width: 100%;\n  float: left;\n  box-sizing: border-box;\n}\n.roles-list-container .roleusers-form .header .header-title {\n  width: 100%;\n  font-weight: bold;\n  display: inline-block;\n  text-transform: uppercase;\n  margin-top: 10px;\n  margin-bottom: 10px;\n}\n.roles-list-container .roleusers-form .header .add-box {\n  width: 100%;\n  float: left;\n  box-sizing: border-box;\n  border-bottom: 1px solid #C8C8C8;\n}\n.roles-list-container .roleusers-form .header .add-box .send-email-box {\n  width: 100%;\n  float: left;\n}\n.roles-list-container .roleusers-form .header .add-box .send-email-box .dnn-checkbox-container {\n  float: left;\n  margin-top: 10px;\n  margin-right: 10px;\n}\n.roles-list-container .roleusers-form .header .add-box .send-email-box .dnn-checkbox-container .checkbox {\n  margin-bottom: 0px;\n}\n.roles-list-container .roleusers-form .header .add-box .send-email-box label {\n  color: #C8C8C8;\n}\n.roles-list-container .roleusers-form .header .add-box span {\n  float: left;\n  width: 100%;\n  display: inline-block;\n  border-left: 1px solid #C8C8C8;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox {\n  float: left;\n  width: 250px;\n  height: 34px;\n  vertical-align: middle;\n  padding: 0;\n  border: none;\n  color: #46292B;\n  border-radius: 0;\n  display: inline-block;\n  box-shadow: none;\n  background: transparent;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox button {\n  display: none;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox ::-webkit-input-placeholder {\n  /* WebKit, Blink, Edge */\n  color: #C8C8C8;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox :-moz-placeholder {\n  /* Mozilla Firefox 4 to 18 */\n  color: #C8C8C8;\n  opacity: 1;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox ::-moz-placeholder {\n  /* Mozilla Firefox 19+ */\n  color: #C8C8C8;\n  opacity: 1;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox :-ms-input-placeholder {\n  /* Internet Explorer 10-11 */\n  color: #C8C8C8;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox input {\n  -webkit-box-shadow: none;\n  padding: 0 0 0 12px;\n  height: 100%;\n  vertical-align: top;\n  background-color: transparent;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox input::-ms-clear {\n  display: none;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox .rw-widget-picker.rw-widget-container {\n  height: 32px;\n  line-height: 32px;\n  border: none;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox .rw-widget-picker.rw-widget-container input {\n  height: inherit;\n  line-height: inherit;\n  border: inherit;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox .rw-popup-container {\n  box-shadow: none;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox .rw-popup-container .rw-popup {\n  border-radius: 0px;\n  border: 1px solid #C8C8C8;\n  box-shadow: none;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox .rw-popup-container .rw-popup ul.rw-list {\n  margin: 0;\n  padding: 0;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox .rw-popup-container .rw-popup ul.rw-list li {\n  padding-left: 15px;\n  height: 30px;\n  line-height: 30px;\n  color: #6F7273;\n  border: none;\n  outline: none;\n  overflow: hidden;\n  text-overflow: ellipsis;\n  white-space: nowrap;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox .rw-popup-container .rw-popup ul.rw-list li:hover {\n  background-color: #EFF0F0;\n  color: #1E88C3;\n}\n.roles-list-container .roleusers-form .header .add-box span .rw-combobox .rw-popup-container .rw-popup ul.rw-list li.rw-state-selected {\n  background-color: transparent;\n  color: #1E88C3;\n}\n.roles-list-container .roleusers-form .header .add-box span .add-user-button {\n  float: right;\n  text-align: right;\n  width: auto;\n  font-weight: bolder;\n  overflow: hidden;\n  cursor: pointer;\n  box-sizing: border-box;\n  padding-top: 10px;\n}\n.roles-list-container .roleusers-form .header .add-box span .add-user-button svg {\n  fill: #6F7273;\n}\n.roles-list-container .roleusers-form .header .add-box span .add-user-button span {\n  border: none;\n}\n.roles-list-container .roleusers-form .header .add-box span button[role=\"secondary\"] {\n  vertical-align: middle;\n  margin: 0 0 0 10px;\n}\n", ""]);



/***/ }),
/* 145 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(146);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(15)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 146 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(14)(false);
// Module
exports.push([module.i, "/* START EVOQ COLOR PALETTE */\n/* END EVOQ COLOR PALETTE */\n/* START ATTENTION COLORS */\n/* END ATTENTION COLORS */\n/* SVG HOVER STATES */\nsvg {\n  fill: #C8C8C8;\n}\nsvg:hover {\n  fill: #6F7273;\n}\nsvg:active {\n  fill: #1E88C3;\n}\n/* END SVG HOVER STATES */\n.role-details-editor {\n  padding: 10px 0;\n  background-color: #FFFFFF;\n  display: block;\n  float: left;\n}\n.role-details-editor .editor-container {\n  float: left;\n  padding: 10px 20px;\n  width: 100%;\n  box-sizing: border-box;\n}\n.role-details-editor .editor-container.right-column {\n  border-left: 1px solid #C8C8C8;\n}\n.role-details-editor .editor-container .title-row {\n  width: 100%;\n  float: left;\n  font-weight: bold;\n  text-transform: uppercase;\n  margin-bottom: 10px;\n}\n.role-details-editor .editor-container .status-row {\n  width: 100%;\n  float: left;\n  margin-bottom: 10px;\n}\n.role-details-editor .editor-container .status-row .left {\n  float: left;\n  width: 70%;\n}\n.role-details-editor .editor-container .status-row .left .dnn-label {\n  float: left;\n}\n.role-details-editor .editor-container .status-row .left .dnn-label label {\n  float: left;\n}\n.role-details-editor .editor-container .status-row .right {\n  float: right;\n  width: 30%;\n  text-align: right;\n}\n.role-details-editor .editor-container .status-row .right .checkbox {\n  float: right;\n}\n.role-details-editor .editor-container .status-row .dnn-switch-container {\n  float: right;\n}\n.role-details-editor .editor-container .editor-row {\n  float: left;\n  width: 100%;\n}\n.role-details-editor .editor-container .editor-row select.full {\n  float: left;\n  width: 100%;\n}\n.role-details-editor .editor-container .editor-row select.two-fifth {\n  float: left;\n  width: 40%;\n}\n.role-details-editor .editor-container .editor-row select.one-fourth {\n  float: left;\n  width: 25%;\n}\n.role-details-editor .editor-container .editor-row .text-section {\n  width: 5%;\n  float: left;\n  text-align: center;\n  vertical-align: middle;\n  margin-top: 9px;\n}\n.role-details-editor .editor-container .editor-row .dnn-single-line-input-with-error,\n.role-details-editor .editor-container .editor-row input {\n  width: 100%;\n}\n.role-details-editor .editor-container .editor-row .dnn-single-line-input-with-error .dnn-label label,\n.role-details-editor .editor-container .editor-row input .dnn-label label {\n  font-weight: 500;\n  cursor: default;\n}\n.role-details-editor .editor-container .editor-row .dnn-single-line-input-with-error.disabled input,\n.role-details-editor .editor-container .editor-row input.disabled input {\n  cursor: default;\n}\n.role-details-editor .editor-container .editor-row.divider {\n  padding-bottom: 20px;\n}\n.role-details-editor .editor-container .editor-row.divider .dnn-uicommon-select,\n.role-details-editor .editor-container .editor-row.divider .dnn-ui-common-single-line-input {\n  border-radius: 0px;\n}\n.role-details-editor .editor-container .editor-row.divider .dnn-ui-common-multi-line-input {\n  padding: 8px 16px 62px;\n  border-radius: 0px;\n  min-height: 106px;\n  margin-bottom: 5px;\n}\n.role-details-editor .editor-container .editor-row.divider .new-group-container {\n  position: absolute;\n  z-index: 999;\n}\n.role-details-editor .editor-container .editor-row.divider .dnn-label {\n  float: left;\n}\n.role-details-editor .editor-container .editor-row.divider .dnn-label label {\n  float: left;\n}\n.role-details-editor .buttons-box {\n  width: 100%;\n  text-align: center;\n  float: left;\n  margin: 15px 0;\n}\n.role-details-editor .buttons-box .dnn-ui-common-button {\n  margin: 5px;\n}\n.role-details-editor .buttons-box .edit-icon {\n  margin: 0px 10px 20px 10px;\n  float: right;\n}\n.role-details-editor .buttons-box .edit-icon svg {\n  width: 16px;\n  float: left;\n  height: 16px;\n}\n", ""]);



/***/ }),
/* 147 */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(148);

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(15)(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),
/* 148 */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(14)(false);
// Module
exports.push([module.i, "", ""]);



/***/ })
/******/ ]);
//# sourceMappingURL=roles-bundle.js.map