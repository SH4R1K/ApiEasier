'use strict';

function _typeof(o) { "@babel/helpers - typeof"; return _typeof = "function" == typeof Symbol && "symbol" == typeof Symbol.iterator ? function (o) { return typeof o; } : function (o) { return o && "function" == typeof Symbol && o.constructor === Symbol && o !== Symbol.prototype ? "symbol" : typeof o; }, _typeof(o); }
function _classCallCheck(a, n) { if (!(a instanceof n)) throw new TypeError("Cannot call a class as a function"); }
function _defineProperties(e, r) { for (var t = 0; t < r.length; t++) { var o = r[t]; o.enumerable = o.enumerable || !1, o.configurable = !0, "value" in o && (o.writable = !0), Object.defineProperty(e, _toPropertyKey(o.key), o); } }
function _createClass(e, r, t) { return r && _defineProperties(e.prototype, r), t && _defineProperties(e, t), Object.defineProperty(e, "prototype", { writable: !1 }), e; }
function _toPropertyKey(t) { var i = _toPrimitive(t, "string"); return "symbol" == _typeof(i) ? i : i + ""; }
function _toPrimitive(t, r) { if ("object" != _typeof(t) || !t) return t; var e = t[Symbol.toPrimitive]; if (void 0 !== e) { var i = e.call(t, r || "default"); if ("object" != _typeof(i)) return i; throw new TypeError("@@toPrimitive must return a primitive value."); } return ("string" === r ? String : Number)(t); }
function _callSuper(t, o, e) { return o = _getPrototypeOf(o), _possibleConstructorReturn(t, _isNativeReflectConstruct() ? Reflect.construct(o, e || [], _getPrototypeOf(t).constructor) : o.apply(t, e)); }
function _possibleConstructorReturn(t, e) { if (e && ("object" == _typeof(e) || "function" == typeof e)) return e; if (void 0 !== e) throw new TypeError("Derived constructors may only return object or undefined"); return _assertThisInitialized(t); }
function _assertThisInitialized(e) { if (void 0 === e) throw new ReferenceError("this hasn't been initialised - super() hasn't been called"); return e; }
function _inherits(t, e) { if ("function" != typeof e && null !== e) throw new TypeError("Super expression must either be null or a function"); t.prototype = Object.create(e && e.prototype, { constructor: { value: t, writable: !0, configurable: !0 } }), Object.defineProperty(t, "prototype", { writable: !1 }), e && _setPrototypeOf(t, e); }
function _wrapNativeSuper(t) { var r = "function" == typeof Map ? new Map() : void 0; return _wrapNativeSuper = function _wrapNativeSuper(t) { if (null === t || !_isNativeFunction(t)) return t; if ("function" != typeof t) throw new TypeError("Super expression must either be null or a function"); if (void 0 !== r) { if (r.has(t)) return r.get(t); r.set(t, Wrapper); } function Wrapper() { return _construct(t, arguments, _getPrototypeOf(this).constructor); } return Wrapper.prototype = Object.create(t.prototype, { constructor: { value: Wrapper, enumerable: !1, writable: !0, configurable: !0 } }), _setPrototypeOf(Wrapper, t); }, _wrapNativeSuper(t); }
function _construct(t, e, r) { if (_isNativeReflectConstruct()) return Reflect.construct.apply(null, arguments); var o = [null]; o.push.apply(o, e); var p = new (t.bind.apply(t, o))(); return r && _setPrototypeOf(p, r.prototype), p; }
function _isNativeReflectConstruct() { try { var t = !Boolean.prototype.valueOf.call(Reflect.construct(Boolean, [], function () {})); } catch (t) {} return (_isNativeReflectConstruct = function _isNativeReflectConstruct() { return !!t; })(); }
function _isNativeFunction(t) { try { return -1 !== Function.toString.call(t).indexOf("[native code]"); } catch (n) { return "function" == typeof t; } }
function _setPrototypeOf(t, e) { return _setPrototypeOf = Object.setPrototypeOf ? Object.setPrototypeOf.bind() : function (t, e) { return t.__proto__ = e, t; }, _setPrototypeOf(t, e); }
function _getPrototypeOf(t) { return _getPrototypeOf = Object.setPrototypeOf ? Object.getPrototypeOf.bind() : function (t) { return t.__proto__ || Object.getPrototypeOf(t); }, _getPrototypeOf(t); }
customElements.define('compodoc-menu', /*#__PURE__*/function (_HTMLElement) {
  function _class() {
    var _this;
    _classCallCheck(this, _class);
    _this = _callSuper(this, _class);
    _this.isNormalMode = _this.getAttribute('mode') === 'normal';
    return _this;
  }
  _inherits(_class, _HTMLElement);
  return _createClass(_class, [{
    key: "connectedCallback",
    value: function connectedCallback() {
      this.render(this.isNormalMode);
    }
  }, {
    key: "render",
    value: function render(isNormalMode) {
      var tp = lithtml.html("\n        <nav>\n            <ul class=\"list\">\n                <li class=\"title\">\n                    <a href=\"index.html\" data-type=\"index-link\">ApiEasier documentation</a>\n                </li>\n\n                <li class=\"divider\"></li>\n                ".concat(isNormalMode ? "<div id=\"book-search-input\" role=\"search\"><input type=\"text\" placeholder=\"Type to search\"></div>" : '', "\n                <li class=\"chapter\">\n                    <a data-type=\"chapter-link\" href=\"index.html\"><span class=\"icon ion-ios-home\"></span>Getting started</a>\n                    <ul class=\"links\">\n                        <li class=\"link\">\n                            <a href=\"overview.html\" data-type=\"chapter-link\">\n                                <span class=\"icon ion-ios-keypad\"></span>Overview\n                            </a>\n                        </li>\n                        <li class=\"link\">\n                            <a href=\"index.html\" data-type=\"chapter-link\">\n                                <span class=\"icon ion-ios-paper\"></span>README\n                            </a>\n                        </li>\n                                <li class=\"link\">\n                                    <a href=\"dependencies.html\" data-type=\"chapter-link\">\n                                        <span class=\"icon ion-ios-list\"></span>Dependencies\n                                    </a>\n                                </li>\n                                <li class=\"link\">\n                                    <a href=\"properties.html\" data-type=\"chapter-link\">\n                                        <span class=\"icon ion-ios-apps\"></span>Properties\n                                    </a>\n                                </li>\n                    </ul>\n                </li>\n                    <li class=\"chapter\">\n                        <div class=\"simple menu-toggler\" data-bs-toggle=\"collapse\" ").concat(isNormalMode ? 'data-bs-target="#components-links"' : 'data-bs-target="#xs-components-links"', ">\n                            <span class=\"icon ion-md-cog\"></span>\n                            <span>Components</span>\n                            <span class=\"icon ion-ios-arrow-down\"></span>\n                        </div>\n                        <ul class=\"links collapse \" ").concat(isNormalMode ? 'id="components-links"' : 'id="xs-components-links"', ">\n                            <li class=\"link\">\n                                <a href=\"components/AlertDeleteComponent.html\" data-type=\"entity-link\" >AlertDeleteComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/ApiDialogComponent.html\" data-type=\"entity-link\" >ApiDialogComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/ApiEndpointListComponent.html\" data-type=\"entity-link\" >ApiEndpointListComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/AppComponent.html\" data-type=\"entity-link\" >AppComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/BackButtonComponent.html\" data-type=\"entity-link\" >BackButtonComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/CardApiComponent.html\" data-type=\"entity-link\" >CardApiComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/CardApiListComponent.html\" data-type=\"entity-link\" >CardApiListComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/CardEndpointComponent.html\" data-type=\"entity-link\" >CardEndpointComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/CardEntityComponent.html\" data-type=\"entity-link\" >CardEntityComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/EndpointCardListComponent.html\" data-type=\"entity-link\" >EndpointCardListComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/EndpointDialogComponent.html\" data-type=\"entity-link\" >EndpointDialogComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/EntityCardListComponent.html\" data-type=\"entity-link\" >EntityCardListComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/EntityDialogComponent.html\" data-type=\"entity-link\" >EntityDialogComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/ErrorDisplayComponent.html\" data-type=\"entity-link\" >ErrorDisplayComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/ExportApiButtonComponent.html\" data-type=\"entity-link\" >ExportApiButtonComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/FilterByInputComponent.html\" data-type=\"entity-link\" >FilterByInputComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/HeaderComponent.html\" data-type=\"entity-link\" >HeaderComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/IconTrashComponent.html\" data-type=\"entity-link\" >IconTrashComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/ImportDialogComponent.html\" data-type=\"entity-link\" >ImportDialogComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/LoadingComponent.html\" data-type=\"entity-link\" >LoadingComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/PageNotFoundComponent.html\" data-type=\"entity-link\" >PageNotFoundComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/PaginationComponent.html\" data-type=\"entity-link\" >PaginationComponent</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"components/SwitchComponent.html\" data-type=\"entity-link\" >SwitchComponent</a>\n                            </li>\n                        </ul>\n                    </li>\n                        <li class=\"chapter\">\n                            <div class=\"simple menu-toggler\" data-bs-toggle=\"collapse\" ").concat(isNormalMode ? 'data-bs-target="#injectables-links"' : 'data-bs-target="#xs-injectables-links"', ">\n                                <span class=\"icon ion-md-arrow-round-down\"></span>\n                                <span>Injectables</span>\n                                <span class=\"icon ion-ios-arrow-down\"></span>\n                            </div>\n                            <ul class=\"links collapse \" ").concat(isNormalMode ? 'id="injectables-links"' : 'id="xs-injectables-links"', ">\n                                <li class=\"link\">\n                                    <a href=\"injectables/ApiHubServiceService.html\" data-type=\"entity-link\" >ApiHubServiceService</a>\n                                </li>\n                                <li class=\"link\">\n                                    <a href=\"injectables/ApiService.html\" data-type=\"entity-link\" >ApiService</a>\n                                </li>\n                                <li class=\"link\">\n                                    <a href=\"injectables/ApiServiceRepositoryService.html\" data-type=\"entity-link\" >ApiServiceRepositoryService</a>\n                                </li>\n                                <li class=\"link\">\n                                    <a href=\"injectables/EndpointRepositoryService.html\" data-type=\"entity-link\" >EndpointRepositoryService</a>\n                                </li>\n                                <li class=\"link\">\n                                    <a href=\"injectables/EndpointService.html\" data-type=\"entity-link\" >EndpointService</a>\n                                </li>\n                                <li class=\"link\">\n                                    <a href=\"injectables/EntityRepositoryService.html\" data-type=\"entity-link\" >EntityRepositoryService</a>\n                                </li>\n                                <li class=\"link\">\n                                    <a href=\"injectables/EntityService.html\" data-type=\"entity-link\" >EntityService</a>\n                                </li>\n                                <li class=\"link\">\n                                    <a href=\"injectables/ErrorHandlerService.html\" data-type=\"entity-link\" >ErrorHandlerService</a>\n                                </li>\n                            </ul>\n                        </li>\n                    <li class=\"chapter\">\n                        <div class=\"simple menu-toggler\" data-bs-toggle=\"collapse\" ").concat(isNormalMode ? 'data-bs-target="#interceptors-links"' : 'data-bs-target="#xs-interceptors-links"', ">\n                            <span class=\"icon ion-ios-swap\"></span>\n                            <span>Interceptors</span>\n                            <span class=\"icon ion-ios-arrow-down\"></span>\n                        </div>\n                        <ul class=\"links collapse \" ").concat(isNormalMode ? 'id="interceptors-links"' : 'id="xs-interceptors-links"', ">\n                            <li class=\"link\">\n                                <a href=\"interceptors/HttpErrorInterceptor.html\" data-type=\"entity-link\" >HttpErrorInterceptor</a>\n                            </li>\n                        </ul>\n                    </li>\n                    <li class=\"chapter\">\n                        <div class=\"simple menu-toggler\" data-bs-toggle=\"collapse\" ").concat(isNormalMode ? 'data-bs-target="#interfaces-links"' : 'data-bs-target="#xs-interfaces-links"', ">\n                            <span class=\"icon ion-md-information-circle-outline\"></span>\n                            <span>Interfaces</span>\n                            <span class=\"icon ion-ios-arrow-down\"></span>\n                        </div>\n                        <ul class=\"links collapse \" ").concat(isNormalMode ? ' id="interfaces-links"' : 'id="xs-interfaces-links"', ">\n                            <li class=\"link\">\n                                <a href=\"interfaces/apiServiceShortStructure.html\" data-type=\"entity-link\" >apiServiceShortStructure</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"interfaces/ApiServiceStructure.html\" data-type=\"entity-link\" >ApiServiceStructure</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"interfaces/Endpoint.html\" data-type=\"entity-link\" >Endpoint</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"interfaces/Entity.html\" data-type=\"entity-link\" >Entity</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"interfaces/EntityShort.html\" data-type=\"entity-link\" >EntityShort</a>\n                            </li>\n                            <li class=\"link\">\n                                <a href=\"interfaces/FileStatus.html\" data-type=\"entity-link\" >FileStatus</a>\n                            </li>\n                        </ul>\n                    </li>\n                    <li class=\"chapter\">\n                        <div class=\"simple menu-toggler\" data-bs-toggle=\"collapse\" ").concat(isNormalMode ? 'data-bs-target="#miscellaneous-links"' : 'data-bs-target="#xs-miscellaneous-links"', ">\n                            <span class=\"icon ion-ios-cube\"></span>\n                            <span>Miscellaneous</span>\n                            <span class=\"icon ion-ios-arrow-down\"></span>\n                        </div>\n                        <ul class=\"links collapse \" ").concat(isNormalMode ? 'id="miscellaneous-links"' : 'id="xs-miscellaneous-links"', ">\n                            <li class=\"link\">\n                                <a href=\"miscellaneous/variables.html\" data-type=\"entity-link\">Variables</a>\n                            </li>\n                        </ul>\n                    </li>\n                    <li class=\"chapter\">\n                        <a data-type=\"chapter-link\" href=\"coverage.html\"><span class=\"icon ion-ios-stats\"></span>Documentation coverage</a>\n                    </li>\n                    <li class=\"divider\"></li>\n                    <li class=\"copyright\">\n                        Documentation generated using <a href=\"https://compodoc.app/\" target=\"_blank\" rel=\"noopener noreferrer\">\n                            <img data-src=\"images/compodoc-vectorise.png\" class=\"img-responsive\" data-type=\"compodoc-logo\">\n                        </a>\n                    </li>\n            </ul>\n        </nav>\n        "));
      this.innerHTML = tp.strings;
    }
  }]);
}(/*#__PURE__*/_wrapNativeSuper(HTMLElement)));