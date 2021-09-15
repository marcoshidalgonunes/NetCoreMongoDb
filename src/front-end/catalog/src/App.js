import React, { Component } from 'react';
import { List } from './components/List';
import { BooksList } from './components/BooksList';

import './App.css';

class App extends Component {
  render() {
    return (
      <List>
        <BooksList />
      </List>
    );
  }
}

export default App;
