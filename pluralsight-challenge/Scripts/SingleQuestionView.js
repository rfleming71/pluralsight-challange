var SingleQuestionView = Backbone.View.extend({
    events: {
        "click #cancelBtn": "cancelView",
        "click #saveBtn": "save",
        "change #questionDistractors": "cleanupDistractors"
    },
    initialize: function (options) {
        this.model = new QuestionModel({ urlRoot: options.url });
        this.listenTo(this.model, "change", this.render);
        this.$el.validator();
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
    cleanupDistractors: function(ev) {
        var target = $(ev.target);
        var distractors = target.val().split("\n");
        distractors = _.filter(distractors, function (distractor) { return distractor && distractor.trim() != ""; });
        target.val(this.buildDistractorList(distractors));
    },
    hide: function () {
        this.$el.hide();
    },
    cancelView: function () {
        window.location = "#";
    },
    save: function () {
        this.$el.validator("validate");
        if (this.$el.find(".has-error").length > 0) {
            return false;
        }

        this.model.set({
            Text: this.$el.find("#questionText").val(),
            Answer: this.$el.find("#questionAnswer").val(),
            Distractors: this.$el.find("#questionDistractors").val().split("\n")
        });
        this.model.save({}, {
            success: function () {
                window.location = "#";
            },
            error: function () {
                alert("Failed to save question");
            }
        });
    },
    buildDistractorList: function(list) {
        return _.reduce(list, function (memo, value, index) {
            if (index != 0) {
                memo += "\r\n";
            }

            return memo + value;
        }, "");
    },
    render: function () {
        this.$el.find("#questionText").val(this.model.get("Text"));
        this.$el.find("#questionAnswer").val(this.model.get("Answer"));
        var distractors = this.buildDistractorList(this.model.get("Distractors"));
        if (this.model.get("QuestionId") == undefined) {
            this.$el.find("#saveBtn").val("Create");
        }
        else {
            this.$el.find("#saveBtn").val("Save");
        }
        this.$el.find("#questionDistractors").val(distractors);
    }
});