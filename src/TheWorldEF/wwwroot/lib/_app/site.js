!function(){var e=$("#username");e.text("FerAcevedo Diaz");var a=$("#main");a.on("mouseenter",function(){a.style="background-color: #888;"}),a.on("mouseleave",function(){a.style=""});var s=$("#sidebar,#wrapper"),l=$("#sidebarToggle i.fa");$("#sidebarToggle").on("click",function(){s.toggleClass("hide-sidebar"),s.hasClass("hide-sidebar")?(l.removeClass("fa-angle-left"),l.addClass("fa-angle-right")):(l.addClass("fa-angle-left"),l.removeClass("fa-angle-right"))})}();