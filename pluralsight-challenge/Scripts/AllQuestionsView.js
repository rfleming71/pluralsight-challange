var AllQuestionsView = Backbone.View.extend({
    events: {
        "click tbody tr": "questionRowClick"
    },
    initialize: function (options) {
        this.collection = new QuestionCollection({ url: options.url });
        this.listenTo(this.collection, "change", this.render);
        this.listenTo(this.collection, "sort", this.render);
        this.listenTo(this.collection, "update", this.render);
    },
    render: function () {
        var totalHtml = "";
        _.each(this.collection.models, function (model) {
            var htmlRow = "<tr data-question-id='" + model.get("QuestionId") + "'><td>" + model.get("Text") + "</td><td>" + model.get("Answer") + "</td><td>";
            _.each(model.get("Distractors"), function (value, key) {
                if (key != 0) {
                    htmlRow += ", ";
                }

                htmlRow += value;
            });

            htmlRow += "</td></tr>";
            totalHtml += htmlRow;
        });

        this.$el.find("tbody").html(totalHtml);
    },
    questionRowClick: function (ev) {
        var target = $(ev.target).closest("tr");
        var questionId = target.attr("data-question-id");
        window.location = "#question/" + questionId;
    },
    show: function () {
        this.collection.fetch();
        this.$el.show();
    },
    hide: function () {
        this.$el.hide();
    }
});