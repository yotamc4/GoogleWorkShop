import React from "react"; // importing FunctionComponent
import { Autocomplete } from "@material-ui/lab";
import { TextField } from "@material-ui/core";

export const AutoComplete: React.FunctionComponent<AutoCompleteProps> = (
  props
) => {
  return (
    <Autocomplete
      freeSolo
      style={{ width: "20rem" }}
      options={props.autoCompleteValues}
      disableClearable
      renderInput={(params) => (
        <TextField
          {...params}
          label="Search for product"
          margin="dense"
          variant="outlined"
          InputProps={{ ...params.InputProps, type: "search" }}
          fullWidth
        />
      )}
      onKeyDown={(event: React.KeyboardEvent<HTMLDivElement>) => {
        if (event.key === "Enter") {
          props.onPressEnter((event.target as HTMLInputElement).value);
        }
      }}
    />
  );
};

interface AutoCompleteProps {
  autoCompleteValues: string[];
  onPressEnter: (value: string) => void;
}
