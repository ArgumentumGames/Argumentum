import{$r as tS,$t as Zp,At as Td,B as JT,Ci as xd,D as Fy,Dn as ev,Dt as Sn,En as er,Er as p_,Fn as gS,Gn as ia,Gt as YT,Hi as x,Jt as Z,Kn as ii,L as Iy,Li as zt$1,Ln as g_,Mi as ye,N as I,Nr as qr,O as Gy,On as fS,Q as Ly,Rr as rS,S as EM,Sn as de,St as SC,Tn as e_,Ut as XT,V as Jb,Vi as w,Wn as i_,Y as Kt,Yr as st,Zn as jt$1,Zr as sy,Zt as Zd,_n as cr,bi as wy,cr as m_,di as v_,en as Zs,g as Cy,gt as QT,hi as wE,hr as nn,ir as m,jt as Te,k as H,kn as f_,li as vM,lt as Ng,m as CM,mn as cS,ni as uo,nr as l_,o as $y,ot as N,pr as nC,qn as ip,qt as Yp,r as $_,rn as _a,rt as Md,sn as _y,st as NE,tr as lS,ui as vU,un as bE,vi as wc,vn as cy,vt as Qf,xt as Ry,yr as oS,zr as rf}from"./chunk-DtJSyZhi.js";import{J as hn$1,L as ar,U as ct,Y as ht$1,dt as un,ft as ve$1,h as H$1,ht as y,j as Xe,lt as rl,nt as le,rt as nn$1,v as J}from"./chunk-D18RyqxO.js";import{a as Rt$1,c as ss,d as yt$1,o as W,r as F,s as rs,t as $t$1,u as wt$1}from"./main-NXRXUN76.js";import{r as rp}from"./chunk-BRhjinLo.js";import{t as B}from"./chunk-CeJolC2W.js";var Y=[`*`];var vt=[`content`];var ut=[[[`mat-drawer`],[`mat-sidenav`]],[[`mat-drawer-content`],[`mat-sidenav-content`]],`*`];var ht=[`mat-drawer, mat-sidenav`,`mat-drawer-content, mat-sidenav-content`,`*`];function wt(r,s){if(r&1){let e=i_();ia(0,`div`,1),Ry(`click`,function(){Zp(e);return Yp(l_()._onBackdropClicked())}),Td()}if(r&2)Gy(`mat-drawer-shown`,l_()._isShowingBackdrop())}function Ct(r,s){r&1&&(ia(0,`mat-drawer-content`),p_(1,2),Td())}function xt(r,s){if(r&1){let e=i_();ia(0,`div`,1),Ry(`click`,function(){Zp(e);return Yp(l_()._onBackdropClicked())}),Td()}if(r&2)Gy(`mat-drawer-shown`,l_()._isShowingBackdrop())}function yt(r,s){r&1&&(ia(0,`mat-sidenav-content`),p_(1,2),Td())}var St=`.mat-drawer-container {
  position: relative;
  z-index: 1;
  color: var(--%NS%mat-sidenav-content-text-color, var(--%NS%mat-sys-on-background));
  background-color: var(--%NS%mat-sidenav-content-background-color, var(--%NS%mat-sys-background));
  box-sizing: border-box;
  display: block;
  overflow: hidden;
}
.mat-drawer-container[fullscreen] {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
}
.mat-drawer-container[fullscreen].mat-drawer-container-has-open {
  overflow: hidden;
}
.mat-drawer-container.mat-drawer-container-explicit-backdrop .mat-drawer-side {
  z-index: 3;
}
.mat-drawer-container.ng-animate-disabled .mat-drawer-backdrop,
.mat-drawer-container.ng-animate-disabled .mat-drawer-content, .ng-animate-disabled .mat-drawer-container .mat-drawer-backdrop,
.ng-animate-disabled .mat-drawer-container .mat-drawer-content {
  transition: none;
}

.mat-drawer-backdrop {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  display: block;
  z-index: 3;
  visibility: hidden;
}
.mat-drawer-backdrop.mat-drawer-shown {
  visibility: visible;
  background-color: var(--%NS%mat-sidenav-scrim-color, color-mix(in srgb, var(--%NS%mat-sys-neutral-variant20) 40%, transparent));
}
.mat-drawer-transition .mat-drawer-backdrop {
  transition-duration: 400ms;
  transition-timing-function: cubic-bezier(0.25, 0.8, 0.25, 1);
  transition-property: background-color, visibility;
}
@media (forced-colors: active) {
  .mat-drawer-backdrop {
    opacity: 0.5;
  }
}

.mat-drawer-content {
  position: relative;
  z-index: 1;
  display: block;
  height: 100%;
  overflow: auto;
}
.mat-drawer-content.mat-drawer-content-hidden {
  opacity: 0;
}
.mat-drawer-transition .mat-drawer-content {
  transition-duration: 400ms;
  transition-timing-function: cubic-bezier(0.25, 0.8, 0.25, 1);
  transition-property: transform, margin-left, margin-right;
}

.mat-drawer {
  position: relative;
  z-index: 4;
  color: var(--%NS%mat-sidenav-container-text-color, var(--%NS%mat-sys-on-surface-variant));
  box-shadow: var(--%NS%mat-sidenav-container-elevation-shadow, none);
  background-color: var(--%NS%mat-sidenav-container-background-color, var(--%NS%mat-sys-surface));
  border-top-right-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-bottom-right-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  width: var(--%NS%mat-sidenav-container-width, 360px);
  display: block;
  position: absolute;
  top: 0;
  bottom: 0;
  z-index: 3;
  outline: 0;
  box-sizing: border-box;
  overflow-y: auto;
  transform: translate3d(-100%, 0, 0);
}
@media (forced-colors: active) {
  .mat-drawer, [dir=rtl] .mat-drawer.mat-drawer-end {
    border-right: solid 1px currentColor;
  }
}
@media (forced-colors: active) {
  [dir=rtl] .mat-drawer, .mat-drawer.mat-drawer-end {
    border-left: solid 1px currentColor;
    border-right: none;
  }
}
.mat-drawer.mat-drawer-side {
  z-index: 2;
}
.mat-drawer.mat-drawer-end {
  right: 0;
  transform: translate3d(100%, 0, 0);
  border-top-left-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-bottom-left-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
}
[dir=rtl] .mat-drawer {
  border-top-left-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-bottom-left-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
  transform: translate3d(100%, 0, 0);
}
[dir=rtl] .mat-drawer.mat-drawer-end {
  border-top-right-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-bottom-right-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-top-left-radius: 0;
  border-bottom-left-radius: 0;
  left: 0;
  right: auto;
  transform: translate3d(-100%, 0, 0);
}
.mat-drawer-transition .mat-drawer {
  transition: transform 400ms cubic-bezier(0.25, 0.8, 0.25, 1);
}
.mat-drawer:not(.mat-drawer-opened):not(.mat-drawer-animating) {
  visibility: hidden;
  box-shadow: none;
}
.mat-drawer:not(.mat-drawer-opened):not(.mat-drawer-animating) .mat-drawer-inner-container {
  display: none;
}
.mat-drawer.mat-drawer-opened.mat-drawer-opened {
  transform: none;
}

.mat-drawer-side {
  box-shadow: none;
  border-right-color: var(--%NS%mat-sidenav-container-divider-color, transparent);
  border-right-width: 1px;
  border-right-style: solid;
}
.mat-drawer-side.mat-drawer-end {
  border-left-color: var(--%NS%mat-sidenav-container-divider-color, transparent);
  border-left-width: 1px;
  border-left-style: solid;
  border-right: none;
}
[dir=rtl] .mat-drawer-side {
  border-left-color: var(--%NS%mat-sidenav-container-divider-color, transparent);
  border-left-width: 1px;
  border-left-style: solid;
  border-right: none;
}
[dir=rtl] .mat-drawer-side.mat-drawer-end {
  border-right-color: var(--%NS%mat-sidenav-container-divider-color, transparent);
  border-right-width: 1px;
  border-right-style: solid;
  border-left: none;
}

.mat-drawer-inner-container {
  width: 100%;
  height: 100%;
  overflow: auto;
}

.mat-sidenav-fixed {
  position: fixed;
}
`;var kt=new I(`MAT_DRAWER_DEFAULT_AUTOSIZE`,{providedIn:`root`,factory:()=>!1});var Ce=new I(`MAT_DRAWER_CONTAINER`);var j=(()=>{class r extends ht$1{_platform=m(y);_changeDetectorRef=m(Zd);_element=m(zt$1);_ngZone=m(H);_isInert=!1;_container=m(ve);ngAfterContentInit(){this._container._contentMarginChanges.subscribe(()=>this._changeDetectorRef.markForCheck())}_drawerToggled(e){e.opened?this._ngZone.runOutsideAngular(()=>{e._animationEnd.pipe(ip(50),Sn(1)).subscribe(()=>this._updateInert())}):this._updateInert()}_drawerModeChanged(){this._updateInert()}_updateInert(){let e=this._container._isShowingBackdrop();if(e!==this._isInert){let t=this._element.nativeElement;this._isInert=e,e?t.setAttribute(`inert`,`true`):t.removeAttribute(`inert`)}}_shouldBeHidden(){if(this._platform.isBrowser)return!1;let{start:e,end:t}=this._container;return e!=null&&e.mode!==`over`&&e.opened||t!=null&&t.mode!==`over`&&t.opened}static ɵfac=(()=>{let e;return function(n){return(e||(e=Ng(r)))(n||r)}})();static ɵcmp=Jb({type:r,selectors:[[`mat-drawer-content`]],hostAttrs:[1,`mat-drawer-content`],hostVars:6,hostBindings:function(t,n){t&2&&($y(`margin-left`,n._container._contentMargins.left,`px`)(`margin-right`,n._container._contentMargins.right,`px`),Gy(`mat-drawer-content-hidden`,n._shouldBeHidden()))},features:[tS([{provide:ht$1,useExisting:r}]),sy],ngContentSelectors:Y,decls:1,vars:0,template:function(t,n){t&1&&(f_(),p_(0))},encapsulation:2})}return r})();var be=(()=>{class r{_elementRef=m(zt$1);_focusTrapFactory=m(un);_focusMonitor=m(nn$1);_platform=m(y);_ngZone=m(H);_renderer=m(er);_interactivityChecker=m(hn$1);_doc=m(Z);_isAnimating=!1;_container=m(Ce,{optional:!0});_focusTrap=null;_elementFocusedBeforeDrawerWasOpened=null;_eventCleanups;_isAttached=!1;_anchor=null;get position(){return this._position}set position(e){e=e===`end`?`end`:`start`,e!==this._position&&(this._isAttached&&this._updatePositionInParent(e),this._position=e,this.onPositionChanged.emit())}_position=`start`;get mode(){return this._mode}set mode(e){this._mode=e,this._updateFocusTrapState(),this._modeChanged.next(),this._getContent()?._drawerModeChanged()}_mode=`over`;get disableClose(){return this._disableClose}set disableClose(e){this._disableClose=rl(e)}_disableClose=!1;get autoFocus(){return this._autoFocus??(this.mode===`side`?`dialog`:`first-tabbable`)}set autoFocus(e){(e===`true`||e===`false`||e==null)&&(e=rl(e)),this._autoFocus=e}_autoFocus;get opened(){return this._opened()}set opened(e){this.toggle(rl(e))}_opened=st(!1);_openedVia=null;_animationStarted=new de;_animationEnd=new de;openedChange=new jt$1(!0);_openedStream=this.openedChange.pipe(Kt(e=>e),Te(()=>{}));openedStart=this._animationStarted.pipe(Kt(()=>this.opened),wc(void 0));_closedStream=this.openedChange.pipe(Kt(e=>!e),Te(()=>{}));closedStart=this._animationStarted.pipe(Kt(()=>!this.opened),wc(void 0));_destroyed=new de;onPositionChanged=new jt$1;_content;_modeChanged=new de;_injector=m(ye);_changeDetectorRef=m(Zd);constructor(){this.openedChange.pipe(NE(this._destroyed)).subscribe(e=>{e?(this._elementFocusedBeforeDrawerWasOpened=this._doc.activeElement,this._takeFocus()):this._isFocusWithinDrawer()&&this._restoreFocus(this._openedVia||`program`)}),this._eventCleanups=this._ngZone.runOutsideAngular(()=>{let e=this._renderer,t=this._elementRef.nativeElement;return[e.listen(t,`keydown`,n=>{n.keyCode===27&&!this.disableClose&&!Xe(n)&&this._ngZone.run(()=>{this.close(),n.stopPropagation(),n.preventDefault()})}),e.listen(t,`transitionend`,this._handleTransitionEvent),e.listen(t,`transitioncancel`,this._handleTransitionEvent)]}),this._animationEnd.subscribe(()=>{this.openedChange.emit(this.opened)})}_focusByCssSelector(e,t){let n=this._elementRef.nativeElement.querySelector(e);n&&(this._interactivityChecker.isFocusable(n)||(n.tabIndex=-1,this._ngZone.runOutsideAngular(()=>{let i=()=>{a(),F(),n.removeAttribute(`tabindex`)},a=this._renderer.listen(n,`blur`,i),F=this._renderer.listen(n,`mousedown`,i)})),n.focus(t))}_takeFocus(){if(!this._focusTrap)return;let e=this._elementRef.nativeElement;switch(this.autoFocus){case!1:case`dialog`:return;case!0:case`first-tabbable`:nC(()=>{let t=this._isAnimating?{preventScroll:!0}:void 0;!this._focusTrap.focusInitialElement(t)&&typeof e.focus==`function`&&e.focus(t)},{injector:this._injector});break;case`first-heading`:this._focusByCssSelector(`h1, h2, h3, h4, h5, h6, [role="heading"]`);break;default:this._focusByCssSelector(this.autoFocus);break}}_restoreFocus(e){this.autoFocus!==`dialog`&&(this._elementFocusedBeforeDrawerWasOpened?this._focusMonitor.focusVia(this._elementFocusedBeforeDrawerWasOpened,e):this._elementRef.nativeElement.blur(),this._elementFocusedBeforeDrawerWasOpened=null)}_isFocusWithinDrawer(){let e=this._doc.activeElement;return!!e&&this._elementRef.nativeElement.contains(e)}ngAfterViewInit(){this._isAttached=!0,this._position===`end`&&this._updatePositionInParent(`end`),this._platform.isBrowser&&(this._focusTrap=this._focusTrapFactory.create(this._elementRef.nativeElement),this._updateFocusTrapState())}ngOnDestroy(){this._eventCleanups.forEach(e=>e()),this._focusTrap?.destroy(),this._anchor?.remove(),this._anchor=null,this._animationStarted.complete(),this._animationEnd.complete(),this._modeChanged.complete(),this._destroyed.next(),this._destroyed.complete()}open(e){return this.toggle(!0,e)}close(){return this.toggle(!1)}_closeViaBackdropClick(){return this._setOpen(!1,!0,`mouse`)}toggle(e=!this.opened,t){e&&t&&(this._openedVia=t);let n=this._setOpen(e,!e&&this._isFocusWithinDrawer(),this._openedVia||`program`);return e||(this._openedVia=null),n}_setOpen(e,t,n){return e===this.opened?Promise.resolve(e?`open`:`close`):(this._opened.set(e),this._getContent()?._drawerToggled(this),this._container?._transitionsEnabled?this._isAnimating?(this._setIsAnimating(!1),this._simulateAnimation()):(this._setIsAnimating(!0),setTimeout(()=>this._animationStarted.next())):this._simulateAnimation(),this._elementRef.nativeElement.classList.toggle(`mat-drawer-opened`,e),!e&&t&&this._restoreFocus(n),this._changeDetectorRef.markForCheck(),this._updateFocusTrapState(),new Promise(i=>{this.openedChange.pipe(Sn(1)).subscribe(a=>i(a?`open`:`close`))}))}_getContent(){return this._container?._content||this._container?._userContent}_setIsAnimating(e){e!==this._isAnimating&&(this._isAnimating=e,this._elementRef.nativeElement.classList.toggle(`mat-drawer-animating`,e))}_simulateAnimation(){setTimeout(()=>{this._animationStarted.next(),this._animationEnd.next()})}_getWidth(){return this._elementRef.nativeElement.offsetWidth||0}_updateFocusTrapState(){this._focusTrap&&(this._focusTrap.enabled=this.opened&&!!this._container?._isShowingBackdrop())}_updatePositionInParent(e){if(!this._platform.isBrowser)return;let t=this._elementRef.nativeElement,n=t.parentNode;e===`end`?(this._anchor||(this._anchor=this._doc.createComment(`mat-drawer-anchor`),n.insertBefore(this._anchor,t)),n.appendChild(t)):this._anchor&&this._anchor.parentNode.insertBefore(t,this._anchor)}_handleTransitionEvent=e=>{let t=this._elementRef.nativeElement;e.target===t&&this._ngZone.run(()=>{e.type===`transitionend`&&this._setIsAnimating(!1),this._animationEnd.next(e)})};static ɵfac=function(t){return new(t||r)};static ɵcmp=Jb({type:r,selectors:[[`mat-drawer`]],viewQuery:function(t,n){if(t&1&&Ly(vt,5),t&2){let i;g_(i=m_())&&(n._content=i.first)}},hostAttrs:[1,`mat-drawer`],hostVars:12,hostBindings:function(t,n){t&2&&(Iy(`align`,null)(`tabIndex`,n.mode!==`side`?`-1`:null),$y(`visibility`,!n._container&&!n.opened?`hidden`:null),Gy(`mat-drawer-end`,n.position===`end`)(`mat-drawer-over`,n.mode===`over`)(`mat-drawer-push`,n.mode===`push`)(`mat-drawer-side`,n.mode===`side`))},inputs:{position:`position`,mode:`mode`,disableClose:`disableClose`,autoFocus:`autoFocus`,opened:`opened`},outputs:{openedChange:`openedChange`,_openedStream:`opened`,openedStart:`openedStart`,_closedStream:`closed`,closedStart:`closedStart`,onPositionChanged:`positionChanged`},exportAs:[`matDrawer`],ngContentSelectors:Y,decls:3,vars:0,consts:[[`content`,``],[`cdkScrollable`,``,1,`mat-drawer-inner-container`]],template:function(t,n){t&1&&(f_(),ia(0,`div`,1,0),p_(2),Td())},dependencies:[ht$1],encapsulation:2})}return r})();var ve=(()=>{class r{_dir=m(le,{optional:!0});_element=m(zt$1);_ngZone=m(H);_changeDetectorRef=m(Zd);_animationDisabled=ar();_transitionsEnabled=!1;_allDrawers;_drawers=new Zs;_content;_userContent;get start(){return this._start}get end(){return this._end}get autosize(){return this._autosize}set autosize(e){this._autosize=rl(e)}_autosize=m(kt);get hasBackdrop(){return this._drawerHasBackdrop(this._start)||this._drawerHasBackdrop(this._end)}set hasBackdrop(e){this._backdropOverride=e==null?null:rl(e)}_backdropOverride=null;backdropClick=new jt$1;_start=null;_end=null;_left=null;_right=null;_destroyed=new de;_doCheckSubject=new de;_contentMargins={left:null,right:null};_contentMarginChanges=new de;get scrollable(){return this._userContent||this._content}_injector=m(ye);constructor(){let e=m(y),t=m(J);this._dir?.change.pipe(NE(this._destroyed)).subscribe(()=>{this._validateDrawers(),this.updateContentMargins()}),t.change().pipe(NE(this._destroyed)).subscribe(()=>this.updateContentMargins()),!this._animationDisabled&&e.isBrowser&&this._ngZone.runOutsideAngular(()=>{setTimeout(()=>{this._element.nativeElement.classList.add(`mat-drawer-transition`),this._transitionsEnabled=!0},200)})}ngAfterContentInit(){this._allDrawers.changes.pipe(Qf(this._allDrawers),NE(this._destroyed)).subscribe(e=>{this._drawers.reset(e.filter(t=>!t._container||t._container===this)),this._drawers.notifyOnChanges()}),this._drawers.changes.pipe(Qf(null)).subscribe(()=>{this._validateDrawers(),this._drawers.forEach(e=>{this._watchDrawerToggle(e),this._watchDrawerPosition(e),this._watchDrawerMode(e)}),(!this._drawers.length||this._isDrawerOpen(this._start)||this._isDrawerOpen(this._end))&&this.updateContentMargins(),this._changeDetectorRef.markForCheck()}),this._ngZone.runOutsideAngular(()=>{this._doCheckSubject.pipe(bE(10),NE(this._destroyed)).subscribe(()=>this.updateContentMargins())})}ngOnDestroy(){this._contentMarginChanges.complete(),this._doCheckSubject.complete(),this._drawers.destroy(),this._destroyed.next(),this._destroyed.complete()}open(){this._drawers.forEach(e=>e.open())}close(){this._drawers.forEach(e=>e.close())}updateContentMargins(){let e=0,t=0;if(this._left&&this._left.opened){if(this._left.mode==`side`)e+=this._left._getWidth();else if(this._left.mode==`push`){let n=this._left._getWidth();e+=n,t-=n}}if(this._right&&this._right.opened){if(this._right.mode==`side`)t+=this._right._getWidth();else if(this._right.mode==`push`){let n=this._right._getWidth();t+=n,e-=n}}e=e||null,t=t||null,(e!==this._contentMargins.left||t!==this._contentMargins.right)&&(this._contentMargins={left:e,right:t},this._ngZone.run(()=>this._contentMarginChanges.next(this._contentMargins)))}ngDoCheck(){this._autosize&&this._isPushed()&&this._ngZone.runOutsideAngular(()=>this._doCheckSubject.next())}_watchDrawerToggle(e){e._animationStarted.pipe(NE(this._drawers.changes)).subscribe(()=>{this.updateContentMargins(),this._changeDetectorRef.markForCheck()}),e.mode!==`side`&&e.openedChange.pipe(NE(this._drawers.changes)).subscribe(()=>this._setContainerClass(e.opened))}_watchDrawerPosition(e){e.onPositionChanged.pipe(NE(this._drawers.changes)).subscribe(()=>{nC({read:()=>this._validateDrawers()},{injector:this._injector})})}_watchDrawerMode(e){e._modeChanged.pipe(NE(wE(this._drawers.changes,this._destroyed))).subscribe(()=>{this.updateContentMargins(),this._changeDetectorRef.markForCheck()})}_setContainerClass(e){let t=this._element.nativeElement.classList,n=`mat-drawer-container-has-open`;e?t.add(n):t.remove(n)}_validateDrawers(){this._start=this._end=null,this._drawers.forEach(e=>{e.position==`end`?(this._end,this._end=e):(this._start,this._start=e)}),this._right=this._left=null,this._dir&&this._dir.value===`rtl`?(this._left=this._end,this._right=this._start):(this._left=this._start,this._right=this._end)}_isPushed(){return this._isDrawerOpen(this._start)&&this._start.mode!=`over`||this._isDrawerOpen(this._end)&&this._end.mode!=`over`}_onBackdropClicked(){this.backdropClick.emit(),this._closeModalDrawersViaBackdrop()}_closeModalDrawersViaBackdrop(){[this._start,this._end].filter(e=>e&&!e.disableClose&&this._drawerHasBackdrop(e)).forEach(e=>e._closeViaBackdropClick())}_isShowingBackdrop(){return this._isDrawerOpen(this._start)&&this._drawerHasBackdrop(this._start)||this._isDrawerOpen(this._end)&&this._drawerHasBackdrop(this._end)}_isDrawerOpen(e){return e!=null&&e.opened}_drawerHasBackdrop(e){return this._backdropOverride==null?!!e&&e.mode!==`side`:this._backdropOverride}static ɵfac=function(t){return new(t||r)};static ɵcmp=Jb({type:r,selectors:[[`mat-drawer-container`]],contentQueries:function(t,n,i){if(t&1&&Fy(i,j,5)(i,be,5),t&2){let a;g_(a=m_())&&(n._content=a.first),g_(a=m_())&&(n._allDrawers=a)}},viewQuery:function(t,n){if(t&1&&Ly(j,5),t&2){let i;g_(i=m_())&&(n._userContent=i.first)}},hostAttrs:[1,`mat-drawer-container`],hostVars:2,hostBindings:function(t,n){t&2&&Gy(`mat-drawer-container-explicit-backdrop`,n._backdropOverride)},inputs:{autosize:`autosize`,hasBackdrop:`hasBackdrop`},outputs:{backdropClick:`backdropClick`},exportAs:[`matDrawerContainer`],features:[tS([{provide:Ce,useExisting:r}])],ngContentSelectors:ht,decls:4,vars:2,consts:[[1,`mat-drawer-backdrop`,3,`mat-drawer-shown`],[1,`mat-drawer-backdrop`,3,`click`]],template:function(t,n){t&1&&(f_(ut),YT(0,wt,1,2,`div`,0),p_(1),p_(2,1),YT(3,Ct,2,0,`mat-drawer-content`)),t&2&&(QT(n.hasBackdrop?0:-1),SC(3),QT(n._content?-1:3))},dependencies:[j],styles:[`.mat-drawer-container {
  position: relative;
  z-index: 1;
  color: var(--%NS%mat-sidenav-content-text-color, var(--%NS%mat-sys-on-background));
  background-color: var(--%NS%mat-sidenav-content-background-color, var(--%NS%mat-sys-background));
  box-sizing: border-box;
  display: block;
  overflow: hidden;
}
.mat-drawer-container[fullscreen] {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
}
.mat-drawer-container[fullscreen].mat-drawer-container-has-open {
  overflow: hidden;
}
.mat-drawer-container.mat-drawer-container-explicit-backdrop .mat-drawer-side {
  z-index: 3;
}
.mat-drawer-container.ng-animate-disabled .mat-drawer-backdrop,
.mat-drawer-container.ng-animate-disabled .mat-drawer-content, .ng-animate-disabled .mat-drawer-container .mat-drawer-backdrop,
.ng-animate-disabled .mat-drawer-container .mat-drawer-content {
  transition: none;
}

.mat-drawer-backdrop {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  display: block;
  z-index: 3;
  visibility: hidden;
}
.mat-drawer-backdrop.mat-drawer-shown {
  visibility: visible;
  background-color: var(--%NS%mat-sidenav-scrim-color, color-mix(in srgb, var(--%NS%mat-sys-neutral-variant20) 40%, transparent));
}
.mat-drawer-transition .mat-drawer-backdrop {
  transition-duration: 400ms;
  transition-timing-function: cubic-bezier(0.25, 0.8, 0.25, 1);
  transition-property: background-color, visibility;
}
@media (forced-colors: active) {
  .mat-drawer-backdrop {
    opacity: 0.5;
  }
}

.mat-drawer-content {
  position: relative;
  z-index: 1;
  display: block;
  height: 100%;
  overflow: auto;
}
.mat-drawer-content.mat-drawer-content-hidden {
  opacity: 0;
}
.mat-drawer-transition .mat-drawer-content {
  transition-duration: 400ms;
  transition-timing-function: cubic-bezier(0.25, 0.8, 0.25, 1);
  transition-property: transform, margin-left, margin-right;
}

.mat-drawer {
  position: relative;
  z-index: 4;
  color: var(--%NS%mat-sidenav-container-text-color, var(--%NS%mat-sys-on-surface-variant));
  box-shadow: var(--%NS%mat-sidenav-container-elevation-shadow, none);
  background-color: var(--%NS%mat-sidenav-container-background-color, var(--%NS%mat-sys-surface));
  border-top-right-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-bottom-right-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  width: var(--%NS%mat-sidenav-container-width, 360px);
  display: block;
  position: absolute;
  top: 0;
  bottom: 0;
  z-index: 3;
  outline: 0;
  box-sizing: border-box;
  overflow-y: auto;
  transform: translate3d(-100%, 0, 0);
}
@media (forced-colors: active) {
  .mat-drawer, [dir=rtl] .mat-drawer.mat-drawer-end {
    border-right: solid 1px currentColor;
  }
}
@media (forced-colors: active) {
  [dir=rtl] .mat-drawer, .mat-drawer.mat-drawer-end {
    border-left: solid 1px currentColor;
    border-right: none;
  }
}
.mat-drawer.mat-drawer-side {
  z-index: 2;
}
.mat-drawer.mat-drawer-end {
  right: 0;
  transform: translate3d(100%, 0, 0);
  border-top-left-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-bottom-left-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
}
[dir=rtl] .mat-drawer {
  border-top-left-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-bottom-left-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-top-right-radius: 0;
  border-bottom-right-radius: 0;
  transform: translate3d(100%, 0, 0);
}
[dir=rtl] .mat-drawer.mat-drawer-end {
  border-top-right-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-bottom-right-radius: var(--%NS%mat-sidenav-container-shape, var(--%NS%mat-sys-corner-large));
  border-top-left-radius: 0;
  border-bottom-left-radius: 0;
  left: 0;
  right: auto;
  transform: translate3d(-100%, 0, 0);
}
.mat-drawer-transition .mat-drawer {
  transition: transform 400ms cubic-bezier(0.25, 0.8, 0.25, 1);
}
.mat-drawer:not(.mat-drawer-opened):not(.mat-drawer-animating) {
  visibility: hidden;
  box-shadow: none;
}
.mat-drawer:not(.mat-drawer-opened):not(.mat-drawer-animating) .mat-drawer-inner-container {
  display: none;
}
.mat-drawer.mat-drawer-opened.mat-drawer-opened {
  transform: none;
}

.mat-drawer-side {
  box-shadow: none;
  border-right-color: var(--%NS%mat-sidenav-container-divider-color, transparent);
  border-right-width: 1px;
  border-right-style: solid;
}
.mat-drawer-side.mat-drawer-end {
  border-left-color: var(--%NS%mat-sidenav-container-divider-color, transparent);
  border-left-width: 1px;
  border-left-style: solid;
  border-right: none;
}
[dir=rtl] .mat-drawer-side {
  border-left-color: var(--%NS%mat-sidenav-container-divider-color, transparent);
  border-left-width: 1px;
  border-left-style: solid;
  border-right: none;
}
[dir=rtl] .mat-drawer-side.mat-drawer-end {
  border-right-color: var(--%NS%mat-sidenav-container-divider-color, transparent);
  border-right-width: 1px;
  border-right-style: solid;
  border-left: none;
}

.mat-drawer-inner-container {
  width: 100%;
  height: 100%;
  overflow: auto;
}

.mat-sidenav-fixed {
  position: fixed;
}
`],encapsulation:2})}return r})();var pt=(()=>{class r extends j{static ɵfac=(()=>{let e;return function(n){return(e||(e=Ng(r)))(n||r)}})();static ɵcmp=Jb({type:r,selectors:[[`mat-sidenav-content`]],hostAttrs:[1,`mat-drawer-content`,`mat-sidenav-content`],features:[tS([{provide:ht$1,useExisting:r},{provide:j,useExisting:r}]),sy],ngContentSelectors:Y,decls:1,vars:0,template:function(t,n){t&1&&(f_(),p_(0))},encapsulation:2})}return r})();var Bt=(()=>{class r extends be{get fixedInViewport(){return this._fixedInViewport}set fixedInViewport(e){this._fixedInViewport=rl(e)}_fixedInViewport=!1;get fixedTopGap(){return this._fixedTopGap}set fixedTopGap(e){this._fixedTopGap=H$1(e)}_fixedTopGap=0;get fixedBottomGap(){return this._fixedBottomGap}set fixedBottomGap(e){this._fixedBottomGap=H$1(e)}_fixedBottomGap=0;static ɵfac=(()=>{let e;return function(n){return(e||(e=Ng(r)))(n||r)}})();static ɵcmp=Jb({type:r,selectors:[[`mat-sidenav`]],hostAttrs:[1,`mat-drawer`,`mat-sidenav`],hostVars:16,hostBindings:function(t,n){t&2&&(Iy(`tabIndex`,n.mode!==`side`?`-1`:null)(`align`,null),$y(`top`,n.fixedInViewport?n.fixedTopGap:null,`px`)(`bottom`,n.fixedInViewport?n.fixedBottomGap:null,`px`),Gy(`mat-drawer-end`,n.position===`end`)(`mat-drawer-over`,n.mode===`over`)(`mat-drawer-push`,n.mode===`push`)(`mat-drawer-side`,n.mode===`side`)(`mat-sidenav-fixed`,n.fixedInViewport))},inputs:{fixedInViewport:`fixedInViewport`,fixedTopGap:`fixedTopGap`,fixedBottomGap:`fixedBottomGap`},exportAs:[`matSidenav`],features:[tS([{provide:be,useExisting:r}]),sy],ngContentSelectors:Y,decls:3,vars:0,consts:[[`content`,``],[`cdkScrollable`,``,1,`mat-drawer-inner-container`]],template:function(t,n){t&1&&(f_(),ia(0,`div`,1,0),p_(2),Td())},dependencies:[ht$1],encapsulation:2})}return r})();var hn=(()=>{class r extends ve{_allDrawers=void 0;_content=void 0;static ɵfac=(()=>{let e;return function(n){return(e||(e=Ng(r)))(n||r)}})();static ɵcmp=Jb({type:r,selectors:[[`mat-sidenav-container`]],contentQueries:function(t,n,i){if(t&1&&Fy(i,pt,5)(i,Bt,5),t&2){let a;g_(a=m_())&&(n._content=a.first),g_(a=m_())&&(n._allDrawers=a)}},hostAttrs:[1,`mat-drawer-container`,`mat-sidenav-container`],hostVars:2,hostBindings:function(t,n){t&2&&Gy(`mat-drawer-container-explicit-backdrop`,n._backdropOverride)},exportAs:[`matSidenavContainer`],features:[tS([{provide:Ce,useExisting:r},{provide:ve,useExisting:r}]),sy],ngContentSelectors:ht,decls:4,vars:2,consts:[[1,`mat-drawer-backdrop`,3,`mat-drawer-shown`],[1,`mat-drawer-backdrop`,3,`click`]],template:function(t,n){t&1&&(f_(ut),YT(0,xt,1,2,`div`,0),p_(1),p_(2,1),YT(3,yt,2,0,`mat-sidenav-content`)),t&2&&(QT(n.hasBackdrop?0:-1),SC(3),QT(n._content?-1:3))},dependencies:[pt],styles:[St],encapsulation:2})}return r})();var _n=(()=>{class r{static ɵfac=function(t){return new(t||r)};static ɵmod=cr({type:r});static ɵinj=nn({imports:[ct,ve$1,ct]})}return r})();var Dt=(r,s)=>({closed:r,open:s,"ml-auto":!0});var Mt=(r,s)=>s.path;function It(r,s){if(r&1&&Cy(0,`app-feature-icon-indicator`,2),r&2)wy(`featureNameId`,l_().navItem().featureRequired)}function Tt(r,s){if(r&1&&(ia(0,`mat-icon`,3),$_(1,` expand_more`),Td()),r&2){let e=l_();wy(`ngClass`,oS(1,Dt,!e.isOpenMenu,e.isOpenMenu))}}function Et(r,s){if(r&1&&Cy(0,`app-nav-item-list`,5),r&2){let e=s.$implicit;wy(`navItem`,e)}}function Ft(r,s){if(r&1&&(ia(0,`div`,4),JT(1,Et,1,1,`app-nav-item-list`,5,Mt),Td()),r&2){let e=l_();SC(),e_(e.navItem().child)}}var kn=(()=>{class r{constructor(){this.navItem=vU(),this.isOpenMenu=!1,this.router=m(F)}ngOnInit(){this.#e(),this.router.events.subscribe(()=>this.#e())}#e(){if(!this.navItem().child?.length)return;let e=this.router.url.split(`/`);this.navItem().child.find(n=>e.includes(n.path))&&(this.isOpenMenu=!0)}static{this.ɵfac=function(t){return new(t||r)}}static{this.ɵcmp=Jb({type:r,selectors:[[`app-nav-item-list`]],inputs:{navItem:[1,`navItem`]},decls:8,vars:7,consts:[[`routerLinkActive`,`active`,`matRipple`,``,1,`app-menu-container`,3,`click`,`routerLink`,`tippy`],[1,`link-text`],[3,`featureNameId`],[3,`ngClass`],[1,`paddingLink`],[3,`navItem`]],template:function(t,n){t&1&&(ia(0,`a`,0),Ry(`click`,function(){return n.isOpenMenu=!n.isOpenMenu}),ia(1,`mat-icon`),$_(2),Td(),ia(3,`div`,1),$_(4),YT(5,It,1,1,`app-feature-icon-indicator`,2),Td(),YT(6,Tt,2,4,`mat-icon`,3),Td(),YT(7,Ft,3,0,`div`,4)),t&2&&(wy(`routerLink`,n.navItem().path)(`tippy`,n.navItem().tippy),SC(2),ev(n.navItem().icon),SC(2),xd(``,n.navItem().name,` `),SC(),QT(n.navItem().featureRequired?5:-1),SC(),QT(n.navItem().child?6:-1),SC(),QT(n.isOpenMenu?7:-1))},dependencies:[r,yt$1,wt$1,vM,$t$1,rs,rp,B],styles:[`.paddingLink[_ngcontent-%COMP%]   a[_ngcontent-%COMP%]{padding-left:40px}.active[_ngcontent-%COMP%]{background-color:#8080801a}.app-menu-container[_ngcontent-%COMP%]{padding:15px 25px;display:flex;align-items:center;text-decoration:none;color:#000;transition:all 225ms ease-in-out}.app-menu-container[_ngcontent-%COMP%]:hover{background-color:#80808033}.app-menu-container[_ngcontent-%COMP%]   .link-text[_ngcontent-%COMP%]{font-size:18px;font-weight:500;margin-left:25px}.app-menu-container[_ngcontent-%COMP%]   .mat-icon[_ngcontent-%COMP%]{transition:all 225ms ease-in-out}.app-menu-container[_ngcontent-%COMP%]   .mat-icon.ml-auto[_ngcontent-%COMP%]{margin-left:auto}.app-menu-container[_ngcontent-%COMP%]   .open[_ngcontent-%COMP%]{transform:rotate(180deg)}
/*# sourceMappingURL=nav-item-list-M2KPBGET.css.map */`]})}}return r})();var Nt=r=>({"xng-breadcrumb-link-disabled":r});var ft=(r,s,e,t,n,i)=>({$implicit:r,info:s,last:e,first:t,index:n,count:i});function Rt(r,s){r&1&&_y(0)}function Pt(r,s){if(r&1&&$_(0),r&2){let e=l_(2).$implicit;xd(` `,e.label,` `)}}function At(r,s){if(r&1&&(ia(0,`a`,3),cy(1,Rt,1,0,`ng-container`,6),YT(2,Pt,1,1),Td()),r&2){let e=l_(),t=e.$implicit,n=e.$index,i=e.$count,a=l_();wy(`ngClass`,rS(10,Nt,t.disable))(`routerLink`,t.routeInterceptor?t.routeInterceptor(t.routeLink,t):t.routeLink)(`queryParams`,a.preserveQueryParams?t.queryParams:void 0)(`fragment`,a.preserveFragment?t.fragment:void 0)(`target`,a.anchorTarget?a.anchorTarget:`_self`),Iy(`aria-disabled`,t.disable)(`tabIndex`,t.disable?-1:0),SC(),wy(`ngTemplateOutlet`,a.itemTemplate)(`ngTemplateOutletContext`,cS(12,ft,t.label,t.info,n===i-1,n===0,n,i)),SC(),QT(a.itemTemplate?-1:2)}}function Ot(r,s){r&1&&_y(0)}function Lt(r,s){if(r&1&&$_(0),r&2){let e=l_(2).$implicit;xd(` `,e.label,` `)}}function jt(r,s){if(r&1&&(ia(0,`span`,4),cy(1,Ot,1,0,`ng-container`,6),YT(2,Lt,1,1),Td()),r&2){let e=l_(),t=e.$implicit,n=e.$index,i=e.$count,a=l_();SC(),wy(`ngTemplateOutlet`,a.itemTemplate)(`ngTemplateOutletContext`,cS(3,ft,t.label,t.info,n===i-1,n===0,n,i)),SC(),QT(a.itemTemplate?-1:2)}}function Vt(r,s){r&1&&_y(0)}function zt(r,s){if(r&1&&$_(0),r&2)xd(` `,l_(3).separator,` `)}function Qt(r,s){if(r&1&&(ia(0,`li`,5),cy(1,Vt,1,0,`ng-container`,7),YT(2,zt,1,1),Td()),r&2){let e=l_(2);SC(),wy(`ngTemplateOutlet`,e.separatorTemplate),SC(),QT(e.separatorTemplate?-1:2)}}function Gt(r,s){if(r&1&&(ia(0,`li`,2),YT(1,At,3,19,`a`,3),YT(2,jt,3,10,`span`,4),Td(),YT(3,Qt,3,2,`li`,5)),r&2){let e=s.$index,t=s.$count;SC(),QT(e!==t-1?1:-1),SC(),QT(e===t-1?2:-1),SC(),QT(e!==t-1?3:-1)}}var xe=(()=>{class r{static{this.ɵfac=function(t){return new(t||r)}}static{this.ɵdir=ii({type:r,selectors:[[``,`xngBreadcrumbItem`,``]]})}}return r})();var ee={PREFIX:`:`,REGEX_IDENTIFIER:`/:[^/]+`,REGEX_REPLACER:`/[^/]+`};var $t=`@`;var _t=r=>!!r&&Object.keys(r).length>0;var Wt=(()=>{class r{constructor(){this.baseHref=`/`,this.dynamicBreadcrumbStore=[],this.currentBreadcrumbs=[],this.previousBreadcrumbs=[],this.breadcrumbs=new uo([]),this.breadcrumbs$=this.breadcrumbs.asObservable(),this.activatedRoute=m(W),this.router=m(F),this.detectRouteChanges()}detectRouteChanges(){this.setupBreadcrumbs(this.activatedRoute.snapshot),this.router.events.pipe(Kt(e=>e instanceof Rt$1)).subscribe(e=>{e.shouldActivate&&this.setupBreadcrumbs(e.state.root)})}setupBreadcrumbs(e){this.previousBreadcrumbs=this.currentBreadcrumbs;let t=this.getRootBreadcrumb();this.currentBreadcrumbs=t?[t]:[],this.prepareBreadcrumbList(e,this.baseHref)}getRootBreadcrumb(){let e=this.router.config.find(i=>i.path===``),t=this.extractObject(e?.data?.breadcrumb),n=this.getFromStore(t.alias,`/`);if(_t(t)||_t(n))return w(x(w(w({},n),t),{routeLink:this.baseHref}),this.getQueryParamsFromPreviousList(`/`))}prepareBreadcrumbItem(e,t){let{path:n,breadcrumb:i}=this.parseRouteData(e.routeConfig),a=this.resolvePathSegment(n,e),F=`${t}${a}`,ye=this.getFromStore(i.alias,F),Se=this.extractLabel(ye?.label||i?.label,a),te=!1,ke=``;return Se||(te=!0,ke=a),w(x(w(w({},ye),i),{label:te?ke:Se,routeLink:F,isAutoGeneratedLabel:te}),this.getQueryParamsFromPreviousList(F))}prepareBreadcrumbList(e,t){if(e.routeConfig?.path){let a=this.prepareBreadcrumbItem(e,t);if(this.currentBreadcrumbs.push(a),e.firstChild)return this.prepareBreadcrumbList(e.firstChild,a.routeLink+`/`)}else if(e.firstChild)return this.prepareBreadcrumbList(e.firstChild,t);let n=this.currentBreadcrumbs[this.currentBreadcrumbs.length-1];this.setQueryParamsForActiveBreadcrumb(n,e);let i=this.currentBreadcrumbs.filter(a=>!a.skip);this.breadcrumbs.next(i)}getFromStore(e,t){return this.dynamicBreadcrumbStore.find(n=>e&&e===n.alias||t&&t===n.routeLink||this.matchRegex(t,n.routeRegex))}matchRegex(e,t){return e.match(new RegExp(t))?.[0]===e}resolvePathSegment(e,t){return e.includes(ee.PREFIX)&&Object.entries(t.params).forEach(([n,i])=>{e=e.replace(`:${n}`,`${i}`)}),e}getQueryParamsFromPreviousList(e){let{queryParams:t,fragment:n}=this.previousBreadcrumbs.find(i=>i.routeLink===e)||{};return{queryParams:t,fragment:n}}setQueryParamsForActiveBreadcrumb(e,t){if(e){let{queryParams:n,fragment:i}=t;e.queryParams=n?w({},n):void 0,e.fragment=i}}parseRouteData(e){let{path:t,data:n}=e;return{path:t,breadcrumb:this.mergeWithBaseChildData(e,n?.breadcrumb)}}mergeWithBaseChildData(e,t){if(!e)return this.extractObject(t);let n;e.loadChildren?n=e._loadedRoutes.find(a=>a.path===``):e.children&&(n=e.children.find(a=>a.path===``));let i=n?.data?.breadcrumb;return i?this.mergeWithBaseChildData(n,w(w({},this.extractObject(t)),this.extractObject(i))):this.extractObject(t)}set(e,t){let n=this.extractObject(t),i;e.startsWith($t)?i=[`alias`,x(w({},n),{alias:e.slice(1)})]:e.includes(ee.PREFIX)?i=[`routeRegex`,x(w({},n),{routeRegex:this.buildRegex(e)})]:i=[`routeLink`,x(w({},n),{routeLink:this.ensureLeadingSlash(e)})],i[1].isAutoGeneratedLabel=!1,this.updateStore(...i),this.updateCurrentBreadcrumbs(...i)}updateStore(e,t){let n=this.dynamicBreadcrumbStore.findIndex(i=>t[e]===i[e]);n>-1?this.dynamicBreadcrumbStore[n]=w(w({},this.dynamicBreadcrumbStore[n]),t):this.dynamicBreadcrumbStore.push(w({},t))}updateCurrentBreadcrumbs(e,t){let n=this.currentBreadcrumbs.findIndex(i=>e===`routeRegex`?this.matchRegex(i.routeLink,t[e]):t[e]===i[e]);if(n>-1){this.currentBreadcrumbs[n]=w(w({},this.currentBreadcrumbs[n]),t);let i=this.currentBreadcrumbs.filter(a=>!a.skip);this.breadcrumbs.next([...i])}}buildRegex(e){return this.ensureLeadingSlash(e).replace(new RegExp(ee.REGEX_IDENTIFIER,`g`),ee.REGEX_REPLACER)}ensureLeadingSlash(e){return e.startsWith(`/`)?e:`/${e}`}extractLabel(e,t){let n=typeof e==`object`?e.label:e;return typeof n==`function`?n(t):n}extractObject(e){return e&&(typeof e==`string`||typeof e==`function`)?{label:e}:e||{}}static{this.ɵfac=function(t){return new(t||r)}}static{this.ɵprov=N({token:r,factory:r.ɵfac,providedIn:`root`})}}return r})();var gt=(()=>{class r{set separator(e){e instanceof qr?(this.separatorTemplate=e,this._separator=void 0):(this.separatorTemplate=void 0,this._separator=e||`/`)}get separator(){return this._separator}constructor(){this._separator=`/`,this.autoGenerate=!0,this.preserveQueryParams=!0,this.preserveFragment=!0,this.class=``,this.setupMessage=`not set up yet`,this.someParameterValue=null,this.breadcrumbService=m(Wt),this.activateRoute=m(W),this.activateRoute.params.subscribe(e=>{this.setupComponent(e.someParam)})}setupComponent(e){this.setupMessage=`set up at `+new Date,this.someParameterValue=e}ngOnInit(){this.breadcrumbs$=this.breadcrumbService.breadcrumbs$.pipe(Te(e=>e.filter(t=>this.autoGenerate?!0:!t.isAutoGeneratedLabel).map(t=>{let{routeInterceptor:n,routeLink:i}=t;return x(w({},t),{routeLink:n?.(i,t)||i})})))}static{this.ɵfac=function(t){return new(t||r)}}static{this.ɵcmp=Jb({type:r,selectors:[[`xng-breadcrumb`]],contentQueries:function(t,n,i){if(t&1&&Fy(i,xe,5,qr),t&2){let a;g_(a=m_())&&(n.itemTemplate=a.first)}},inputs:{autoGenerate:`autoGenerate`,preserveQueryParams:`preserveQueryParams`,preserveFragment:`preserveFragment`,class:`class`,anchorTarget:`anchorTarget`,separator:`separator`},decls:5,vars:3,consts:[[`aria-label`,`breadcrumb`,1,`xng-breadcrumb-root`,3,`ngClass`],[1,`xng-breadcrumb-list`],[1,`xng-breadcrumb-item`],[`rel`,`noopener noreferrer`,1,`xng-breadcrumb-link`,3,`ngClass`,`routerLink`,`queryParams`,`fragment`,`target`],[1,`xng-breadcrumb-trail`],[`aria-hidden`,`true`,1,`xng-breadcrumb-separator`],[4,`ngTemplateOutlet`,`ngTemplateOutletContext`],[4,`ngTemplateOutlet`]],template:function(t,n){t&1&&(ia(0,`nav`,0)(1,`ol`,1),JT(2,Gt,4,3,null,null,XT),lS(4,`async`),Td()()),t&2&&(wy(`ngClass`,n.class),SC(2),e_(fS(4,1,n.breadcrumbs$)))},dependencies:[rf,vM,EM,CM,ss,$t$1],styles:[`.xng-breadcrumb-root{margin:0}.xng-breadcrumb-list{display:flex;align-items:center;flex-wrap:wrap;margin:0;padding:0}.xng-breadcrumb-item{list-style:none}.xng-breadcrumb-trail{display:flex;align-items:center}.xng-breadcrumb-link{display:flex;align-items:center;white-space:nowrap;color:inherit;text-decoration:none;transition:text-decoration .3s;cursor:pointer}.xng-breadcrumb-link:hover{text-decoration:underline}.xng-breadcrumb-link-disabled{pointer-events:none;cursor:disabled}.xng-breadcrumb-separator{display:flex;-webkit-user-select:none;user-select:none;margin-left:8px;margin-right:8px}
`],encapsulation:2,changeDetection:1})}}return r})();function Ht(r,s){if(r&1&&(ia(0,`mat-icon`,3),$_(1,`chevron_right`),Td(),$_(2)),r&2){let e=l_().$implicit;SC(2),xd(` `,e,` `)}}function qt(r,s){if(r&1&&(Md(0),YT(1,Ht,3,1),_a()),r&2){let e=s.last;SC(),QT(e?1:-1)}}function Zt(r,s){}var $n=(()=>{class r{static{this.ɵfac=function(t){return new(t||r)}}static{this.ɵcmp=Jb({type:r,selectors:[[`app-breadcrumb`]],decls:4,vars:1,consts:[[`emptyTemplate`,``],[3,`separator`],[4,`xngBreadcrumbItem`],[1,`breadcrumb-icon`]],template:function(t,n){if(t&1&&(ia(0,`xng-breadcrumb`,1),cy(1,qt,2,1,`ng-container`,2),Td(),cy(2,Zt,0,0,`ng-template`,null,0,gS)),t&2)wy(`separator`,v_(3))},dependencies:[gt,xe,wt$1],encapsulation:2})}}return r})();export{kn as a,hn as i,Bt as n,pt as o,_n as r,$n as t};
//# debugId=315deef3-f99f-574c-9123-f0289c1af69a
//# sourceMappingURL=https://sources.2sxc.org/22.01.00/dist/ng-edit/chunk-BG2iw_Zk.js.map