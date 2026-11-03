import{$t as Zp,At as Td,B as JT,Ci as xd,Dn as ev,E as Ey,Ei as xy,G as KS,Gn as ia,Gt as YT,L as Iy,N as I,Sn as de,St as SC,Tn as e_,Ut as XT,V as Jb,Vn as he,Wn as i_,Yr as st,Zn as jt$1,Zt as Zd,_n as cr,bi as wy,di as v_,g as Cy,gt as QT,hn as ch,hr as nn,ht as QS,ir as m,jn as fo,ln as ah,lt as Ng,nr as l_,ot as N,pt as Pe,qt as Yp,r as $_,xt as Ry}from"./chunk-DtJSyZhi.js";import"./chunk-C4dVjHVF.js";import{r as Y}from"./chunk-B9PE1Af2.js";import"./chunk-CmUuQ47m.js";import{B as be}from"./chunk-D18RyqxO.js";import{c as Tt$1,m as oe,r as Ht$1}from"./chunk-CwWNeagF.js";import{t as Qt}from"./chunk-T2ghyHO0.js";import{i as l,r as d}from"./chunk-kNQ1WOz-.js";import{b as ti,d as yt$1,o as W,v as je}from"./main-NXRXUN76.js";import"./chunk-CdiB5whn.js";import"./chunk-D4NcDKI9.js";import"./chunk-CJWIt6fL.js";import"./chunk-BRhjinLo.js";import{t as z}from"./chunk-CpDLQUtk.js";import{i as V$1}from"./chunk-Bo0R4CmS.js";import{t as S}from"./chunk-DtGJjCwE.js";import"./chunk-DnwOxnK32.js";import"./chunk-CLnQcnI42.js";import{i as Ae,s as W$1}from"./chunk-BltJQS3W2.js";import"./chunk-DG5aRMVO2.js";import{n as Nt$1,r as Pt$1}from"./chunk-Ce8KoDwL2.js";import{o as xn,r as fn}from"./chunk-lIKt1-2j2.js";import{t as Ct$1}from"./chunk-B9HY_jEv.js";import{n as Ce,r as Pe$1,t as Ae$1}from"./chunk-Gqxm6Juw.js";function yt(e,r){if(e&1&&(ia(0,`mat-option`,17),$_(1),Td()),e&2){let t=r.$implicit;wy(`value`,t),SC(),xd(` `,t,` `)}}function Pt(e,r){if(e&1){let t=i_();ia(0,`mat-form-field`,14)(1,`mat-select`,16,0),Ry(`selectionChange`,function(i){Zp(t);return Yp(l_(2)._changePageSize(i.value))}),JT(3,yt,2,2,`mat-option`,17,XT),Td(),ia(5,`div`,18),Ry(`click`,function(){Zp(t);return Yp(v_(2).open())}),Td()()}if(e&2){let t=l_(2);wy(`appearance`,t._formFieldAppearance)(`color`,t.color),SC(),wy(`value`,t.pageSize)(`disabled`,t.disabled),Ey(`aria-labelledby`,t._pageSizeLabelId),wy(`panelClass`,t.selectConfig.panelClass||``)(`disableOptionCentering`,t.selectConfig.disableOptionCentering),SC(2),e_(t._displayedPageSizeOptions)}}function zt(e,r){if(e&1&&(ia(0,`div`,15),$_(1),Td()),e&2){let t=l_(2);SC(),ev(t.pageSize)}}function It(e,r){if(e&1&&(ia(0,`div`,3)(1,`div`,13),$_(2),Td(),YT(3,Pt,6,7,`mat-form-field`,14),YT(4,zt,2,1,`div`,15),Td()),e&2){let t=l_();SC(),Iy(`id`,t._pageSizeLabelId),SC(),xd(` `,t._intl.itemsPerPageLabel,` `),SC(),QT(t._displayedPageSizeOptions.length>1?3:-1),SC(),QT(t._displayedPageSizeOptions.length<=1?4:-1)}}function Mt(e,r){if(e&1){let t=i_();ia(0,`button`,19),Ry(`click`,function(){Zp(t);let i=l_();return Yp(i._buttonClicked(0,i._previousButtonsDisabled()))}),ah(),ia(1,`svg`,8),Cy(2,`path`,20),Td()()}if(e&2){let t=l_();wy(`matTooltip`,t._intl.firstPageLabel)(`matTooltipDisabled`,t._previousButtonsDisabled())(`disabled`,t._previousButtonsDisabled())(`tabindex`,t._previousButtonsDisabled()?-1:null),Iy(`aria-label`,t._intl.firstPageLabel)}}function wt(e,r){if(e&1){let t=i_();ia(0,`button`,21),Ry(`click`,function(){Zp(t);let i=l_();return Yp(i._buttonClicked(i.getNumberOfPages()-1,i._nextButtonsDisabled()))}),ah(),ia(1,`svg`,8),Cy(2,`path`,22),Td()()}if(e&2){let t=l_();wy(`matTooltip`,t._intl.lastPageLabel)(`matTooltipDisabled`,t._nextButtonsDisabled())(`disabled`,t._nextButtonsDisabled())(`tabindex`,t._nextButtonsDisabled()?-1:null),Iy(`aria-label`,t._intl.lastPageLabel)}}var Ot=(()=>{class e{changes=new de;itemsPerPageLabel=`Items per page:`;nextPageLabel=`Next page`;previousPageLabel=`Previous page`;firstPageLabel=`First page`;lastPageLabel=`Last page`;getRangeLabel=(t,a,i)=>{if(i==0||a==0)return`0 of ${i}`;i=Math.max(i,0);let s=t*a,v=s<i?Math.min(s+a,i):s+a;return`${s+1} \u2013 ${v} of ${i}`};static ɵfac=function(a){return new(a||e)};static ɵprov=Pe({token:e,factory:e.ɵfac})}return e})();var Tt=50;var Dt=new I(`MAT_PAGINATOR_DEFAULT_OPTIONS`);var V=(()=>{class e{_intl=m(Ot);_changeDetectorRef=m(Zd);_formFieldAppearance;_pageSizeLabelId=m(be).getId(`mat-paginator-page-size-label-`);_intlChanges;_isInitialized=!1;_initializedStream=new fo(1);color;get pageIndex(){return this._pageIndex}set pageIndex(t){this._pageIndex=Math.max(t||0,0),this._changeDetectorRef.markForCheck()}_pageIndex=0;get length(){return this._length}set length(t){this._length=t||0,this._changeDetectorRef.markForCheck()}_length=0;get pageSize(){return this._pageSize}set pageSize(t){this._pageSize=Math.max(t||0,0),this._updateDisplayedPageSizeOptions()}_pageSize;get pageSizeOptions(){return this._pageSizeOptions}set pageSizeOptions(t){this._pageSizeOptions=(t||[]).map(a=>KS(a,0)),this._updateDisplayedPageSizeOptions()}_pageSizeOptions=[];hidePageSize=!1;showFirstLastButtons=!1;selectConfig={};disabled=!1;page=new jt$1;_displayedPageSizeOptions;initialized=this._initializedStream;constructor(){let t=this._intl,a=m(Dt,{optional:!0});if(this._intlChanges=t.changes.subscribe(()=>this._changeDetectorRef.markForCheck()),a){let{pageSize:i,pageSizeOptions:s,hidePageSize:v,showFirstLastButtons:x}=a;i!=null&&(this._pageSize=i),s!=null&&(this._pageSizeOptions=s),v!=null&&(this.hidePageSize=v),x!=null&&(this.showFirstLastButtons=x)}this._formFieldAppearance=a?.formFieldAppearance||`outline`}ngOnInit(){this._isInitialized=!0,this._updateDisplayedPageSizeOptions(),this._initializedStream.next()}ngOnDestroy(){this._initializedStream.complete(),this._intlChanges.unsubscribe()}nextPage(){this.hasNextPage()&&this._navigate(this.pageIndex+1)}previousPage(){this.hasPreviousPage()&&this._navigate(this.pageIndex-1)}firstPage(){this.hasPreviousPage()&&this._navigate(0)}lastPage(){this.hasNextPage()&&this._navigate(this.getNumberOfPages()-1)}hasPreviousPage(){return this.pageIndex>=1&&this.pageSize!=0}hasNextPage(){let t=this.getNumberOfPages()-1;return this.pageIndex<t&&this.pageSize!=0}getNumberOfPages(){return this.pageSize?Math.ceil(this.length/this.pageSize):0}_changePageSize(t){let a=this.pageIndex*this.pageSize,i=this.pageIndex;this.pageIndex=Math.floor(a/t)||0,this.pageSize=t,this._emitPageEvent(i)}_nextButtonsDisabled(){return this.disabled||!this.hasNextPage()}_previousButtonsDisabled(){return this.disabled||!this.hasPreviousPage()}_updateDisplayedPageSizeOptions(){this._isInitialized&&(this.pageSize||(this._pageSize=this.pageSizeOptions.length!=0?this.pageSizeOptions[0]:Tt),this._displayedPageSizeOptions=this.pageSizeOptions.slice(),this._displayedPageSizeOptions.indexOf(this.pageSize)===-1&&this._displayedPageSizeOptions.push(this.pageSize),this._displayedPageSizeOptions.sort((t,a)=>t-a),this._changeDetectorRef.markForCheck())}_emitPageEvent(t){this.page.emit({previousPageIndex:t,pageIndex:this.pageIndex,pageSize:this.pageSize,length:this.length})}_navigate(t){let a=this.pageIndex;t!==a&&(this.pageIndex=t,this._emitPageEvent(a))}_buttonClicked(t,a){a||this._navigate(t)}static ɵfac=function(a){return new(a||e)};static ɵcmp=Jb({type:e,selectors:[[`mat-paginator`]],hostAttrs:[`role`,`group`,1,`mat-mdc-paginator`],inputs:{color:`color`,pageIndex:[2,`pageIndex`,`pageIndex`,KS],length:[2,`length`,`length`,KS],pageSize:[2,`pageSize`,`pageSize`,KS],pageSizeOptions:`pageSizeOptions`,hidePageSize:[2,`hidePageSize`,`hidePageSize`,QS],showFirstLastButtons:[2,`showFirstLastButtons`,`showFirstLastButtons`,QS],selectConfig:`selectConfig`,disabled:[2,`disabled`,`disabled`,QS]},outputs:{page:`page`},exportAs:[`matPaginator`],decls:14,vars:14,consts:[[`selectRef`,``],[1,`mat-mdc-paginator-outer-container`],[1,`mat-mdc-paginator-container`],[1,`mat-mdc-paginator-page-size`],[1,`mat-mdc-paginator-range-actions`],[`aria-atomic`,`true`,`aria-live`,`polite`,`role`,`status`,1,`mat-mdc-paginator-range-label`],[`matIconButton`,``,`type`,`button`,`matTooltipPosition`,`above`,`disabledInteractive`,``,1,`mat-mdc-paginator-navigation-first`,3,`matTooltip`,`matTooltipDisabled`,`disabled`,`tabindex`],[`matIconButton`,``,`type`,`button`,`matTooltipPosition`,`above`,`disabledInteractive`,``,1,`mat-mdc-paginator-navigation-previous`,3,`click`,`matTooltip`,`matTooltipDisabled`,`disabled`,`tabindex`],[`viewBox`,`0 0 24 24`,`focusable`,`false`,`aria-hidden`,`true`,1,`mat-mdc-paginator-icon`],[`d`,`M15.41 7.41L14 6l-6 6 6 6 1.41-1.41L10.83 12z`],[`matIconButton`,``,`type`,`button`,`matTooltipPosition`,`above`,`disabledInteractive`,``,1,`mat-mdc-paginator-navigation-next`,3,`click`,`matTooltip`,`matTooltipDisabled`,`disabled`,`tabindex`],[`d`,`M10 6L8.59 7.41 13.17 12l-4.58 4.59L10 18l6-6z`],[`matIconButton`,``,`type`,`button`,`matTooltipPosition`,`above`,`disabledInteractive`,``,1,`mat-mdc-paginator-navigation-last`,3,`matTooltip`,`matTooltipDisabled`,`disabled`,`tabindex`],[`aria-hidden`,`true`,1,`mat-mdc-paginator-page-size-label`],[1,`mat-mdc-paginator-page-size-select`,3,`appearance`,`color`],[1,`mat-mdc-paginator-page-size-value`],[`hideSingleSelectionIndicator`,``,3,`selectionChange`,`value`,`disabled`,`aria-labelledby`,`panelClass`,`disableOptionCentering`],[3,`value`],[1,`mat-mdc-paginator-touch-target`,3,`click`],[`matIconButton`,``,`type`,`button`,`matTooltipPosition`,`above`,`disabledInteractive`,``,1,`mat-mdc-paginator-navigation-first`,3,`click`,`matTooltip`,`matTooltipDisabled`,`disabled`,`tabindex`],[`d`,`M18.41 16.59L13.82 12l4.59-4.59L17 6l-6 6 6 6zM6 6h2v12H6z`],[`matIconButton`,``,`type`,`button`,`matTooltipPosition`,`above`,`disabledInteractive`,``,1,`mat-mdc-paginator-navigation-last`,3,`click`,`matTooltip`,`matTooltipDisabled`,`disabled`,`tabindex`],[`d`,`M5.59 7.41L10.18 12l-4.59 4.59L7 18l6-6-6-6zM16 6h2v12h-2z`]],template:function(a,i){a&1&&(ia(0,`div`,1)(1,`div`,2),YT(2,It,5,4,`div`,3),ia(3,`div`,4)(4,`div`,5),$_(5),Td(),YT(6,Mt,3,5,`button`,6),ia(7,`button`,7),Ry(`click`,function(){return i._buttonClicked(i.pageIndex-1,i._previousButtonsDisabled())}),ah(),ia(8,`svg`,8),Cy(9,`path`,9),Td()(),ch(),ia(10,`button`,10),Ry(`click`,function(){return i._buttonClicked(i.pageIndex+1,i._nextButtonsDisabled())}),ah(),ia(11,`svg`,8),Cy(12,`path`,11),Td()(),YT(13,wt,3,5,`button`,12),Td()()()),a&2&&(SC(2),QT(i.hidePageSize?-1:2),SC(3),xd(` `,i._intl.getRangeLabel(i.pageIndex,i.pageSize,i.length),` `),SC(),QT(i.showFirstLastButtons?6:-1),SC(),wy(`matTooltip`,i._intl.previousPageLabel)(`matTooltipDisabled`,i._previousButtonsDisabled())(`disabled`,i._previousButtonsDisabled())(`tabindex`,i._previousButtonsDisabled()?-1:null),Iy(`aria-label`,i._intl.previousPageLabel),SC(3),wy(`matTooltip`,i._intl.nextPageLabel)(`matTooltipDisabled`,i._nextButtonsDisabled())(`disabled`,i._nextButtonsDisabled())(`tabindex`,i._nextButtonsDisabled()?-1:null),Iy(`aria-label`,i._intl.nextPageLabel),SC(3),QT(i.showFirstLastButtons?13:-1))},dependencies:[je,Nt$1,W$1,Ht$1,Ct$1],styles:[`.mat-mdc-paginator {
  display: block;
  -moz-osx-font-smoothing: grayscale;
  -webkit-font-smoothing: antialiased;
  color: var(--%NS%mat-paginator-container-text-color, var(--%NS%mat-sys-on-surface));
  background-color: var(--%NS%mat-paginator-container-background-color, var(--%NS%mat-sys-surface));
  font-family: var(--%NS%mat-paginator-container-text-font, var(--%NS%mat-sys-body-small-font));
  line-height: var(--%NS%mat-paginator-container-text-line-height, var(--%NS%mat-sys-body-small-line-height));
  font-size: var(--%NS%mat-paginator-container-text-size, var(--%NS%mat-sys-body-small-size));
  font-weight: var(--%NS%mat-paginator-container-text-weight, var(--%NS%mat-sys-body-small-weight));
  letter-spacing: var(--%NS%mat-paginator-container-text-tracking, var(--%NS%mat-sys-body-small-tracking));
  --%NS%mat-form-field-container-height: var(--%NS%mat-paginator-form-field-container-height, 40px);
  --%NS%mat-form-field-container-vertical-padding: var(--%NS%mat-paginator-form-field-container-vertical-padding, 8px);
}
.mat-mdc-paginator .mat-mdc-select-value {
  font-size: var(--%NS%mat-paginator-select-trigger-text-size, var(--%NS%mat-sys-body-small-size));
}
.mat-mdc-paginator .mat-mdc-form-field-subscript-wrapper {
  display: none;
}
.mat-mdc-paginator .mat-mdc-select {
  line-height: 1.5;
}

.mat-mdc-paginator-outer-container {
  display: flex;
}

.mat-mdc-paginator-container {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  padding: 0 8px;
  flex-wrap: wrap;
  width: 100%;
  min-height: var(--%NS%mat-paginator-container-size, 56px);
}

.mat-mdc-paginator-page-size {
  display: flex;
  align-items: baseline;
  margin-right: 8px;
}
[dir=rtl] .mat-mdc-paginator-page-size {
  margin-right: 0;
  margin-left: 8px;
}

.mat-mdc-paginator-page-size-label {
  margin: 0 4px;
}

.mat-mdc-paginator-page-size-select {
  margin: 0 4px;
  width: var(--%NS%mat-paginator-page-size-select-width, 84px);
}

.mat-mdc-paginator-range-label {
  margin: 0 32px 0 24px;
}

.mat-mdc-paginator-range-actions {
  display: flex;
  align-items: center;
}

.mat-mdc-paginator-icon {
  display: inline-block;
  width: 28px;
  fill: var(--%NS%mat-paginator-enabled-icon-color, var(--%NS%mat-sys-on-surface-variant));
}
.mat-mdc-icon-button[aria-disabled] .mat-mdc-paginator-icon {
  fill: var(--%NS%mat-paginator-disabled-icon-color, color-mix(in srgb, var(--%NS%mat-sys-on-surface) 38%, transparent));
}
[dir=rtl] .mat-mdc-paginator-icon {
  transform: rotate(180deg);
}

@media (forced-colors: active) {
  .mat-mdc-icon-button[aria-disabled] .mat-mdc-paginator-icon,
  .mat-mdc-paginator-icon {
    fill: currentColor;
  }
  .mat-mdc-paginator-range-actions .mat-mdc-icon-button {
    outline: solid 1px;
  }
  .mat-mdc-paginator-range-actions .mat-mdc-icon-button[aria-disabled] {
    color: GrayText;
  }
}
.mat-mdc-paginator-touch-target {
  display: var(--%NS%mat-paginator-touch-target-display, block);
  position: absolute;
  top: 50%;
  left: 50%;
  width: var(--%NS%mat-paginator-page-size-select-width, 84px);
  height: var(--%NS%mat-paginator-page-size-select-touch-target-height, 48px);
  background-color: transparent;
  transform: translate(-50%, -50%);
  cursor: pointer;
}
`],encapsulation:2})}return e})();var St=(()=>{class e{static ɵfac=function(a){return new(a||e)};static ɵmod=cr({type:e});static ɵinj=nn({imports:[oe,Pt$1,Ce,V]})}return e})();var Bt=`cms/history/restore`;var Ct=(()=>{class e extends Y{#t=Tt$1(S);fetchVersions(t,a=null){return this.#t.get({source:`System.ItemHistory`,params:{EntityId:t},noCamel:!0})}restore(t,a){return this.http.post(this.apiUrl(Bt),{entityId:t},{params:{appId:this.appId,changeId:a.toString()}})}static{this.ɵfac=(()=>{let t;return function(i){return(t||(t=Ng(e)))(i||e)}})()}static{this.ɵprov=N({token:e,factory:e.ɵfac})}}return e})();var Nt=(e,r)=>r.versionNumber;function Lt(e,r){if(e&1){let t=i_();ia(0,`div`,3)(1,`mat-form-field`,4)(2,`mat-select`,5),Ry(`selectionChange`,function(i){Zp(t);return Yp(l_(2).compareChange(i.value))}),ia(3,`mat-option`,6),$_(4,`Compare with: Previous`),Td(),ia(5,`mat-option`,7),$_(6,`Compare with: Live`),Td()()()()}if(e&2){let t=l_(2);SC(2),wy(`value`,t.compareWith())}}function Et(e,r){e&1&&YT(0,Lt,7,1,`div`,3),e&2&&QT(r.length>0?0:-1)}function Ft(e,r){e&1&&(ia(0,`div`),$_(1,`Loading...`),Td())}function Vt(e,r){e&1&&(ia(0,`div`),$_(1,`No previous versions of this item found`),Td())}function At(e,r){if(e&1){let t=i_();ia(0,`app-history-item`,10),Ry(`restore`,function(i){Zp(t);return Yp(l_(3).restore(i))}),Td()}if(e&2){let t=r.$implicit;wy(`historyItem`,t)}}function Ht(e,r){if(e&1&&(ia(0,`mat-accordion`,8),JT(1,At,1,1,`app-history-item`,9,Nt),Td()),e&2){let t=l_();SC(),e_(t)}}function Rt(e,r){if(e&1&&YT(0,Ft,2,0,`div`)(1,Vt,2,0,`div`)(2,Ht,3,0,`mat-accordion`,8),e&2){let t=r;QT(t===null?0:t.length===0?1:2)}}function jt(e,r){if(e&1){let t=i_();ia(0,`mat-paginator`,12),Ry(`page`,function(i){Zp(t);return Yp(l_(2).pageChange(i))}),Td()}if(e&2){let t=l_(2);wy(`length`,t.version()?.length??0)(`pageSize`,t.pageSize())(`pageSizeOptions`,t.pageSizeOptions)}}function Gt(e,r){e&1&&YT(0,jt,1,3,`mat-paginator`,11),e&2&&QT(r.length>0?0:-1)}var Be=(()=>{class e{#t;constructor(t,a,i){this.dialog=t,this.route=a,this.snackBar=i,this.hostClass=`dialog-component`,this.#t=Tt$1(Ct),this.#i=parseInt(this.route.snapshot.paramMap.get(`itemId`),10),this.version=this.#t.fetchVersions(this.#i),this.pageSizeOptions=[10,20,50],this.expandedPanels={},this.expandedAttributes={},this.#e=st(1),this.pageSize=l(`pageSize`,this.pageSizeOptions[0]),this.compareWith=st(`live`),this.historyItems=d(`historyItems`,()=>Ae$1(this.version(),this.#e(),this.pageSize(),this.compareWith()))}#i;#e;compareChange(t){this.compareWith.set(t)}panelExpandedChange(t,a){this.expandedPanels[a]=t}attributeExpandedToggle(t,a){this.expandedAttributes[t+a]=!this.expandedAttributes[t+a]}pageChange(t){let a=t.pageIndex+1;a!==this.#e()&&(this.expandedPanels={},this.expandedAttributes={},this.#e.set(a)),this.pageSize.set(t.pageSize)}restore(t){this.snackBar.open(`Restoring previous version...`),this.#t.restore(this.#i,t).subscribe(a=>{this.snackBar.open(`Previous version restored. Will reload edit dialog`,null,{duration:3e3}),this.dialog.close({refreshEdit:!0})})}static{this.ɵfac=function(a){return new(a||e)(he(V$1),he(W),he(Qt))}}static{this.ɵcmp=Jb({type:e,selectors:[[`app-item-history`]],hostVars:1,hostBindings:function(a,i){a&2&&xy(`className`,i.hostClass)},decls:10,vars:3,consts:[[1,`eav-dialog`,`eav-no-scrollbar`],[3,`close`],[1,`eav-dialog-content`],[1,`eav-compare-box`],[`color`,`accent`,1,`eav-mat-form-field`,`eav-compare-box__dropdown`],[`name`,`Scope`,3,`selectionChange`,`value`],[`value`,`previous`],[`value`,`live`],[`multi`,``,1,`eav-history-tables`],[3,`historyItem`],[3,`restore`,`historyItem`],[`color`,`accent`,`showFirstLastButtons`,`true`,3,`length`,`pageSize`,`pageSizeOptions`],[`color`,`accent`,`showFirstLastButtons`,`true`,3,`page`,`length`,`pageSize`,`pageSizeOptions`]],template:function(a,i){if(a&1&&(ia(0,`div`,0)(1,`app-dialog-header`,1),Ry(`close`,function(){return i.dialog.close()}),$_(2,` Item History `),Td(),ia(3,`div`,2)(4,`p`),$_(5,`Check version history for this item and restore the version you need.`),Td(),YT(6,Et,1,1),ia(7,`div`),YT(8,Rt,3,1),Td(),YT(9,Gt,1,1),Td()()),a&2){let s,v,x;SC(6),QT((s=i.historyItems())?6:-1,s),SC(2),QT((v=i.historyItems())?8:-1,v),SC(),QT((x=i.historyItems())?9:-1,x)}},dependencies:[oe,yt$1,ti,je,Pt$1,Nt$1,W$1,Ae,fn,xn,St,V,Pe$1,z],styles:[`.eav-compare-box[_ngcontent-%COMP%]{display:flex;justify-content:flex-end;align-items:flex-end;height:40px}.eav-compare-box__dropdown[_ngcontent-%COMP%]{width:220px;margin-top:-14px;font-size:14px;height:auto}.eav-accordion[_ngcontent-%COMP%]{display:block;padding:4px 0}.eav-expansion-panel-header[_ngcontent-%COMP%], .eav-table-row[_ngcontent-%COMP%]{font-size:14px}.eav-table-row__main[_ngcontent-%COMP%]{display:flex}.eav-table-row__details[_ngcontent-%COMP%]{display:block}.eav-row-main[_ngcontent-%COMP%]{cursor:pointer;padding:4px 0}.eav-row-main__label[_ngcontent-%COMP%]{width:160px;flex:0 0 auto}.eav-row-main__value[_ngcontent-%COMP%]{flex:1 1 auto}.eav-row-main__type[_ngcontent-%COMP%]{flex:0 0 auto;font-size:12px}.eav-row-main__type[_ngcontent-%COMP%]   i[_ngcontent-%COMP%]:not(:last-of-type){margin-right:2px}.eav-detail-row[_ngcontent-%COMP%]{display:flex;padding-bottom:4px;cursor:default}.eav-detail-row__label[_ngcontent-%COMP%]{width:160px;flex:0 0 auto}.eav-detail-row__value[_ngcontent-%COMP%]{flex:1 1 auto}.eav-row-actions[_ngcontent-%COMP%]{margin-top:16px;display:flex;justify-content:flex-end}
/*# sourceMappingURL=item-history-Y4YX5OPK.css.map */`]})}}return e})();export{Be as ItemHistoryComponent};
//# debugId=ff1855c5-f85c-5eb7-a9c5-4352d063eb9d
//# sourceMappingURL=https://sources.2sxc.org/22.01.00/dist/ng-edit/item-history-pvVdlfqR.js.map