(window.webpackJsonp=window.webpackJsonp||[]).push([[37],{qtGM:function(t,n,e){"use strict";e.r(n),e.d(n,"ExportAppComponent",(function(){return m}));var o=e("D57K"),i=e("C05f"),p=e("HM3f"),c=e("YtkY"),a=e("EM62"),s=e("OZ4H"),r=e("q3Tv"),b=e("2kYt"),l=e("PBFl"),u=e("csyo"),f=e("+Tre"),d=e("nIj0");function g(t,n){1&t&&a.Nb(0,"mat-spinner",10)}function h(t,n){if(1&t){var e=a.Tb();a.Sb(0,"div",11),a.Sb(1,"p",12),a.Oc(2,"Specs"),a.Rb(),a.Sb(3,"ul",13),a.Sb(4,"li"),a.Oc(5),a.Rb(),a.Sb(6,"li"),a.Oc(7),a.Rb(),a.Sb(8,"li"),a.Oc(9),a.Rb(),a.Rb(),a.Sb(10,"p",12),a.Oc(11,"Contains"),a.Rb(),a.Sb(12,"ul",13),a.Sb(13,"li"),a.Oc(14),a.Rb(),a.Sb(15,"li"),a.Oc(16),a.Rb(),a.Sb(17,"li"),a.Oc(18),a.Rb(),a.Sb(19,"li"),a.Oc(20),a.Rb(),a.Sb(21,"li"),a.Oc(22),a.Rb(),a.Rb(),a.Sb(23,"div",14),a.Sb(24,"div",15),a.Sb(25,"mat-checkbox",16),a.ac("ngModelChange",(function(t){return a.Ac(e),a.ec(2).includeContentGroups=t})),a.Sb(26,"span",17),a.Oc(27," Include all content-groups to re-import the app in an exact copy of this site. Only select this option when you copy an entire DNN site. "),a.Rb(),a.Rb(),a.Rb(),a.Sb(28,"div",15),a.Sb(29,"mat-checkbox",16),a.ac("ngModelChange",(function(t){return a.Ac(e),a.ec(2).resetAppGuid=t})),a.Sb(30,"span",17),a.Oc(31," Reset the app GUID to zero. You only need this option for special tutorial apps, and usually must not select it. "),a.Rb(),a.Rb(),a.Rb(),a.Rb(),a.Rb()}if(2&t){var o=a.ec().ngIf,i=a.ec();a.Ab(5),a.Qc("Name: ",o.appInfo.Name,""),a.Ab(2),a.Qc("Guid: ",o.appInfo.Guid,""),a.Ab(2),a.Qc("Version: ",o.appInfo.Version,""),a.Ab(5),a.Qc("",o.appInfo.EntitiesCount," entities"),a.Ab(2),a.Qc("",o.appInfo.LanguagesCount," languages"),a.Ab(2),a.Sc(" ",o.appInfo.TemplatesCount," templates (Token: ",o.appInfo.HasTokenTemplates,", Razor: ",o.appInfo.HasRazorTemplates,") "),a.Ab(2),a.Qc("",o.appInfo.TransferableFilesCount," files to export"),a.Ab(2),a.Qc("",o.appInfo.FilesCount," files in the app folder totally"),a.Ab(3),a.kc("ngModel",i.includeContentGroups)("disabled",i.resetAppGuid||o.isExporting),a.Ab(4),a.kc("ngModel",i.resetAppGuid)("disabled",i.includeContentGroups||o.isExporting)}}function x(t,n){if(1&t){var e=a.Tb();a.Qb(0),a.Sb(1,"div",1),a.Sb(2,"div",2),a.Oc(3,"Export app"),a.Rb(),a.Rb(),a.Mc(4,g,1,0,"mat-spinner",3),a.Sb(5,"p",4),a.Oc(6," Pack the entire app to a "),a.Sb(7,"em"),a.Oc(8,"zip"),a.Rb(),a.Oc(9," folder which can be imported again to another site. For further help visit "),a.Sb(10,"a",5),a.Oc(11,"2sxc Help"),a.Rb(),a.Oc(12,". "),a.Rb(),a.Mc(13,h,32,14,"div",6),a.Sb(14,"div",7),a.Sb(15,"button",8),a.ac("click",(function(){return a.Ac(e),a.ec().closeDialog()})),a.Oc(16," Cancel "),a.Rb(),a.Sb(17,"button",8),a.ac("click",(function(){return a.Ac(e),a.ec().exportGit()})),a.Oc(18," Export Data for Github versioning "),a.Rb(),a.Sb(19,"button",9),a.ac("click",(function(){return a.Ac(e),a.ec().exportApp()})),a.Oc(20," Export App "),a.Rb(),a.Rb(),a.Pb()}if(2&t){var o=n.ngIf;a.Ab(4),a.kc("ngIf",o.isExporting),a.Ab(9),a.kc("ngIf",o.appInfo),a.Ab(2),a.kc("disabled",o.isExporting),a.Ab(2),a.kc("disabled",o.isExporting),a.Ab(2),a.kc("disabled",o.isExporting)}}var m=function(){function t(t,n,e){this.dialogRef=t,this.exportAppService=n,this.zone=e,this.hostClass="dialog-component",this.includeContentGroups=!1,this.resetAppGuid=!1,this.appInfo$=new i.a(null),this.isExporting$=new i.a(!1),this.templateVars$=Object(p.a)([this.appInfo$,this.isExporting$]).pipe(Object(c.a)((function(t){var n=Object(o.f)(t,2);return{appInfo:n[0],isExporting:n[1]}})))}return t.prototype.ngOnInit=function(){var t=this;this.exportAppService.getAppInfo().subscribe((function(n){t.appInfo$.next(n)}))},t.prototype.ngOnDestroy=function(){this.appInfo$.complete(),this.isExporting$.complete()},t.prototype.closeDialog=function(){this.dialogRef.close()},t.prototype.exportApp=function(){this.isExporting$.next(!0),this.exportAppService.exportApp(this.includeContentGroups,this.resetAppGuid),this.isExporting$.next(!1)},t.prototype.exportGit=function(){var t=this;this.isExporting$.next(!0),this.exportAppService.exportForVersionControl(this.includeContentGroups,this.resetAppGuid).subscribe({next:function(n){t.isExporting$.next(!1),alert("Done - please check your '.data' folder")},error:function(n){t.isExporting$.next(!1)}})},t.\u0275fac=function(n){return new(n||t)(a.Mb(s.g),a.Mb(r.a),a.Mb(a.A))},t.\u0275cmp=a.Gb({type:t,selectors:[["app-export-app"]],hostVars:1,hostBindings:function(t,n){2&t&&a.Vb("className",n.hostClass)},decls:2,vars:3,consts:[[4,"ngIf"],["mat-dialog-title",""],[1,"dialog-title-box"],["mode","indeterminate","diameter","20","color","accent",4,"ngIf"],[1,"dialog-description"],["href","https://2sxc.org/en/help?tag=export-app","target","_blank"],["class","dialog-component-content fancy-scrollbar-light",4,"ngIf"],[1,"dialog-component-actions"],["mat-raised-button","",3,"disabled","click"],["mat-raised-button","","color","accent",3,"disabled","click"],["mode","indeterminate","diameter","20","color","accent"],[1,"dialog-component-content","fancy-scrollbar-light"],[1,"app-info__title"],[1,"app-info__content"],[1,"options-wrapper"],[1,"option-box"],[3,"ngModel","disabled","ngModelChange"],[1,"option-box__text"]],template:function(t,n){1&t&&(a.Mc(0,x,21,5,"ng-container",0),a.fc(1,"async")),2&t&&a.kc("ngIf",a.gc(1,1,n.templateVars$))},directives:[b.t,s.h,l.b,u.b,f.a,d.s,d.v],pipes:[b.b],styles:[".app-info__title[_ngcontent-%COMP%]{font-size:18px;margin:24px 0 0;font-weight:700}.app-info__content[_ngcontent-%COMP%]{font-size:14px}.app-info__content[_ngcontent-%COMP%]   li[_ngcontent-%COMP%]{padding:2px 0}.options-wrapper[_ngcontent-%COMP%]{margin:24px 0}.option-box[_ngcontent-%COMP%]{margin:2px 0}.option-box__text[_ngcontent-%COMP%]{white-space:normal;font-size:14px}"],changeDetection:0}),t}()}}]);
//# sourceMappingURL=https://sources.2sxc.org/11.11.04/ng-edit/export-app-component.e883818e9d82347c9bda.js.map