$(function () {

    var opt = {
        postBlockUrl: $("#core-script").data("post-block-url"),
        postCompUrl: $("#core-script").data("post-comp-url"),
        wsContainer: $(".block-factory"),
        compContainer: $(".component-container"),
        compPropContainer: $(".comp-property-container"),

        defaultComp: {
            json: JSON.stringify($("#core-script").data("default-json")),
            src: $("#core-script").data("default-src")
        },

        blockSideSize: 3, // u
        componentSideSize: 150, //px

        physics: [
			"Solide",
			"Destructible"
        ]
    };

    var init = function () {
        initBlockFactory();
        initAvailableComponents();
        initBtn();
        initComponentFactory();
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
					.attr("data-comp", opt.defaultComp.json)
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
            var componentEls = opt.wsContainer.find(".comp");
            var block = {
                name: $(".work-space h5 .form-control").val(),
                components: []
            }
            $.each(componentEls, function (i, e) {
                var compJson = $(e).attr("data-comp");
                block.components.push(
						JSON.parse(compJson)
					);
            });



            $.ajax({
                url: opt.postBlockUrl,
                dataType: "json",
                method: "POST",
                data: {
                    json_data: JSON.stringify({
                        name: block.name,
                        components: JSON.stringify(block.components)
                    })
                }
            })
			.done(function (msg) {
			    var data = msg.data;
                if(data.success == true)
                    alert("Data Saved !");
			    else
                    alert("/!\ Data Not Saved !");
			});
        });

        $(".component-factory form").submit(function (e) {
            e.preventDefault();

            var $form = $(this);
            var formdata = (window.FormData) ? new FormData($form[0]) : null;
            var data = (formdata !== null) ? formdata : $form.serialize();

            $.ajax({
                url: $form.attr('action'),
                type: $form.attr('method'),
                contentType: false, // obligatoire pour de l'upload
                processData: false, // obligatoire pour de l'upload
                dataType: 'json', // selon le retour attendu
                data: data,
                success: function (response) {
                    alert("data saved !")
                }
            });
        });

    };

    var initComponentFactory = function () {
        var tableEl = $(".component-factory .physics-box");
        for (var item in opt.physics) {
            $("<div>").addClass("row")
				.append($("<div>")
					.addClass("col-md-1")
					.css("text-align", "right")
					.append($("<input>")
						.prop("type", "checkbox")
						.prop("name", opt.physics[item])
						.prop("value", "true")
					)
				)
				.append($("<div>")
					.addClass("col-md-10")
					.text(opt.physics[item])
				)
				.appendTo(tableEl);
        }
    };

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
        if (selectedComponent.length == 1) {
            var imgEl = selectedComponent.find("img");
            el.find("img").remove();

            el.append($("<img>").prop("src", imgEl.prop("src")));
            el.attr("data-comp", selectedComponent.attr("data-comp"));

            if (isBlockValid()) {
                $("#dl-block").prop("disabled", false);
            };
        }
    };

    var isBlockValid = function () {
        var result = true;
        var components = opt.wsContainer.find(".comp");

        if (!$(".work-space h5 .form-control").val())
            result = false;

        $.each(components, function (i, e) {
            if (!$(e).attr("data-comp")) {
                result = false;
            }
        });
        return result;
    };

    init();
});