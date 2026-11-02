import{$r as tS,$t as Zp,An as fc,At as Td,B as JT,Br as rm,Ci as xd,Cr as om,D as Fy,Dn as ev,Dr as pb,Dt as Sn,Ei as xy,Er as p_,Et as Sd,G as KS,Gn as ia,Gt as YT,In as gU,Jn as iv,Jt as Z,Kn as ii,L as Iy,Li as zt$1,Ln as g_,Mi as ye,N as I,O as Gy,On as fS,Q as Ly,Si as xM,Sn as de,St as SC,Ti as xt,Tn as e_,V as Jb,Vn as he,Wn as i_,Xr as sv,Y as Kt$1,Yr as st,Yt as Z_,Zn as jt,Zr as sy,Zt as Zd,_n as cr,bi as wy,bn as db,cr as m_,di as v_,dt as O_,en as Zs,g as Cy,gt as QT,hi as wE,hr as nn,ht as QS,in as _d,ir as m,k as H,kn as f_,lr as mc,lt as Ng,n as $N,nr as l_,qt as Yp,r as $_,st as NE,tr as lS,vt as Qf,xt as Ry,zr as rf}from"./chunk-DtJSyZhi.js";import{$ as xl}from"./chunk-C4dVjHVF.js";import{B as be,L as ar,X as j,Z as je,c as Dt$1,ft as ve,nt as le,rt as nn$1}from"./chunk-D18RyqxO.js";import{c as Tt,f as kt$1,i as It$1,m as oe$1,p as ne$1,s as Q,t as Et$1}from"./chunk-CwWNeagF.js";import{t as Qt$1}from"./chunk-T2ghyHO0.js";import{E as Z$1,T as J,b as ti,d as yt}from"./main-NXRXUN76.js";import{m as fe,p as an,s as Pn,x as wn$1,y as tn}from"./chunk-CJWIt6fL.js";import{r as rp}from"./chunk-BRhjinLo.js";import{t as z}from"./chunk-CpDLQUtk.js";import{a as Vt$1,i as V,o as jt$1,s as qe}from"./chunk-Bo0R4CmS.js";import{t as A}from"./chunk-Dht4ZADc.js";import{t as I$1}from"./chunk-sarzC6P2.js";import{t as c}from"./chunk-Cd_AXJ8z.js";import{t as d}from"./chunk-CLnQcnI42.js";import{i as je$1,n as V$1,r as j$1}from"./chunk-D0sGJIv02.js";import{r as Pt$1}from"./chunk-Ce8KoDwL2.js";import{r as y}from"./chunk-fEXW6uIj.js";import{a as un,n as Oe,o as xn,r as fn,t as Fe}from"./chunk-lIKt1-2j2.js";import{n as pe,t as ce$1}from"./chunk-DFJEUd1i.js";import{t as b}from"./chunk-DBBeY2a1.js";import{n as we,t as te$1}from"./chunk-DRQj-7GJ.js";var Pt=[`*`,[[`mat-chip-avatar`],[``,`matChipAvatar`,``]],[[`mat-chip-trailing-icon`],[``,`matChipRemove`,``],[``,`matChipTrailingIcon`,``]]];var Bt=[`*`,`mat-chip-avatar, [matChipAvatar]`,`mat-chip-trailing-icon,[matChipRemove],[matChipTrailingIcon]`];function Ot(n,d){n&1&&(ia(0,`span`,3),p_(1,1),Td())}function Lt(n,d){n&1&&(ia(0,`span`,6),p_(1,2),Td())}var Vt=[`*`];var zt=new I(`mat-chips-default-options`,{providedIn:`root`,factory:()=>({separatorKeyCodes:[13]})});var It=new I(`MatChipAvatar`);var Et=new I(`MatChipTrailingIcon`);var Nt=new I(`MatChipEdit`);var kt=new I(`MatChipRemove`);var Dt=new I(`MatChip`);var Ft=(()=>{class n{_elementRef=m(zt$1);_parentChip=m(Dt);_isPrimary=!0;_isLeading=!1;get disabled(){return this._disabled||this._parentChip?.disabled||!1}set disabled(i){this._disabled=i}_disabled=!1;tabIndex=-1;_allowFocusWhenDisabled=!1;_getDisabledAttribute(){return this.disabled&&!this._allowFocusWhenDisabled?``:null}constructor(){m(j).load(kt$1),this._elementRef.nativeElement.nodeName===`BUTTON`&&this._elementRef.nativeElement.setAttribute(`type`,`button`)}focus(){this._elementRef.nativeElement.focus()}static ɵfac=function(e){return new(e||n)};static ɵdir=ii({type:n,selectors:[[``,`matChipContent`,``]],hostAttrs:[1,`mat-mdc-chip-action`,`mdc-evolution-chip__action`,`mdc-evolution-chip__action--presentational`],hostVars:8,hostBindings:function(e,t){e&2&&(Iy(`disabled`,t._getDisabledAttribute())(`aria-disabled`,t.disabled),Gy(`mdc-evolution-chip__action--primary`,t._isPrimary)(`mdc-evolution-chip__action--secondary`,!t._isPrimary)(`mdc-evolution-chip__action--trailing`,!t._isPrimary&&!t._isLeading))},inputs:{disabled:[2,`disabled`,`disabled`,QS],tabIndex:[2,`tabIndex`,`tabIndex`,i=>i==null?-1:KS(i)],_allowFocusWhenDisabled:`_allowFocusWhenDisabled`}})}return n})();var Ht=(()=>{class n extends Ft{_getTabindex(){return this.disabled&&!this._allowFocusWhenDisabled?null:this.tabIndex.toString()}_handleClick(i){!this.disabled&&this._isPrimary&&(i.preventDefault(),this._parentChip._handlePrimaryActionInteraction())}_handleKeydown(i){(i.keyCode===13||i.keyCode===32)&&!this.disabled&&this._isPrimary&&!this._parentChip._isEditing&&(i.preventDefault(),this._parentChip._handlePrimaryActionInteraction())}static ɵfac=(()=>{let i;return function(t){return(i||(i=Ng(n)))(t||n)}})();static ɵdir=ii({type:n,selectors:[[``,`matChipAction`,``]],hostVars:3,hostBindings:function(e,t){e&1&&Ry(`click`,function(c){return t._handleClick(c)})(`keydown`,function(c){return t._handleKeydown(c)}),e&2&&(Iy(`tabindex`,t._getTabindex()),Gy(`mdc-evolution-chip__action--presentational`,!1))},features:[sy]})}return n})();var ei=(()=>{class n{_changeDetectorRef=m(Zd);_elementRef=m(zt$1);_tagName=m(gU);_ngZone=m(H);_focusMonitor=m(nn$1);_globalRippleOptions=m(Q,{optional:!0});_document=m(Z);_onFocus=new de;_onBlur=new de;_isBasicChip=!1;role=null;_hasFocusInternal=!1;_pendingFocus=!1;_actionChanges;_animationsDisabled=ar();_allLeadingIcons;_allTrailingIcons;_allEditIcons;_allRemoveIcons;_hasFocus(){return this._hasFocusInternal}id=m(be).getId(`mat-mdc-chip-`);ariaLabel=null;ariaDescription=null;_chipListDisabled=!1;_hadFocusOnRemove=!1;_textElement;get value(){return this._value!==void 0?this._value:this._textElement.textContent.trim()}set value(i){this._value=i}_value;color;removable=!0;highlighted=!1;disableRipple=!1;get disabled(){return this._disabled||this._chipListDisabled}set disabled(i){this._disabled=i}_disabled=!1;removed=new jt;destroyed=new jt;basicChipAttrName=`mat-basic-chip`;leadingIcon;editIcon;trailingIcon;removeIcon;primaryAction;_rippleLoader=m(Et$1);_injector=m(ye);constructor(){let i=m(j);i.load(kt$1),i.load(je),this._monitorFocus(),this._rippleLoader?.configureRipple(this._elementRef.nativeElement,{className:`mat-mdc-chip-ripple`,disabled:this._isRippleDisabled()})}ngOnInit(){this._isBasicChip=this._elementRef.nativeElement.hasAttribute(this.basicChipAttrName)||this._tagName.toLowerCase()===this.basicChipAttrName}ngAfterViewInit(){this._textElement=this._elementRef.nativeElement.querySelector(`.mat-mdc-chip-action-label`),this._pendingFocus&&(this._pendingFocus=!1,this.focus())}ngAfterContentInit(){this._actionChanges=wE(this._allLeadingIcons.changes,this._allTrailingIcons.changes,this._allEditIcons.changes,this._allRemoveIcons.changes).subscribe(()=>this._changeDetectorRef.markForCheck())}ngDoCheck(){this._rippleLoader.setDisabled(this._elementRef.nativeElement,this._isRippleDisabled())}ngOnDestroy(){this.destroyed.emit({chip:this}),this.destroyed.complete(),this._focusMonitor.stopMonitoring(this._elementRef),this._rippleLoader?.destroyRipple(this._elementRef.nativeElement),this._actionChanges?.unsubscribe()}remove(){this.removable&&(this._hadFocusOnRemove=this._hasFocus(),this.removed.emit({chip:this}))}_isRippleDisabled(){return this.disabled||this.disableRipple||this._animationsDisabled||this._isBasicChip||!this._hasInteractiveActions()||!!this._globalRippleOptions?.disabled}_hasTrailingIcon(){return!!(this.trailingIcon||this.removeIcon)}_handleKeydown(i){(i.keyCode===8&&!i.repeat||i.keyCode===46)&&(i.preventDefault(),this.remove())}focus(){this.disabled||(this.primaryAction?this.primaryAction.focus():this._pendingFocus=!0)}_getSourceAction(i){return this._getActions().find(e=>{let t=e._elementRef.nativeElement;return t===i||t.contains(i)})}_getActions(){let i=[];return this.editIcon&&i.push(this.editIcon),this.primaryAction&&i.push(this.primaryAction),this.removeIcon&&i.push(this.removeIcon),i}_handlePrimaryActionInteraction(){}_hasInteractiveActions(){return this._getActions().length>0}_edit(i){}_monitorFocus(){this._focusMonitor.monitor(this._elementRef,!0).subscribe(i=>{let e=i!==null;e!==this._hasFocusInternal&&(this._hasFocusInternal=e,e?this._onFocus.next({chip:this}):(this._changeDetectorRef.markForCheck(),setTimeout(()=>this._ngZone.run(()=>this._onBlur.next({chip:this})))))})}static ɵfac=function(e){return new(e||n)};static ɵcmp=Jb({type:n,selectors:[[`mat-basic-chip`],[``,`mat-basic-chip`,``],[`mat-chip`],[``,`mat-chip`,``]],contentQueries:function(e,t,a){if(e&1&&Fy(a,It,5)(a,Nt,5)(a,Et,5)(a,kt,5)(a,It,5)(a,Et,5)(a,Nt,5)(a,kt,5),e&2){let c;g_(c=m_())&&(t.leadingIcon=c.first),g_(c=m_())&&(t.editIcon=c.first),g_(c=m_())&&(t.trailingIcon=c.first),g_(c=m_())&&(t.removeIcon=c.first),g_(c=m_())&&(t._allLeadingIcons=c),g_(c=m_())&&(t._allTrailingIcons=c),g_(c=m_())&&(t._allEditIcons=c),g_(c=m_())&&(t._allRemoveIcons=c)}},viewQuery:function(e,t){if(e&1&&Ly(Ht,5),e&2){let a;g_(a=m_())&&(t.primaryAction=a.first)}},hostAttrs:[1,`mat-mdc-chip`],hostVars:31,hostBindings:function(e,t){e&1&&Ry(`keydown`,function(c){return t._handleKeydown(c)}),e&2&&(xy(`id`,t.id),Iy(`role`,t.role)(`aria-label`,t.ariaLabel),O_(`mat-`+(t.color||`primary`)),Gy(`mdc-evolution-chip`,!t._isBasicChip)(`mdc-evolution-chip--disabled`,t.disabled)(`mdc-evolution-chip--with-trailing-action`,t._hasTrailingIcon())(`mdc-evolution-chip--with-primary-graphic`,t.leadingIcon)(`mdc-evolution-chip--with-primary-icon`,t.leadingIcon)(`mdc-evolution-chip--with-avatar`,t.leadingIcon)(`mat-mdc-chip-with-avatar`,t.leadingIcon)(`mat-mdc-chip-highlighted`,t.highlighted)(`mat-mdc-chip-disabled`,t.disabled)(`mat-mdc-basic-chip`,t._isBasicChip)(`mat-mdc-standard-chip`,!t._isBasicChip)(`mat-mdc-chip-with-trailing-icon`,t._hasTrailingIcon())(`_mat-animation-noopable`,t._animationsDisabled))},inputs:{role:`role`,id:`id`,ariaLabel:[0,`aria-label`,`ariaLabel`],ariaDescription:[0,`aria-description`,`ariaDescription`],value:`value`,color:`color`,removable:[2,`removable`,`removable`,QS],highlighted:[2,`highlighted`,`highlighted`,QS],disableRipple:[2,`disableRipple`,`disableRipple`,QS],disabled:[2,`disabled`,`disabled`,QS]},outputs:{removed:`removed`,destroyed:`destroyed`},exportAs:[`matChip`],features:[tS([{provide:Dt,useExisting:n}])],ngContentSelectors:Bt,decls:8,vars:2,consts:[[1,`mat-mdc-chip-focus-overlay`],[1,`mdc-evolution-chip__cell`,`mdc-evolution-chip__cell--primary`],[`matChipContent`,``],[1,`mdc-evolution-chip__graphic`,`mat-mdc-chip-graphic`],[1,`mdc-evolution-chip__text-label`,`mat-mdc-chip-action-label`],[1,`mat-mdc-chip-primary-focus-indicator`,`mat-focus-indicator`],[1,`mdc-evolution-chip__cell`,`mdc-evolution-chip__cell--trailing`]],template:function(e,t){e&1&&(f_(Pt),Cy(0,`span`,0),ia(1,`span`,1)(2,`span`,2),YT(3,Ot,2,0,`span`,3),ia(4,`span`,4),p_(5),Cy(6,`span`,5),Td()()(),YT(7,Lt,2,0,`span`,6)),e&2&&(SC(3),QT(t.leadingIcon?3:-1),SC(4),QT(t._hasTrailingIcon()?7:-1))},dependencies:[Ft],styles:[`.mdc-evolution-chip,
.mdc-evolution-chip__cell,
.mdc-evolution-chip__action {
  display: inline-flex;
  align-items: center;
}

.mdc-evolution-chip {
  position: relative;
  max-width: 100%;
}

.mdc-evolution-chip__cell,
.mdc-evolution-chip__action {
  height: 100%;
}

.mdc-evolution-chip__cell--primary {
  flex-basis: 100%;
  overflow-x: hidden;
}

.mdc-evolution-chip__cell--trailing {
  flex: 1 0 auto;
}

.mdc-evolution-chip__action {
  align-items: center;
  background: none;
  border: none;
  box-sizing: content-box;
  cursor: pointer;
  display: inline-flex;
  justify-content: center;
  outline: none;
  padding: 0;
  text-decoration: none;
  color: inherit;
}

.mdc-evolution-chip__action--presentational {
  cursor: auto;
}

.mdc-evolution-chip--disabled,
.mdc-evolution-chip__action:disabled {
  pointer-events: none;
}
@media (forced-colors: active) {
  .mdc-evolution-chip--disabled,
  .mdc-evolution-chip__action:disabled {
    forced-color-adjust: none;
  }
}

.mdc-evolution-chip__action--primary {
  font: inherit;
  letter-spacing: inherit;
  white-space: inherit;
  overflow-x: hidden;
}
.mat-mdc-standard-chip .mdc-evolution-chip__action--%NS%primary::before {
  border-width: var(--%NS%mat-chip-outline-width, 1px);
  border-radius: var(--%NS%mat-chip-container-shape-radius, 8px);
  box-sizing: border-box;
  content: "";
  height: 100%;
  left: 0;
  position: absolute;
  pointer-events: none;
  top: 0;
  width: 100%;
  z-index: 1;
  border-style: solid;
}
.mat-mdc-standard-chip .mdc-evolution-chip__action--primary {
  padding-left: 12px;
  padding-right: 12px;
}
.mat-mdc-standard-chip.mdc-evolution-chip--with-primary-graphic .mdc-evolution-chip__action--primary {
  padding-left: 0;
  padding-right: 12px;
}
[dir=rtl] .mat-mdc-standard-chip.mdc-evolution-chip--with-primary-graphic .mdc-evolution-chip__action--primary {
  padding-left: 12px;
  padding-right: 0;
}
.mat-mdc-standard-chip:not(.mdc-evolution-chip--disabled) .mdc-evolution-chip__action--%NS%primary::before {
  border-color: var(--%NS%mat-chip-outline-color, var(--%NS%mat-sys-outline));
}
.mdc-evolution-chip__action--%NS%primary:not(.mdc-evolution-chip__action--presentational):not(.mdc-ripple-upgraded):focus::before {
  border-color: var(--%NS%mat-chip-focus-outline-color, var(--%NS%mat-sys-on-surface-variant));
}
.mat-mdc-standard-chip.mdc-evolution-chip--disabled .mdc-evolution-chip__action--%NS%primary::before {
  border-color: var(--%NS%mat-chip-disabled-outline-color, color-mix(in srgb, var(--%NS%mat-sys-on-surface) 12%, transparent));
}
.mat-mdc-standard-chip.mdc-evolution-chip--selected .mdc-evolution-chip__action--%NS%primary::before {
  border-width: var(--%NS%mat-chip-flat-selected-outline-width, 0);
}
.mat-mdc-basic-chip .mdc-evolution-chip__action--primary {
  font: inherit;
}
.mat-mdc-standard-chip.mdc-evolution-chip--with-leading-action .mdc-evolution-chip__action--primary {
  padding-left: 0;
  padding-right: 12px;
}
[dir=rtl] .mat-mdc-standard-chip.mdc-evolution-chip--with-leading-action .mdc-evolution-chip__action--primary {
  padding-left: 12px;
  padding-right: 0;
}
.mat-mdc-standard-chip.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--primary {
  padding-left: 12px;
  padding-right: 0;
}
[dir=rtl] .mat-mdc-standard-chip.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--primary {
  padding-left: 0;
  padding-right: 12px;
}
.mat-mdc-standard-chip.mdc-evolution-chip--with-leading-action.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--primary {
  padding-left: 0;
  padding-right: 0;
}
.mat-mdc-standard-chip.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--primary {
  padding-left: 0;
  padding-right: 0;
}
[dir=rtl] .mat-mdc-standard-chip.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--primary {
  padding-left: 0;
  padding-right: 0;
}
.mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic .mdc-evolution-chip__action--primary {
  padding-left: 0;
  padding-right: 12px;
}
[dir=rtl] .mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic .mdc-evolution-chip__action--primary {
  padding-left: 12px;
  padding-right: 0;
}
.mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--primary {
  padding-left: 0;
  padding-right: 0;
}
[dir=rtl] .mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--primary {
  padding-left: 0;
  padding-right: 0;
}

.mdc-evolution-chip__action--secondary {
  position: relative;
  overflow: visible;
}
.mat-mdc-standard-chip:not(.mdc-evolution-chip--disabled) .mdc-evolution-chip__action--secondary {
  color: var(--%NS%mat-chip-with-trailing-icon-trailing-icon-color, var(--%NS%mat-sys-on-surface-variant));
}
.mat-mdc-standard-chip.mdc-evolution-chip--disabled .mdc-evolution-chip__action--secondary {
  color: var(--%NS%mat-chip-with-trailing-icon-disabled-trailing-icon-color, var(--%NS%mat-sys-on-surface));
}
.mat-mdc-standard-chip.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--secondary, .mat-mdc-standard-chip.mdc-evolution-chip--with-leading-action .mdc-evolution-chip__action--secondary {
  padding-left: 8px;
  padding-right: 8px;
}
.mat-mdc-standard-chip.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--secondary, .mat-mdc-standard-chip.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-leading-action .mdc-evolution-chip__action--secondary {
  padding-left: 8px;
  padding-right: 8px;
}
.mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--secondary, .mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-leading-action .mdc-evolution-chip__action--secondary {
  padding-left: 8px;
  padding-right: 8px;
}
[dir=rtl] .mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__action--secondary, [dir=rtl] .mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-leading-action .mdc-evolution-chip__action--secondary {
  padding-left: 8px;
  padding-right: 8px;
}

.mdc-evolution-chip__text-label {
  -webkit-user-select: none;
  user-select: none;
  white-space: nowrap;
  text-overflow: ellipsis;
  overflow: hidden;
}
.mat-mdc-standard-chip .mdc-evolution-chip__text-label {
  font-family: var(--%NS%mat-chip-label-text-font, var(--%NS%mat-sys-label-large-font));
  line-height: var(--%NS%mat-chip-label-text-line-height, var(--%NS%mat-sys-label-large-line-height));
  font-size: var(--%NS%mat-chip-label-text-size, var(--%NS%mat-sys-label-large-size));
  font-weight: var(--%NS%mat-chip-label-text-weight, var(--%NS%mat-sys-label-large-weight));
  letter-spacing: var(--%NS%mat-chip-label-text-tracking, var(--%NS%mat-sys-label-large-tracking));
}
.mat-mdc-standard-chip:not(.mdc-evolution-chip--disabled) .mdc-evolution-chip__text-label {
  color: var(--%NS%mat-chip-label-text-color, var(--%NS%mat-sys-on-surface-variant));
}
.mat-mdc-standard-chip.mdc-evolution-chip--%NS%selected:not(.mdc-evolution-chip--disabled) .mdc-evolution-chip__text-label {
  color: var(--%NS%mat-chip-selected-label-text-color, var(--%NS%mat-sys-on-secondary-container));
}
.mat-mdc-standard-chip.mdc-evolution-chip--disabled .mdc-evolution-chip__text-label, .mat-mdc-standard-chip.mdc-evolution-chip--selected.mdc-evolution-chip--disabled .mdc-evolution-chip__text-label {
  color: var(--%NS%mat-chip-disabled-label-text-color, color-mix(in srgb, var(--%NS%mat-sys-on-surface) 38%, transparent));
}

.mdc-evolution-chip__graphic {
  align-items: center;
  display: inline-flex;
  justify-content: center;
  overflow: hidden;
  pointer-events: none;
  position: relative;
  flex: 1 0 auto;
}
.mat-mdc-standard-chip .mdc-evolution-chip__graphic {
  width: var(--%NS%mat-chip-with-avatar-avatar-size, 24px);
  height: var(--%NS%mat-chip-with-avatar-avatar-size, 24px);
  font-size: var(--%NS%mat-chip-with-avatar-avatar-size, 24px);
}
.mdc-evolution-chip--selecting .mdc-evolution-chip__graphic {
  transition: width 150ms 0ms cubic-bezier(0.4, 0, 0.2, 1);
}
.mdc-evolution-chip--%NS%selectable:not(.mdc-evolution-chip--selected):not(.mdc-evolution-chip--with-primary-icon) .mdc-evolution-chip__graphic {
  width: 0;
}
.mat-mdc-standard-chip.mdc-evolution-chip--with-primary-graphic .mdc-evolution-chip__graphic {
  padding-left: 6px;
  padding-right: 6px;
}
.mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic .mdc-evolution-chip__graphic {
  padding-left: 4px;
  padding-right: 8px;
}
[dir=rtl] .mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic .mdc-evolution-chip__graphic {
  padding-left: 8px;
  padding-right: 4px;
}
.mat-mdc-standard-chip.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__graphic {
  padding-left: 6px;
  padding-right: 6px;
}
.mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__graphic {
  padding-left: 4px;
  padding-right: 8px;
}
[dir=rtl] .mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-trailing-action .mdc-evolution-chip__graphic {
  padding-left: 8px;
  padding-right: 4px;
}
.mdc-evolution-chip--with-avatar.mdc-evolution-chip--with-primary-graphic.mdc-evolution-chip--with-leading-action .mdc-evolution-chip__graphic {
  padding-left: 0;
}

.mdc-evolution-chip__checkmark {
  position: absolute;
  opacity: 0;
  top: 50%;
  left: 50%;
  height: 20px;
  width: 20px;
}
.mat-mdc-standard-chip:not(.mdc-evolution-chip--disabled) .mdc-evolution-chip__checkmark {
  color: var(--%NS%mat-chip-with-icon-selected-icon-color, var(--%NS%mat-sys-on-secondary-container));
}
.mat-mdc-standard-chip.mdc-evolution-chip--disabled .mdc-evolution-chip__checkmark {
  color: var(--%NS%mat-chip-with-icon-disabled-icon-color, var(--%NS%mat-sys-on-surface));
}
.mdc-evolution-chip--selecting .mdc-evolution-chip__checkmark {
  transition: transform 150ms 0ms cubic-bezier(0.4, 0, 0.2, 1);
  transform: translate(-75%, -50%);
}
.mdc-evolution-chip--selected .mdc-evolution-chip__checkmark {
  transform: translate(-50%, -50%);
  opacity: 1;
}

.mdc-evolution-chip__checkmark-svg {
  display: block;
}

.mdc-evolution-chip__checkmark-path {
  stroke-width: 2px;
  stroke-dasharray: 29.7833385;
  stroke-dashoffset: 29.7833385;
  stroke: currentColor;
}
.mdc-evolution-chip--selecting .mdc-evolution-chip__checkmark-path {
  transition: stroke-dashoffset 150ms 45ms cubic-bezier(0.4, 0, 0.2, 1);
}
.mdc-evolution-chip--selected .mdc-evolution-chip__checkmark-path {
  stroke-dashoffset: 0;
}
@media (forced-colors: active) {
  .mdc-evolution-chip__checkmark-path {
    stroke: CanvasText !important;
  }
}

.mat-mdc-standard-chip .mdc-evolution-chip__icon--trailing {
  height: 18px;
  width: 18px;
  font-size: 18px;
}
.mdc-evolution-chip--disabled .mdc-evolution-chip__icon--trailing.mat-mdc-chip-remove {
  opacity: calc(var(--%NS%mat-chip-trailing-action-opacity, 1) * var(--%NS%mat-chip-with-trailing-icon-disabled-trailing-icon-opacity, 0.38));
}
.mdc-evolution-chip--disabled .mdc-evolution-chip__icon--trailing.mat-mdc-chip-remove:focus {
  opacity: calc(var(--%NS%mat-chip-trailing-action-focus-opacity, 1) * var(--%NS%mat-chip-with-trailing-icon-disabled-trailing-icon-opacity, 0.38));
}

.mat-mdc-standard-chip {
  border-radius: var(--%NS%mat-chip-container-shape-radius, 8px);
  height: var(--%NS%mat-chip-container-height, 32px);
}
.mat-mdc-standard-chip:not(.mdc-evolution-chip--disabled) {
  background-color: var(--%NS%mat-chip-elevated-container-color, transparent);
}
.mat-mdc-standard-chip.mdc-evolution-chip--disabled {
  background-color: var(--%NS%mat-chip-elevated-disabled-container-color);
}
.mat-mdc-standard-chip.mdc-evolution-chip--%NS%selected:not(.mdc-evolution-chip--disabled) {
  background-color: var(--%NS%mat-chip-elevated-selected-container-color, var(--%NS%mat-sys-secondary-container));
}
.mat-mdc-standard-chip.mdc-evolution-chip--selected.mdc-evolution-chip--disabled {
  background-color: var(--%NS%mat-chip-flat-disabled-selected-container-color, color-mix(in srgb, var(--%NS%mat-sys-on-surface) 12%, transparent));
}
@media (forced-colors: active) {
  .mat-mdc-standard-chip {
    outline: solid 1px;
  }
}

.mat-mdc-standard-chip .mdc-evolution-chip__icon--primary {
  border-radius: var(--%NS%mat-chip-with-avatar-avatar-shape-radius, 24px);
  width: var(--%NS%mat-chip-with-icon-icon-size, 18px);
  height: var(--%NS%mat-chip-with-icon-icon-size, 18px);
  font-size: var(--%NS%mat-chip-with-icon-icon-size, 18px);
}
.mdc-evolution-chip--selected .mdc-evolution-chip__icon--primary {
  opacity: 0;
}
.mat-mdc-standard-chip:not(.mdc-evolution-chip--disabled) .mdc-evolution-chip__icon--primary {
  color: var(--%NS%mat-chip-with-icon-icon-color, var(--%NS%mat-sys-on-surface-variant));
}
.mat-mdc-standard-chip.mdc-evolution-chip--disabled .mdc-evolution-chip__icon--primary {
  color: var(--%NS%mat-chip-with-icon-disabled-icon-color, var(--%NS%mat-sys-on-surface));
}

.mat-mdc-chip-highlighted {
  --%NS%mat-chip-with-icon-icon-color: var(--%NS%mat-chip-with-icon-selected-icon-color, var(--%NS%mat-sys-on-secondary-container));
  --%NS%mat-chip-elevated-container-color: var(--%NS%mat-chip-elevated-selected-container-color, var(--%NS%mat-sys-secondary-container));
  --%NS%mat-chip-label-text-color: var(--%NS%mat-chip-selected-label-text-color, var(--%NS%mat-sys-on-secondary-container));
  --%NS%mat-chip-outline-width: var(--%NS%mat-chip-flat-selected-outline-width, 0);
}

.mat-mdc-chip-focus-overlay {
  background: var(--%NS%mat-chip-focus-state-layer-color, var(--%NS%mat-sys-on-surface-variant));
}
.mat-mdc-chip-selected .mat-mdc-chip-focus-overlay, .mat-mdc-chip-highlighted .mat-mdc-chip-focus-overlay {
  background: var(--%NS%mat-chip-selected-focus-state-layer-color, var(--%NS%mat-sys-on-secondary-container));
}
.mat-mdc-chip:hover .mat-mdc-chip-focus-overlay {
  background: var(--%NS%mat-chip-hover-state-layer-color, var(--%NS%mat-sys-on-surface-variant));
  opacity: var(--%NS%mat-chip-hover-state-layer-opacity, var(--%NS%mat-sys-hover-state-layer-opacity));
}
.mat-mdc-chip-focus-overlay .mat-mdc-chip-selected:hover, .mat-mdc-chip-highlighted:hover .mat-mdc-chip-focus-overlay {
  background: var(--%NS%mat-chip-selected-hover-state-layer-color, var(--%NS%mat-sys-on-secondary-container));
  opacity: var(--%NS%mat-chip-selected-hover-state-layer-opacity, var(--%NS%mat-sys-hover-state-layer-opacity));
}
.mat-mdc-chip.cdk-focused .mat-mdc-chip-focus-overlay {
  background: var(--%NS%mat-chip-focus-state-layer-color, var(--%NS%mat-sys-on-surface-variant));
  opacity: var(--%NS%mat-chip-focus-state-layer-opacity, var(--%NS%mat-sys-focus-state-layer-opacity));
}
.mat-mdc-chip-selected.cdk-focused .mat-mdc-chip-focus-overlay, .mat-mdc-chip-highlighted.cdk-focused .mat-mdc-chip-focus-overlay {
  background: var(--%NS%mat-chip-selected-focus-state-layer-color, var(--%NS%mat-sys-on-secondary-container));
  opacity: var(--%NS%mat-chip-selected-focus-state-layer-opacity, var(--%NS%mat-sys-focus-state-layer-opacity));
}

.mdc-evolution-chip--%NS%disabled:not(.mdc-evolution-chip--selected) .mat-mdc-chip-avatar {
  opacity: var(--%NS%mat-chip-with-avatar-disabled-avatar-opacity, 0.38);
}

.mdc-evolution-chip--disabled .mdc-evolution-chip__icon--trailing {
  opacity: var(--%NS%mat-chip-with-trailing-icon-disabled-trailing-icon-opacity, 0.38);
}

.mdc-evolution-chip--disabled.mdc-evolution-chip--selected .mdc-evolution-chip__checkmark {
  opacity: var(--%NS%mat-chip-with-icon-disabled-icon-opacity, 0.38);
}

.mat-mdc-standard-chip.mdc-evolution-chip--disabled {
  opacity: var(--%NS%mat-chip-disabled-container-opacity, 1);
}
.mat-mdc-standard-chip.mdc-evolution-chip--selected .mdc-evolution-chip__icon--trailing, .mat-mdc-standard-chip.mat-mdc-chip-highlighted .mdc-evolution-chip__icon--trailing {
  color: var(--%NS%mat-chip-selected-trailing-icon-color, var(--%NS%mat-sys-on-secondary-container));
}
.mat-mdc-standard-chip.mdc-evolution-chip--selected.mdc-evolution-chip--disabled .mdc-evolution-chip__icon--trailing, .mat-mdc-standard-chip.mat-mdc-chip-highlighted.mdc-evolution-chip--disabled .mdc-evolution-chip__icon--trailing {
  color: var(--%NS%mat-chip-selected-disabled-trailing-icon-color, var(--%NS%mat-sys-on-surface));
}

.mat-mdc-chip-edit, .mat-mdc-chip-remove {
  opacity: var(--%NS%mat-chip-trailing-action-opacity, 1);
}
.mat-mdc-chip-edit:focus, .mat-mdc-chip-remove:focus {
  opacity: var(--%NS%mat-chip-trailing-action-focus-opacity, 1);
}
.mat-mdc-chip-edit::after, .mat-mdc-chip-remove::after {
  background-color: var(--%NS%mat-chip-trailing-action-state-layer-color, var(--%NS%mat-sys-on-surface-variant));
}
.mat-mdc-chip-edit:hover::after, .mat-mdc-chip-remove:hover::after {
  opacity: calc(var(--%NS%mat-chip-hover-state-layer-opacity, var(--%NS%mat-sys-hover-state-layer-opacity)) + var(--%NS%mat-chip-trailing-action-hover-state-layer-opacity, var(--%NS%mat-sys-hover-state-layer-opacity)));
}
.mat-mdc-chip-edit:focus::after, .mat-mdc-chip-remove:focus::after {
  opacity: calc(var(--%NS%mat-chip-hover-state-layer-opacity, var(--%NS%mat-sys-hover-state-layer-opacity)) + var(--%NS%mat-chip-trailing-action-focus-state-layer-opacity, var(--%NS%mat-sys-focus-state-layer-opacity)));
}

.mat-mdc-chip-selected .mat-mdc-chip-remove::after,
.mat-mdc-chip-highlighted .mat-mdc-chip-remove::after {
  background-color: var(--%NS%mat-chip-selected-trailing-action-state-layer-color, var(--%NS%mat-sys-on-secondary-container));
}

.mat-mdc-chip.cdk-focused .mat-mdc-chip-edit:focus::after, .mat-mdc-chip.cdk-focused .mat-mdc-chip-remove:focus::after {
  opacity: calc(var(--%NS%mat-chip-selected-focus-state-layer-opacity, var(--%NS%mat-sys-focus-state-layer-opacity)) + var(--%NS%mat-chip-trailing-action-focus-state-layer-opacity, var(--%NS%mat-sys-focus-state-layer-opacity)));
}
.mat-mdc-chip.cdk-focused .mat-mdc-chip-edit:hover::after, .mat-mdc-chip.cdk-focused .mat-mdc-chip-remove:hover::after {
  opacity: calc(var(--%NS%mat-chip-selected-focus-state-layer-opacity, var(--%NS%mat-sys-focus-state-layer-opacity)) + var(--%NS%mat-chip-trailing-action-hover-state-layer-opacity, var(--%NS%mat-sys-hover-state-layer-opacity)));
}

.mat-mdc-standard-chip {
  -webkit-tap-highlight-color: transparent;
}
.mat-mdc-standard-chip .mat-mdc-chip-graphic,
.mat-mdc-standard-chip .mat-mdc-chip-trailing-icon {
  box-sizing: content-box;
}
.mat-mdc-standard-chip._mat-animation-noopable,
.mat-mdc-standard-chip._mat-animation-noopable .mdc-evolution-chip__graphic,
.mat-mdc-standard-chip._mat-animation-noopable .mdc-evolution-chip__checkmark,
.mat-mdc-standard-chip._mat-animation-noopable .mdc-evolution-chip__checkmark-path {
  transition-duration: 1ms;
  animation-duration: 1ms;
}

.mat-mdc-chip-focus-overlay {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  pointer-events: none;
  opacity: 0;
  border-radius: inherit;
  transition: opacity 150ms linear;
}
._mat-animation-noopable .mat-mdc-chip-focus-overlay {
  transition: none;
}
.mat-mdc-basic-chip .mat-mdc-chip-focus-overlay {
  display: none;
}

.mat-mdc-chip .mat-ripple.mat-mdc-chip-ripple {
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  position: absolute;
  pointer-events: none;
  border-radius: inherit;
}

.mat-mdc-chip-avatar {
  text-align: center;
  line-height: 1;
  color: var(--%NS%mat-chip-with-icon-icon-color, currentColor);
}

.mat-mdc-chip {
  position: relative;
  z-index: 0;
}

.mat-mdc-chip-action-label {
  text-align: left;
  z-index: 1;
}
[dir=rtl] .mat-mdc-chip-action-label {
  text-align: right;
}
.mat-mdc-chip.mdc-evolution-chip--with-trailing-action .mat-mdc-chip-action-label {
  position: relative;
}
.mat-mdc-chip-action-label .mat-mdc-chip-primary-focus-indicator {
  position: absolute;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  pointer-events: none;
}
.mat-mdc-chip-action-label .mat-focus-indicator::before {
  margin: calc(calc(var(--%NS%mat-focus-indicator-border-width, 3px) + 2px) * -1);
}

.mat-mdc-chip-edit::before, .mat-mdc-chip-remove::before {
  margin: calc(var(--%NS%mat-focus-indicator-border-width, 3px) * -1);
  left: 8px;
  right: 8px;
}
.mat-mdc-chip-edit::after, .mat-mdc-chip-remove::after {
  content: "";
  display: block;
  opacity: 0;
  position: absolute;
  top: -3px;
  bottom: -3px;
  left: 5px;
  right: 5px;
  border-radius: 50%;
  box-sizing: border-box;
  padding: 12px;
  margin: -12px;
  background-clip: content-box;
}
.mat-mdc-chip-edit .mat-icon, .mat-mdc-chip-remove .mat-icon {
  width: 18px;
  height: 18px;
  font-size: 18px;
  box-sizing: content-box;
}

.mat-chip-edit-input {
  cursor: text;
  display: inline-block;
  color: inherit;
  outline: 0;
}

@media (forced-colors: active) {
  .mat-mdc-chip-selected:not(.mat-mdc-chip-multiple) {
    outline-width: 3px;
  }
}

.mat-mdc-chip-action:focus-visible .mat-focus-indicator::before {
  content: "";
}

.mdc-evolution-chip__icon, .mat-mdc-chip-edit .mat-icon, .mat-mdc-chip-remove .mat-icon {
  min-height: fit-content;
}

img.mdc-evolution-chip__icon {
  min-height: 0;
}
`],encapsulation:2})}return n})();var Rt=(()=>{class n{_elementRef=m(zt$1);_changeDetectorRef=m(Zd);_dir=m(le,{optional:!0});_lastDestroyedFocusedChipIndex=null;_keyManager;_destroyed=new de;_defaultRole=`presentation`;get chipFocusChanges(){return this._getChipStream(i=>i._onFocus)}get chipDestroyedChanges(){return this._getChipStream(i=>i.destroyed)}get chipRemovedChanges(){return this._getChipStream(i=>i.removed)}get disabled(){return this._disabled}set disabled(i){this._disabled=i,this._syncChipsState()}_disabled=!1;get empty(){return!this._chips||this._chips.length===0}get role(){return this._explicitRole?this._explicitRole:this.empty?null:this._defaultRole}tabIndex=0;set role(i){this._explicitRole=i}_explicitRole=null;get focused(){return this._hasFocusedChip()}_chips;_chipActions=new Zs;ngAfterViewInit(){this._setUpFocusManagement(),this._trackChipSetChanges(),this._trackDestroyedFocusedChip()}ngOnDestroy(){this._keyManager?.destroy(),this._chipActions.destroy(),this._destroyed.next(),this._destroyed.complete()}_hasFocusedChip(){return this._chips&&this._chips.some(i=>i._hasFocus())}_syncChipsState(){this._chips?.forEach(i=>{i._chipListDisabled=this._disabled,i._changeDetectorRef.markForCheck()})}focus(){}_handleKeydown(i){this._originatesFromChip(i)&&this._keyManager.onKeydown(i)}_isValidIndex(i){return i>=0&&i<this._chips.length}_allowFocusEscape(){let i=this._elementRef.nativeElement.tabIndex;i!==-1&&(this._elementRef.nativeElement.tabIndex=-1,setTimeout(()=>this._elementRef.nativeElement.tabIndex=i))}_getChipStream(i){return this._chips.changes.pipe(Qf(null),mc(()=>wE(...this._chips.map(i))))}_originatesFromChip(i){let e=i.target;for(;e&&e!==this._elementRef.nativeElement;){if(e.classList.contains(`mat-mdc-chip`))return!0;e=e.parentElement}return!1}_setUpFocusManagement(){this._chips.changes.pipe(Qf(this._chips)).subscribe(i=>{let e=[];i.forEach(t=>t._getActions().forEach(a=>e.push(a))),this._chipActions.reset(e),this._chipActions.notifyOnChanges()}),this._keyManager=new Dt$1(this._chipActions).withVerticalOrientation().withHorizontalOrientation(this._dir?this._dir.value:`ltr`).withHomeAndEnd().skipPredicate(i=>this._skipPredicate(i)),this.chipFocusChanges.pipe(NE(this._destroyed)).subscribe(({chip:i})=>{let e=i._getSourceAction(document.activeElement);e&&this._keyManager.updateActiveItem(e)}),this._dir?.change.pipe(NE(this._destroyed)).subscribe(i=>this._keyManager.withHorizontalOrientation(i))}_skipPredicate(i){return i.disabled}_trackChipSetChanges(){this._chips.changes.pipe(Qf(null),NE(this._destroyed)).subscribe(()=>{this.disabled&&Promise.resolve().then(()=>this._syncChipsState()),this._redirectDestroyedChipFocus()})}_trackDestroyedFocusedChip(){this.chipDestroyedChanges.pipe(NE(this._destroyed)).subscribe(i=>{let t=this._chips.toArray().indexOf(i.chip),a=i.chip._hasFocus(),c=i.chip._hadFocusOnRemove&&this._keyManager.activeItem&&i.chip._getActions().includes(this._keyManager.activeItem),w=a||c;this._isValidIndex(t)&&w&&(this._lastDestroyedFocusedChipIndex=t)})}_redirectDestroyedChipFocus(){if(this._lastDestroyedFocusedChipIndex!=null){if(this._chips.length){let i=Math.min(this._lastDestroyedFocusedChipIndex,this._chips.length-1),e=this._chips.toArray()[i];e.disabled?this._chips.length===1?this.focus():this._keyManager.setPreviousItemActive():e.focus()}else this.focus();this._lastDestroyedFocusedChipIndex=null}}static ɵfac=function(e){return new(e||n)};static ɵcmp=Jb({type:n,selectors:[[`mat-chip-set`]],contentQueries:function(e,t,a){if(e&1&&Fy(a,ei,5),e&2){let c;g_(c=m_())&&(t._chips=c)}},hostAttrs:[1,`mat-mdc-chip-set`,`mdc-evolution-chip-set`],hostVars:1,hostBindings:function(e,t){e&1&&Ry(`keydown`,function(c){return t._handleKeydown(c)}),e&2&&Iy(`role`,t.role)},inputs:{disabled:[2,`disabled`,`disabled`,QS],role:`role`,tabIndex:[2,`tabIndex`,`tabIndex`,i=>i==null?0:KS(i)]},ngContentSelectors:Vt,decls:2,vars:0,consts:[[`role`,`presentation`,1,`mdc-evolution-chip-set__chips`]],template:function(e,t){e&1&&(f_(),_d(0,`div`,0),p_(1),Sd())},styles:[`.mat-mdc-chip-set {
  display: flex;
}
.mat-mdc-chip-set:focus {
  outline: none;
}
.mat-mdc-chip-set .mdc-evolution-chip-set__chips {
  min-width: 100%;
  margin-left: -8px;
  margin-right: 0;
}
.mat-mdc-chip-set .mdc-evolution-chip {
  margin: 4px 0 4px 8px;
}
[dir=rtl] .mat-mdc-chip-set .mdc-evolution-chip-set__chips {
  margin-left: 0;
  margin-right: -8px;
}
[dir=rtl] .mat-mdc-chip-set .mdc-evolution-chip {
  margin-left: 0;
  margin-right: 8px;
}

.mdc-evolution-chip-set__chips {
  display: flex;
  flex-flow: wrap;
  min-width: 0;
}

.mat-mdc-chip-set-stacked {
  flex-direction: column;
  align-items: flex-start;
}
.mat-mdc-chip-set-stacked .mat-mdc-chip {
  width: 100%;
}
.mat-mdc-chip-set-stacked .mdc-evolution-chip__graphic {
  flex-grow: 0;
}
.mat-mdc-chip-set-stacked .mdc-evolution-chip__action--primary {
  flex-basis: 100%;
  justify-content: start;
}

input.mat-mdc-chip-input {
  flex: 1 0 150px;
  margin-left: 8px;
}
[dir=rtl] input.mat-mdc-chip-input {
  margin-left: 0;
  margin-right: 8px;
}
.mat-mdc-form-field:not(.mat-form-field-hide-placeholder) input.mat-mdc-chip-input::placeholder {
  opacity: 1;
}
.mat-mdc-form-field:not(.mat-form-field-hide-placeholder) input.mat-mdc-chip-input::-moz-placeholder {
  opacity: 1;
}
.mat-mdc-form-field:not(.mat-form-field-hide-placeholder) input.mat-mdc-chip-input::-webkit-input-placeholder {
  opacity: 1;
}
.mat-mdc-form-field:not(.mat-form-field-hide-placeholder) input.mat-mdc-chip-input:-ms-input-placeholder {
  opacity: 1;
}
.mat-mdc-chip-set + input.mat-mdc-chip-input {
  margin-left: 0;
  margin-right: 0;
}
`],encapsulation:2})}return n})();var At=(()=>{class n{static ɵfac=function(e){return new(e||n)};static ɵmod=cr({type:n});static ɵinj=nn({providers:[d,{provide:zt,useValue:{separatorKeyCodes:[13]}}],imports:[It$1,ve]})}return n})();var qt=[`installerWindow`];var Gt=(n,d)=>d.key;var Qt=(n,d)=>d.edition;function Kt(n,d){if(n&1){let i=i_();ia(0,`p`),$_(1),Td(),ia(2,`p`),$_(3,`Drag and drop files here or click to select`),Td(),ia(4,`input`,13,0),Ry(`change`,function(t){Zp(i);return Yp(l_(2).filesChanged(t))}),Td(),ia(6,`button`,14),Ry(`click`,function(){Zp(i);return Yp(v_(5).click())}),$_(7,` Select Files `),Td(),ia(8,`button`,15),Ry(`click`,function(){Zp(i);return Yp(l_(2).toggleShowExtensionCatalog())}),$_(9,` Extensions Catalog `),Td()}if(n&2){let i=l_(2);SC(),ev(i.dialogData.description),SC(3),wy(`accept`,i.dialogData.allowedFileTypes)(`multiple`,i.dialogData.multiple)}}function Ut(n,d){if(n&1&&Cy(0,`iframe`,16,1),n&2)wy(`src`,l_(3).remoteInstallerUrl,om)}function Wt(n,d){if(n&1&&YT(0,Ut,2,1,`iframe`,16),n&2)QT(l_(2).ready?0:-1)}function $t(n,d){if(n&1){let i=i_();ia(0,`div`,12),Ry(`filesDropped`,function(t){Zp(i);return Yp(l_().filesDropped(t))}),YT(1,Kt,10,3),YT(2,Wt,1,1),Td()}if(n&2){let i=l_();SC(),QT(i.showExtensionCatalog()?-1:1),SC(),QT(i.showExtensionCatalog()?2:-1)}}function Zt(n,d){n&1&&(ia(0,`div`,6),Cy(1,`mat-spinner`,17),ia(2,`p`),$_(3,`Running preflight check...`),Td()())}function Jt(n,d){if(n&1){let i=i_();ia(0,`div`,7)(1,`h3`),$_(2,`Error`),Td(),ia(3,`p`),$_(4),Td(),ia(5,`button`,18),Ry(`click`,function(){Zp(i);return Yp(l_().cancelPreflight())}),$_(6,` Back `),Td()()}if(n&2){let i=l_();SC(4),ev(i.preflightError())}}function Yt(n,d){if(n&1&&($_(0,` File: `),ia(1,`em`),$_(2),Td()),n&2){let i=l_(2);SC(2),ev(i.currentFile?.name)}}function Xt(n,d){if(n&1&&($_(0,` Remote Source: `),ia(1,`em`)(2,`a`,27),$_(3),Td()()),n&2){let i=l_(2);SC(2),wy(`href`,i.preflightSource,rm),SC(),xd(` `,i.preflightSource.toString().substring(0,50),`... `)}}function ie(n,d){if(n&1&&(ia(0,`mat-chip`),$_(1),Td()),n&2){let i=d.$implicit;Gy(`disabled`,!i.value),SC(),xd(` `,i.key.split(`Inside`).join(``),` `)}}function te(n,d){if(n&1&&(ia(0,`mat-accordion`)(1,`mat-expansion-panel`,19)(2,`mat-expansion-panel-header`)(3,`mat-panel-title`),$_(4,`Extension Details`),Td()(),ia(5,`div`,20),YT(6,Yt,3,1)(7,Xt,4,2),ia(8,`h4`),$_(9),Td(),ia(10,`span`),$_(11),Td(),Cy(12,`br`),ia(13,`div`,21)(14,`div`,22)(15,`span`,23),$_(16,`File Count:`),Td(),ia(17,`span`,24),$_(18),Td()(),ia(19,`div`,22)(20,`span`,23),$_(21,`Supports Editions:`),Td(),ia(22,`span`,24),$_(23),Td()()(),ia(24,`div`,25)(25,`h4`),$_(26,`Features used by this Extension`),Td(),ia(27,`mat-chip-set`),JT(28,ie,2,3,`mat-chip`,26,Gt),lS(30,`keyvalue`),Td()()()()()),n&2){let i=l_();SC(),wy(`expanded`,!0),SC(5),QT(i.isFileSource?6:7),SC(3),xd(``,i.extension().name,` Details`),SC(2),xd(`Version `,i.extension().version),SC(7),xd(` `,i.extension().fileCount,` `),SC(5),xd(` `,i.extension().editionsSupported?`Yes`:`No`,` `),SC(5),e_(fS(30,6,i.extension().features))}}function ee(n,d){if(n&1){let i=i_();ia(0,`mat-checkbox`,30),Ry(`change`,function(t){let a=Zp(i).$implicit;return Yp(l_(2).onEditionToggle(a.edition,t.checked))}),$_(1),Td()}if(n&2){let i=d.$implicit;wy(`checked`,l_(2).selectedEditions().includes(i.edition)),SC(),xd(` `,i.edition||`Default`,` `)}}function ne(n,d){if(n&1&&(ia(0,`div`,8)(1,`h4`),$_(2,`Editions to install this Extension to`),Td(),ia(3,`div`,28),JT(4,ee,2,2,`mat-checkbox`,29,Qt),Td()()),n&2){let i=l_();SC(4),e_(i.editions())}}function ae(n,d){if(n&1){let i=i_();ia(0,`div`,9)(1,`h4`),$_(2,`Caution: Import causes an Overwrite`),Td(),ia(3,`p`),$_(4,` Upgrading this extension will overwrite your changes. You must enable Force Install to proceed. `),Td(),ia(5,`mat-slide-toggle`,31),sv(`ngModelChange`,function(t){Zp(i);let a=l_();return Z_(a.forceInstall,t)||(a.forceInstall=t),Yp(t)}),$_(6,` Force / Overwrite `),Td(),db(),Td()}if(n&2){let i=l_();SC(5),iv(`ngModel`,i.forceInstall),pb()}}function oe(n,d){n&1&&(ia(0,`div`,9)(1,`h4`),$_(2,`Upgrade Available`),Td(),ia(3,`p`),$_(4,` This extension is already installed, but no local changes were detected. Force Install is `),ia(5,`strong`),$_(6,`not required`),Td(),$_(7,`. `),Td()())}function ce(n,d){n&1&&(ia(0,`div`,10),Cy(1,`mat-spinner`,17),ia(2,`p`),$_(3,`Installing extension...`),Td()())}function re(n,d){if(n&1){let i=i_();ia(0,`button`,32),Ry(`click`,function(){Zp(i);return Yp(l_().cancelPreflight())}),$_(1,` Cancel `),Td(),ia(2,`button`,33),Ry(`click`,function(){Zp(i);return Yp(l_().install())}),$_(3,` Install `),Td()}if(n&2){let i=l_();wy(`disabled`,i.isInstalling()),SC(2),wy(`disabled`,!i.canInstall())}}var wn=(()=>{class n extends c{#i;#e;#n;#a;#t;constructor(i,e,t){super(),this.dialogData=i,this.dialogRef=e,this.sanitizer=t,this.#i=Tt(b),this.#e=Tt(A),this.#n=Tt(an),this.#a=m(Qt$1),this.uploadType=je$1.Extension,this.importModeValues=j$1,this.fallbackEditions=[`staging`,`live`],this.extension=st(null),this.isLoadingPreflight=st(!1),this.isInstalling=st(!1),this.preflightError=st(null),this.editions=st([]),this.showExtensionCatalog=st(!1),this.forceInstall=!1,this.alreadyInstalled=xt(()=>{let a=this.extension();return!a||!this.selectedEditions().length?!1:a.editions?.some(c=>this.selectedEditions().includes(c.edition??``)&&c.isInstalled)??!1}),this.hasChanges=xt(()=>{let a=this.extension();return!a||!this.selectedEditions().length?!1:a.editions?.some(c=>this.selectedEditions().includes(c.edition??``)&&c.hasFileChanges)??!1}),this.selectedEditions=st([]),this.preflightSource=null,this.urlChangeImportMode=``,this.ready=!1,this.#t=!1,this.importForm=this.#n.group({importMode:[this.importModeValues.importOriginal,fe.required],name:[``]}),this.context=m(xl),this.messages=new V$1(this.context.moduleId),i.title??=`Import Extension`,i.description??=`Select Extension folder from your computer to import.`,i.allowedFileTypes??=`zip`,i.multiple??=!0,i.file&&this.runFilePreflight(i.file),this.subscriptions.add(this.#e.settings$.subscribe(a=>{this.settings=a,this.urlChangeImportMode=a.remoteUrl,this.updateRemoteUrl(),this.ready=!0}))}ngOnInit(){this.#o(),this.#e.loadGettingStarted(!1),this.importForm.get(`importMode`)?.valueChanges.subscribe(()=>{this.updateRemoteUrl()}),this.subscriptions.add(this.messages.messages$.pipe(Kt$1(i=>i.action===`specs`)).subscribe(()=>{})),this.subscriptions.add(fc(window,`message`).pipe(Kt$1(()=>!!this.installerWindow)).subscribe(i=>{let e;try{e=typeof i.data==`string`?JSON.parse(i.data):i.data}catch(t){return}if(this.showExtensionCatalog()&&e.action===`install`&&e.packages){let t=Object.values(e.packages)[0];if(!t?.url||this.#t)return;this.#t=!0,this.handleRemotePreflight(t.url)}}))}get isFileSource(){return this.preflightSource instanceof File}get currentFile(){return this.isFileSource?this.preflightSource:null}updateRemoteUrl(){this.remoteInstallerUrl=this.sanitizer.bypassSecurityTrustResourceUrl(this.buildRemoteUrl(this.urlChangeImportMode,{view:`app-extensions`,selectOnlyMode:`true`,excludeInstalled:`true`}))}buildRemoteUrl(i,e={}){let[t,a]=i.split(`?`),c=new URLSearchParams(a||``);for(let w in e)c.set(w,e[w]);return`${t}?${c.toString()}`}#o(){this.dialogRef.keydownEvents().subscribe(i=>{y(i)&&(i.preventDefault(),this.install())})}filesDropped(i){this.runFilePreflight(i[0])}filesChanged(i){let e=i.target.files[0];this.runFilePreflight(e)}toggleShowExtensionCatalog(){this.showExtensionCatalog.set(!this.showExtensionCatalog())}closeDialog(){this.dialogRef.close(!1)}cancelPreflight(){this.extension.set(null),this.preflightSource=null,this.selectedEditions.set([]),this.preflightError.set(null),this.forceInstall=!1,this.#t=!1}runFilePreflight(i){this.isLoadingPreflight.set(!0),this.#i.installPreflightExtension([i]).pipe(Sn(1)).subscribe({next:e=>this.handlePreflightResult(e.extensions[0],i),error:e=>this.handlePreflightError(e)})}handleRemotePreflight(i){this.showExtensionCatalog.set(!1),this.isLoadingPreflight.set(!0),this.#i.installPreflightExtensionFromUrl(i).pipe(Sn(1)).subscribe({next:e=>this.handlePreflightResult(e.extensions[0],i),error:e=>this.handlePreflightError(e)})}handlePreflightResult(i,e){this.isLoadingPreflight.set(!1),this.preflightError.set(null),this.extension.set(i),this.preflightSource=e,this.forceInstall=!1;let t=i.editions?.length>0?i.editions:this.fallbackEditions.map(a=>({edition:a}));this.editions.set(t),this.selectedEditions.set([this.editions()[0].edition??``])}handlePreflightError(i){this.isLoadingPreflight.set(!1),this.extension.set(null),this.preflightSource=null,this.preflightError.set(i?.message||`Preflight check failed`),this.#t=!1}canInstall(){return!(!this.preflightSource||this.preflightError()||this.isInstalling()||this.isLoadingPreflight()||!this.extension()||this.selectedEditions().length===0)}install(){if(!this.canInstall())return;if(this.hasChanges()&&this.alreadyInstalled()&&!this.forceInstall){this.#a.open(`Installation requires force. Please enable "Force Install" and try again.`,`OK`,{duration:1e4});return}this.isInstalling.set(!0);let i=this.forceInstall||!this.hasChanges()&&this.alreadyInstalled(),e,t=this.selectedEditions().length?this.selectedEditions().join(`,`):void 0;if(this.preflightSource instanceof File)e=this.#i.uploadExtensions(this.preflightSource,t,i);else if(typeof this.preflightSource==`string`)e=this.#i.installRemoteExtension(this.preflightSource,t,i);else return;e.pipe(Sn(1)).subscribe({next:()=>{this.isInstalling.set(!1),this.dialogRef.close(!0)},error:a=>{this.isInstalling.set(!1),this.preflightError.set(a?.message||`Installation failed`),console.error(`Installation error:`,a)}})}onEditionToggle(i,e){e?this.selectedEditions().includes(i)||this.selectedEditions.set([...this.selectedEditions(),i]):this.selectedEditions.set(this.selectedEditions().filter(t=>t!==i))}static{this.ɵfac=function(e){return new(e||n)(he(qe),he(V),he($N))}}static{this.ɵcmp=Jb({type:n,selectors:[[`ng-component`]],viewQuery:function(e,t){if(e&1&&Ly(qt,5),e&2){let a;g_(a=m_())&&(t.installerWindow=a.first)}},features:[sy],decls:14,vars:10,consts:[[`fileInput`,``],[`installerWindow`,``],[1,`eav-dialog`],[3,`close`],[1,`eav-dialog-content`],[`appDragAndDrop`,``,1,`file-drop-zone`],[1,`preflight-loading`],[1,`preflight-error`],[1,`ext-editions`],[1,`install-options`,2,`margin-top`,`16px`],[1,`installing`],[`align`,`end`],[`appDragAndDrop`,``,1,`file-drop-zone`,3,`filesDropped`],[`type`,`file`,2,`display`,`none`,3,`change`,`accept`,`multiple`],[`mat-raised-button`,``,`color`,`primary`,3,`click`],[`mat-raised-button`,``,`tippy`,`Open extension catalog`,1,`eav-app-catalog-button`,`eav-action-button`,3,`click`],[1,`eav-app-catalog-iframe`,3,`src`],[`diameter`,`40`],[`mat-stroked-button`,``,3,`click`],[3,`expanded`],[1,`unified-preflight-details`],[1,`ext-info`],[1,`info-row`],[1,`label`],[1,`value`],[1,`ext-features`],[3,`disabled`],[3,`href`],[1,`edition-selector`],[`color`,`accent`,3,`checked`],[`color`,`accent`,3,`change`,`checked`],[`color`,`primary`,3,`ngModelChange`,`ngModel`],[`mat-stroked-button`,``,3,`click`,`disabled`],[`mat-raised-button`,``,1,`eav-action-button`,3,`click`,`disabled`]],template:function(e,t){e&1&&(ia(0,`div`,2)(1,`app-dialog-header`,3),Ry(`close`,function(){return t.closeDialog()}),$_(2),Td(),ia(3,`div`,4),YT(4,$t,3,2,`div`,5),YT(5,Zt,4,0,`div`,6),YT(6,Jt,7,1,`div`,7),YT(7,te,31,8,`mat-accordion`),YT(8,ne,6,0,`div`,8),YT(9,ae,7,1,`div`,9),YT(10,oe,8,0,`div`,9),YT(11,ce,4,0,`div`,10),Td(),ia(12,`mat-dialog-actions`,11),YT(13,re,4,2),Td()()),e&2&&(SC(2),xd(` `,t.dialogData.title,` `),SC(2),QT(!t.extension()&&!t.isLoadingPreflight()?4:-1),SC(),QT(t.isLoadingPreflight()?5:-1),SC(),QT(t.preflightError()?6:-1),SC(),QT(t.extension()&&!t.isLoadingPreflight()?7:-1),SC(),QT(t.extension()?.editionsSupported&&t.editions()?.length>1?8:-1),SC(),QT(t.alreadyInstalled()&&t.hasChanges()?9:-1),SC(),QT(t.alreadyInstalled()&&!t.hasChanges()?10:-1),SC(),QT(t.isInstalling()?11:-1),SC(2),QT(t.extension()&&!t.isLoadingPreflight()?13:-1))},dependencies:[rf,oe$1,ne$1,J,Z$1,Vt$1,jt$1,At,ei,Rt,ti,Pt$1,pe,ce$1,Pn,wn$1,tn,I$1,yt,rp,we,te$1,fn,xn,Oe,Fe,un,z,xM],styles:[`.eav-app-catalog-button[_ngcontent-%COMP%]{margin-left:12px}.eav-app-catalog-iframe[_ngcontent-%COMP%]{margin-top:16px;width:100%;height:500px;border:none}.eav-progress[_ngcontent-%COMP%]{position:absolute;inset:0;background:#fffc;display:flex;flex-direction:column;justify-content:center;text-align:center}.eav-progress[_ngcontent-%COMP%]   mat-progress-spinner[_ngcontent-%COMP%]{margin:0 auto}.eav-progress[_ngcontent-%COMP%]   span[_ngcontent-%COMP%]{line-height:48px}.disabled[_ngcontent-%COMP%]{opacity:.25;pointer-events:none}mat-slide-toggle[_ngcontent-%COMP%]{margin-left:5px}
/*# sourceMappingURL=import-extension-WEI5S7H7.css.map */`]})}}return n})();export{wn as i,Rt as n,ei as r,At as t};
//# debugId=eca53869-2ba7-5994-99eb-f616f9580d48
//# sourceMappingURL=https://sources.2sxc.org/22.01.00/dist/ng-edit/chunk-29_8nKHS.js.map