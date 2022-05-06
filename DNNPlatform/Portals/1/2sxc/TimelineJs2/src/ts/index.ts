let { Timeline } = require('../../node_modules/@knight-lab/timelinejs');

declare const $2sxc: any;
var winAny = window as any;
winAny.appTimelineJs2 = winAny.appTimelineJs2 || {};
winAny.appTimelineJs2.toolbars = {};

function init({ domAttribute, moduleId }: { domAttribute: string, moduleId: string}) {
  const sxc = $2sxc(moduleId);
  const timeLineElement = document.querySelector(`[${domAttribute}]`) as HTMLElement;

  // #Upgrade Breaking Change #DeprecatedInstanceData
  // ------------------------------------------------------------
  // This is the old code using the now deprecated data-on
  // We are leaving this here for people to see when they want to upgrade old apps
  // Load the data from the 2SexyContent module and define a callback
  // ------------------------------------------------------------
  // sxc.data.on("load", function (source, data) {
  //     processDataAndCreateTimeline(source, data, sxc.id, timeLineElement)
  // }).load();

  // New version using fetch and the default Query we just created
  sxc.webApi
    .fetchJson("app/auto/query/ModuleData")
    .then((data: any) => {
      // #Upgrade Breaking Change #DeprecatedInstanceData
      // ------------------------------------------------------------
      // Old code using the `in`, the `ListContent` and `.List[...]` stream
      // ------------------------------------------------------------
      // var content = data.in.Default.List;
      // var listContent = data.in.ListContent.List[0];

      // Re-format streams that are contained in data
      let content = data.Default;
      let listContent = data.Header[0];
      let timelineData = getTimelineData(moduleId, content, listContent);

      const timelineOptions = {
        width: timeLineElement.offsetWidth,
        height: timeLineElement.offsetHeight,
        start_at_slide: listContent.StartAtSlide ?? 1,
        initial_zoom: listContent.StartZoomAdjust ?? 1,
        debug: true, // have to enable debug to not use default timeline-min.js
      };

      // Create the timeline
      new Timeline(timeLineElement, timelineData, timelineOptions);


      setTimeout(() => {
        // Replace toolbar, since they are removed by the timelinejs library
        timeLineElement.querySelectorAll(".sxc-js-toolbar")
          .forEach((toolbar) => {
            const sxcToolbar = winAny.appTimelineJs2.toolbars[`${toolbar.id}-${moduleId}`];
            toolbar.innerHTML = sxcToolbar
            
            // Add Mutation to replace toolbar after sxc action
            let timelineObserver = new window.MutationObserver(() => {
              toolbar.innerHTML = sxcToolbar;
            });

            timelineObserver.observe(toolbar, {
              childList: true,
              characterData: true,
              subtree: true
            });
          });
      }, 500)
    });
}

// Reformat data from 2sxcContent, so TimelineJS can work with it
function getTimelineData(moduleId: string, content: any, listContent: any) {
  let isEditMode = $2sxc(moduleId).isEditMode();

  if (isEditMode) {
    let listToolbar = $2sxc(moduleId).manage.getToolbar({
      entityId: listContent.Id,
      action: "edit"
    });
    listContent.Text += listToolbar;
  }

  return {
    title: {
      text: {
        headline: listContent.Headline,
        text: listContent.Text
      },
      start_date: getTimelineDate(listContent.StartDate),
      end_date: getTimelineDate(listContent.EndDate),
      unique_id: `app-timelinejs2-${moduleId}`
    },
    events: content.map((event: any) => {
      if (isEditMode) {
        let toolbar = $2sxc(moduleId).manage.getToolbar([
          { entityId: event.Id, action: "edit" },
          { entity: event, action: "new" },
          { entity: event, action: "remove" },
        ]);

        event.Body += addSxcToolbar(toolbar, moduleId).outerHTML;
      }

      return {
        start_date: getTimelineDate(event.StartDate),
        end_date: getTimelineDate(event.EndDate),
        text: {
          headline: event.Headline,
          text: event.Body,
        },
        media: {
          url: event.Media,
          caption: event.MediaCaption,
          credit: event.MediaCredit,
        },
        unique_id: `app-timelinejs2-${moduleId}`
      };
    })
  };
}

function getTimelineDate(date: Date) {
  return {
    year: new Date(date).getFullYear() ?? null,
    month: new Date(date).getMonth() + 1 ?? null,
    day: new Date(date).getDate() ?? null,
  }
}

function addSxcToolbar(toolbar: string, moduleId: string) {
  let jsToolbar = new DOMParser().parseFromString(toolbar, 'text/html').querySelector("ul");
  const toolbarId = jsToolbar.getAttribute("toolbar-identifier");
  jsToolbar.id = toolbarId;
  winAny.appTimelineJs2.toolbars[toolbarId + "-" + moduleId] = jsToolbar.outerHTML;

  const toolbarWrapper = document.createElement("div");
  toolbarWrapper.id = toolbarId;
  toolbarWrapper.innerHTML = jsToolbar.outerHTML;
  toolbarWrapper.classList.add("sxc-js-toolbar");

  return toolbarWrapper;
}

winAny.appTimelineJs2.init = winAny.appTimelineJs2.init || init;
