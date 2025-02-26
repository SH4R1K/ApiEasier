'use strict';

customElements.define('compodoc-menu', class extends HTMLElement {
    constructor() {
        super();
        this.isNormalMode = this.getAttribute('mode') === 'normal';
    }

    connectedCallback() {
        this.render(this.isNormalMode);
    }

    render(isNormalMode) {
        let tp = lithtml.html(`
        <nav>
            <ul class="list">
                <li class="title">
                    <a href="index.html" data-type="index-link">ApiEasier documentation</a>
                </li>

                <li class="divider"></li>
                ${ isNormalMode ? `<div id="book-search-input" role="search"><input type="text" placeholder="Type to search"></div>` : '' }
                <li class="chapter">
                    <a data-type="chapter-link" href="index.html"><span class="icon ion-ios-home"></span>Getting started</a>
                    <ul class="links">
                        <li class="link">
                            <a href="overview.html" data-type="chapter-link">
                                <span class="icon ion-ios-keypad"></span>Overview
                            </a>
                        </li>
                        <li class="link">
                            <a href="index.html" data-type="chapter-link">
                                <span class="icon ion-ios-paper"></span>README
                            </a>
                        </li>
                                <li class="link">
                                    <a href="dependencies.html" data-type="chapter-link">
                                        <span class="icon ion-ios-list"></span>Dependencies
                                    </a>
                                </li>
                                <li class="link">
                                    <a href="properties.html" data-type="chapter-link">
                                        <span class="icon ion-ios-apps"></span>Properties
                                    </a>
                                </li>
                    </ul>
                </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#components-links"' :
                            'data-bs-target="#xs-components-links"' }>
                            <span class="icon ion-md-cog"></span>
                            <span>Components</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="components-links"' : 'id="xs-components-links"' }>
                            <li class="link">
                                <a href="components/AlertDeleteComponent.html" data-type="entity-link" >AlertDeleteComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ApiDialogComponent.html" data-type="entity-link" >ApiDialogComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ApiEndpointListComponent.html" data-type="entity-link" >ApiEndpointListComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/AppComponent.html" data-type="entity-link" >AppComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/BackButtonComponent.html" data-type="entity-link" >BackButtonComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/CardApiComponent.html" data-type="entity-link" >CardApiComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/CardApiListComponent.html" data-type="entity-link" >CardApiListComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/CardEndpointComponent.html" data-type="entity-link" >CardEndpointComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/CardEntityComponent.html" data-type="entity-link" >CardEntityComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/EndpointCardListComponent.html" data-type="entity-link" >EndpointCardListComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/EndpointDialogComponent.html" data-type="entity-link" >EndpointDialogComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/EntityCardListComponent.html" data-type="entity-link" >EntityCardListComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/EntityDialogComponent.html" data-type="entity-link" >EntityDialogComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ErrorDisplayComponent.html" data-type="entity-link" >ErrorDisplayComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ExportApiButtonComponent.html" data-type="entity-link" >ExportApiButtonComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/FilterByInputComponent.html" data-type="entity-link" >FilterByInputComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/HeaderComponent.html" data-type="entity-link" >HeaderComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/IconTrashComponent.html" data-type="entity-link" >IconTrashComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/ImportDialogComponent.html" data-type="entity-link" >ImportDialogComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/LoadingComponent.html" data-type="entity-link" >LoadingComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/PageNotFoundComponent.html" data-type="entity-link" >PageNotFoundComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/PaginationComponent.html" data-type="entity-link" >PaginationComponent</a>
                            </li>
                            <li class="link">
                                <a href="components/SwitchComponent.html" data-type="entity-link" >SwitchComponent</a>
                            </li>
                        </ul>
                    </li>
                        <li class="chapter">
                            <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#injectables-links"' :
                                'data-bs-target="#xs-injectables-links"' }>
                                <span class="icon ion-md-arrow-round-down"></span>
                                <span>Injectables</span>
                                <span class="icon ion-ios-arrow-down"></span>
                            </div>
                            <ul class="links collapse " ${ isNormalMode ? 'id="injectables-links"' : 'id="xs-injectables-links"' }>
                                <li class="link">
                                    <a href="injectables/ApiHubServiceService.html" data-type="entity-link" >ApiHubServiceService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ApiService.html" data-type="entity-link" >ApiService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ApiServiceRepositoryService.html" data-type="entity-link" >ApiServiceRepositoryService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EndpointRepositoryService.html" data-type="entity-link" >EndpointRepositoryService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EndpointService.html" data-type="entity-link" >EndpointService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EntityRepositoryService.html" data-type="entity-link" >EntityRepositoryService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/EntityService.html" data-type="entity-link" >EntityService</a>
                                </li>
                                <li class="link">
                                    <a href="injectables/ErrorHandlerService.html" data-type="entity-link" >ErrorHandlerService</a>
                                </li>
                            </ul>
                        </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#interceptors-links"' :
                            'data-bs-target="#xs-interceptors-links"' }>
                            <span class="icon ion-ios-swap"></span>
                            <span>Interceptors</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="interceptors-links"' : 'id="xs-interceptors-links"' }>
                            <li class="link">
                                <a href="interceptors/HttpErrorInterceptor.html" data-type="entity-link" >HttpErrorInterceptor</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#interfaces-links"' :
                            'data-bs-target="#xs-interfaces-links"' }>
                            <span class="icon ion-md-information-circle-outline"></span>
                            <span>Interfaces</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? ' id="interfaces-links"' : 'id="xs-interfaces-links"' }>
                            <li class="link">
                                <a href="interfaces/apiServiceShortStructure.html" data-type="entity-link" >apiServiceShortStructure</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/ApiServiceStructure.html" data-type="entity-link" >ApiServiceStructure</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Endpoint.html" data-type="entity-link" >Endpoint</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/Entity.html" data-type="entity-link" >Entity</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/EntityShort.html" data-type="entity-link" >EntityShort</a>
                            </li>
                            <li class="link">
                                <a href="interfaces/FileStatus.html" data-type="entity-link" >FileStatus</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <div class="simple menu-toggler" data-bs-toggle="collapse" ${ isNormalMode ? 'data-bs-target="#miscellaneous-links"'
                            : 'data-bs-target="#xs-miscellaneous-links"' }>
                            <span class="icon ion-ios-cube"></span>
                            <span>Miscellaneous</span>
                            <span class="icon ion-ios-arrow-down"></span>
                        </div>
                        <ul class="links collapse " ${ isNormalMode ? 'id="miscellaneous-links"' : 'id="xs-miscellaneous-links"' }>
                            <li class="link">
                                <a href="miscellaneous/variables.html" data-type="entity-link">Variables</a>
                            </li>
                        </ul>
                    </li>
                    <li class="chapter">
                        <a data-type="chapter-link" href="coverage.html"><span class="icon ion-ios-stats"></span>Documentation coverage</a>
                    </li>
                    <li class="divider"></li>
                    <li class="copyright">
                        Documentation generated using <a href="https://compodoc.app/" target="_blank" rel="noopener noreferrer">
                            <img data-src="images/compodoc-vectorise.png" class="img-responsive" data-type="compodoc-logo">
                        </a>
                    </li>
            </ul>
        </nav>
        `);
        this.innerHTML = tp.strings;
    }
});