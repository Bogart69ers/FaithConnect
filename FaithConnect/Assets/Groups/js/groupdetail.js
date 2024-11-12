document.addEventListener("DOMContentLoaded", function () {
    // Select all the navigation tab links
    const tabLinks = document.querySelectorAll(".group-navigation .nav-link");
    const tabContent = document.querySelectorAll(".tab-pane");

    // Function to handle tab switching
    function switchTab(event) {
        event.preventDefault();

        // Remove 'active' class from all tabs and contents
        tabLinks.forEach((link) => link.classList.remove("active"));
        tabContent.forEach((content) => {
            content.classList.remove("show");
            content.classList.remove("active");
        });

        // Add 'active' class to the clicked tab
        event.currentTarget.classList.add("active");

        // Find the target pane by the data-target attribute
        const targetId = event.currentTarget.getAttribute("data-target");
        const targetContent = document.getElementById(targetId);

        // Show the target content
        targetContent.classList.add("show");
        targetContent.classList.add("active");
    }

    // Check for the activeTab parameter in the URL
    const urlParams = new URLSearchParams(window.location.search);
    const activeTab = urlParams.get('activeTab');

    // Determine the tab to activate initially
    let initialTabLink;
    if (activeTab) {
        initialTabLink = Array.from(tabLinks).find(link => link.getAttribute("data-target") === activeTab);
    }
    if (!initialTabLink) {
        initialTabLink = tabLinks[0]; // Default to the first tab if no activeTab is specified or found
    }

    // Activate the initial tab
    if (initialTabLink) {
        initialTabLink.classList.add("active");
        const initialTabContent = document.getElementById(initialTabLink.getAttribute("data-target"));
        if (initialTabContent) {
            initialTabContent.classList.add("show");
            initialTabContent.classList.add("active");
        }
    }

    // Attach click event to each tab link
    tabLinks.forEach((link) => link.addEventListener("click", switchTab));
});
