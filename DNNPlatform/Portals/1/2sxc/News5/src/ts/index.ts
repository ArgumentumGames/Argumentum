declare let Fancybox: any;

var winAny = window as any;
winAny.appNews5 ??= {};
winAny.appNews5.activateFancybox ??= activateFancybox;

function activateFancybox({ groupId }: { groupId: string }) {
  console.log(groupId);
  Fancybox.bind(`[data-app-news5-fancybox="${groupId}"]`);
}