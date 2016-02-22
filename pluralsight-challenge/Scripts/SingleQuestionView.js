var SingleQuestionView = Backbone.View.extend({
    events: {
        "click #cancelBtn": "cancelView",
        "click #saveBtn": "save",
    },
    initialize: function (options) {
        this.model = new QuestionModel({ urlRoot: options.url });
        this.listenTo(this.model, "change", this.render);
    },
    show: function (questionId) {
        if (questionId) {
            this.model.set({ QuestionId: questionId }, { silent: true });
            this.model.fetch();
        }
        else {
            this.model.clear().set(QuestionModel.defaults);
        }
        this.$el.show();
    },
    hide: function () {
        this.$el.hide();
    },
    cancelView: function () {
        window.location = "#";
    },
    save: function () {
        this.model.set({
            Text: this.$el.find("#questionText").val(),
            Answer: this.$el.find("#questionAnswer").val(),
            Distractors: this.$el.find("#questionDistractors").val().split("\n")
        });
        this.model.save();
        window.location = "#";
    },
    render: function () {
        this.$el.find("#questionText").val(this.model.get("Text"));
        this.$el.find("#questionAnswer").val(this.model.get("Answer"));
        var distractors = _.reduce(this.model.get("Distractors"), function (memo, value, index) {
            if (index != 0) {
                memo += "\r\n";
            }

            return memo + value;
        }, "");
        if (this.model.get("QuestionId") == undefined) {
            this.$el.find("#saveBtn").val("Create");
        }
        else {
            this.$el.find("#saveBtn").val("Save");
        }
        this.$el.find("#questionDistractors").val(distractors);
    }
});