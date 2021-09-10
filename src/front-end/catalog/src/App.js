import React, { Component } from 'react';
import { List } from './components/List';
import { FetchBooks } from './components/FetchBooks';

import './App.css';

class App extends Component {
  render() {
    return (
      <List>
        <FetchBooks />
      </List>
    );
  }
}

export default App;
