// so it can be called from the HTML when content re-initializes dynamically
const winAny = (window as any)
winAny.appJobs2 ??= {}
winAny.appJobs2.init ??= init

function init({ domAttribute, currentCategory }: { domAttribute: string, currentCategory: string }) {
  const appWrapper = document.querySelector(`[${domAttribute}]`)
  const categoryButtons = appWrapper.querySelectorAll(`[app-jobs2-category-button]`)
  const noReloadRequired = currentCategory == ""
  setPrimary(currentCategory, categoryButtons)

  categoryButtons.forEach((categoryButton: HTMLElement) =>
    categoryButton.addEventListener('click', ((e) => {
      const filter = categoryButton.getAttribute('app-jobs2-filter');

      if (noReloadRequired) {
        e.preventDefault()
        window.history.pushState(null, document.title, categoryButton.getAttribute('href'));
      }

      setPrimary(filter, categoryButtons)
      filterItems(filter, appWrapper)
    }))
  )
}

// match category button to filter and add/remove primary class
function setPrimary(filter: string, buttons: NodeListOf<Element>) {
  if (filter == "") filter = "nofilter"
  buttons.forEach((button) => {
    if (button.getAttribute('app-jobs2-filter') == filter) {
      button.classList.remove('btn-outline-primary')
      button.classList.add('btn-primary')
      return
    }
    button.classList.remove('btn-primary')
    button.classList.add('btn-outline-primary')
  })
}

// filter shown items by selected Filter
function filterItems(selectedFilter: string, appWrapper: Element) {
  appWrapper.querySelectorAll(`[app-jobs2-filterelem]`).forEach((jobElement: HTMLElement) => {
    if (selectedFilter === "nofilter" || [...JSON.parse(jobElement.getAttribute('app-jobs2-filterelem'))].find(filter => filter == selectedFilter)) {
      jobElement.style.display = "block"
      return
    }
    jobElement.style.display = "none"
  })
}