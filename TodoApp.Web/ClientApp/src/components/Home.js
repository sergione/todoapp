import React, {Component } from 'react';
import TodoList from './TodoList';

export class Home extends Component {
  displayName = Home.name

  render() {
    return (
      <div>
        <AddTodo />
        <TodoList />    
      </div>
    );
  }
}
