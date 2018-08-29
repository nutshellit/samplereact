import React, { Component } from 'react';

class RespondQuestionnaire extends Component {
    constructor(props) {
        super(props);
        this.state = {};
        this.handleQuestionChange = this.handleQuestionChange.bind(this);
    }

    handleQuestionChange(e) {
        console.log(e.target.value);
        this.props.onQuestionChange(
            {
                QuestionId: this.props.question.questionId,
                NewValue: e.target.value
            });
    }

    render() {
        let question = this.props.question;
        let qi = this.props.questionItems.get(question.questionId);
        if (qi.isHidden) {
            return <span>--hidden--</span>
        }
        else {
            return (
                <div>
                    <label>
                        {qi.questionId}
                        {qi.isHidden ? "hidden" : "show"}
                        {question.questionText}

                    </label>
                    <input type='text'
                        value={qi.response}
                        onChange={this.handleQuestionChange} />
                </div>
            );
        }
    }
}

export default RespondQuestionnaire;        