import React, {Component } from 'react';
import TodoList from './TodoList';
import TodoAdd from "./TodoAdd";

export class Home extends Component {
  displayName = Home.name

  render() {
    return (
      <div>
        <TodoAdd />
        <TodoList />    
      </div>
    );
  }
}
