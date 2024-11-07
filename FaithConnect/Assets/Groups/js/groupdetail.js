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

    // Initialize the first tab as active
    if (tabLinks.length > 0) {
        tabLinks[0].classList.add("active");
        const firstTabContent = document.getElementById(tabLinks[0].getAttribute("data-target"));
        if (firstTabContent) {
            firstTabContent.classList.add("show");
            firstTabContent.classList.add("active");
        }
    }

    // Attach click event to each tab link
    tabLinks.forEach((link) => link.addEventListener("click", switchTab));
});
