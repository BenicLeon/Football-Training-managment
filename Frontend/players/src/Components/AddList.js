import React, { useRef } from 'react';

const AddList = ({ newItem, setNewItem, handleSubmit }) => {
  const inputRef = useRef();

  return (
    <form className="addForm" onSubmit={handleSubmit}>
      <input
        type="text"
        id="addItem"
        autoFocus
        ref={inputRef}
        placeholder="Add or Update Player"
        required
        value={newItem}
        onChange={(e) => setNewItem(e.target.value)}
      />
      <button
        type="submit"
        aria-label="Add or Update Item"
        onClick={() => inputRef.current.focus()}
      >
        +
      </button>
    </form>
  );
};

export default AddList;
