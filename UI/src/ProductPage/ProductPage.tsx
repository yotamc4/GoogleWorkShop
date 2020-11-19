import React from "react";
import * as Styles from "./ProductPageStyles";
import {
  DefaultButton,
  FontIcon,
  Icon,
  Image,
  Separator,
  Stack,
  Text,
} from "@fluentui/react";

export const ProductPage: React.FunctionComponent = () => {
  return (
    <Stack>
      <Stack
        horizontal
        tokens={{
          childrenGap: "2rem",
          padding: 10,
        }}
      >
        <Image
          src="https://bstore.bezeq.co.il/media/20696/740-2-blue.jpg"
          height="30rem"
          width="30rem"
        ></Image>
        <Stack
          tokens={{
            childrenGap: "1rem",
            padding: 10,
          }}
        >
          <Text
            className="Bold"
            styles={Styles.headerStyle}
            variant="xLargePlus"
          >
            Lenovo ThinkPad T4800
          </Text>
          <Separator />
          <Text>Maximum Acceptable Price: 140₪</Text>
          <Text>Group's expiration date: {new Date().toString()}</Text>
          <Text variant="large">Description </Text>
          <Text styles={Styles.descriptionStyle}>
            This example shows how components that used to be styled using CSS
            can be styled using JS styling. (Look at the bottom of the code to
            see the equivalent SCSS.) The preferred method is JS styling for
            several reasons: type safety for styling, more predictable behavior,
            and clear feedback via typing when component changes affect existing
            styling code.
          </Text>
          <Separator />
          <Stack horizontal verticalAlign="center">
            <FontIcon
              iconName="AddGroup"
              className={Styles.classNames.greenYellow}
            />
            <Text>170 pepole have joined to the group so far</Text>
          </Stack>
          <DefaultButton
            text="Join The Group"
            primary
            iconProps={{
              iconName: "AddFriend",
              styles: { root: { fontSize: "1.5rem" } },
            }}
            styles={{
              root: { borderRadius: 25, height: "4rem" },
              textContainer: { padding: "1rem", fontSize: "1.5rem" },
            }}
            height={"4rem"}
          />
        </Stack>
      </Stack>
    </Stack>
  );
};
