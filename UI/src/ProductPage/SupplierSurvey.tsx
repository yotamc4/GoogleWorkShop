import * as React from "react";
import {
  ChoiceGroup,
  IChoiceGroupOption,
  IChoiceGroupStyles,
} from "office-ui-fabric-react/lib/ChoiceGroup";
import {
  IStackTokens,
  ITextStyles,
  Stack,
  Text,
  DefaultButton,
} from "@fluentui/react";

export interface ISuppliersNamesList {
  supliersNames: IChoiceGroupOption[];
}

const choiceGroupStyles: IChoiceGroupStyles = {
  root: {
    marginLeft: "-22rem",
  },
};

const textStyles: ITextStyles = {
  root: {
    marginLeft: "-22rem",
  },
};

const verticalGapStackTokens: IStackTokens = {
  childrenGap: 30,
};

export const SuppliersSurvey: React.FunctionComponent<ISuppliersNamesList> = ({
  supliersNames,
}) => {
  const [selectedKey, setSelectedKey] = React.useState<string>();

  const onChange = React.useCallback(
    (
      evt?: React.FormEvent<HTMLElement | HTMLInputElement>,
      option: IChoiceGroupOption | undefined = undefined
    ) => {
      setSelectedKey(option?.key as string);
    },
    []
  );

  return (
    <Stack tokens={verticalGapStackTokens}>
      <Text
        block={true}
        className="Bold"
        styles={textStyles}
        variant="xLargePlus"
      >
        Time to choose you're favorite supplier
      </Text>
      <ChoiceGroup
        styles={choiceGroupStyles}
        selectedKey={selectedKey}
        options={supliersNames}
        onChange={onChange}
      />
      <DefaultButton
        text={"Vote"}
        primary
        styles={{
          root: { borderRadius: 25, height: "2.5rem", width:"20rem",  marginLeft:"-10rem" },
          textContainer: { padding: "1rem", fontSize: "1.5rem" },
        }}
      ></DefaultButton>
    </Stack>
  );
};
