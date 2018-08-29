import React, { Component } from 'react'

class RespondQuestionnaireRadio extends Component {
    constructor(props) {
        super(props);
        this.state = {};
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(e) {
        console.log(e.target);
        this.props.onQuestionChange(
            {
                QuestionId: this.props.question.questionId,
                NewValue: e.target.value
            }
        );
    }

    render() {
        let question = this.props.question;
        let qi = this.props.questionItems.get(question.questionId);
        return (
            <div>
                <label> {this.props.question.questionText}</label>
                <div >
                    {
                        this.props.question.options.map(opt => {
                            let qid = opt.optionId;
                            let resp = this.props.questionItems.get(question.questionId);
                            //console.log(`OptionId : ${qid} - QuestionResponse : ${resp.response} `);
                            let checked = false;
                            if (resp.response == qid) { 
                                
                                checked = true; 
                            }
                            return (
                                <label key={qid} >
                                    <input type='radio'
                                        name={question.questionId}
                                        checked={checked}
                                        value={qid}
                                        onChange={this.handleChange}
                                    />
                                    <span>{opt.optionText}</span>
                                </label>
                            );
                        }

                        )
                    }
                </div>
            </div>
        );
    }
}

export default RespondQuestionnaireRadio;