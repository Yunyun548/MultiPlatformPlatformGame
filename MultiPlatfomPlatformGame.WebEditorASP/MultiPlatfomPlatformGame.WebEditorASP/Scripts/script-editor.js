$(function () {

    var opt = {
        postBlockUrl: $("#core-script").data("post-block-url"),
        wsContainer: $(".block-factory"),
        compContainer: $(".component-container"),
        compPropContainer: $(".comp-property-container"),

        defaultComp: {
            id: $("#core-script").data("default-id"),
            src: $("#core-script").data("default-src")
        },

        blockSideSize: 3, // u
        componentSideSize: 150 //px

    };

    var init = function () {
        initBlockFactory();
        initAvailableComponents();
        initBtn();
    };

    var initAvailableComponents = function () {
        var components = opt.compContainer.find("li");

        components.click(function () {
            selectComponent($(this));
        });
    };
    var initBlockFactory = function () {
        // +10 pour le margin
        var blockFactorySideSize = (opt.componentSideSize + 10) * opt.blockSideSize;

        $(".block-factory")
            .css("width", blockFactorySideSize)
            .css("height", blockFactorySideSize)


        for (var i = opt.blockSideSize - 1; i >= 0; i--) {
            for (var j = opt.blockSideSize - 1; j >= 0; j--) {
                $("<div>").addClass("comp")
                    .css("width", opt.componentSideSize)
                    .css("height", opt.componentSideSize)
                    .click(function () {
                        attachComponent($(this));
                    })
                    .append($("<img>").prop("src", opt.defaultComp.src))
                    .attr("data-comp-id", opt.defaultComp.id)
                    .appendTo(opt.wsContainer);
            };
        };
    };

    var initBtn = function () {

        $(".work-space h5 .form-control").keypress(function () {

            if (isBlockValid()) {
                $("#dl-block").prop("disabled", false);
            };
        })


        $("#dl-block").click(function () {
            var components = opt.wsContainer.find(".comp");
            var block = {
                name: $(".work-space h5 .form-control").val(),
                matrix: []
            }
            $.each(components, function (i, e) {
                block.matrix.push($(e).attr("data-comp-id"));
            });
            console.log(block);
            $.ajax({
                url: opt.postBlockUrl,
                dataType: "json",
                method: "POST",
                data: block
            })
                .done(function (msg) {
                    alert("Data Saved: " + msg);
                });

        });
    }


    var selectComponent = function (el) {
        var selClass = "selected";

        opt.compPropContainer.empty();

        if (el.hasClass(selClass))
            el.removeClass(selClass);
        else {
            opt.compContainer.find("li").removeClass(selClass);
            el.addClass(selClass);

            var propertiesEl = el.find(".comp-property").clone();

            opt.compPropContainer.append(propertiesEl);
        }
    };

    var attachComponent = function (el) {
        var selectedComponent = opt.compContainer.find(".selected");
        var imgEl = selectedComponent.find("img");
        el.find("img").remove();

        el.append($("<img>").prop("src", imgEl.prop("src")));
        el.attr("data-comp-id", selectedComponent.attr("data-comp-id"));

        if (isBlockValid()) {
            $("#dl-block").prop("disabled", false);
        };

    };

    var isBlockValid = function () {
        var result = true;
        var components = opt.wsContainer.find(".comp");

        if (!$(".work-space h5 .form-control").val())
            result = false;

        $.each(components, function (i, e) {
            if (!$(e).attr("data-comp-id")) {
                result = false;
            }
        });
        return result;
    };


    init();
});