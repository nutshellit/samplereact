import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import {ListQuestionnaires} from './components/ListQuestionnaires';
import RespondQuestionnaire from './components/RespondQuestionnaire';
export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetchdata' component={FetchData} />
        <Route path='/listquestionnaires' component={ListQuestionnaires} />
        <Route path='/respondquestionnaire/:questionnairename' component={RespondQuestionnaire} />
      </Layout>
    );
  }
}
