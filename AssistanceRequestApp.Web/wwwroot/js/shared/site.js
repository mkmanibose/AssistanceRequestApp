(function () {
    document.getElementById("SearchInput").addEventListener("input", function () {
        const menus = document.querySelectorAll(".sidebar li");
        const searches = this.value.toLowerCase().split(" ");

        for (let i = 0; i < menus.length; i++) {
            let isMatch = true;

            const menuWords = menus[i].innerText.toLowerCase().split(" ");

            for (let j = 0; j < searches.length; j++) {
                let hasMatch = false;

                for (let k = 0; k < menuWords.length; k++) {
                    if (menuWords[k].indexOf(searches[j]) >= 0) {
                        hasMatch = true;
                    }
                }

                if (!hasMatch) {
                    isMatch = false;
                }
            }

            if (isMatch) {
                menus[i].style.display = "";
            } else {
                menus[i].style.display = "none";
            }
        }
    });

    document.querySelectorAll(".mvc-grid").forEach(element => new MvcGrid(element));
})();
