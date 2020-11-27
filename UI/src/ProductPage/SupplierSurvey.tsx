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
  supliersNames: string[];
}

const choiceGroupStyles: IChoiceGroupStyles = {
  root: {
    marginLeft: "50rem",
  },
};

const textStyles: ITextStyles = {
  root: {
    marginLeft: "50rem",
  },
};

const verticalGapStackTokens: IStackTokens = {
  childrenGap: 30,
};

export const SuppliersSurvey: React.FunctionComponent<ISuppliersNamesList> = ({
  supliersNames,
}) => {
  const [selectedKey, setSelectedKey] = React.useState<string>();

  //TODO: react use effect create the list of supplliers
  let options: IChoiceGroupOption[] = [];

  for (let i = 0; i < supliersNames.length; i++) {
    options.push({
      key: String(i),
      text: supliersNames[i] as string,
    });
  }

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
        options={options}
        onChange={onChange}
      />
      <DefaultButton
        text={"Vote"}
        primary
        styles={{
          root: { borderRadius: 25, height: "2.5rem", width:"20rem",  marginLeft:"55rem" },
          textContainer: { padding: "1rem", fontSize: "1.5rem" },
        }}
      ></DefaultButton>
    </Stack>
  );
};
