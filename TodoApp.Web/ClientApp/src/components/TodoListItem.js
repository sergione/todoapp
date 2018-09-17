import {Checkbox} from "react-bootstrap";
import React from "react";
import * as PropTypes from "prop-types";
import {Mutation} from "react-apollo";
import gql from "graphql-tag";
import {Link} from "react-router-dom";

const COMPLETE_TODO = gql`
    mutation completeTodo($todoId: String!) {
        completeTodo(todoId: $todoId) {
            id
            description
            complete
        }
    }`;

const TodoListItem = (props) => {
    return (
        <Mutation
            mutation={COMPLETE_TODO} >
            {(completeTodo, {data}) => (
                <div>
                    <Checkbox 
                        value={props.id}
                        checked={props.checked}
                        onChange={(e) => {
                            e.preventDefault();
                            completeTodo({ variables: {todoId: e.target.value}})
                        }}
                    ><Link to={`/todos/${props.id}`}> {props.description}</Link></Checkbox>
                </div>)}
        </Mutation>
        );
};

TodoListItem.propTypes = {
    id: PropTypes.any,
    checked: PropTypes.any,
    description: PropTypes.any
};

export default TodoListItem;